using System;
using static IISConsole.IpmTypes;

namespace IISConsole.Ipc
{
    internal class GetPidMessage : IpmMessage
    {
        public uint ProcessId { get; set; }
        public GetPidMessage(IPM_OPCODE opCode, uint messageSize, uint processId): base(opCode, messageSize)
        {
            ProcessId = processId;
        }
        public GetPidMessage(IpmMessage header, byte[] buffer) : base(header)
        {
            ProcessId = BitConverter.ToUInt32(buffer, 0);
        }

        public override byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }
    }
}
