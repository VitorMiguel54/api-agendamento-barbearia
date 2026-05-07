using Dominio.Models;
using SolucaoBarbearia.infra.Repositorios;

public class LojaService
{
    private readonly LojaRepositorio _repositorio;

    public LojaService(LojaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public List<Loja> Listar()
    {
        return _repositorio.Listar();
    }

    public Loja BuscarPorId(int id)
    {
        return _repositorio.BuscarPorId(id);
    }

    public void Cadastrar(Loja loja)
    {
        if (loja.HoraFechamento <= loja.HoraAbertura)
        {
            throw new Exception(
                "A hora de fechamento deve ser maior que a hora de abertura.");
        }
        _repositorio.Cadastrar(loja);
    }

    public void Atualizar(Loja lojaAtualizado)
    {
        if (lojaAtualizado.HoraFechamento <= lojaAtualizado.HoraAbertura)
        {
            throw new Exception(
                "A hora de fechamento deve ser maior que a hora de abertura.");
        }
        _repositorio.Atualizar(lojaAtualizado);
    }

    public void Remover(int id)
    {
        _repositorio.Remover(id);
    }
}