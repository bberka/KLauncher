using KLauncher.Shared.Interface;

namespace KLauncher.ServerLib.Models;

public class AuthServerConfiguration : IAuthServerConfiguration
{
  public const string SectionName = "AuthServerConfiguration";

  public string[] AllowIpAddress { get; }
  public string RealIpAddressHeader { get; set; }
}