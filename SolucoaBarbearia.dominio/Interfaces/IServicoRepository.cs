using Dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.dominio.Interfaces
{
    public interface IServicoRepository
    {
        bool Cadastrar(Servico loja);
        List<Servico> Listar();
        Servico BuscarPorId(int id);
        void Atualizar(Servico loja);
        void Remover(int id);
    }
}
