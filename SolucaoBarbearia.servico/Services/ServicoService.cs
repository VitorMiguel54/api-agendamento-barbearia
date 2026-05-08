using Dominio.Models;
using SolucaoBarbearia.infra.Repositorios;

public class ServicoService
{
    private readonly ServicoRepositorio _repositorio;
    private readonly ServicoLojaRepositorio _servicoLojaRepositorio;

    public ServicoService(ServicoRepositorio repositorio, ServicoLojaRepositorio servicoLojaRepositorio)
    {
        _repositorio = repositorio;
        _servicoLojaRepositorio = servicoLojaRepositorio;
    }

    public List<dynamic> ListarServicosLoja()
    {
        return _repositorio.ListarServicosLoja();
    }

    public List<Servico> Listar()
    {
        return _repositorio.Listar();
    }

    public Servico BuscarPorId(int id)
    {
        return _repositorio.BuscarPorId(id);
    }

    public void Cadastrar(Servico servico)
    {
        _repositorio.Cadastrar(servico);

        var servicoLoja = new ServicoLoja
        {
            LojaId = servico.LojaId,
            ServicoId = servico.Id,
            Preco = servico.Preco,
            TempoMinutos = servico.TempoMinutos,
            Ativo = true
        };
        _servicoLojaRepositorio.Criar(servicoLoja);
    }

    public void Atualizar(Servico servicoAtualizado)
    {
        if (string.IsNullOrWhiteSpace(servicoAtualizado.Nome))
        {
            throw new Exception("Nome obrigatório.");
        }
        _repositorio.Atualizar(servicoAtualizado);
    }

    public void Remover(int id)
    {
        _repositorio.Remover(id);
    }
}