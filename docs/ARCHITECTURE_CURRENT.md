# Arquitetura atual do ZagoSheetsWin

**Base:** `feature/zagosheetswin`, candidato 0.9.30, em 15/10/2026. Descrição da **implementação**, não do arquivo histórico [ARCHITECTURE.md](ARCHITECTURE.md), que nasceu como arquitetura-alvo. Diagramas e regras abaixo foram verificados nos projetos C# e scripts do repositório. Não representam auditoria formal de segurança.

## Organização do código

| Diretório ou módulo | Responsabilidade |
|---|---|
| `src/SheetsWindows.Core/` | Modelos `ImportOperation`, `Snapshot`, `SourceDescriptor`; interfaces e preparação local |
| `src/SheetsWindows.Infrastructure/` | Drive, OAuth, backup, banco SQLite, upload retomável, formatos, integração Windows, logs e recuperação |
| `src/SheetsWindows.Windows/` | Formulários WinForms: início, primeiro uso, configurações, opções avançadas, processamento, recuperação, tutorial e seletor de idiomas |
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
5. Fazer upload/retomada de sessão; confirmar ID, tipo Google Sheets, permissão, estado da lixeira e marcadores `sw_operation` / `sw_hash`. Associação não é escolhida por nome.
6. Para substituição elegível: verificar backup, publicar o atalho sem sobrescrever outro arquivo, solicitar abertura do navegador e só então retirar o arquivo original sob handle Windows que evita corrida por nome/caminho.
7. Persistir cada transição no journal. Se uma etapa falhar, preservar representação recuperável; uma criação remota incerta exige reconciliação, não repetição cega.
8. Se a importação exigir cópia: manter a fonte. Com atalhos habilitados, publicar atalho na pasta privada do aplicativo.

**Reabrir `.url` não chama o importador**: o Windows abre diretamente a URL HTTPS gravada. Não há sincronização reversa nem atualização remota silenciosa de arquivo conhecido alterado.

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
- Fonte de rede/sincronizada não é retirada; fórmulas e recursos de planilha sem prova de fidelidade forçam cópia ou bloqueiam o fluxo.
- Diagnósticos sanitizados, no máximo quatro arquivos de 64 KiB e até 96 checkpoints por tentativa; nome sugerido de exportação pode conter basename sanitizado, mas os campos do JSONL não contêm nomes de documentos.
- Nenhuma afirmação de certificação de segurança, assinatura Authenticode ou Google OAuth publicamente verificado sem evidência externa.

## Interface e internacionalização

`FirstUseWizard` tem três páginas. `TutorialForm` tem quatro páginas. `SetupForm` contém conexão Google, padrões Windows e opções usuais; `AdvancedPreferencesForm` contém escopo de pasta, formatos e XLS. Temas claro (padrão)/escuro. Catálogo de 51 idiomas gerado a partir de fonte e ordem T51; RTL e fontes alternativas dependem também das fontes instaladas no Windows.

## Arquitetura herdada

A origem foi o projeto MIT [Open in Google](https://github.com/SwatiK425/open-in-google) de Swati K. Os scripts PowerShell e `SETUP.md` no repositório são **legado de referência**. O fluxo de produto que esta documentação descreve é C#/WinForms. O projeto original não é o responsável pelo suporte da evolução Zagotools.

## Mudanças de arquitetura

Alterações em identidade, OAuth, schema SQLite, backup, substituição ou política de ambientes exigem testes de regressão e revisão de migração. Não elimine dados persistentes para “corrigir” um upgrade e não deixe uma falha de logging decidir o resultado da importação.
