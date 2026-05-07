using Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;

[ApiController]
[Route("[controller]")]

public class ProfissionalController : ControllerBase
{
    private readonly ProfissionalService _service;

    public ProfissionalController(ProfissionalService service)
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
        return Ok("Profissional cadastrado com sucesso");
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

        return Ok("Atualizado com sucesso!");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var profissional = _service.BuscarPorId(id);

        if (profissional == null)
            return NotFound("Profissional não encontrado!");

        _service.Remover(id);

        return Ok("Excluído com sucesso!");
    }
}