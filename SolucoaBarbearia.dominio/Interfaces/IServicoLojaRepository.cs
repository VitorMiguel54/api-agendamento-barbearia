using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Models;

namespace SolucaoBarbearia.dominio.Interfaces
{
    public interface IServicoLojaRepository
    {
        bool Cadastrar(ServicoLoja servico);
        List<ServicoLoja> Listar();
        ServicoLoja BuscarPorId(int id);
        void Atualizar(ServicoLoja servico);
        void Remover(int id);
    }
}
