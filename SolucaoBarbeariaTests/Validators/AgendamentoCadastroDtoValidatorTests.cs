using Barbearia.Service.Validators;
using Dominio.Models;
using FluentValidation.TestHelper;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.DTOs;
using Xunit;

namespace SolucaoBarbeariaTests.Validators;

public class AgendamentoCadastroDtoValidatorTests
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IServicoRepository> _servicoRepositoryMock;
    private readonly Mock<IProfissionalRepository> _profissionalRepositoryMock;
    private readonly AgendamentoCadastroDtoValidator _validator;

    public AgendamentoCadastroDtoValidatorTests()
    {
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _servicoRepositoryMock = new Mock<IServicoRepository>();
        _profissionalRepositoryMock = new Mock<IProfissionalRepository>();

        _clienteRepositoryMock
            .Setup(x => x.BuscarPorId(It.IsAny<int>()))
            .Returns(new Cliente());

        _servicoRepositoryMock
            .Setup(x => x.BuscarPorId(It.IsAny<int>()))
            .Returns(new Servico());

        _profissionalRepositoryMock
            .Setup(x => x.BuscarPorId(It.IsAny<int>()))
            .Returns(new Profissional());

        _validator = new AgendamentoCadastroDtoValidator(
            _clienteRepositoryMock.Object,
            _servicoRepositoryMock.Object,
            _profissionalRepositoryMock.Object);
    }

    [Fact]
    public void Validator_ComDadosValidos_NaoDeveGerarErros()
    {
        var dto = CriarDtoValido();

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ClienteId_Vazio_DeveGerarErro()
    {
        var dto = CriarDtoValido(clienteId: 0);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ClienteId)
                 .WithErrorMessage("ID do cliente obrigatório!");
    }

    [Fact]
    public void ClienteId_Inexistente_DeveGerarErro()
    {
        _clienteRepositoryMock
            .Setup(x => x.BuscarPorId(It.IsAny<int>()))
            .Returns((Cliente)null!);

        var dto = CriarDtoValido(clienteId: 9999);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ClienteId)
                 .WithErrorMessage("ID não localizado!");
    }

    [Fact]
    public void ServicoLojaId_Vazio_DeveGerarErro()
    {
        var dto = CriarDtoValido(servicoLojaId: 0);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ServicoLojaId)
                 .WithErrorMessage("ID do serviço obrigatório!");
    }

    [Fact]
    public void ServicoLojaId_Inexistente_DeveGerarErro()
    {
        _servicoRepositoryMock
            .Setup(x => x.BuscarPorId(It.IsAny<int>()))
            .Returns((Servico)null!);

        var dto = CriarDtoValido(servicoLojaId: 9999);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ServicoLojaId)
                 .WithErrorMessage("ID não localizado!");
    }

    [Fact]
    public void ProfissionalId_Vazio_DeveGerarErro()
    {
        var dto = CriarDtoValido(profissionalId: 0);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ProfissionalId)
                 .WithErrorMessage("ID do profissional obrigatório!");
    }

    [Fact]
    public void ProfissionalId_Inexistente_DeveGerarErro()
    {
        _profissionalRepositoryMock
            .Setup(x => x.BuscarPorId(It.IsAny<int>()))
            .Returns((Profissional)null!);

        var dto = CriarDtoValido(profissionalId: 9999);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ProfissionalId)
                 .WithErrorMessage("ID não localizado!");
    }

    [Fact]
    public void DataAgendamento_Vazia_DeveGerarErro()
    {
        var dto = CriarDtoValido(dataAgendamento: DateTime.MinValue);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.DataAgendamento)
                 .WithErrorMessage("Data de Agendamento é obrigatório!");
    }

    [Fact]
    public void DataAgendamento_NoPassado_DeveGerarErro()
    {
        var dto = CriarDtoValido(dataAgendamento: DateTime.Now.AddDays(-1));

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.DataAgendamento)
                 .WithErrorMessage("Data de Agendamento deve ser futura!");
    }

    [Fact]
    public void DataAgendamento_ForaDoIntervaloDeCincoMinutos_DeveGerarErro()
    {
        var dto = CriarDtoValido(dataAgendamento: ProximaDataValida().AddMinutes(1));

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.DataAgendamento)
                 .WithErrorMessage("Os horários devem seguir intervalos de 5 minutos.");
    }

    [Fact]
    public void Validator_ComTodosCamposInvalidos_DeveRetornarErrosEmTodosOsCampos()
    {
        var dto = new AgendamentoCadastroDto(0, 0, 0, default);

        var resultado = _validator.TestValidate(dto);

        resultado.ShouldHaveValidationErrorFor(x => x.ClienteId);
        resultado.ShouldHaveValidationErrorFor(x => x.ServicoLojaId);
        resultado.ShouldHaveValidationErrorFor(x => x.ProfissionalId);
        resultado.ShouldHaveValidationErrorFor(x => x.DataAgendamento);
    }

    private static AgendamentoCadastroDto CriarDtoValido(
        int clienteId = 1,
        int servicoLojaId = 1,
        int profissionalId = 1,
        DateTime? dataAgendamento = null)
    {
        return new AgendamentoCadastroDto(
            clienteId,
            servicoLojaId,
            profissionalId,
            dataAgendamento ?? ProximaDataValida());
    }

    private static DateTime ProximaDataValida()
    {
        var agora = DateTime.Now.AddHours(1);
        var minutosAteProximoIntervalo = (5 - agora.Minute % 5) % 5;

        return new DateTime(
            agora.Year,
            agora.Month,
            agora.Day,
            agora.Hour,
            agora.Minute,
            0).AddMinutes(minutosAteProximoIntervalo);
    }
}
