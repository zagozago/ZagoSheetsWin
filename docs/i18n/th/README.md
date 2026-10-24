# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> เปิดสเปรดชีตใน Google Sheets แล้ว ไฟล์ต้นฉบับถูกแทนที่ด้วยทางลัด หากต้องการกู้คืนไฟล์ต้นฉบับ ให้เปิด ข้อมูลสำรอง
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 ภาษาของเอกสาร · เลือกภาษา</summary>

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
- [VI - Tiếng Việt / Vietnamese (vi)](../vi/README.md)
- [TR - Türkçe / Turkish (tr)](../tr/README.md)
- [KO - 한국어 / Korean (ko)](../ko/README.md)
- [IT - Italiano / Italian (it)](../it/README.md)
- **TH - ไทย / Thai (th)** — หน้าปัจจุบัน
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

**เปิดไฟล์สเปรดชีตในเครื่องด้วย Google ชีตได้โดยตรงจาก Windows**

ZagoSheetsWin เป็นแอป Windows ขนาดเล็กที่ช่วยให้การเปิดสเปรดชีตในเครื่องเป็นเรื่องง่าย:

**ดับเบิลคลิกไฟล์ → อัปโหลดและแปลง → เปิดใน Google ชีต**

หลังนำเข้าเสร็จ แอปสามารถแทนที่ไฟล์ต้นฉบับในเครื่องด้วยทางลัดอินเทอร์เน็ต (`.url`) ที่เชื่อมไปยังเอกสาร Google ชีต และเก็บข้อมูลสำรองของไฟล์ต้นฉบับไว้ในเครื่องเพื่อให้กู้คืนได้

เป้าหมายคือทำให้ Google ชีตใช้งานได้เหมือนแอป Windows ทั่วไปสำหรับเปิดไฟล์สเปรดชีตในเครื่อง

## ทำอะไรได้บ้าง

ZagoSheetsWin เชื่อมไฟล์สเปรดชีตใน Windows กับ Google ชีต เมื่อเปิดไฟล์ที่รองรับ แอปสามารถ:

- ตรวจจับและตรวจสอบไฟล์สเปรดชีต
- สร้างข้อมูลสำรองที่กู้คืนได้
- อัปโหลดโดยตรงไปยัง Google ไดรฟ์ผ่าน API ทางการของ Google
- แปลงเป็นเอกสาร Google ชีตรูปแบบดั้งเดิม
- เปิดผลลัพธ์ในเบราว์เซอร์เริ่มต้น
- สร้างทางลัด `.url` ในเครื่องไปยังเอกสาร Google
- ไม่อัปโหลดไฟล์เดิมซ้ำเมื่อเปิดอีกครั้ง

ไม่ต้องอัปโหลดผ่านไดรฟ์ด้วยตนเอง ไม่ต้องค้นหาเอกสารในเบราว์เซอร์ และไม่ต้องแปลงไฟล์ซ้ำ

## รูปแบบไฟล์ที่รองรับ

รูปแบบไฟล์เป้าหมายในการพัฒนาปัจจุบัน ได้แก่ `.xlsx`, `.xls`, `.ods`, `.csv` และ `.tsv`

บางรูปแบบอาจมีข้อจำกัดด้านความเข้ากันได้ แอปจะจัดการไฟล์ที่มีคุณสมบัติซึ่งไม่สามารถรักษาไว้ได้อย่างปลอดภัยด้วยความระมัดระวัง เพื่อหลีกเลี่ยงข้อมูลสูญหายโดยไม่แจ้งให้ทราบ

## ออกแบบมาสำหรับ Windows

ZagoSheetsWin พัฒนาสำหรับ Windows โดยเฉพาะ และทำงานร่วมกับระบบผ่าน **เปิดด้วย (Open with)** การลงทะเบียนประเภทไฟล์ การดับเบิลคลิกเปิดไฟล์ การเชื่อมต่อกับ File Explorer แบบเลือกใช้ และตัวติดตั้ง Windows แบบเนทีฟ

แอปไม่เปลี่ยนแอปเริ่มต้นของ Windows โดยพลการ ผู้ใช้ยังคงควบคุมการเชื่อมโยงประเภทไฟล์ได้

## ความปลอดภัยตั้งแต่ขั้นออกแบบ

