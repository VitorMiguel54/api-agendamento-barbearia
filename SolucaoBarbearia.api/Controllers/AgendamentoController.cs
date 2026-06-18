using Dominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;
using SolucaoBarbearia.servico.Interfaces;

namespace SolucaoBarbearia.api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AgendamentoController : ControllerBase
{
    private readonly IAgendamentoService _service;

    public AgendamentoController(IAgendamentoService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Post([FromBody] CriarAgendamentoDTO dto)
    {
        try
        {
            var agendamento = new Agendamento
            {
                ClienteId = dto.ClienteId,
                ProfissionalId = dto.ProfissionalId,
                ServicoLojaId = dto.ServicoLojaId,
                DataAgendamento = dto.DataAgendamento
            };

            _service.Cadastrar(agendamento);
            return CreatedAtAction(nameof(GetById), new { id = agendamento.Id }, "Agendamento criado com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        var lista = _service.Listar();
        return Ok(lista);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var agendamento = _service.BuscarPorId(id);

        if (agendamento == null)
            return NotFound();

        return Ok(agendamento);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] CriarAgendamentoDTO dto)
    {
        try
        {
            var existente = _service.BuscarPorId(id);

            if (existente == null)
                return NotFound();

            var agendamentoAtualizado = new Agendamento
            {
                Id = id,
                ClienteId = dto.ClienteId,
                ProfissionalId = dto.ProfissionalId,
                ServicoLojaId = dto.ServicoLojaId,
                DataAgendamento = dto.DataAgendamento
            };

            _service.Atualizar(agendamentoAtualizado);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var agendamento = _service.BuscarPorId(id);

        if (agendamento == null)
            return NotFound("Agendamento não encontrado!");

        _service.Remover(id);

        return NoContent();
    }
}
