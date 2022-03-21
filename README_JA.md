# uPalette: 色や文字スタイルの一元管理システム

[![license](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE.md)
[![license](https://img.shields.io/badge/PR-welcome-green.svg)](https://github.com/CyberAgentGameEntertainment/NovaShader/pulls)
[![license](https://img.shields.io/badge/Unity-2020.3-green.svg)](#Requirements)

**ドキュメント** ([English](README.md), [日本語](README_JA.md))
| [デモ](Assets/Demo/Demo.unity)

Unityプロジェクトにおける色や文字スタイルを一元的に管理するためのシステムです。

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/47441314/158066417-1ecf9278-4204-45ff-802b-7793b0eadfd6.gif" alt="Demo">
</p>

## 目次

<!-- START doctoc -->
<!-- param::title::詳細:: -->

<!-- END doctoc -->

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

* https://github.com/Haruma-K/uPalette.git?path=/Assets/uPalette#1.0.0

バージョンの更新もインストールと同様の手順で実行できます。

{% note %}

`No 'git' executable was found. Please install Git on your system and restart Unity` のようなメッセージが出た場合、マシンにGitをセットアップする必要がある点にご注意ください。

{% endnote %}
