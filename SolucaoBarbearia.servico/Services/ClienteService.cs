using Dominio.Models;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.DTOs;
using SolucaoBarbearia.servico.Interfaces;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repositorio;

    public ClienteService(IClienteRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public List<Cliente> Listar()
    {
        return _repositorio.Listar();
    }

    public Cliente BuscarPorId(int id)
    {
        if (id <= 0)
            throw new Exception("Id inválido.");

        return _repositorio.BuscarPorId(id);
    }

    public bool Cadastrar(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nome))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(cliente.Email) && string.IsNullOrWhiteSpace(cliente.Telefone))
        {
            throw new Exception("Informe pelo menos um email ou telefone.");
        }

        bool clienteExistente = _repositorio.ExisteEmailOuTelefone(cliente.Email, cliente.Telefone);

        if (clienteExistente)
        {
            throw new Exception("Já existe um cliente com esse email ou telefone.");
        }

        return _repositorio.Cadastrar(cliente);
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
        var cliente = _repositorio.BuscarPorId(id);

        if (cliente == null)
        {
            throw new Exception("Cliente não encontrado.");
        }

        _repositorio.Remover(id);
    }
}
