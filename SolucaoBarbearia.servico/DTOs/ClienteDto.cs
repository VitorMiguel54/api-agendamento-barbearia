using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoBarbearia.servico.DTOs;

public record ClienteDto(int Id, string Nome, string Telefone, string Email, DateTime DataCriacao);

public record ClienteCadastroDto(string Nome, string Telefone, string Email);

