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

public class LojaCadastroDtoValidatorTests
{
    private readonly LojaCadastroDtoValidator _validator = new();

    [Fact]
    public void Validator_ComDadosValidos_NaoDeveGerarErros()
    {
        var dto = new LojaCadastroDto("Zé Barbeiro", new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Nome_Vazio_DeveGerarErro()
    {
        var dto = new LojaCadastroDto("", new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome é obrigatório!");
    }

    [Fact]
    public void Nome_ComUmCaractere_DeveGerarErroDeTamanhoMinimo()
    {
        var dto = new LojaCadastroDto("Z", new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no mínimo 2 caracteres!");
    }

    [Fact]
    public void Nome_Com101Caracteres_DeveGerarErroDeTamanhoMaximo()
    {
        var nomeGrande = new string('A', 101);
        var dto = new LojaCadastroDto(nomeGrande, new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                 .WithErrorMessage("Nome deve ter no máximo 100 caracteres!");
    }

    [Theory]
    [InlineData("Ze")]
    [InlineData("Zé Barbearia")]
    [InlineData("Zeto")]
    public void Nome_Valido_NaoDeveGerarErro(string nome)
    {
        var dto = new LojaCadastroDto(nome, new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void HoraAbertura_Vazio_DeveGerarErro()
    {
        var dto = new LojaCadastroDto("Zé Barbearia", TimeSpan.Zero, new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.HoraAbertura)
                 .WithErrorMessage("Hora abertura é obrigatório!");
    }

    [Theory]
    [InlineData("10:00:00")]
    [InlineData("08:00:00")]
    [InlineData("15:00:00")]
    public void HoraAbertura_Valido_NaoDeveGerarErro(string hora)
    {
        var horaAbertura = TimeSpan.Parse(hora);
        var dto = new LojaCadastroDto("Zé Barbearia", horaAbertura, new TimeSpan(18, 0, 0));
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.HoraAbertura);
    }
    
    [Fact]
    public void HoraFechamento_Vazio_DeveGerarErro()
    {
        var dto = new LojaCadastroDto("Zé Barbearia", new TimeSpan(10, 0, 0),  TimeSpan.Zero);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.HoraFechamento)
                 .WithErrorMessage("Hora Fechamento é obrigatório!");
    }

    [Theory]
    [InlineData("19:00:00")]
    [InlineData("15:00:00")]
    [InlineData("20:00:00")]
    public void HoraFechamento_Valido_NaoDeveGerarErro(string hora)
    {
        var horaFechamento = TimeSpan.Parse(hora);
        var dto = new LojaCadastroDto("Zé Barbearia", new TimeSpan(10, 0, 0), horaFechamento);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveValidationErrorFor(x => x.HoraFechamento);
    }

    [Theory]
    [InlineData("10:00:00", "09:00:00")]
    [InlineData("10:00:00", "09:30:00")]
    [InlineData("10:00:00", "06:00:00")]
    public void HoraFechamentoMaiorQueHoraAbertura_GerarErro(string horaAb, string horaFe)
    {
        var horaAbertura = TimeSpan.Parse(horaAb);
        var horaFechamento = TimeSpan.Parse(horaFe);
        var dto = new LojaCadastroDto("Zé Barbearia", horaAbertura, horaFechamento);
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.HoraFechamento)
                 .WithErrorMessage("Horário de Fechamento precisa ser MAIOR que horário de abertura!");
    }

    [Fact]
    public void Validator_ComTodosCamposInvalidos_DeveRetornarErrosEmTodosOsCampos()
    {
        var dto = new LojaCadastroDto("Z", new TimeSpan(), new TimeSpan());
        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        resultado.ShouldHaveValidationErrorFor(x => x.HoraAbertura);
        resultado.ShouldHaveValidationErrorFor(x => x.HoraFechamento);
    }
}