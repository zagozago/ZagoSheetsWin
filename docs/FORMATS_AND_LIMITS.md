# Formatos, ambientes e limites — estado atual

**Referência:** código de `SpreadsheetFormats`, `GoogleImport`, `ConversionVerifier`, `ReplacementCoordinator`, `WindowsLauncher` e políticas Windows no candidato alfa **0.9.35** ([estado e commit](RELEASE_STATUS.md)). Esta é a matriz vigente, não os limites históricos do piloto 0.7 ou a política de fidelidade da 0.9.30.

## Entrada e destino

| Extensão | Preparação antes do Drive | Critério para retirar o original |
|---|---|---|
| `.xlsx` | Validar pacote OOXML e enviar XLSX | Fonte local elegível; backup íntegro, associação e atalho verificados, sem VBA; não há certificação de todos os recursos visuais |
| `.csv` | Decodificar texto estritamente, analisar e gerar XLSX com células de texto | Divergência de valores conferidos gera **advertência**, não cópia automática, se a importação Google estiver confirmada e as demais condições de segurança forem satisfeitas |
| `.tsv` | Interpretar tabulação, normalizar para XLSX com texto literal | A análise estrutural deve ser válida; a divergência de valores conferidos deixa de bloquear por si só a retirada do original |
| `.ods` | Validar assinatura/ZIP/mimetype e interpretar estruturas; enviar ODS verdadeiro | Fórmulas, datas, mesclagens e recursos não verificáveis **podem não ser preservados**; isso **não impede por si só** a substituição após importação confirmada. Estrutura inválida e scripts são recusados |
| `.xls` | Binário: ExcelDataReader + NPOI para gerar XLSX. Alguns relatórios HTML nomeados `.xls` têm leitor HTML específico | Fórmulas/valores complexos podem mudar **e a origem ainda pode ser substituída** se a opção de XLS permitir. Macros não são executadas nem preservadas |

**Fora do escopo:** `.xlsm`, `.xlsb`, Word, PowerPoint e qualquer extensão não listada. Arquivo HTML **renomeado para `.ods`** não é um arquivo ODS: a importação é rejeitada sem upload.

## Mudança crítica da política de substituição (0.9.33–0.9.35)

A presença de fórmulas, recursos complexos ou uma **diferença de valores identificada na verificação** já não impõe importação como cópia. A classe `ConversionVerifier` marca a limitação e permite a conclusão quando o documento Google está confirmado, o vínculo remoto é válido, o backup original está íntegro e as demais regras de retirada foram atendidas.

**Isso não garante fidelidade.** O conteúdo no Google Sheets pode ser diferente. Após uma substituição, o arquivo da pasta original vira um **atalho `.url`**; os bytes anteriores continuam no backup e podem ser restaurados separadamente. Se conservar o arquivo no lugar for essencial, selecione **Importar como cópia** antes da operação.

São bloqueios reais, não simples avisos: pacote inválido, script/estrutura recusada, importação remota não confirmada, resposta de exportação com falha, associação/hash inconsistente, ausência/corrupção de backup, alteração conflitante da fonte ou impossibilidade de remover o original. A política de cópia automática para **rede/pastas sincronizadas** e as escolhas do usuário continuam aplicáveis. A 0.9.35 reconhece que o atributo **somente leitura** do Windows pode impedir a retirada; nesse caso, preserva original e backup.

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

O `.xls` binário usa leitura independente e conversão para XLSX. Recursos como VBA, desenho, proteção, formatação condicional e rich text **não** têm fidelidade garantida. Depois de importação Google confirmada, a substituição segue a preferência de XLS e os controles de integridade **mesmo que haja equivalência apenas parcial**. Para conservar o original no mesmo local, use o modo de cópia ou desative substituição de XLS. Arquivos HTML de exportadores antigos com extensão `.xls` são tratados como **HTML**, sem executar scripts ou carregar URLs externas; tabelas são convertidas para abas e informações externas podem ficar em uma aba específica. Isso **não** equivale a suporte amplo a qualquer HTML.

## ODS: pendência explícita

- ODS válido é um pacote OpenDocument identificável por estrutura, `mimetype` e conteúdo. Arquivos inválidos não se tornam válidos pela extensão.
- ODS simples com valores conferíveis pode ser importado e, se elegível, substituído.
- ODS com fórmulas, datas/horários, objetos, mesclagens, estilos ou outros elementos **sem equivalência comprovada pode ser importado e ter o original substituído**, conforme preferências e após importação confirmada. O backup mantém os bytes originais; **não pressupor que o Sheets terá os mesmos dados ou fórmulas**. Estruturas inválidas e scripts podem ser recusados.
- O exame estrutural de exemplos reais não prova que todos os documentos ODS funcionem. **ODS continua sob validação de compatibilidade e homologação real.**

## Localização do arquivo

- Pasta local, estável e não sincronizada, com acesso de escrita: candidata à substituição.
- OneDrive, pasta sincronizada reconhecida, UNC e unidade de rede: **cópia**, sem remoção da fonte.
- Arquivo somente na nuvem ou referência simbólica incompatível: bloqueio até estar disponível localmente, ou recusa.
- A detecção de sincronização é uma proteção, **não uma certificação para todos os provedores**.

O Google Sheets criado é a cópia de trabalho online. O backup da importação inicial não recebe edições posteriores. Se o Google Sheets associado for confirmado na lixeira e a fonte local ainda existir, uma **nova** operação pode ser criada preservando os registros antigos. Erros 404, acesso negado ou falha de rede não significam “na lixeira”.

## Critério de verificação

Quando o formato tem valores esperados, o aplicativo pode exportar o Sheets como XLSX e compará-los. Na **0.9.35**, diferenças representadas por `ConversionMismatchException` ou conteúdo não comparável marcam uma advertência interna e **não bloqueiam por si sós** a substituição. Falhas reais na exportação, associação remota, snapshot ou conteúdo estrutural ainda bloqueiam. A comparação não abrange gráficos, macros, formatação nem a semântica total das fórmulas.

**Interface atual:** os cinco formatos estão habilitados sem toggle. A seção de formato/codificação/delimitador foi removida das Opções avançadas em 0.9.32; escolhas `formats.json` pré-existentes permanecem aplicáveis, mas **não existe seletor atual para alterá-las na interface**.
