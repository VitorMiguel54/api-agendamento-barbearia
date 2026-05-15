using Dominio.Models;

namespace SolucaoBarbearia.servico.Interfaces
{
    public interface IServicoLojaService
    {
        List<ServicoLoja> Listar();

        ServicoLoja BuscarPorId(int id);

        bool Cadastrar(ServicoLoja servicoLoja);

        void Atualizar(ServicoLoja servicoLojaAtualizado);

        void Remover(int id);
    }
}