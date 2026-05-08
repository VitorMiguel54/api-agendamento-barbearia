namespace Dominio.Models
{
    public class ServicoLoja
    {
        public int Id { get; set; }

        public int LojaId { get; set; }

        public int ServicoId { get; set; }

        public decimal Preco { get; set; }

        public int TempoMinutos { get; set; }

        public bool Ativo { get; set; }
    }
}