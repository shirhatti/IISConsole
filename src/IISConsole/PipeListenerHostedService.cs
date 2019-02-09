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
    public class PipeListenerHostedService : HostedService
    {
        private readonly MessagingService _messagingService;

        public PipeListenerHostedService(MessagingService messagingService)
        {
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));

        }
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var token = cts.Token;
                await Task.WhenAny(
                    _messagingService.ListenAsync(token),
                    _messagingService.PingAsync(token)
                    );

                // If either Task completes, cancel the other one
                cts.Cancel();
            }
        }
    }
}