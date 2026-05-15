using Dominio.Models;
using global::SolucaoBarbearia.dominio.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class LojaRepositorio : ILojaRepository
    {
        private readonly string _connectionString;

        public LojaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Cadastrar(Loja loja)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"INSERT INTO tb_loja 
                (nome, hora_abertura, hora_fechamento)
                VALUES 
                (@nome, @hora_abertura, @hora_fechamento)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", loja.Nome);
                    comando.Parameters.AddWithValue("@hora_abertura", loja.HoraAbertura);
                    comando.Parameters.AddWithValue("@hora_fechamento", loja.HoraFechamento);

                    int linhasAfetadas = comando.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
        }

        public List<Loja> Listar()
        {
            var listarLojas = new List<Loja>();

            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = $"SELECT id, nome, hora_abertura, hora_fechamento, data_criacao, data_atualizacao, ativo FROM tb_loja WHERE ativo = 1";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        listarLojas.Add(new Loja()
                        {
                            Id = Convert.ToInt32(leitor["id"]),
                            Nome = leitor["nome"].ToString(),
                            HoraAbertura = (TimeSpan)leitor["hora_abertura"],
                            HoraFechamento = (TimeSpan)leitor["hora_fechamento"],
                            DataCriacao = Convert.ToDateTime(leitor["data_criacao"]),
                            DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null : Convert.ToDateTime(leitor["data_atualizacao"]),
                            Ativo = Convert.ToBoolean(leitor["ativo"])
                        });
                    }
                }
            }

            return listarLojas;
        }

        public Loja BuscarPorId(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        SELECT 
            l.id,
            l.nome,
            l.hora_abertura,
            l.hora_fechamento,
            l.data_criacao,
            l.data_atualizacao,
            l.ativo
        FROM tb_loja l
        WHERE l.id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new Loja
                            {
                                Id = (int)leitor["id"],
                                Nome = (string)leitor["nome"],
                                HoraAbertura = (TimeSpan)leitor["hora_abertura"],
                                HoraFechamento = (TimeSpan)leitor["hora_fechamento"],
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

        public void Atualizar(Loja loja)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        UPDATE tb_loja
        SET 
            nome = @nome,
            hora_abertura = @hora_abertura,
            hora_fechamento = @hora_fechamento,
            data_atualizacao = SYSDATETIME()
        WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", loja.Id);
                    comando.Parameters.AddWithValue("@nome", loja.Nome);
                    comando.Parameters.AddWithValue("@hora_abertura", loja.HoraAbertura);
                    comando.Parameters.AddWithValue("@hora_fechamento", loja.HoraFechamento);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Remover(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();


                string sql = @"UPDATE tb_loja
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

        public bool Existe(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = "SELECT COUNT(1) FROM tb_loja WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    int quantidade = (int)comando.ExecuteScalar();

                    return quantidade > 0;
                }
            }
        }
    }
}