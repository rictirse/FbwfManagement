# Fbwf Management
Fbwf Managementk base on 微軟開發的File-Based Write Filter
起先用於沙盤環境操作
基於市場上的各項ramdisk都非動態分配使用量，而是設定多少直接把記憶體吃掉
所以想到拿Fbwf + vhd 開發出非常羽量級但相容性極高的動態分配ramdisk
 
載點：
https://github.com/rictirse/FbwfManagement/releases/download/FbwfManagement/FbwfManagement_1.0.1641.zip
 
## 安裝說明
1. 使用Windows UAC開啟
2. 按下Fbwf installed後重開
3. 重開後按下Fbwf enable ※爾後修改任何項目都需要重開機才會生效
4. 如果純做動態RamDisk建議勾選
* Virtual display mode
* Auto mount at boot
* 設定RamDisk虛擬磁碟機掛載磁碟代號
* 記憶體配置建議為系統的60~80%，看電腦與使用需求
* 按Apply套用後重開機即可

## 各項功能說明
* Fbwf installed > 啟用即為安裝，安裝後須重開機
* Fbwf enable > 啟用Fbwf
* Overlay cache data compression > 壓縮快取記憶體，會消耗額外的CPU使用率，如記憶體容量夠大不建議開啟
* Overlay cache pre-allocation > Fbwf快取記憶體大小會直接佔用系統記憶體，而非用多少吃多少，沒有特殊需求不建議開啟
* Virtual display mode > 預設為顯示原磁碟容量，Virtual模式為顯示快取記憶體大小
* Auto mount at boot > 會掛載一顆vhd當Fbwf快取的引導碟，如果看不懂這句，建議直接開啟
* Driver letter > 為掛載的VHD磁碟代號
* Cache size > 設定給快取記憶體大小，建議為總記憶體的60~80%
![](https://i.imgur.com/jzaJDGd.png)

## 開發者資訊
程式主要功能可能為幾個class
### VHDMounter 
* 使用diskpart.exe 建立 / 掛載 / 卸載vhd
### FbwfTaskScheduler
* 使用Microsoft.Win32.TaskScheduler.dll 建立工作排成，每次重開機自動掛載vhd
### FbwfInstall & FbwfRegistry
* 從embedded resource安裝 fbwf驅動，安裝 / 移除 / reg寫入 / reg刪除
### FbwfMgr
* 專案的主要Class 有MVVM要用的Property與Fbwf CLI的parser 
### FbwfMgrCommand
* 跟FbwfMgr 是同一個partial，主要是static function用於跟fbwfmgr的CLI溝通用
