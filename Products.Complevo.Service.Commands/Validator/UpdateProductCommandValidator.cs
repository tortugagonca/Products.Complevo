using FluentValidation;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Service.Commands.Products;

namespace Products.Complevo.Service.Commands.Validator
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private IProductRepository _productRepository;

        public UpdateProductCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The Product must have a name.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("The Product must have a positive price.");

            RuleFor(x => x.ProductCode)
                .GreaterThan((uint)0)
                .WithMessage("The product code mmust informed.");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("The Product must be categorized");

            RuleFor(x => x)
                .Must((command) => ProductMustBeUnique(command))
                .WithMessage("The product must have unique name and code.");

            RuleFor(x => x)
                .Must((command) => ProductMustExists(command))
                .WithMessage("The product must exists on database.");
        }

        private bool ProductMustBeUnique(UpdateProductCommand command)
        {
            var product = _productRepository.GetProductByNameAndCodeAsync(command.Name, command.ProductCode)
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();

            return product == null || product.Id == command.Id;
        }

        private bool ProductMustExists(UpdateProductCommand command)
        {
            var product = _productRepository.GetByIdAsync(command.Id)
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();

            return product != null;
        }
    }
}
