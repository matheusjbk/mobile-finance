# Mobile Finance

Esta é uma API para controle financeiro, onde é possível adicionar, editar ou excluir entradas e saídas de dinheiro. Também é possível visualizar as transações por meio de calendário. Todas as transações ficam atreladas somente a um usuário.

## Requisitos

- Docker
- Docker Compose

## Instalação

1. Clone o repositório em um diretório

```bash
  git clone https://github.com/matheusjbk/mobile-finance.git
```

2. Dentro do diretório do repositório, gere a imagem da API e rode o projeto

```bash
  docker-compose up -d --build
```

3. No navegador, abra o endereço `http://localhost:5000/swagger/index.html` e comece a testar a aplicação

4. Após finalizar o teste, encerre o projeto

```bash
  docker-compose down -v
```

## Documentação

É possível encontrar todas as rotas e detalhes sobre elas no Swagger: `http://localhost:5000/swagger/index.html`
