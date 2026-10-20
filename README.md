# iBudget

Projeto de portfólio para gerenciar orçamentos comerciais, feito com .NET 8, ASP.NET Core MVC, Razor e PostgreSQL.

[Experimentar a demonstração](https://ibudget.allandeba.dev.br) · [LinkedIn](https://www.linkedin.com/in/allan-debastiani/)

## Funcionalidades

- Cadastro de pessoas com contatos e documentos, além de itens com imagens e catálogo visual.
- Orçamentos com quantidades, descontos e histórico de alterações.
- Cadastro da empresa e geração de PDF com Playwright.
- Compartilhamento de um link para baixar o PDF pelo WhatsApp, com confirmação de envio pelo usuário.

Comece por Pessoas, depois cadastre os Itens e crie os Orçamentos. Os links para imprimir e baixar orçamentos podem ser acessados sem login.

## Tecnologias

Os controllers recebem as requisições, Business reúne as regras de negócio e Repository acessa o banco com Entity Framework Core/Npgsql. As telas usam Razor, Bootstrap, jQuery e Select2. Bogus gera dados fictícios; ImageSharp trata imagens. Os testes usam xUnit e Selenium.

## Executar localmente

Para rodar localmente, você precisa de:

- SDK e runtimes .NET/ASP.NET Core 8.
- PostgreSQL 14 ou superior com a extensão `unaccent` e permissão para aplicar migrações.
- PowerShell 7 (`pwsh`) e internet para instalar o Chromium headless.
- Internet para obter imagens fictícias do Picsum.

> **Use um banco exclusivo e descartável.** Os perfis locais usam `Development`. Nesse ambiente, o serviço de reset executa `DROP TABLE ... CASCADE` ao iniciar e a cada oito horas, e recria os dados de exemplo. A aplicação também bloqueia a edição da empresa.

Na raiz do repositório, configure os valores locais com user secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DB_CONNECTION" "Host=localhost;Database=ibudget_demo;Username=ibudget;Password=<senha-do-banco>" --project src/iBudget.csproj
dotnet user-secrets set "ConnectionStrings:USER_PASSWORD" "<senha-local-do-usuario>" --project src/iBudget.csproj

dotnet restore iBudget.sln
dotnet build iBudget.sln --no-restore
pwsh src/bin/Debug/net8.0/playwright.ps1 install --only-shell chromium
dotnet run --project src/iBudget.csproj --launch-profile http
```

Abra [localhost:5105](http://localhost:5105). O perfil `https` usa [localhost:7186](https://localhost:7186) e exige certificado de desenvolvimento confiável.

`USER_PASSWORD` define a senha do usuário inicial quando não há login cadastrado. Em Development, apenas o usuário `admin` vem preenchido. Informe a senha cadastrada; a aplicação não envia `USER_PASSWORD` ao HTML. Alterar essa variável não modifica a senha de um usuário já existente.

A aplicação consulta `ConnectionStrings` e depois as variáveis `DB_CONNECTION` e `USER_PASSWORD`. Ela também carrega o arquivo `.env`, ignorado pelo Git, e aplica as migrações ao iniciar.

A geração de PDF usa Playwright com Chromium headless. Valide a exportação em Linux com as dependências do [Dockerfile](Dockerfile). Em Development, os PDFs recebem uma marca d’água de teste. `Pdf:NavigationTimeoutSeconds` controla o carregamento do HTML e `Pdf:MaxConcurrentPages` limita conversões simultâneas.

O comando de instalação do Chromium acima precisa ser executado antes do primeiro F5 ou `dotnet run` e repetido após atualizar `Microsoft.Playwright`. A aplicação reutiliza o cache, sem baixar navegadores ao iniciar. Se definir `PLAYWRIGHT_BROWSERS_PATH`, use o mesmo valor na instalação e na execução.

O Dockerfile instala o navegador em `/ms-playwright` durante o build. Uma falha de rede pode interromper essa instalação; o container em execução usa o navegador incluído na imagem.

<details>
<summary>Falha de download no Playwright 1.63</summary>

O Playwright 1.63 pode falhar ao baixar o navegador quando há uma rota IPv6 sem acesso à internet. A correção está prevista para a versão 1.64: [#42769](https://github.com/microsoft/playwright/issues/42769), [#43186](https://github.com/microsoft/playwright/issues/43186) e [#42660](https://github.com/microsoft/playwright/pull/42660). A falha depende da rede e também pode afetar o build Docker.

Para recuperar a instalação no Windows x64, execute o procedimento abaixo na raiz do repositório com PowerShell 7. Ele usa o build Debug, lê a versão do pacote compilado e baixa o arquivo oficial do Chrome for Testing para o cache padrão. Execute somente se a pasta da revisão ainda não existir; ajuste o destino se usar `PLAYWRIGHT_BROWSERS_PATH`.

```powershell
$metadata = Get-Content src/bin/Debug/net8.0/.playwright/package/browsers.json -Raw | ConvertFrom-Json
$browser = $metadata.browsers | Where-Object name -eq 'chromium-headless-shell'
$destination = Join-Path $env:LOCALAPPDATA "ms-playwright/chromium_headless_shell-$($browser.revision)"
if (Test-Path -LiteralPath $destination) { throw 'A pasta da revisão já existe; verifique a instalação antes de continuar.' }
$archive = Join-Path $env:TEMP "chromium-headless-shell-$($browser.revision).zip"
curl.exe --fail --location --output $archive "https://storage.googleapis.com/chrome-for-testing-public/$($browser.browserVersion)/win64/chrome-headless-shell-win64.zip"
if ($LASTEXITCODE -ne 0) { throw 'Falha ao baixar o arquivo oficial do Chromium.' }
Expand-Archive -LiteralPath $archive -DestinationPath $destination -ErrorAction Stop
New-Item -ItemType File -Path (Join-Path $destination 'INSTALLATION_COMPLETE') -ErrorAction Stop | Out-Null
pwsh src/bin/Debug/net8.0/playwright.ps1 install --only-shell chromium
```

O último comando confirma a instalação e registra o cache no Playwright.

</details>

### Acesso à demonstração

A demonstração vem desabilitada. Para ativá-la, pare a aplicação e execute no mesmo terminal:

```powershell
$env:Demo__Enabled = 'true'
dotnet run --project src/iBudget.csproj --launch-profile http
```

Em `Development`, clique em **Explorar demonstração** para entrar como "Visitante", sem informar senha. A sessão dura até oito horas e não se renova com o uso. Os visitantes compartilham os mesmos dados, por isso use informações fictícias. O servidor bloqueia o cadastro de contas e as alterações da empresa.

No container, configure `Demo__Enabled=true` e `ASPNETCORE_ENVIRONMENT=Development` somente com um banco descartável. A demonstração não funciona em Production ou Staging. Ao desativar a opção, o servidor rejeita as sessões demo na próxima requisição autenticada.

## Testes

A suíte inclui testes unitários e HTTP. Os testes HTTP usam um banco em memória, sem PostgreSQL, imagens externas, download de navegador ou serviço de reset:

```powershell
dotnet test test/iBudget.UnitTest/iBudget.UnitTest.csproj
```

Para rodar os testes Selenium, instale o Chrome e deixe a aplicação em execução em Development, com a demonstração habilitada conforme a seção anterior. Em outro terminal, execute:

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:IBUDGET_TEST_URL = 'http://127.0.0.1:5105/Login'
dotnet test test/iBudget.AutomatedTest/iBudget.AutomatedTest.csproj
```

Se a demonstração estiver desligada, o helper Selenium preenche `Constants.DefaultUsername` e a variável `USER_PASSWORD` do executor de testes. Essa senha precisa corresponder à senha cadastrada no banco de homologação. No GitHub Actions, configure o secret `USER_PASSWORD`, repassado ao passo de testes; o arquivo de ambiente do container não é compartilhado com o executor. Com Demo ligado, Selenium não precisa dessa senha.

**Os testes Selenium alteram dados.** Confira o `.env`, pois ele pode sobrescrever as variáveis acima. Sem uma URL explícita, a fixture usa localhost em Development e a homologação remota nos demais ambientes. Para verificar o projeto localmente, prefira os comandos por projeto em vez de rodar `dotnet test` na solução inteira.

A validação da data de criação de pessoas usa `America/Sao_Paulo`, independentemente do fuso do executor. Se a sessão PostgreSQL usar outro fuso, configure `IBUDGET_TEST_DATABASE_TIMEZONE` com o mesmo identificador (por exemplo, `UTC`). O banco gera essa data com `CURRENT_TIMESTAMP`; a edição deve preservar a data original.

## Publicação e validação

Um push em `main` aciona o [workflow](.github/workflows/ibudget.yml): ele compila o projeto, executa os testes unitários e implanta em homologação. Depois dos testes Selenium, implanta em produção. Os secrets do GitHub guardam os endereços, as credenciais e os caminhos dos arquivos de ambiente.

O Dockerfile usa .NET 8, a porta HTTP `80` e instala somente o Chromium headless necessário para gerar PDFs. Informe a conexão com o banco ao executar o container.
