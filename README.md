# edanouenohato

枝の上の鳩のプロジェクトです.  
Issues: [🚀 EH Beta - EH Issues](https://scrapbox.io/eh-issues/%F0%9F%9A%80_EH_Beta)

## Installation

### Install Unity Editor

`Unity 2023.2` を使用しています, 最新版をインストールしてください.

![](https://i.gyazo.com/adcd4196aed56184686272682baa4496.png)

`Android Build Support` modules が必要です.  
`Rider` を使用するため, Visual Studio は必要ありません.  
また, ローカル環境で自分でビルドを作成する場合は現在 `android sdk 32` が必要です (@minami まで)

### Clone project

```console
git clone --recursive https://github.com/edanouenohato/edanouenohato.git
```

[Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules) でパッケージを管理しているので, `--recursive`
オプションを付けて clone
してください.

> **Note**  
> `--recursive` オプションをつけ忘れた場合は, Clone 後に Repository のディレクトリに移動して以下のコマンドを実行してください.
> ```console
> cd edanouenohato
> git submodule update --init --recursive
> ```

## For Developers

### IDE

[JetBrains Rider](https://www.jetbrains.com/rider/) を使用してください.
ライセンスを所有していない方は [@minami110](https://github.com/minami110) まで.    
プロジェクトを初めて clone した際は [Rider 最初の設定](https://scrapbox.io/edanoue/Rider_最初の設定) を参考に初期設定を行ってください.

### VR Headset

[Meta Quest 2](https://www.meta.com/jp/quest/products/quest-2/) を使用してください.
所有していない方は [@minami110](https://github.com/minami110) まで.

### Setup Houdini Engine for Unity (任意)

[HEU & HDA を利用した GrayBoxing - ⚗️EH Proto Gamedesign](https://scrapbox.io/eh-proto-gamedesign/HEU_&_HDA_%E3%82%92%E5%88%A9%E7%94%A8%E3%81%97%E3%81%9F_GrayBoxing)
を読んでください.

## Building and publishing

| Targets       |                                    |
|---------------|------------------------------------|
| Meta Quest2   | [BUILD_QUEST2.md](BUILD_QUEST2.md) |
| Pico          | Not Supported                      |
| Windows (x64) | [BUILD_WIN64.md](BUILD_WIN64.md)   |

### Environment Values

| Key              | Description                     |
|------------------|---------------------------------|
| EH_RELEASE_BUILD | 製品版のビルド時に有効な環境変数                |
| EH_USE_SDK_QUEST | Meta Quest の SDK を使用する際に有効な環境変数 |

## Third Party Licenses

### Edanoue.Rx

minami: UniRX 改変ライブラリ, 一応リリース時に記載しておいたほうが良いかも. (とはいえだいぶいじったので面影がない)

### Oculus 関連

Meta の公式 SDK である (com.meta.xxx) 関連を使用.


## Layer Settings
- `PHY_XXX`: 物理的な判定に使用する
- `VIS_XXX`: カメラなど, レンダリング関連で使用する
- `META_XXX`: ハッキングデバイスのターゲットなど, 特定のトリガーに設定する