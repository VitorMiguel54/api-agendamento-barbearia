using Dominio.Models;

namespace SolucaoBarbearia.servico.Interfaces
{
    public interface IProfissionalService
    {
        List<Profissional> Listar();

        Profissional BuscarPorId(int id);

        bool Cadastrar(Profissional profissional);

        void Atualizar(Profissional profissionalAtualizado);

        void Remover(int id);
    }
}