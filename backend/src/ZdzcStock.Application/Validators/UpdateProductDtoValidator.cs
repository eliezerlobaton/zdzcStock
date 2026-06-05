using FluentValidation;
using ZdzcStock.Application.DTOs;

namespace ZdzcStock.Application.Validators;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O campo Nome é obrigatório.")
            .MinimumLength(5)
            .WithMessage("O campo Nome deve ter no mínimo 5 caracteres.")
            .MaximumLength(100)
            .WithMessage("O campo Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("O preço deve ser maior que zero.");

        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("A Categoria é obrigatória.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("O campo Descrição deve ter no máximo 500 caracteres.");
    }
}
