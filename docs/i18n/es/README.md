# ZagoSheetsWin

> ⚠️ **0.9.35 — Important safety update (English notice; full translation review pending).**
>
> La hoja se abrió en Google Sheets. El archivo original se sustituyó por un acceso directo. Para recuperarlo, abre Copias de seguridad.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.35 behavior.


<details>
<summary>🌐 Idiomas de documentación · Elegir idioma</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- **ES - Español / Spanish (es)** — página actual
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

**Abre archivos de hojas de cálculo locales directamente en Hojas de cálculo de Google desde Windows.**

ZagoSheetsWin es una aplicación ligera para Windows que convierte la apertura de una hoja de cálculo local en un proceso sencillo:

**doble clic en el archivo → subir y convertir → abrir en Hojas de cálculo de Google**

Después de una importación correcta, ZagoSheetsWin puede sustituir el archivo local original por un acceso directo de Internet (`.url`) que apunta al documento de Google, conservando una copia de seguridad local recuperable del archivo original.

El objetivo es sencillo: hacer que Hojas de cálculo de Google se sienta como una aplicación nativa de Windows al abrir archivos locales.

## Qué hace

ZagoSheetsWin integra los archivos de hojas de cálculo con Windows y Hojas de cálculo de Google.

Al abrir un archivo local compatible, la aplicación puede:

- detectar y validar la hoja de cálculo;
- crear una copia de seguridad recuperable;
- subir el archivo directamente a Google Drive mediante las API oficiales de Google;
- convertirlo en un documento nativo de Hojas de cálculo de Google;
- abrir la hoja resultante en tu navegador predeterminado;
- crear un acceso directo local `.url` al documento de Google;
- evitar volver a subir el mismo archivo al abrirlo posteriormente.

Sin subir archivos manualmente a Drive. Sin navegar por el navegador. Sin repetir la conversión.

## Formatos compatibles

Formatos previstos en el desarrollo actual:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Algunos formatos pueden tener restricciones adicionales de compatibilidad. Los archivos con funciones que no pueden conservarse de forma segura se tratan con precaución para evitar pérdidas de datos silenciosas.

## Diseñado para Windows

ZagoSheetsWin está desarrollado específicamente para Windows y se integra con el sistema operativo mediante:

- **Abrir con**
- registro de tipos de archivo
- apertura de archivos con doble clic
- integración opcional con el Explorador de archivos
- instalador nativo de Windows

La aplicación no cambia silenciosamente las aplicaciones predeterminadas de Windows. El usuario mantiene el control de las asociaciones de archivos.

## Seguridad desde el diseño

ZagoSheetsWin trata la sustitución de un archivo local como una operación recuperable.

Antes de retirar un archivo original de su carpeta, la aplicación verifica que:

1. existe una copia de seguridad recuperable;
2. el documento de Hojas de cálculo de Google se creó correctamente;
3. la asociación local quedó guardada de forma persistente;
4. el acceso directo de Internet se escribió y validó correctamente.

Si el proceso falla, se conserva el archivo original.

La aplicación sigue una regla sencilla:

> Nunca destruir datos del usuario sin avisar.

## Copias de seguridad

Los archivos originales pueden guardarse en un área privada de copias de seguridad locales antes de sustituirse por accesos directos.

La gestión de copias de seguridad incluye límites de almacenamiento configurables, políticas de conservación y controles de limpieza.

Las copias protegen el archivo original importado. **No constituyen un sistema de sincronización bidireccional**: los cambios realizados posteriormente en Hojas de cálculo de Google no se escriben en el archivo original.

## Privacidad y acceso a Google

ZagoSheetsWin se comunica directamente con las API de Google desde tu ordenador.

- El contenido de tus hojas de cálculo no se envía a ningún servidor de Zagotools.
- Los tokens OAuth se almacenan localmente y se protegen mediante mecanismos de seguridad de Windows.
- La aplicación utiliza el permiso `drive.file` de Google Drive, que limita el acceso a los archivos creados o abiertos mediante la aplicación.
- No se necesitan herramientas de análisis para el proceso de importación.

Política de privacidad y condiciones de uso:

https://zagotools.top/legal.html

## Estado del proyecto

ZagoSheetsWin está en desarrollo activo y debe considerarse **software en fase alfa**.

El flujo principal de Windows → Hojas de cálculo de Google funciona y sigue mejorando en instalación, recuperación, compatibilidad de formatos, internacionalización y experiencia de usuario.

Es posible que haya cambios hasta el lanzamiento de la primera versión estable.

## Relación con Open in Google

ZagoSheetsWin se basa en [Open in Google](https://github.com/SwatiK425/open-in-google), de [SwatiK425](https://github.com/SwatiK425), y deriva de ese proyecto.

Open in Google proporcionó los fundamentos y la inspiración originales del proyecto.

Desde entonces, ZagoSheetsWin se ha convertido en una aplicación independiente para Windows, con arquitectura, instalador, interfaz, sistema de copias de seguridad y recuperación, asociaciones de archivos, gestión de formatos y experiencia de apertura de archivos locales en Hojas de cálculo de Google propios.

El proyecto original sigue siendo independiente. Cuando corresponda, podrán proponerse mejoras generales al proyecto original, mientras ZagoSheetsWin continúa evolucionando por separado.

## Código abierto

ZagoSheetsWin es software gratuito y de código abierto.

El proyecto conserva las atribuciones y las condiciones de licencia del código original de Open in Google, e identifica claramente los desarrollos posteriores de ZagoSheetsWin / Zagotools.

Consulta:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licencia

Licencia MIT.

Consulta los detalles en [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — un proyecto de Zagotools**

Software pequeño para problemas reales.
