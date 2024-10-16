namespace KLauncher.Shared.Interface;

public interface ICacheManager
{
  public void AddCache(string key, object value);
  public void RemoveCache(string key);
  public object? GetCache(string key);
  public T? GetCache<T>(string key);
  public bool HasCache(string key);
  public void ClearCache();
}