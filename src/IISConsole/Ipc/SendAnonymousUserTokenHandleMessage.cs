using System;
using static IISConsole.IpmTypes;

namespace IISConsole.Ipc
{
    internal class SendAnonymousUserTokenHandleMessage : IpmMessage
    {
        private readonly UInt64 _tokenHandle;
        public SendAnonymousUserTokenHandleMessage(UInt64 TokenHandle) : base(IPM_OPCODE.IPM_OP_ANONYMOUS_TOKEN_HANDLE)
        {
            _tokenHandle = TokenHandle;
            MessageBodySize = sizeof(UInt64);
        }

        public override byte[] ToByteArray()
        {
            var buffer = new byte[MessageSize];
            Buffer.BlockCopy(base.ToByteArray(), 0, buffer, 0, MessageHeaderSize);
            buffer[MessageHeaderSize] = Convert.ToByte(_tokenHandle);
            return buffer;
        }
    }
}
