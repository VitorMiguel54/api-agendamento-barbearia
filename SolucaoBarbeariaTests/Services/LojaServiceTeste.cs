using Dominio.Models;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;
using Xunit;

namespace SolucaoBarbeariaTests.Services
{
    public class LojaServiceTeste
    {
        private readonly Mock<ILojaRepository> _repositoryMock;
        private readonly ILojaService _service;

        public LojaServiceTeste()
        {
            _repositoryMock = new Mock<ILojaRepository>();
            _service = new LojaService(_repositoryMock.Object);
        }

        // Cenário Feliz (LISTAR)
        [Fact]
        public void ListarLojas_DeveRetornarLojasPreenchidas()
        {
            List<Loja> lojasFakes = new List<Loja>();
            lojasFakes.Add(new Loja() { Id = 1, Nome = "Senador" });
            lojasFakes.Add(new Loja() { Id = 2, Nome = "Patao" });
       
            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(lojasFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Senador", resultado[0].Nome);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        // Cenário Triste (LISTAR)
        [Fact]
        public void ListarLojas_NaoExistemLojas()
        {
            List<Loja> lojasFakes = new List<Loja>();

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(lojasFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        //Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarLoja_DeveRetornarLojaCadastrada()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Loja>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Loja() { Nome = "Patao", HoraAbertura = new TimeSpan(9,0,0), HoraFechamento = new TimeSpan(17,0,0)});

            Assert.True(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Loja>()), Times.Once);
        }

        //Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarLoja_HorarioDivergente()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Loja>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Loja() { Nome = "Patao", HoraAbertura = new TimeSpan(9, 0, 0), HoraFechamento = new TimeSpan(8, 0, 0) });

            Assert.False(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Loja>()), Times.Never);
        }

        //Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarLoja_NomeVazio()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Loja>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Loja() { Nome = "", HoraAbertura = new TimeSpan(9, 0, 0), HoraFechamento = new TimeSpan(18, 0, 0) });

            Assert.False(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Loja>()), Times.Never);
        }

        // Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarLoja_HoraAberturaVazio()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Loja>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Loja() { Nome = "Patao", HoraAbertura = new TimeSpan(), HoraFechamento = new TimeSpan(18, 0, 0) });

            Assert.False(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Loja>()), Times.Never);
        }

        // Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarLoja_HoraFechamentoVazio()
        {
            bool retornoRepositorio = true;

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Loja>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Loja() { Nome = "Patao", HoraAbertura = new TimeSpan(10, 0, 0), HoraFechamento = new TimeSpan() });

            Assert.False(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Loja>()), Times.Never);
        }

        // Cenário Feliz (BUSCAR ID)
        [Fact]
        public void BuscarPorIdLoja_RetornarIdSelecionado()
        {
            var lojaFalsa = new Loja
            {
                Id = 1,
                Nome = "Barbearia Senador",
                DataCriacao = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(lojaFalsa);

            var resultado = _service.BuscarPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Barbearia Senador", resultado.Nome);

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Once);
        }

        // Cenário Triste (BUSCAR ID)
        [Fact]
        public void BuscarPorIdLoja_IdInvalido_RetornarErro()
        {
            int idInvalido = 0;

            Assert.Throws<Exception>(() => _service.BuscarPorId(idInvalido));

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Never);
        }

        // Cenário Feliz (ALTERAR)
        [Fact]
        public void AtualizarLoja_RetornarLojaAtualizada()
        {
            var LojaNomeAtual = new Loja
            {
                Id = 1,
                Nome = "Loja Do Pedro",
                HoraAbertura = new TimeSpan(10, 0, 0),
                HoraFechamento = new TimeSpan(22, 0, 0),
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(LojaNomeAtual);

            _service.Atualizar(new Loja() { Id = 1, Nome = "Loja do Zé", HoraAbertura = new TimeSpan(10, 0, 0), HoraFechamento = new TimeSpan(22, 0, 0), });

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Loja>()), Times.Once);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarLoja_NaoDeveAtualizar()
        {
            var LojaASerAlterada = new Loja
            {
                Id = 1,
                Nome = "Loja Do Pedro",
            };

            Assert.Throws<Exception>(() =>
        _service.Atualizar(new Loja()
        {
            Id = 1,
            Nome = ""
        }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Loja>()), Times.Never);
        }

        //Cenário Feliz (REMOVER)
        [Fact]
        public void RemoverLoja_ExcluirLoja()
        {
            var loja = new Loja() { Id = 1 };

            _repositoryMock.Setup(r => r.BuscarPorId(It.IsAny<int>())).Returns(loja);
            _repositoryMock.Setup(r => r.Remover(It.IsAny<int>()));

            _service.Remover(1);

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.Remover(1), Times.Once);
        }

        //Cenário Triste (REMOVER)
        [Fact]
        public void RemoverLoja_LojaInexistente()
        {

            Assert.Throws<Exception>(() => _service.Remover(1));

            _repositoryMock.Verify(r => r.Remover(It.IsAny<int>()), Times.Never);
        }
    }
}
