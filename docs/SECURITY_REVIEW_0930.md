# Revisão de segurança e distribuição — candidato 0.9.30

Data: 2026-10-15. Base revisada: f76ea12fe06d9c19a8ed035045d7df398ac16d69, branch feature/zagosheetswin. Este relatório cobre o aplicativo C# atual; docs/AUDIT.md registra a auditoria histórica do upstream PowerShell.

## Resultado e correções

| Item | Evidência / resultado |
|---|---|
| ODS Mendel | O anexo de 3.811 bytes é HTML com links e script, não um pacote ODS. Falha local na conversão, sem upload. A assinatura de pacote agora é verificada explicitamente e o diagnóstico usa OdsPackage. Não executamos o script. |
| ODS P1-11 | ZIP/ODS válido: três abas, Instrucoes, Vendas e Resumo. Fórmulas, datas, mesclagens e recursos sem verificação de equivalência continuam em modo de cópia, preservando o original. A validação estrutural do anexo foi local; o upload real é validado pelo usuário. |
| Versões | A distribuição protegida compilava o instalador como 0.9.28, enquanto o aplicativo era 0.9.29. O candidato passa a 0.9.30. Os workflows leem a versão do projeto, e a auditoria verifica catálogo, projeto, fallback do instalador e avisos de terceiros. |
| OAuth | Leituras de respostas limitadas a 64 KiB e profundidade JSON limitada; cabeçalho Host único no callback; rejeição de fragmentos/alvos não locais; checagem de cancelamento antes de persistir credenciais renovadas. |
| Escopos | A requisição continua usando somente drive.file. Respostas que informam escopos adicionais são recusadas sem substituir o cofre anterior. Respostas de refresh sem campo scope preservam o comportamento previsto pelo protocolo. |
| Configuração desktop | JSON de cliente web/service account ou com tokens pessoais em installed é recusado. A configuração de distribuição contém apenas client_id/client_secret desktop. |
| Revogação | Credenciais locais são removidas antes da solicitação de revogação. HTTP 400 só é tratado como revogação já efetuada quando o Google informa invalid_token. Outros erros são mostrados; falha de rede não reativa credenciais locais. |
| Pacotes maliciosos | ODS tem assinatura/MIME verificados, XML sem DTD/resolução externa, rejeição de scripts e orçamento de ZIP. A soma dos tamanhos expandidos agora interrompe antes do limite, evitando overflow aritmético. |
| Dependências | Restore com lockfile, NuGetAudit habilitado para dependências diretas e transitivas, relatório JSON de vulnerabilidades e bloqueio se houver vulnerabilidade ou falha de consulta. Evidência em dependency-audit-Windows/Linux do run correspondente ao commit. |
| Integridade da distribuição | SHA256SUMS.txt e build-info.json acompanham o instalador. O manifesto associa versão, commit, run e estado Authenticode ao executável produzido. Não há certificado de assinatura configurado neste código. |

## Controles confirmados no código

- OAuth authorization code com PKCE S256, state aleatório comparado em tempo constante, navegador do sistema e listener 127.0.0.1 em porta aleatória já reservada antes de abrir o navegador. Nenhuma senha Google é coletada.
- Token e refresh token protegidos por DPAPI CurrentUser. E-mail da conta fica no mesmo cofre protegido, apenas para exibição; não é exportado nos diagnósticos.
- Armazenamento privado com ACL do usuário e SYSTEM, recusa de redirecionamentos/reparse points; sessões de upload protegidas, vinculação por conta/hash/payload e URLs HTTPS do endpoint Google permitido.
- Identidade local, locks, snapshot SHA-256, backup verificado e journals persistentes precedem a retirada do original. A associação usa IDs e marcadores, não nomes. Edições locais conflitantes e associações inválidas preservam o original.
- Upload incerto é reconciliado; uma falha não autoriza recriação às cegas. Documento confirmado na lixeira ganha uma nova operação, com o conteúdo local atual; o documento antigo permanece intocado.
- Restauração verifica backup e preserva arquivos existentes. A desinstalação mantém documentos, atalhos e dados recuperáveis. Os testes de ciclo de instalação verificam preservação de estado e preferências.
- XML tem resolução externa desativada; macros/scripts não são executados. O aplicativo não extrai entradas de ZIP para caminhos arbitrários. Formatos complexos sem prova de fidelidade permanecem como cópia.
- Limites: arquivo de entrada de 20 MiB, tabelas com 50 mil registros, 500 mil células, mil colunas; pacotes lidos localmente com orçamento expandido de 96 MiB e XML de até 64 MiB. Há limites adicionais por formato.
- Logs com campos tipados permitidos, quatro arquivos de até 64 KiB e até 96 checkpoints por tentativa. Sem mensagens brutas de exceção, células, caminhos, e-mails, IDs do Drive, tokens ou URLs de upload.
- Nome sugerido ZSW-diagn-{nomeextensaoconcatenados}.jsonl: apenas basename, minúsculas, letras/dígitos, até 100 caracteres. Este nome revela deliberadamente o nome do arquivo ao compartilhar o anexo, conforme solicitado; o conteúdo do JSONL continua sem esse dado e pode incluir histórico de tentativas anteriores.
- Ações GitHub fixadas por SHA, token de workflow com contents:read, ausência de pull_request_target. O JSON OAuth só entra no job de distribuição, protegido pelo Environment zagosheets-distribution e restrito à branch prevista no workflow. Temporário removido em always.
- Varredura da árvore atual por padrões de chaves privadas, segredos OAuth Google e API keys. O script não imprime valores encontrados. Isso é uma checagem limitada de padrões, não uma varredura completa de histórico Git.

