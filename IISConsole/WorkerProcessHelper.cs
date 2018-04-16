using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IISConsole
{
    public class WorkerProcessHelper : IDisposable
    {
        // TODO move to WorkerProcessOptions
        private static readonly string _w3wpPath = "C:\\Windows\\System32\\inetsrv\\w3wp.exe";
        private static readonly string _w3wpArguments = "-u";
        private Process _process = new Process();

        private volatile State _state;
        private object _internalLock;

        public WorkerProcessHelper(WorkerProcessOptions workerProcessOptions)
        {
            _state = State.Stopped;
            _internalLock = new object();

            Options = workerProcessOptions;
            if (Options.UrlPrefixes.Count == 0)
            {
                Options.UrlPrefixes.Add("http://localhost:1333");
            }
        }

        public WorkerProcessOptions Options { get; }
        internal ServerSession ServerSession { get; private set; }
        internal UrlGroup UrlGroup { get; private set; }
        internal RequestQueue RequestQueue { get; private set; }

        internal enum State
        {
            Stopped,
            Started,
            Disposed,
        }

        public bool IsListening
        {
            get { return _state == State.Started; }
        }

        public void Start()
        {
            // 1. Create Http.Sys queue
            try
            {
                ServerSession = new ServerSession();
                UrlGroup = new UrlGroup(ServerSession);
                RequestQueue = new RequestQueue(UrlGroup);
            }
            catch (Exception)
            {
                // If Url group or request queue creation failed, close server session before throwing.
                RequestQueue?.Dispose();
                UrlGroup?.Dispose();
                ServerSession?.Dispose();
                throw;
            }

            // 2. Attach request queue to url group
            lock (_internalLock)
            {
                try
                {
                    CheckDisposed();
                    if (_state == State.Started)
                    {
                        return;
                    }

                    Options.Apply(UrlGroup, RequestQueue);
                    RequestQueue.AttachToUrlGroup();

                    // 3. Register url prefixes
                    try
                    {
                        Options.UrlPrefixes.RegisterAllPrefixes(UrlGroup);
                    }
                    catch (HttpSysException)
                    {
                        RequestQueue.DetachFromUrlGroup();
                        throw;
                    }
                }
                catch (Exception)
                {
                    _state = State.Disposed;
                    DisposeInternal();
                    throw;
                }
            }
            // 4. Start Process
            //_process.StartInfo.FileName = _w3wpPath;
            //_process.StartInfo.Arguments = _w3wpArguments;
            //_process.StartInfo.UseShellExecute = false;
            //_process.StartInfo.CreateNoWindow = true;
            //_process.Start();
            //// TODO
            //// Start process in suspended mode, add to job object, and then resume
            //ChildProcessTracker.AddProcess(_process);
        }

        private void Stop()
        {
            // TODO
            // Send Ctrl+C signal to worker process

            // Clean up Http.Sys
            lock (_internalLock)
            {
                CheckDisposed();
                if (_state == State.Stopped)
                {
                    return;
                }

                Options.UrlPrefixes.UnregisterAllPrefixes();

                _state = State.Stopped;

                RequestQueue.DetachFromUrlGroup();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (_internalLock)
            {
                try
                {
                    if (_state == State.Disposed)
                    {
                        return;
                    }

                    Stop();
                    DisposeInternal();
                }
                finally
                {
                    _state = State.Disposed;
                }
            }
        }
        private void DisposeInternal()
        {
            // V2 stopping sequence:
            // 1. Detach request queue from url group - Done in Stop()/Abort()
            // 2. Remove urls from url group - Done in Stop()
            // 3. Close request queue - Done in Stop()/Abort()
            // 4. Close Url group.
            // 5. Close server session.

            RequestQueue.Dispose();

            UrlGroup.Dispose();

            Debug.Assert(ServerSession != null, "ServerSessionHandle is null in CloseV2Config");
            Debug.Assert(!ServerSession.Id.IsInvalid, "ServerSessionHandle is invalid in CloseV2Config");

            ServerSession.Dispose();
        }

        private void CheckDisposed()
        {
            if (_state == State.Disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
