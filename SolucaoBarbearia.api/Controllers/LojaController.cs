using Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;

[ApiController]
[Route("[controller]")]

public class LojaController : ControllerBase
{
    private readonly LojaService _service;

    public LojaController(LojaService service)
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
        return Ok("Loja cadastrada com sucesso!");
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

        return Ok("Atualizado com sucesso!");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var loja = _service.BuscarPorId(id);

        if (loja == null)
            return NotFound("Loja não encontrada!");

        _service.Remover(id);

        return Ok("Excluído com sucesso!");
    }
}