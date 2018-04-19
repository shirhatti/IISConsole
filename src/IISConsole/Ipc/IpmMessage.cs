using System;
using static IISConsole.IpmTypes;

namespace IISConsole.Ipc
{
    internal class IpmMessage
    {
        public IPM_OPCODE OpCode { get; set; }

        public uint MessageSize { get; set; }
        public uint MessageBodySize
        {
            get => (MessageSize - (uint)MessageHeaderSize);
            set => MessageSize = value + (uint)MessageHeaderSize;
        }

        public static readonly int MessageHeaderSize = sizeof(IPM_OPCODE) + sizeof(uint);

        public IpmMessage(IPM_OPCODE opCode)
        {
            OpCode = opCode;
            MessageSize = (uint)MessageHeaderSize;
        }
        public IpmMessage(IPM_OPCODE opCode, uint messageSize)
        {
            OpCode = opCode;
            MessageSize = messageSize;
        }

        public IpmMessage(byte[] buffer)
        {
            OpCode = (IPM_OPCODE)BitConverter.ToInt32(buffer, 0);
            MessageSize = BitConverter.ToUInt32(buffer, sizeof(IPM_OPCODE));
        }

        public IpmMessage(IpmMessage message)
        {
            OpCode = message.OpCode;
            MessageSize = message.MessageSize;
        }

        public virtual byte[] ToByteArray()
        {
            byte[] ret = new byte[MessageHeaderSize];
            Buffer.BlockCopy(BitConverter.GetBytes((int)OpCode), 0, ret, 0, sizeof(IPM_OPCODE));
            Buffer.BlockCopy(BitConverter.GetBytes(MessageSize), 0, ret, sizeof(IPM_OPCODE), sizeof(uint));
            return ret;
        }
    }
}
