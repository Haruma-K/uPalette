<h1 align="center">uPalette</h1>

[![license](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE.md)
[![license](https://img.shields.io/badge/PR-welcome-green.svg)](https://github.com/pulls)
[![license](https://img.shields.io/badge/Unity-2020.1-green.svg)](#Requirements)

**Docs** ([English](README.md), [日本語](README_JA.md))
| [Demo](Assets/Demo/Demo.unity)

Centrally manage colors and text styles in Unity projects.

<p align="center">
  <img width=800 src="https://user-images.githubusercontent.com/47441314/158066417-1ecf9278-4204-45ff-802b-7793b0eadfd6.gif" alt="Demo">
</p>

## Table of Contents

<!-- START doctoc -->
<!-- END doctoc -->

## Concept & Features
In typical application development, one color is applied to multiple locations. The same blue color is applied to the button background, icon color, and outline in the following example.

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/159170066-1bd16348-b013-4f47-8d64-988f43f2fde7.png" alt="Apply blue color">
</p>

Now think about changing this color from blue to green. In Unity, color values are serialized in Prefabs and Scenes, so we need to change all these values one by one.

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/158061951-ff91aaee-019a-4ea4-9c74-012a93f0558f.png" alt="Change the color to green">
</p>

This workload increases with the size of the project. With uPalette, such changes can be applied in batches by centralizing color management.

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/158061961-153d13ba-a4ee-45ee-b513-9d7956f21fa4.png" alt="uPalette">
</p>

uPalette also allows you to manage text styles and gradients as well as colors.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158065275-5b9e5801-88e8-4667-8cbe-67bc428d4b1e.png" alt="Character Styles & Gradients">
</p>

In addition, the theme feature allows you to save a set of colors and text styles as a theme. By switching the active theme, you can reflect the colors and text style according to that theme.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158065218-21a3f422-ad00-4da9-ab61-455408c2f7d1.gif" alt="Theme Feature">
</p>

## Setup

### Requirements
Unity2020.1 or higher.

### Installation
インストールは以下の手順で行います。

1. Select `Window > Package Manager` in Unity Editor.
2. Select `“+” Button > Add package from git URL` .
3. Enter the following URL.
    - https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/118421190-97842b00-b6fb-11eb-9f94-4dc94e82367a.png" alt="Install">
</p>

If you want to install a specific version, add the version to the end of the URL as follows.

- https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette#1.0.0

You can also update the version in the same manner as installation.

Note that if you get a message like `No 'git' executable was found. Please install Git on your system and restart Unity`, you need to set up Git on your machine.

## Getting Started

### Create Palette Store
To use uPalette, first open the Palette Editor from `Window > uPalette > Palette Editor`. You will see a window like as follows when you open the Palette Editor.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157675097-e260f475-5ba0-42af-adfc-06d8155103d8.png" alt="Palette Editor">
</p>

Next, create a Palette Store asset by clicking the center `Create Palette Store` button. Palette Store is the asset that will hold the data for the uPalette. You can place this anywhere in the project, but not in the Editor folder because this asset is used at runtime.

After creating a Palette Store asset, the Palette Editor will change to the following display.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157675124-bf3471c4-5f0f-4a07-ae10-8a97b3d986ad.png" alt="Palette Editor">
</p>

### Create Entry
In uPalette, color and character style settings are called Entries. You can add an Entry by pressing the “+” button in the upper right corner of the Palette Editor.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157674758-981455be-7770-4a54-af49-71f69dd01276.gif" alt="Add Entry">
</p>

You can rename an Entry by clicking on its name. Entries can also be deleted by right-clicking menu of them.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157676311-1b7d12fc-a410-4303-a38a-fbe2f3192265.gif" alt="Rename & Remove Entry">
</p>

You can also drag elements to reorder them.

### Apply Entry
To apply the color and character style you created to components, select the target GameObject and press the Apply button for the target entry. The names of applicable components and properties will then be listed, and you can select the one you want to apply.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157679154-0e1aa71a-27f4-49c4-9c28-9eca8080f96d.gif" alt="Apply Entry">
</p>

This will synchronize the entries and properties. When the value of a synchronized Entry changed, the property is automatically rewritten.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157680482-2df5fe4c-3756-4422-89fb-208a89b1f657.gif" alt="Change Entry Value">
</p>

At this time, a component named Synchronizer is attached to the target GameObject. If you want to remove synchronization with the Entry, detach this component.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157680907-388bedd1-01e9-4a71-af7b-af09f2360bce.png" alt="Synchroizer">
</p>

Note that When the Entry is applied to Prefab, It is not serialized in Prefab as in the normal Prefab workflow. You can serialize by right-clicking menu of Prefab.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157682845-22d8ad29-a245-4f37-a817-9fdfa3bdbd48.gif" alt="Serialization">
</p>

### Highlight synchronized GameObjects
Select Highlight from the Entry’s right-clicking menu to highlight (select) the synchronized GameObjects.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157684607-0b28a34a-c892-4458-9b0d-a3cdf8ea10e5.gif" alt="Highlight">
</p>

### Handling non-color Entries.
Up to this point, I have described how to manage colors in uPalette. In addition to colors, there are other palette types in uPalette, such as gradients and character styles. You can switch between Palette types from the drop-down menu in the upper left corner of the Palette Editor.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157685702-e2d83f7c-4cfa-4b37-9561-0067f5c828c0.gif" alt="Various Palettes">
</p>

Each drop-down menu is described below.

| Name | Description |
| --- | --- |
| Color | Use to manage color. |
| Gradient | Use to manage gradient. |
| Character Style | Use to manage character style of uGUI Text. |
| Character Style TMP | Use to manage character style of Text Mesh Pro. |

## Theme Feature Usage

### What is Theme?
The Theme feature allows you to save a set of the Entries as “theme”. You can save multiple Themes, and switch between them to reflect the colors and character styles of each Theme.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157786384-dc33d7a0-eec3-4413-9639-2b61b8c9f1b5.gif" alt="Theme">
</p>

### Create Theme
To create a Theme, open the Theme Editor from `Window > uPalette > Theme Editor`. By default, there is a Theme named Default, and you can create a new Theme from the upper left “+” button. You can rename, delete, reorder, etc. using the same operations as in the Entry Editor.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157786982-b19be4af-ffd4-407e-a8dc-3ba39c9426f4.gif" alt="Theme Editor">
</p>

When a Theme is added, a column is added to the Palette Editor for setting Entries for that theme. By editing this column, you can set values according to the Theme.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/159245296-ad887c65-27f9-4274-a641-811833683130.png" alt="Palette Editor">
</p>

You can set the Theme for each Palette type. The palette type can be changed from the drop-down menu in the upper left corner of the Theme Editor.

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/157789707-b2103a3a-cf9b-4e55-a7ac-157604608cb9.gif" alt="Change Palette Type">
</p>

### Switch Themes (Editor)
You can switch Themes by pressing the Activate button from the Theme Editor.  When you switch themes, the values of the Entry in that Theme are immediately reflected.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157788787-e1cf2500-7b20-4a60-86cf-421613089517.gif" alt="Change Theme">
</p>

### Switch Themes (Script)
To switch Themes at runtime, use `SetActiveTheme()` in the `Palette` class. Below is an example script that switches the ColorPalette theme using [automatically generated theme enum](Automatic-generation-of-enums-for-Entries-and-Themes).

```csharp
using System;
using UnityEngine;
using uPalette.Generated;
using uPalette.Runtime.Core;

public class Example : MonoBehaviour
{
    public void OnGUI()
    {
        foreach (ColorTheme colorTheme in Enum.GetValues(typeof(ColorTheme)))
            if (GUILayout.Button(colorTheme.ToString()))
            {
                var colorPalette = PaletteStore.Instance.ColorPalette;
                colorPalette.SetActiveTheme(colorTheme.ToThemeId());
            }
    }
}
```

Attach this to GameObject and play to switch Themes as follows.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158050236-389b9798-e9e9-46fe-bb15-c862d263bff4.gif" alt="Change Theme">
</p>

## Advanced Usage

### Automatic generation of enums for Entries and Themes
When working with uPalette from script, it is useful to have a script automatically generated to access Theme and Entry information.

If you set `Project Settings > uPalette > Name Enums File Generation`to`When Window Loses Focus`, this file will be automatically generated when the focus is lost from the Palette Editor or Theme Editor. If you set a folder to `Name Enums File Location`, the file will be generated in that folder. If not set, the file will be generated in the Assets folder.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158021815-2cec00b7-46f1-403b-b459-e03c8754b29d.png" alt="Project Settings">
</p>

The following enum is generated.

```csharp
using System;

namespace uPalette.Generated
{
    public enum ColorEntry
    {
        Red,
        Green,
        Blue,
    }
}
```

`ToEntryId()`, defined as extension method of this enum, can be used to get the Entry ID.

```csharp
using uPalette.Generated;

public class Example
{
    private void Foo()
    {
        ColorEntry.Red.ToEntryId();
    }
}
```

The same can be used for other types of Entries and Themes.

### Edit uPalette data from scripts
You can edit uPalette data from scripts by retrieving a palette from the PaletteStore as shown below. Note that you must always set the dirty flag after editing the PaletteStore, because it is a ScriptableObject.

```csharp
// Get PaletteStore.
var paletteStore = PaletteStore.Instance;

// Get each palette.
var colorPalette = PaletteStore.Instance.ColorPalette;
var gradientPalette = PaletteStore.Instance.GradientPalette;
var characterStylePalette = PaletteStore.Instance.CharacterStylePalette;
var characterStyleTMPPalette = PaletteStore.Instance.CharacterStyleTMPPalette;

// Set the dirty flag after editing.
EditorUtility.SetDirty(paletteStore);

// Save assets if you need.
AssetDatabase.SaveAssets();
```

### Reflects values to your own components
uPalette includes [Synchronizers to reflect values to standard component](#Implemented-Synchronizers).

You can also create a Synchronizer to reflect values in your own components. As an example, consider a custom component with Gradient as a property.

```csharp
using UnityEngine;

public class SampleGradient : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;

    public Gradient Gradient
    {
        get => _gradient;
        set => _gradient = value;
    }
}
```

A Synchronizer to reflect the value to this property can be created as follows.

```csharp
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Gradient;

[AddComponentMenu("")]
[DisallowMultipleComponent]
[RequireComponent(typeof(SampleGradient))]
[GradientSynchronizer(typeof(SampleGradient), "Gradient")]
public sealed class GraphicColorSynchronizer : GradientSynchronizer<SampleGradient>
{
    protected override Gradient GetValue()
    {
        return _component.Gradient;
    }

    protected override void SetValue(Gradient value)
    {
        _component.Gradient = value;
    }

    protected override bool EqualsToCurrentValue(Gradient value)
    {
        return _component.Gradient.Equals(value);
    }
}
```

### Configure behavior when an entry is not found

When a target Entry is not found, you may want to output error logs, or you may want to ignore it. You can configure the behavior when an Entry is not found by `Project Settings > uPalette > Missing Entry Error`.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158050990-59e1fae4-d8ec-4ae8-8d15-4009016aee29.png" alt="Missing Entry Error">
</p>

The selections are as follows.

| Name | Description |
| --- | --- |
| None | Do nothing. |
| Warning | Output warning logs. |
| Error | Output error logs. |
| Exception | Throw exceptions. |

## Implemented Synchronizers
The Synchronizer implemented in uPalette is as follows.

| Entry Type | Target Class Name | Target Property Name |
| --- | --- | --- |
| Color | UnityEngine.UI.Graphic | color |
| Color | UnityEngine.UI.Outline | effectColor |
| Color | UnityEngine.UI.Selectable | colors.normalColor |
| Color | UnityEngine.UI.Selectable | colors.selectedColor |
| Color | UnityEngine.UI.Selectable | colors.pressedColor |
| Color | UnityEngine.UI.Selectable | colors.disabledColor |
| Color | UnityEngine.UI.Selectable | colors.highlightedColor |
| Color | UnityEngine.UI.InputField | caretColor |
| Color | UnityEngine.UI.InputField | selectionColor |
| CharacterStyle | UnityEngine.UI.Text | font / fontStyle / fontSize / lineSpacing |
| CharacterStyleTMP | TMPro.TextMeshProUGUI | font / fontStyle / fontSize / enableAutoSizing / characterSpacing / wordSpacing / lineSpacing / paragraphSpacing |

## Technical details

## About the timing of reflecting Entries

In Unity, information such as colors and character styles set for each component are serialized as values as they are. Therefore, these serialized values should be rewritten when they are changed.

However, this would result in changes to many Scenes and Prefabs when an Entry is changed. So uPalette reflects the Entry according to the following rules.

- Entries are serialized as IDs, not values.
- In Edit Mode, this Entry is reflected and observed for changes when `OnEnable` is called.
- In Play Mode, Entries are reflected when `Start()` is called.

In addition, to prevent changes when a Scene is opened in Edit Mode, the dirty flags is not set when an Entry are reflected.

## How to update from version 1

In upgrading uPalette from version 1 to version 2, I have made significant changes to the data structure and data placement.

If you have been using version 1, you can transfer your data to version 2 by pressing the following button from Project Settings before creating Palette Store.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158051937-fe364df4-7105-4de0-83d4-a15e6c7a3517.png" alt="How to update">
</p>

## Demo
1. Clone this repository.
2. Open and play the following scene.
    - [https://github.com/Haruma-K/uPalette/blob/master/Assets/Demo/Demo.unity](https://github.com/Haruma-K/uPalette/blob/master/Assets/Demo/Demo.unity)

## Licenses
This software is released under the MIT license. You are free to use it within the scope of the license, but the following copyright and license notices are required.

- [LICENSE.md](LICENSE.md)

In addition, the table of contents for this document has been created using the following software

- [toc-generator](https://github.com/technote-space/toc-generator)

See [Third Party Notices.md](Third%20Party%20Notices.md) for more information about the license of toc-generator.
