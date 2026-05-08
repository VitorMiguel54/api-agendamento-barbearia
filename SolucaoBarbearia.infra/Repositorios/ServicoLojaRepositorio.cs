using Dominio.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class ServicoLojaRepositorio
    {
        private readonly string _connectionString;

        public ServicoLojaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Criar(ServicoLoja servicoLoja)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"INSERT INTO tb_servico_loja
                (loja_id, servico_id, preco, tempo_minutos, ativo)
                VALUES
                (@lojaId, @servicoId, @preco, @tempoMinutos, @ativo)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@lojaId", servicoLoja.LojaId);
                    comando.Parameters.AddWithValue("@servicoId", servicoLoja.ServicoId);
                    comando.Parameters.AddWithValue("@preco", servicoLoja.Preco);
                    comando.Parameters.AddWithValue("@tempoMinutos", servicoLoja.TempoMinutos);
                    comando.Parameters.AddWithValue("@ativo", servicoLoja.Ativo);

                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}