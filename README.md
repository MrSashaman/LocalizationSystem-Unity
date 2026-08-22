# Unity Localization System

A lightweight localization system for Unity with runtime language switching and support for separate localization files for different scenes.

The system was originally created for use in a real Unity game project and later separated into a standalone reusable package.

## Features

* Runtime language switching
* English and Russian localization
* TextMeshPro support
* Standard Unity UI text support
* Scene-specific localization files
* Simple text-based localization format
* Easy integration into existing projects
* No external dependencies except TextMeshPro

## Included Scripts

### `LocalizationSystem.cs`

Main localization manager.

Responsible for:

* loading localization files;
* storing localization keys and translated values;
* switching the current language;
* returning localized strings;
* changing localization files at runtime.

### `LocalizedText.cs`

Updates standard Unity UI text using a localization key.

### `LocalizedTextMeshPro.cs`

Localization component for TextMeshPro UI elements.

### `LanguageButtonHandler.cs`

Allows UI buttons to switch the current language.

### `SceneLocalizationLoader.cs`

Allows each scene to use its own localization files.

Example:

```csharp
using UnityEngine;

public class SceneLocalizationLoader : MonoBehaviour
{
    [SerializeField] private TextAsset localizationFileEN;
    [SerializeField] private TextAsset localizationFileRU;

    private void Start()
    {
        LocalizationSystem.instance.SetLocalizationFiles(
            localizationFileEN,
            localizationFileRU
        );
    }
}
```

This makes it possible to keep menu localization and gameplay localization in separate files.

## Installation

1. Copy the localization scripts into your Unity project.
2. Create a `LocalizationSystem` object in your scene.
3. Attach the `LocalizationSystem.cs` component.
4. Assign the English and Russian localization files.
5. Add `LocalizedText` or `LocalizedTextMeshPro` to UI objects that should be translated.
6. Set the appropriate localization key.
7. Use `LanguageButtonHandler` or your own code to switch languages.

## Localization Files

Translations are stored inside Unity `TextAsset` files.

Example structure:

```text
PLAY_BUTTON=Play
SETTINGS_BUTTON=Settings
EXIT_BUTTON=Exit
```

Russian version:

```text
PLAY_BUTTON=Играть
SETTINGS_BUTTON=Настройки
EXIT_BUTTON=Выйти
```

Both files should contain the same keys.

## Scene-Specific Localization

Different scenes can use different localization files.

For example:

```text
MainMenu
├── localization_en_menu.txt
└── localization_ru_menu.txt

Game
├── localization_en_game.txt
└── localization_ru_game.txt
```

Add `SceneLocalizationLoader` to the scene and assign the required files.

The localization system will automatically load them when the scene starts.

## Example Usage

Get a localized string directly:

```csharp
string text = LocalizationSystem.instance.GetLocalizedValue("PLAY_BUTTON");
```

Switch localization files:

```csharp
LocalizationSystem.instance.SetLocalizationFiles(
    englishFile,
    russianFile
);
```

## Requirements

* Unity
* TextMeshPro

The system was developed for a modern Unity project, but it should be easy to adapt to other Unity versions.

## Planned Improvements

Possible future improvements include:

* additional languages;
* fallback language support;
* improved localization file parsing;
* editor tools;
* automatic UI refresh;
* JSON localization support.

## License

This project is licensed under the MIT License.

See the `LICENSE` file for details.
