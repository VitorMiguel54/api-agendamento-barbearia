namespace Dominio.Models
{
    public class Servico
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TempoMinutos { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
    }
}
