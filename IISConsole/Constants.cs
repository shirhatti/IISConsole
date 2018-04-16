using System;

namespace IISConsole
{
    internal static class Constants
    {
        internal const string HttpScheme = "http";
        internal const string HttpsScheme = "https";
        internal const string Chunked = "chunked";
        internal const string Close = "close";
        internal const string Zero = "0";
        internal const string SchemeDelimiter = "://";

        internal static Version V1_0 = new Version(1, 0);
        internal static Version V1_1 = new Version(1, 1);
    }
}