namespace SolucaoBarbearia.api.DTOs
{
    public class ServicoDTO
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int TempoMinutos { get; set; }
        public decimal Preco { get; set; }
    }
}
