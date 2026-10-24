# Documentação do ZagoSheetsWin

**Referência vigente:** código da branch `feature/zagosheetswin`, candidato alfa **0.9.48** em 24/10/2026. Não existe Release 1.0.0 publicada. O README internacional e as traduções mantidas na `main` são incorporados à branch de documentação para preservar a navegação sem rebaixar a documentação do software.

## Guias vigentes (não são documentos de planejamento)

| Documento | Uso |
|---|---|
| [README internacional](../README.md) | Visão do produto, seletor das 51 línguas de documentação, instalação e avisos legais |
| [Estado da versão](RELEASE_STATUS.md) | **Referência central 0.9.48**; testes de UI e distribuição anterior aprovados, hotfix RTL com **Local core aprovado** no código `08dbced`; distribuição protegida ainda aguardando aprovação; sem tag/Release nem aceite Google real |
| [Manual do usuário](USER_GUIDE.md) | Instalação, primeiro uso, abertura, preferências, backups e restauração |
| [Arquitetura atual](ARCHITECTURE_CURRENT.md) | Componentes reais, dados locais e ciclo de importação |
| [Formatos e limites](FORMATS_AND_LIMITS.md) | Matriz atual com **substituição mesmo após conversão parcial**, regras de cópia, atributos somente leitura e limites |
| [Solução de problemas](TROUBLESHOOTING.md) | Erros e procedimentos de recuperação |
| [Checklist de preparação estável](PRE_RELEASE_CHECKLIST.md) | Critérios pendentes antes de anunciar 1.0.0 |
| [Plano da central de ajuda](HELP_CENTER_PLAN.md) | Links canônicos atuais e URLs futuras ainda não publicadas |
| [Changelog](../CHANGELOG.md) | Linha do tempo com commits, sem inventar tags/Releases |
| [Contribuição](../CONTRIBUTING.md) | Compilação, testes, segurança e PRs |

## Referências técnicas auxiliares

- [Segurança e dependências — fotografia 0.9.30 com adendo de política 0.9.33](SECURITY_REVIEW_0930.md); não representa certificação de segurança nem auditoria de 0.9.35.
- [Integração dos 51 idiomas](I18N_INTEGRATION.md).
- [Conversão XLS e relatórios HTML com extensão XLS](XLS_CONVERSION.md).
- [Revisão do Escopo B (0.9.36), testes reais de entrada sem Google e pendências](IMPORT_REVIEW_0936.md): relatório datado, referência de evidências, não certificado.
- [Refinamento 0.9.47 — 51 idiomas, UI e 85 mensagens](implementation/refinement-20261024.md), [mapeamento](implementation/refinement-20261024-mapping.json) e [acabamento 0.9.48 — aceite visual](implementation/final-ui-20261024.md).
- [Regressões 0.9.38–0.9.43](REGRESSION_0943.md): acompanhe também [0.9.38](REGRESSION_0938.md), [0.9.39](REGRESSION_0939.md), [0.9.40](REGRESSION_0940.md), [0.9.41](REGRESSION_0941.md) , [0.9.42](REGRESSION_0942.md) e a [matriz 26+78](REGRESSION_MATRIX_0938.md), documentos técnicos históricos que não substituem o comportamento atual nem o aceite Google real.
- [Associações na lixeira e JSONL de diagnóstico](diagnostics-and-trashed-associations.md).
- [Decisões arquiteturais](adr/DECISIONS.md): justificam escolhas **no momento em que foram feitas**; quando houver divergência, o código e o guia atual prevalecem.
- [Auditoria upstream](AUDIT.md): auditoria do projeto original, não implementação atual.

## Histórico preservado, não vigente

Consulte [HISTORY.md](HISTORY.md) para fases `STAGE_*`, `ALPHA_*`, `ALPHA_NEXT.md`, `ROADMAP.md`, `ARCHITECTURE.md` (arquitetura **proposta**, não necessariamente atual), `DEVELOPMENT.md` e documentação de UI/traduções de momentos anteriores.

**Política de conservação:** links históricos não são alterados ou apagados silenciosamente. A documentação atual fica em arquivos com nomes próprios e este índice separa os dois contextos. Não use um pacote ou limite de um registro histórico como orientação operacional. A documentação do upstream em `../SETUP.md` não instala o atual produto C#.

## Documentação em 51 idiomas

O [README raiz](../README.md) apresenta o seletor de documentação: inglês + 50 arquivos em `docs/i18n/<código>/README.md`. Esses arquivos foram preservados a partir da `main` como **traduções históricas existentes**. A interface e o instalador usam 51 idiomas a partir de catálogos próprios; a existência de traduções não certifica revisão editorial, funcionalidade idêntica ou atualização sincronizada em cada idioma.

Antes de publicar 1.0.0: revisar atualidade dos 50 READMEs traduzidos e decidir processo de sincronização. **A política de conversão parcial foi alterada na 0.9.33:** as páginas localizadas contêm aviso de segurança introduzido na 0.9.35, atualizado para a referência 0.9.48, mas ainda precisam de tradução editorial integral para não induzir o usuário ao comportamento de cópia antigo. O guia atual pode ser usado como fonte de conteúdo revisado, sem substituir traduções por cópias automáticas sem validação.

## Fontes de verdade

1. Fonte e testes no commit do pacote distribuído.
2. Workflow e artefatos do mesmo commit, com digests e data.
3. Documentação **vigente** deste índice.
4. Registro de alterações e materiais históricos, com data explícita.

Se duas fontes conflitarem, registrar a divergência e corrigir o documento. **Para a futura 1.0, atualizar prioritariamente o arquivo central [RELEASE_STATUS.md](RELEASE_STATUS.md), o README e o CHANGELOG; as descrições de comportamento dos demais guias foram escritas de modo a não exigir troca de número em cada parágrafo.** Não classificar “testado em CI” como “aprovado com Google real”.

- [Aceite RTL 0.9.48 — recuos físicos de Home e Configurações](implementation/rtl-insets-20261024.md), com CI `08dbced` aprovado e registro de que Core/Infrastructure não mudaram.
