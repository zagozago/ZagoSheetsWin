# Refinamento dirigido 2026-10-24 — 0.9.47

Referências: PROMPT_02 e ASISTOBEZAGOTOOLs2 fornecidos pelo usuário. CSV autorizado: [branch de traduções](https://github.com/zagozago/ZagoSheetsWin/blob/translations/zagosheetswin-refinamento-51-20261024/ZagoSheetsWin_i18n_refinamento_51_idiomas.csv).

## Alterações por requisito

| Requisitos | Componentes | Resultado |
|---|---|---|
| G-01–04 | Branding, LauncherForm | Marca +2 pt (12→14), logo proporcionalmente maior, informação próxima/elevada, linha fina global 9→7 pt; contexto após `|`; link legal central no rodapé, exceto primeiro uso; Sobre sem link próprio. |
| G-05, TEL-01 | AdaptiveButton, VisualStyle | Desenho conjunto e central de ícone/legenda, intervalo calculado sem espaços literais; pasta preenchida, engrenagem, banco de dados e ajuda. |
| TEL-02 B-01/02 | RecoveryForm, BackupSelectionList | Cinco ações, Restaurar +2 pt, ícones solicitados; checkbox estreito e cabeçalho com nenhum/parcial/todos. SelectedItems continua sendo a única origem das operações em lote. Colunas principais, ordenação e regras preservadas. |
| TEL-02 B-03 | DisclosureCard, RecoveryForm | Três painéis independentes iniciam fechados em cada instância, sem persistência. Uso/limite/percentual, total de concluídos e pendentes e disponível usam dados reais. Dica integral do CSV; sem tooltip/informação no painel de configuração. |
| TEL-04 | AboutForm | Quatro boxes, fundos suaves nos três solicitados, títulos localizados, versão real destacada, links externos com ícone; licença integral com rolagem e MIT dentro do texto. |
| TEL-05 | SetupForm, NumberBadge, ConnectionStatusLabel, ToggleSwitch | Números circulares, status verdadeiro central e confirmação em círculo escuro, descrição da seção 2, toggles equilibrados e opções avançadas sem ícone. |
| TEL-06 | AdvancedPreferencesForm | Explicação integral transferida para informação contextual; JSON e estados preservados. |
| TEL-07/08/09 | FirstUseWizard, TutorialPicture | Janela se ajusta por etapa; status 11→13 pt; título de dois cliques 11→15 pt; botão padrão 9→11 pt; legenda centralizada sob a miniatura. Três etapas e validações preservadas. |
| TEL-03 | Branding | Apenas cabeçalho compartilhado. Mensagens, orientação, retry e ações não foram redesenhados. |

As novas interações aprovadas são a sincronização de checkbox/seleção e o recolhimento independente dos painéis. As métricas são apenas apresentações derivadas: não alteram quota, retenção, limpeza ou descarte. Espaço disponível é limitado a zero quando o uso ultrapassa a quota; o percentual continua mostrando o uso real, enquanto a barra satura em 100%. Tamanhos usam MB decimais, como a política atual, com a unidade do recurso já localizado e formatação numérica do locale selecionado. Se o Windows não reconhecer um identificador de cultura, somente a formatação numérica usa a cultura invariável; o texto continua vindo do pacote do idioma escolhido.

## CSV e decisões de integração

- 4.335 registros: 85 mensagens × 51 locales oficiais, com UTF-8, unicidade, preenchimento e placeholders validados.
- [Mapeamento completo dos 85 IDs](refinement-20261024-mapping.json): discrimina reuso/criação/não aplicabilidade. O CSV original está em `i18n/refinement-20261024.csv`; importador em `tools/i18n/import_refinement.py`.
- Nenhuma tradução foi produzida nesta etapa. Mnemônicos, indicadores de menu e reticências preexistentes são decorações preservadas.
- `recovery.usage` era uma mensagem completa com quatro campos: títulos e contagem possuem recursos próprios, sem quebrar outros consumidores.
- `about.open` continua identificando Sobre/Licenças; o título curto Sobre possui recurso próprio. Título nativo Primeiro uso mantém seu texto original; contexto usa chave própria.
- `google.status.confirmed` é somente o fragmento “autorização confirmada”. Não é concatenado a outros fragmentos: a mensagem completa existente de estado permanece intacta. Essa linha está explicitamente não aplicável no mapeamento.
- `advanced.xls.save_preference` permanece no catálogo, sem criar botão Salvar: preferências já possuem autosave.
- A dica explica setas como navegação. O controle continua sendo um ListView nativo multisseleção, com Ctrl/Shift e teclado; não se promete seleção múltipla por setas isoladas. Ctrl+A não é oferecido pelo controle atual e não foi acrescentado como novo atalho.

## Validação

- Compilação local sem erros/avisos; 364 testes aprovados e 44 específicos de Windows não executados no Linux.
- Validação do CSV em modo `--check`, geração de recursos, 14 contratos de catálogo e auditoria de fonte aprovados.
- [Local core 40099207181](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40099207181): Windows e Linux aprovados no código `f1f4bcd4ec79ac5b319ceb7ea0ea11f7f5f1221e`.
- Matriz nativa aprovada nos 51 idiomas: telas e textos novos nos dois temas; painéis recolhidos/expandidos; sincronização de seleção, cabeçalho parcial/total, intervalo com Shift; geometria de layouts e medição tipográfica em 100%, 125%, 150% e 200%. O JSON de verificação registra os 51 locales e essas quatro escalas.
- Verificação das 51 transições de idioma do instalador e ciclo de instalação/atualização/desinstalação aprovados.
- Capturas reais da compilação final comparadas com a montagem; amostras adicionais em árabe/RTL, alemão e tâmil, incluindo temas claro/escuro e os painéis abertos. Artefato de capturas: `ZagoSheetsWin-branding-preview`, ID 12562351601.
- Não foi executado um novo login real no Google nesta rodada. As verificações nativas são offline; os testes de regressão de importação/autenticação e de armazenamento permanecem no fluxo automatizado.

Resultado: 65 IDs com reuso de chave, 18 com criação e 2 não aplicáveis, conforme o mapeamento completo. Nenhum arquivo de produção em Core ou Infrastructure foi alterado; somente os recursos gerados de localização consumidos pela aplicação.
