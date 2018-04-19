using System;
using System.Collections.Generic;
using System.Text;
using static IISConsole.IpmTypes;

namespace IISConsole.Ipc
{
    internal class ShutdownMessage : IpmMessage
    {
        private static readonly bool immidiately = true;
        public ShutdownMessage() : base(IPM_OPCODE.IPM_OP_SHUTDOWN)
        {
            MessageBodySize = 1;
        }
        public override byte[] ToByteArray()
        {
            var buffer = new byte[MessageSize];
            Buffer.BlockCopy(base.ToByteArray(), 0, buffer, 0, MessageHeaderSize);
            buffer[MessageHeaderSize] = Convert.ToByte(immidiately);
            return buffer;
        }
    }
}
