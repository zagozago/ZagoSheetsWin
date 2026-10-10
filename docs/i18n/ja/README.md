# ZagoSheetsWin

<details>
<summary>🌐 ドキュメントの言語を選択</summary>

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
- **日本語** — 現在のページ
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
- [粵語](../yue/README.md)
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
