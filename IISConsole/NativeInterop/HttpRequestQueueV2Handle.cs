using Microsoft.Win32.SafeHandles;

namespace IISConsole
{
    internal sealed class HttpRequestQueueV2Handle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private HttpRequestQueueV2Handle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return (HttpApi.HttpCloseRequestQueue(handle) ==
                        UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS);
        }
    }
}