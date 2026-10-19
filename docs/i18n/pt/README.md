# ZagoSheetsWin

> ⚠️ **Alfa 0.9.42 — atualização importante; conteúdo completo ainda em revisão.**
>
> Planilha aberta no Google Sheets. O original foi substituído por um atalho. Para recuperá-lo, abra Backups.
>
> **CSV/TSV v4:** até **2.000.000 de células**, **100.000 registros lógicos**, 1.000 colunas e 20 MiB, sujeitos a outros limites. Ambiguidade de separador apresenta prévia local. Se a conferência dos valores convertidos no Google encontrar divergências, **o original não deve ser substituído por um atalho**.
>
> **XLS/ODS:** recursos complexos podem perder fidelidade e, ainda assim, o original **pode** ser substituído após importação confirmada. O backup mantém os bytes originais. Se quiser conservar o arquivo na pasta, escolha **Importar como cópia** antes de abrir.
>
> **Backups:** limpeza por prazo e cota **automática e sem toggle**. Backups concluídos elegíveis podem ser removidos antes de vencer o prazo para liberar espaço. Arquivos diferentes podem importar em paralelo; somente falta real de espaço gera espera cancelável.
>
> **Estado da versão:** 0.9.42 **não é estável**. O último CI Windows e a distribuição protegida anterior tiveram falhas; a homologação com Google real permanece pendente. O restante desta página pode descrever versões alfa antigas. Consulte o [README canônico](../../../README.md) ou o [manual atual](../../USER_GUIDE.md).

<details>
<summary>🌐 Idiomas da documentação · Escolha seu idioma</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- [FR - Français / French (fr)](../fr/README.md)
- [BN - বাংলা / Bengali (bn)](../bn/README.md)
- **PT - Português / Portuguese (pt)** — página atual
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

**Abra arquivos de planilhas locais diretamente no Google Sheets pelo Windows.**

O ZagoSheetsWin é um aplicativo leve para Windows que simplifica a abertura de planilhas locais:

**dois cliques no arquivo → upload e conversão → abertura no Google Sheets**

Após uma importação bem-sucedida, o ZagoSheetsWin pode substituir o arquivo local original por um atalho de Internet (`.url`) apontando para o documento no Google Sheets, mantendo um backup local recuperável do arquivo original.

O objetivo é simples: fazer com que o Google Sheets funcione, na prática, como um aplicativo nativo do Windows para abrir arquivos de planilhas locais.

## O que ele faz

O ZagoSheetsWin integra arquivos de planilhas do Windows ao Google Sheets.

Ao abrir um arquivo local compatível, o aplicativo pode:

- identificar e validar a planilha;
- criar um backup recuperável;
- enviar o arquivo diretamente ao Google Drive pelas APIs oficiais do Google;
- convertê-lo em um documento nativo do Google Sheets;
- abrir a planilha resultante no navegador padrão;
- criar um atalho `.url` local para o documento no Google;
- evitar novos uploads do mesmo arquivo nas próximas aberturas.

Sem upload manual pelo Drive. Sem precisar navegar pelo navegador. Sem repetir o processo de conversão.

## Formatos compatíveis

Formatos previstos no desenvolvimento atual:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Alguns formatos podem ter restrições adicionais de compatibilidade. Arquivos com recursos que não podem ser preservados com segurança são tratados de maneira conservadora para evitar perda silenciosa de dados.

## Feito para Windows

O ZagoSheetsWin foi desenvolvido especificamente para Windows e se integra ao sistema operacional por meio de:

- **Abrir com**
- registro de tipos de arquivo
- abertura de arquivos com dois cliques
- integração opcional ao Explorador de Arquivos
- instalador nativo do Windows

O aplicativo não altera silenciosamente os programas padrão do Windows. O controle das associações de arquivos continua com o usuário.

## Segurança desde o início

O ZagoSheetsWin trata a substituição de um arquivo local como uma operação recuperável.

Antes de retirar um arquivo original de sua pasta, o aplicativo verifica se:

1. existe um backup recuperável;
2. o documento do Google Sheets foi criado com sucesso;
3. a associação local foi salva de forma persistente;
4. o atalho de Internet foi gravado e validado corretamente.

Se o processo falhar, o arquivo original é preservado.

O aplicativo segue uma regra simples:

> Nunca destruir os dados do usuário silenciosamente.

## Backups

Os arquivos originais podem ser armazenados em uma área privada de backup local antes de serem substituídos por atalhos.

O gerenciamento de backups inclui limites configuráveis de armazenamento, políticas de retenção e controles de limpeza.

Os backups protegem o arquivo original importado. Eles **não constituem sincronização bidirecional**: alterações feitas posteriormente no Google Sheets não são gravadas de volta no arquivo de planilha original.

## Privacidade e acesso ao Google

O ZagoSheetsWin se comunica diretamente do seu computador com as APIs do Google.

- O conteúdo das suas planilhas não é enviado a servidores da Zagotools.
- Os tokens OAuth são armazenados localmente e protegidos pelos mecanismos de segurança do Windows.
- O aplicativo usa o escopo `drive.file` do Google Drive, limitando o acesso aos arquivos criados ou abertos por meio do aplicativo.
- A importação de planilhas não depende de ferramentas de análise de uso.

Política de Privacidade e Termos de Uso:

https://zagotools.top/legal.html

## Estado do projeto

O ZagoSheetsWin está em desenvolvimento ativo e deve ser considerado **um software em fase alfa**.

O fluxo principal Windows → Google Sheets está funcional e continua recebendo melhorias na instalação, recuperação, compatibilidade de formatos, internacionalização e experiência de uso.

Mudanças são esperadas até o lançamento da primeira versão estável.

## Relação com o Open in Google

O ZagoSheetsWin é baseado no projeto [Open in Google](https://github.com/SwatiK425/open-in-google), de [SwatiK425](https://github.com/SwatiK425), e deriva dele.

O Open in Google forneceu a base original e a inspiração para este projeto.

Desde então, o ZagoSheetsWin evoluiu para um aplicativo Windows independente, com arquitetura, instalador, interface, sistema de backup e recuperação, fluxo de associação de arquivos, tratamento de formatos e experiência de abertura de planilhas locais no Google Sheets próprios.

O projeto original permanece independente. Melhorias genéricas poderão ser propostas ao projeto original quando fizer sentido, enquanto o ZagoSheetsWin continua evoluindo de forma independente.

## Código aberto

O ZagoSheetsWin é um software gratuito e de código aberto.

O projeto preserva os créditos e requisitos de licença do código original do Open in Google, identificando claramente as evoluções posteriores do ZagoSheetsWin / Zagotools.

Consulte:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licença

Licença MIT.

Consulte os detalhes em [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — um projeto Zagotools**

Software pequeno para problemas reais.
