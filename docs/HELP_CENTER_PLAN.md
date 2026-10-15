# Endereços da futura central de ajuda Zagotools

**Estado:** planejamento de documentação e navegação. Este arquivo **não afirma que a página de ajuda do ZagoSheetsWin já está publicada**.

## Links existentes usados pelo aplicativo

- [Site Zagotools](https://zagotools.top/)
- [Política de privacidade](https://zagotools.top/legal.html#privacidade)
- [Termos de uso](https://zagotools.top/legal.html#termos)
- [Licenciamento](https://zagotools.top/legal.html#licencas)
- [Código e issues do ZagoSheetsWin](https://github.com/zagozago/ZagoSheetsWin)
- [Guia atual versionado](USER_GUIDE.md) e [solução de problemas](TROUBLESHOOTING.md)

Os links legais estão no código atual. A disponibilidade HTTP, texto publicado e requisitos do Google Cloud devem ser conferidos antes da distribuição ampla.

## Endereço de ajuda em discussão

O [PR #12](https://github.com/zagozago/ZagoSheetsWin/pull/12), **ainda não incorporado à branch de produto durante esta revisão**, propõe que o botão de Ajuda abra a URL:

```text
https://zagotools.top/apps/zagosheetswin/ajuda/?lang=<codigo-do-idioma>
```

Esse caminho é **proposta de integração**, não confirmação de página publicada. Não incluí-lo como link ativo de suporte nos READMEs até que a hospedagem exista, o conteúdo esteja revisado e o comportamento do idioma tenha sido testado.

## Checklist de publicação do site

1. Confirmar caminho definitivo e proprietário responsável.
2. Publicar manual de usuário, formatos, limitações, privacidade e recuperação em HTTPS.
3. Conferir links internos, idioma padrão, códigos T51, fallback, dispositivos móveis e acessibilidade.
4. Conferir se idiomas ainda não revisados não exibem conteúdo enganoso.
5. Testar URL com e sem `?lang=` e comportamento para códigos desconhecidos.
6. Só depois alterar link de Ajuda do aplicativo e inserir link público na documentação principal.
7. Registrar aprovação, data de publicação e endereço efetivo, sem supor redirecionamentos.

Até essa validação, a fonte pública de documentação atual é o repositório GitHub.
