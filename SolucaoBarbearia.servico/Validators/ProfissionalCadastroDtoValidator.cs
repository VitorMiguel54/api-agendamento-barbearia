using System.Data;
using SolucaoBarbearia.servico.DTOs;
using FluentValidation;
using SolucaoBarbearia.dominio.Interfaces;

namespace Barbearia.Service.Validators;

public class ProfissionalCadastroDtoValidator : AbstractValidator<ProfissionalCadastroDto>
{
    private readonly ILojaRepository _lojaRepository;
    public ProfissionalCadastroDtoValidator(ILojaRepository lojaRepository)
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
    }

    private bool ExisteLoja(int lojaId)
    {
        return _lojaRepository.Existe(lojaId);
    }
}