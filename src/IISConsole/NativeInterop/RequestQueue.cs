using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace IISConsole
{
    public class RequestQueue
    {
        private static readonly int BindingInfoSize =
            Marshal.SizeOf<HttpApiTypes.HTTP_BINDING_INFO>();

        private readonly UrlGroup _urlGroup;
        private bool _disposed;



        internal RequestQueue(UrlGroup urlGroup)
        {
            _urlGroup = urlGroup;

            var queueName = "IISConsole";
            HttpRequestQueueV2Handle requestQueueHandle = null;

            var statusCode = HttpApi.HttpCreateRequestQueue(
                    HttpApi.Version, queueName, null, HttpApiTypes.HTTP_CREATE_REQUEST_QUEUE_FLAGS.HTTP_CREATE_REQUEST_QUEUE_FLAG_CONTROLLER, out requestQueueHandle);

            if (statusCode != UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS)
            {
                throw new HttpSysException((int)statusCode);
            }

            Handle = requestQueueHandle;
            BoundHandle = ThreadPoolBoundHandle.BindHandle(Handle);
        }

        internal SafeHandle Handle { get; }
        internal ThreadPoolBoundHandle BoundHandle { get; }

        internal unsafe void AttachToUrlGroup()
        {
            CheckDisposed();
            // Set the association between request queue and url group. After this, requests for registered urls will 
            // get delivered to this request queue.

            var info = new HttpApiTypes.HTTP_BINDING_INFO();
            info.Flags = HttpApiTypes.HTTP_FLAGS.HTTP_PROPERTY_FLAG_PRESENT;
            info.RequestQueueHandle = Handle.DangerousGetHandle();

            var infoptr = new IntPtr(&info);

            _urlGroup.SetProperty(HttpApiTypes.HTTP_SERVER_PROPERTY.HttpServerBindingProperty,
                infoptr, (uint)BindingInfoSize);
        }

        internal unsafe void DetachFromUrlGroup()
        {
            CheckDisposed();
            // Break the association between request queue and url group. After this, requests for registered urls 
            // will get 503s.
            // Note that this method may be called multiple times (Stop() and then Abort()). This
            // is fine since http.sys allows to set HttpServerBindingProperty multiple times for valid 
            // Url groups.

            var info = new HttpApiTypes.HTTP_BINDING_INFO();
            info.Flags = HttpApiTypes.HTTP_FLAGS.NONE;
            info.RequestQueueHandle = IntPtr.Zero;

            var infoptr = new IntPtr(&info);

            _urlGroup.SetProperty(HttpApiTypes.HTTP_SERVER_PROPERTY.HttpServerBindingProperty,
                infoptr, (uint)BindingInfoSize, throwOnError: false);
        }

        internal unsafe void SetLengthLimit(long length)
        {
            CheckDisposed();

            var result = HttpApi.HttpSetRequestQueueProperty(Handle,
                HttpApiTypes.HTTP_SERVER_PROPERTY.HttpServerQueueLengthProperty,
                new IntPtr((void*)&length), (uint)Marshal.SizeOf<long>(), 0, IntPtr.Zero);

            if (result != 0)
            {
                throw new HttpSysException((int)result);
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            BoundHandle.Dispose();
            Handle.Dispose();
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
