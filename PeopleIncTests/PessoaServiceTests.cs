using Moq;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;
using X.PagedList;


namespace PeopleIncTests
{
    public class PessoaServiceTests
    {
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

        //Método para chamar apenas uma pessoa de exemplo por ID
        private Pessoa GetSamplePerson(int id)
        {
            return new Pessoa { Id = id, Nome = "Alice", Idade = 25, Email = "alice@example.com" };
        }
    }
}