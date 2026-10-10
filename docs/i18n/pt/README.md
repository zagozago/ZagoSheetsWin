# ZagoSheetsWin

<details>
<summary>🌐 Idiomas da documentação · Escolha seu idioma</summary>

- [English](../../../README.md)
- [简体中文](../zh/README.md)
- [हिन्दी](../hi/README.md)
- [Español](../es/README.md)
- [العربية](../ar/README.md)
- [Français](../fr/README.md)
- [বাংলা](../bn/README.md)
- **Português (Brasil)** — página atual
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
