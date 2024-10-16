namespace KLauncher.Shared.Interface;

public interface IBaseServerConfiguration
{
  /// <summary>
  ///     Whitelist of allowed ip addresses. If empty, all ip addresses are allowed.
  /// </summary>
  public string[] AllowIpAddress { get; }

  public string RealIpAddressHeader { get; set; }
}