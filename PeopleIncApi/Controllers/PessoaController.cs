using Microsoft.AspNetCore.Mvc;
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
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        /// <summary>
        /// Obtém todas as pessoas.
        /// </summary>
        /// <returns>Uma lista de pessoas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            return Ok(await _pessoaService.GetAllPessoas());
        }

        /// <summary>
        /// Obtém uma pessoa pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da pessoa.</param>
        /// <returns>A pessoa encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _pessoaService.GetPessoa(id);

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
        [HttpPost]
        public async Task<ActionResult<Pessoa>> AddPessoa([FromBody] PessoaRequest pessoaRequest)
        {
            await _pessoaService.AddPessoa(pessoaRequest.Nome, pessoaRequest.Idade, pessoaRequest.Email);
            return Ok(pessoaRequest);
        }

        /// <summary>
        /// Atualiza uma pessoa existente.
        /// </summary>
        /// <param name="id">O ID da pessoa a ser atualizada.</param>
        /// <param name="pessoa">Os novos dados da pessoa.</param>
        /// <returns>Um status HTTP indicando o resultado da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pessoaService.UpdatePessoa(id, pessoa);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _pessoaService.GetPessoa(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            await _pessoaService.DeletePessoa(id);

            return NoContent();
        }

        /// <summary>
        /// Faz upload de um arquivo CSV contendo informações de pessoas.
        /// </summary>
        /// <param name="file">O arquivo CSV a ser processado.</param>
        /// <returns>Um status HTTP indicando o resultado da operação.</returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            try
            {
                await _pessoaService.UploadCSV(file);
                return Ok("Pessoas adicionadas com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao processar o arquivo: {ex.Message}");
            }
        }
    }
}
