using Dominio.Models;
using SolucaoBarbearia.infra.Repositorios;

public class ProfissionalService
{
    private readonly ProfissionalRepositorio _repositorio;
    private readonly LojaRepositorio _lojaRepositorio;

    public ProfissionalService(ProfissionalRepositorio repositorio, LojaRepositorio lojaRepositorio)
    {
        _repositorio = repositorio;
        _lojaRepositorio = lojaRepositorio;
    }

    public List<Profissional> Listar()
    {
        return _repositorio.Listar();
    }

    public Profissional BuscarPorId(int id)
    {
        return _repositorio.BuscarPorId(id);
    }

    public void Cadastrar(Profissional profissional)
    {
        Console.WriteLine(profissional.LojaId);

        var loja = _lojaRepositorio.BuscarPorId(profissional.LojaId);

        if (loja == null)
            throw new Exception("Loja não encontrada.");

        _repositorio.Cadastrar(profissional);
    }

    public void Atualizar(Profissional profissionalAtualizado)
    {
        _repositorio.Atualizar(profissionalAtualizado);
    }

    public void Remover(int id)
    {
        _repositorio.Remover(id);
    }
}