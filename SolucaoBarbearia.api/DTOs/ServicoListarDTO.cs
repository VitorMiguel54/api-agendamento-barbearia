namespace SolucaoBarbearia.api.DTOs
{
    public class ServicoListarDTO
    {
        public int ServicoLojaId { get; set; }

        public int ServicoId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public int TempoMinutos { get; set; }
    }
}
