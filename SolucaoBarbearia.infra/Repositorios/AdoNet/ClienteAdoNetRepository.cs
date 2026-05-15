//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using SolucaoBarbearia.dominio.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SolucaoBarbearia.dominio.Entities;

//namespace SolucaoBarbearia.infra.Repositorios.AdoNet
//{
//    public class ClienteAdoNetRepository : IClienteRepository
//    {
//        private readonly string _connectionString;

//        public ClienteAdoNetRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
//        }

//        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
//        {
//            var clientes = new List<Cliente>();

//            using var connection = new SqlConnection(_connectionString);
//            await connection.OpenAsync();

//            using var command = new SqlCommand(
//                "SELECT Id, Nome, Telefone, Email, DataCadastro FROM tb_cliente",
//                connection);

//            using var reader = await command.ExecuteReaderAsync();

//            while (await reader.ReadAsync())
//                clientes.Add(MapearCliente(reader));

//            return clientes;
//        }

//        public async Task<Cliente?> ObterPorIdAsync(int id)
//        {
//            using var connection = new SqlConnection(_connectionString);
//            await connection.OpenAsync();

//            using var command = new SqlCommand(
//                "SELECT Id, Nome, Telefone, Email, DataCadastro FROM tb_cliente WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", id);

//            using var reader = await command.ExecuteReaderAsync();

//            return await reader.ReadAsync() ? MapearCliente(reader) : null;
//        }

//        public async Task<int> CadastrarAsync(Cliente cliente)
//        {
//            using var connection = new SqlConnection(_connectionString);
//            await connection.OpenAsync();

//            const string sql = """
//            INSERT INTO tb_cliente (Nome, Telefone, Email, DataCadastro)
//            VALUES (@Nome, @Telefone, @Email, @DataCadastro);
//            SELECT SCOPE_IDENTITY();
//            """;

//            using var command = new SqlCommand(sql, connection);
//            command.Parameters.AddWithValue("@Nome", cliente.Nome);
//            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
//            command.Parameters.AddWithValue("@Email", cliente.Email);
//            command.Parameters.AddWithValue("@DataCadastro", cliente.DataCriacao);

//            var result = await command.ExecuteScalarAsync();
//            return Convert.ToInt32(result);
//        }

//        public async Task<bool> AtualizarAsync(Cliente cliente)
//        {
//            using var connection = new SqlConnection(_connectionString);
//            await connection.OpenAsync();

//            const string sql = """
//            UPDATE tb_cliente
//            SET Nome = @Nome, Telefone = @Telefone, Email = @Email
//            WHERE Id = @Id
//            """;

//            using var command = new SqlCommand(sql, connection);
//            command.Parameters.AddWithValue("@Id", cliente.Id);
//            command.Parameters.AddWithValue("@Nome", cliente.Nome);
//            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
//            command.Parameters.AddWithValue("@Email", cliente.Email);

//            var rowsAffected = await command.ExecuteNonQueryAsync();
//            return rowsAffected > 0;
//        }

//        public async Task<bool> RemoverAsync(int id)
//        {
//            using var connection = new SqlConnection(_connectionString);
//            await connection.OpenAsync();

//            using var command = new SqlCommand("DELETE FROM tb_cliente WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);

//            var rowsAffected = await command.ExecuteNonQueryAsync();
//            return rowsAffected > 0;
//        }

//        private static Cliente MapearCliente(IDataReader reader) => new()
//        {
//            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//            Nome = reader.GetString(reader.GetOrdinal("Nome")),
//            Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone"))
//                ? string.Empty
//                : reader.GetString(reader.GetOrdinal("Telefone")),
//            Email = reader.IsDBNull(reader.GetOrdinal("Email"))
//                ? string.Empty
//                : reader.GetString(reader.GetOrdinal("Email")),
//            DataCriacao = reader.GetDateTime(reader.GetOrdinal("DataCadastro"))
//        };
//    }
//}
