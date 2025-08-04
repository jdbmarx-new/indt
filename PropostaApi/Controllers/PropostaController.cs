using Microsoft.AspNetCore.Mvc;
using PropostaApi.Application.Interfaces;
using PropostaApi.Domain.Entities;
using PropostaApi.Dto;

namespace PropostaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostaController : ControllerBase
{
    private readonly IPropostaService _service;

    public PropostaController(IPropostaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CadastroPropostaDto proposta)
    {
        if (proposta is null)
        {
            return BadRequest("Proposta não pode ser nula.");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var criada = await _service.CriarAsync(proposta).ConfigureAwait(false);
        return CreatedAtAction(nameof(Criar), new { id = criada.Id }, criada);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var propostas = await _service.ListarAsync().ConfigureAwait(false);
        return Ok(propostas);
    }

    [HttpPatch("{id}/status/{status}")]
    public async Task<IActionResult> AlterarStatusAsync(int id, PropostaStatus status)
    {
        var atualizada = await _service.AlterarStatusAsync(id, status).ConfigureAwait(false);
        if (atualizada is null)
        {
            return NotFound();
        }

        return Ok(atualizada);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorIdAsync(int id)
    {
        var atualizada = await _service.ObterPorIdAsync(id).ConfigureAwait(false);
        if (atualizada is null)
        {
            return NotFound();
        }

        return Ok(atualizada);
    }
}