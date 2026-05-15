using FluentValidation;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.DTOs;

namespace Barbearia.Service.Validators;

public class AgendamentoCadastroDtoValidator : AbstractValidator<AgendamentoCadastroDto>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IProfissionalRepository _profissionalRepository;

    public AgendamentoCadastroDtoValidator(
        IClienteRepository clienteRepository,
        IServicoRepository servicoRepository,
        IProfissionalRepository profissionalRepository)
    {
        _clienteRepository = clienteRepository;
        _servicoRepository = servicoRepository;
        _profissionalRepository = profissionalRepository;

        RuleFor(x => x.ClienteId)
            .NotEmpty()
                .WithMessage("ID do cliente obrigatório!")
            .Must(ExisteCliente)
                .WithMessage("ID não localizado!");

        RuleFor(x => x.ServicoLojaId)
            .NotEmpty()
                .WithMessage("ID do serviço obrigatório!")
            .Must(ExisteServico)
                .WithMessage("ID não localizado!");

        RuleFor(x => x.ProfissionalId)
            .NotEmpty()
                .WithMessage("ID do profissional obrigatório!")
            .Must(ExisteProfissional)
                .WithMessage("ID não localizado!");

        RuleFor(x => x.DataAgendamento)
            .NotEmpty()
                .WithMessage("Data de Agendamento é obrigatório!")
            .GreaterThan(DateTime.Now)
                .WithMessage("Data de Agendamento deve ser futura!")
            .Must(EstarEmIntervaloDeCincoMinutos)
                .WithMessage("Os horários devem seguir intervalos de 5 minutos.");
    }

    private bool ExisteCliente(int clienteId)
    {
        return _clienteRepository.BuscarPorId(clienteId) is not null;
    }

    private bool ExisteServico(int servicoId)
    {
        return _servicoRepository.BuscarPorId(servicoId) is not null;
    }

    private bool ExisteProfissional(int profissionalId)
    {
        return _profissionalRepository.BuscarPorId(profissionalId) is not null;
    }

    private static bool EstarEmIntervaloDeCincoMinutos(DateTime dataAgendamento)
    {
        return dataAgendamento.Minute % 5 == 0;
    }
}
