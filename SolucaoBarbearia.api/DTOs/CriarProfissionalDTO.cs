using System.ComponentModel.DataAnnotations;

namespace SolucaoBarbearia.api.DTOs
{
    public class CriarProfissionalDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID da Loja deve ser maior que 0")]
        public int LojaId { get; set; }

        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Nome { get; set; } = string.Empty;
    }
}
