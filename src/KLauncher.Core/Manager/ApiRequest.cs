using Ardalis.Result;
using KLauncher.Shared.Enum;

namespace KLauncher.Core.Manager;

/// <summary>
/// ApiRequest client for sending requests to the api
/// </summary>
public class ApiRequest
{
    private readonly string _url;
    private readonly HttpMethodType _httpMethod;
    private readonly Dictionary<string, string> _headerDictionary;
    private object? _data;
    public ApiRequest(string url, HttpMethodType httpMethod) {
        _url = url;
        _httpMethod = httpMethod;
        _headerDictionary = new Dictionary<string, string>();
        _data = default;
    }

    /// <summary>
    ///  Adds the header to the request. If the header already exists, replaces the value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddHeader(string key, string value) {
        var exists = _headerDictionary.ContainsKey(key);
        if (exists) {
            _headerDictionary[key] = value;
            return;
        }
        _headerDictionary.Add(key, value);
    }
    /// <summary>
    /// Removes the header from the request.
    /// </summary>
    /// <param name="key"></param>
    public void RemoveHeader(string key) {
        var exists = _headerDictionary.ContainsKey(key);
        if (exists) {
            _headerDictionary.Remove(key);
        }
    }
    /// <summary>
    /// Sets the user agent header of the request.
    /// </summary>
    /// <param name="userAgent"></param>
    public void SetUserAgent(string userAgent) {
        AddHeader("User-Agent", userAgent);
    }

    /// <summary>
    /// Sets the authorization header of the request.
    /// </summary>
    /// <param name="authorization"></param>
    public void SetAuthorization(string authorization) {
        AddHeader("Authorization", authorization);
    }
    /// <summary>
    /// Sets the content type of the request. This is only used for POST requests.
    /// </summary>
    /// <param name="contentType"></param>
    public void SetContentType(string contentType) {
        AddHeader("Content-Type", contentType);
    }
    /// <summary>
    /// Sets the body of the request. This is only used for POST requests.
    /// </summary>
    /// <param name="obj"></param>
    public void SetBody(object obj) {
        _data = obj;
    }

    /// <summary>
    /// Sends the request with given timeout. If the request is successful, returns the result. If not, returns the result.
    /// </summary>
    /// <param name="timeoutSeconds"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Result<string> Send(int timeoutSeconds = 10) {
        return _httpMethod switch {
            HttpMethodType.Get => ApiManager.This.Get(_url, _headerDictionary, timeoutSeconds),
            HttpMethodType.Post => ApiManager.This.Post(_url, _headerDictionary, _data, timeoutSeconds),
            //HttpMethod.Put => ApiManager.Put(_url, _headerDictionary, _data),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Sends the request with given retry count. If the request is successful, returns the result. If not, returns the last result.
    /// </summary>
    /// <param name="retryCount"></param>
    /// <param name="timeoutSeconds"></param>
    /// <returns></returns>
    public Result<string> SendWithRetry(byte retryCount, int timeoutSeconds = 10) {
        if (retryCount is 0 or 1) {
            return Send(timeoutSeconds);
        }
        var lastResult = default(Result<string>);
        for (var i = 0; i < retryCount; i++) {
            var result = Send(timeoutSeconds);
            if (result.IsSuccess) {
                return result;
            }
            lastResult = result;
        }
        return lastResult;
    }
}