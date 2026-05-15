using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SolucaoBarbearia.dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Models;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class ClienteRepositorio : IClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Cadastrar(Cliente cliente)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"INSERT INTO tb_cliente 
                (nome, telefone, email)
                OUTPUT INSERTED.id
                VALUES 
                (@nome, @telefone, @email)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@email", cliente.Email);

                    int linhasAfetadas = comando.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
        }

        public bool ExisteEmailOuTelefone(string email, string telefone)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
                SELECT COUNT(*)
                FROM tb_cliente
                WHERE email = @email OR telefone = @telefone";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@email", email ?? "");
                    comando.Parameters.AddWithValue("@telefone", telefone ?? "");

                    int quantidade = Convert.ToInt32(comando.ExecuteScalar());

                    return quantidade > 0;
                }
            }
        }

        public List<Cliente> Listar()
        {
            var listaClientes = new List<Cliente>();

            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = $"SELECT id, nome, telefone, email, data_criacao, data_atualizacao, ativo FROM tb_cliente WHERE ativo = 1";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        listaClientes.Add(new Cliente()
                        {
                            Id = Convert.ToInt32(leitor["id"]),
                            Nome = leitor["nome"].ToString(),
                            Telefone = leitor["telefone"].ToString(),
                            Email = leitor["email"].ToString(),
                            DataCriacao = Convert.ToDateTime(leitor["data_criacao"]),
                            DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null : Convert.ToDateTime(leitor["data_atualizacao"]),
                            Ativo = Convert.ToBoolean(leitor["ativo"])
                        });
                    }
                }
            }

            return listaClientes;
        }

        public Cliente BuscarPorId(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        SELECT 
            c.id,
            c.nome,
            c.telefone,
            c.email,
            c.data_criacao,
            c.data_atualizacao,
            c.ativo
        FROM tb_cliente c
        WHERE c.id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new Cliente
                            {
                                Id = (int)leitor["id"],
                                Nome = (string)leitor["nome"],
                                Telefone = (string)leitor["telefone"],
                                Email = (string)leitor["email"],
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

        public void Atualizar(Cliente cliente)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        UPDATE tb_cliente
        SET 
            nome = @nome,
            telefone = @telefone,
            email = @email,
            data_atualizacao = SYSDATETIME()
        WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", cliente.Id);
                    comando.Parameters.AddWithValue("@nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@email", cliente.Email);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Remover(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();


                string sql = @"UPDATE tb_cliente
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
