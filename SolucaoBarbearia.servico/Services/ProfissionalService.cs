using Dominio.Models;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;

public class ProfissionalService : IProfissionalService
{
    private readonly IProfissionalRepository _repositorio;
    private readonly ILojaRepository _lojaRepositorio;

    public ProfissionalService(IProfissionalRepository repositorio, ILojaRepository lojaRepositorio)
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
        if (id <= 0)
            throw new Exception("Id inválido.");

        return _repositorio.BuscarPorId(id);
    }

    public bool Cadastrar(Profissional profissional)
    {
        Console.WriteLine(profissional.LojaId);

        var loja = _lojaRepositorio.BuscarPorId(profissional.LojaId);

        if (string.IsNullOrWhiteSpace(profissional.Nome))
        {
            return false;
        }
        if (loja == null)
            throw new Exception("Loja não encontrada.");

        return _repositorio.Cadastrar(profissional);
    }

    public void Atualizar(Profissional profissionalAtualizado)
    {
        if (profissionalAtualizado.LojaId <= 0)
        {
            throw new Exception("Loja inválida.");
        }
        if (string.IsNullOrWhiteSpace(profissionalAtualizado.Nome))
        {
            throw new Exception("Nome obrigatório.");
        }
        _repositorio.Atualizar(profissionalAtualizado);
    }

    public void Remover(int id)
    {
        var profissional = _repositorio.BuscarPorId(id);

        if (profissional == null)
        {
            throw new Exception("Profissional não encontrado.");
        }

        _repositorio.Remover(id);
    }
}
