using PeopleIncApi.Exceptions;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;


namespace PeopleIncApi.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }
        public async Task AddPessoa(string nome, int idade, string email)
        {
            try
            {
                await _pessoaRepository.AddPessoa(nome, idade,  email);
            }
            catch(Exception ex)
            {
                throw new ServiceException ("Erro ao adicionar Pessoa", ex);
            }
        }

        public async Task DeletePessoa(int id)
        {
            try
            {
                await _pessoaRepository.DeletePessoa(id);
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
            return await _pessoaRepository.GetAllPessoas();
        }

        public async Task<Pessoa> GetPessoa(int id)
        {
            return await _pessoaRepository.GetPessoa(id);
        }

        public async Task UpdatePessoa(int id, Pessoa pessoa)
        {
           try{
                await _pessoaRepository.UpdatePessoa(id, pessoa);
           }
           catch (NotFoundException)
            {
                throw new NotFoundException("Produto não encontrado");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("ID fornecido não corresponde a nenhum registro no Banco de Dados", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro ao atualizar produto", ex);
            }
        }

        public async Task UploadCSV(IFormFile file)
        {
            try
            {
                await _pessoaRepository.UploadCSV(file);
            }
            catch(Exception ex)
            {
                throw new ServiceException("Erro ao fazer upload", ex);
            }
        }
    }
}