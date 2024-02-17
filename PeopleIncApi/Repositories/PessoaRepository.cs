using PeopleIncApi.Exceptions;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;
using X.PagedList;

namespace PeopleIncApi.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly IPessoaService _pessoaService;

        public PessoaRepository(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }
        public async Task AddPessoa(string nome, int idade, string email)
        {
            try
            {
                await _pessoaService.AddPessoa(nome, idade, email);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro ao adicionar Pessoa", ex);
            }
        }

        public async Task DeletePessoa(int id)
        {
            try
            {
                await _pessoaService.DeletePessoa(id);
            }
            catch (NotFoundException)
            {
                throw new NotFoundException("Pessoa não encontrado");
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro ao excluir pessoa", ex);
            }
        }

        public async Task<IEnumerable<Pessoa>> GetAllPessoas()
        {
            return await _pessoaService.GetAllPessoas();
        }

        public async Task<Pessoa> GetPessoa(int id)
        {
            return await _pessoaService.GetPessoa(id);
        }

        public async Task<IPagedList<Pessoa>> GetPessoasPaginadas(int pageNumber, int pageSize)
        {
            return await _pessoaService.GetPessoasPaginadas(pageNumber, pageSize);
        }

        public async Task UpdatePessoa(int id, Pessoa pessoa)
        {
            try
            {
                await _pessoaService.UpdatePessoa(id, pessoa);
            }
            catch (NotFoundException)
            {
                throw new NotFoundException("Pessoa não encontrado");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("ID fornecido não corresponde a nenhum registro no Banco de Dados", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro ao atualizar pessoa", ex);
            }
        }

        public async Task UploadCSV(IFormFile file)
        {
            try
            {
                await _pessoaService.UploadCSV(file);
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (InvalidHeaderException) 
            {
                throw;
            }
            catch (FormatException) 
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro ao fazer upload", ex);
            }
        }
    }
}