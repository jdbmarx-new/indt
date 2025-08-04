using Microsoft.AspNetCore.Mvc;
using ContratacaoApi.Application.Interfaces;

namespace ContratacaoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratacaoController : ControllerBase
    {
        private readonly IContratacaoService _contratacaoService;
        private ILogger<ContratacaoController> _logger;

        public ContratacaoController(IContratacaoService contratacaoService, ILogger<ContratacaoController> logger)
        {
            _contratacaoService = contratacaoService;
            _logger = logger;
        }

        [HttpPost("proposta/{propostaId}")]
        public async Task<IActionResult> Contratar(int propostaId)
        {
            try
            {
                var contratacao = await _contratacaoService.ContratarPropostaAsync(propostaId).ConfigureAwait(false);
                return CreatedAtAction(nameof(Contratar), new { id = contratacao.Id }, contratacao);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao contratar proposta com ID {PropostaId}", propostaId);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorIdAsync(int id)
        {
            var contratacao = await _contratacaoService.ObterPorIdAsync(id).ConfigureAwait(false);
            if (contratacao == null)
            {
                return NotFound();
            }

            return Ok(contratacao);
        }

        [HttpGet]
        public async Task<IActionResult> ListarAsync()
        {
            var contratacao = await _contratacaoService.ListarAsync().ConfigureAwait(false);

            return Ok(contratacao);
        }
    }
}