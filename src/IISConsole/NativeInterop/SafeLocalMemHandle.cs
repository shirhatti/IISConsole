using Microsoft.Win32.SafeHandles;
using System;

namespace IISConsole
{
    internal sealed class SafeLocalMemHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal SafeLocalMemHandle()
            : base(true)
        {
        }

        internal SafeLocalMemHandle(IntPtr existingHandle, bool ownsHandle)
            : base(ownsHandle)
        {
            SetHandle(existingHandle);
        }

        protected override bool ReleaseHandle()
        {
            return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(handle) == IntPtr.Zero;
        }
    }
}