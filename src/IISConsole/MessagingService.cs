using IISConsole.Ipc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IISConsole.IpmTypes;

namespace IISConsole
{
    public class MessagingService
    {
        private readonly ILogger _logger;
        private readonly PipeListenerOptions _options;
        private NamedPipeServerStream _pipeServerStream;
        private readonly object _lock = new object();

        public MessagingService(ILoggerFactory loggerFactory, IOptions<PipeListenerOptions> options) 
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;

            _logger = loggerFactory.CreateLogger<MessagingService>();

            _pipeServerStream = new NamedPipeServerStream(
                _options.PipeName,
                PipeDirection.InOut,
                maxNumberOfServerInstances: 1,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);
        }

        internal void SendMessage(IpmMessage message)
        {
            if (!_pipeServerStream.CanWrite)
            {
                throw new ApplicationException();
            }

            var buffer = message.ToByteArray();
            lock (_lock)
            {
                _pipeServerStream.Write(buffer, 0, buffer.Length);
            }
            _logger.LogInformation("Ping message sent");
        }
        public async Task PingAsync(CancellationToken token)
        {
            var message = new PingMessage();
            while (!token.IsCancellationRequested)
            {
                if (_pipeServerStream.CanWrite)
                {
                    SendMessage(message);
                }
                await Task.Delay(_options.PingFrequency);
            }
        }

        public async Task ListenAsync(CancellationToken cancellationToken)
        {
            await _pipeServerStream.WaitForConnectionAsync(cancellationToken);
            _logger.LogInformation("Worker process connect to {pipeName}", _options.PipeName);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var buffer = new byte[IpmMessage.MessageHeaderSize];
                    if (await _pipeServerStream.ReadAsync(buffer, 0, IpmMessage.MessageHeaderSize, cancellationToken) != IpmMessage.MessageHeaderSize)
                    {
                        // TODO better exceptions
                        throw new ApplicationException();
                    }

                    var messageHeader = new IpmMessage(buffer);
                    var messageBodyBuffer = new byte[messageHeader.MessageBodySize];
                    if (await _pipeServerStream.ReadAsync(messageBodyBuffer, 0, (int)messageHeader.MessageBodySize, cancellationToken) != messageHeader.MessageBodySize)
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
                            _pipeServerStream.Position += (messageHeader.MessageBodySize);
                            break;
                    }

                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine("Aborting listener thread");
                    break;
                }
            }
        }
    }
}
