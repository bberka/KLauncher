using Microsoft.AspNetCore.Http;

namespace KLauncher.Core.Manager;

public static class HttpContextLib
{
  private static readonly HttpContextAccessor? Accessor = new();

  public static HttpContext? Current => Accessor?.HttpContext;
}