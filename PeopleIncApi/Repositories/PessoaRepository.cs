using PeopleIncApi.Data;
using PeopleIncApi.Exceptions;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleIncApi.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly Context _context;
        public PessoaRepository(Context context)
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

        public async Task<Pessoa> GetPessoa(int id)
        {
            return await _context.Pessoas.FindAsync(id);
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

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var headerLine = await reader.ReadLineAsync();
                    var expectedHeader = "Nome,Idade,Email";

                    if (headerLine != expectedHeader)
                    {
                        throw new InvalidHeaderException("O cabeçalho do arquivo CSV é inválido.");
                    }

                    List<PessoaCSVModel> pessoas = new List<PessoaCSVModel>();

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(';');

                        pessoas.Add(new PessoaCSVModel
                        {
                            Nome = values[0],
                            Idade = int.Parse(values[1]),
                            Email = values[2]
                        });
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

            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }
        }

        private bool PessoaExists(int id)
        {
            return _context.Pessoas.Any(e => e.Id == id);
        }
    }
}