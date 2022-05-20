using FluentValidation;
using MediatR;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Application.Core.Exceptions;
using Products.Complevo.Application.Core.Extensions;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Application.Core.Resources;
using Products.Complevo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Products.Complevo.Service.Commands.Products
{
    public class UpdateProductCommand : ProductDto, IRequest<Unit>
    {
        public Guid Id { get; set; }
        public UpdateProductCommand(Guid id, uint productCode, string name, string category, double price)
        {
            Id = id;
            ProductCode = productCode;
            Name = name;
            Category = category;
            Price = price;
        }
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
    { 
        private readonly IValidator<UpdateProductCommand> _validator;
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IValidator<UpdateProductCommand> validator, IProductRepository repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            validationResult.ThrowExceptionIfNotValid();

            var product = await _repository.GetByIdWithTrackingAsync(request.Id);

            UpdateProductProperties(request, product);

            var registerAffected = await _repository.SaveChangesAsync();
            if (registerAffected == 0)
            {
                throw new ProductsComplevoValidationException(ProductsComplevoResource.NoRecordsPersisted);
            }
            return Unit.Value;
        }

        private void UpdateProductProperties(UpdateProductCommand request, Product product)
        {
            product.Name = request.Name;
            product.ProductCode = request.ProductCode;
            product.Price = request.Price;
            product.Category = request.Category;
        }
    }
}
