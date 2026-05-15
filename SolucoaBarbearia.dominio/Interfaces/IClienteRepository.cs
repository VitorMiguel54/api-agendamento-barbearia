using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Models;

namespace SolucaoBarbearia.dominio.Interfaces
{
    public interface IClienteRepository
    {
        bool Cadastrar(Cliente cliente);
        bool ExisteEmailOuTelefone(string email, string telefone);
        List<Cliente> Listar();
        Cliente BuscarPorId(int id);
        void Atualizar(Cliente cliente);
        void Remover(int id);
    }
}
