using KLauncher.Shared.Enum;

namespace KLauncher.Core;

public static class SharedConfig
{
  public static readonly HashType HashType = HashType.XxHash;

  public static readonly string[] IgnoreGameFileExtensions = {
    ".log"
  };

  public const long MaxGameFileSize = 1024 * 1024 * 1024; //1GB

  public const bool CheckZeroByteGameFiles = true;
}