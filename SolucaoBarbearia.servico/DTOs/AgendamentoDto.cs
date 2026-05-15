using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.servico.DTOs;

public record AgendamentoDto(int Id, int ClienteId, int ServicoLojaId, int ProfissionalId, 
    DateTime DataAgendamento, DateTime DataCriacao);

public record AgendamentoCadastroDto(int ClienteId, int ServicoLojaId, int ProfissionalId,
    DateTime DataAgendamento);

