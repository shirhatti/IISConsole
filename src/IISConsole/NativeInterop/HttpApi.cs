using System;
using System.Runtime.InteropServices;
using static IISConsole.HttpApiTypes;

namespace IISConsole
{
    internal static unsafe class HttpApi
    {
        private const string HTTPAPI = "httpapi.dll";

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpInitialize(HTTPAPI_VERSION version, uint flags, void* pReserved);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpCreateServerSession(HTTPAPI_VERSION version, ulong* serverSessionId, uint reserved);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpCreateUrlGroup(ulong serverSessionId, ulong* urlGroupId, uint reserved);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint HttpAddUrlToUrlGroup(ulong urlGroupId, string pFullyQualifiedUrl, ulong context, uint pReserved);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpSetUrlGroupProperty(ulong urlGroupId, HTTP_SERVER_PROPERTY serverProperty, IntPtr pPropertyInfo, uint propertyInfoLength);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint HttpRemoveUrlFromUrlGroup(ulong urlGroupId, string pFullyQualifiedUrl, uint flags);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpCloseServerSession(ulong serverSessionId);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpCloseUrlGroup(ulong urlGroupId);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern uint HttpSetRequestQueueProperty(SafeHandle requestQueueHandle, HTTP_SERVER_PROPERTY serverProperty, IntPtr pPropertyInfo, uint propertyInfoLength, uint reserved, IntPtr pReserved);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern unsafe uint HttpCreateRequestQueue(HTTPAPI_VERSION version, string pName,
            UnsafeNclNativeMethods.SECURITY_ATTRIBUTES pSecurityAttributes, HTTP_CREATE_REQUEST_QUEUE_FLAGS flags, out HttpRequestQueueV2Handle pReqQueueHandle);

        [DllImport(HTTPAPI, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern unsafe uint HttpCloseRequestQueue(IntPtr pReqQueueHandle);


        private static HTTPAPI_VERSION version;

        // This property is used by HttpListener to pass the version structure to the native layer in API
        // calls. 

        internal static HTTPAPI_VERSION Version
        {
            get
            {
                return version;
            }
        }

        // This property is used by HttpListener to get the Api version in use so that it uses appropriate 
        // Http APIs.

        internal static HTTP_API_VERSION ApiVersion
        {
            get
            {
                if (version.HttpApiMajorVersion == 2 && version.HttpApiMinorVersion == 0)
                {
                    return HTTP_API_VERSION.Version20;
                }
                else if (version.HttpApiMajorVersion == 1 && version.HttpApiMinorVersion == 0)
                {
                    return HTTP_API_VERSION.Version10;
                }
                else
                {
                    return HTTP_API_VERSION.Invalid;
                }
            }
        }

        static HttpApi()
        {
            InitHttpApi(2, 0);
        }

        private static void InitHttpApi(ushort majorVersion, ushort minorVersion)
        {
            version.HttpApiMajorVersion = majorVersion;
            version.HttpApiMinorVersion = minorVersion;

            var statusCode = HttpInitialize(version, (uint)HTTP_FLAGS.HTTP_INITIALIZE_SERVER, null);

            supported = statusCode == UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS;
        }

        private static volatile bool supported;
        internal static bool Supported
        {
            get
            {
                return supported;
            }
        }
    }
}