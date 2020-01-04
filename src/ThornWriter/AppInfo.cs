using System;
using System.Reflection;

namespace ThornWriter
{
    public static class AppInfo
    {
        public static Version GetVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        public static string GetFormattedVersion()
        {
            var version = GetVersion();
            return version.Major + "." + version.Minor;
        }

        public static string GetLicense()
        {
            return Resources.License.Replace("{CurrentYear}", DateTime.Now.Year.ToString());
        }
    }
}
