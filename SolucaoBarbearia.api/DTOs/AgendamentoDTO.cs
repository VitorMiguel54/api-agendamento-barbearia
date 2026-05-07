namespace SolucaoBarbearia.api.DTOs
{
    public class AgendamentoDTO
    {
        public int Id { get; set; }

        public string ClienteNome { get; set; }
        public string ServicoNome { get; set; }
        public string ProfissionalNome { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string Status { get; set; }
    }
}
