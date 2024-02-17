using Microsoft.EntityFrameworkCore;
using PeopleIncApi.Data;
using PeopleIncApi.Exceptions;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;
using X.PagedList;

namespace PeopleIncApi.Services
{
    public class PessoaService : IPessoaService
    {
        public const string header = "Nome;Idade;Email";
        private readonly Context _context;
        public PessoaService(Context context)
        {
            _context = context;
        }

        public async Task AddPessoa(string nome, int idade, string email)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validar a presença de todos os parâmetros
                if (string.IsNullOrEmpty(nome) || idade <= 0 || string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Todos os parâmetros (nome, idade e email) são obrigatórios.");
                }

                var pessoa = new Pessoa
                {
                    Nome = nome,
                    Idade = idade,
                    Email = email
                };

                await _context.Pessoas.AddAsync(pessoa);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeletePessoa(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var pessoa = await _context.Pessoas.FindAsync(id);

                if (pessoa == null)
                {
                    throw new NotFoundException("Pessoa não encontrado");
                }

                _context.Pessoas.Remove(pessoa);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Pessoa>> GetAllPessoas()
        {
            return await _context.Pessoas.ToListAsync();
        }

        public async Task<Pessoa> GetPessoa(long id)
        {
            return await _context.Pessoas.FindAsync(id);
        }

        public async Task<IPagedList<Pessoa>> GetPessoasPaginadas(int pageNumber, int pageSize)
        {
            var pessoas = await GetAllPessoas();

            var pessoasPaginadas = pessoas
                .OrderBy(p => p.Id)
                .ToPagedList(pageNumber, pageSize);

            return pessoasPaginadas;
        }

        public async Task UpdatePessoa(int id, Pessoa pessoa)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (id != pessoa.Id)
                {
                    throw new ArgumentException("ID fornecido não corresponde a nenhum registro no Banco de Dados");
                }

                _context.Entry(pessoa).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                if (!PessoaExists(id))
                {
                    throw new NotFoundException("Pessoa não encontrada");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ServiceException("Arquivo não fornecido ou vazio.");
            }

            if (file.Length > 1024 * 1024) // 1MB
            {
                throw new ServiceException("O arquivo é muito grande. O tamanho máximo permitido é de 1MB.");
            }

            List<string> linhasInvalidas = new List<string>();

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var headerLine = await reader.ReadLineAsync();
                    var expectedHeader = header;

                    if (headerLine != expectedHeader)
                    {
                        throw new InvalidHeaderException("O cabeçalho do arquivo CSV é inválido.");
                    }

                    List<PessoaCSVModel> pessoas = new List<PessoaCSVModel>();

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(';');

                        if (values.Length != 3) // Verifica se a linha tem o formato esperado
                        {
                            linhasInvalidas.Add(line);
                            continue; // Ignora a linha e passa para a próxima
                        }

                        try
                        {
                            pessoas.Add(new PessoaCSVModel
                            {
                                Nome = values[0],
                                Idade = int.Parse(values[1]),
                                Email = values[2]
                            });
                        }
                        catch (FormatException)
                        {
                            linhasInvalidas.Add(line); // Adiciona a linha inválida à lista
                        }
                    }

                    foreach (var pessoa in pessoas)
                    {
                        await AddPessoa(
                             pessoa.Nome,
                             pessoa.Idade,
                             pessoa.Email
                        );
                    }
                }

                if (linhasInvalidas.Any())
                {
                    throw new InvalidDataException($"Linhas inválidas no arquivo: {string.Join(", ", linhasInvalidas)}.");
                }
            }
            catch
            {
                throw;
            }

        }


        private bool PessoaExists(int id)
        {
            return _context.Pessoas.Any(e => e.Id == id);
        }
    }
}