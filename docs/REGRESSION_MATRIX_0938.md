# Matriz de regressão 0.9.38 — 26 + 78 casos

Atualização 0.9.39: T012 aceito após correção de coluna única citada; T027 real anexado tem largura inválida e deve ser rejeitado, enquanto a variante corrigida sem aspas ao redor da linha inteira deve passar; T031 tem correção de CRLF e retomada. Ver REGRESSION_0939.md.

Especificação revisada; não equivale a execução de todos os binários. Os arquivos mestres permanecem intactos. Testes locais sintéticos por forma e gramática não substituem o smoke no Google.

| ID | Arquivo | Critério nesta versão | Evidência desta iteração |
|---|---|---|---|
| B001 | 01_59999_registros_10_col_600000_celulas.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B002 | 02_60000_registros_9_col.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B003 | 03_60001_registros_9_col.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B004 | 04_60000_registros_10_col_600010_celulas.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B005 | 05_29999_registros_20_col_600000_celulas.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B006 | 06_30000_registros_20_col_600020_celulas.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B007 | 07_60000_registros_1_col.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B008 | 08_60001_registros_1_col.csv | Positivo experimental se ≤100.000 linhas totais e ≤1.000.000 células; demais cotas continuam. | Formas sintéticas convertidas e comparadas localmente; Google pendente. |
| B009 | 09_20MiB_menos_1_byte.csv | aceitar pela origem, sujeito a verificacao do tamanho do payload | Pendente: binário mestre e Google real. |
| B010 | 10_20MiB_exato.csv | aceitar se comparacao do limite e inclusiva e payload nao exceder | Pendente: binário mestre e Google real. |
| B011 | 11_20MiB_mais_1_byte.csv | rejeitar em preflight: tamanho da origem excede 20 MiB | Pendente: binário mestre e Google real. |
| B012 | 12_celula_maior_que_50000_chars.csv | rejeitar se limite de caracteres por celula ultrapassado | Pendente: binário mestre e Google real. |
| O001 | 01_ods_basico.ods | aceitar | Pendente: binário mestre e Google real. |
| O002 | 02_vazios_internos_esparsos.ods | aceitar | Pendente: binário mestre e Google real. |
| O003 | 03_milhao_vazios_finais.ods | Rejeitar explicitamente repetição acima da grade; sem normalização do payload ODS. | Rejeição sintética sem expansão proporcional. |
| O004 | 04_oito_vazias_antes_de_dado.ods | aceitar | Pendente: binário mestre e Google real. |
| O005 | 05_linhas_vazias_repetidas.ods | aceitar | Pendente: binário mestre e Google real. |
| O006 | 06_dois_milhoes_linhas_vazias_apos_valor.ods | Rejeitar explicitamente repetição acima da grade; sem normalização do payload ODS. | Rejeição sintética sem expansão proporcional. |
| O007 | 07_formula_e_literal.ods | aceitar | Pendente: binário mestre e Google real. |
| O008 | 08_mescladas_e_cobertas.ods | aceitar | Pendente: binário mestre e Google real. |
| O009 | 09_valores_nao_vazios_repetidos.ods | aceitar | Pendente: binário mestre e Google real. |
| O010 | 10_multiplas_abas_unicode.ods | aceitar | Pendente: binário mestre e Google real. |
| O011 | 11_valor_depois_70000_colunas.ods | rejeitar com diagnostico de coordenada fora do limite, sem expandir 70000 | Pendente: binário mestre e Google real. |
| O012 | 12_valor_depois_70000_linhas.ods | rejeitar com diagnostico de coordenada fora do limite sem expandir 70000 | Pendente: binário mestre e Google real. |
| O013 | 13_repeticao_zero_invalida.ods | rejeitar atributo de repeticao invalido sem travar | Pendente: binário mestre e Google real. |
| O014 | 14_repeticao_negativa_invalida.ods | rejeitar atributo de repeticao invalido sem travar | Pendente: binário mestre e Google real. |
| O015 | 15_repeticao_2b_limite_segurança.ods | rejeitar com erro de limite ou coordenada sem alocacao massiva | Pendente: binário mestre e Google real. |
| O016 | 16_ods_content_ausente.ods | rejeitar ODS sem content.xml | Pendente: binário mestre e Google real. |
| O017 | 17_ods_xml_malformado.ods | rejeitar ODS com XML invalido | Pendente: binário mestre e Google real. |
| O018 | 18_vazias_com_estilo_antes_dado.ods | aceitar | Pendente: binário mestre e Google real. |
| R001 | historico.csv | aceitar | Pendente: binário mestre e Google real. |
| R002 | historico.csv | aceitar | Pendente: binário mestre e Google real. |
| R003 | historico.csv | nao reutilizar destino baseado somente no hash/nome | Pendente: binário mestre e Google real. |
| R004 | ação № 42 🧪.csv | aceitar | Pendente: binário mestre e Google real. |
| R005 | arquivo_bloqueavel.csv | aceitar | Pendente: binário mestre e Google real. |
| T001 | 01_utf8_pv_aspas_crlf.csv | aceitar | Pendente: binário mestre e Google real. |
| T002 | 02_cp1252_pv_euro.csv | aceitar | Pendente: binário mestre e Google real. |
| T003 | 03_utf8bom_pv_multilinha.csv | aceitar | Pendente: binário mestre e Google real. |
| T004 | 04_virgula_com_delimitadores_citados.csv | aceitar | Pendente: binário mestre e Google real. |
| T005 | 05_pipe_aspas_embutidas.csv | aceitar | Pendente: binário mestre e Google real. |
| T006 | 06_tsv_quoted_tabs_e_linhas.tsv | aceitar | Pendente: binário mestre e Google real. |
| T007 | 07_tsv_cp1252.tsv | aceitar | Pendente: binário mestre e Google real. |
| T008 | 08_tsv_bom_crlf.tsv | aceitar | Pendente: binário mestre e Google real. |
| T009 | 09_csv_separado_por_tab.csv | aceitar | Pendente: binário mestre e Google real. |
| T010 | 10_tsv_separado_por_pv.tsv | aceitar | Pendente: binário mestre e Google real. |
| T011 | 11_csv_uma_coluna_legitima.csv | aceitar | Pendente: binário mestre e Google real. |
| T012 | 12_csv_coluna_unica_aspas.csv | aceitar | Pendente: binário mestre e Google real. |
| T013 | 13_ambiguo_virgula_pv.csv | Exigir escolha pontual com prévia; nunca desempatar arbitrariamente. | Ambas as gramáticas testadas; UI Windows pendente. |
| T014 | 14_ambiguo_virgula_pipe.csv | Exigir escolha pontual com prévia; nunca desempatar arbitrariamente. | Ambas as gramáticas testadas; UI Windows pendente. |
| T015 | 15_aspas_nao_fechadas.csv | rejeitar com erro de aspas nao fechadas | Pendente: binário mestre e Google real. |
| T016 | 16_aspas_apos_caractere.csv | rejeitar ou exigir interpretação explícita; nunca truncar texto | Pendente: binário mestre e Google real. |
| T017 | 17_cp1252_byte_indefinido.csv | erro de codificação ou substituição explícita e diagnóstica | Pendente: binário mestre e Google real. |
| T018 | 18_utf16le_nao_suportado.csv | detectar UTF-16 e processar explicitamente ou rejeitar de forma clara | Pendente: binário mestre e Google real. |
| T019 | 19_tsv_campos_vazios.tsv | aceitar | Pendente: binário mestre e Google real. |
| T020 | 20_csv_formulas_literal.csv | aceitar | Pendente: binário mestre e Google real. |
| T021 | 21_tsv_formulas_literal.tsv | aceitar | Pendente: binário mestre e Google real. |
| T022 | 22_csv_acentos_emoji_unicode.csv | aceitar | Pendente: binário mestre e Google real. |
| T023 | 23_csv_sem_cabecalho.csv | Aceitar; preservar valores e dimensões do catálogo. | Regressão sintética por gramática; Google pendente. |
| T024 | 24_csv_lf_retorno_cr.csv | aceitar | Pendente: binário mestre e Google real. |
| T025 | 25_csv_spaces_trailing.csv | aceitar | Pendente: binário mestre e Google real. |
| T026 | 26_csv_cabecalhos_duplicados.csv | aceitar | Pendente: binário mestre e Google real. |
| T027 | 27_csv_sep_excel_hint.csv | Omitir linha exata sep=; e interpretar tabela regular seguinte; rejeitar conflito. Dimensões antigas 3×1 deixam de ser critério. | Quatro dicas de separador, aspas e multilinhas testadas localmente. |
| T028 | 28_tsv_delimitador_citado_pipe.tsv | aceitar | Pendente: binário mestre e Google real. |
| T029 | 29_csv_sem_terminador_final.csv | aceitar | Pendente: binário mestre e Google real. |
| T030 | 30_csv_200_colunas_esparsas.csv | aceitar | Pendente: binário mestre e Google real. |
| T031 | 31_csv_aspas_multilinha_crlf.csv | aceitar | Pendente: binário mestre e Google real. |
| T032 | 32_csv_pv_decimal_virgula.csv | aceitar | Pendente: binário mestre e Google real. |
| T033 | 33_csv_pipe_com_texto_livre.csv | aceitar | Pendente: binário mestre e Google real. |
| T034 | 34_tsv_coluna_unica.tsv | Aceitar; preservar valores e dimensões do catálogo. | Regressão sintética por gramática; Google pendente. |
| T035 | 35_csv_misto_cabecalho_errado.csv | rejeitar contagem de colunas inconsistentes ou tratar ausentes explicitamente | Pendente: binário mestre e Google real. |
| T036 | 36_tsv_bom_sem_cabecalho.tsv | aceitar | Pendente: binário mestre e Google real. |
| T037 | 37_bytes_NUL_no_texto.csv | rejeitar byte NUL ou representar literalmente sem truncamento e com aviso | Pendente: binário mestre e Google real. |
| T038 | 38_arquivo_csv_vazio.csv | rejeitar com erro de arquivo vazio; nunca criar Google Sheets em branco | Pendente: binário mestre e Google real. |
| T039 | 39_csv_so_cabecalho.csv | aceitar zero registros se a política permitir; caso contrário erro claro | Pendente: binário mestre e Google real. |
| T040 | 40_multilinhas_muitas_linhas_fisicas.csv | aceitar | Pendente: binário mestre e Google real. |
| X001 | 01_controle_basico.xlsx | controle: preservar funcionamento anterior | Pendente: binário mestre e Google real. |
| X002 | 02_controle_formulas.xlsx | controle: preservar funcionamento anterior | Pendente: binário mestre e Google real. |
| X003 | 03_controle_multiplas_abas.xlsx | controle: preservar funcionamento anterior | Pendente: binário mestre e Google real. |

