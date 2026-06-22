namespace SolucaoBarbearia.api.DTOs
{
    public class AgendamentoDTO
    {
        public int Id { get; set; }

        public string ClienteNome { get; set; } = string.Empty;
        public string ServicoNome { get; set; } = string.Empty;
        public string ProfissionalNome { get; set; } = string.Empty;
        public DateTime DataAgendamento { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
