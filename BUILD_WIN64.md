# Building for Windos 64

- Win64 向けのビルドの手順書です

> **Note**  
> 現時点では, テスター向けへのビルドのみで, 公式な Win64 サポートの内容ではありません.

### 対応 Platform

| 動作  | Platform | OpenXR Backend |
|-----|----------|----------------|
| ✔   | Win64    | Oculus         |
| 未確認 | Win64    | SteamVR        |

### Switch Branch

- Main Branch: `quest2`
- Base Package Branch: `main`

### Build .exe

![](https://i.gyazo.com/cdb41e5357ce0d94ae2130159e84653e.png)

- `File > Build Settings` から `Windows, Mac, Linux` に Switch Platform
  を行ってください.
- ビルドを作成します

### Deploy to test users

> **Note**  
> Android ビルドとはちがって, 複数のファイルを配布する必要があります

![](https://i.gyazo.com/d760776a44fe5e31634787ccdfd24962.png)

- 指定先のディレクトリにある上記のファイルを `.zip` に固めて配布します.

