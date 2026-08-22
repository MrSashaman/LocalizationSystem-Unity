using UnityEngine;

public class SceneLocalizationLoader : MonoBehaviour
{
    [SerializeField] private TextAsset localizationFileEN;
    [SerializeField] private TextAsset localizationFileRU;

    private bool applied;

    private void Awake()
    {
        ApplySceneLocalizationFiles();
    }

    private void Start()
    {
        ApplySceneLocalizationFiles();
    }

    private void ApplySceneLocalizationFiles()
    {
        if (applied)
            return;

        if (LocalizationSystem.instance == null)
        {
            Debug.LogWarning(
                "SceneLocalizationLoader: LocalizationSystem instance not found!",
                this
            );
            return;
        }

        if (localizationFileEN == null || localizationFileRU == null)
        {
            Debug.LogWarning(
                "SceneLocalizationLoader: one or more localization files are not assigned.",
                this
            );
        }

        LocalizationSystem.instance.SetLocalizationFiles(
            localizationFileEN,
            localizationFileRU
        );

        applied = true;
    }
}
