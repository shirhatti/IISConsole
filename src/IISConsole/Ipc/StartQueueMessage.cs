using System;
using System.Collections.Generic;
using System.Text;
using static IISConsole.IpmTypes;

namespace IISConsole.Ipc
{
    internal class StartQueueMessage : IpmMessage
    {
        private static readonly string protocolId = "http";
        private static readonly string protocolManagerDll = "";
        private static readonly string protocolManagerDllInitFunction = "";
        // Add two bytes for alignment if not divisible by 4
        private static readonly int listenerChannelId = 0;
        private static readonly int listenerChannelBlobByteCount = 0;
        private int _alignmentSize;
        public StartQueueMessage() : base(IPM_OPCODE.IPM_OP_START_QUEUE)
        {
            // We need length including null character. That's why the +1
            var size = (protocolId.Length + 1
                        + protocolManagerDll.Length + 1
                        + protocolManagerDllInitFunction.Length + 1)
                        * sizeof(Char);
            // We need to align on the DWORD
            _alignmentSize = (size % 4);
            size += _alignmentSize;
            size += sizeof(Int32) * 2;

            MessageBodySize = (uint)size;
        }

        public override byte[] ToByteArray()
        {
            var buffer = new byte[MessageSize];
            Buffer.BlockCopy(base.ToByteArray(), 0, buffer, 0, MessageHeaderSize);
            var index = MessageHeaderSize;

            // protocolId
            var writeSize = protocolId.Length * sizeof(Char);
            Buffer.BlockCopy(protocolId.ToCharArray(), 0, buffer, index, writeSize);
            index += writeSize;

            // protocolManagerDll
            writeSize = protocolManagerDll.Length * sizeof(Char);
            Buffer.BlockCopy(protocolManagerDll.ToCharArray(), 0, buffer, index, writeSize);
            index += writeSize;

            // protocolManagerDllInitFunction
            writeSize = protocolManagerDllInitFunction.Length * sizeof(Char);
            Buffer.BlockCopy(protocolManagerDllInitFunction.ToCharArray(), 0, buffer, index, writeSize);
            index += writeSize;

            // Alignment
            index += _alignmentSize;

            // listenerChannelId
            writeSize = sizeof(int);
            Buffer.BlockCopy(BitConverter.GetBytes(listenerChannelId), 0, buffer, index, writeSize);
            index += writeSize;

            // listenerChannelBlobByteCount
            writeSize = sizeof(int);
            Buffer.BlockCopy(BitConverter.GetBytes(listenerChannelBlobByteCount), 0, buffer, index, writeSize);
            index += writeSize;

            return buffer;
        }
    }
}
