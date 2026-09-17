\# ScriptSquid


ScriptSquid は、C# プロジェクト内の `.cs` ファイルを高速スキャンし、  

\*\*行数 / サイズ / 更新日時 / パス\*\* を一覧表示する解析ツールです。  

フィルタ式・フィルタ履歴・CommandList 表示に対応しています。

\---



\## 📌 Features

\### 🔍 Fast Scan

指定フォルダ以下の `.cs` を再帰検索し、以下を一覧化します：

\- FileName  

\- Lines  

\- SizeKB  

\- Updated  

\- FullPath  

\---



\### 🎯 Filter System（式パーサー）

filterBox に式を書くとリアルタイムで絞り込みできます。

\#### 対応変数

\- `Lines`

\- `SizeKB`

\- `FileName`

\- `FullPath`

\- `Updated`



\#### 対応演算子

| 演算子 | 意味           |

|--------|----------------|

| `>`    | より大きい     |

| `<`    | より小さい     |

| `=`    | 完全一致       |

| `>=`   | 以上           |

| `<=`   | 以下           |

| `≒`    | 部分一致（文字列） |



\#### 使用例

```text

Lines>100

SizeKB<50

Updated≒"2026"

FileName≒"Player"

FullPath≒"/UI/"



🕘 Filter History

入力したフィルタ式を自動で履歴に追加

ComboBox から選択すると filterBox に再適用されます



📄 CommandList Viewer

右側に CommandList.txt の内容を表示します。

必須ファイルではありません

無くても ScriptSquid は動作します

「フィルタ式テンプレをまとめておきたいユーザー向け」の任意ファイルです

配置場所（EXE と同じフォルダ）：

ScriptSquid.exe

CommandList.txt



CommandList.txt の例

\# ScriptSquid Command List

Lines>100

Lines<50

SizeKB>500

Updated≒"2026"



FileName≒"Player"

FullPath≒"/UI/"



LargeFiles: SizeKB>500

PlayerScripts: FileName≒"Player"



🧭 How to Use

1\. フォルダ選択

「Browse」ボタンでフォルダを選択

または folderBox にパスを直接入力



2\. スキャン

「Scan」ボタンで .cs ファイルを解析

resultList に一覧表示されます



3\. フィルタ

filterBox に式を入力すると即時反映

フィルタ履歴から選択して再適用可能



4\. ファイルを開く

resultList の項目をダブルクリック

対応ファイルが Notepad で開きます



🖼 UI Screenshot

GitHub に画像をアップロードしたらここに貼ってください

例：

!\[ScriptSquid UI](docs/screenshot.png)



🛠 Build

OS: Windows

Runtime: .NET 9

UI: WinForms（コードUI構成）



⚠ Notes

.cs ファイルのみ解析対象です



CommandList.txt は任意ファイルです（無くても動作します）

フィルタ式が不正な場合は全件表示になります







