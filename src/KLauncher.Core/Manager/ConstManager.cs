using System.Reflection;

namespace KLauncher.Core.Manager;

public static class ConstManager
{
    public static int BuildNumber {
        get {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (assemblyVersion is null) return 1;
            var major = assemblyVersion.Major;
            var minor = assemblyVersion.Minor;
            var build = assemblyVersion.Build;
            var revision = assemblyVersion.Revision;
            var buildNumber = major * 1000000 + minor * 10000 + build * 100 + revision;
            return buildNumber;
        }
    }

    public static bool IsDevelopment {
        get {
#if DEBUG
            return true;
#endif
            return false;
        }
    }
    
    
}