using Dominio.Models;

namespace SolucaoBarbearia.servico.Interfaces
{
    public interface IClienteService
    {
        List<Cliente> Listar();

        Cliente BuscarPorId(int id);

        bool Cadastrar(Cliente cliente);

        void Atualizar(Cliente clienteAtualizado);

        void Remover(int id);
    }
}

