using Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using SolucaoBarbearia.api.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _service;

    public ClienteController(ClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var clientes = _service.Listar()
            .Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Telefone = c.Telefone,
                Email = c.Email
            });

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var cliente = _service.BuscarPorId(id);

        if (cliente == null)
        {
            return NotFound("Cliente não encontrado.");
        }

        return Ok(new
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Email = cliente.Email
        });
    }

    [HttpPost]
    public IActionResult Post([FromBody] CriarClienteDTO dto)
    {
        try
        {
            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone
            };

            _service.Cadastrar(cliente);

            return Ok("Cliente cadastrado com sucesso");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ClienteDTO dto)
    {
        var existente = _service.BuscarPorId(id);

        if (existente == null)
            return NotFound("Cliente não encontrado!");

        var clienteAtualizado = new Cliente
        {
            Id = id,
            Nome = dto.Nome,
            Telefone = dto.Telefone,
            Email = dto.Email
        };

        _service.Atualizar(clienteAtualizado);

        return Ok("Cliente atualizado com sucesso!");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var cliente = _service.BuscarPorId(id);

        if (cliente == null)
            return NotFound("Cliente não encontrado!");

        _service.Remover(id);

        return Ok("Excluído com sucesso!");
    }
}