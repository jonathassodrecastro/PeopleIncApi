using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleIncApi.Exceptions;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Models;
using PeopleIncApi.Requests;

namespace PeopleIncApi.Controllers
{
    /// <summary>
    /// Controller responsável por operações relacionadas a pessoas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaController(IPessoaRepository pessoaService)
        {
            _pessoaRepository = pessoaService;
        }

        /// <summary>
        /// Obtém todas as pessoas.
        /// </summary>
        /// <param name="pageNumber">Número de páginas.</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <returns>Uma lista de pessoas.</returns>
        [Authorize]
        [HttpGet("GetPessoas")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas(int pageNumber, int pageSize)
        {
            return Ok(await _pessoaRepository.GetPessoasPaginadas(pageNumber, pageSize));
        }

        /// <summary>
        /// Obtém uma pessoa pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da pessoa.</param>
        /// <returns>A pessoa encontrada.</returns>
        [Authorize]
        [HttpGet("GetPessoa/{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _pessoaRepository.GetPessoa(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(pessoa);
        }

        /// <summary>
        /// Adiciona uma nova pessoa.
        /// </summary>
        /// <param name="pessoaRequest">Os dados da pessoa a ser adicionada.</param>
        /// <returns>A pessoa adicionada.</returns>
        [Authorize]
        [HttpPost("AddPessoa")]
        public async Task<ActionResult<Pessoa>> AddPessoa([FromBody] PessoaRequest pessoaRequest)
        {         

            var pessoaCriada = await _pessoaRepository.AddPessoa(pessoaRequest.Nome, pessoaRequest.Idade, pessoaRequest.Email);

            return Ok(pessoaCriada);
        }

        /// <summary>
        /// Atualiza uma pessoa existente.
        /// </summary>
        /// <param name="id">O ID da pessoa a ser atualizada.</param>
        /// <param name="pessoa">Os novos dados da pessoa.</param>
        /// <returns>Um status HTTP indicando o resultado da operação.</returns>
        [HttpPut("UpdatePessoa/{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pessoaRepository.UpdatePessoa(id, pessoa);
                return Ok(pessoa);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove uma pessoa pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da pessoa a ser removida.</param>
        /// <returns>Um status HTTP indicando o resultado da operação.</returns>
        [Authorize]
        [HttpDelete("DeletePessoa/{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _pessoaRepository.GetPessoa(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            await _pessoaRepository.DeletePessoa(id);

            return Ok("Pessoa deletada com sucesso.");
        }

        /// <summary>
        /// Faz upload de um arquivo CSV contendo informações de pessoas.
        /// </summary>
        /// <param name="file">O arquivo CSV a ser processado.</param>
        /// <returns>Um status HTTP indicando o resultado da operação.</returns>
        [Authorize]
        [HttpPost("UploadCSV")]
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            try
            {
                await _pessoaRepository.UploadCSV(file);
                return Ok("Pessoas adicionadas com sucesso.");
            }
            catch (InvalidDataException ex)
            {               
                return BadRequest(ex.Message); 
            }
            catch (InvalidHeaderException ex)
            {             
                return BadRequest(ex.Message); 
            }
            catch (ServiceException ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); 
            }
        }
    }
}
