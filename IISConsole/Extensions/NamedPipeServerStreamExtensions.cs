using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IISConsole.IpmTypes;

namespace System.IO.Pipes
{
    internal static class NamedPipeServerStreamExtensions
    {
        internal static async Task<IPM_MESSAGE_HEADER> ReadIpmMessageHeaderAsync(this NamedPipeServerStream namedPipe, CancellationToken ct)
        {
            IPM_MESSAGE_HEADER header;
            var size = Marshal.SizeOf(typeof(IPM_MESSAGE_HEADER));
            byte[] buffer = new byte[size];

            if (await namedPipe.ReadAsync(buffer, 0, size, ct) != size)
            {
                // TODO better exceptions
                throw new ApplicationException();
            }
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    header = Marshal.PtrToStructure<IPM_MESSAGE_HEADER>((IntPtr)ptr);
                }
            }
            return header;
        }
    }
}
