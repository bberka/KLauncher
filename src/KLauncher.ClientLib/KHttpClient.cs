﻿using KLauncher.Core.Manager;
using KLauncher.Shared.Models;
using Microsoft.Extensions.Options;

namespace KLauncher.ClientLib;

public class KHttpClient : HttpClient
{
  private readonly EncryptionManager _encryptionManager;
  private readonly IOptionsSnapshot<LauncherInformation> _options;

  public KHttpClient(
    IOptionsSnapshot<LauncherInformation> options) {
    _options = options;
    _encryptionManager = new EncryptionManager();
    var encryptedAgent = _encryptionManager.Encrypt(_options.Value.Name);
    DefaultRequestHeaders.Add("User-Agent", encryptedAgent);
  }
}