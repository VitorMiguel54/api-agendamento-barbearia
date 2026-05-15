using Dominio.Models;

namespace SolucaoBarbearia.servico.Interfaces
{
    public interface ILojaService
    {
        List<Loja> Listar();

        Loja BuscarPorId(int id);

        bool Cadastrar(Loja loja);

        void Atualizar(Loja lojaAtualizado);

        void Remover(int id);
    }
}
