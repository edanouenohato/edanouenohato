# edanouenohato
枝の上の鳩のプロジェクトです.  
Issues: [🚀 EH Beta - EH Issues](https://scrapbox.io/eh-issues/%F0%9F%9A%80_EH_Beta)

## Installation
### Install Unity Editor
`Unity 2023.1` を使用しています, 最新版をインストールしてください.

![](https://i.gyazo.com/adcd4196aed56184686272682baa4496.png)

`Android Build Support` modules が必要です.  
`Rider` を使用するため, Visual Studio は必要ありません.

### Clone project
```console
git clone --recursive https://github.com/edanouenohato/edanouenohato.git
```
[Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules) でパッケージを管理しているので, `--recursive` オプションを付けて clone してください.

> **Note**  
> `--recursive` オプションをつけ忘れた場合は, Clone 後に Repository のディレクトリに移動して以下のコマンドを実行してください.
> ```console
> cd edanouenohato
> git submodule update --init --recursive
> ```

### Project Settings
#### Platform を Androidに
![](https://i.gyazo.com/fd703822c40d424345dc69255fb0e76d.png)

Unity Editor の起動後, `File > Build Settings` から `Android` に Switch Platform
 を行ってください.

## For Developers  
### IDE
[JetBrains Rider](https://www.jetbrains.com/rider/) を使用してください. ライセンスを所有していない方は [@minami110](https://github.com/minami110) まで.    
プロジェクトを初めて clone した際は [Rider 最初の設定](https://scrapbox.io/edanoue/Rider_最初の設定) を参考に初期設定を行ってください.

### VR Headset
[Meta Quest 2](https://www.meta.com/jp/quest/products/quest-2/) を使用してください. 所有していない方は [@minami110](https://github.com/minami110) まで.

## Deployments
### Environment Values
| Key                   | Description                |
|-----------------------|----------------------------|
| EH_RELEASE_BUILD      | 製品版のビルド時に有効な環境変数           |
| TARGET_PLATFORM_QUEST | Meta Quest 向けのビルド時に有効な環境変数 |

## Third Party Licenses
### Edanoue.Rx
UniRX 改変ライブラリ, 一応リリース時に記載しておいたほうが良いかも.

### Oculus 関連
Meta の公式 SDK である (com.meta.xxx) 関連を使用.