## Catálogo anterior — 26 controles

Os 26 foram informados como aprovados pelo usuário antes do novo lote. A tabela preserva os controles, sem declarar uma nova execução dos respectivos binários.

| Arquivo | Forma indicada no catálogo | Critério nesta iteração | Evidência atual |
|---|---|---|---|
| 01_anagrams_pontovirgula.csv | 20 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 02_anscombe_pontovirgula.csv | 44 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 03_flights_pontovirgula.csv | 24 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 04_attention_pontovirgula.csv | 20 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 05_catalogo50_pontovirgula.csv | 50 × 8 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 06_ptbr_pontovirgula.csv | 5 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 07_anscombe_decimal_virgula.csv | 44 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 08_catalogo50_pontovirgula_bom.csv | 50 × 8 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 09_anscombe_pontovirgula_lf.csv | 44 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 10_ptbr_pontovirgula_cp1252.csv | 5 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 11_aspas_multilinhas_pontovirgula.csv | 3 × 4 | Manter gramática multiline válida e reconciliar a operação existente sem novo upload cego. | Gramática sintética e falhas remotas simuladas; estado real A-01 pendente. |
| 12_anagrams_pontovirgula_sem_cabecalho.csv | 20 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 13_anscombe_pontovirgula_bom.csv | 44 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 14_catalogo50_pontovirgula_lf.csv | 50 × 8 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 15_ptbr_pontovirgula_lf.csv | 5 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 16_anagrams_controle_virgula.csv | 20 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 17_anscombe_controle_virgula.csv | 44 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 18_ptbr_virgula_com_aspas.csv | 5 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 19_anagrams_tab_em_csv.csv | 20 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 20_catalogo50_tab_em_csv.csv | 50 × 8 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 21_flights_tab_em_csv_bom.csv | 24 × 3 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 22_catalogo50_pipe.csv | 50 × 8 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 23_aspas_multilinhas_pipe.csv | 3 × 4 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 24_ptbr_pipe.csv | 5 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; não reexecutado aqui. |
| 25_anagrams_pontovirgula_grande_1mb.csv | 90,000 × 5 | 90.001×5: positivo experimental com limites v3; Google real pendente. | Forma sintética completa verificada. |
| 26_anagrams_pontovirgula_medio.csv | 32,000 × 5 | Preservar dimensões e valores; repetir smoke em cópia no Google real. | Aceite anterior informado pelo usuário; o catálogo descreve 32.000 registros ×5, e o mestre não foi reprocessado aqui. |
