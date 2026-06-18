using Dominio.Models;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;
using Xunit;

namespace SolucaoBarbeariaTests.Services
{
    public class ServicoServiceTeste
    {
        private readonly Mock<IServicoRepository> _repositoryMock;
        private readonly Mock<IServicoLojaRepository> _servicoLojaRepositoryMock;
        private readonly Mock<ILojaRepository> _lojaRepositoryMock;
        private readonly IServicoService _service;

        public ServicoServiceTeste()
        {
            _repositoryMock = new Mock<IServicoRepository>();
            _servicoLojaRepositoryMock = new Mock<IServicoLojaRepository>();
            _lojaRepositoryMock = new Mock<ILojaRepository>();
            _service = new ServicoService(_repositoryMock.Object, _servicoLojaRepositoryMock.Object, _lojaRepositoryMock.Object);
        }


        // CENARIO FELIZ
        [Fact]
        public void ListarServicos_DeveRetornarServicosPreenchidos()
        {
            List<Servico> servicosFakes = new List<Servico>();
            servicosFakes.Add(new Servico() { Id = 1, Nome = "Lenhador" });
            servicosFakes.Add(new Servico() { Id = 2, Nome = "Cortador" });

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(servicosFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Lenhador", resultado[0].Nome);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        // CENARIO TRISTE
        [Fact]
        public void ListarServicos_NaoExistemServicos()
        {
            List<Servico> servicosFakes = new List<Servico>();

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(servicosFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Count);

            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        //Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarServico_RetornarServicoCadastrado()
        {
            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(new Loja());

            _repositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Servico>()))
                .Returns(true);

            var resultado = _service.Cadastrar(new Servico() { LojaId = 1, Nome = "Engraxate", Descricao = "Engraxar Sapatos", TempoMinutos = 45, Preco = 30 });

            Assert.True(resultado);

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Servico>()), Times.Once);
        }

        // Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarServico_RetornarErroNomeVazio()
        {

            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns((Loja)null);

            Assert.Throws<Exception>(() => _service.Cadastrar(new Servico() { Nome = "", Descricao = "Engraxar Sapatos", TempoMinutos = 45, Preco = 30 }));

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Servico>()), Times.Never);
        }

        // Cenário Feliz (CADASTRAR)
        [Fact]
        public void CadastrarServico_RetornarCadastroSemDescricao()
        {

            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns((Loja)null);

            Assert.Throws<Exception>(() => _service.Cadastrar(new Servico() { Nome = "Engraxate", Descricao = "", TempoMinutos = 45, Preco = 30 }));

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Servico>()), Times.Never);
        }

        // Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarServico_RetornarErroTempoMinutosMenorQueZero()
        {

            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(new Loja());

            Assert.Throws<Exception>(() => _service.Cadastrar(new Servico() { Nome = "Engraxate", Descricao = "Engraxar Sapatos", TempoMinutos = 0, Preco = 45 }));

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Servico>()), Times.Never);
        }

        // Cenário Triste (CADASTRAR)
        [Fact]
        public void CadastrarServico_RetornarErroTempoMinutosMenorQueDez()
        {
                
            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(new Loja());

            Assert.Throws<Exception>(() => _service.Cadastrar(new Servico() { Nome = "Engraxate", Descricao = "Engraxar Sapatos", Preco = 30, TempoMinutos = 8 }));

            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Servico>()), Times.Never);
        }

        // Cenário Feliz (BUSCAR ID)
        [Fact]
        public void BuscarPorIdServico_RetornarIdServicoSelecinado()
        {
            var servicoFalso = new Servico
            {
                Id = 1,
                LojaId = 1,
                Nome = "Engraxate",
                Descricao = "Engraxar Sapatos",
                TempoMinutos = 30,
                Preco = 45,
                DataCriacao = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(It.IsAny<int>()))
                .Returns(servicoFalso);

            var resultado = _service.BuscarPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(1, resultado.LojaId);
            Assert.Equal("Engraxate", resultado.Nome);
            Assert.Equal("Engraxar Sapatos", resultado.Descricao);
            Assert.Equal(30, resultado.TempoMinutos);
            Assert.Equal(45, resultado.Preco);

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
        public void AtualizarServico_RetornarAtualizacaoServico()
        {
            var ServicoNomeAtual = new Servico
            {
                Id = 1,
                LojaId = 1,
                Nome = "Engraxate",
                Descricao = "Engraxar Sapatos",
                TempoMinutos = 30,
                Preco = 45,
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(ServicoNomeAtual);

            _service.Atualizar(new Servico() { Id = 1, LojaId = 1, Nome = "Zeladoria", Descricao = "Limpar Salão", TempoMinutos = 300, Preco = 1850 });

            _repositoryMock.Verify(r => r.Atualizar(It.Is<Servico>(l =>
                l.Id == 1 &&
                l.LojaId == 1 &&
                l.Nome == "Zeladoria" &&
                l.Descricao == "Limpar Salão" &&
                l.TempoMinutos == 300 &&
                l.Preco == 1850)), Times.Once);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarServico_RetornarErroNomeVazio()
        {
            var NomeASerAlterado = new Servico
            {
                Id = 1,
                LojaId = 1,
                Nome = "Engraxate",
                Descricao = "Engraxar Sapatos",
                TempoMinutos = 30,
                Preco = 45,
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(NomeASerAlterado);

            Assert.Throws<Exception>(() => _service.Atualizar(new Servico() { Id = 1, LojaId = 1, Nome = "", Descricao = "Limpar Salão", TempoMinutos = 300, Preco = 1850 }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Servico>()), Times.Never);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarServico_RetornarErroLojaIdVazio()
        {
            var servicoASerAlterado = new Servico
            {
                Id = 1,
                LojaId = 1,
                Nome = "Engraxate",
                Descricao = "Engraxar Sapatos",
                TempoMinutos = 30,
                Preco = 45,
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(servicoASerAlterado);

            Assert.Throws<Exception>(() => _service.Atualizar(new Servico() { Id = 1, LojaId = 0, Nome = "Engraxate", Descricao = "Limpar Salão", TempoMinutos = 300, Preco = 1850 }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Servico>()), Times.Never);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarServico_RetornarErroTempoMinutosMenorQueDez()
        {
            var servicoASerAlterado = new Servico
            {
                Id = 1,
                LojaId = 1,
                Nome = "Engraxate",
                Descricao = "Engraxar Sapatos",
                TempoMinutos = 30,
                Preco = 45,
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(servicoASerAlterado);

            Assert.Throws<Exception>(() => _service.Atualizar(new Servico() { Id = 1, LojaId = 1, Nome = "Engraxate", Descricao = "Limpar Salão", TempoMinutos = 5, Preco = 1850 }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Servico>()), Times.Never);
        }

        //Cenário Triste (ALTERAR)
        [Fact]
        public void AtualizarServico_RetornarErroPrecoMenorQueZero()
        {
            var servicoASerAlterado = new Servico
            {
                Id = 1,
                LojaId = 1,
                Nome = "Engraxate",
                Descricao = "Engraxar Sapatos",
                TempoMinutos = 30,
                Preco = 45,
            };

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(servicoASerAlterado);

            Assert.Throws<Exception>(() => _service.Atualizar(new Servico() { Id = 1, LojaId = 1, Nome = "Engraxate", Descricao = "Limpar Salão", TempoMinutos = 30, Preco = 0 }));

            _repositoryMock.Verify(r => r.Atualizar(It.IsAny<Servico>()), Times.Never);
        }

        //Cenário Feliz (REMOVER)
        [Fact]
        public void RemoverServico_RetornarServicoExcluido()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Servico { Id = 1, LojaId = 1, Nome = "Corte", Descricao = "Corte simples", TempoMinutos = 30, Preco = 50 });

            _service.Remover(1);

            _repositoryMock.Verify(r => r.Remover(1), Times.Once);
        }

        //Cenário Triste (REMOVER)
        [Fact]
        public void RemoverServico_RetornarServicoInexistente()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns((Servico)null);

            Assert.Throws<Exception>(() => _service.Remover(1));

            _repositoryMock.Verify(r => r.Remover(It.IsAny<int>()), Times.Never);
        }
    }
}
