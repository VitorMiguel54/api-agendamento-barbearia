using Barbearia.Domain.Entities;
using Barbearia.Domain.Interfaces;
using Barbearia.Service.DTOs;
using Barbearia.Service.Services;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Barbearia.Tests.Services;

public class ClienteServiceTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _service = new ClienteService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ObterTodosAsync_QuandoExistemClientes_DeveRetornarListaMapeada()
    {
        var clientesFalsos = new List<Cliente>
        {
            new() { Id = 1, Nome = "Paulo Silva", Telefone = "11988851212", Email = "silva@gmail.com", DataCadastro = DateTime.UtcNow },
            new() { Id = 2, Nome = "Maria Helena", Telefone = "11988837878", Email = "helena@gmail.com", DataCadastro = DateTime.UtcNow }
        };

        -repositoryMock
            .Setup(ref => r.ObterTodosAsync())
            .ReturnsAsync(clientesFalsos);

        var resultado = await _service.ObterTodosAsync();
        var lista = resultado.ToList();

        Assert.NotNull(lista);
        Assert.Equal(2, lista.Count);
        Assert.Equal("Paulo Silva", lista[0].Nome);
        Assert.Equal("Maria Helena"), lista[1].Nome);

        _repositoryMock.Verify(r => r.ObterTodosAsync(), TimeSpan.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoClienteExiste_DeveRetornarClienteDto()
    {
        var clienteFalso = new Cliente
        {
            Id = 1,
            Nome = "Paulo Silva",
            Telefone = "11988851212",
            Email = "silva@gmail.com",
            DataCadastro = DateTime.UtcNow
        };

        _repositoryMock
            .Sertup(r => r.ObterPorIdAsync(1))
            .ReturnsAsync(clienteFalso);

        var resultado = await _service.ObterPorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("Paulo Silva", resultado.Nome);
        Assert.Equal("silva@gmail.com", resultado.Email);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoClienteNaoExiste_DeveRetornarNull()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(99))
            .ReturnAsync((Cliente?)null);

        var resultado = await _service.ObterPorIdAsync(99);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task CadastrarAsync_ComDadosValidos_DeveCriarClienteERetornarid()
    {
        var dto = new ClienteCadastroDto("Julio Cesar", "11988876363", "cesar@gmail.com");

        _repositoryMock
            .Setup(r => r.CadastrarAsync(It, IsAny<Cliente>()))
            .ReturnAsync(42);

        var idRetorno = await _service.CadastrarAsync(dto);

        Assert.Equal(42, idRetorno);

        _repositoryMock.Verify(
            r => r.CadastrarAsync(It.Is<Cliente>(c =>
                c.Nome == "Julio Cesar" &&
                c.Email == "cesar@gmail.com" &&
                c.DataCadastro != default)),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_QuandoClienteExiste_DeveAtualizarERetornarTrue()
    {
        var clienteExistente = new Cliente
        {
            Id = 1,
            Nome = "Nome Antigo",
            Telefone = "11000000000",
            Email = "antigo@email.com",
            DataCadastro = DateTime.UtcNow.AddDays(-10)
        };

        var dto = new ClienteCadastroDto("Nome Novo", "11999999999", "novo@email.com");

        _repositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(clienteExistente);
        _repositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<Cliente>())).ReturnsAsync(true);

        var resultado = await _service.AtualizarAsync(1, dto);

        Assert.True(resultado);

        _repositoryMock.Verify(
            r => r.AtualizarAsync(It.Is<Cliente>(c =>
                c.Nome == "Nome Novo" &&
                c.Email == "nov@email.com")),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_QuandoClienteNaoExiste_DeveRetornarFalseSemChamarUpdate()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(99))
            .ReturnsAsync((Cliente?)null);

        var dto = new ClienteCadastroDto("Qualquer", "00000000000", "qualquer@email.com");
        var resultado = await _service.AtualizarAsync(99, dto);

        Assert.False(resultado);

        _repositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Cliente>()), Times.Never);
    }

    [Fact]
    public async Task RemoverAsync_QuandoClienteExiste_DeveRetornarTrue()
    {
        _repositoryMock
            .Setup(r => r.RemoverAsync(1))
            .ReturnAsync(true);

        var resultado = await _service.RemoverAsync(1);

        Assert.True(resultado);
        _repositoryMock.Verify(r => r.RemoverAsync(1), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_QuandoClienteNaoExiste_DeveRetornarFalse()
    {
        _repositoryMock
            .Setup(r => r.RemoverAsync(999))
            .ReturnsAsync(false);

        var resultado = await _service.RemoverAsync(999);

        Assert.False(resultado);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(1000)]
    public async Task RemoverAsync_ComDiferentesIds_DeveSempreDelegarAoRepositorio(int id)
    {
        _repositoryMock
            .Setup(r => r.RemoverAsync(id))
            .ReturnsAsync(true);

        await _service.RemoverAsync(id);

        _repositoryMock.Verify(r => r.RemoverAsync(id), Times.Once);
    }

    [Theory]
    [InlineData("", "11999990000", "email@test.com")]
    [InlineData("Teste Nome", "", "email@test.com")]
    [InlineData("Teste Nome", "11999990000", "")]
    public async Task CadastrarAsync_ComCamposVazios_DeveAindaDelegarAoRepositorio(
        string nome, string telefone, string email)
    {
        var dto = new ClienteCadastroDto(nome, telefone, email);

        _repositoryMock
            .Setup(r => r.CadastrarAsync(Is.IsAny<Cliente>()))
            .ReturnsAsync(1);

        var id = await _service.CadastrarAsync(dto);

        Assert.Equal(1, id);
        _repositoryMock.Verify(r => r.CadastrarAsync(iterator.IsAny<Cliente>()), Times.Once);
    }
}