# ZagoSheetsWin

> ⚠️ **Alpha 0.9.42 — English safety notice; full editorial translation pending.**
>
> スプレッドシートを Google Sheets で開きました。元のファイルはショートカットに置き換えられました。元のファイルを復元するには、バックアップ(B) を開いてください。
>
> **CSV/TSV v4:** up to **2,000,000 cells**, **100,000 logical rows**, 1,000 columns and 20 MiB input/payload, subject to other limits. Real delimiter ambiguity shows a local preview. If converted Google cell values differ, **the original must NOT be replaced by a shortcut**.
>
> **XLS/ODS:** complex features may still be converted with partial fidelity and the source **may** be replaced after verified import; the original bytes remain recoverable in Backups. Use **Import as copy** if you must keep the source file in place.
>
> **Backups:** automatic cleanup by age and quota, **no toggle**. Only eligible completed backups are cleaned; quota pressure may delete them before retention expires. Files from different sources can import in parallel; only actual backup-capacity shortages can cause a cancellable wait.
>
> **Release state:** 0.9.42 is **not a stable release**. The latest Windows CI and preceding protected distribution had failing checks; real-Google acceptance remains pending. Some text below describes older alpha behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 ドキュメントの言語を選択</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- [FR - Français / French (fr)](../fr/README.md)
- [BN - বাংলা / Bengali (bn)](../bn/README.md)
- [PT - Português / Portuguese (pt)](../pt/README.md)
- [ID - Bahasa Indonesia / Indonesian (id)](../id/README.md)
- [UR - اردو / Urdu (ur)](../ur/README.md)
- [RU - Русский / Russian (ru)](../ru/README.md)
- [DE - Deutsch / German (de)](../de/README.md)
- **JA - 日本語 / Japanese (ja)** — 現在のページ
- [VI - Tiếng Việt / Vietnamese (vi)](../vi/README.md)
- [TR - Türkçe / Turkish (tr)](../tr/README.md)
- [KO - 한국어 / Korean (ko)](../ko/README.md)
- [IT - Italiano / Italian (it)](../it/README.md)
- [TH - ไทย / Thai (th)](../th/README.md)
- [TL - Filipino](../fil/README.md)
- [MS - Bahasa Melayu / Malay (ms)](../ms/README.md)
- [SW - Kiswahili / Swahili (sw)](../sw/README.md)
- [PCM - Nigerian Pidgin](../pcm/README.md)
- [MR - मराठी / Marathi (mr)](../mr/README.md)
- [TE - తెలుగు / Telugu (te)](../te/README.md)
- [HA - Hausa](../ha/README.md)
- [PA - ਪੰਜਾਬੀ / Punjabi (pa)](../pa/README.md)
- [TA - தமிழ் / Tamil (ta)](../ta/README.md)
- [YUE - 粵語 / Cantonese (yue)](../yue/README.md)
- [FA - فارسی / Persian (fa)](../fa/README.md)
- [AM - አማርኛ / Amharic (am)](../am/README.md)
- [JV - Basa Jawa / Javanese (jv)](../jv/README.md)
- [GU - ગુજરાતી / Gujarati (gu)](../gu/README.md)
- [KN - ಕನ್ನಡ / Kannada (kn)](../kn/README.md)
- [YO - Yorùbá / Yoruba (yo)](../yo/README.md)
- [BHO - भोजपुरी / Bhojpuri (bho)](../bho/README.md)
- [PS - پښتو / Pashto (ps)](../ps/README.md)
- [OR - ଓଡ଼ିଆ / Odia (or)](../or/README.md)
- [MY - မြန်မာဘာသာ / Burmese (my)](../my/README.md)
- [ML - മലയാളം / Malayalam (ml)](../ml/README.md)
- [PL - Polski / Polish (pl)](../pl/README.md)
- [SU - Basa Sunda / Sundanese (su)](../su/README.md)
- [MAI - मैथिली / Maithili (mai)](../mai/README.md)
- [UK - Українська / Ukrainian (uk)](../uk/README.md)
- [OM - Afaan Oromoo / Oromo (om)](../om/README.md)
- [UZ - Oʻzbekcha / Uzbek (uz)](../uz/README.md)
- [SD - سنڌي / Sindhi (sd)](../sd/README.md)
- [NE - नेपाली / Nepali (ne)](../ne/README.md)
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Windows からローカルの表計算ファイルを直接 Google スプレッドシートで開けます。**

