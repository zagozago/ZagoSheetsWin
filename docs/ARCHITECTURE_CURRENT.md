# Arquitetura atual do ZagoSheetsWin

**Base:** `feature/zagosheetswin`, candidato **0.9.37** em 18/10/2026 ([commit e evidências](RELEASE_STATUS.md)). Descrição da **implementação**, não do arquivo histórico [ARCHITECTURE.md](ARCHITECTURE.md), que nasceu como arquitetura-alvo. Não representa auditoria formal de segurança.

## Organização do código

| Diretório ou módulo | Responsabilidade |
|---|---|
| `src/SheetsWindows.Core/` | Modelos `ImportOperation`, `Snapshot`, `SourceDescriptor`; interfaces e preparação local |
| `src/SheetsWindows.Infrastructure/` | Drive, OAuth, backup, banco SQLite, upload retomável, `ImportInterpretations` (interpretação versionada), `BackupRepairSource` (reparo a partir de snapshot), formatos, integração Windows, logs e recuperação |
| `src/SheetsWindows.Windows/` | Formulários WinForms: início, primeiro uso, configurações, opções avançadas, `TextInterpretationForm` contextual, processamento/erros persistentes, recuperação, tutorial e seletor de idiomas |
| `src/SheetsWindows.Cli/` | Entrada CLI auxiliar para operações do fluxo |
| `tests/SheetsWindows.Tests/` | Testes automatizados de núcleo, Drive simulado, recuperação, formatos, UI e integração Windows |
| `installer/SheetsWindows.iss` | Instalação/desinstalação por usuário com Inno Setup 6 |
| `i18n/source.json`, `i18n/packs/`, `i18n/generated/` | Catálogos-fonte, traduções e recursos incorporados |
| `.github/workflows/` | Testes offline, publicação Windows e distribuição alfa protegida |

Tecnologia: C# / .NET 10, WinForms, SQLite e APIs oficiais do Google. O executável principal permanece `SheetsWindows.exe`, com nome de produto **ZagoSheetsWin**. O SDK base é selecionado por `global.json` (10.0.401 com roll-forward de patch); a distribuição protegida especifica SDK 10.0.402.

## Diagrama de responsabilidade

```text
Windows Explorer / Home / CLI
           |
           v
WindowsLauncher + ProcessingForm
     |        |        |
     |     GoogleOAuth (PKCE, DPAPI)
     |        |
     v        v
LocalPreparation --> GoogleImport --> GoogleDriveClient --> Google Drive API
  |  |                    |                   |
  |  +-- BackupStore      +-- GoogleRemoteRegistry
  +----- SqliteOperationRegistry
                             |
                        ConversionVerifier
                             |
                  ReplacementCoordinator / PublishCopyAsync
                      |            |
                ReplacementJournal   InternetShortcut (.url)
                      |
               WindowsRetirementReader (fonte elegível)
```

## Fluxo de importação e substituição

1. Interpretar caminho e extensão suportada; verificar a política de abertura e ambiente da fonte.
2. Validar leitura, autorização da conta Google e eventual operação anterior.
3. Preparar um snapshot consistente, salvar backup e SHA-256, com lock da identidade de origem.
4. Normalizar o formato quando exigido e registrar a **intenção remota** antes de criar o documento.
5. Para CSV/TSV, **fixar a versão e opções de interpretação por operação** antes da intenção remota; ambiguidades novas usam escolha contextual. Fazer upload/retomada de sessão; confirmar ID, tipo Google Sheets, permissão, estado da lixeira e marcadores `sw_operation` / `sw_hash`. Associação não é escolhida por nome.
6. Conferir as células suportadas, quando possível. **Desde 0.9.33, diferença de valores ou verificação incompleta é advertência, não veto automático**. Falhas de integridade, importação ou vínculo continuam bloqueando.
7. Para substituição elegível: verificar backup, publicar o atalho sem sobrescrever outro arquivo, solicitar abertura do navegador e só então retirar o arquivo original sob handle Windows que evita corrida por nome/caminho.
8. Persistir cada transição no journal. Se uma etapa falhar, preservar representação recuperável; uma criação remota incerta exige reconciliação, não repetição cega.
9. Se a importação for em modo de cópia: manter a fonte. Com atalhos habilitados, publicar atalho na pasta privada do aplicativo.
10. Após substituição concluída, a interface apresenta confirmação com acesso a Backups e opção persistente de ocultação. Uma proteção por mutex impede avisos simultâneos; a confirmação **ocorre após a retirada**, não antes.

**Reabrir `.url` não chama o importador**: o Windows abre diretamente a URL HTTPS gravada. Não há sincronização reversa nem atualização remota silenciosa de arquivo conhecido alterado.

## Interpretação de texto e reparo sem alterar documento anterior (0.9.36)

A classe `SpreadsheetFormats` diferencia **v1 e v2** para CSV/TSV. A v2 identifica vírgula, ponto e vírgula, TAB ou pipe, oferece fallback CP1252 limitado e usa limites de 60.000 registros/600.000 células. A classe `ImportInterpretations` vincula a versão e `TextImportOptions` à identidade local da operação em SQLite; operações antigas com intenção remota conservam v1. `ConversionVerifier` recupera a interpretação da operação antes de comparar valores exportados.

O reparo explícito usa `BackupRepairSource` para ler **bytes de backup íntegros** com identidade de operação nova (`repair:<uuid>`). `WindowsLauncher.RepairAsync` envia para um documento Google novo via `GoogleImport` e publica resultado como **cópia**, sem alterar o Google Sheets, atalho ou journal anterior. Locks, hash SHA-256 e disponibilidade do backup continuam necessários.

