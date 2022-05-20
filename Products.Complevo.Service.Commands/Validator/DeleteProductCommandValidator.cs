using FluentValidation;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Service.Commands.Products;

namespace Products.Complevo.Service.Commands.Validator
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        private IProductRepository _productRepository;

        public DeleteProductCommandValidator(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
            RuleFor(x => x)
                .Must((command) => ProductMustExists(command))
                .WithMessage("The product must exists on database.");
        }

        private bool ProductMustExists(DeleteProductCommand command)
        {
            var product = _productRepository.GetByIdAsync(command.Id)
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();

            return product != null;
        }
    }
}
