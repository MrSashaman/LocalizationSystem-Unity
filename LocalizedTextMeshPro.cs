using TMPro;
using UnityEngine;

public class LocalizedTextMeshPro : MonoBehaviour
{
    [SerializeField] private string key;

    private TMP_Text uiText;

    private void Awake()
    {
        uiText = GetComponent<TMP_Text>();

        if (uiText == null)
        {
            Debug.LogError("LocalizedTextMeshPro: TMP_Text component not found!", this);
        }
    }

    private void OnEnable()
    {
        LocalizationSystem.LanguageChanged += UpdateTextMeshPro;
        UpdateTextMeshPro();
    }

    private void OnDisable()
    {
        LocalizationSystem.LanguageChanged -= UpdateTextMeshPro;
    }

    public void UpdateTextMeshPro()
    {
        if (uiText == null)
            return;

        uiText.text = LocalizationSystem.GetText(key);
    }
}
