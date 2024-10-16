namespace KLauncher.ClientLib.Models;

public class ClientConfiguration
{
  /// <summary>
  ///     File information and download api domain. Used to list files and download them.
  ///     If file download fails with Cloud domain and Cloud domain is not set, real download api domain will be used.
  /// </summary>
  public const string DownloadApiDomain = "";

  /// <summary>
  ///     If set, cloud download api will be used to download files. If cloud download api not available, real download api
  ///     will be used.
  /// </summary>
  public const string CloudDownloadApiDomain = "";

  /// <summary>
  ///     Domain of the auth api
  /// </summary>
  public const string AuthApiDomain = "";

  public string ClientFilesDirectoryPath { get; set; }
  public string Language { get; set; }
}