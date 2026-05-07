using System.ComponentModel.DataAnnotations;

namespace SolucaoBarbearia.api.DTOs
{
    public class CriarAgendamentoDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID do Cliente deve ser maior que 0")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID do Serviço deve ser maior que 0")]
        public int ServicoLojaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID do Profissional deve ser maior que 0")]
        public int ProfissionalId { get; set; }

        
        public DateTime DataAgendamento { get; set; }
    }
}
