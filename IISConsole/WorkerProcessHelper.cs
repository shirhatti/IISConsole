using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IISConsole
{
    public class WorkerProcessHelper
    {
        private static readonly string _w3wpPath = "C:\\Windows\\System32\\inetsrv\\w3wp.exe";
        private Process _process = new Process();
        private RequestQueue _requestQueue;
        public void Start()
        {
            // 1. Create Http.Sys queue
            _requestQueue = new RequestQueue();

            // 2. Start Process
            _process.StartInfo.FileName = _w3wpPath;
            _process.StartInfo.Arguments = "-ap IISConsole";
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.CreateNoWindow = true;
            _process.Start();
            // Considering starting process in suspended mode
            ChildProcessTracker.AddProcess(_process);
        }


        public void Stop()
        {
            //_process.StandardInput.Write("Q");
        }
    }
}
