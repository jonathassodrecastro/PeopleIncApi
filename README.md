# API de Gerenciamento de Pessoas

Esta API foi desenvolvida para fornecer operações básicas de gerenciamento de pessoas, incluindo adição, remoção, atualização e obtenção de informações de pessoas. Além disso, a API oferece suporte à importação de dados de pessoas a partir de um arquivo CSV.

## Endpoints Disponíveis

- **GET /api/Pessoa**: Retorna todas as pessoas cadastradas.
- **GET /api/Pessoa/{id}**: Retorna informações sobre uma pessoa específica com o ID fornecido.
- **POST /api/Pessoa**: Adiciona uma nova pessoa com base nos dados fornecidos.
- **PUT /api/Pessoa/{id}**: Atualiza as informações de uma pessoa existente com o ID fornecido.
- **DELETE /api/Pessoa/{id}**: Remove uma pessoa do sistema com base no ID fornecido.
- **POST /api/Pessoa/Upload**: Importa dados de pessoas de um arquivo CSV.

## Como Usar

### Requisitos

- .NET Core SDK
- SQL Server (ou outro banco de dados suportado pelo Entity Framework Core)

### Configuração

1. Clone este repositório em seu ambiente local.
2. Execute o comando `dotnet restore` para restaurar as dependências do projeto.
3. Configure a conexão com o banco de dados no arquivo `appsettings.json`.
4. Execute as migrações do banco de dados usando o comando `dotnet ef database update`.

### Execução

1. Navegue até o diretório raiz do projeto.
2. Execute o comando `dotnet run` para iniciar o servidor da API.
3. Acesse os endpoints da API usando uma ferramenta como Postman ou qualquer cliente HTTP.

## Exemplos de Requisições

### Adicionar Pessoa

```http
POST /api/Pessoa
Content-Type: application/json

{
  "nome": "John Doe",
  "idade": 30,
  "email": "john.doe@example.com"
}
```

### Atualizar Pessoa

```http
PUT /api/Pessoa/1
Content-Type: application/json

{
  "id": 1,
  "nome": "Jane Doe",
  "idade": 25,
  "email": "jane.doe@example.com"
}
```

### Importar Dados de Pessoas de um Arquivo CSV

```http
POST /api/Pessoa/Upload
Content-Type: multipart/form-data
```

Anexe um arquivo CSV contendo os dados das pessoas no corpo da requisição.

## Contribuindo

Contribuições são bem-vindas! Se você encontrar um problema, tem uma sugestão ou deseja adicionar uma nova funcionalidade, sinta-se à vontade para abrir uma issue ou enviar um pull request.

## Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).
