namespace KLauncher.Shared.Interface;

public interface ILanguageManager
{
    public string Get(string key);
    public string Get(string key, params object[] args);
    public void SetLanguage(string language);
    public string GetLanguage();
    public string GetLanguageName(string language);
    public string[] GetLanguages();
    public string[] GetLanguageNames();
}