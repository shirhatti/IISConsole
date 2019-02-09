using System;

namespace IISConsole
{
    public class PipeListenerOptions
    {
        public string PipeName { get; set; }
        public TimeSpan PingFrequency { get; set; }
    }
}