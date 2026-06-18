using Dominio.Models;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;

public class ServicoService : IServicoService
{
    private readonly IServicoRepository _repositorio;
    private readonly IServicoLojaRepository _servicoLojaRepositorio;
    private readonly ILojaRepository _lojaRepositorio;

    public ServicoService(IServicoRepository repositorio, IServicoLojaRepository servicoLojaRepositorio, ILojaRepository lojaRepositorio)
    {
        _repositorio = repositorio;
        _servicoLojaRepositorio = servicoLojaRepositorio;
        _lojaRepositorio = lojaRepositorio;
    }

    public List<ServicoLoja> ListarServicosLoja()
    {
        return _servicoLojaRepositorio.Listar();
    }

    public List<Servico> Listar()
    {
        return _repositorio.Listar();
    }

    public Servico BuscarPorId(int id)
    {
        if (id <= 0)
            throw new Exception("Id inválido.");

        return _repositorio.BuscarPorId(id);
    }

    public bool Cadastrar(Servico servico)
    {
        if (servico == null)
            throw new Exception("Serviço inválido.");

        if (servico.LojaId <= 0)
            throw new Exception("Loja inválida.");

        if (servico.Preco <= 0)
            throw new Exception("Preço inválido.");

        if (servico.TempoMinutos < 10)
            throw new Exception("Tempo inválido.");

        if (string.IsNullOrWhiteSpace(servico.Nome))
            return false;

        var loja = _lojaRepositorio.BuscarPorId(servico.LojaId);

        if (loja == null)
            throw new Exception("Loja não encontrada.");

        return _repositorio.Cadastrar(servico);
    }

    public void Atualizar(Servico servicoAtualizado)
    {
        if (servicoAtualizado.LojaId <= 0)
        {
            throw new Exception("Loja inválida.");
        }
        if (string.IsNullOrWhiteSpace(servicoAtualizado.Nome))
        {
            throw new Exception("Nome obrigatório.");
        }
        if (servicoAtualizado.TempoMinutos < 10)
        {
            throw new Exception("Tempo inválido.");
        }
        if (servicoAtualizado.Preco < 10)
        {
            throw new Exception("Preço inválido.");
        }
        _repositorio.Atualizar(servicoAtualizado);
    }

    public void Remover(int id)
    {
        var servico = _repositorio.BuscarPorId(id);

        if (servico == null)
        {
            throw new Exception("Serviço não encontrado.");
        }

        _repositorio.Remover(id);
    }
}
