using FluentValidation;
using ZdzcStock.Application.DTOs;

namespace ZdzcStock.Application.Validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O campo Nome é obrigatório.")
            .MinimumLength(5)
            .WithMessage("O campo Nome deve ter no mínimo 5 caracteres.")
            .MaximumLength(100)
            .WithMessage("O campo Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("O campo Descrição deve ter no máximo 500 caracteres.");
    }
}
