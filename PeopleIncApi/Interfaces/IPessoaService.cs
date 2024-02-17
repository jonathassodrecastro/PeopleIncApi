using PeopleIncApi.Models;
using X.PagedList;

namespace PeopleIncApi.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<Pessoa>> GetAllPessoas();
        Task<Pessoa> GetPessoa(long id);
        Task AddPessoa(string nome, int idade, string email);
        Task UpdatePessoa(int id, Pessoa pessoa);
        Task DeletePessoa(int id);
        Task UploadCSV(IFormFile file);
        Task<IPagedList<Pessoa>> GetPessoasPaginadas(int pageNumber, int pageSize);
    }
}