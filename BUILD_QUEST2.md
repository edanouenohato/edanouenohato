# Building and publishing for Meta Quest 2 (Release Channel)

- Quest2 向けのビルドとリリースチャンネルへのアップロードの手順書です

### Download Android SDK 32

> **Note**
> .apk をビルドする人は全員必要な作業です.

> **Note**
> Unity のバージョンが更新するたびに毎回やる必要があります, `android-32` フォルダは使いまわしていいです.

1. Download SDK from [Google Drive](https://drive.google.com/drive/u/1/folders/1veY9o9A6R2ql1FwOFO0pbo8SLrUwEYwu).

![](https://i.gyazo.com/aaf7c3f09c0c983580703fef5ee969ed.png)

2. Extracts `android-32.zip` then move
   into `C:\Program Files\Unity\Hub\Editor\<UNITY VERSION>\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platforms`.

### KeyStore settings (for API signature)

> **Note**
> Meta Quest のリリースチャンネルにアップロードする人は必要な作業です. 単に .apk をビルドしてそのまま配布するだけであれば必要ありません.

1. Download KeyStore from [Google Drive](https://drive.google.com/drive/u/1/folders/1Lnag0VZSbb2BxFMgSzJCirLm5YEf2WV0).

> **Warning**
> このファイルは絶対に公開しないでください.

![](https://i.gyazo.com/29e09c3557956f5993d4f528b1d683b6.png)

2. Copy `branch-alpha.keystore` into project root directory.

### Per build settings

> Meta Quest のリリースチャンネルにアップロードする人は必要な作業です. 単に .apk をビルドしてそのまま配布するだけであれば必要ありません.

![](https://i.gyazo.com/bed4d3b305d31457658b948a5aa0cbc4.png)

1. Open `Edit > Project Settings... > Player' and update version.

> **Note**
> 事前に前の誰かが上げているバージョン名を確認しておいて, 必ず重複しないように設定します.

![](https://i.gyazo.com/c79fc22f174d5119c9a3320e00f1e1a8.png)

3. Open `Edit > Project Settings... > Player > Android tab > Publishing Settings`.
4. Check `Custom Keystore` and enter Password and Alias Password. (mention to @minami110)

### Build .apk

- 普通に Android ビルドを作成します

### Deploy to Meta release channel

> Meta Quest のリリースチャンネルにアップロードする人は必要な作業です. 単に .apk をビルドしてそのまま配布するだけであれば必要ありません.

1. `Meta Quest Developer Hub` に `.apk` を **左側に** ドラッグドロップします.

![](https://i.gyazo.com/75a25b8cf75675939938afc9ebb77161.png)

2. App: `Branch`, Release Channel: `ALPHA` を選択して Next. その後は特に何も入力せずにアップロード
3. しばらく待って問題があればエラーが報告される, なければ OK


