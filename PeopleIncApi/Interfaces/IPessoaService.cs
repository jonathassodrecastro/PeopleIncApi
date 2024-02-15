using Microsoft.AspNetCore.Mvc;
using PeopleIncApi.Models;

namespace PeopleIncApi.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<Pessoa>> GetAllPessoas();
        Task<Pessoa> GetPessoa(int id);
        Task AddPessoa(string nome, int idade, string email);
        Task UpdatePessoa(int id, Pessoa pessoa);
        Task DeletePessoa(int id);
        Task UploadCSV(IFormFile file);
    }
}