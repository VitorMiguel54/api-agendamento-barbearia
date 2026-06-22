using System.ComponentModel.DataAnnotations;

namespace SolucaoBarbearia.api.DTOs
{
    public class CriarLojaDTO
    {
        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Horário de abertura obrigatório!")]
        public TimeSpan HoraAbertura { get; set; }

        [Required(ErrorMessage = "Horário de Fechamento obrigatório!")]
        public TimeSpan HoraFechamento { get; set; }
    }
}
