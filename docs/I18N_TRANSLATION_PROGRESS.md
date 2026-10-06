# Tradução dos 49 idiomas - progresso

A entrega 1 inclui catálogos e revisão editorial. A entrega 2 integra o seletor,
fontes, escritas RTL e validação visual. A versão instalada permanece PT/EN.

## Pack 1 - chinês simplificado

`i18n/packs/zh.json`, locale `zh-CN`, direção LTR, caracteres Han simplificados.
São 593 entradas preenchidas: 249 textos do aplicativo e acessibilidade,
42 termos de ênfase, 9 mensagens próprias e 293 mensagens padrão do instalador.

- `status=translated`, `editorialQa=passed`.
- `layoutQa=not_reviewed`, `nativeReview=not_reviewed`.
- O gerador valida o catálogo, mas não o incorpora aos recursos distribuídos.
- O gate de release recusa o pack até a integração e a validação visual.

Textos próprios produzidos por IA e revisados contra a fonte PT e o catálogo EN,
com atenção à conservação do original, exclusão de backups, consentimento,
autorização Google, restrições de XLS e ausência de um servidor intermediário.
Os termos de negrito seguem a mesma terminologia dos avisos e ações. Não houve
revisão por falante nativo; o estado separado deixa isso explícito.

Mensagens padrão do instalador adaptadas da tradução MIT mantida por
Zhenghan Yang (Kira), revisão `1ff90acc4ed4aee82b1cda43253243deee3daed4`:
https://github.com/kira-96/Inno-Setup-Chinese-Simplified-Translation
O original e sua licença estão em `i18n/reference/ChineseSimplified.source.isl`
e `i18n/reference/Inno-ChineseSimplified-LICENSE.txt`. O pack registra a revisão,
origem, hash do arquivo original e créditos. A incorporação futura deve incluir
a licença correspondente no pacote de distribuição.

Validação verifica a matriz exata, hash da fonte, variáveis e seus formatos,
marcas, máscaras de arquivo, separadores, números e presença de escrita Han.
A unidade técnica `%1 KB` é invariável; não se exige uma letra Han nesse rótulo.
As capacidades CSV/TSV podem ser escritas integralmente como 500,000 células,
50,000 linhas e 1,000 colunas. A exceção revisada verifica os valores 20,
500000, 50000 e 1000; não ignora alterações nos limites.

## Catálogos recebidos em 2026-10-06

Os 48 packs restantes foram fornecidos pelo usuário no arquivo
`zagosheetswin_48_locales_reviewed.zip`: 28.464 entradas, matriz e hash da fonte
corretos. Os 49 novos catálogos estão armazenados com `status=translated`.
PT e EN continuam sendo os idiomas distribuídos até a entrega 2.

A conferência técnica preserva placeholders, marcas, máscaras, separadores e
valores. Não equivale a revisão por falantes nativos. Os metadados de revisão
editorial foram fornecidos com os packs; `layoutQa` e `nativeReview` permanecem
`not_reviewed`.

Correções: quatro rótulos em inglês de malaiala e um rótulo de inicialização
em urdu; capacidades em hindi e coreano expressas numericamente com os mesmos
valores; `%1 KB` preservado no instalador francês e russo. Notações de unidades
francesas, russas e árabes têm equivalências explícitas no validador. Rótulos
técnicos sem escrita local têm exceções restritas por chave e valor exatos.
Números expandidos de capacidades continuam limitados a 20 MiB, 500000 células,
50000 linhas e 1000 colunas.

Por decisão do usuário, as ênfases automáticas em frases foram desativadas em
`EmphasisLabel`. Fontes explícitas de títulos e controles continuam aplicáveis.
As 42 chaves históricas de ênfase permanecem no contrato do catálogo para não
invalidar as traduções recebidas, mas não são usadas para pintar negritos.

Validação local: 51 idiomas e dez testes de contrato, incluindo rejeição de
alterações em capacidades, unidades, variáveis e exceções técnicas. Próxima
entrega: integrar seletor, fontes, direção RTL e layouts no Windows.
