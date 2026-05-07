using Dominio.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class ServicoRepositorio
    {
        private readonly string _connectionString;

        public ServicoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Cadastrar(Servico servico)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"INSERT INTO tb_servico
                (loja_id, nome, descricao, tempo_minutos, preco)
                VALUES 
                (@loja_id, @nome, @descricao, @tempo_minutos, @preco)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@loja_id", servico.LojaId);
                    comando.Parameters.AddWithValue("@nome", servico.Nome);
                    comando.Parameters.AddWithValue("@descricao", servico.Descricao);
                    comando.Parameters.AddWithValue("@tempo_minutos", servico.TempoMinutos);
                    comando.Parameters.AddWithValue("@preco", servico.Preco);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<Servico> Listar()
        {
            var listarServicos = new List<Servico>();

            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = $"SELECT id, loja_id, nome, descricao, tempo_minutos, preco, data_criacao, data_atualizacao, ativo FROM tb_servico WHERE ativo = 1";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        listarServicos.Add(new Servico()
                        {
                            Id = Convert.ToInt32(leitor["id"]),
                            LojaId = Convert.ToInt32(leitor["loja_id"]),
                            Nome = leitor["nome"].ToString(),
                            Descricao = leitor["descricao"].ToString(),
                            TempoMinutos = Convert.ToInt32(leitor["tempo_minutos"]),
                            Preco = Convert.ToDecimal(leitor["preco"]),
                            DataCriacao = Convert.ToDateTime(leitor["data_criacao"]),
                            DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null : Convert.ToDateTime(leitor["data_atualizacao"]),
                            Ativo = Convert.ToBoolean(leitor["ativo"])
                        });
                    }
                }
            }

            return listarServicos;
        }

        public Servico BuscarPorId(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        SELECT 
            s.id,
            s.loja_id,
            s.nome,
            s.descricao,
            s.tempo_minutos,
            s.preco,
            s.data_criacao,
            s.data_atualizacao,
            s.ativo
        FROM tb_servico s
        WHERE s.id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new Servico
                            {
                                Id = (int)leitor["id"],
                                LojaId = (int)leitor["loja_id"],
                                Nome = (string)leitor["nome"],
                                Descricao = (string)leitor["descricao"],
                                TempoMinutos = (int)leitor["tempo_minutos"],
                                Preco = (decimal)leitor["preco"],
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

        public void Atualizar(Servico servico)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        UPDATE tb_servico
        SET 
            loja_id = @loja_id,
            nome = @nome,
            descricao = @descricao,
            tempo_minutos = @tempo_minutos,
            preco = @preco,
            data_atualizacao = SYSDATETIME()
        WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", servico.Id);
                    comando.Parameters.AddWithValue("@loja_id", servico.LojaId);
                    comando.Parameters.AddWithValue("@nome", servico.Nome);
                    comando.Parameters.AddWithValue("@descricao", servico.Descricao);
                    comando.Parameters.AddWithValue("@tempo_minutos", servico.TempoMinutos);
                    comando.Parameters.AddWithValue("@preco", servico.Preco);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Remover(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();


                string sql = @"UPDATE tb_servico
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