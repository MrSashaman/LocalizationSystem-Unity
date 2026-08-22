using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;

    private Text uiText;

    private void Awake()
    {
        uiText = GetComponent<Text>();

        if (uiText == null)
        {
            Debug.LogError("LocalizedText: Text component not found!", this);
        }
    }

    private void OnEnable()
    {
        LocalizationSystem.LanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        LocalizationSystem.LanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (uiText == null)
            return;

        uiText.text = LocalizationSystem.GetText(key);
    }
}
