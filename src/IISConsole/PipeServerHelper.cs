using IISConsole.Ipc;
using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static IISConsole.IpmTypes;

namespace IISConsole
{
    public class PipeServerHelper : IDisposable
    {
        private CancellationTokenSource _cts;
        private readonly object _internalLock;
        public NamedPipeServerStream PipeServer { get; set; }
        public PipeServerHelper(string pipeName)
        {
            _internalLock = new object();
            PipeServer = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);
        }

        public void StartServer()
        {
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            
            // Create listener thread
            Task.Run(async () =>
            {
                await ListenAsync(token);
            }, token);

            // Create thread to send ping
            Task.Run(() =>
            {
                var message = new PingMessage();
                while (true)
                {
                    token.WaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                    SendMessage(message);
                }

            }, token);

        }

        public void StopServer()
        {
            _cts.Cancel();
        }

        internal void SendMessage(IpmMessage message)
        {
            var buffer = message.ToByteArray();
            lock (_internalLock)
            {

                PipeServer.Write(buffer, 0, buffer.Length);
            }
        }

        private async Task ListenAsync(CancellationToken ct)
        {
            await PipeServer.WaitForConnectionAsync(ct);
            Console.WriteLine("w3wp connected");

            while(true)
            {
                try
                {
                    var buffer = new byte[IpmMessage.MessageHeaderSize];
                    if (await PipeServer.ReadAsync(buffer, 0, IpmMessage.MessageHeaderSize, ct) != IpmMessage.MessageHeaderSize)
                    {
                        // TODO better exceptions
                        throw new ApplicationException();
                    }

                    var messageHeader = new IpmMessage(buffer);
                    var messageBodyBuffer = new byte[messageHeader.MessageBodySize];
                    if (await PipeServer.ReadAsync(messageBodyBuffer, 0, (int)messageHeader.MessageBodySize, ct) != messageHeader.MessageBodySize)
                    {
                        // TODO better exceptions
                        throw new ApplicationException();
                    }
                    // Dispatch based on the message received
                    switch (messageHeader.OpCode)
                    {
                        case IPM_OPCODE.IPM_OP_GETPID:
                            var pidMessage = new GetPidMessage(messageHeader, messageBodyBuffer);
                            Console.WriteLine($"w3wp[{pidMessage.ProcessId}] connected");
                            break;
                        default:
                            // For now we're just ignoring the message
                            Console.WriteLine(Enum.GetName(typeof(IPM_OPCODE), messageHeader.OpCode));
                            Console.WriteLine(messageHeader.MessageBodySize);
                            PipeServer.Position += (messageHeader.MessageBodySize);
                            break;
                    }
                    
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine("Aborting listener thread");
                    break;
                }

                if (ct.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public void Dispose()
        {
            // TODO need to make sure we're stopped before we dispose
            // There is probably some race condition here
            PipeServer.Dispose();
        }
    }
}