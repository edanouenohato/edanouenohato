# CI/CD Build scripts

## Build.bat
Unity で .apk のビルド, その後 Quest の Release Channel のアップロードを行うバッチファイルです.

### 使い方
1. env ファイルを .env という名前でコピーします
2. 以下の環境変数を設定します
    - `UNITY_EDITOR_ROOT`: Unity Editor がインストールされているルートパス
    - `ANDROID_BUNDLE_VERSION_CODE`: Android のバージョンコードを指定
    - `OVR_APP_ID`: Oculus のアプリケーション ID
    - `UNITY_KEYSTORE_PASS`: KeyStore のパスワード
    - `UNITY_KEYALIAS_PASS`: KeyAlias のパスワード
    - `OVR_PLATFORM_TOOL`: ovr-platform-util.exe へのパス
    - `OVR_APP_ID`: Oculus のアプリケーション ID
    - `OVR_APP_SECRET`: Oculus のアプリケーションシークレット
3. PowerShell を起動し, `Build.bat` を実行します
    - $ `./Build.bat`