using Dominio.Models;
using Moq;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.servico.Interfaces;
using Xunit;

namespace SolucaoBarbeariaTests.Services
{
    public class AgendamentoServiceTeste
    {
        private readonly Mock<IAgendamentoRepository> _repositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<ILojaRepository> _lojaRepositoryMock;
        private readonly Mock<IProfissionalRepository> _profissionalRepositoryMock;
        private readonly Mock<IServicoLojaRepository> _servicoLojaRepositoryMock;
        private readonly IAgendamentoService _service;

        public AgendamentoServiceTeste()
        {
            _repositoryMock = new Mock<IAgendamentoRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _lojaRepositoryMock = new Mock<ILojaRepository>();
            _profissionalRepositoryMock = new Mock<IProfissionalRepository>();
            _servicoLojaRepositoryMock = new Mock<IServicoLojaRepository>();

            _service = new AgendamentoService(
                _repositoryMock.Object,
                _clienteRepositoryMock.Object,
                _profissionalRepositoryMock.Object,
                _servicoLojaRepositoryMock.Object,
                _lojaRepositoryMock.Object);
        }

        [Fact]
        public void ListarAgendamentos_DeveRetornarAgendamentosPreenchidos()
        {
            var agendamentosFakes = new List<Agendamento>
            {
                CriarAgendamentoValido()
            };

            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(agendamentosFakes);

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(1, resultado[0].ClienteId);
            Assert.Equal(1, resultado[0].ServicoLojaId);
            Assert.Equal(1, resultado[0].ProfissionalId);
            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        [Fact]
        public void ListarAgendamentos_NaoExistemAgendamentos()
        {
            _repositoryMock
                .Setup(r => r.Listar())
                .Returns(new List<Agendamento>());

            var resultado = _service.Listar();

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _repositoryMock.Verify(r => r.Listar(), Times.Once);
        }

        [Fact]
        public void BuscarPorIdAgendamento_DeveRetornarAgendamentoEncontrado()
        {
            var agendamento = CriarAgendamentoValido();

            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(agendamento);

            var resultado = _service.BuscarPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(1, resultado.ClienteId);
            Assert.Equal(1, resultado.ServicoLojaId);
            Assert.Equal(1, resultado.ProfissionalId);
            _repositoryMock.Verify(r => r.BuscarPorId(1), Times.Once);
        }

        [Fact]
        public void CadastrarAgendamento_DeveCadastrarAgendamentoValido()
        {
            var agendamento = CriarAgendamentoValido();
            ConfigurarDependenciasValidas();

            _repositoryMock
                .Setup(r => r.Cadastrar(agendamento))
                .Returns(true);

            var resultado = _service.Cadastrar(agendamento);

            Assert.True(resultado);
            Assert.Equal("PENDENTE", agendamento.Status);
            _repositoryMock.Verify(r => r.Cadastrar(It.Is<Agendamento>(a => a.Status == "PENDENTE")), Times.Once);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteDataNoPassado()
        {
            var agendamento = CriarAgendamentoValido();
            agendamento.DataAgendamento = DateTime.Now.AddMinutes(-5);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Não é possível agendar no passado.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteMinutoForaDoIntervaloDeCincoMinutos()
        {
            var agendamento = CriarAgendamentoValido();
            agendamento.DataAgendamento = ProximaData(10, 7);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Os horários devem seguir intervalos de 5 minutos.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteClienteInexistente()
        {
            var agendamento = CriarAgendamentoValido();

            _clienteRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ClienteId))
                .Returns((Cliente)null);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Cliente não encontrado.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteProfissionalInexistente()
        {
            var agendamento = CriarAgendamentoValido();

            _clienteRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ClienteId))
                .Returns(new Cliente { Id = agendamento.ClienteId });
            _profissionalRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ProfissionalId))
                .Returns((Profissional)null);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Profissional não encontrado.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteServicoInexistente()
        {
            var agendamento = CriarAgendamentoValido();

            _clienteRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ClienteId))
                .Returns(new Cliente { Id = agendamento.ClienteId });
            _profissionalRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ProfissionalId))
                .Returns(new Profissional { Id = agendamento.ProfissionalId, LojaId = 1 });
            _servicoLojaRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ServicoLojaId))
                .Returns((ServicoLoja)null);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Serviço não encontrado.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteLojaInexistente()
        {
            var agendamento = CriarAgendamentoValido();

            _clienteRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ClienteId))
                .Returns(new Cliente { Id = agendamento.ClienteId });
            _profissionalRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ProfissionalId))
                .Returns(new Profissional { Id = agendamento.ProfissionalId, LojaId = 1 });
            _servicoLojaRepositoryMock
                .Setup(r => r.BuscarPorId(agendamento.ServicoLojaId))
                .Returns(new ServicoLoja { Id = agendamento.ServicoLojaId, TempoMinutos = 30 });
            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns((Loja)null);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Loja não encontrada.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteHorarioAntesDaAbertura()
        {
            var agendamento = CriarAgendamentoValido();
            agendamento.DataAgendamento = ProximaData(7, 55);
            ConfigurarDependenciasValidas();

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Loja ainda está fechada.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteServicoTerminarAposFechamento()
        {
            var agendamento = CriarAgendamentoValido();
            agendamento.DataAgendamento = ProximaData(17, 45);
            ConfigurarDependenciasValidas();

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("O serviço termina após o fechamento.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_NaoPermiteConflitoDeHorarioParaMesmoProfissional()
        {
            var agendamento = CriarAgendamentoValido();
            var agendamentoExistente = new Agendamento
            {
                Id = 2,
                ClienteId = 2,
                ServicoLojaId = 2,
                ProfissionalId = agendamento.ProfissionalId,
                DataAgendamento = agendamento.DataAgendamento.AddMinutes(15),
                Status = "PENDENTE"
            };

            ConfigurarDependenciasValidas();
            _repositoryMock
                .Setup(r => r.ExisteConflito(
                    agendamento.ProfissionalId,
                    agendamento.DataAgendamento,
                    agendamento.DataAgendamento.AddMinutes(30),
                    null))
                .Returns(true);

            var excecao = Assert.Throws<Exception>(() => _service.Cadastrar(agendamento));

            Assert.Equal("Horário indisponível.", excecao.Message);
            _repositoryMock.Verify(r => r.Cadastrar(It.IsAny<Agendamento>()), Times.Never);
        }

        [Fact]
        public void CadastrarAgendamento_PermiteMesmoHorarioParaProfissionalDiferente()
        {
            var agendamento = CriarAgendamentoValido();
            var agendamentoExistente = new Agendamento
            {
                Id = 2,
                ClienteId = 2,
                ServicoLojaId = 2,
                ProfissionalId = 99,
                DataAgendamento = agendamento.DataAgendamento,
                Status = "PENDENTE"
            };

            ConfigurarDependenciasValidas();
            _repositoryMock
                .Setup(r => r.ExisteConflito(
                    agendamento.ProfissionalId,
                    agendamento.DataAgendamento,
                    agendamento.DataAgendamento.AddMinutes(30),
                    null))
                .Returns(false);
            _repositoryMock
                .Setup(r => r.Cadastrar(agendamento))
                .Returns(true);

            var resultado = _service.Cadastrar(agendamento);

            Assert.True(resultado);
            _repositoryMock.Verify(r => r.Cadastrar(agendamento), Times.Once);
        }

        [Fact]
        public void AtualizarAgendamento_DeveRepassarAgendamentoParaRepositorio()
        {
            var agendamento = CriarAgendamentoValido();
            ConfigurarDependenciasValidas();

            _service.Atualizar(agendamento);

            _repositoryMock.Verify(r => r.Atualizar(agendamento), Times.Once);
        }

        [Fact]
        public void RemoverAgendamento_DeveRemoverAgendamentoFuturo()
        {
            var agendamento = CriarAgendamentoValido();

            _repositoryMock
                .Setup(r => r.BuscarPorId(agendamento.Id))
                .Returns(agendamento);

            _service.Remover(agendamento.Id);

            _repositoryMock.Verify(r => r.Remover(agendamento.Id), Times.Once);
        }

        [Fact]
        public void RemoverAgendamento_NaoPermiteAgendamentoInexistente()
        {
            _repositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns((Agendamento)null);

            var excecao = Assert.Throws<Exception>(() => _service.Remover(1));

            Assert.Equal("Agendamento não encontrado.", excecao.Message);
            _repositoryMock.Verify(r => r.Remover(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RemoverAgendamento_NaoPermiteAgendamentoJaIniciado()
        {
            var agendamento = CriarAgendamentoValido();
            agendamento.DataAgendamento = DateTime.Now.AddMinutes(-1);

            _repositoryMock
                .Setup(r => r.BuscarPorId(agendamento.Id))
                .Returns(agendamento);

            var excecao = Assert.Throws<Exception>(() => _service.Remover(agendamento.Id));

            Assert.Equal("Não é possível cancelar um agendamento já iniciado.", excecao.Message);
            _repositoryMock.Verify(r => r.Remover(It.IsAny<int>()), Times.Never);
        }

        private void ConfigurarDependenciasValidas()
        {
            _clienteRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Cliente { Id = 1, Nome = "Cliente Teste" });
            _profissionalRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Profissional { Id = 1, LojaId = 1, Nome = "Profissional Teste" });
            _servicoLojaRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new ServicoLoja { Id = 1, LojaId = 1, ServicoId = 1, TempoMinutos = 30, Preco = 50, Ativo = true });
            _lojaRepositoryMock
                .Setup(r => r.BuscarPorId(1))
                .Returns(new Loja { Id = 1, Nome = "Loja Teste", HoraAbertura = new TimeSpan(8, 0, 0), HoraFechamento = new TimeSpan(18, 0, 0) });
            _repositoryMock
                .Setup(r => r.ExisteConflito(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int?>()))
                .Returns(false);
        }

        private static Agendamento CriarAgendamentoValido()
        {
            return new Agendamento
            {
                Id = 1,
                ClienteId = 1,
                ServicoLojaId = 1,
                ProfissionalId = 1,
                DataAgendamento = ProximaData(10, 0),
                Status = "PENDENTE"
            };
        }

        private static DateTime ProximaData(int hora, int minuto)
        {
            return DateTime.Now.Date.AddDays(1).AddHours(hora).AddMinutes(minuto);
        }
    }
}
