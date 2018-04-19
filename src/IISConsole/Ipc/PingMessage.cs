using System;
using System.Collections.Generic;
using System.Text;
using static IISConsole.IpmTypes;

namespace IISConsole.Ipc
{
    internal class PingMessage : IpmMessage
    {
        public PingMessage() : base(IPM_OPCODE.IPM_OP_PING)
        {
        }
    }
}
