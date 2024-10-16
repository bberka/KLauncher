using System.Management;
using System.Security.Principal;
using System.Text;

namespace KLauncher.Core.Manager;

/// <summary>
///     Creates a unique device id based on the device information. This is only supported on Windows.
/// </summary>
public class DeviceIdManager
{
  private static DeviceIdManager? _instance;

  private DeviceIdManager() {
    RegisterDeviceIds();
  }

  public static DeviceIdManager This {
    get {
      _instance ??= new DeviceIdManager();
      return _instance;
    }
  }

  public string FullId => $"{Id1}-{Id2}-{Id3}-{Id4}-{Id5}-{Id6}-{Id7}";
  public string Id1 { get; private set; } = string.Empty;
  public string Id2 { get; private set; } = string.Empty;
  public string Id3 { get; private set; } = string.Empty;
  public string Id4 { get; private set; } = string.Empty;
  public string Id5 { get; private set; } = string.Empty;
  public string Id6 { get; private set; } = string.Empty;
  public string Id7 { get; private set; } = string.Empty;

  private void RegisterDeviceIds() {
    Id1 = HashManager.HashAsHexString(GetNtAccountSecIdentifier());
    Id2 = HashManager.HashAsHexString(GetBIOSCombinationIdentifier());
    Id3 = HashManager.HashAsHexString(GetBaseBoardIdentifier());
    Id4 = HashManager.HashAsHexString(GetMACIdentifier());
    Id5 = HashManager.HashAsHexString(GetVideoControllerIdentifier());
    Id6 = HashManager.HashAsHexString(GetCPUIdentifier());
    Id7 = HashManager.HashAsHexString(GetDiskDriveIdentifier());
  }

  private static string GetNtAccountSecIdentifier() {
    var nTAccount = new NTAccount(WindowsIdentity.GetCurrent().Name);
    var securityIdentifier = (SecurityIdentifier)nTAccount.Translate(typeof(SecurityIdentifier));
    return securityIdentifier.ToString();
  }

  private static string GetBIOSCombinationIdentifier() {
    return string.Concat(ManagementExtension.GetManagementObjectFromName("Win32_BIOS", "Manufacturer"),
                         ManagementExtension.GetManagementObjectFromName("Win32_BIOS", "SMBIOSBIOSVersion"),
                         ManagementExtension.GetManagementObjectFromName("Win32_BIOS", "IdentificationCode"),
                         ManagementExtension.GetManagementObjectFromName("Win32_BIOS", "SerialNumber"),
                         ManagementExtension.GetManagementObjectFromName("Win32_BIOS", "ReleaseDate"),
                         ManagementExtension.GetManagementObjectFromName("Win32_BIOS", "Version"));
  }

  private static string GetBaseBoardIdentifier() {
    return string.Concat(new[] {
      ManagementExtension.GetManagementObjectFromName("Win32_BaseBoard", "Model"),
      ManagementExtension.GetManagementObjectFromName("Win32_BaseBoard", "Manufacturer"),
      ManagementExtension.GetManagementObjectFromName("Win32_BaseBoard", "Name"),
      ManagementExtension.GetManagementObjectFromName("Win32_BaseBoard", "SerialNumber")
    });
  }

  private static string GetMACIdentifier() {
    return ManagementExtension.GetManagementObjectFromNameWithStatement("Win32_NetworkAdapterConfiguration",
                                                                        "MACAddress",
                                                                        "IPEnabled");
  }

  private static string GetVideoControllerIdentifier() {
    return string.Concat(new[] {
      ManagementExtension.GetManagementObjectFromName("Win32_VideoController", "DriverVersion"),
      ManagementExtension.GetManagementObjectFromName("Win32_VideoController", "Name")
    });
  }

  private static string GetCPUIdentifier() {
    var rv = ManagementExtension.GetManagementObjectFromName("Win32_Processor", "UniqueId");

    while (rv == "") {
      rv = ManagementExtension.GetManagementObjectFromName("Win32_Processor", "ProcessorId");
      rv = ManagementExtension.GetManagementObjectFromName("Win32_Processor", "Name");
      rv = ManagementExtension.GetManagementObjectFromName("Win32_Processor", "Manufacturer") +
           ManagementExtension.GetManagementObjectFromName("Win32_Processor", "MaxClockSpeed");
    }

    return rv;
  }

  private static string GetDiskDriveIdentifier() {
    return string.Concat(new[] {
      ManagementExtension.GetManagementObjectFromName("Win32_DiskDrive", "Model"),
      ManagementExtension.GetManagementObjectFromName("Win32_DiskDrive", "Manufacturer"),
      ManagementExtension.GetManagementObjectFromName("Win32_DiskDrive", "Signature"),
      ManagementExtension.GetManagementObjectFromName("Win32_DiskDrive", "TotalHeads")
    });
  }

  private class ManagementExtension
  {
    internal static string GetManagementObjectFromName(string className, string instanceName) {
      var rv = "";

      var sb = new StringBuilder();
      using var mosearcher = new ManagementObjectSearcher($"SELECT * FROM {className}");
      using var mocollection = mosearcher.Get();

      foreach (ManagementObject mo in mocollection) {
        rv = mo[instanceName] != null
               ? mo[instanceName].ToString()
               : "";
        break;
      }

      return rv;
    }

    internal static string GetManagementObjectFromNameWithStatement(string className,
                                                                    string instanceName,
                                                                    string statement) {
      var rv = "";

      var managementClass = new ManagementClass(className);
      var instances = managementClass.GetInstances();

      foreach (var current in instances)
        if (!(current[statement].ToString() != "True") && !(rv != "")) {
          rv = current[instanceName].ToString();
          break;
        }

      return rv;
    }
  }
}