# ZagoSheetsWin

<details>
<summary>🌐 文件語言 · 揀選語言</summary>

- [English](../../../README.md)
- [简体中文](../zh/README.md)
- [हिन्दी](../hi/README.md)
- [Español](../es/README.md)
- [العربية](../ar/README.md)
- [Français](../fr/README.md)
- [বাংলা](../bn/README.md)
- [Português (Brasil)](../pt/README.md)
- [Bahasa Indonesia](../id/README.md)
- [اردو](../ur/README.md)
- [Русский](../ru/README.md)
- [Deutsch](../de/README.md)
- [日本語](../ja/README.md)
- [Tiếng Việt](../vi/README.md)
- [Türkçe](../tr/README.md)
- [한국어](../ko/README.md)
- [Italiano](../it/README.md)
- [ไทย](../th/README.md)
- [Filipino](../fil/README.md)
- [Bahasa Melayu](../ms/README.md)
- [Kiswahili](../sw/README.md)
- [Nigerian Pidgin](../pcm/README.md)
- [मराठी](../mr/README.md)
- [తెలుగు](../te/README.md)
- [Hausa](../ha/README.md)
- [ਪੰਜਾਬੀ](../pa/README.md)
- [தமிழ்](../ta/README.md)
- **粵語** — 目前頁面
- [فارسی](../fa/README.md)
- [አማርኛ](../am/README.md)
- [Basa Jawa](../jv/README.md)
- [ગુજરાતી](../gu/README.md)
- [ಕನ್ನಡ](../kn/README.md)
- [Yorùbá](../yo/README.md)
- [भोजपुरी](../bho/README.md)
- [پښتو](../ps/README.md)
- [ଓଡ଼ିଆ](../or/README.md)
- [မြန်မာ](../my/README.md)
- [മലയാളം](../ml/README.md)
- [Polski](../pl/README.md)
- [Basa Sunda](../su/README.md)
- [मैथिली](../mai/README.md)
- [Українська](../uk/README.md)
- [Afaan Oromoo](../om/README.md)
- [Oʻzbekcha](../uz/README.md)
- [سنڌي](../sd/README.md)
- [नेपाली](../ne/README.md)
- [Asụsụ Igbo](../ig/README.md)
- [Azərbaycan dili](../az/README.md)
- [Nederlands](../nl/README.md)
- [Avañe’ẽ](../gn/README.md)

</details>

**喺 Windows 直接用 Google 試算表開啟電腦入面嘅試算表檔案。**

ZagoSheetsWin 係一款輕巧嘅 Windows 應用程式，令開啟本機試算表變得簡單：

**雙擊檔案 → 上載同轉換 → 用 Google 試算表開啟**

成功匯入之後，ZagoSheetsWin 可以用指向 Google 試算表文件嘅互聯網捷徑（`.url`）取代原本嘅本機檔案，同時保留可以還原嘅原始檔案本機備份。

目標好簡單：令 Google 試算表喺開啟本機試算表檔案時，用落好似 Windows 原生應用程式咁自然。

## 有咩功能

ZagoSheetsWin 將 Windows 嘅試算表檔案同 Google 試算表連接起來。當你開啟支援嘅本機檔案，程式可以：

- 辨認同驗證試算表；
- 建立可以還原嘅備份；
- 透過 Google 官方 API 直接上載去 Google 雲端硬碟；
- 轉換成原生 Google 試算表文件；
- 喺預設瀏覽器開啟轉換結果；
- 建立指向 Google 文件嘅本機 `.url` 捷徑；
- 之後再次開啟同一檔案時，避免重複上載。

唔使手動上載去雲端硬碟、唔使喺瀏覽器搵檔案，亦唔使一次又一次轉換。

## 支援嘅格式

目前開發目標包括 `.xlsx`、`.xls`、`.ods`、`.csv` 同 `.tsv`。

部分格式可能有額外相容性限制。對於有啲功能無法安全保留嘅檔案，程式會審慎處理，避免喺你唔知情嘅情況下遺失資料。

## 專為 Windows 而設

ZagoSheetsWin 專為 Windows 開發，支援 **開啟方式（Open with）**、檔案類型註冊、雙擊開檔、可選擇嘅檔案總管整合，以及原生 Windows 安裝程式。

程式唔會擅自更改 Windows 預設應用程式。檔案關聯仍然由用戶控制。

## 設計上重視安全

取代本機檔案係一項必須可以復原嘅操作。程式喺移走原始檔案之前，會確認：

1. 已經有可以還原嘅備份；
2. Google 試算表文件已成功建立；
3. 本機檔案關聯已持久儲存；
4. 互聯網捷徑已成功寫入同驗證。

如果過程失敗，原始檔案會保留。

> 絕對唔可以喺用戶唔知情嘅情況下銷毀資料。

## 備份

原始檔案可以喺換成捷徑之前，保存喺私人本機備份空間。儲存上限、保留期限同清理選項都可以設定。

備份保護嘅係匯入嗰刻嘅原始檔案。**呢個唔係雙向同步**：之後喺 Google 試算表所做嘅修改，唔會寫返落原本嘅試算表檔案。

## 私隱同 Google 存取權限

ZagoSheetsWin 由你嘅電腦直接同 Google API 通訊。

- 試算表內容唔會傳送去 Zagotools 伺服器。
- OAuth 權杖儲存喺本機，並由 Windows 安全機制保護。
- 使用 `drive.file` 權限範圍，將存取限制喺透過程式建立或開啟嘅檔案。
- 匯入流程唔需要分析追蹤工具。

私隱政策及使用條款：https://zagotools.top/legal.html

## 項目狀態

ZagoSheetsWin 仍然積極開發中，目前屬於 **Alpha 測試版軟件**。Windows → Google 試算表嘅核心流程已經可以運作，而安裝、復原、格式相容性、國際化同使用體驗會繼續改善。首個穩定版本發佈之前，功能可能會改動。

## 同 Open in Google 嘅關係

ZagoSheetsWin 以 [SwatiK425](https://github.com/SwatiK425) 開發嘅 [Open in Google](https://github.com/SwatiK425/open-in-google) 為基礎，並由此衍生。原項目提供咗最初嘅技術基礎同靈感。

其後 ZagoSheetsWin 發展成獨立 Windows 應用程式，有自己嘅架構、安裝程式、介面、備份同復原系統、檔案關聯、格式處理，以及本機檔案轉入 Google 試算表嘅使用流程。原項目繼續獨立發展；合適嘅通用改進可以回饋 upstream。

## 開放原始碼

ZagoSheetsWin 係免費、開放原始碼軟件。項目保留原本 Open in Google 程式碼嘅署名同授權要求，同時清楚標示 ZagoSheetsWin / Zagotools 嘅後續開發。

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## 授權條款

MIT 授權條款。詳情請參閱 [LICENSE](../../../LICENSE)。

---

**ZagoSheetsWin — Zagotools 項目**

用細小軟件，解決真實問題。
