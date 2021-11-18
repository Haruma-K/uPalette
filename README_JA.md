<h1 align="center">uPalette</h1>

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE.md)

[English Documents Available(英語ドキュメント)](README.md)

Unityにおけるプロジェクト内の色の一元管理・一括変更システム。

<p align="center">
  <img width=700 src="https://user-images.githubusercontent.com/47441314/118399146-af759380-b696-11eb-882c-9ee8adb2ee99.gif" alt="Demo">
</p>

## 特徴
* Unityプロジェクト内で使用している色を一元管理
* 色を変更するとプロジェクト全体に自動反映されるため、デザインの変更が容易に
* Adobe XDのアセットパネルのようなUX
* Prefab/Prefab Variantにも対応

## セットアップ

#### 要件
Unityのバージョンは2020.1以上に対応しています（Generic型のシリアライズを使っているため）。

#### インストール
1. Window > Package ManagerからPackage Managerを開く
2. 「+」ボタン > Add package from git URL
3. 以下を入力
    * https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/118421190-97842b00-b6fb-11eb-9f94-4dc94e82367a.png" alt="Package Manager">
</p>

あるいはPackages/manifest.jsonを開き、dependenciesブロックに以下を追記します。

```json
{
    "dependencies": {
        "com.harumak.upalette": "https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette"
    }
}
```

バージョンを指定したい場合には以下のように記述します。

* https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette#0.1.0

#### ライセンス
本ソフトウェアはMITライセンスで公開しています。  
ライセンスの範囲内で自由に使っていただいてかまいませんが、  
使用の際は以下の著作権表示とライセンス表示が必須となります。

[https://github.com/Haruma-K/uPalette/blob/master/LICENSE.md]

## 使い方

#### 色を作成する
uPaletteで管理する色は以下の手順で作成します。

1. Window > uPalette
2. uPaletteウィンドウ左上のCreateボタンを押下
3. 色と名前を自由に設定する
4. 削除は右クリックメニューから

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/118421129-6efc3100-b6fb-11eb-8431-c1f0548be99d.gif" alt="Create Colors">
</p>

設定データは`StreamingAssets/uPalette`以下に保存されます。  
プレイヤービルド時に必要なデータとなりますので削除しないようご注意ください。

#### 色を反映する
作成した色は以下の手順で反映します。

1. 反映したいGameObjectを選択
2. Applyボタンを押下
3. 色を反映するコンポーネント/プロパティ名を選択

<p align="center">
  <img width=700 src="https://user-images.githubusercontent.com/47441314/118421148-7b808980-b6fb-11eb-9b50-15c1dcfdc7cd.gif" alt="Apply Colors">
</p>

以上の手順で色とプロパティがリンクされます。  
リンクを解除するにはInspectorからColorSetterコンポーネントをデタッチします。

また、デフォルトで反映できるクラス/プロパティは以下の通りです。

|クラス名|プロパティ名|
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

これ以外のプロパティに色を反映する方法については「独自のコンポーネントに色を適用する」を参照してください。

なおPrefabのインスタンスに色を反映した場合には、通常のPrefabワークフローと同様、Prefabには反映されていない状態となります。  
Prefabに反映するには右クリックメニューなどからColor SetterをApplyしてください。

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/118421157-80ddd400-b6fb-11eb-8173-c8801da7c653.gif" alt="Apply to Prefab">
</p>

#### 色を適用しているGameObjectをハイライトする
色を右クリック > Highlightを選択すると、その色が適用されているGameObjectがハイライト（選択）されます。

<p align="center">
  <img width=700 src="https://user-images.githubusercontent.com/47441314/118421169-86d3b500-b6fb-11eb-86ce-1946a6f1a8d5.gif" alt="Highlight">
</p>

#### 独自のコンポーネントに色を反映する
ColorSetterクラスを継承したクラスを作成すると、独自のコンポーネントに色を反映できます。

```cs
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("")]
[DisallowMultipleComponent]
[RequireComponent(typeof(Outline))]
[ColorSetter(typeof(Outline), "Color")] // ColorSetterアトリビュートを付ける
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

    // Applyメソッドをオーバーライドして色を適用する
    protected override void Apply(Color color)
    {
        _outline.effectColor = color;
    }
}
```

##### スクリプトから色を取得する
登録済みの色をスクリプトから取得するには以下のように `UPaletteUtility` を使用します。

```cs
var keyColor = UPaletteUtility.GetColor("KeyColor");
```

一度取得した色はキャッシュされます。  
キャッシュを使用しない場合には上記のメソッドの第二引数に `false` を与えます。

またキャッシュは以下のようにして削除できます。

```cs
UPaletteUtility.ClearColorCache();
```

##### スクリプトから色を操作する
色をスクリプトから操作するには以下のようにします。

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
            var store = app.UPaletteStore; // アプリケーションの状態

            // エントリを取得して変更を加える
            var entry = store.Entries.First();
            entry.Name.Value = "Example";
            entry.SetColor(Color.red);

            // 変更を加えたらStoreにDirtyフラグを立てる（自動的に保存される）
            store.IsDirty.Value = true;
        }
        finally
        {
            UPaletteEditorApplication.ReleaseInstance();
        }
    }
}
```

## 技術的詳細

#### 変更した色を反映するタイミングについて
Unityでは、各コンポーネントに設定されている色はそのまま値としてシリアライズされます。  
したがって、uPaletteで色を変更したときにはこのシリアライズされた値を書き換えるべきです。

しかしこれでは、色を変更するだけで多くのSceneやPrefabに変更が加わってしまいます。  
そこでuPaletteでは以下のルールに従って色を反映しています。

* uPaletteの色は値ではなくIDとしてシリアライズ
* Edit ModeではOnEnable時にこの色を反映・色の変更を監視する
* Play ModeではStart()のタイミングで色を反映する

また、Edit ModeでSceneを開いたときに変更が加わらないよう、  
シリアライズされたIDの色を反映するときにはDirtyフラグを立てない実装にしています。

## デモ
デモシーンは以下の手順で再生できます。

1. リポジトリをクローンする
2. 以下のシーンを開いて再生
    * https://github.com/Haruma-K/uPalette/blob/master/Assets/Demo/Demo.unity
