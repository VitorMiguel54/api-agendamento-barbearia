using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.servico.DTOs;

public record ServicoLojaDto(int Id, int LojaId, int ServicoId, decimal Preco, int TempoMinutos);

public record ServicoLojaCadastroDto(int LojaId, int ServicoId, decimal Preco, int TempoMinutos);
