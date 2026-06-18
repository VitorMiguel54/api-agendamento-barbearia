using Dominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;
using SolucaoBarbearia.servico.Interfaces;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProfissionalController : ControllerBase
{
    private readonly IProfissionalService _service;

    public ProfissionalController(IProfissionalService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok(_service.Listar()
            .Select(p => new
            {
                Id = p.Id,
                LojaId = p.LojaId,
                Nome = p.Nome
            }));
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var profissional = _service.BuscarPorId(id);

        if (profissional == null)
        {
            return NotFound("Profissional não encontrado.");
        }

        return Ok(new
        {
            Id = profissional.Id,
            LojaId = profissional.LojaId,
            Nome = profissional.Nome
        });
    }

    [HttpPost("")]
    public IActionResult Post([FromBody] CriarProfissionalDTO dto)
    {
        var profissional = new Profissional
        {
            LojaId = dto.LojaId,
            Nome = dto.Nome
        };

        _service.Cadastrar(profissional);
        return CreatedAtAction(nameof(GetById), new { id = profissional.Id }, "Profissional cadastrado com sucesso");
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ProfissionalDTO dto)
    {
        var existente = _service.BuscarPorId(id);

        if (existente == null)
            return NotFound("Profissional não encontrado!");

        var profissionalAtualizado = new Profissional
        {
            Id = id,
            LojaId = dto.LojaId,
            Nome = dto.Nome
        };

        _service.Atualizar(profissionalAtualizado);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var profissional = _service.BuscarPorId(id);

        if (profissional == null)
            return NotFound("Profissional não encontrado!");

        _service.Remover(id);

        return NoContent();
    }
}
