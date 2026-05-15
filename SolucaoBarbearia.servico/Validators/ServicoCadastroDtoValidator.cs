using System.Data;
using SolucaoBarbearia.servico.DTOs;
using FluentValidation;
using SolucaoBarbearia.dominio.Interfaces;

namespace Barbearia.Service.Validators;

public class ServicoCadastrarDtoValidator : AbstractValidator<ServicoCadastroDto>
{
    private readonly ILojaRepository _lojaRepository;
    public ServicoCadastrarDtoValidator(ILojaRepository lojaRepository)
    {
        _lojaRepository = lojaRepository;

        RuleFor(x => x.LojaId)
            .NotEmpty()
                .WithMessage("ID da Loja obrigatório!")
            .Must(ExisteLoja)
                .WithMessage("ID não localizado!");

        RuleFor(x => x.Nome)
            .NotEmpty()
                .WithMessage("Nome é obrigatório!")
            .MinimumLength(2)
                .WithMessage("Nome deve ter no mínimo 2 caracteres!")
            .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres!");

        RuleFor(x => x.Descricao)
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Descricao))
                .WithMessage("Descrição só pode conter letras, sem números ou caracteres especiais!");

        RuleFor(x => x.TempoMinutos)
            .NotEmpty()
                .WithMessage("Tempo/Duração obrigatório")
            .InclusiveBetween(31, 179)
                .WithMessage("Tempo de Serviço precisa ser MAIOR que 30 minutos e MENOR que 180 minutos!");

        RuleFor(x => x.Preco)
            .NotEmpty()
                .WithMessage("Preço obrigatório!")
            .InclusiveBetween(11, 999)
                .WithMessage("Preço do Serviço precisa ser MAIOR que R$10,00 e MENOR que R$1000,00!");
    }

    private bool ExisteLoja(int lojaId)
    {
        return _lojaRepository.Existe(lojaId);
    }
}