Para erro permanente de leitura, `ImportRetry.RequiresInputChange` impede retries automáticos e a janela de erro habilita nova tentativa após mudança observável de tamanho/mtime da fonte. Sem intenção remota anterior, `GoogleImport` pode aposentar associação local não enviada; havendo intenção remota, requer reconciliação antes de reinterpretação. O estado terminal em `ProcessingForm` não é substituído por atualizações atrasadas, e a janela recalcula altura ao voltar de minimizada.

A exportação `DiagnosticLog.ExportAsync(..., currentAttempt)` produz registros da tentativa atual por `AttemptId`; o histórico geral segue ação separada em Backups. Novos códigos diferenciam limites de texto, ODS e tamanho remoto, sem expor células nem tokens.

## Estados persistentes e armazenamento

Estado por usuário em **`%LOCALAPPDATA%\SheetsWindows`**, separado do executável em **`%LOCALAPPDATA%\Programs\SheetsWindows`**.

| Arquivo/pasta | Finalidade |
|---|---|
| `registry.db` | Operações locais, snapshots, aliases e journal |
| `google.db` | Tentativas e associações remotas, IDs e marcadores |
| `replacement.db` | Transições de publicação e retirada do original |
| `backups/` | Snapshots dos bytes originais |
| `auth/` | Tokens Google criptografados por DPAPI CurrentUser |
| `uploads/` | Dados de sessão resumable protegidos por DPAPI |
| `shortcuts/` | Atalhos de importações como cópia |
| `locks/` | Arquivos de lock para concorrência |
| `logs/` | Eventos JSONL rotacionados e limitados |
| `opening-policy.json`, `backup-policy.json`, `formats.json`, `language.json` e outras preferências | Configurações validadas e persistidas |

O banco SQLite registra operações locais e remotas em estruturas separadas. **Não existe transação atômica única envolvendo Drive, SQLite e o filesystem do Windows**. O journal e a reconciliação compensam falhas entre etapas. A desinstalação conserva o estado do usuário.

## Controles de dados e segurança

- OAuth authorization code desktop com PKCE S256, `state`, retorno loopback em 127.0.0.1 e scope `drive.file`. Não obtém senha Google.
- Tokens de acesso/atualização e URLs de sessão protegidos por DPAPI. Backup local usa ACL e não tem criptografia própria de conteúdo.
- Snapshot e verificação de hash SHA-256, bloqueio por identidade local e validação do destino HTTPS Google Sheets para atalhos.
- Upload resumable com offsets validados, limite por prazo de operação e reconciliação por marcador. POST de criação não é repetido cegamente.
- Fonte de rede/sincronizada não é retirada. **Conversão com equivalência parcial deixa de forçar cópia** após importação confirmada; o backup deve permanecer íntegro. Falhas estruturais, remotas, de hash e de retirada continuam bloqueando. Atributo Windows somente leitura pode impedir a retirada e exige recuperação sem alterar o original.
- Diagnósticos sanitizados, no máximo quatro arquivos de 64 KiB e até 96 checkpoints por tentativa; nome sugerido de exportação pode conter basename sanitizado, mas os campos do JSONL não contêm nomes de documentos.
- Nenhuma afirmação de certificação de segurança, assinatura Authenticode ou Google OAuth publicamente verificado sem evidência externa.

## Interface e internacionalização

`FirstUseWizard` tem três páginas. `TutorialForm` tem quatro páginas. `SetupForm` contém conexão Google, padrões Windows e opções usuais; `AdvancedPreferencesForm` contém cliente Google, escopo de pasta/sincronização, consentimento de substituição e regra XLS. **A seção global Formatos e texto foi retirada em 0.9.32**, mantendo configurações existentes; o `TextInterpretationForm` da 0.9.36 é contextual à importação/reparo. `RecoveryForm` salva políticas sem botão Salvar, usa seleção direta de linhas e ordena Arquivo/Data/Tamanho/Estado. `ProcessingForm` exibe a confirmação após substituição e a opção persistente **Não mostrar novamente**. O botão Ajuda inicial abre uma URL Zagotools ainda não comprovada como publicada. Temas claro (padrão)/escuro. Catálogo de 51 idiomas gerado a partir de fonte e ordem T51; RTL e fontes alternativas dependem também das fontes instaladas no Windows.

## Identidade visual (0.9.37)

`branding/zagosheetswin.ico` foi substituído pela arte de planilha já usada por `branding/sheet-shortcut.ico`, preservando frames de 16 a 256 px. `branding/app-icon-source.png` registra o raster correspondente. Aplicação, janelas, instalador e documentos/atalhos associados compartilham o desenho; `zonly.png`/`zfull.png` permanecem marcas de cabeçalho Zagotools. Direitos, avisos MIT e atribuição de Swati K não mudaram.

## Arquitetura herdada

A origem foi o projeto MIT [Open in Google](https://github.com/SwatiK425/open-in-google) de Swati K. Os scripts PowerShell e `SETUP.md` no repositório são **legado de referência**. O fluxo de produto que esta documentação descreve é C#/WinForms. O projeto original não é o responsável pelo suporte da evolução Zagotools.

## Mudanças de arquitetura

Alterações em identidade, OAuth, schema SQLite, backup, substituição ou política de ambientes exigem testes de regressão e revisão de migração. Não elimine dados persistentes para “corrigir” um upgrade e não deixe uma falha de logging decidir o resultado da importação.
