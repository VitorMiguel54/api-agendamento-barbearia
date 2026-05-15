using Dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.dominio.Interfaces
{
    public interface IAgendamentoRepository
    {
        bool Cadastrar(Agendamento agendamento);
        List<Agendamento> Listar();
        Agendamento BuscarPorId(int id);
        void Atualizar(Agendamento agendamento);
        void Remover(int id);
    }
}
