using System.Data;
using SolucaoBarbearia.servico.DTOs;
using FluentValidation;

namespace Barbearia.Service.Validators;

public class ClienteCadastroDtoValidator : AbstractValidator<ClienteCadastroDto>
{
    public ClienteCadastroDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
                .WithMessage("Nome é obrigatório!")
            .MinimumLength(2)
                .WithMessage("Nome deve ter no mínimo 2 caracteres!")
            .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres!");

        RuleFor(x => x.Telefone)
            .NotEmpty()
                .WithMessage("Telefone é obrigatório!")
            .Matches(@"^\d{10,11}$")
                .WithMessage("Telefone deve conter apenas dígitos (10 para fixo, 11 para celular).");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("E-mail é obrigatório!")
            .EmailAddress()
                .WithMessage("E-mail inválido!")
            .MaximumLength(150)
                .WithMessage("E-mail deve ter no máximo 150 caracteres!");
    }
}