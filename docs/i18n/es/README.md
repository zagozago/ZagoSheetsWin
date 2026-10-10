# ZagoSheetsWin

<details>
<summary>🌐 Idiomas de documentación · Elegir idioma</summary>

- [English](../../../README.md)
- [简体中文](../zh/README.md)
- [हिन्दी](../hi/README.md)
- **Español** — página actual
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
- [粵語](../yue/README.md)
- [فارسی](../fa/README.md)
- [አማርኛ](../am/README.md)
- [Basa Jawa](../jv/README.md)
- [ગુજરાતી](../gu/README.md)

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
