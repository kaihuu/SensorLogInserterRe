# SensorLogInserterRe
走行ログをDBに格納するためのプログラム。  
種々のECOLOG推定も行える。  

## 2018春LEAF実験用使い方
別の**観光オプション**を用いた実験を行った場合には、後述する観光インサーターの準備の項目に記載されている変更を行う。  

**TODO**
**道路リンクデータをDBに登録させる必要がある**

1. 起動後研究室メンバーにチェック
2. 期間指定ありにチェック、カレンダーで0424~0425を選択する
3. 推定モデルやGPS補正を指定
4. 観光オプションにチェック
5. データ挿入ボタンをクリック

以上の手順でインサートが行える。  

## 観光インサーターの準備
- itsserverの個人ディレクトリに運転ログのアップロード
- NMEAデータのECOLOGDBへのアップロード
- ECOLOGDBのPLACESテーブルへのレコードの挿入
- 使用した車をDBとコードに追加
- マップマッチング用道路リンクの追加

### itsserverの個人ディレクトリに運転ログcsvのアップロード
`\\itsserver\ECOLOG_LogData_itsserver\[DriverName]\[CarName]\[SensorName]`以下のディレクトリに以下のファイルをアップロード  
- [数字列]MetaDataLog
- [数字列]Unsent16HzAccel
- [数字列]UnsentGPS
- [数字列]UnsentIlluminance
- [数字列]UnsentNmea

**TODO*
各ファイルの詳細  

### NMEAデータをECOLOGDBへアップロード
`NMEAInserter`を利用する。  
**TODO**
NMEAInserterについての記述。  

### ECOLOGDBのPLACESテーブルへのレコード挿入
ドライバー交代した地点、駐車した地点の座標をDBに挿入する。  
この際、PROPERTY='sightseeing'を設定する。  

### 使用した車をDBとコードに追加
**TODO**
コード側からDBのCarsテーブルを参照するようにしてDBとコードの重複を無くす。  

Carsテーブルに使用した車の情報を登録する。  
TIRE_RADIUSはメートル単位のタイヤ半径を記載する。  
(ホイール半径ではないことに注意が必要。)  
[このページの情報](http://cars-japan.net/index.html)の情報を参考にする。  

FRONTAL_PROJECTED_AREAは前方投影面積を示す。  
ECOLOGDBでの計算式は以下の通りである。  
`車幅 x (車高 - タイヤ半径)`  
以上の式で求めた値の小数第3位を四捨五入した値を挿入する。  

CD_VALUEには空気抵抗係数を挿入する。  
カタログスペックに記載があったりするらしい。  

### マップマッチング用の道路リンクの追加
国土交通省が公開している交差点を示すノードと道路形状を示す道路リンク構成点により構成されるデータを挿入する必要がある。  

**TODO**
データ挿入方法の記述。  