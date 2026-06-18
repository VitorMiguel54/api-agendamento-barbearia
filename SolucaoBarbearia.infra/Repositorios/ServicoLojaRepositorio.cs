using Dominio.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SolucaoBarbearia.dominio.Interfaces;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class ServicoLojaRepositorio : IServicoLojaRepository
    {
        private readonly string _connectionString;

        public ServicoLojaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Cadastrar(ServicoLoja servicoLoja)
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

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<ServicoLoja> Listar()
        {
            var servicosLoja = new List<ServicoLoja>();

            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"SELECT id, loja_id, servico_id, preco, tempo_minutos, ativo
                               FROM tb_servico_loja
                               WHERE ativo = 1";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        servicosLoja.Add(new ServicoLoja
                        {
                            Id = Convert.ToInt32(leitor["id"]),
                            LojaId = Convert.ToInt32(leitor["loja_id"]),
                            ServicoId = Convert.ToInt32(leitor["servico_id"]),
                            Preco = Convert.ToDecimal(leitor["preco"]),
                            TempoMinutos = Convert.ToInt32(leitor["tempo_minutos"]),
                            Ativo = Convert.ToBoolean(leitor["ativo"])
                        });
                    }
                }
            }

            return servicosLoja;
        }

        public ServicoLoja BuscarPorId(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"SELECT id, loja_id, servico_id, preco, tempo_minutos, ativo
                               FROM tb_servico_loja
                               WHERE id = @id AND ativo = 1";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new ServicoLoja
                            {
                                Id = Convert.ToInt32(leitor["id"]),
                                LojaId = Convert.ToInt32(leitor["loja_id"]),
                                ServicoId = Convert.ToInt32(leitor["servico_id"]),
                                Preco = Convert.ToDecimal(leitor["preco"]),
                                TempoMinutos = Convert.ToInt32(leitor["tempo_minutos"]),
                                Ativo = Convert.ToBoolean(leitor["ativo"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Atualizar(ServicoLoja servicoLoja)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"UPDATE tb_servico_loja
                               SET loja_id = @lojaId,
                                   servico_id = @servicoId,
                                   preco = @preco,
                                   tempo_minutos = @tempoMinutos,
                                   ativo = @ativo
                               WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", servicoLoja.Id);
                    comando.Parameters.AddWithValue("@lojaId", servicoLoja.LojaId);
                    comando.Parameters.AddWithValue("@servicoId", servicoLoja.ServicoId);
                    comando.Parameters.AddWithValue("@preco", servicoLoja.Preco);
                    comando.Parameters.AddWithValue("@tempoMinutos", servicoLoja.TempoMinutos);
                    comando.Parameters.AddWithValue("@ativo", servicoLoja.Ativo);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Remover(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"UPDATE tb_servico_loja
                               SET ativo = 0
                               WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
