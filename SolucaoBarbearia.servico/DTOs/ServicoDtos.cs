using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.servico.DTOs;

public record ServicoDto(int Id, int LojaId, string Nome, string Descricao, int TempoMinutos,
    decimal Preco, DateTime DataCriacao);

public record ServicoCadastroDto(int LojaId, string Nome, string Descricao, int TempoMinutos,
    decimal Preco);