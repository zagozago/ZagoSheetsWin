# Escopo B — candidato 0.9.36

Implementação autorizada após a auditoria de 17/10/2026. Este documento distingue conferência local de conversão real no Google. Não declara concluída a publicação.

| Item do relatório | Implementação | Validação |
|---|---|---|
| IMP-01 / IMP-02 | CSV e TSV usam conteúdo para detectar `,`, `;`, TAB e `|`; aspas, linhas internas, escape e largura consistente; ambiguidade exige escolha por importação | Testes independentes de posição/valor e sete amostras reais do ZIP conferidas célula por célula |
| IMP-03 | BOM respeitado; UTF-8 estrito; fallback CP1252 sem BOM rejeita bytes indefinidos e controles de aparência binária | CP1252 do ZIP, acentos e decimais preservados; BOM conflitante e bytes inválidos recusados |
| IMP-04 | Posições ODS calculadas sem materializar caudas vazias; lacunas internas preservadas, conteúdo e repetições limitados; bytes enviados continuam originais | ODS fornecido aceito na preparação; testes de coordenadas, caudas e abuso; conversão real ainda pendente |
| MIG-01 | Opções e versão do interpretador imutáveis por operação; intenções remotas antigas mantêm versão 1; ação explícita cria novo documento/identidade a partir de backup verificado | Teste com servidor Google simulado confirma documento antigo e associação intactos, duas colunas no novo e retomada sem novo upload |
| UI-01 | Estado terminal separado do texto do controle; geração impede progresso atrasado; mínimo e layout recalculados na restauração; exportar diagnóstico mantém a causa da falha | Regressão nativa de minimizar/restaurar e mensagens atrasadas; confirmar no Windows real que apresentou o sintoma |
| DIAG-01 / DIAG-02 | Exportação da janela filtra AttemptId; histórico completo tem ação e nome próprios; códigos específicos para linhas/células/bytes, estrutura, ODS e exportação remota | Testes de correlação, privacidade, persistência e exportação; esquema anterior continua legível |
| RETRY-01 | Sem repetição automática de erro permanente; repetição manual de entrada inválida aguarda edição; ambiguidade abre escolha; entrada corrigida sem intenção remota inicia operação nova | Testes de preservação dos dois snapshots, bloqueio após intenção remota e revalidação nativa |
| CAP-01 | Somente CSV/TSV: 60.000 registros e 600.000 células; allowance explícito na conferência XLSX | Testes de 53.941×10 e 60.000×10; outro formato continua no limite anterior; diamonds real conferido localmente |
| CAP-02 | Bytes grandes/streaming/capacidade ilimitada continuam fora do escopo | AudioSet não foi fornecido; nenhuma alegação de validação desses arquivos |

## Amostras originais, sem chamadas Google

Leitura de referência produzida com `csv.reader` Python, encoding e delimitador definidos independentemente. O código C# foi executado em Debug no Linux; comparou todas as células com a referência e verificou o XLSX gerado por releitura. Os arquivos do usuário e os JSONL originais não são publicados no repositório.

| Amostra | Registros × colunas | Resultado local |
|---|---:|---|
| 10 CP1252 | 6 × 5 | Passou |
| 19 TAB em CSV | 21 × 5 | Passou |
| 20 catálogo TAB | 51 × 8 | Passou |
| 22 catálogo pipe | 51 × 8 | Passou |
| 23 aspas/multilinhas pipe | 4 × 4 | Passou |
| 24 PT-BR pipe | 6 × 5 | Passou |
| diamonds.csv | 53.941 × 10 | Passou; aproximadamente 2,1 s para preparar e reler XLSX |
| ODS com caudas vazias | Quatro abas | Preparação passou; bytes preservados; verificação tipada indisponível por recursos complexos |

Diamonds no processo de conferência local: XLSX de 2.440.988 bytes; aproximadamente 460 MB de alocações acumuladas (não memória residente) e pico residente do processo de aproximadamente 196 MB. São medidas de uma execução, sem rede; não são garantia de tempo/memória em todo computador.

A reprodução nativa no Windows confirmou a causa visual de UI-01: após minimizar/restaurar o modal, o controle de mensagem ficava com apenas 31 pixels de altura, menos que seu padding vertical de 36 pixels. O texto permanecia no estado, mas não era desenhado. A correção mantém o tamanho mínimo compatível com mensagem/cabeçalho/ações e recalcula o layout ao restaurar. O teste mede a presença de texto renderizado antes e depois, além do estado textual.

## Invariantes preservados

- Entrada/payload até 20 MiB, exportação XLSX Google até 10 MiB; outros formatos conservam limites anteriores.
- Backup verificado, identidade e associação remota continuam necessários para substituir o original; preferência de cópia permanece válida.
- Não volta aviso de perda de fórmulas nem exceção automática de cópia por equivalência parcial. O aviso genérico e “Não mostrar novamente” continuam iguais.
- Reparar cria uma cópia nova. Não sobrescreve a planilha antiga, não troca seu atalho e não retira o original; o snapshot anterior permanece disponível sob a política normal de backups.
- Nada muda em OAuth, escopos, permissões Google ou configurações de publicação.
- A versão do interpretador fixa a semântica de `auto`, além das opções explícitas. Mudanças futuras nessa semântica precisam de uma nova versão; não reinterpretar operações antigas.

## Validação final e verificações manuais

Resultados da automação Windows/Linux e do instalador serão registrados após a execução do candidato. O build oficial continua usando a licença privada configurada no CI; a compilação local Debug usa o modo de desenvolvimento permitido pelo fornecedor.

Antes de publicar, no Windows 11 e na conta Google:

1. Abrir as oito amostras do ZIP; conferir colunas, acentos, `19,90`, aspas e células de início/meio/fim. Para diamonds, conferir 53.941 registros incluindo cabeçalho e dez colunas.
2. Verificar o ODS nas quatro abas e confirmar backup/restauração binária. Os outros nove ODS ausentes não são considerados validados.
3. Em Backups → Mais ações → Reimportar em novo documento, reparar um TAB/CSV importado incorretamente pela versão antiga. Confirmar novo documento e antigo intacto, inclusive edições online e atalho anterior.
4. Provocar erro; minimizar/restaurar, exportar diagnóstico, trocar idioma/tema quando disponível e repetir após corrigir o arquivo. A causa deve continuar legível e o JSONL da janela deve conter só seu AttemptId.
5. Confirmar substituição/cópia conforme preferências, sucesso sem aviso de fórmulas e persistência de “Não mostrar novamente”.

Pendências externas de OAuth/branding/domínio/assinatura da revisão de segurança continuam separadas deste pacote.
