# API de Agendamento para Barbearia

API REST em .NET 8 para gerenciar clientes, lojas, profissionais, servicos e agendamentos de uma barbearia. O projeto foi organizado em camadas para separar responsabilidades entre API, servicos, dominio e infraestrutura.

## Objetivo

Demonstrar uma solucao backend completa para agendamento de servicos, com regras de negocio, autenticacao por API Key, persistencia em SQL Server, validacoes e testes automatizados.

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- C#
- SQL Server
- ADO.NET
- FluentValidation
- Swagger / OpenAPI
- xUnit
- Moq

## Funcionalidades principais

- Cadastro, listagem, consulta, atualizacao e exclusao logica de clientes
- Cadastro, listagem, consulta, atualizacao e exclusao logica de lojas
- Cadastro, listagem, consulta, atualizacao e exclusao logica de profissionais
- Cadastro, listagem, consulta, atualizacao e exclusao logica de servicos
- Associacao de servicos a lojas com preco e duracao
- Criacao, consulta, atualizacao e cancelamento de agendamentos
- Validacao de horario de funcionamento da loja
- Prevencao de agendamentos no passado
- Prevencao de conflito de horario para o mesmo profissional
- Autenticacao por API Key

## Autenticacao

Os endpoints protegidos exigem o header `X-Api-Key`.

Exemplo:

```http
GET /api/cliente HTTP/1.1
Host: localhost:5266
X-Api-Key: dev-api-key
Accept: application/json
```

Em ambiente de desenvolvimento, o valor local pode ser configurado em `SolucaoBarbearia.api/appsettings.Development.json`.

## Configuracao

A connection string nao deve conter usuario ou senha real versionados no repositorio.

Opcao recomendada por variavel de ambiente:

```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost;Database=Barbearia;Trusted_Connection=True;TrustServerCertificate=True"
$env:Authentication__ApiKey="sua-api-key-local"
```

Tambem e possivel usar um valor local ficticio em `SolucaoBarbearia.api/appsettings.Development.json` para desenvolvimento.

## Banco de dados

O script de criacao do banco esta em `script_banco.sql.txt`.

Passos sugeridos:

1. Abra o SQL Server Management Studio, Azure Data Studio ou ferramenta equivalente.
2. Crie ou selecione o banco de dados local.
3. Execute o conteudo de `script_banco.sql.txt`.
4. Configure `ConnectionStrings__DefaultConnection` ou `appsettings.Development.json` apontando para esse banco.

## Como executar

Restaurar e compilar:

```powershell
dotnet build SolucaoAgendamento_Barbearia.sln
```

Executar testes:

```powershell
dotnet test SolucaoAgendamento_Barbearia.sln --no-build
```

Executar a API:

```powershell
dotnet run --project SolucaoBarbearia.api/SolucaoBarbearia.api.csproj
```

Swagger em desenvolvimento:

```text
http://localhost:5266/swagger
```

## Endpoints principais

Clientes:

```http
GET /api/cliente
GET /api/cliente/{id}
POST /api/cliente
PUT /api/cliente/{id}
DELETE /api/cliente/{id}
```

Lojas:

```http
GET /api/loja
GET /api/loja/{id}
POST /api/loja
PUT /api/loja/{id}
DELETE /api/loja/{id}
```

Profissionais:

```http
GET /api/profissional
GET /api/profissional/{id}
POST /api/profissional
PUT /api/profissional/{id}
DELETE /api/profissional/{id}
```

Servicos:

```http
GET /api/servico
GET /api/servico/{id}
POST /api/servico
PUT /api/servico/{id}
DELETE /api/servico/{id}
```

Agendamentos:

```http
GET /api/agendamento
GET /api/agendamento/{id}
POST /api/agendamento
PUT /api/agendamento/{id}
DELETE /api/agendamento/{id}
```

## Estrutura da solucao

```text
SolucaoBarbearia.api       API, controllers, autenticacao e Swagger
SolucaoBarbearia.servico   Regras de negocio, servicos e validacoes
SolucoaBarbearia.dominio   Modelos e interfaces de dominio
SolucaoBarbearia.infra     Repositorios e acesso a dados
SolucaoBarbeariaTests      Testes automatizados
```
