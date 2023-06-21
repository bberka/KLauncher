using Ardalis.Result;
using KLauncher.Shared.Enum;

namespace KLauncher.Shared;

public static class DomainResult
{
    public static Result Ok(ResultCode resultCode) => Result.SuccessWithMessage(resultCode.ToString());
    public static Result Ok() => Result.Success();
    public static Result Forbidden() => Result.Forbidden();
    public static Result Unauthorized() => Result.Unauthorized();
    public static Result NotFound() => Result.NotFound();
    public static Result Error() => Result.Error();
    public static Result Error(ResultCode resultCode) => Result.Error(resultCode.ToString());

    //public static Result Error(ResultCode resultCode, params string[] errors) {
    //    var codeErrorString = resultCode.ToString();
    //    Result.Error(errors.Append(codeErrorString).ToArray());
    //    return Result.Error(errors.Append(codeErrorString).ToArray());
    //}
    public static Result Invalid(List<ValidationError> errors) => Result.Invalid(errors);
    public static Result Invalid(params ValidationError[] errors) => Result.Invalid(errors.ToList());
    public static Result<T> Ok<T>(T value) => Result<T>.Success(value);
    public static Result Fail(string message) => Result.Error(message);
    public static Result<T> Fail<T>(string message) => Result<T>.Error(message);


}