using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Models;

public class Agendamento
{
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public int ServicoLojaId { get; set; }
    public int ProfissionalId { get; set; }
    public DateTime DataAgendamento { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}