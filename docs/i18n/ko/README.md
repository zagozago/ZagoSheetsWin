# ZagoSheetsWin

<details>
<summary>🌐 문서 언어 · 언어 선택</summary>

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
- **한국어** — 현재 페이지
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

</details>

**Windows의 로컬 스프레드시트 파일을 Google 스프레드시트에서 바로 여세요.**

ZagoSheetsWin은 로컬 스프레드시트를 간단하게 여는 가벼운 Windows 애플리케이션입니다.

**파일 더블클릭 → 업로드 및 변환 → Google 스프레드시트에서 열기**

가져오기에 성공하면 원본 파일을 Google 스프레드시트로 연결되는 인터넷 바로 가기(`.url`)로 교체하면서 복구 가능한 원본 백업을 로컬에 보관할 수 있습니다.

목표는 로컬 스프레드시트 파일을 Google 스프레드시트에서 여는 과정이 Windows 기본 앱처럼 자연스럽게 느껴지도록 하는 것입니다.

## 주요 기능

ZagoSheetsWin은 Windows 파일과 Google 스프레드시트를 연결합니다. 지원되는 파일을 열면 다음 작업을 수행할 수 있습니다.

- 파일을 인식하고 검증
- 복구 가능한 백업 생성
- Google 공식 API를 통해 Google Drive로 직접 업로드
- Google 스프레드시트 기본 문서로 변환
- 기본 브라우저에서 결과 열기
- Google 문서를 가리키는 로컬 `.url` 바로 가기 생성
- 이후 같은 파일을 열 때 중복 업로드 방지

Drive에 수동으로 올리거나 브라우저에서 파일을 찾거나 변환을 반복할 필요가 없습니다.

## 지원 형식

현재 개발 대상: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

일부 형식에는 호환성 제한이 있습니다. 안전하게 보존할 수 없는 기능이 포함된 파일은 데이터가 사용자 몰래 손실되지 않도록 신중하게 처리합니다.

## Windows 전용 설계

ZagoSheetsWin은 Windows용으로 개발되었으며 **연결 프로그램(Open with)**, 파일 형식 등록, 더블클릭 열기, 선택적 파일 탐색기 연동, Windows 기본 설치 프로그램을 지원합니다.

사용자 몰래 Windows 기본 앱을 변경하지 않습니다. 파일 연결은 사용자가 제어합니다.

## 안전을 고려한 설계

로컬 파일 교체는 복구 가능한 작업으로 취급합니다. 원본 파일을 폴더에서 제거하기 전에 확인하는 항목은 다음과 같습니다.

1. 복구 가능한 백업이 존재함
2. Google 스프레드시트 문서가 정상적으로 생성됨
3. 로컬 연결 정보가 영구 저장됨
4. 인터넷 바로 가기 작성과 검증이 완료됨

작업에 실패하면 원본 파일은 그대로 보존됩니다.

> 사용자 모르게 데이터를 절대 삭제하지 않습니다.

## 백업

원본 파일을 바로 가기로 교체하기 전에 개인 로컬 백업 공간에 보관할 수 있습니다. 백업의 저장 용량 한도, 보관 기간, 정리 기능을 설정할 수 있습니다.

백업은 최초로 가져온 원본 파일을 보호합니다. **양방향 동기화가 아닙니다.** 이후 Google 스프레드시트에서 편집한 내용은 원본 파일로 다시 기록되지 않습니다.

## 개인정보 보호 및 Google 접근

ZagoSheetsWin은 PC에서 Google API와 직접 통신합니다.

- 스프레드시트 내용이 Zagotools 서버로 전송되지 않습니다.
- OAuth 토큰은 Windows 보안 기능으로 보호하여 로컬에 저장합니다.
- `drive.file` 권한 범위는 앱으로 생성하거나 연 파일로 접근을 제한합니다.
- 가져오기 과정에 분석 추적이 필요하지 않습니다.

개인정보 처리방침 및 이용 약관: https://zagotools.top/legal.html

## 프로젝트 상태

ZagoSheetsWin은 현재 개발 중인 **알파 소프트웨어**입니다. Windows → Google 스프레드시트 핵심 흐름은 작동하며 설치, 복구, 형식 호환성, 다국어 지원, 사용자 경험을 계속 개선하고 있습니다. 첫 안정 버전 출시 전 변경될 수 있습니다.

## Open in Google과의 관계

ZagoSheetsWin은 [SwatiK425](https://github.com/SwatiK425)의 [Open in Google](https://github.com/SwatiK425/open-in-google)에서 파생된 프로젝트입니다. 원본은 초기 기반과 영감을 제공했습니다.

이후 ZagoSheetsWin은 고유한 아키텍처, 설치 프로그램, 사용자 인터페이스, 백업 및 복구, 파일 연결, 형식 처리, 로컬 파일을 Google 스프레드시트로 여는 경험을 갖춘 독립 Windows 앱으로 발전했습니다. 원본 프로젝트는 독립적이며 범용 개선 사항은 필요에 따라 upstream에 기여할 수 있습니다.

## 오픈 소스

ZagoSheetsWin은 무료 오픈 소스 소프트웨어입니다. 원본 Open in Google 코드의 저작권 표시와 라이선스 조건을 유지하고, ZagoSheetsWin / Zagotools의 후속 개발을 구분합니다.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## 라이선스

MIT 라이선스. 자세한 내용은 [LICENSE](../../../LICENSE)를 참고하세요.

---

**ZagoSheetsWin — Zagotools 프로젝트**

실제 문제를 해결하는 작은 소프트웨어.
