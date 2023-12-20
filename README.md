# Projeto de Liberação de Crédito

Este repositório é composto por três componentes principais: uma API em .NET, scripts SQL para manipulação de banco de dados e informações sobre microserviços.

## 1. API

A API é desenvolvida em .NET 6.0 e utiliza as seguintes tecnologias:

- **FluentValidations**: Biblioteca para validação de dados de entrada de forma elegante e fluente.
- **xUnit**: Framework de testes para garantir a qualidade do código.
- **Moq**: Framework de mocking para facilitar a criação de objetos simulados durante os testes.

### Configuração e Execução

Certifique-se de ter o .NET 6.0 instalado em sua máquina. Para executar a API, siga os passos abaixo:

```bash
cd 1.API
cd LiberacaoCredito.Api
dotnet restore
dotnet build
dotnet run
```

### Testes
Execute os testes para garantir a integridade do código:
```bash
cd 1.API
cd LiberacaoCredito.Tests
dotnet restore
dotnet test
```

### Utilizando Dockerfile
Cria a imagem e executa o contêiner.

```bash
cd 1.API
docker build -t liberacao-credito .
docker run -p 8080:80 liberacao-credito
```

## 2. SQL

Esta Pasta contem o SCRIPT SQL, para o SQL SERVER

### A seguir, uma descrição textual do que está sendo feito no script:

Função de Validação de CPF:
- Uma função chamada ValidarCPF é criada para validar a integridade de números de CPF. Essa função retorna 1 se o CPF for válido e 0 caso contrário.

Tabela Cliente:
- Uma tabela chamada "Cliente" é criada, contendo colunas como CPF (chave primária), Nome, UF (Unidade Federativa) e Celular.
- A restrição CHECK é aplicada à coluna CPF usando a função ValidarCPF, garantindo que apenas CPFs válidos sejam aceitos.

Tabela Financiamento:
- Uma tabela chamada "Financiamento" é criada, contendo colunas como IdFinanciamento, CPF (chave estrangeira referenciando a tabela Cliente), TipoFinanciamento, ValorTotal e DataUltimoVencimento.

Tabela Parcela:
- Uma tabela chamada "Parcela" é criada, contendo colunas como IdParcela, IdFinanciamento (chave estrangeira referenciando a tabela Financiamento), NumeroParcela, ValorParcela, DataVencimento e DataPagamento.

Índices:
- Índices são criados para melhorar o desempenho de consultas fictícias, como índices na coluna UF da tabela Cliente e nas colunas DataVencimento e DataPagamento da tabela Parcela.

Inserção de Dados:
- Diversos clientes são inseridos na tabela Cliente, alguns com financiamentos e parcelas associadas.

Consultas SQL:
 - Duas consultas SQL são realizadas:
- Consulta 1: Lista todos os clientes do estado de SP que possuem mais de 60% das parcelas pagas.
- Consulta 2: Lista os primeiros quatro clientes que possuem alguma parcela com mais de cinco dias sem atraso.
Essencialmente, este script cria uma estrutura de banco de dados para um sistema de financiamento, permitindo a validação de CPF, gerenciamento de clientes, financiamentos e parcelas, além de realizar consultas específicas para obter informações úteis.

## 3.Microsservicos

Por último a pasta Microsservicos contem uma breve explicação do que é um microsservico, e consta também um exemplo em diagrama e a explicação do mesmo.
