# Formatos, ambientes e limites — estado atual

**Referência:** `SpreadsheetFormats.cs`, `GoogleImport.cs`, `LegacyWorkbookConverter.cs`, `HtmlWorkbookReader.cs`, `SourceEnvironment.cs`, `Replacement.cs` no candidato alfa 0.9.30. Esta é a matriz vigente, não os limites históricos do piloto 0.7.

## Entrada e destino

| Extensão | Preparação antes do Drive | Critério para retirar o original |
|---|---|---|
| `.xlsx` | Validar pacote OOXML e enviar XLSX | Fonte local elegível; backup íntegro, associação e atalho verificados, sem VBA; não há certificação de todos os recursos visuais |
| `.csv` | Decodificar texto estritamente, analisar e gerar XLSX com células de texto | Conferir planilha exportada quanto a posições, tipos e valores; recusar fórmula divergente |
| `.tsv` | Interpretar tabulação, normalizar para XLSX com texto literal | Mesma conferência exigida para CSV |
| `.ods` | Validar ZIP/mimetype, interpretar estruturas e enviar ODS verdadeiro | Somente quando conteúdo/recursos forem verificáveis; estrutura complexa pode exigir cópia; scripts recusados |
| `.xls` | Binário: ExcelDataReader + NPOI para gerar XLSX. Alguns relatórios HTML nomeados `.xls` têm leitor HTML específico | Somente após verificação de valores e recursos compatíveis. Fórmulas e recursos avançados podem forçar cópia. Macros não são executadas nem preservadas |

**Fora do escopo:** `.xlsm`, `.xlsb`, Word, PowerPoint e qualquer extensão não listada. Arquivo HTML **renomeado para `.ods`** não é um arquivo ODS: a importação é rejeitada sem upload.

## Limites implementados

| Recurso | Limite |
|---|---:|
| Bytes de entrada e payload normalizado | 20 MiB (`20 × 1024 × 1024` bytes) |
| Células processadas | 500.000 |
| Linhas | 50.000 |
| Colunas | 1.000 |
| Abas nos leitores aplicáveis | Até 100 |
| Texto por célula/campo | 32.767 caracteres |
| Conteúdo expandido de pacote ZIP | 96 MiB |
| Parte XML individual | 64 MiB |
| Resposta de exportação XLSX via Google | 10 MiB |
| Metadados HTTP Google (com restrições adicionais por endpoint) | Até 1 MiB |
| Checkpoints no diagnóstico por tentativa | 96 |
| Rotação de logs | 4 arquivos × 64 KiB |

Todos os limites pertinentes se aplicam simultaneamente. Um arquivo abaixo de 20 MiB pode falhar por excesso de linhas, células, XML expandido ou exportação. O Google também aplica regras próprias. Nenhum limite representa compromisso de fidelidade total.

## CSV e TSV

- Automático: UTF-8 estrito, com ou sem BOM; UTF-16 LE/BE somente com BOM. Não adivinha Windows-1252.
- Windows-1252 requer seleção explícita nas configurações e é rejeitado se conflitar com BOM.
- CSV: separadores vírgula ou ponto e vírgula; ambiguidade deve ser corrigida com escolha explícita.
- TSV: separador fixo de tabulação.
- Parser suporta campos vazios e aspas escapadas; estrutura irregular é rejeitada.
- Todos os valores CSV/TSV são mantidos como **texto literal**, incluindo zeros à esquerda, números longos, datas escritas como texto e cadeias com `=`, `+`, `-` ou `@`. Essa regra evita execução de fórmulas na importação, mas também exige conversões manuais posteriores no Sheets quando desejadas.

## XLS e relatórios HTML

O `.xls` binário usa verificação de leitura e conversão para XLSX. Recursos como VBA, desenho, proteção, formatação condicional e rich text não são garantia de fidelidade; a política pode exigir importação como cópia. Arquivos HTML de exportadores antigos com extensão `.xls` são tratados como **HTML**, sem executar scripts ou carregar URLs externas; tabelas são convertidas para abas e informações externas podem ficar em uma aba específica. Isso **não** equivale a suporte amplo a qualquer HTML.

## ODS: pendência explícita

- ODS válido é um pacote OpenDocument identificável por estrutura, `mimetype` e conteúdo. Arquivos inválidos não se tornam válidos pela extensão.
- ODS simples com valores conferíveis pode ser importado e, se elegível, substituído.
- ODS com fórmulas, datas/horários, objetos, mesclagens, estilos ou elementos sem equivalência comprovada pode ser importado **somente como cópia**, conservando o original; estruturas inválidas e scripts podem ser recusados.
- O exame estrutural de exemplos reais não prova que todos os documentos ODS funcionem. **ODS continua sob validação de compatibilidade e homologação real.**

## Localização do arquivo

- Pasta local, estável e não sincronizada, com acesso de escrita: candidata à substituição.
- OneDrive, pasta sincronizada reconhecida, UNC e unidade de rede: **cópia**, sem remoção da fonte.
- Arquivo somente na nuvem ou referência simbólica incompatível: bloqueio até estar disponível localmente, ou recusa.
- A detecção de sincronização é uma proteção, **não uma certificação para todos os provedores**.

O Google Sheets criado é a cópia de trabalho online. O backup da importação inicial não recebe edições posteriores. Se o Google Sheets associado for confirmado na lixeira e a fonte local ainda existir, uma **nova** operação pode ser criada preservando os registros antigos. Erros 404, acesso negado ou falha de rede não significam “na lixeira”.

## Critério de verificação

Para CSV, TSV e formatos com leitura de valores esperados, a conferência exporta o Sheets como XLSX e compara elementos suportados; é uma **verificação de valores**, não de gráficos, macros, formatação ou semântica completa. A documentação e o aplicativo devem preservar essa distinção.
