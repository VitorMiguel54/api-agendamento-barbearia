using Dominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;
using SolucaoBarbearia.servico.Interfaces;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class LojaController : ControllerBase
{
    private readonly ILojaService _service;

    public LojaController(ILojaService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok(_service.Listar()
            .Select(l => new
            {
                Id = l.Id,
                Nome = l.Nome,
                HoraAbertura = l.HoraAbertura,
                HoraFechamento = l.HoraFechamento
            }));
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var loja = _service.BuscarPorId(id);

        if (loja == null)
        {
            return NotFound("Loja não encontrada.");
        }

        return Ok(new
        {
            Id = loja.Id,
            Nome = loja.Nome,
            HoraAbertura = loja.HoraAbertura,
            HoraFechamento = loja.HoraFechamento
        });
    }

    [HttpPost("")]
    public IActionResult Post([FromBody] CriarLojaDTO dto)
    {
        var loja = new Loja
        {
            Nome = dto.Nome,
            HoraAbertura = dto.HoraAbertura,
            HoraFechamento = dto.HoraFechamento
        };

        _service.Cadastrar(loja);
        return CreatedAtAction(nameof(GetById), new { id = loja.Id }, "Loja cadastrada com sucesso!");
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] LojaDTO dto)
    {
        var existente = _service.BuscarPorId(id);

        if (existente == null)
            return NotFound("Loja não encontrada!");

        var lojaAtualizado = new Loja
        {
            Id = id,
            Nome = dto.Nome,
            HoraAbertura = dto.HoraAbertura,
            HoraFechamento = dto.HoraFechamento
        };

        _service.Atualizar(lojaAtualizado);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var loja = _service.BuscarPorId(id);

        if (loja == null)
            return NotFound("Loja não encontrada!");

        _service.Remover(id);

        return NoContent();
    }
}
