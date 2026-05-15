using Dominio.Models;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;

public class AgendamentoService : IAgendamentoService
{
    private readonly IAgendamentoRepository _repositorio;
    private readonly IClienteRepository _clienteRepositorio;
    private readonly IProfissionalRepository _profissionalRepositorio;
    private readonly IServicoRepository _servicoRepositorio;
    private readonly ILojaRepository _lojaRepositorio;

    public AgendamentoService(IAgendamentoRepository repositorio, IClienteRepository clienteRepositorio, IProfissionalRepository profissionalRepositorio, IServicoRepository servicoRepositorio, ILojaRepository lojaRepositorio)
    {
        _repositorio = repositorio;
        _clienteRepositorio = clienteRepositorio;
        _profissionalRepositorio = profissionalRepositorio;
        _servicoRepositorio = servicoRepositorio;
        _lojaRepositorio = lojaRepositorio;
    }

    public List<Agendamento> Listar()
    {
        return _repositorio.Listar();
    }

    public Agendamento BuscarPorId(int id)
    {
        return _repositorio.BuscarPorId(id);
    }

    public bool Cadastrar(Agendamento agendamento)
    {
        agendamento.Status = "PENDENTE";

        if (agendamento.DataAgendamento < DateTime.Now)
            throw new Exception("Não é possível agendar no passado.");

        if (agendamento.DataAgendamento.Minute % 5 != 0)
        {
            throw new Exception(
                "Os horários devem seguir intervalos de 5 minutos.");
        }

        var cliente = _clienteRepositorio.BuscarPorId(agendamento.ClienteId);

        if (cliente == null)
            throw new Exception("Cliente não encontrado.");

        var profissional = _profissionalRepositorio.BuscarPorId(agendamento.ProfissionalId);

        if (profissional == null)
            throw new Exception("Profissional não encontrado.");

        var servico = _servicoRepositorio.BuscarPorId(agendamento.ServicoLojaId);

        if (servico == null)
            throw new Exception("Serviço não encontrado.");

        var inicioNovo = agendamento.DataAgendamento;
        var fimNovo = inicioNovo.AddMinutes(servico.TempoMinutos);

        var loja = _lojaRepositorio.BuscarPorId(profissional.LojaId);

        if (loja == null)
            throw new Exception("Loja não encontrada.");

        if (inicioNovo.TimeOfDay < loja.HoraAbertura)
        {
            throw new Exception(
                "Loja ainda está fechada.");
        }

        if (fimNovo.TimeOfDay > loja.HoraFechamento)
        {
            throw new Exception(
                "O serviço termina após o fechamento.");
        }

        var agendamentos = _repositorio.Listar();

        foreach (var existente in agendamentos)
        {
            if (existente.ProfissionalId != agendamento.ProfissionalId)
                continue;

            var servicoExistente = _servicoRepositorio.BuscarPorId(existente.ServicoLojaId);

            if (servicoExistente == null)
                continue;

            var inicioExistente = existente.DataAgendamento;
            var fimExistente = inicioExistente.AddMinutes(servicoExistente.TempoMinutos);

            bool conflito = inicioNovo < fimExistente &&
                            fimNovo > inicioExistente;

            if (conflito)
                throw new Exception("Horário indisponível.");
        }

        return _repositorio.Cadastrar(agendamento);
    }

    public void Atualizar(Agendamento agendamentoAtualizado)
    {
        _repositorio.Atualizar(agendamentoAtualizado);
    }

    public void Remover(int id)
    {
        var agendamento = _repositorio.BuscarPorId(id);

        if (agendamento == null)
            throw new Exception("Agendamento não encontrado.");

        if (agendamento.DataAgendamento <= DateTime.Now)
        {
            throw new Exception(
                "Não é possível cancelar um agendamento já iniciado.");
        }

        _repositorio.Remover(id);
    }
}
