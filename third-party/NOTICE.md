# Licenças de terceiros - ZagoSheetsWin 0.9.25

Mensagens padrão do instalador em chinês simplificado adaptadas da tradução
de Zhenghan Yang (Kira), copyright 2019-2020 kirakira, sob MIT.
Origem: https://github.com/kira-96/Inno-Setup-Chinese-Simplified-Translation
Licença incluída em `Inno-ChineseSimplified-LICENSE.txt`.

O aplicativo e a atribuição original ao Open in Google mantêm a licença MIT.
Cada dependência mantém sua própria licença. O NPOI está fixado na versão
2.7.6; não se deve presumir que pacotes binários mais recentes sejam Apache-2.0.

| Componente | Versão | Licença / texto incluído |
|---|---|---|
| HtmlAgilityPack (ZZZ Projects, Simon Mourrier e colaboradores) | 1.12.4 | MIT - HtmlAgilityPack-LICENSE.txt |
| ExcelDataReader | 3.9.0 | MIT - ExcelDataReader-LICENSE.txt |
| NPOI (Tony Qu, colaboradores do NPOI, Nissl LLC) | 2.7.6 | Apache-2.0 - NPOI-LICENSE.txt |
| BouncyCastle.Cryptography | 2.6.2 | MIT - BouncyCastle-LICENSE.txt |
| Enums.NET | 5.0.0 | MIT - Enums.NET-LICENSE.txt |
| ExtendedNumerics.BigDecimal | 2025.1001.2.129 | MIT - BigDecimal-LICENSE.txt |
| MathNet.Numerics.Signed | 5.0.0 | MIT - MathNet-LICENSE.txt |
| Microsoft.IO.RecyclableMemoryStream | 3.0.1 | MIT - RecyclableMemoryStream-LICENSE.txt |
| NSax | 1.0.2 | LGPL-3.0-only - NSax-LICENSE.txt e GPL-3.0.txt |
| SharpZipLib | 1.4.2 | MIT - SharpZipLib-LICENSE.txt |
| SixLabors.Fonts | 1.0.1 | Apache-2.0 - Fonts-LICENSE.txt |
| SixLabors.ImageSharp | 4.1.3 | Six Labors Split License (concessão Apache-2.0) - ImageSharp-LICENSE.txt |
| ZString | 2.6.0 | MIT - ZString-LICENSE.txt |

O NPOI deriva do Apache POI (Apache Software Foundation), licenciado sob
Apache-2.0. Código-fonte: https://github.com/nissl-lab/npoi.

NSax é uma dependência sem modificações, distribuída em uma NSax.dll separada.
Seu uso é coberto pela LGPL-3.0-only. Código-fonte correspondente ao NuGet 1.0.2:
https://github.com/antony-liu/NSax/tree/b75861cbc49be1ce4b02410e324b5755ffcb17a2

Os usuários podem modificar e recompilar o NSax e substituir sua DLL por uma
versão com interface compatível. Este aplicativo não restringe essa modificação
nem a engenharia reversa para depurá-la. O instalador não incorpora a DLL ao
executável. As instruções de compilação estão disponíveis no repositório NSax.

System.Security.Cryptography.Xml foi atualizado explicitamente para 10.0.12
para evitar vulnerabilidades da dependência padrão antiga do NPOI. Ele e as
outras bibliotecas de execução da Microsoft mantêm seus avisos MIT originais
(consulte ThirdPartyNotices.txt do .NET incluído nas instalações autossuficientes).

ImageSharp 4.1.3: distribuição MIT enquadrada na concessão Apache-2.0
para software open source/source available, conforme LICENSE do mantenedor.
A compilação requer chave Six Labors válida para SheetsWindows.Infrastructure.
A licença comunitária limitada ao assembly está em sixlabors.lic, conforme
a autorização do fornecedor. Válida até 2028-01-08.
