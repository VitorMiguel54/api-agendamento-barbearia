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
    public class AgendamentoRepositorio
    {
        private readonly string _connectionString;

        public AgendamentoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Cadastrar(Agendamento agendamento)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"INSERT INTO tb_agendamento
                (cliente_id, servico_loja_id, profissional_id, data_agendamento, status)
                VALUES 
                (@cliente_id, @servico_loja_id, @profissional_id, @data_agendamento, @status)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@cliente_id", agendamento.ClienteId);
                    comando.Parameters.AddWithValue("@servico_loja_id", agendamento.ServicoLojaId);
                    comando.Parameters.AddWithValue("@profissional_id", agendamento.ProfissionalId);
                    comando.Parameters.AddWithValue("@data_agendamento", agendamento.DataAgendamento);
                    comando.Parameters.AddWithValue("@status", agendamento.Status);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<Agendamento> Listar()
        {
            var listaAgendamentos = new List<Agendamento>();

            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = $"SELECT id, cliente_id, servico_loja_id, profissional_id, data_agendamento, status, data_criacao, data_atualizacao FROM tb_agendamento";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        listaAgendamentos.Add(new Agendamento()
                        {
                            Id = Convert.ToInt32(leitor["id"]),
                            ClienteId = Convert.ToInt32(leitor["cliente_id"]),
                            ServicoLojaId = Convert.ToInt32(leitor["servico_loja_id"]),
                            ProfissionalId = Convert.ToInt32(leitor["profissional_id"]),
                            DataAgendamento = Convert.ToDateTime(leitor["data_agendamento"]),
                            Status = leitor["status"]?.ToString() ?? "",
                            DataCriacao = Convert.ToDateTime(leitor["data_criacao"]),
                            DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null : Convert.ToDateTime(leitor["data_atualizacao"]),
                        });
                    }
                }
            }

            return listaAgendamentos;
        }

        public Agendamento BuscarPorId(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        SELECT 
            a.id,
            a.cliente_id,
            a.servico_loja_id,
            a.profissional_id,
            a.data_agendamento,
            a.status,
            a.data_criacao,
            a.data_atualizacao
        FROM tb_agendamento a
        WHERE a.id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new Agendamento
                            {
                                Id = (int)leitor["id"],
                                ClienteId = (int)leitor["cliente_id"],
                                ServicoLojaId = (int)leitor["servico_loja_id"],
                                ProfissionalId = (int)leitor["profissional_id"],
                                DataAgendamento = Convert.ToDateTime(leitor["data_agendamento"]),
                                Status = leitor["status"]?.ToString() ?? "",
                                DataCriacao = (DateTime)leitor["data_criacao"],
                                DataAtualizacao = leitor["data_atualizacao"] == DBNull.Value ? null : (DateTime)leitor["data_atualizacao"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Atualizar(Agendamento agendamento)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"
        UPDATE tb_agendamento
        SET 
            cliente_id = @cliente_id,
            servico_loja_id = @servico_loja_id,
            profissional_id = @profissional_id,
            data_agendamento = @data_agendamento,
            status = @status,
            data_atualizacao = SYSDATETIME()
        WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", agendamento.Id);
                    comando.Parameters.AddWithValue("@cliente_id", agendamento.ClienteId);
                    comando.Parameters.AddWithValue("@servico_loja_id", agendamento.ServicoLojaId);
                    comando.Parameters.AddWithValue("@profissional_id", agendamento.ProfissionalId);
                    comando.Parameters.AddWithValue("@data_agendamento", agendamento.DataAgendamento);
                    comando.Parameters.AddWithValue("@status", agendamento.Status);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Remover(int id)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();


                string sql = @"UPDATE tb_agendamento
                           SET
                                status = 'CANCELADO',
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