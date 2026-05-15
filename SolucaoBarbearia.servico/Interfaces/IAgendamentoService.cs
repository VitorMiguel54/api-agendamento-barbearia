using Dominio.Models;

namespace SolucaoBarbearia.servico.Interfaces
{
    public interface IAgendamentoService
    {
        List<Agendamento> Listar();

        Agendamento BuscarPorId(int id);

        bool Cadastrar(Agendamento agendamento);

        void Atualizar(Agendamento agendamentoAtualizado);

        void Remover(int id);
    }
}