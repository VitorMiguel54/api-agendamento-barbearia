namespace Dominio.Models
{
    public class Loja
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TimeSpan HoraAbertura { get; set; }
        public TimeSpan HoraFechamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
    }
}