﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IISConsole
{
    internal class UrlGroup : IDisposable
    {
        private static readonly int QosInfoSize =
            Marshal.SizeOf<HttpApiTypes.HTTP_QOS_SETTING_INFO>();

        private ServerSession _serverSession;
        private bool _disposed;

        internal unsafe UrlGroup(ServerSession serverSession)
        {
            _serverSession = serverSession;

            ulong urlGroupId = 0;
            var statusCode = HttpApi.HttpCreateUrlGroup(
                _serverSession.Id.DangerousGetServerSessionId(), &urlGroupId, 0);

            if (statusCode != UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS)
            {
                throw new HttpSysException((int)statusCode);
            }

            Debug.Assert(urlGroupId != 0, "Invalid id returned by HttpCreateUrlGroup");
            Id = urlGroupId;
        }

        internal ulong Id { get; private set; }

        internal unsafe void SetMaxConnections(long maxConnections)
        {
            var connectionLimit = new HttpApiTypes.HTTP_CONNECTION_LIMIT_INFO();
            connectionLimit.Flags = HttpApiTypes.HTTP_FLAGS.HTTP_PROPERTY_FLAG_PRESENT;
            connectionLimit.MaxConnections = (uint)maxConnections;

            var qosSettings = new HttpApiTypes.HTTP_QOS_SETTING_INFO();
            qosSettings.QosType = HttpApiTypes.HTTP_QOS_SETTING_TYPE.HttpQosSettingTypeConnectionLimit;
            qosSettings.QosSetting = new IntPtr(&connectionLimit);

            SetProperty(HttpApiTypes.HTTP_SERVER_PROPERTY.HttpServerQosProperty, new IntPtr(&qosSettings), (uint)QosInfoSize);
        }

        internal void SetProperty(HttpApiTypes.HTTP_SERVER_PROPERTY property, IntPtr info, uint infosize, bool throwOnError = true)
        {
            Debug.Assert(info != IntPtr.Zero, "SetUrlGroupProperty called with invalid pointer");
            CheckDisposed();

            var statusCode = HttpApi.HttpSetUrlGroupProperty(Id, property, info, infosize);

            if (statusCode != UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS)
            {
                var exception = new HttpSysException((int)statusCode);
                if (throwOnError)
                {
                    throw exception;
                }
            }
        }

        internal void RegisterPrefix(string uriPrefix, int contextId)
        {
            CheckDisposed();

            var statusCode = HttpApi.HttpAddUrlToUrlGroup(Id, uriPrefix, (ulong)contextId, 0);

            if (statusCode != UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS)
            {
                if (statusCode == UnsafeNclNativeMethods.ErrorCodes.ERROR_ALREADY_EXISTS)
                {
                    throw new HttpSysException((int)statusCode);
                }
                else
                {
                    throw new HttpSysException((int)statusCode);
                }
            }
        }

        internal bool UnregisterPrefix(string uriPrefix)
        {
            CheckDisposed();

            var statusCode = HttpApi.HttpRemoveUrlFromUrlGroup(Id, uriPrefix, 0);

            if (statusCode == UnsafeNclNativeMethods.ErrorCodes.ERROR_NOT_FOUND)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            Debug.Assert(Id != 0, "HttpCloseUrlGroup called with invalid url group id");

            uint statusCode = HttpApi.HttpCloseUrlGroup(Id);

            if (statusCode != UnsafeNclNativeMethods.ErrorCodes.ERROR_SUCCESS)
            {
            }
            Id = 0;
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