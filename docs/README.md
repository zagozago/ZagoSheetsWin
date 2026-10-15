# Documentação do ZagoSheetsWin

**Referência vigente:** código da branch `feature/zagosheetswin`, candidato alfa **0.9.30** em 15/10/2026. Não existe Release 1.0.0 publicada. O README internacional e as traduções mantidas na `main` são incorporados à branch de documentação para preservar a navegação sem rebaixar a documentação do software.

## Guias vigentes (não são documentos de planejamento)

| Documento | Uso |
|---|---|
| [README internacional](../README.md) | Visão do produto, seletor das 51 línguas de documentação, instalação e avisos legais |
| [Manual do usuário](USER_GUIDE.md) | Instalação, primeiro uso, abertura, preferências, backups e restauração |
| [Arquitetura atual](ARCHITECTURE_CURRENT.md) | Componentes reais, dados locais e ciclo de importação |
| [Formatos e limites](FORMATS_AND_LIMITS.md) | Matriz de formato, comportamento de cópia/substituição e limites |
| [Solução de problemas](TROUBLESHOOTING.md) | Erros e procedimentos de recuperação |
| [Checklist de preparação estável](PRE_RELEASE_CHECKLIST.md) | Critérios pendentes antes de anunciar 1.0.0 |
| [Plano da central de ajuda](HELP_CENTER_PLAN.md) | Links canônicos atuais e URLs futuras ainda não publicadas |
| [Changelog](../CHANGELOG.md) | Linha do tempo com commits, sem inventar tags/Releases |
| [Contribuição](../CONTRIBUTING.md) | Compilação, testes, segurança e PRs |

## Referências técnicas auxiliares

- [Segurança e dependências — fotografia 0.9.30](SECURITY_REVIEW_0930.md).
- [Integração dos 51 idiomas](I18N_INTEGRATION.md).
- [Conversão XLS e relatórios HTML com extensão XLS](XLS_CONVERSION.md).
- [Associações na lixeira e JSONL de diagnóstico](diagnostics-and-trashed-associations.md).
- [Decisões arquiteturais](adr/DECISIONS.md): justificam escolhas **no momento em que foram feitas**; quando houver divergência, o código e o guia atual prevalecem.
- [Auditoria upstream](AUDIT.md): auditoria do projeto original, não implementação atual.

## Histórico preservado, não vigente

Consulte [HISTORY.md](HISTORY.md) para fases `STAGE_*`, `ALPHA_*`, `ALPHA_NEXT.md`, `ROADMAP.md`, `ARCHITECTURE.md` (arquitetura **proposta**, não necessariamente atual), `DEVELOPMENT.md` e documentação de UI/traduções de momentos anteriores.

**Política de conservação:** links históricos não são alterados ou apagados silenciosamente. A documentação atual fica em arquivos com nomes próprios e este índice separa os dois contextos. Não use um pacote ou limite de um registro histórico como orientação operacional. A documentação do upstream em `../SETUP.md` não instala o atual produto C#.

## Documentação em 51 idiomas

O [README raiz](../README.md) apresenta o seletor de documentação: inglês + 50 arquivos em `docs/i18n/<código>/README.md`. Esses arquivos foram preservados a partir da `main` como **traduções históricas existentes**. A interface e o instalador usam 51 idiomas a partir de catálogos próprios; a existência de traduções não certifica revisão editorial, funcionalidade idêntica ou atualização sincronizada em cada idioma.

Antes de publicar 1.0.0: revisar atualidade dos 50 READMEs traduzidos e decidir processo de sincronização. O guia atual pode ser usado como fonte de conteúdo revisado, sem substituir traduções por cópias automáticas sem validação.

## Fontes de verdade

1. Fonte e testes no commit do pacote distribuído.
2. Workflow e artefatos do mesmo commit, com digests e data.
3. Documentação **vigente** deste índice.
4. Registro de alterações e materiais históricos, com data explícita.

Se duas fontes conflitarem, registrar a divergência e corrigir o documento. Não classificar “testado em CI” como “aprovado com Google real”.
