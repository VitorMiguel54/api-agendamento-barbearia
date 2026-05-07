namespace Dominio.Models
{
    public class Profissional
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
    }
}