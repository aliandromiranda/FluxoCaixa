Controle de Fluxo de Caixa (C# - ASP.NET Core)

Este projeto implementa uma API para controle de lançamentos financeiros (débitos e créditos) e geração de relatório consolidado diário.


#Tecnologias utilizadas
- C# 10 / .NET 7
- ASP.NET Core Web API
- *Entity Framework Core + SQLite
- Swagger / OpenAPI**

---

Estrutura do projeto

FluxoCaixa/
src/
	FluxoCaixa.Api/                - API principal (Controllers)
	FluxoCaixa.Core/               - Modelos, DTOs e Enums
	FluxoCaixa.Infrastructure/     - Banco de dados e Serviços


# Configuração e execução local

# Pré-requisitos
- .NET SDK 7.0 ou superior instalado

# Para executar o projeto
No PowerShell, CMD ou terminal, acesse a pasta  src/FluxoCaixa.Api e execute:

dotnet restore
dotnet run

A aplicação será iniciada em https://localhost:5157/swagger.


#Endpoints 

# Criar lançamento
POST /api/transactions

Body (JSON):
json
{
  "date": "2025-10-01T10:30:00",
  "amount": 150.50,
  "type": "Credit",
  "description": "Venda cartão"
}




#Listar lançamentos
GET /api/transactions?from=2025-10-01&to=2025-10-07

Retorna todos os lançamentos no período especificado.



# Relatório consolidado diário
GET /api/consolidated?from=2025-10-01&to=2025-10-07&initialBalance=1000

Retorna uma lista de dias com o total diário e saldo acumulado.



# Arquitetura e justificativa
- Arquitetura em camadas: API, domínio e infraestrutura.
- SQLite: banco leve e portável, próprio para demonstração deste projeto.
- Entity Framework Core: abstração de acesso a dados com suporte a migraçõa
- Swagger: documentação automática dos endpoints, com interface gráfica



# Melhorias específicas dentro deste projeto
- Adicionar autenticação JWT.
- Configurar CI/CD (GitHub Actions ou Azure DevOps).


