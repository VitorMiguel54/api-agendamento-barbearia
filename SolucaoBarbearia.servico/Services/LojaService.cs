using Dominio.Models;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.infra.Repositorios;
using SolucaoBarbearia.servico.Interfaces;

public class LojaService : ILojaService
{
    private readonly ILojaRepository _repositorio;

    public LojaService(ILojaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public List<Loja> Listar()
    {
        return _repositorio.Listar();
    }

    public Loja BuscarPorId(int id)
    {
        if (id <= 0)
            throw new Exception("Id inválido.");

        return _repositorio.BuscarPorId(id);
    }

    public bool Cadastrar(Loja loja)
    {
        if (string.IsNullOrWhiteSpace(loja.Nome))
        {
            return false;
        }
        if (loja.HoraAbertura == TimeSpan.Zero)
        {
            return false;
        }
        if (loja.HoraFechamento == TimeSpan.Zero)
        {
            return false;
        }
        if (loja.HoraFechamento <= loja.HoraAbertura)
        {
            return false;
        }
        
        return _repositorio.Cadastrar(loja);
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
        var loja = _repositorio.BuscarPorId(id);

        if (loja == null)
        {
            throw new Exception("Loja não encontrada.");
        }

        _repositorio.Remover(id);
    }
}