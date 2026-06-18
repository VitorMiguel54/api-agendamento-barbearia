using Dominio.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SolucaoBarbearia.dominio.Interfaces;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class ProfissionalRepositorio : IProfissionalRepository
    {
        private readonly string _connectionString;

        public ProfissionalRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Cadastrar(Profissional profissional)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"INSERT INTO tb_profissional 
                (loja_id, nome)
                OUTPUT INSERTED.id
                VALUES 
                (@loja_id, @nome)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@loja_id", profissional.LojaId);
                    comando.Parameters.AddWithValue("@nome", profissional.Nome);

                    profissional.Id = Convert.ToInt32(comando.ExecuteScalar());
                    return profissional.Id > 0;
                }
            }
        }

        public List<Profissional> Listar()
        {
            var listarProfissionais = new List<Profissional>();

            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = $"SELECT id, loja_id, nome, data_criacao, data_atualizacao, ativo FROM tb_profissional WHERE ativo = 1";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        listarProfissionais.Add(new Profissional()
                        {
                            Id = Convert.ToInt32(leitor["id"]),
                            LojaId = Convert.ToInt32(leitor["loja_id"]),
                            Nome = leitor["nome"].ToString(),
                            DataCriacao = Convert.ToDateTime(leitor["data_criacao"]),
                            DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null
                        :   Convert.ToDateTime(leitor["data_atualizacao"]),

                            Ativo = Convert.ToBoolean(leitor["ativo"])
                        });
                    }
                }
            }

            return listarProfissionais;
        }

        public Profissional BuscarPorId(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        SELECT 
            p.id,
            p.loja_id,
            p.nome,
            p.data_criacao,
            p.data_atualizacao,
            p.ativo
        FROM tb_profissional p
        WHERE p.id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new Profissional
                            {
                                Id = (int)leitor["id"],
                                LojaId = (int)leitor["loja_id"],
                                Nome = (string)leitor["nome"],
                                DataCriacao = (DateTime)leitor["data_criacao"],
                                DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null : (DateTime)leitor["data_atualizacao"],
                                Ativo = (bool)leitor["ativo"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Atualizar(Profissional profissional)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        UPDATE tb_profissional
        SET 
            loja_id = @loja_id,
            nome = @nome,
            data_atualizacao = SYSDATETIME()
        WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", profissional.Id);
                    comando.Parameters.AddWithValue("@loja_id", profissional.LojaId);
                    comando.Parameters.AddWithValue("@nome", profissional.Nome);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Remover(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();


                string sql = @"UPDATE tb_profissional
                           SET ativo = 0,
                           data_atualizacao = SYSDATETIME()
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
