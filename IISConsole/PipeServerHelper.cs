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
        private object _internalLock;
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

            //// Create thread to send ping
            //Task.Run(() =>
            //{
            //    while(true)
            //    {
            //        token.WaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            //        SendMessage(IPM_OPCODE.IPM_OP_PING, 0, null);
            //    }
                
            //}, token);

        }

        public void StopServer()
        {
            _cts.Cancel();
        }

        internal void SendMessage(IPM_OPCODE opCode, uint size, byte[] buffer)
        {
            var header = new IPM_MESSAGE_HEADER(opCode, size);
            var headerSize = Marshal.SizeOf(typeof(IPM_MESSAGE_HEADER));
            var headerBuffer = new byte[headerSize];
            var ptr = Marshal.AllocHGlobal(headerSize);
            Marshal.StructureToPtr(header, ptr, false);
            Marshal.Copy(ptr, headerBuffer, 0, headerSize);
            lock(_internalLock)
            {
                PipeServer.Write(headerBuffer, 0, headerSize);
            }

            if (size != 0)
            {
                throw new NotImplementedException();
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
                    var header = await PipeServer.ReadIpmMessageHeaderAsync(ct);
                    Console.WriteLine(Enum.GetName(typeof(IPM_OPCODE), header.OpCode));
                    Console.WriteLine(header.MessageSize - Marshal.SizeOf(header));

                    // Dispatch based on the message received
                    switch (header.OpCode)
                    {
                        case IPM_OPCODE.IPM_OP_GETPID:
                            var buffer = new byte[4];
                            PipeServer.Read(buffer, 0, 4);
                            var pid = BitConverter.ToUInt32(buffer, 0);
                            Console.WriteLine($"w3wp[{pid}] connected");
                            break;
                        default:
                            // For now we're just ignoring the message
                            PipeServer.Position += (header.MessageSize - Marshal.SizeOf(header));
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