## Limites da revisão

Revisão estática do código atual, testes automatizados/offline e verificações do instalador não são certificação de segurança nem teste de invasão. DPAPI/ACL não protegem contra malware executando com a identidade do próprio usuário. Backups são cópias locais legíveis pelo usuário, protegidas por ACL; não há criptografia própria dos backups.

Credenciais de aplicativo desktop são extraíveis de um binário distribuído. O segredo do cliente não funciona como segredo de servidor; a proteção de autorização vem do consentimento, PKCE, state e tokens pessoais. O segredo no Environment evita sua inclusão acidental no Git e nos logs de build.

Checksum comprova correspondência com um digest obtido de fonte confiável; não substitui assinatura de publicador. Certificado Authenticode e reputação SmartScreen dependem de configuração externa. A análise de vulnerabilidades reflete o feed e as versões no momento do build.

## Ações de Fernando antes da distribuição ampla

1. Google Auth Platform → Branding: conferir nome ZagoSheetsWin, logotipo, suporte, homepage e links públicos de privacidade/termos. Conferir propriedade de zagotools.top conforme solicitado pelo Console. As páginas HTTPS não puderam ser validadas pela ferramenta de navegação nesta revisão.
2. Audience: External para terceiros; publicar In production quando pronto. Em Testing, autorizações com drive.file têm restrições de usuários de teste e refresh tokens normalmente expiram em sete dias.
3. Data Access: manter somente https://www.googleapis.com/auth/drive.file entre os escopos usados pelo produto; remover escopos amplos antigos não utilizados.
4. Clients: confirmar cliente Desktop e Drive API habilitada. Preservar o client_id já instalado: trocar a identidade do cliente exige migração e não é uma atualização comum.
5. Se o Console pedir verificação de marca ou exibir aviso de app não verificado, concluir o processo indicado. drive.file é não sensível; o status do projeto e da marca ainda precisa ser conferido no Console. Não prometer ausência de telas de aviso antes dessa conferência.
6. Revisar Environment zagosheets-distribution: revisores obrigatórios, restrição de branch e secrets mínimos. Aprovar somente o run do commit validado.
7. Confirmar elegibilidade/licença de dependências sujeitas a condições, especialmente ImageSharp, e manter os avisos third-party incluídos. A elegibilidade do titular e valores de secrets não foram inspecionados.
8. Se desejar assinatura de publicador, providenciar certificado/serviço Authenticode e configuração protegida; o pacote informa o estado real da assinatura.
9. Testar com conta Google real: autorização, cancelamento, desconexão, reconexão, duas contas, importação dos cinco formatos, restauração e abertura por atalho. Testes de rede automatizados simulam respostas do Google.

## Referências primárias

- OAuth desktop/PKCE/loopback/revogação: https://developers.google.com/identity/protocols/oauth2/native-app
- Escopos Drive e drive.file não sensível: https://developers.google.com/workspace/drive/api/guides/api-specific-auth
- Produção e políticas: https://developers.google.com/identity/protocols/oauth2/production-readiness/policy-compliance
- Verificação de marca: https://developers.google.com/identity/protocols/oauth2/production-readiness/brand-verification
- Auditoria de pacotes: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-package-list
