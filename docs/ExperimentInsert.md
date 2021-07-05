# 実験用インサートの設定について

## 実験インサート手順

### DBの変更

使用した車の登録を行う.  
車両諸元などからCARテーブルにレコードを追加.  
運転手が新人の場合には, DRIVERSテーブルにレコードを追加.  
今回行った場所をPLACEテーブルに追加.  
ドライバー交代地点も含む.  

### SensorLogInserterの変更

`SensorLogInserter/Constant/CarName.cs`に車名とidの対応を記述.  
`SensorLogInserter/Constant/DirectoryNames.cs`
`SensorLogInserter/Handlers/DirectorySearcher.cs`にドライバー毎のログファイル格納場所の検索処理追加.  
`SensorLogInserter/ViewModels/MainWindowViewModel.cs`の`GenerateInsertConfig()`に研究室メンバーを選択した場合のドライバーオプションの設定を記述.  

## インサートされない場合

### ログファイル名に関する問題

ロガーでのドライバー名やセンサ名, 車名が間違っている場合にはインサートされない場合がある.  
修正後, インサート用の中間テーブルのデータを削除し, 再度インサートをする.  