using Dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.dominio.Interfaces
{
    public interface IProfissionalRepository
    {
        bool Cadastrar(Profissional profissional);
        List<Profissional> Listar();
        Profissional BuscarPorId(int id);
        void Atualizar(Profissional profissional);
        void Remover(int id);
    }
}
