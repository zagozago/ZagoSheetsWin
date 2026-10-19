# Endereços da futura central de ajuda Zagotools

**Estado conferido na 0.9.42:** o aplicativo já contém código para abrir a URL de ajuda no navegador, porém **a publicação e disponibilidade da página não foram comprovadas**. Este arquivo não apresenta a URL como central de ajuda já disponível. [Base do produto e CI](RELEASE_STATUS.md).

## Links existentes usados pelo aplicativo

- [Site Zagotools](https://zagotools.top/)
- [Política de privacidade](https://zagotools.top/legal.html#privacidade)
- [Termos de uso](https://zagotools.top/legal.html#termos)
- [Licenciamento](https://zagotools.top/legal.html#licencas)
- [Código e issues do ZagoSheetsWin](https://github.com/zagozago/ZagoSheetsWin)
- [Guia atual versionado](USER_GUIDE.md) e [solução de problemas](TROUBLESHOOTING.md)

Os links legais estão no código atual. A disponibilidade HTTP, texto publicado e requisitos do Google Cloud devem ser conferidos antes da distribuição ampla.

## Endereço implementado no aplicativo, publicação ainda não comprovada

O código **atual** de `LauncherForm.cs` na `feature/zagosheetswin` já usa o botão **Ajuda** para tentar abrir a URL abaixo e acrescenta o código do idioma selecionado. O [PR #12](https://github.com/zagozago/ZagoSheetsWin/pull/12) também permanece aberto e deve ser conciliado; seu estado não significa que a integração esteja ausente do código principal:

```text
https://zagotools.top/apps/zagosheetswin/ajuda/?lang=<codigo-do-idioma>
```

**O endereço está codificado no aplicativo**, mas não foi possível verificar seu funcionamento pelo acesso web desta revisão. Não promovê-lo como link garantido de suporte nos READMEs até que a hospedagem exista, o conteúdo esteja revisado e o comportamento do idioma seja testado. Se o navegador falhar ao abrir, o aplicativo pode exibir o endereço para consulta manual, mas isso não prova que a página responda.

## Checklist de publicação do site

1. Confirmar caminho definitivo e proprietário responsável.
2. Publicar manual de usuário, formatos, limitações, privacidade e recuperação em HTTPS.
3. Conferir links internos, idioma padrão, códigos T51, fallback, dispositivos móveis e acessibilidade.
4. Conferir se idiomas ainda não revisados não exibem conteúdo enganoso.
5. Testar URL com e sem `?lang=` e comportamento para códigos desconhecidos.
6. Só depois alterar link de Ajuda do aplicativo e inserir link público na documentação principal.
7. Registrar aprovação, data de publicação e endereço efetivo, sem supor redirecionamentos.

Até essa validação, a fonte pública de documentação atual é o repositório GitHub.
