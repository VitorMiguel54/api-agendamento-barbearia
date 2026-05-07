*** API de Agendamento de Serviços ***

## Descrição
API REST desenvolvida em .NET 8 para gerenciamento de agendamentos de serviços de barbearia.

O projeto foi desenvolvido utilizando arquitetura em camadas:
- API
- Services
- Domain
- Infra

## Tecnologias utilizadas
- .NET 8
- C#
- SQL Server
- Swagger

## Funcionalidades
### Cliente
- Criar
- Listar
- Buscar por ID
- Atualizar
- Excluir

### Serviço
- Criar
- Listar
- Buscar por ID
- Atualizar
- Excluir

### Profissional
- Criar
- Listar
- Buscar por ID
- Atualizar
- Excluir

### Loja
- Criar
- Listar
- Buscar por ID
- Atualizar
- Excluir

### Agendamento
- Criar agendamento
- Listar agendamentos
- Buscar por ID
- Cancelar
- Finalizar

## Regras de negócio
- Cliente deve possuir email ou telefone
- Não permite duplicidade de cliente
- Serviço deve pertencer a uma loja
- Profissional deve pertencer a uma loja
- Não permite agendamento no passado
- Não permite conflito de horários
- Controle de status do agendamento
- Soft delete

## Como executar
1. Clonar o repositório
2. Executar o script SQL
3. Configurar a connection string
4. Executar a API

## Banco de dados
SQL Server