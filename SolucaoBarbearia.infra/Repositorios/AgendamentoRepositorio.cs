using Dominio.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SolucaoBarbearia.dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.infra.Repositorios
{
    public class AgendamentoRepositorio : IAgendamentoRepository
    {
        private readonly string _connectionString;

        public AgendamentoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Cadastrar(Agendamento agendamento)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (SqlTransaction transacao = conexao.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    string sqlDuracao = @"SELECT tempo_minutos
                                          FROM tb_servico_loja
                                          WHERE id = @servico_loja_id AND ativo = 1";

                    int? duracaoMinutos = null;

                    using (SqlCommand comandoDuracao = new SqlCommand(sqlDuracao, conexao, transacao))
                    {
                        comandoDuracao.Parameters.AddWithValue("@servico_loja_id", agendamento.ServicoLojaId);
                        var resultado = comandoDuracao.ExecuteScalar();

                        if (resultado != null && resultado != DBNull.Value)
                        {
                            duracaoMinutos = Convert.ToInt32(resultado);
                        }
                    }

                    if (duracaoMinutos == null)
                    {
                        transacao.Rollback();
                        throw new Exception("Serviço não encontrado.");
                    }

                    DateTime fimAgendamento = agendamento.DataAgendamento.AddMinutes(duracaoMinutos.Value);

                    if (ExisteConflitoTransacao(conexao, transacao, agendamento.ProfissionalId, agendamento.DataAgendamento, fimAgendamento, null))
                    {
                        transacao.Rollback();
                        throw new Exception("Horário indisponível.");
                    }

                    string sql = @"INSERT INTO tb_agendamento
                (cliente_id, servico_loja_id, profissional_id, data_agendamento, status)
                OUTPUT INSERTED.id
                VALUES 
                (@cliente_id, @servico_loja_id, @profissional_id, @data_agendamento, @status)";

                    using (SqlCommand comando = new SqlCommand(sql, conexao, transacao))
                    {
                        comando.Parameters.AddWithValue("@cliente_id", agendamento.ClienteId);
                        comando.Parameters.AddWithValue("@servico_loja_id", agendamento.ServicoLojaId);
                        comando.Parameters.AddWithValue("@profissional_id", agendamento.ProfissionalId);
                        comando.Parameters.AddWithValue("@data_agendamento", agendamento.DataAgendamento);
                        comando.Parameters.AddWithValue("@status", agendamento.Status);

                        agendamento.Id = Convert.ToInt32(comando.ExecuteScalar());
                        transacao.Commit();
                        return agendamento.Id > 0;
                    }
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

        public bool ExisteConflito(int profissionalId, DateTime inicio, DateTime fim, int? agendamentoIgnoradoId = null)
        {
            using (SqlConnection conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                return ExisteConflitoTransacao(conexao, null, profissionalId, inicio, fim, agendamentoIgnoradoId);
            }
        }

        private static bool ExisteConflitoTransacao(
            SqlConnection conexao,
            SqlTransaction? transacao,
            int profissionalId,
            DateTime inicio,
            DateTime fim,
            int? agendamentoIgnoradoId)
        {
            string sql = @"
        SELECT COUNT(1)
        FROM tb_agendamento a WITH (UPDLOCK, HOLDLOCK)
        INNER JOIN tb_servico_loja sl ON sl.id = a.servico_loja_id
        WHERE a.profissional_id = @profissional_id
          AND a.status IN ('PENDENTE', 'CONFIRMADO')
          AND (@agendamento_ignorado_id IS NULL OR a.id <> @agendamento_ignorado_id)
          AND @inicio < DATEADD(MINUTE, sl.tempo_minutos, a.data_agendamento)
          AND @fim > a.data_agendamento";

            using (SqlCommand comando = new SqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@profissional_id", profissionalId);
                comando.Parameters.AddWithValue("@inicio", inicio);
                comando.Parameters.AddWithValue("@fim", fim);
                comando.Parameters.AddWithValue("@agendamento_ignorado_id", agendamentoIgnoradoId.HasValue ? agendamentoIgnoradoId.Value : DBNull.Value);

                int quantidade = Convert.ToInt32(comando.ExecuteScalar());
                return quantidade > 0;
            }
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
            data_atualizacao = SYSDATETIME()
        WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", agendamento.Id);
                    comando.Parameters.AddWithValue("@cliente_id", agendamento.ClienteId);
                    comando.Parameters.AddWithValue("@servico_loja_id", agendamento.ServicoLojaId);
                    comando.Parameters.AddWithValue("@profissional_id", agendamento.ProfissionalId);
                    comando.Parameters.AddWithValue("@data_agendamento", agendamento.DataAgendamento);

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
