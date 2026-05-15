using Dominio.Models;

namespace SolucaoBarbearia.servico.Interfaces
{
    public interface IServicoService
    {
        List<Servico> Listar();

        Servico BuscarPorId(int id);

        bool Cadastrar(Servico servico);

        void Atualizar(Servico servicoAtualizado);

        void Remover(int id);
    }
}