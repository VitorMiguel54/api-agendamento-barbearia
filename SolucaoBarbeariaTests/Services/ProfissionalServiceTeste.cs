using Dominio.Models;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;
using Xunit;

namespace SolucaoBarbeariaTests.Services
{
    public class ProfissionalServiceTeste
    {
        private readonly Mock<IProfissionalRepository> _repositoryMock;
        private readonly Mock<ILojaRepository> _lojaRepositoryMock;
        private readonly IProfissionalService _service;

        public ProfissionalServiceTeste()
        {
            _repositoryMock = new Mock<IProfissionalRepository>();
            _lojaRepositoryMock = new Mock<ILojaRepository>();
            _service = new ProfissionalService(_repositoryMock.Object, _lojaRepositoryMock.Object);
        }


        // Cenário Feliz (LISTAR)
        [Fact]
        public void ListarProfissionais_DeveRetornarProfissionaisPreenchidos()
        {
            List<Profissional> profissionaisFakes = new List<Profissional>();
            profissionaisFakes.Add(new Profissional() { Id = 1, Nome = "Leandro" });
            profissionaisFakes.Add(new Profissional() { Id = 2, Nome = "Gustavo" });

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(profissionaisFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Leandro", resultado[0].Nome);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        // Cenário Triste (LISTAR)
        [Fact]
        public void ListarProfissionais_NaoExistemProfissionais()
        {
            List<Profissional> profissionaisFakes = new List<Profissional>();

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(profissionaisFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Count);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        //Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarProfissional_RetornarProfissionalCadastrado()
        {
            bool retornoRepositorio = true;

            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Loja() { Id = 1, Nome = "LojaTeste" });

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Profissional>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Profissional() { LojaId = 1, Nome = "Leando Garcia" });

            Assert.True(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Profissional>()), Times.Once);
        }

        //Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarProfissional_RetornarFalhaNomeVazio()
        {
            bool retornoRepositorio = true;

            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Loja() { Id = 1, Nome = "LojaTeste" });

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Profissional>()))
                .Returns(retornoRepositorio);

            var resultado = _service.Cadastrar(new Profissional() { LojaId = 1, Nome = "" });

            Assert.False(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Profissional>()), Times.Never);
        }

        // Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarProfissional_RetornarErroLojaIdVazio()
        {
            Assert.Throws<Exception>(() => _service.Cadastrar(new Profissional() { LojaId = 0, Nome = "Leandro Garcia" }));

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Profissional>()), Times.Never);
        }

        // Cenário Feliz (BUSCAR ID)
        [Fact]
        public void BuscarPorIdProfissional_RetornarIdProfissionalSelecionado()
        {
            var profissionalFalso = new Profissional
            {
                Id = 1,
                LojaId = 1,
                Nome = "Leandro Garcia",
                DataCriacao = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(profissionalFalso);

            var resultado = _service.BuscarPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(1, resultado.LojaId);
            Assert.Equal("Leandro Garcia", resultado.Nome);

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Once);
        }

        // Cenário Triste (BUSCAR ID)
        [Fact]
        public void BuscarPorIdProfissional_RetornarErroIdInvalido()
        {
            int idInvalido = 0;

            Assert.Throws<Exception>(() => _service.BuscarPorId(idInvalido));

            _repositoryMock.Verify(r => r.BuscarPorId(It.IsAny<int>()), Times.Never);
        }

        // Cenário Feliz (ALTERAR)
        [Fact]
        public void AtualizarProfissional_RetornarAtualizacaoDeProfissional()
        {
            var ProfissionalNomeAtual = new Profissional
            {
                Id = 1,
                LojaId = 1,
                Nome = "Leandro Garcia"
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(ProfissionalNomeAtual);

            _service.Atualizar(new Profissional() { Id = 1, LojaId = 1, Nome = "Fernando Costa" });

            _repositoryMock.Verify(r => r.Atualizar(It.Is<Profissional>(l =>
                l.Id == 1 &&
                l.LojaId == 1 &&
                l.Nome == "Fernando Costa")), Times.Once);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarProfissional_RetornarErroNomeVazio()
        {
            var profissionalASerAlterado = new Profissional
            {
                Id = 1,
                LojaId = 1,
                Nome = "Leandro Garcia",
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(profissionalASerAlterado);

            Assert.Throws<Exception>(() => _service.Atualizar(new Profissional() { Id = 1, LojaId = 1, Nome = "" }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Profissional>()), Times.Never);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarProfissional_RetornarErroLojaIdVazio()
        {
            var profissionalASerAlterado = new Profissional
            {
                Id = 1,
                LojaId = 1,
                Nome = "Leandro Garcia",
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(profissionalASerAlterado);

            Assert.Throws<Exception>(() => _service.Atualizar(new Profissional() { Id = 1, LojaId = 0, Nome = "Leandro Garcia" }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Profissional>()), Times.Never);
        }

        //Cenário Feliz (REMOVER)
        [Fact]
        public void RemoverProfissional_RetornarProfissionalExcluido()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Profissional { Id = 1, LojaId = 1, Nome = "Leandro Garcia" });

            _service.Remover(1);

            _repositoryMock.Verify(r => r.Remover(1), Times.Once);
        }

        //Cenário Triste (REMOVER)
        [Fact]
        public void RemoverProfissional_RetornarProfissionalInexistente()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns((Profissional)null);

            Assert.Throws<Exception>(() => _service.Remover(1));

            _repositoryMock.Verify(r => r.Remover(It.IsAny<int>()), Times.Never);
        }
    }
}
