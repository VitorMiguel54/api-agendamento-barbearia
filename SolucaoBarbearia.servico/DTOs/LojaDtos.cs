using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.servico.DTOs;

public record LojaDto(int Id, string Nome, TimeSpan HoraAbertura, TimeSpan HoraFechamento,
    DateTime DataCriacao);

public record LojaCadastroDto(string Nome, TimeSpan HoraAbertura, TimeSpan HoraFechamento);

