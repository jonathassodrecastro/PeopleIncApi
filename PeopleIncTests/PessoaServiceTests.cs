using Moq;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;
using X.PagedList;


namespace PeopleIncTests
{
    public class PessoaServiceTests
    {
        [Fact]
        public async Task GetPessoasPaginadas_ReturnsPagedList()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var mockService = new Mock<IPessoaService>();

            mockService.Setup(s => s.GetAllPessoas()).ReturnsAsync(GetSamplePeople()); // Configure o comportamento do mock

            var pessoaService = mockService.Object;

            // Act
            var result = await pessoaService.GetPessoasPaginadas(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result); // Verifique se o resultado não é nulo
            Assert.IsAssignableFrom<IPagedList<Pessoa>>(result); // Verifique se o resultado é uma instância de IPagedList<Pessoa>
        }

        [Fact]
        public async Task GetPessoa_Id_ReturnsJson()
        {
            var id = 1;

            var mockService = new Mock<IPessoaService>();

            mockService.Setup(s => s.GetPessoa(id)).ReturnsAsync(GetSamplePerson(id));

            var pessoaService = mockService.Object;

            var result = await pessoaService.GetPessoa(id);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pessoa>(result);
        }

        // Método de exemplo para criar uma lista de pessoas para simular dados retornado
        private IEnumerable<Pessoa> GetSamplePeople()
        {
            // Crie uma lista de amostra de pessoas para serem usadas no teste
            return new List<Pessoa>
            {
                new Pessoa { Id = 1, Nome = "Alice", Idade = 25, Email = "alice@example.com" },
                new Pessoa { Id = 2, Nome = "Bob", Idade = 30, Email = "bob@example.com" },
                new Pessoa { Id = 3, Nome = "Carol", Idade = 28, Email = "carol@example.com"},
                new Pessoa { Id = 4, Nome = "David", Idade = 35, Email = "david@example.com"},
                new Pessoa { Id = 5, Nome = "Eve", Idade = 22, Email = "eve@example.com"}
            };
        }

        //Método para chamar apenas uma pessoa de exemplo por ID
        private Pessoa GetSamplePerson(int id)
        {
            return new Pessoa { Id = id, Nome = "Alice", Idade = 25, Email = "alice@example.com" };
        }
    }
}