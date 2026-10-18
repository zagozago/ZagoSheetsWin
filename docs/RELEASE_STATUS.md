# Estado da versão — referência vigente

**Versão declarada no código: `0.9.37` (alfa).** Não existe lançamento estável 1.0.0 nem certificação.  
**Branch do aplicativo:** `feature/zagosheetswin`.  
**Commit-fonte conferido:** [f58b630263cd](https://github.com/zagozago/ZagoSheetsWin/commit/f58b630263cdee07e473a54ee40ff6d3beafe4f6) — 18/10/2026.  
**Sem tags de versão e sem GitHub Releases** na consulta de 18/10/2026. O objetivo futuro é homologar um commit e só então considerar a promoção para 1.0.0.

## Evidências verificadas

- [Projeto C#](../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj): versão `0.9.37`.
- [Inno Setup](../installer/SheetsWindows.iss): fallback `0.9.37`; distribuição usa versão do projeto.
- [Fonte internacional](../i18n/source.json): `productVersion = 0.9.37`, interface/instalador com 51 idiomas.
- [Avisos de terceiros](../third-party/NOTICE.md): cabeçalho 0.9.37, licenças preservadas.
- [Branding](../branding/README.md): aplicativo, janelas e instalador com ícone de planilha fornecido; mesmo desenho dos arquivos associados e atalhos.
- [Local core 0.9.37](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39099713350): **concluído com sucesso** no SHA `f58b630`.
- [Protected alpha distribution 0.9.37](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39099713370): **concluído com sucesso** no mesmo SHA `f58b630`.
- [Relatório de aceite técnico e amostras 0.9.36](IMPORT_REVIEW_0936.md): testes offline/CI e pendências Google real. É uma **fotografia datada** da 0.9.36, não homologação do 0.9.37 com Google real.

**Distinção:** sucesso do GitHub Actions não prova que todos os formatos convertam corretamente documentos reais, que a aplicação OAuth esteja verificada/publicada, nem assinatura Authenticode. A disponibilidade de um artefato de Actions não equivale a Release.

## Mudanças de comportamento vigentes na 0.9.37

1. **Importação por atalho:** após Google Sheets confirmado e verificações essenciais de backup/hash/vínculo, um original elegível pode ser substituído por `.url`, **mesmo quando há fidelidade parcial de fórmulas/valores** (política 0.9.33). A preferência de **importar como cópia**, ambiente sincronizado/rede e a opção específica de XLS continuam protegendo a origem nos casos previstos.
2. **Aviso pós-substituição:** Backups, `Não mostrar novamente` persistente e uma confirmação de cada vez (0.9.34). Não é pedido de consentimento a cada arquivo.
3. **Somente leitura:** falha de retirada conserva original e backup; correção do atributo permite retomada, sem reupload cego (0.9.35).
4. **CSV/TSV v2:** leitor por conteúdo (`,`, `;`, TAB e `|`), escolha manual por importação ambígua, UTF-8 estrito com fallback CP1252 limitado **sem BOM**, preservação de textos literais; limites próprios de **60.000 registros / 600.000 células**, além do limite geral de 20 MiB e 1.000 colunas (0.9.36).
5. **Operações antigas:** interpretação gravada por versão e operação. Uma intenção remota anterior conserva semântica v1, evitando reinterpretar o mesmo documento sem autorização.
6. **Reparo explícito:** `Backups → Mais ações → Reimportar em novo documento` lê snapshot original verificado e cria outra operação/documento. Não substitui o Google Sheets antigo nem retira a origem, e conserva o atalho anterior (0.9.36).
7. **ODS:** valida pacotes, suporta cálculos de coordenadas/caudas vazias sem materialização exagerada e envia o ODS original; **não declara equivalência de fórmulas, dados complexos nem homologação de todos os ODS**.
8. **Diagnósticos/repetição:** erro terminal persiste na janela e após minimizar/restaurar; progresso atrasado é ignorado; exportação da **tentativa atual** é diferente de exportação de **histórico**; erros permanentes exigem correção de entrada ou escolha de delimitador, sem retry automático (0.9.36).
9. **Visual:** ícone de planilha único para programa, janelas, instalador, arquivos associados e atalhos; identidade Zagotools do cabeçalho e créditos Open in Google mantidos (0.9.37).
10. **Configurações:** seção global de Formatos e texto não existe desde 0.9.32. Escolha de interpretação **por arquivo** para entrada ambígua ou reparo não é a volta do antigo controle global.

## Preparação para a futura 1.0

Ver [checklist](PRE_RELEASE_CHECKLIST.md). No congelamento do commit candidato:

- Conferir o mesmo SHA em código, pacote protegido, hashes, manifesto e testes.
- Fazer aceite manual no Windows 11 com conta Google, cinco formatos e amostras de CSV/TSV/ODS indicadas no [relatório 0.9.36](IMPORT_REVIEW_0936.md).
- Validar importação como cópia, substituição, falha/somente leitura, retries, diagnósticos e **novo reparo em documento separado**.
- Revisar tradução editorial dos READMEs e central de ajuda efetivamente hospedada.
- Verificar OAuth/distribuição, licenças de terceiros e eventual assinatura do instalador.
- Depois de aprovado: atualizar estado, README e CHANGELOG, criar tag/Release oficiais. **Não há promessa de data nem de conclusão prévia do aceite**.

Os relatórios `SECURITY_REVIEW_0930.md` e `IMPORT_REVIEW_0936.md` são evidências de seus respectivos marcos, não certificados de segurança nem garantia geral de conversão.
