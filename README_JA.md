<h1 align="center">uPalette</h1>

[![license](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE.md)
[![license](https://img.shields.io/badge/PR-welcome-green.svg)](https://github.com/pulls)
[![license](https://img.shields.io/badge/Unity-2020.1-green.svg)](#要件)

**ドキュメント** ([English](README.md), [日本語](README_JA.md))
| [デモ](Assets/Demo/Demo.unity)

Unityプロジェクトにおける色や文字スタイルを一元的に管理するためのシステムです。

<p align="center">
  <img width=800 src="https://user-images.githubusercontent.com/47441314/159275911-0445d1da-690b-4b56-86e8-85d57d79f257.gif" alt="Demo">
</p>

## 目次

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<!-- param::title::詳細:: -->
<details>
<summary>詳細</summary>

- [コンセプトと特徴](#%E3%82%B3%E3%83%B3%E3%82%BB%E3%83%97%E3%83%88%E3%81%A8%E7%89%B9%E5%BE%B4)
- [セットアップ](#%E3%82%BB%E3%83%83%E3%83%88%E3%82%A2%E3%83%83%E3%83%97)
  - [要件](#%E8%A6%81%E4%BB%B6)
  - [インストール](#%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)
- [基本的な使い方](#%E5%9F%BA%E6%9C%AC%E7%9A%84%E3%81%AA%E4%BD%BF%E3%81%84%E6%96%B9)
  - [Palette Storeを作成する](#palette-store%E3%82%92%E4%BD%9C%E6%88%90%E3%81%99%E3%82%8B)
  - [エントリを作成する](#%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%82%92%E4%BD%9C%E6%88%90%E3%81%99%E3%82%8B)
  - [エントリを適用する](#%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%82%92%E9%81%A9%E7%94%A8%E3%81%99%E3%82%8B)
  - [同期中のGameObjectをハイライトする](#%E5%90%8C%E6%9C%9F%E4%B8%AD%E3%81%AEgameobject%E3%82%92%E3%83%8F%E3%82%A4%E3%83%A9%E3%82%A4%E3%83%88%E3%81%99%E3%82%8B)
  - [色以外のエントリを取り扱う](#%E8%89%B2%E4%BB%A5%E5%A4%96%E3%81%AE%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%82%92%E5%8F%96%E3%82%8A%E6%89%B1%E3%81%86)
- [テーマ機能の使い方](#%E3%83%86%E3%83%BC%E3%83%9E%E6%A9%9F%E8%83%BD%E3%81%AE%E4%BD%BF%E3%81%84%E6%96%B9)
  - [テーマとは？](#%E3%83%86%E3%83%BC%E3%83%9E%E3%81%A8%E3%81%AF)
  - [テーマを作成する](#%E3%83%86%E3%83%BC%E3%83%9E%E3%82%92%E4%BD%9C%E6%88%90%E3%81%99%E3%82%8B)
  - [テーマを切り替える（エディタ）](#%E3%83%86%E3%83%BC%E3%83%9E%E3%82%92%E5%88%87%E3%82%8A%E6%9B%BF%E3%81%88%E3%82%8B%E3%82%A8%E3%83%87%E3%82%A3%E3%82%BF)
  - [テーマを切り替える（スクリプト）](#%E3%83%86%E3%83%BC%E3%83%9E%E3%82%92%E5%88%87%E3%82%8A%E6%9B%BF%E3%81%88%E3%82%8B%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%97%E3%83%88)
- [応用的な使い方](#%E5%BF%9C%E7%94%A8%E7%9A%84%E3%81%AA%E4%BD%BF%E3%81%84%E6%96%B9)
  - [エントリの値の変更を通知するSynchronizeEvent](#%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%81%AE%E5%80%A4%E3%81%AE%E5%A4%89%E6%9B%B4%E3%82%92%E9%80%9A%E7%9F%A5%E3%81%99%E3%82%8Bsynchronizeevent)
  - [エントリやテーマを表すEnumを自動生成する](#%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%82%84%E3%83%86%E3%83%BC%E3%83%9E%E3%82%92%E8%A1%A8%E3%81%99enum%E3%82%92%E8%87%AA%E5%8B%95%E7%94%9F%E6%88%90%E3%81%99%E3%82%8B)
  - [uPaletteのデータをスクリプトから編集する](#upalette%E3%81%AE%E3%83%87%E3%83%BC%E3%82%BF%E3%82%92%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%97%E3%83%88%E3%81%8B%E3%82%89%E7%B7%A8%E9%9B%86%E3%81%99%E3%82%8B)
  - [独自のコンポーネントに値を反映する](#%E7%8B%AC%E8%87%AA%E3%81%AE%E3%82%B3%E3%83%B3%E3%83%9D%E3%83%BC%E3%83%8D%E3%83%B3%E3%83%88%E3%81%AB%E5%80%A4%E3%82%92%E5%8F%8D%E6%98%A0%E3%81%99%E3%82%8B)
  - [エントリが見つからなかった時の挙動を設定する](#%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%81%8C%E8%A6%8B%E3%81%A4%E3%81%8B%E3%82%89%E3%81%AA%E3%81%8B%E3%81%A3%E3%81%9F%E6%99%82%E3%81%AE%E6%8C%99%E5%8B%95%E3%82%92%E8%A8%AD%E5%AE%9A%E3%81%99%E3%82%8B)
- [実装されているSynchronizer一覧](#%E5%AE%9F%E8%A3%85%E3%81%95%E3%82%8C%E3%81%A6%E3%81%84%E3%82%8Bsynchronizer%E4%B8%80%E8%A6%A7)
- [技術的詳細](#%E6%8A%80%E8%A1%93%E7%9A%84%E8%A9%B3%E7%B4%B0)
  - [エントリを反映するタイミングについて](#%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%82%92%E5%8F%8D%E6%98%A0%E3%81%99%E3%82%8B%E3%82%BF%E3%82%A4%E3%83%9F%E3%83%B3%E3%82%B0%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6)
- [バージョン1からのアップデート方法](#%E3%83%90%E3%83%BC%E3%82%B8%E3%83%A7%E3%83%B31%E3%81%8B%E3%82%89%E3%81%AE%E3%82%A2%E3%83%83%E3%83%97%E3%83%87%E3%83%BC%E3%83%88%E6%96%B9%E6%B3%95)
- [デモ](#%E3%83%87%E3%83%A2)
- [ライセンス](#%E3%83%A9%E3%82%A4%E3%82%BB%E3%83%B3%E3%82%B9)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## コンセプトと特徴
アプリケーション開発では一般的に、一つの色を複数の箇所に適用します。  
以下はボタンの背景色やアイコンの色、アウトラインに同じ青色を適用している例です。

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/159170066-1bd16348-b013-4f47-8d64-988f43f2fde7.png" alt="Apply blue color">
</p>

次に、この色を青色から緑色に変更することを考えます。  
Unityでは色の値はPrefabやSceneにシリアライズされるので、これらすべての値を一つ一つ変更する必要があります。

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/158061951-ff91aaee-019a-4ea4-9c74-012a93f0558f.png" alt="Change the color to green">
</p>

当然ながら、この作業量はプロジェクトの規模に応じて増加します。  
uPaletteを使えば色を一元管理することでこのような変更を一括で適用することができます。

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/158061961-153d13ba-a4ee-45ee-b513-9d7956f21fa4.png" alt="uPalette">
</p>

またuPaletteでは、色だけではなく文字スタイルやグラデーションを管理することもできます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158065275-5b9e5801-88e8-4667-8cbe-67bc428d4b1e.png" alt="Character Styles & Gradients">
</p>

さらにテーマ機能を使えば、色や文字スタイルのセットをテーマとして保存できます。  
アクティブなテーマを切り替えることでそのテーマに応じた色や文字スタイルが反映されます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158065218-21a3f422-ad00-4da9-ab61-455408c2f7d1.gif" alt="Theme Feature">
</p>

## セットアップ

### 要件
Unity2020.1 以上

### インストール
インストールは以下の手順で行います。

1. Window > Package Manager を選択
2. 「+」ボタン > Add package from git URL を選択
3. 以下のURLを入力してインストール
    - https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/118421190-97842b00-b6fb-11eb-9f94-4dc94e82367a.png" alt="Install">
</p>

バージョンを指定したい場合には以下のようにURLの末尾にバージョンを付与します。

* https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette#2.0.0

バージョンの更新もインストールと同様の手順で実行できます。

`No 'git' executable was found. Please install Git on your system and restart Unity` のようなメッセージが出た場合、マシンにGitをセットアップする必要がある点にご注意ください。

## 基本的な使い方

### Palette Storeを作成する
uPaletteを使うにはまず`Window > uPalette > Palette Editor`からPalette Editorを開きます。  
Palette Editorを開くと下図のようなウィンドウが表示されます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157675097-e260f475-5ba0-42af-adfc-06d8155103d8.png" alt="Palette Editor">
</p>

次に、中央の`Create Palette Store`ボタンを押下することでPalette Storeアセットを作成します。  
Palette StoreはuPaletteで扱うデータを保持するためのアセットです。  
プロジェクト内の任意の場所に配置できますが、ランタイムで使うアセットなのでEditorフォルダやStreamingAssetsフォルダ配下には置かないよう注意してください。

Palette Storeアセットを作成するとPalette Editorは以下のような表示に切り替わります。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157675124-bf3471c4-5f0f-4a07-ae10-8a97b3d986ad.png" alt="Palette Editor">
</p>

### エントリを作成する
uPaletteでは、色や文字スタイルの設定のことをエントリと呼びます。  
Palette Editorの右上にある「+」ボタンを押下することで、エントリを追加することができます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157674758-981455be-7770-4a54-af49-71f69dd01276.gif" alt="Add Entry">
</p>

エントリ名をクリックすることでリネームすることができます。  
また、エントリの削除は右クリックから行えます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157676311-1b7d12fc-a410-4303-a38a-fbe2f3192265.gif" alt="Rename & Remove Entry">
</p>

要素をドラッグすると順番を並び替えることもできます。

### エントリを適用する
作成した色や文字スタイルをコンポーネントに反映するには、対象のGameObjectを選択した状態で対象のエントリのApplyボタンを押下します。  
すると適用可能なコンポーネントとプロパティの名前がリストアップされるので、適用したいものを選択します。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157679154-0e1aa71a-27f4-49c4-9c28-9eca8080f96d.gif" alt="Apply Entry">
</p>

これで、エントリとプロパティが同期されます。  
同期されているエントリの値が変化するとプロパティが自動的に書き変わります。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157680482-2df5fe4c-3756-4422-89fb-208a89b1f657.gif" alt="Change Entry Value">
</p>

この時、対象のGameObjectにはSynchronizerと呼ばれるコンポーネントがアタッチされています。  
このコンポーネントのInspectorからエントリを切り替えることもできます。  
またこのコンポーネントをデタッチすると、エントリとの同期が解除されます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/162608969-56152f04-00f1-4b86-8d07-08537bd15c34.png" alt="Synchroizer">
</p>

なお、Prefabに対してエントリを適用した場合には、通常のPrefabワークフローと同様、Prefabにはシリアライズされていない状態となります。  
シリアライズを行うには右クリックメニューなどからApplyしてください。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/162609447-23ed99fb-2173-4717-84b6-79951ed70d88.gif" alt="Serialization">
</p>

### 同期中のGameObjectをハイライトする
エントリの右クリックメニューからHighlightを選択すると、同期中のGameObjectをハイライト（選択）できます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157684607-0b28a34a-c892-4458-9b0d-a3cdf8ea10e5.gif" alt="Highlight">
</p>

### 色以外のエントリを取り扱う
ここまで、uPaletteで色を管理する方法について説明しました。  

uPaletteには色の他にも文字スタイルやグラデーションといったパレットの種類が存在します。  
PaletteEditorの左上のドロップダウンメニューから、パレットの種類を切り替えることができます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157685702-e2d83f7c-4cfa-4b37-9561-0067f5c828c0.gif" alt="Various Palettes">
</p>

各ドロップダウンメニューの説明は以下の通りです。

| 名前 | 説明 |
| --- | --- |
| Color | 色を管理するために使用します。 |
| Gradient | グラデーションを管理するために使用します。 |
| Character Style | uGUI Textの文字スタイルを管理するために使用します。 |
| Character Style TMP | Text Mesh Proの文字スタイルを管理するために使用します。 |

## テーマ機能の使い方

### テーマとは？
テーマ機能を使うと、エントリのセットを「テーマ」として保存できます。  
テーマは複数保存でき、それを切り替えることでテーマに応じた色や文字スタイルを反映することができます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157786384-dc33d7a0-eec3-4413-9639-2b61b8c9f1b5.gif" alt="Theme">
</p>

### テーマを作成する
テーマを作成するには、`Window > uPalette > Theme Editor`からTheme Editorを開きます。  
デフォルトでは、Defaultという名前のテーマが存在しており、左上の「+」ボタンを押下することで新しいテーマを作成できます。  
Entry Editorと同様の操作でリネーム、削除、並び替えなどができます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157786982-b19be4af-ffd4-407e-a8dc-3ba39c9426f4.gif" alt="Theme Editor">
</p>

テーマを追加すると、Palette Editorにそのテーマのエントリを設定するためのカラムが追加されます。  
これを編集することでそのテーマに応じた値を設定できます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/159245296-ad887c65-27f9-4274-a641-811833683130.png" alt="Palette Editor">
</p>

なおテーマはパレットの種類ごとに設定できます。  
パレットの種類はTheme Editor左上のドロップダウンから変更できます。

<p align="center">
  <img width=400 src="https://user-images.githubusercontent.com/47441314/157789707-b2103a3a-cf9b-4e55-a7ac-157604608cb9.gif" alt="Change Palette Type">
</p>

### テーマを切り替える（エディタ）
Theme EditorからActivateボタンを押下することでテーマを切り替えることができます。  
テーマを切り替えると、そのテーマのエントリの値が即座に反映されます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/157788787-e1cf2500-7b20-4a60-86cf-421613089517.gif" alt="Change Theme">
</p>

### テーマを切り替える（スクリプト）
ランタイムにおけるテーマの切り替えには`Palette`クラスの`SetActiveTheme()`を使用します。  
以下は[自動生成したテーマのEnum](#エントリやテーマを表すEnumを自動生成する)を使用して、`ColorPalette`のテーマを切り替えるスクリプトの例です。

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

これを適当なGameObjectにアタッチして再生すると、以下のようにテーマを切り替えることができます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158050236-389b9798-e9e9-46fe-bb15-c862d263bff4.gif" alt="Change Theme">
</p>

## 応用的な使い方

### エントリの値の変更を通知するSynchronizeEvent
上述の通り、Synchronizerコンポーネントは指定したエントリの値が変更されたときに対象のプロパティにその値を反映します。  
これに対し、以下のSynchronize Eventコンポーネントを使用すると、値の変更通知だけをイベントとして受け取ることができます。

* Color Synchronize Event
* Gradient Synchronize Event
* Character Style Synchronize Event
* Character Style TMP Synchronize Event

使用するには上記のコンポーネントをアタッチし、値が変わったときの処理をUnityEventに設定します。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/162609856-a64ab4de-9f44-4c92-9762-cd262f3ceeb9.png" alt="Change Theme">
</p>

### エントリやテーマを表すEnumを自動生成する
スクリプトからuPaletteを操作する場合、テーマやエントリの情報にアクセスするためのスクリプトを自動生成しておくと便利です。  
`Project Settings > uPalette > Name Enums File Generation`を`When Window Loses Focus`に設定すると、Palette EditorやTheme Editorからフォーカスが外れた際にこのファイルが自動生成されます。  
`Name Enums File Location`にフォルダを指定するとそのフォルダに生成されます。未指定の場合にはAssetsフォルダ直下に生成されます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158021815-2cec00b7-46f1-403b-b459-e03c8754b29d.png" alt="Project Settings">
</p>

以下のようなEnumが生成されます。

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

またこのEnumの拡張メソッドとして定義されている`ToEntryId()`を使用すると、当該エントリのIDを取得することができます。

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

他の種類のエントリやテーマについても同様にして使用できます。

### uPaletteのデータをスクリプトから編集する
以下のようにPaletteStoreから各パレットを取得することで、uPaletteのデータをスクリプトから編集することができます。  
PaletteStoreは`ScriptableObject`なので、編集した後には必ずDirtyフラグを立ててUnityに編集したことを知らせる必要がある点にご注意ください。

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

### 独自のコンポーネントに値を反映する
uPaletteには[標準的なコンポーネントのプロパティに値を反映するためのSynchronizerがあらかじめ用意](#実装されているSynchronizer一覧)されています。

これとは別に、独自のコンポーネントに値を反映するためのSynchronizerを作成することもできます。  
例として、グラデーションをプロパティとして持つ独自のコンポーネントを考えます。

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

このプロパティに値を反映するためのSynchronizerは以下のように作成できます。

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

### エントリが見つからなかった時の挙動を設定する
対象のエントリが見つからなかった場合、エラーログを出したい場合もあれば、それを無視したい場合もあるでしょう。  
`Project Settings > uPalette > Missing Entry Error`から、エントリが見つからなかった時の挙動を設定できます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158050990-59e1fae4-d8ec-4ae8-8d15-4009016aee29.png" alt="Missing Entry Error">
</p>

選択肢は以下の通りです。

| 名前 | 説明 |
| --- | --- |
| None | 何もしない。 |
| Warning | 警告ログを出力する。 |
| Error | エラーログを出力する。 |
| Exception | 例外をスローする。 |

## 実装されているSynchronizer一覧
uPaletteに標準で実装されているSynchronizerは以下の通りです。

| エントリの種類 | 対象クラス名 | 対象プロパティ名 |
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

## 技術的詳細

### エントリを反映するタイミングについて

Unityでは、各コンポーネントに設定されている色やテキストスタイルなどの情報はそのまま値としてシリアライズされます。  
したがって、これら変更したときにはこのシリアライズされた値を書き換えるべきです。

しかしこれでは、エントリを変更した際に多くのSceneやPrefabに変更が加わってしまいます。  
そこでuPaletteでは以下のルールに従って色を反映しています。

- uPaletteのエントリは値ではなくIDとしてシリアライズ
- Edit ModeではOnEnable時にこのエントリを反映・変更を監視する
- Play ModeではStart()のタイミングでエントリを反映する

また、Edit ModeでSceneを開いたときに変更が加わらないよう、シリアライズされたIDのエントリを反映するときにはDirtyフラグを立てない実装にしています。

## バージョン1からのアップデート方法

uPaletteをバージョン1からバージョン2にバージョンアップする上で、データ構造やデータの置き場所を大きく変更しました。

バージョン1を使用していた方は、Palette Storeを作成する前にProject Settingsから以下のボタンを押下することでバージョン2にデータを移行できます。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158051937-fe364df4-7105-4de0-83d4-a15e6c7a3517.png" alt="How to update">
</p>

ボタンを押下するとPalette Storeアセットの保存パネルが表示されるので、任意の場所に保存してください。  
ただしこのアセットはランタイムで使うアセットなので、EditorフォルダやStreamingAssetsフォルダ配下には置かないよう注意してください。

## デモ
デモシーンは以下の手順で再生できます。

1. リポジトリをクローンする
2. 以下のシーンを開いて再生
    - [https://github.com/Haruma-K/uPalette/blob/master/Assets/Demo/Demo.unity](Assets/Demo/Demo.unity)

## ライセンス
本ソフトウェアはMITライセンスで公開しています。ライセンスの範囲内で自由に使っていただけますが、使用の際は以下の著作権表示とライセンス表示が必須となります。

- [LICENSE](LICENSE.md)

また、本ドキュメントの目次は以下のソフトウェアを使用して作成されています。

- [toc-generator](https://github.com/technote-space/toc-generator)

toc-generatorのライセンスの詳細は [Third Party Notices.md](Third%20Party%20Notices.md) を参照してください。
