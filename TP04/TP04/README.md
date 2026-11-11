# TP04 - Web API com ASP.NET Core e CRUD em Página Web

Disciplina CBTSWE2 (ADS 671) - IFSP Campus Cubatão - Prof. Wellington Tuler Moraes

Sistema de gerenciamento de produtos composto por uma Web API em ASP.NET Core, uma biblioteca de classes (DLL) integrada à API, persistência com Entity Framework Core + SQLite, uma página web que executa o CRUD e testes unitários.

## Tecnologias

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8 com SQLite
- Swagger (Swashbuckle) para documentação dos endpoints
- xUnit para testes unitários
- HTML + JavaScript (fetch) para a página web

## Estrutura

```
TP04/
├── TP04.Api/            Web API, EF Core e página web (wwwroot)
│   ├── Controllers/     ProdutosController
│   ├── Data/            AppDbContext
│   ├── Middleware/      TratamentoExcecaoMiddleware
│   ├── Models/          Produto, ProdutoDto, ValorEstoqueDto
│   └── wwwroot/         index.html
├── TP04.Biblioteca/     DLL com validação e cálculo de estoque
├── TP04.Tests/          Testes unitários (xUnit)
└── docs/img/            Capturas de tela
```

## Configuração do ambiente

1. Instalar o [.NET SDK 8.0](https://dotnet.microsoft.com/download).
2. Nenhuma instalação de banco é necessária: o SQLite usa o arquivo `produtos.db`, criado automaticamente na primeira execução em `TP04.Api/`.

A string de conexão fica em `TP04.Api/appsettings.json` (`ConnectionStrings:Padrao`).

## Como executar

Criar a solução (uma única vez):

```
dotnet new sln -n TP04
dotnet sln add TP04.Biblioteca TP04.Api TP04.Tests
```

Executar a aplicação:

```
dotnet run --project TP04.Api
```

- Página web: http://localhost:5080
- Swagger: http://localhost:5080/swagger

Executar os testes:

```
dotnet test
```

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/produtos` | Lista todos os produtos |
| GET | `/api/produtos/{id}` | Obtém um produto |
| GET | `/api/produtos/{id}/valor-estoque` | Valor total em estoque (DLL) |
| POST | `/api/produtos` | Cadastra um produto |
| PUT | `/api/produtos/{id}` | Atualiza um produto |
| DELETE | `/api/produtos/{id}` | Exclui um produto |

Corpo de POST e PUT:

```json
{
  "nome": "Teclado",
  "descricao": "Teclado mecânico",
  "preco": 250.90,
  "quantidade": 10
}
```

Respostas: `200`, `201`, `204`, `400` (com a lista `erros`), `404` e `500` (tratado pelo middleware).

## Integração com a DLL

O projeto `TP04.Biblioteca` gera a `TP04.Biblioteca.dll`, referenciada pela API em `TP04.Api.csproj`.

- `ValidadorProduto`: valida nome (obrigatório, até 100 caracteres), preço (maior que zero) e quantidade (não negativa). Usado em `POST` e `PUT`.
- `CalculadoraEstoque`: calcula e formata em reais o valor total em estoque. Usada em `GET /api/produtos/{id}/valor-estoque`.

## Boas práticas aplicadas

- Separação em projetos (API, biblioteca, testes) e pastas por responsabilidade.
- Nomenclatura em PascalCase para tipos e métodos e camelCase para variáveis locais.
- Injeção de dependência do `AppDbContext`.
- DTO separado da entidade para entrada de dados.
- Tratamento de exceções centralizado em middleware, com log e resposta `500` padronizada.
- Validação de entrada e códigos HTTP adequados a cada situação.

## Testes unitários

`TP04.Tests` cobre todas as ações do controller (sucesso, validação e não encontrado) com o provedor InMemory do EF Core, além das classes da DLL.

## Funcionalidades da página web

![Listagem](docs/img/listagem.png)

1. **Listagem**: a tabela exibe id, nome, descrição, preço e quantidade dos produtos cadastrados.

![Cadastro](docs/img/cadastro.png)

2. **Cadastro**: o formulário envia `POST /api/produtos`; erros de validação retornados pela API aparecem acima da tabela.

![Edição](docs/img/edicao.png)

3. **Edição**: o botão *Editar* carrega o produto no formulário e o envio usa `PUT /api/produtos/{id}`.

![Exclusão](docs/img/exclusao.png)

4. **Exclusão**: o botão *Excluir* pede confirmação e chama `DELETE /api/produtos/{id}`.

![Valor em estoque](docs/img/valor-estoque.png)

5. **Valor em estoque**: o botão *Valor em estoque* consulta a API, que usa a DLL para calcular e formatar o total.

## Entrega

- Código-fonte: repositório no GitHub (link a ser informado).
- Vídeo de demonstração: link a ser informado.
