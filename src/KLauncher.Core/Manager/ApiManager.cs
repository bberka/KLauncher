using System.Text;
using Ardalis.Result;
using Newtonsoft.Json;

namespace KLauncher.Core.Manager;

/// <summary>
/// Basic api request sender class
/// </summary>
public class ApiManager
{
    private ApiManager() { }
    public static ApiManager This {
        get {
            Instance ??= new();
            return Instance;
        }
    }
    private static ApiManager? Instance;
    public Result<string> Post(string url, Dictionary<string,string> headerDictionary,object? data,int timeoutSeconds) {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        foreach (var (key, value) in headerDictionary) {
            request.Headers.Add(key,value);
        }
        var json = data != null ? JsonConvert.SerializeObject(data) : "{}";
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = client.SendAsync(request).Result;
        var responseContent = response.Content.ReadAsStringAsync().Result;
        return response.IsSuccessStatusCode ? Result.Success(responseContent) : Result.Error(responseContent);
    }

    public Result<string> Get(string url, Dictionary<string, string> headerDictionary, int timeout) {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(timeout);
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        foreach (var (key, value) in headerDictionary) {
            request.Headers.Add(key,value);
        }
        var response = client.SendAsync(request).Result;
        var responseContent = response.Content.ReadAsStringAsync().Result;
        return response.IsSuccessStatusCode ? Result.Success(responseContent) : Result.Error(responseContent);

    }

    
}