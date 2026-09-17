# ScriptSquid

ScriptSquid is a lightweight analysis tool that scans all `.cs` files inside a folder and displays  
**file name, line count, size, last updated time, and full path** in a sortable list.  
It supports filter expressions, filter history, and a CommandList viewer.

---

## 🚀 Features

### 🔍 Fast Scan
Recursively scans all `.cs` files and displays:

- FileName  
- Lines  
- SizeKB  
- Updated  
- FullPath  

---

### 🎯 Filter System (Expression Parser)

Enter filter expressions in the filterBox to instantly narrow down results.

#### Supported Variables
- `Lines`
- `SizeKB`
- `FileName`
- `FullPath`
- `Updated`

#### Supported Operators

| Operator | Meaning            |
|---------|--------------------|
| `>`     | greater than       |
| `<`     | less than          |
| `=`     | equal              |
| `>=`    | greater or equal   |
| `<=`    | less or equal      |
| `≒`     | contains (string)  |

#### Examples

```text
Lines>100
SizeKB<50
Updated≒"2026"
FileName≒"Player"
FullPath≒"/UI/"

🕘 Filter History
Every filter expression you enter is automatically added to the history list
Selecting an item re-applies the filter

📄 CommandList Viewer
Displays the contents of CommandList.txt on the right side of the UI.
This file is optional
ScriptSquid works even if it does not exist
It is intended for users who want to store filter templates or custom commands

Place it next to the executable:

ScriptSquid.exe
CommandList.txt

Example CommandList.txt
# ScriptSquid Command List

Lines>100
Lines<50
SizeKB>500
Updated≒"2026"

FileName≒"Player"
FullPath≒"/UI/"

LargeFiles: SizeKB>500
PlayerScripts: FileName≒"Player"

🧭 How to Use
1. Select Folder
Click Browse
Or manually enter a path

2. Scan
Click Scan to analyze all .cs files

3. Filter
Type expressions in the filterBox
Choose from filter history to reapply

4. Open File
Double‑click an item in the result list
The file opens in Notepad

🖼 UI Screenshot
Add your screenshot here after uploading it to GitHub
Example:
![ScriptSquid UI](docs/screenshot.png)

🛠 Build
OS: Windows
Runtime: .NET 9
UI: WinForms (Code‑based layout)

⚠ Notes
Only .cs files are scanned

CommandList.txt is optional
Invalid filter expressions fall back to showing all results