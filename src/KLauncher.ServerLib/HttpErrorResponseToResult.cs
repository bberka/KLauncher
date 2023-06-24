using EasMe.Result;
using Microsoft.AspNetCore.Mvc;

namespace KLauncher.ServerLib;

public class HttpErrorResponseToResult
{
    private readonly ActionContext _actionContext;

    public HttpErrorResponseToResult(ActionContext actionContext) {
        _actionContext = actionContext;
    }

    public IActionResult ToResult() {
        var errors = _actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => new ValidationError {
            Message =  e.ErrorMessage,
        }).ToList();
        return new BadRequestObjectResult(Result.ValidationError(errors));
    }
}