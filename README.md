# iBudget

![GitHub repo size](https://img.shields.io/github/repo-size/allandeba/iBudget)
![GitHub language count](https://img.shields.io/github/languages/count/allandeba/iBudget)

## Visão Geral

Seja bem-vindo ao iBudget, um aplicativo intuitivo para gestão de orçamentos que possibilita a sua organização e manipulação, mantendo históricos de alterações, gerenciando cadastros de pessoas, itens e orçamentos. 

O sistema oferece suporte à geração de orçamentos em formato PDF e permite o compartilhamento direto através do WhatsApp.

## Tecnologias Utilizadas

- **Banco de Dados:** PostgreSQL
- **Framework:** .Net Core 7.0
- **Geração de PDF:** Syncfusion
- **Geração de Dados Falsos:** Bogus
- **Testes Unitários:** xUnit
- **Conteinerização:** Docker
- **Hospedagem:** Servidor VPS Linux
- **Arquitetura:** MVC
- **Reusabilidade de Código:** Partial Pages para evitar replicação de código
- **ORM:** Entity Framework
- **Paginação:** X.PageList
- **UI:** Bootstrap

## 💻 Pré-requisitos debug aplicação

Para efetuar o debug da aplicação com todas as funcionalidade se faz necessário informar os seguintes campos no arquivo "appsettings.Development.json":

```json
"ConnectionStrings": {
  "DB_CONNECTION": "Host=#;Database=#;Username=#;Password=#",
  "SYNC_FUSION_LICENSING": "",
  "USER_PASSWORD": ""
}
DB_CONNECTION: String de conexão com o banco de dados. O sistema está configurado atualmente com uma base remota hospedada na ElephantSQL para testes.
SYNC_FUSION_LICENSING: Licença da Syncfusion necessária para a impressão correta do PDF.
USER_PASSWORD: Senha para a criação do usuário administrador.
```

## 🚀 Instalando <iBudget>

Para instalar o <iBudget>, siga estas etapas:

macOS:

```
1. dotnet build
2. Acesse a pasta gerada em: ./bin/Debug/net7.0/runtimes/osx/
3. Extraia o arquivo Chromium.zip
4. dotnet run
```

Linux, Windows:

```
1. dotnet build
2. dotnet run
```

## ☕ Usando <iBudget>

```
Caso não houver dados no banco, siga estes passos para gerar um orçamento:
1. Adicione uma nova pessoa
2. Adicione novos itens
3. Crie um novo orçamento, selecionando uma pessoa e os itens que o compõem
4. No modo de depuração, o registro da empresa é bloqueado para edição
5. Imprima/Envie o Orçamento
5.1. O compartilhamento via WhatsApp é feito por meio de um link de download direto no WhatsApp da pessoa
```

Aprecie a facilidade de gerenciar orçamentos com o iBudget!
