using Barbearia.Service.Validators;
using FluentValidation.TestHelper;
using SolucaoBarbearia.servico.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SolucaoBarbeariaTests.Validators;

public class ClienteCadastroDtoValidatorTests
{
    private readonly ClienteCadastroDtoValidator _validator = new();

    [Fact]
    public void Validator_ComDadosValidos_NaoDeveGerarErros()
    {
        var dto = new ClienteCadastroDto("João Silva", "11999990001", "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Nome_Vazio_DeveGerarErro()
    {
        var dto = new ClienteCadastroDto("", "11999990001", "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome é obrigatório!");
    }

    [Fact]
    public void Nome_ComUmCaractere_DeveGerarErroDeTamanhoMinimo()
    {
        var dto = new ClienteCadastroDto("J", "11999990001", "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no mínimo 2 caracteres!");
    }

    [Fact]
    public void Nome_Com101Caracteres_DeveGerarErroDeTamanhoMaximo()
    {
        var nomeGrande = new string('A', 101);
        var dto = new ClienteCadastroDto(nomeGrande, "11999990001", "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no máximo 100 caracteres!");
    }

    [Theory]
    [InlineData("Jo")]
    [InlineData("João Silva")]
    [InlineData("Ana")]
    public void Nome_Valido_NaoDeveGerarErro(string nome)
    {
        var dto = new ClienteCadastroDto(nome, "11999990001", "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Telefone_Vazio_DeveGerarErro()
    {
        var dto = new ClienteCadastroDto("João Silva", "", "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Telefone)
                 .WithErrorMessage("Telefone é obrigatório!");
    }

    [Theory]
    [InlineData("1199999")]
    [InlineData("119999900011")]
    [InlineData("(11)99999-0001")]
    [InlineData("11 99999 0001")]
    [InlineData("abc")]
    public void Telefone_FormatoInvalido_DeveGerarErro(string telefone)
    {
        var dto = new ClienteCadastroDto("João Silva", telefone, "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Telefone)
                 .WithErrorMessage("Telefone deve conter apenas dígitos (10 para fixo, 11 para celular).");
    }

    [Theory]
    [InlineData("1133334444")]
    [InlineData("11999990001")]
    public void Telefone_Valido_NaoDeveGerarErro(string telefone)
    {
        var dto = new ClienteCadastroDto("João Silva", telefone, "joao@email.com");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Telefone);
    }

    [Fact]
    public void Email_Vazio_DeveGerarErro()
    {
        var dto = new ClienteCadastroDto("João Silva", "11999990001", "");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Email)
                 .WithErrorMessage("E-mail é obrigatório!");
    }

    [Theory]
    [InlineData("nao-e-email")]
    [InlineData("@semdominio.com")]
    [InlineData("sem arroba")]
    [InlineData("duplo@@email.com")]
    public void Email_FormatoInvalido_DeveGerarErro(string email)
    {
        var dto = new ClienteCadastroDto("João Silva", "11999990001", email);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Email)
                 .WithErrorMessage("E-mail inválido!");
    }

    [Fact]
    public void Email_Com151Caracteres_DeveGerarErroDeMaximo()
    {
        var emailGrande = new string('a', 141) + "@email.com";
        var dto = new ClienteCadastroDto("João Silva", "11999990001", emailGrande);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Email)
                 .WithErrorMessage("E-mail deve ter no máximo 150 caracteres!");
    }

    [Theory]
    [InlineData("joao@email.com")]
    [InlineData("cliente.novo@barbearia.com.br")]
    [InlineData("a@b.co")]
    public void Email_Valido_NaoDeveGerarErro(string email)
    {
        var dto = new ClienteCadastroDto("João Silva", "11999990001", email);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validator_ComTodosCamposInvalidos_DeveRetornarErrosEmTodosOsCampos()
    {
        var dto = new ClienteCadastroDto("", "abc", "nao-e-email");
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        resultado.ShouldHaveValidationErrorFor(x => x.Telefone);
        resultado.ShouldHaveValidationErrorFor(x => x.Email);
    }
}