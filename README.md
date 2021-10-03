# TinyTimyの使い方
TinyTimyはフローティング表示に適したWindowsタイマーアプリです。

![UIサンプル](https://user-images.githubusercontent.com/43531665/135755658-a5861019-dd6c-4376-8b7a-541d933c9d79.png)

- [x] 最前面の表示をサポート
- [x] 時刻の表示形式をカスタマイズ可能
- [x] タスクバーアイコン非表示
- [x] 限定範囲でのリサイズ可能
- [x] ダークモード対応  


## 設定画面

![](https://user-images.githubusercontent.com/43531665/135755761-4979498a-5de6-409a-8854-762e099af783.png)

### タイマー設定
計測したい時間を入力します。

### ディスプレイ書式
表示形式はC#の[TimeSpan書式指定文字列](https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/custom-timespan-format-strings)をそのまま使用しています。

概要は以下の通りになります。
| 文字 | 説明 |
----|---- 
| dd | 日 |
| hh | 時間 |
| mm | 分間 |
| ss | 秒間 |
| 'hoge' or \hoge | hogeを文字としてそのまま挿入 |

### DarkMode
ダークモードも対応しています。

![image](https://user-images.githubusercontent.com/43531665/135757715-8a208c78-37b5-484a-a653-a6aaafef01cc.png)
