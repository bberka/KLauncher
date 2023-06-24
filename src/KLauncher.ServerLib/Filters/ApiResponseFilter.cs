using Microsoft.AspNetCore.Mvc.Filters;

namespace KLauncher.ServerLib.Filters;

public class ApiResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) {
    }

    public void OnActionExecuted(ActionExecutedContext context) {
        var httpContext = context.HttpContext;
        var statusCode = httpContext.Response.StatusCode;
        if (statusCode == 200) return;
        // var isBadRequest = statusCode == 400;
        // if (isNotFound) {
        //     
        //     
        //     var exceptionHandlerFeature = context.HttpContext.Features.Get<IExceptionHandlerFeature>();
        //     var exception = exceptionHandlerFeature?.Error;
        //     var message = exception?.Message;
        //     var stackTrace = exception?.StackTrace;
        //     var result = Result.Error(message,stackTrace);
        //     context.Result = new ObjectResult(result) {
        //         StatusCode = 500
        //     };
        //     return;
        // }
        //isBadRequest || isUnauthorized || isForbidden || isNotFound || 
    }
}