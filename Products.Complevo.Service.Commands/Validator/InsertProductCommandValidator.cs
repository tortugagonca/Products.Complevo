using FluentValidation;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Service.Commands.Products;

namespace Products.Complevo.Service.Commands.Validator
{
    public class InsertProductCommandValidator : AbstractValidator<InsertProductCommand>
    {
        private IProductRepository productRepository;
          
        public InsertProductCommandValidator(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The Product must have a name.")
                .WithName("Name");

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
        }

        private bool ProductMustBeUnique(InsertProductCommand command)
        {
            var product = productRepository.GetProductByNameAndCodeAsync(command.Name, command.ProductCode)
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();

            return product == null;
        }
    }
}
