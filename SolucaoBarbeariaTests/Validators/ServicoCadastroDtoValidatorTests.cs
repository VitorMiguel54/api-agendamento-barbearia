using Barbearia.Service.Validators;
using FluentValidation.TestHelper;
using SolucaoBarbearia.servico.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;

namespace SolucaoBarbeariaTests.Validators;

public class ServicoCadastroDtoValidatorTests
{
    private readonly ServicoCadastrarDtoValidator _validator;
    private readonly Mock<ILojaRepository> _lojaRepositoryMock;

    public ServicoCadastroDtoValidatorTests()
    {
        _lojaRepositoryMock = new Mock<ILojaRepository>();

        _lojaRepositoryMock
            .Setup(x => x.Existe(It.IsAny<int>()))
            .Returns(true);

        _validator = new ServicoCadastrarDtoValidator(_lojaRepositoryMock.Object);
    }

    [Fact]
    public void Validator_ComDadosValidos_NaoDeveGerarErros()
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void LojaId_Vazio_DeveGerarErro()
    {
        var dto = new ServicoCadastroDto(0, "Transplante Capilar", "Recupere seus cabelos!", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.LojaId)
                 .WithErrorMessage("ID da Loja obrigatório!");
    }

    [Fact]
    public void LojaId_Inexistente_DeveGerarErro()
    {
        _lojaRepositoryMock
            .Setup(x => x.Existe(It.IsAny<int>()))
            .Returns(false);

        var dto = new ServicoCadastroDto(9999, "Transplante Capilar", "Recupere seus cabelos!", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.LojaId)
                 .WithErrorMessage("ID não localizado!");
    }

    [Fact]
    public void Nome_Vazio_DeveGerarErro()
    {
        var dto = new ServicoCadastroDto(1, "", "Recupere seus cabelos!", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome é obrigatório!");
    }

    [Fact]
    public void Nome_ComUmCaractere_DeveGerarErroDeTamanhoMinimo()
    {
        var dto = new ServicoCadastroDto(1, "T", "Recupere seus cabelos!", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no mínimo 2 caracteres!");
    }

    [Fact]
    public void Nome_Com101Caracteres_DeveGerarErroDeTamanhoMaximo()
    {
        var nomeGrande = new string('A', 101);
        var dto = new ServicoCadastroDto(1, nomeGrande, "Recupere seus cabelos!", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no máximo 100 caracteres!");
    }

    [Theory]
    [InlineData("Tr")]
    [InlineData("Transplante")]
    [InlineData("Transp")]
    public void Nome_Valido_NaoDeveGerarErro(string nome)
    {
        var dto = new ServicoCadastroDto(1, nome, "Recupere seus cabelos!", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Theory]
    [InlineData("")]
    public void Descricao_Vazia_NaoDeveGerarErro(string descricao)
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", descricao, 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Descricao);
    }

    [Fact]
    public void Descricao_CaracteresInvalidos_GerarErro()
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "12345", 120, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Descricao)
                 .WithErrorMessage("Descrição só pode conter letras, sem números ou caracteres especiais!");
    }

    [Fact]
    public void TempoMinutos_Vazio_DeveGerarErro()
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "Recupere seus cabelos!", 0, 500);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.TempoMinutos)
                 .WithErrorMessage("Tempo/Duração obrigatório");
    }

    [Fact]
    public void TempoMinutos_ForaDoPadraoPermitido_DeveGerarErro()
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "Recupere seus cabelos!", 10, 500);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.TempoMinutos)
                 .WithErrorMessage("Tempo de Serviço precisa ser MAIOR que 30 minutos e MENOR que 180 minutos!");
    }

    [Theory]
    [InlineData("40")]
    [InlineData("60")]
    [InlineData("95")]
    public void TempoMinutos_Valido_NaoDeveGerarErro(string tempoM)
    {
        var tempoMinutos = int.Parse(tempoM);
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "Recupere seus cabelos!", tempoMinutos, 500);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.TempoMinutos);
    }

    [Fact]
    public void Preco_Vazio_DeveGerarErro()
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "Recupere seus cabelos!", 120, 0);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Preco)
                 .WithErrorMessage("Preço obrigatório!");
    }

    [Fact]
    public void Preco_ForaDoPadraoPermitido_DeveGerarErro()
    {
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "Recupere seus cabelos!", 120, 5);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Preco)
                 .WithErrorMessage("Preço do Serviço precisa ser MAIOR que R$10,00 e MENOR que R$1000,00!");
    }

    [Theory]
    [InlineData("40")]
    [InlineData("60")]
    [InlineData("95")]
    public void Preco_Valido_NaoDeveGerarErro(string valor)
    {
        var preco = int.Parse(valor);
        var dto = new ServicoCadastroDto(1, "Transplante Capilar", "Recupere seus cabelos!", 120, preco);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Preco);
    }

    [Fact]
    public void Validator_ComTodosCamposInvalidos_DeveRetornarErrosEmTodosOsCampos()
    {
        var dto = new ServicoCadastroDto(0, "T", "12345", 5000, 5);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.LojaId);
        resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        resultado.ShouldHaveValidationErrorFor(x => x.Descricao);
        resultado.ShouldHaveValidationErrorFor(x => x.TempoMinutos);
        resultado.ShouldHaveValidationErrorFor(x => x.Preco);
    }
}