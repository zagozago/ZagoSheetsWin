# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Bảng tính đã được mở trong Google Sheets. Tệp gốc đã được thay bằng lối tắt. Để khôi phục tệp, hãy mở Sao lưu.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Ngôn ngữ tài liệu · Chọn ngôn ngữ</summary>

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
- [JA - 日本語 / Japanese (ja)](../ja/README.md)
- **VI - Tiếng Việt / Vietnamese (vi)** — trang hiện tại
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

**Mở trực tiếp các tệp bảng tính trên máy tính bằng Google Sheets từ Windows.**

ZagoSheetsWin là ứng dụng Windows gọn nhẹ giúp mở bảng tính cục bộ trở nên đơn giản:

**nhấp đúp vào tệp → tải lên và chuyển đổi → mở trong Google Sheets**

Sau khi nhập thành công, ZagoSheetsWin có thể thay tệp gốc bằng lối tắt Internet (`.url`) dẫn đến tài liệu Google Sheets, đồng thời giữ bản sao lưu cục bộ có thể khôi phục.

Mục tiêu là giúp Google Sheets hoạt động như ứng dụng Windows thông thường để mở bảng tính trên máy.

## Chức năng

ZagoSheetsWin kết nối các tệp bảng tính trên Windows với Google Sheets. Khi mở tệp được hỗ trợ, ứng dụng có thể:

- nhận diện và kiểm tra bảng tính;
- tạo bản sao lưu có thể khôi phục;
- tải tệp trực tiếp lên Google Drive bằng API chính thức của Google;
- chuyển thành tài liệu Google Sheets gốc;
- mở bảng tính trong trình duyệt mặc định;
- tạo lối tắt `.url` trên máy đến tài liệu Google;
- tránh tải lại cùng một tệp ở những lần mở sau.

Không cần tải lên Drive thủ công, tìm tệp trong trình duyệt hay chuyển đổi lặp lại.

## Định dạng được hỗ trợ

Các định dạng đang được phát triển: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

Một số định dạng có thể có giới hạn tương thích. Tệp chứa những tính năng không thể bảo toàn an toàn sẽ được xử lý thận trọng để tránh mất dữ liệu âm thầm.

## Thiết kế cho Windows

ZagoSheetsWin dành riêng cho Windows, tích hợp **Open with (Mở bằng)**, đăng ký loại tệp, mở bằng nhấp đúp, tích hợp File Explorer tùy chọn và trình cài đặt Windows gốc.

Ứng dụng không âm thầm thay đổi chương trình mặc định. Người dùng kiểm soát các liên kết loại tệp.

## An toàn ngay từ thiết kế

Việc thay tệp cục bộ được coi là thao tác có thể khôi phục. Trước khi đưa tệp gốc ra khỏi thư mục, ứng dụng kiểm tra rằng:

1. đã tồn tại bản sao lưu có thể khôi phục;
2. tài liệu Google Sheets được tạo thành công;
3. liên kết tệp cục bộ được lưu bền vững;
4. lối tắt Internet được ghi và xác thực.

Nếu xảy ra lỗi, tệp gốc vẫn được giữ nguyên.

> Không bao giờ âm thầm hủy dữ liệu của người dùng.

## Sao lưu

Tệp gốc có thể được lưu trong vùng sao lưu cục bộ riêng tư trước khi thay bằng lối tắt. Có thể cấu hình giới hạn dung lượng, thời gian lưu giữ và dọn dẹp.

Bản sao lưu bảo vệ tệp tại thời điểm nhập. Đây **không phải đồng bộ hai chiều**: các thay đổi về sau trong Google Sheets không được ghi ngược vào tệp bảng tính gốc.

## Quyền riêng tư và truy cập Google

ZagoSheetsWin giao tiếp trực tiếp từ máy tính của bạn với API Google.

- Nội dung bảng tính không được gửi tới máy chủ Zagotools.
- Token OAuth được lưu cục bộ, bảo vệ bằng cơ chế bảo mật Windows.
- Ứng dụng sử dụng phạm vi quyền `drive.file`, giới hạn truy cập vào các tệp được tạo hoặc mở bằng ứng dụng.
- Nhập bảng tính không đòi hỏi theo dõi phân tích.

Chính sách quyền riêng tư và Điều khoản sử dụng: https://zagotools.top/legal.html

## Trạng thái dự án

ZagoSheetsWin đang phát triển và hiện là **phần mềm alpha**. Quy trình Windows → Google Sheets chính đã hoạt động; việc cài đặt, khôi phục, tương thích định dạng, quốc tế hóa và trải nghiệm người dùng tiếp tục được cải thiện. Các tính năng có thể thay đổi trước bản ổn định đầu tiên.

## Mối quan hệ với Open in Google

ZagoSheetsWin được xây dựng dựa trên [Open in Google](https://github.com/SwatiK425/open-in-google) của [SwatiK425](https://github.com/SwatiK425), nguồn gốc và cảm hứng ban đầu của dự án.

ZagoSheetsWin nay là ứng dụng Windows độc lập với kiến trúc, trình cài đặt, giao diện, sao lưu và khôi phục, liên kết tệp, xử lý định dạng và quy trình mở tệp cục bộ trong Google Sheets riêng. Dự án gốc vẫn độc lập; các cải tiến chung có thể được đóng góp trở lại khi phù hợp.

## Mã nguồn mở

ZagoSheetsWin là phần mềm miễn phí, mã nguồn mở. Các yêu cầu ghi công và giấy phép của mã Open in Google gốc được giữ nguyên, đồng thời phân biệt phần phát triển tiếp theo của ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Giấy phép

Giấy phép MIT. Xem [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — một dự án của Zagotools**

Phần mềm nhỏ cho vấn đề thực tế.
