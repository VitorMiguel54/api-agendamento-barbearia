using Dominio.Models;
using SolucaoBarbearia.infra.Repositorios;

public class ServicoService
{
    private readonly ServicoRepositorio _repositorio;

    public ServicoService(ServicoRepositorio repositorio)
    {
        _repositorio = repositorio;
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