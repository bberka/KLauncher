using System.Net;
using EasMe.Result;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KLauncher.ServerLib.Middlewares;

public class ApiResponseToResultMiddleware : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    next(context);
    if (context.Response.StatusCode > 199 || context.Response.StatusCode < 300) {
      await next(context);
      return;
    }

    if (context.Response.StatusCode == (int)HttpStatusCode.NotFound) {
      var result = Result.NotFound();
      context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }

    await next(context);
  }
}