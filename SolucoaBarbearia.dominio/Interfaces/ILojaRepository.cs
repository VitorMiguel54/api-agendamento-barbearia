using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Models;
using Dominio.Models;

namespace SolucaoBarbearia.dominio.Interfaces
{
    public interface ILojaRepository
    {
        bool Existe(int id);
        bool Cadastrar(Loja loja);
        List<Loja> Listar();
        Loja BuscarPorId(int id);
        void Atualizar(Loja loja);
        void Remover(int id);
    }
}
