namespace SolucaoBarbearia.api.DTOs
{
    public class LojaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TimeSpan HoraAbertura { get; set; }
        public TimeSpan HoraFechamento { get; set; }
    }
}