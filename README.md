# Projeto de Seguros com Arquitetura Hexagonal

Este projeto implementa um sistema de gerenciamento de propostas de seguro e contratação, utilizando uma arquitetura hexagonal (Ports & Adapters) e microserviços em .NET 8 e C#.




## Estrutura do Projeto

O projeto é composto por dois microserviços e três bibliotecas de infraestrutura, organizados da seguinte forma:

- **ContratacaoApi**: Microserviço responsável pela contratação de propostas de seguro. Expõe uma API REST.
- **ContratacaoApi.Tests**: Projeto de testes unitários para o `ContratacaoApi`.
- **PropostaApi**: Microserviço responsável pela criação, listagem e alteração de status de propostas de seguro. Expõe uma API REST.
- **PropostaApi.Tests**: Projeto de testes unitários para o `PropostaApi`.
- **Common.Domain**: Biblioteca que contém entidades de domínio, agregados e interfaces comuns a todos os microserviços, seguindo os princípios de Domain-Driven Design (DDD).
- **Common.Infrastructure.Data.EFCore**: Biblioteca de infraestrutura para acesso a dados, utilizando Entity Framework Core para persistência de dados.
- **Common.Infrastructure.ServiceAgent.Http**: Biblioteca de infraestrutura para comunicação entre microserviços via HTTP, atuando como um Service Agent.




## Pré-requisitos

Para executar este projeto, você precisará ter instalado:

- **.NET 8 SDK**: O SDK do .NET 8 é necessário para compilar e executar as aplicações.
- **Docker Desktop**: Essencial para executar as APIs e o banco de dados SQL Server em contêineres.
- **Visual Studio 2022 (ou superior)**: Recomendado para desenvolvimento e depuração, com as cargas de trabalho de desenvolvimento web e Docker instaladas.




## Configuração e Execução

Este projeto foi projetado para ser executado facilmente utilizando Docker e Docker Compose.

### 1. Configuração do Banco de Dados

Um arquivo `docker-compose.yml` está disponível na raiz do projeto para facilitar a criação de uma instância padrão do SQL Server em um contêiner Docker. Além disso, um script `init.sql` é fornecido na pasta `db-init` para criar os bancos de dados necessários para a aplicação.

Para iniciar o contêiner do SQL Server e criar os bancos de dados, execute o seguinte comando na raiz do projeto:

```bash
docker-compose up -d
```

As aplicações (`ContratacaoApi` e `PropostaApi`) são configuradas para executar as migrations necessárias na inicialização, criando automaticamente a estrutura do banco de dados.

### 2. Execução via Visual Studio

Para executar o projeto diretamente pelo Visual Studio, siga os passos abaixo:

1. Abra a solução do projeto no Visual Studio.
2. Na janela `Solutoin Explorer`, clique com o botão direito na solução e selecione `Configure Startup projects...`.
3. Escolha a opção `Multiple Startup projects`.
4. Para `ContratacaoApi` e `PropostaApi`, defina a Ação como `Start`.
5. Certifique-se de que a configuração de depuração para ambos os projetos esteja definida para `Container (Docker)`.
6. Pressione `F5` ou clique no botão `Iniciar` para compilar e executar as APIs em contêineres Docker gerenciados pelo Visual Studio.

