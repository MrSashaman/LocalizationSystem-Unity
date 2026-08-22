using UnityEngine;

public class LanguageButtonHandler : MonoBehaviour
{
    public string languageCode; // Код языка ("en" или "ru")

    public void OnButtonClick()
    {
        if (LocalizationSystem.instance == null)
        {
            Debug.LogError("LocalizationSystem instance not found!");
            return;
        }

        if (string.IsNullOrWhiteSpace(languageCode))
        {
            Debug.LogWarning("LanguageButtonHandler: languageCode is empty.", this);
            return;
        }

        LocalizationSystem.instance.LoadLanguage(languageCode);
    }
}
