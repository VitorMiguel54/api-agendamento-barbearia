using Dominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;
using SolucaoBarbearia.servico.Interfaces;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ServicoController : ControllerBase
{
    private readonly IServicoService _service;

    public ServicoController(IServicoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var servicos = _service.ListarServicosLoja();

        return Ok(servicos);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var servico = _service.BuscarPorId(id);

        if (servico == null)
        {
            return NotFound("Serviço não encontrado.");
        }

        return Ok(new
        {
            Id = servico.Id,
            LojaId = servico.LojaId,
            Nome = servico.Nome,
            Descricao = servico.Descricao,
            TempoMinutos = $"{servico.TempoMinutos} min",
            Preco = $"R$ {servico.Preco:N2}"
        });
    }

    [HttpPost("")]
    public IActionResult Post([FromBody] ServicoDTO dto)
    {
        var servico = new Servico
        {
            LojaId = dto.LojaId,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            TempoMinutos = dto.TempoMinutos,
            Preco = dto.Preco
        };

        _service.Cadastrar(servico);
        return CreatedAtAction(nameof(GetById), new { id = servico.Id }, "Serviço cadastrado com sucesso");
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ServicoDTO dto)
    {
        var existente = _service.BuscarPorId(id);

        if (existente == null)
            return NotFound("Serviço não encontrado!");

        var servicoAtualizado = new Servico
        {
            Id = id,
            LojaId = dto.LojaId,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            TempoMinutos = dto.TempoMinutos,
            Preco = dto.Preco
        };

        _service.Atualizar(servicoAtualizado);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var servico = _service.BuscarPorId(id);

        if (servico == null)
            return NotFound("Serviço não encontrado!");

        _service.Remover(id);

        return NoContent();
    }
}
