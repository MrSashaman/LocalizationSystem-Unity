using UnityEngine;
using System.Collections.Generic;

public class LocalizationSystem : MonoBehaviour
{
    public static LocalizationSystem instance;

    private const string LanguagePrefsKey = "language";
    private const string DefaultLanguage = "en";

    private readonly Dictionary<string, string> localizedText =
        new Dictionary<string, string>();

    private string currentLanguage = DefaultLanguage;

    [SerializeField] private TextAsset localizationFileEN;
    [SerializeField] private TextAsset localizationFileRU;

    public delegate void OnLanguageChanged();
    public static event OnLanguageChanged LanguageChanged;

    public string CurrentLanguage => currentLanguage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        currentLanguage = NormalizeLanguage(
            PlayerPrefs.GetString(LanguagePrefsKey, DefaultLanguage)
        );
        LoadLanguage(currentLanguage);
    }

    public void LoadLanguage(string languageCode)
    {
        currentLanguage = NormalizeLanguage(languageCode);
        PlayerPrefs.SetString(LanguagePrefsKey, currentLanguage);
        PlayerPrefs.Save();

        TextAsset selectedFile = GetLocalizationFile(currentLanguage);
        if (selectedFile == null)
        {
            Debug.LogWarning(
                "Localization file not found for language: " + currentLanguage,
                this
            );
            return;
        }

        Dictionary<string, string> loadedText =
            new Dictionary<string, string>();

        string[] lines = selectedFile.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].TrimEnd('\r');

            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] keyValue = line.Split(new[] { '=' }, 2);
            if (keyValue.Length != 2)
            {
                Debug.LogWarning(
                    $"Invalid line in localization file {selectedFile.name} at line {i + 1}: {line}",
                    this
                );
                continue;
            }

            string key = keyValue[0].Trim();
            string value = keyValue[1].Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.LogWarning(
                    $"Empty key in localization file {selectedFile.name} at line {i + 1}.",
                    this
                );
                continue;
            }

            if (loadedText.ContainsKey(key))
            {
                Debug.LogWarning(
                    $"Duplicate localization key '{key}' in {selectedFile.name} at line {i + 1}. Last value wins.",
                    this
                );
            }

            loadedText[key] = value;
        }

        localizedText.Clear();
        foreach (KeyValuePair<string, string> entry in loadedText)
        {
            localizedText[entry.Key] = entry.Value;
        }

        LanguageChanged?.Invoke();
    }

    public void SetLocalizationFiles(TextAsset enFile, TextAsset ruFile)
    {
        localizationFileEN = enFile;
        localizationFileRU = ruFile;

        LoadLanguage(string.IsNullOrWhiteSpace(currentLanguage)
            ? PlayerPrefs.GetString(LanguagePrefsKey, DefaultLanguage)
            : currentLanguage);
    }

    public string GetLocalizedText(string key)
    {
        return GetLocalizedText(key, null);
    }

    public string GetLocalizedText(string key, string fallback)
    {
        if (string.IsNullOrWhiteSpace(key))
            return fallback ?? string.Empty;

        if (localizedText.TryGetValue(key, out string value))
            return value;

        Debug.LogWarning($"Translation missing for key: {key}", this);
        return key;
    }

    public string GetLocalizedFormat(string key, params object[] args)
    {
        string format = GetLocalizedText(key);

        if (args == null || args.Length == 0)
            return format;

        try
        {
            return string.Format(format, args);
        }
        catch (System.FormatException exception)
        {
            Debug.LogWarning(
                $"Invalid localization format for key '{key}': {exception.Message}",
                this
            );
            return format;
        }
    }

    public static string GetText(string key)
    {
        return instance != null ? instance.GetLocalizedText(key) : key;
    }

    public static string GetTextOrFallback(string key, string fallback)
    {
        if (string.IsNullOrWhiteSpace(key))
            return fallback ?? string.Empty;

        return instance != null
            ? instance.GetLocalizedText(key, fallback)
            : fallback ?? key;
    }

    public static string FormatText(string key, params object[] args)
    {
        return instance != null
            ? instance.GetLocalizedFormat(key, args)
            : key;
    }

    private TextAsset GetLocalizationFile(string languageCode)
    {
        return NormalizeLanguage(languageCode) == "ru"
            ? localizationFileRU
            : localizationFileEN;
    }

    private static string NormalizeLanguage(string languageCode)
    {
        return languageCode == "ru" ? "ru" : DefaultLanguage;
    }
}
