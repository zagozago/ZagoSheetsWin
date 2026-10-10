# ZagoSheetsWin

<details>
<summary>🌐 Dokumentationssprachen · Sprache wählen</summary>

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
- **Deutsch** — aktuelle Seite
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

</details>

**Lokale Tabellenkalkulationsdateien unter Windows direkt in Google Tabellen öffnen.**

ZagoSheetsWin ist eine schlanke Windows-Anwendung, die das Öffnen lokaler Tabellen vereinfacht:

**Datei doppelt anklicken → hochladen und konvertieren → in Google Tabellen öffnen**

Nach einem erfolgreichen Import kann ZagoSheetsWin die ursprüngliche lokale Datei durch eine Internetverknüpfung (`.url`) zum Google-Tabellen-Dokument ersetzen. Gleichzeitig bleibt eine wiederherstellbare lokale Sicherung der Originaldatei erhalten.

Das Ziel ist einfach: Google Tabellen soll sich beim Öffnen lokaler Tabellen wie eine native Windows-Anwendung anfühlen.

## Funktionen

ZagoSheetsWin verbindet Tabellenkalkulationsdateien unter Windows mit Google Tabellen.

Beim Öffnen einer unterstützten lokalen Datei kann die Anwendung:

- die Tabelle erkennen und prüfen;
- eine wiederherstellbare Sicherung erstellen;
- die Datei über die offiziellen Google-APIs direkt nach Google Drive hochladen;
- sie in ein natives Google-Tabellen-Dokument umwandeln;
- die resultierende Tabelle im Standardbrowser öffnen;
- eine lokale `.url`-Verknüpfung zum Google-Dokument erstellen;
- beim nächsten Öffnen derselben Datei einen erneuten Upload vermeiden.

Kein manueller Drive-Upload. Keine Suche im Browser. Keine wiederholte Konvertierung.

## Unterstützte Formate

Derzeit vorgesehene Dateiformate:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Für einige Formate können zusätzliche Kompatibilitätseinschränkungen gelten. Dateien mit Funktionen, die nicht sicher erhalten werden können, werden vorsichtig behandelt, um unbemerkten Datenverlust zu vermeiden.

## Für Windows entwickelt

ZagoSheetsWin wurde speziell für Windows entwickelt und integriert sich über folgende Funktionen ins Betriebssystem:

- **Öffnen mit**
- Registrierung von Dateitypen
- Öffnen per Doppelklick
- optionale Integration in den Datei-Explorer
- natives Windows-Installationsprogramm

Die Anwendung ändert die Windows-Standardprogramme nicht heimlich. Die Zuordnung von Dateitypen bleibt unter der Kontrolle der Benutzer.

## Sicherheit von Anfang an

ZagoSheetsWin behandelt das Ersetzen einer lokalen Datei als wiederherstellbaren Vorgang.

Bevor die Originaldatei aus ihrem Ordner entfernt wird, prüft die Anwendung:

1. ob eine wiederherstellbare Sicherung vorhanden ist;
2. ob das Google-Tabellen-Dokument erfolgreich erstellt wurde;
3. ob die lokale Zuordnung dauerhaft gespeichert wurde;
4. ob die Internetverknüpfung erfolgreich geschrieben und geprüft wurde.

Schlägt der Vorgang fehl, bleibt die Originaldatei erhalten.

Die Anwendung folgt einer einfachen Regel:

> Benutzerdaten niemals unbemerkt zerstören.

## Sicherungen

Originaldateien können vor dem Ersetzen durch Verknüpfungen in einem privaten lokalen Sicherungsbereich aufbewahrt werden.

Die Sicherungsverwaltung bietet einstellbare Speichergrenzen, Aufbewahrungsrichtlinien und Bereinigungsfunktionen.

Sicherungen schützen die ursprünglich importierte Datei. Sie sind **keine bidirektionale Synchronisierung**: Änderungen, die später in Google Tabellen vorgenommen werden, werden nicht in die ursprüngliche Tabellenkalkulationsdatei zurückgeschrieben.

## Datenschutz und Google-Zugriff

ZagoSheetsWin kommuniziert von Ihrem Computer aus direkt mit den Google-APIs.

- Die Inhalte Ihrer Tabellen werden nicht an Server von Zagotools gesendet.
- OAuth-Tokens werden lokal gespeichert und durch Windows-Sicherheitsmechanismen geschützt.
- Die Anwendung nutzt den Google-Drive-Berechtigungsumfang `drive.file` und begrenzt so den Zugriff auf Dateien, die über die Anwendung erstellt oder geöffnet wurden.
- Für den Tabellenimport ist keine Nutzungsanalyse erforderlich.

Datenschutzerklärung und Nutzungsbedingungen:

https://zagotools.top/legal.html

## Projektstatus

ZagoSheetsWin wird aktiv weiterentwickelt und sollte derzeit als **Alpha-Software** betrachtet werden.

Der grundlegende Ablauf Windows → Google Tabellen funktioniert bereits. Installation, Wiederherstellung, Formatkompatibilität, Internationalisierung und Benutzererlebnis werden weiter verbessert.

Bis zur ersten stabilen Version sind Änderungen zu erwarten.

## Beziehung zu Open in Google

ZagoSheetsWin basiert auf [Open in Google](https://github.com/SwatiK425/open-in-google) von [SwatiK425](https://github.com/SwatiK425) und wurde daraus weiterentwickelt.

Open in Google lieferte die ursprüngliche Grundlage und Inspiration für das Projekt.

Seitdem hat sich ZagoSheetsWin zu einer eigenständigen Windows-Anwendung mit eigener Architektur, eigenem Installer und Benutzeroberfläche, Sicherungs- und Wiederherstellungssystem, Dateizuordnungen, Formatverarbeitung und lokalem Google-Tabellen-Arbeitsablauf entwickelt.

Das ursprüngliche Projekt bleibt unabhängig. Allgemeine Verbesserungen können gegebenenfalls an das Ursprungsprojekt zurückgegeben werden, während ZagoSheetsWin eigenständig weiterentwickelt wird.

## Open Source

ZagoSheetsWin ist kostenlose Open-Source-Software.

Das Projekt wahrt die Urheberhinweise und Lizenzbedingungen des ursprünglichen Open-in-Google-Codes und kennzeichnet die spätere Entwicklung durch ZagoSheetsWin / Zagotools eindeutig.

Siehe:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lizenz

MIT-Lizenz.

Einzelheiten finden Sie in [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — ein Projekt von Zagotools**

Kleine Software für echte Probleme.
