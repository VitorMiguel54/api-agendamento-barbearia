using Dominio.Models;
using SolucaoBarbearia.infra.Repositorios;

public class ClienteService
{
    private readonly ClienteRepositorio _repositorio;

    public ClienteService(ClienteRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public List<Cliente> Listar()
    {
        return _repositorio.Listar();
    }

    public Cliente BuscarPorId(int id)
    {
        return _repositorio.BuscarPorId(id);
    }

    public void Cadastrar(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Email) &&
    string.IsNullOrWhiteSpace(cliente.Telefone))
        {
            throw new Exception(
                "Informe pelo menos um email ou telefone.");
        }

        bool clienteExistente = _repositorio.ExisteEmailOuTelefone(cliente.Email, cliente.Telefone);

        if (clienteExistente)
        {
            throw new Exception("Já existe um cliente com esse email ou telefone.");
        }

        _repositorio.Cadastrar(cliente);
    }

    public void Atualizar(Cliente clienteAtualizado)
    {
        if (string.IsNullOrWhiteSpace(clienteAtualizado.Nome))
        {
            throw new Exception("Nome obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(clienteAtualizado.Email) &&
            string.IsNullOrWhiteSpace(clienteAtualizado.Telefone))
        {
            throw new Exception(
                "Informe pelo menos um email ou telefone.");
        }
        _repositorio.Atualizar(clienteAtualizado);
    }

    public void Remover(int id)
    {
        _repositorio.Remover(id);
    }
}