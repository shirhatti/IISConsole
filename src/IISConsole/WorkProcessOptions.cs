using System;

namespace IISConsole
{
    public class WorkerProcessOptions
    {
        private const long DefaultRequestQueueLength = 1000; // Http.sys default
        private const long DefaultMaxRequestBodySize = 30000000;
        private long _requestQueueLength = DefaultRequestQueueLength;
        private long? _maxConnections;
        private RequestQueue _requestQueue;
        private UrlGroup _urlGroup;
        private long? _maxRequestBodySize = DefaultMaxRequestBodySize;

        public WorkerProcessOptions()
        {
        }

        /// <summary>
        /// Gets or sets the maximum number of requests that will be queued up in Http.Sys.
        /// </summary>
        public long RequestQueueLimit
        {
            get
            {
                return _requestQueueLength;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "The value must be greater than zero.");
                }

                if (_requestQueue != null)
                {
                    _requestQueue.SetLengthLimit(_requestQueueLength);
                }
                // Only store it if it succeeds or hasn't started yet
                _requestQueueLength = value;
            }
        }
        public UrlPrefixCollection UrlPrefixes { get; } = new UrlPrefixCollection();

        internal void Apply(UrlGroup urlGroup, RequestQueue requestQueue)
        {
            _urlGroup = urlGroup;
            _requestQueue = requestQueue;

            if (_maxConnections.HasValue)
            {
                _urlGroup.SetMaxConnections(_maxConnections.Value);
            }

            if (_requestQueueLength != DefaultRequestQueueLength)
            {
                _requestQueue.SetLengthLimit(_requestQueueLength);
            }
        }
    }
}