//using Microsoft.AspNetCore.Mvc;
//using SolucaoBarbearia.api.DTOs;
//using Barbearia.Service.Interfaces;
//using SolucaoBarbearia.servico.DTOs;

//[ApiController]
//[Route("api/[controller]")]
//public class ClienteController : ControllerBase
//{
//    private readonly IClienteService _service;

//    public ClienteController(IClienteService service)
//    {
//        _service = service;
//    }

//    [HttpGet]
//    public async Task<IActionResult> Get()
//    {
//        var clientes = await _service.ObterTodosAsync();

//        var resultado = clientes.Select(c => new ClienteDTO
//        {
//            Id = c.Id,
//            Nome = c.Nome,
//            Telefone = c.Telefone,
//            Email = c.Email
//        });

//        return Ok(resultado);
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        var cliente = await _service.ObterPorIdAsync(id);

//        if (cliente == null)
//        {
//            return NotFound("Cliente não encontrado.");
//        }

//        return Ok(new ClienteDTO
//        {
//            Id = cliente.Id,
//            Nome = cliente.Nome,
//            Telefone = cliente.Telefone,
//            Email = cliente.Email
//        });
//    }

//    [HttpPost]
//    public async Task<IActionResult> Post([FromBody] ClienteCadastroDto dto)
//    {
//        try
//        {
//            await _service.CadastrarAsync(dto);

//            return Ok("Cliente cadastrado com sucesso");
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(ex.Message);
//        }
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Put(int id, [FromBody] ClienteCadastroDto dto)
//    {
//        var existente = await _service.ObterPorIdAsync(id);

//        if (existente == null)
//            return NotFound("Cliente não encontrado!");

//        await _service.AtualizarAsync(id, dto);

//        return Ok("Cliente atualizado com sucesso!");
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var cliente = await _service.ObterPorIdAsync(id);

//        if (cliente == null)
//            return NotFound("Cliente não encontrado!");

//        await _service.RemoverAsync(id);

//        return Ok("Excluído com sucesso!");
//    }
//}