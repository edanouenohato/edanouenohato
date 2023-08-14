# Building and publishing for Meta Quest 2 (Release Channel)

- Quest2 向けのビルドとリリースチャンネルへのアップロードの手順書です

### 対応 Platform
- ❌ Meta Quest
- ✔ Meta Quest 2

### Switch Branch
- Main Branch: `quest2`
- Base Package Branch: `main`

### Download Android SDK 32

> **Note**
> .apk をビルドする人は全員必要な作業です.

1. Download SDK from [Google Drive](https://drive.google.com/drive/u/1/folders/1veY9o9A6R2ql1FwOFO0pbo8SLrUwEYwu).

![](https://i.gyazo.com/aaf7c3f09c0c983580703fef5ee969ed.png)

2. Extracts `android-32.zip` then move
   into `C:\Program Files\Unity\Hub\Editor\<UNITY VERSION>\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platforms`.

> **Note**
> Unity のバージョンが更新するたびに毎回やる必要があります, `android-32` フォルダは使いまわしていいです.

### KeyStore settings (for API signature)

> **Note**
> Meta Quest のリリースチャンネルにアップロードする人は必要な作業です. 単に .apk をビルドしてそのまま配布するだけであれば必要ありません.

1. Download KeyStore from [Google Drive](https://drive.google.com/drive/u/1/folders/1Lnag0VZSbb2BxFMgSzJCirLm5YEf2WV0).

> **Warning**
> このファイルは絶対に公開しないでください.

![](https://i.gyazo.com/29e09c3557956f5993d4f528b1d683b6.png)

2. Copy `branch-alpha.keystore` into project root directory.

### Per build settings

> **Note**
> Meta Quest のリリースチャンネルにアップロードする人は必要な作業です. 単に .apk をビルドしてそのまま配布するだけであれば必要ありません.

![](https://i.gyazo.com/e6a5f0a549b95005f69db1ce1ccac25a.png)

1. Open `Edit > Project Settings... > Player > Android Tab > Other Settings > Bundle Version Code` and set incremented version code.

> **Note**
> ![](https://i.gyazo.com/f83afe0e41802db97982af3bc876deb3.png)  
> [Meta Quest for Developers](https://developer.oculus.com/manage/applications/9709965022407423/) の下にある "最近アップロードされたビルド" にある "コード: XX" を確認して, +1 した数字を設定してください(上記の画像であれば, "7" を入力してビルド).

![](https://i.gyazo.com/c79fc22f174d5119c9a3320e00f1e1a8.png)

3. Open `Edit > Project Settings... > Player > Android tab > Publishing Settings`.
4. Check `Custom Keystore` and enter Password and Alias Password. (mention to @minami110)

### Build .apk

![](https://i.gyazo.com/fd703822c40d424345dc69255fb0e76d.png)

- `File > Build Settings` から `Android` に Switch Platform
を行ってください.
- ビルドを作成します
- .apk のファイル名は適当で大丈夫です.

### Deploy to Meta release channel

> **Note**
> Meta Quest のリリースチャンネルにアップロードする人は必要な作業です. 単に .apk をビルドしてそのまま配布するだけであれば必要ありません.

1. `Meta Quest Developer Hub` に `.apk` を **左側に** ドラッグドロップします.

![](https://i.gyazo.com/75a25b8cf75675939938afc9ebb77161.png)

2. App: `Branch`, Release Channel: `ALPHA` を選択して Next をクリック
3. 次のページで, 一番下にリリースノートを書く欄があるので, 簡単な内容を記載, Next をクリック
4. さらに次のページでは特に何もせずに, Upload をクリック
5. しばらく待って問題があればエラーが報告される, なければ OK


