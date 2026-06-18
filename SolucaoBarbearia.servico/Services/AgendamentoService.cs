using Dominio.Models;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;

public class AgendamentoService : IAgendamentoService
{
    private readonly IAgendamentoRepository _repositorio;
    private readonly IClienteRepository _clienteRepositorio;
    private readonly IProfissionalRepository _profissionalRepositorio;
    private readonly IServicoLojaRepository _servicoLojaRepositorio;
    private readonly ILojaRepository _lojaRepositorio;

    public AgendamentoService(IAgendamentoRepository repositorio, IClienteRepository clienteRepositorio, IProfissionalRepository profissionalRepositorio, IServicoLojaRepository servicoLojaRepositorio, ILojaRepository lojaRepositorio)
    {
        _repositorio = repositorio;
        _clienteRepositorio = clienteRepositorio;
        _profissionalRepositorio = profissionalRepositorio;
        _servicoLojaRepositorio = servicoLojaRepositorio;
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
        ValidarAgendamento(agendamento);

        return _repositorio.Cadastrar(agendamento);
    }

    private void ValidarAgendamento(Agendamento agendamento, int? agendamentoIgnoradoId = null)
    {

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

        var servicoLoja = _servicoLojaRepositorio.BuscarPorId(agendamento.ServicoLojaId);

        if (servicoLoja == null)
            throw new Exception("Serviço não encontrado.");

        var inicioNovo = agendamento.DataAgendamento;
        var fimNovo = inicioNovo.AddMinutes(servicoLoja.TempoMinutos);

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

        if (_repositorio.ExisteConflito(agendamento.ProfissionalId, inicioNovo, fimNovo, agendamentoIgnoradoId))
            throw new Exception("Horário indisponível.");
    }

    public void Atualizar(Agendamento agendamentoAtualizado)
    {
        ValidarAgendamento(agendamentoAtualizado, agendamentoAtualizado.Id);
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
