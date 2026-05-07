using System.ComponentModel.DataAnnotations;

namespace SolucaoBarbearia.api.DTOs
{
    public class CriarClienteDTO
    {
        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}
