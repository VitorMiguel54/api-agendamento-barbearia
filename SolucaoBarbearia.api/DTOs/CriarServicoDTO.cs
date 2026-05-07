using System.ComponentModel.DataAnnotations;

namespace SolucaoBarbearia.api.DTOs
{
    public class CriarServicoDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Loja obrigatória")]
        public int LojaId { get; set; }

        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Tempo deve ser maior que 0")]
        public int TempoMinutos { get; set; }

        [Range(typeof(decimal), "0.01", "999999", ErrorMessage = "Preço deve ser maior que 0")]
        public decimal Preco { get; set; }
    }
}
