using System;

namespace Nekoyume
{
    public readonly struct AppProtocolVersionExtra
    {
        private const string MacOSBinaryUrlKey = "macOSBinaryUrl";

        private const string WindowsBinaryUrlKey = "WindowsBinaryUrl";

        private const string TimestampKey = "timestamp";

        public readonly string MacOSBinaryUrl;

        public readonly string WindowsBinaryUrl;

        public readonly DateTimeOffset Timestamp;

        public AppProtocolVersionExtra(string macOSBinaryUrl, string windowsBinaryUrl, DateTimeOffset timestamp)
        {
            MacOSBinaryUrl = macOSBinaryUrl;
            WindowsBinaryUrl = windowsBinaryUrl;
            Timestamp = timestamp;
        }
    }
}
