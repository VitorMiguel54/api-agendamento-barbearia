using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.servico.DTOs;

public record ProfissionalDto(int Id, int LojaId, string Nome, DateTime DataCriacao);

public record ProfissionalCadastroDto(int LojaId, string Nome);

