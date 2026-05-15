using System.Data;
using SolucaoBarbearia.servico.DTOs;
using FluentValidation;
using SolucaoBarbearia.dominio.Interfaces;
using SolucaoBarbearia.infra.Repositorios;
using Microsoft.Data.SqlClient;

namespace Barbearia.Service.Validators;

public class LojaCadastroDtoValidator : AbstractValidator<LojaCadastroDto>
{
    public LojaCadastroDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
                .WithMessage("Nome é obrigatório!")
            .MinimumLength(2)
                .WithMessage("Nome deve ter no mínimo 2 caracteres!")
            .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres!");

        RuleFor(x => x.HoraAbertura)
            .NotEmpty()
                .WithMessage("Hora abertura é obrigatório!");

        RuleFor(x => x.HoraFechamento)
            .NotEmpty()
                .WithMessage("Hora Fechamento é obrigatório!")
            .GreaterThan(x => x.HoraAbertura)
                .WithMessage("Horário de Fechamento precisa ser MAIOR que horário de abertura!");
    }
}