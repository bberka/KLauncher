using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KLauncher.ServerLib;

public class HttpErrorResponseToResult
{
    private readonly ActionContext _actionContext;

    public HttpErrorResponseToResult(ActionContext actionContext) {
        _actionContext = actionContext;
    }

    public IActionResult ToResult() {
        var errors = _actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => new ValidationError() {
            Identifier = e.Exception?.GetType().Name ?? e.ErrorMessage.Replace(" ","_"),
            ErrorMessage = e.Exception?.Message ?? e.ErrorMessage,
            Severity = e.Exception is not null ? ValidationSeverity.Error : ValidationSeverity.Warning
        }).ToList();
        return new BadRequestObjectResult(Result.Invalid(errors));
    }
}