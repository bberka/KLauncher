namespace KLauncher.Shared.Models;

public class LauncherInformation
{
    public const string SectionName = "LauncherInformation";
    
    public Version Version { get; set; }
    public string Name { get; set; }
    public string UpdaterName { get; set; }
}