ZagoSheetsWin は、ローカルの表計算ファイルを簡単に開ける軽量な Windows アプリです。

**ファイルをダブルクリック → アップロードと変換 → Google スプレッドシートで開く**

インポートが成功すると、元のローカルファイルを Google スプレッドシートへのインターネットショートカット（`.url`）に置き換え、復元可能なバックアップをローカルに保存できます。

目標は、Google スプレッドシートをローカルファイル用の Windows ネイティブアプリのように使えるようにすることです。

## できること

ZagoSheetsWin は Windows の表計算ファイルと Google スプレッドシートをつなぎます。対応ファイルを開くと、以下の処理が可能です。

- ファイルの種類を判別し、検証する
- 復元可能なバックアップを作成する
- Google の公式 API で Google ドライブへ直接アップロードする
- Google スプレッドシート形式へ変換する
- 変換結果を既定のブラウザーで開く
- Google ドキュメントへのローカル `.url` ショートカットを作成する
- 次回以降、同じファイルの重複アップロードを避ける

ドライブへの手動アップロードも、ブラウザーでのファイル探しも、繰り返しの変換も不要です。

## 対応形式

現在の開発対象は `.xlsx`、`.xls`、`.ods`、`.csv`、`.tsv` です。

形式によって互換性上の制限があります。保持できない機能を含むファイルは、気付かないうちにデータが失われないよう慎重に扱います。

## Windows 専用設計

ZagoSheetsWin は Windows 向けに開発され、**「プログラムから開く」（Open with）**、ファイル形式の登録、ダブルクリック起動、任意のエクスプローラー連携、Windows ネイティブのインストーラーを備えます。

既定のアプリを無断で変更せず、関連付けはユーザーが管理できます。

## 安全性を重視した設計

ローカルファイルの置き換えは復元可能な操作として扱います。元のファイルをフォルダーから取り除く前に、次の条件を確認します。

1. 復元可能なバックアップがある
2. Google スプレッドシートが正常に作成された
3. ローカルの関連付け情報が永続的に保存された
4. インターネットショートカットの書き込みと検証が成功した

失敗時は元のファイルを保持します。

> ユーザーのデータを通知せずに失わせない。

## バックアップ

元のファイルを置き換える前に、プライベートなローカルバックアップ領域に保存できます。容量上限、保持期間、クリーンアップを設定できます。

バックアップが保護するのはインポート時の元ファイルです。**双方向同期ではありません**。Google スプレッドシートで後から行った変更は元ファイルに書き戻されません。

## プライバシーと Google へのアクセス

ZagoSheetsWin は PC から Google の公式 API に直接通信します。

- 表計算ファイルの内容は Zagotools のサーバーへ送信されません。
- OAuth トークンは Windows のセキュリティ機構で保護してローカルに保存します。
- `drive.file` スコープを使用し、アプリで作成または開いたファイルにアクセスを制限します。
- インポート処理に分析用トラッキングは必要ありません。

プライバシーポリシーと利用規約：https://zagotools.top/legal.html

## 開発状況

ZagoSheetsWin は開発中の **アルファ版**です。Windows → Google スプレッドシートの基本機能は動作しており、インストール、復元、互換性、多言語対応、操作性を改善中です。最初の安定版までに変更される場合があります。

## Open in Google との関係

ZagoSheetsWin は [SwatiK425](https://github.com/SwatiK425) による [Open in Google](https://github.com/SwatiK425/open-in-google) を基にした派生プロジェクトです。同プロジェクトは最初の技術的基盤と着想を提供しました。

現在は独自のアーキテクチャ、インストーラー、UI、バックアップと復元、ファイル関連付け、形式処理、ローカルファイルを Google スプレッドシートで開く体験を持つ独立した Windows アプリです。元のプロジェクトも独立しており、汎用的な改善は必要に応じて upstream に提案できます。

## オープンソース

ZagoSheetsWin は無料のオープンソースソフトウェアです。元のコードに必要な著作権表示とライセンスを維持し、ZagoSheetsWin / Zagotools による追加開発を区別しています。

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## ライセンス

MIT ライセンス。詳細は [LICENSE](../../../LICENSE) をご覧ください。

---

**ZagoSheetsWin — Zagotools プロジェクト**

現実の問題を解決する、小さなソフトウェア。
