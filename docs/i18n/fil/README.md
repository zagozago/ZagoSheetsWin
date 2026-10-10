# ZagoSheetsWin

<details>
<summary>🌐 Wika ng dokumentasyon · Piliin ang wika</summary>

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
- **Filipino** — kasalukuyang pahina
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

</details>

**Direktang buksan ang mga lokal na spreadsheet file sa Google Sheets mula sa Windows.**

Ang ZagoSheetsWin ay magaan na Windows application na nagpapadali sa pagbubukas ng mga spreadsheet sa iyong computer:

**i-double-click ang file → i-upload at i-convert → buksan sa Google Sheets**

Pagkatapos ng matagumpay na pag-import, maaaring palitan ng ZagoSheetsWin ang orihinal na lokal na file ng isang Internet shortcut (`.url`) patungo sa dokumento sa Google Sheets, habang nagtatago ng lokal na backup ng orihinal na file na maaaring maibalik.

Simple ang layunin: gawing parang karaniwang Windows app ang Google Sheets kapag nagbubukas ng lokal na spreadsheet.

## Ano ang ginagawa nito

Pinag-uugnay ng ZagoSheetsWin ang mga spreadsheet file sa Windows at Google Sheets. Kapag nagbukas ng suportadong file, maaari nitong:

- tukuyin at tiyaking wasto ang spreadsheet;
- gumawa ng backup na maaaring maibalik;
- direktang mag-upload sa Google Drive gamit ang mga opisyal na API ng Google;
- i-convert ang file sa katutubong Google Sheets na dokumento;
- buksan ang resulta sa default na browser;
- gumawa ng lokal na `.url` shortcut sa Google document;
- iwasan ang muling pag-upload ng parehong file sa mga susunod na pagbubukas.

Hindi na kailangang manu-manong mag-upload sa Drive, maghanap sa browser, o ulit-ulitin ang conversion.

## Mga suportadong format

Mga format na target ng kasalukuyang pag-develop: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

Maaaring may karagdagang limitasyon sa compatibility ang ilang format. Maingat na pinangangasiwaan ang mga file na may feature na hindi ligtas mapapanatili upang maiwasan ang tahimik na pagkawala ng data.

## Dinisenyo para sa Windows

Partikular na ginawa ang ZagoSheetsWin para sa Windows at sumusuporta sa **Open with (Buksan gamit ang)**, pagrerehistro ng uri ng file, pagbubukas sa double-click, opsyonal na integrasyon sa File Explorer, at native Windows installer.

Hindi nito palihim na binabago ang mga default app ng Windows. Ang user pa rin ang may kontrol sa file associations.

## Ligtas mula pa sa disenyo

Itinuturing ang pagpapalit ng lokal na file bilang operasyong maaaring maibalik. Bago alisin ang orihinal sa folder, tinitiyak ng app na:

1. may backup na maaaring maibalik;
2. matagumpay na nagawa ang Google Sheets document;
3. permanenteng naitala ang lokal na ugnayan ng file;
4. naisulat at napatunayang wasto ang Internet shortcut.

Kung mabigo ang proseso, mananatili ang orihinal na file.

> Huwag kailanman tahimik na sirain ang data ng user.

## Mga backup

Maaaring itago ang mga orihinal na file sa pribadong lokal na backup area bago palitan ng shortcut. Maaaring itakda ang limitasyon ng storage, panahon ng pagpapanatili, at paglilinis.

Pinoprotektahan ng backup ang orihinal na na-import na file. **Hindi ito two-way synchronization**: ang mga susunod na pagbabago sa Google Sheets ay hindi isinusulat pabalik sa orihinal na spreadsheet file.

## Privacy at access sa Google

Direktang nakikipag-ugnayan ang ZagoSheetsWin mula sa iyong computer sa mga API ng Google.

- Hindi ipinapadala ang nilalaman ng spreadsheet sa mga server ng Zagotools.
- Lokal na nakaimbak ang OAuth tokens at protektado ng mekanismo ng seguridad ng Windows.
- Ginagamit ng app ang `drive.file` permission scope upang limitahan ang access sa mga file na ginawa o binuksan sa app.
- Hindi kailangan ang analytics tracking para sa pag-import.

Patakaran sa Privacy at Mga Tuntunin ng Paggamit: https://zagotools.top/legal.html

## Katayuan ng proyekto

Patuloy na ginagawa ang ZagoSheetsWin at sa ngayon ay **alpha software**. Gumagana na ang pangunahing proseso ng Windows → Google Sheets at patuloy na pinapahusay ang pag-install, recovery, compatibility, internationalization, at user experience. Posibleng may pagbabago bago ang unang stable release.

## Ugnayan sa Open in Google

Ang ZagoSheetsWin ay hango sa [Open in Google](https://github.com/SwatiK425/open-in-google) ni [SwatiK425](https://github.com/SwatiK425), na nagbigay ng orihinal na pundasyon at inspirasyon.

Mula rito, naging hiwalay na Windows app ang ZagoSheetsWin na may sariling arkitektura, installer, interface, backup at recovery, file association, paghawak sa mga format, at proseso ng pagbubukas sa Google Sheets. Nanatiling hiwalay ang orihinal na proyekto at maaaring ibahagi rito ang mga pangkalahatang pagpapahusay kung naaangkop.

## Open source

Libre at open-source software ang ZagoSheetsWin. Pinananatili nito ang mga pagkilala at lisensya ng orihinal na Open in Google code at malinaw na tinutukoy ang mga sumunod na pagbabago ng ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lisensya

MIT License. Tingnan ang [LICENSE](../../../LICENSE) para sa detalye.

---

**ZagoSheetsWin — proyekto ng Zagotools**

Maliit na software para sa tunay na problema.
