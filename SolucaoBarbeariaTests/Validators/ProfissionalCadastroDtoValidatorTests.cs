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

public class ProfissionalCadastroDtoValidatorTests
{
    private readonly ProfissionalCadastroDtoValidator _validator;
    private readonly Mock<ILojaRepository> _lojaRepositoryMock;

    public ProfissionalCadastroDtoValidatorTests()
    {
        _lojaRepositoryMock = new Mock<ILojaRepository>();

        _lojaRepositoryMock
            .Setup(x => x.Existe(It.IsAny<int>()))
            .Returns(true);

        _validator = new ProfissionalCadastroDtoValidator(_lojaRepositoryMock.Object);
    }

    [Fact]
    public void Validator_ComDadosValidos_NaoDeveGerarErros()
    {
        var dto = new ProfissionalCadastroDto(1, "Homer Simpson");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void LojaId_Vazio_DeveGerarErro()
    {
        var dto = new ProfissionalCadastroDto(0, "Homer Simpson");
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

        var dto = new ProfissionalCadastroDto(9999, "Homer Simpson");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.LojaId)
                 .WithErrorMessage("ID não localizado!");
    }

    [Fact]
    public void Nome_Vazio_DeveGerarErro()
    {
        var dto = new ProfissionalCadastroDto(1, "");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome é obrigatório!");
    }

    [Fact]
    public void Nome_ComUmCaractere_DeveGerarErroDeTamanhoMinimo()
    {
        var dto = new ProfissionalCadastroDto(1, "H");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no mínimo 2 caracteres!");
    }

    [Fact]
    public void Nome_Com101Caracteres_DeveGerarErroDeTamanhoMaximo()
    {
        var nomeGrande = new string('A', 101);
        var dto = new ProfissionalCadastroDto(1, nomeGrande);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no máximo 100 caracteres!");
    }

    [Theory]
    [InlineData("Ho")]
    [InlineData("Homer Simpson")]
    [InlineData("Homer")]
    public void Nome_Valido_NaoDeveGerarErro(string nome)
    {
        var dto = new ProfissionalCadastroDto(1, nome);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Validator_ComTodosCamposInvalidos_DeveRetornarErrosEmTodosOsCampos()
    {
        var dto = new ProfissionalCadastroDto(0, "H");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.LojaId);
        resultado.ShouldHaveValidationErrorFor(x => x.Nome);
    }
}