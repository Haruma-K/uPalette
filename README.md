<h1 align="center">uPalette</h1>

Centralized management & batch change system of colors in a project.

<p align="center">
  <img width=700 src="https://user-images.githubusercontent.com/47441314/118399146-af759380-b696-11eb-882c-9ee8adb2ee99.gif" alt="Demo">
</p>

## Features
* You can centrally manage the colors used in your Unity project.  
* When you change a color, it is automatically reflected in the entire project, making it easy to change the design.
* Prefab / Prefab Variant support.
* UX like the assets panel of Adobe XD.

## Setup

#### Requirement
Unity 2020.1 or higher (because of generic type serialization).

#### Install
1. Open the Package Manager from Window > Package Manager
2. "+" button > Add package from git URL
3. Enter the following
   * https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/118421190-97842b00-b6fb-11eb-9f94-4dc94e82367a.png" alt="Package Manager">
</p>


Or, open Packages/manifest.json and add the following to the dependencies block.

```json
{
    "dependencies": {
        "com.harumak.upalette": "https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette"
    }
}
```

If you want to set the target version, specify it like follow.

* https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette#0.1.0

#### License
This software is released under the MIT License.  
You are free to use it within the scope of the license.  
However, the following copyright and license notices are required for use.

* https://github.com/Haruma-K/uPalette/blob/master/LICENSE.md

## Usage

#### Create Colors
You can create colors to be managed by uPalette by following the steps below.

1. Window > uPalette.
2. Click "Create" button on the upper left of the window.
3. Set the color and name as desired.
4. Open the right-click menu if you want to remove.

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/118421129-6efc3100-b6fb-11eb-8431-c1f0548be99d.gif" alt="Create Colors">
</p>

The configuration data will be stored in `StreamingAssets/uPalette`.  
This is required when building the player, so be careful not to delete it.

#### Apply Colors
Next, apply the colors by following the steps below.

1. Select the GameObject you want to apply it to.
2. Click "Apply" button.
3. Select a component/property name you want to apply it to.

<p align="center">
  <img width=700 src="https://user-images.githubusercontent.com/47441314/118421148-7b808980-b6fb-11eb-9b50-15c1dcfdc7cd.gif" alt="Apply Colors">
</p>

Now the color will be linked to the property.  
If you want to break the link, detach the ColorSetter component from the Inspector.

The classes/properties that can be applied by default are as follows.

|Class Name|Property Name|
|-|-|
|Graphic|color|
|Outline|effectColor|
|Selectable|colors.normalColor|
|Selectable|colors.selectedColor|
|Selectable|colors.pressedColor|
|Selectable|colors.disabledColor|
|Selectable|colors.highlightedColor|
|InputField|caretColor|
|InputField|selectionColor|

To see how to apply color to other properties, see "Apply color to your own components".

And when you apply a color to prefab instance, it is not applied in the prefab as in normal prefab workflow.  
To apply the color to the prefab, use the right-click menu to apply the ColorSetter.

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/118421157-80ddd400-b6fb-11eb-8173-c8801da7c653.gif" alt="Apply to Prefab">
</p>

#### Highlight applied GameObjects
You can highlight (select) the GameObject to which the color is applied by right-clicking on the color and selecting "Highlight".

<p align="center">
  <img width=700 src="https://user-images.githubusercontent.com/47441314/118421169-86d3b500-b6fb-11eb-86ce-1946a6f1a8d5.gif" alt="Highlight">
</p>

#### Apply color to your own components
You can apply colors to your own components by creating a class that inherits from the `ColorSetter` class.

```cs
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("")]
[DisallowMultipleComponent]
[RequireComponent(typeof(Outline))]
[ColorSetter(typeof(Outline), "Color")] // Add ColorSetter attribute.
public class OutlineColorSetter : ColorSetter
{
    [SerializeField] [HideInInspector] private Outline _outline;

    private void Awake()
    {
        if (Application.isEditor)
        {
            _outline = GetComponent<Outline>();
        }
    }

    // Override Apply method
    protected override void Apply(Color color)
    {
        _outline.effectColor = color;
    }
}
```

##### Edit via script
To edit the color via script, do as follows.

```cs
using System.Linq;
using UnityEngine;
using uPalette.Editor.Core;
using uPalette.Runtime.Core;

public class Example
{
    private void Main()
    {
        var app = UPaletteApplication.RequestInstance();

        try
        {
            var store = app.UPaletteStore; // The state of the application

            // Get and edit a entry
            var entry = store.Entries.First();
            entry.Name.Value = "Example";
            entry.SetColor(Color.red);

            // You need to set dirty after editing (Save automatically).
            store.IsDirty.Value = true;
        }
        finally
        {
            UPaletteEditorApplication.ReleaseInstance();
        }
    }
}
```

## Technical details

#### About applying colors
Unity serializes the color as it is as a value.  
Therefore, uPalette also should rewrite this serialized value when the color is changed.

However, this means that every time you change the color, many Scenes and Prefabs will be changed.  
Therefore, uPalette applies the color according to the following rules.

* uPalette Colors are serialized as IDs, not values.
* In Edit Mode, reflect this color and start observing for changes when `OnEnable`.
* In Play Mode, reflect the color at the timing of `Start`. 

In addition, uPalette does not set the dirty flag when reflecting colors so that changes are not applied when the Scene is opened in Edit Mode.

## Demo
1. Clone this repository.
2. Open and play the following scene.
    * https://github.com/Haruma-K/uPalette/blob/master/Assets/Demo/Demo.unity

