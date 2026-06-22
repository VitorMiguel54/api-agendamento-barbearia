using Dominio.Models;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;
using Xunit;

namespace SolucaoBarbeariaTests.Services
{
    public class ClienteServiceTeste
    {
        private readonly Mock<IClienteRepository> _repositoryMock;
        private readonly IClienteService _service;

        public ClienteServiceTeste()
        {
            _repositoryMock = new Mock<IClienteRepository>();
            _service = new ClienteService(_repositoryMock.Object);
        }


        // Cenário Feliz (LISTAR)
        [Fact]
        public void ListarClientes_DeveRetornarClientesPreenchidos()
        {
            List<Cliente> clientesFakes = new List<Cliente>();
            clientesFakes.Add(new Cliente() { Id = 1, Nome = "Fabio" });
            clientesFakes.Add(new Cliente() { Id = 2, Nome = "Julia" });

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(clientesFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Fabio", resultado[0].Nome);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        // Cenário Triste (LISTAR)
        [Fact]
        public void ListarClientes_NaoExistemClientes()
        {
            List<Cliente> clientesFakes = new List<Cliente>();

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(clientesFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        //Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarCliente_DeveRetornarClienteCadastrado()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Cliente>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Cliente() { Nome = "Fabio", Telefone = "11988852320", Email = "fabio@gmail.com" });

            Assert.True(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Cliente>()), Times.Once);
        }

        //Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarCliente_RetornarNomeSemPreenchimento()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Cliente>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Cliente() { Nome = "", Telefone = "11988852320", Email = "fabio@gmail.com" });

            Assert.False(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Cliente>()), Times.Never);
        }

        //Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarCliente_RetornarClienteCadastradoSemTelefone()
        {

            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Cliente>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Cliente() { Nome = "Fabio", Telefone = "", Email = "fabio@gmail.com" });

            Assert.True(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Cliente>()), Times.Once);
        }

        //Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarCliente_RetornarClienteCadastradoSemEmail()
        {

            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Cliente>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Cliente() { Nome = "Fabio", Telefone = "11988852320", Email = "" });

            Assert.True(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Cliente>()), Times.Once);
        }

        //Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarCliente_RetornarEmailETelefoneSemPreenchimento()
        {
            Assert.Throws<Exception>(() => _service.Cadastrar(new Cliente() { Nome = "Fabio", Telefone = "", Email = "" }));

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Cliente>()), Times.Never);
        }

        //Cenário Feliz (BUSCAR ID)
        [Fact]
        public void BuscarPorIdCliente_RetornarIdClienteEncontrado()
        {
            var clienteFalso = new Cliente
            {
                Id = 1,
                Nome = "Fabio Luis",
                Telefone = "11988852320",
                Email = "fabio@gmail.com",
                DataCriacao = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(clienteFalso);

            var resultado = _service.BuscarPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Fabio Luis", resultado.Nome);
            Assert.Equal("11988852320", resultado.Telefone);
            Assert.Equal("fabio@gmail.com", resultado.Email);

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Once);
        }

        //Cenário Triste (BUSCA ID)
        [Fact]
        public void BuscaPorIdCliente_RetornarIdClienteInvalido()
        {
            int idInvalido = 0;

            Assert.Throws<Exception>(() => _service.BuscarPorId(idInvalido));

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Never);
        }

        // Cenário Feliz (ALTERAR)
        [Fact]
        public void AtualizarCliente_RetornarClienteAtualizado()
        {
            var ClienteNomeAtual = new Cliente
            {
                Id = 1,
                Nome = "Fabio Luis",
                Telefone = "11988852320",
                Email = "fabio@gmail.com",
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(ClienteNomeAtual);

            _service.Atualizar(new Cliente() { Id = 1, Nome = "Fabio Luis", Telefone = "11988852320", Email = "luis@gmail.com" });

            _repositoryMock.Verify(r => r.Atualizar(It.Is<Cliente>(l =>
                l.Id == 1 &&
                l.Nome == "Fabio Luis" &&
                l.Telefone == "11988852320" &&
                l.Email == "luis@gmail.com")), Times.Once);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarCliente_NaoDeveAtualizar()
        {
            var clienteASerAlterado = new Cliente
            {
                Id = 1,
                Nome = "Loja Do Pedro",
                Telefone = "11988852320",
                Email = "fabio@gmail.com",
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(clienteASerAlterado);

            Assert.Throws<Exception>(() =>
        _service.Atualizar(new Cliente()
        {
            Id = 1,
            Nome = "",
            Telefone = "11988852320",
            Email = "fabio@gmail.com"
        }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Cliente>()), Times.Never);
        }

        //Cenário Feliz (REMOVER)
        [Fact]
        public void RemoverCliente_RetornarClienteExcluido()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Cliente { Id = 1, Nome = "Fabio" });

            _service.Remover(1);

            _repositoryMock.Verify(r => r.Remover(1), Times.Once);
        }

        //Cenário Triste (REMOVER)
        [Fact]
        public void RemoverCliente_ClienteInexistente()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(default(Cliente)!);

            Assert.Throws<Exception>(() => _service.Remover(1));

            _repositoryMock.Verify(r => r.Remover(It.IsAny<int>()), Times.Never);
        }
    }
}