การแทนที่ไฟล์ในเครื่องถือเป็นกระบวนการที่ต้องกู้คืนได้ ก่อนนำไฟล์ต้นฉบับออกจากโฟลเดอร์ แอปจะตรวจสอบว่า:

1. มีข้อมูลสำรองที่กู้คืนได้
2. สร้างเอกสาร Google ชีตสำเร็จแล้ว
3. บันทึกการเชื่อมโยงไฟล์ในเครื่องอย่างถาวรแล้ว
4. เขียนและตรวจสอบทางลัดอินเทอร์เน็ตเรียบร้อยแล้ว

หากกระบวนการล้มเหลว ไฟล์ต้นฉบับจะยังคงอยู่

> จะไม่ทำลายข้อมูลของผู้ใช้โดยไม่แจ้งให้ทราบ

## ข้อมูลสำรอง

สามารถจัดเก็บไฟล์ต้นฉบับไว้ในพื้นที่ข้อมูลสำรองส่วนตัวบนเครื่องก่อนแทนที่ด้วยทางลัด ตั้งค่าขีดจำกัดพื้นที่จัดเก็บ ระยะเวลาการเก็บรักษา และการล้างข้อมูลได้

ข้อมูลสำรองปกป้องไฟล์ต้นฉบับในขณะนำเข้า **ไม่ใช่การซิงค์สองทาง**: การเปลี่ยนแปลงภายหลังใน Google ชีตจะไม่ถูกเขียนกลับไปยังไฟล์ต้นฉบับ

## ความเป็นส่วนตัวและการเข้าถึง Google

ZagoSheetsWin ติดต่อ API ของ Google โดยตรงจากคอมพิวเตอร์ของคุณ

- เนื้อหาสเปรดชีตไม่ถูกส่งไปยังเซิร์ฟเวอร์ของ Zagotools
- โทเค็น OAuth จัดเก็บในเครื่องและป้องกันด้วยระบบความปลอดภัยของ Windows
- ใช้ขอบเขตสิทธิ์ `drive.file` ซึ่งจำกัดการเข้าถึงไฟล์ที่สร้างหรือเปิดผ่านแอป
- ขั้นตอนนำเข้าไม่จำเป็นต้องใช้ระบบติดตามวิเคราะห์

นโยบายความเป็นส่วนตัวและข้อกำหนดการใช้งาน: https://zagotools.top/legal.html

## สถานะโครงการ

ZagoSheetsWin กำลังพัฒนาอย่างต่อเนื่องและยังเป็น **ซอฟต์แวร์รุ่นอัลฟา** ขั้นตอนหลัก Windows → Google ชีตใช้งานได้แล้ว และกำลังปรับปรุงการติดตั้ง การกู้คืน ความเข้ากันได้ของรูปแบบไฟล์ การรองรับภาษา และประสบการณ์ผู้ใช้ อาจมีการเปลี่ยนแปลงก่อนออกรุ่นเสถียร

## ความสัมพันธ์กับ Open in Google

ZagoSheetsWin พัฒนาต่อยอดจาก [Open in Google](https://github.com/SwatiK425/open-in-google) ของ [SwatiK425](https://github.com/SwatiK425) ซึ่งเป็นพื้นฐานและแรงบันดาลใจเริ่มต้น

ปัจจุบันได้พัฒนาเป็นแอป Windows อิสระที่มีสถาปัตยกรรม ตัวติดตั้ง อินเทอร์เฟซ ระบบสำรองและกู้คืน การเชื่อมโยงไฟล์ การจัดการรูปแบบ และขั้นตอนเปิดไฟล์ใน Google ชีตของตนเอง โครงการต้นฉบับยังคงอิสระ โดยอาจเสนอการปรับปรุงทั่วไปกลับไปยังต้นทางเมื่อเหมาะสม

## โอเพนซอร์ส

ZagoSheetsWin เป็นซอฟต์แวร์ฟรีและโอเพนซอร์ส โดยคงเครดิตและข้อกำหนดใบอนุญาตของโค้ด Open in Google เดิม และระบุการพัฒนาต่อโดย ZagoSheetsWin / Zagotools อย่างชัดเจน

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## ใบอนุญาต

ใบอนุญาต MIT ดูรายละเอียดใน [LICENSE](../../../LICENSE)

---

**ZagoSheetsWin — โครงการจาก Zagotools**

ซอฟต์แวร์เล็ก ๆ สำหรับปัญหาจริง
