using AutoMapper;
using FluentValidation;
using MediatR;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Application.Core.Exceptions;
using Products.Complevo.Application.Core.Extensions;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Application.Core.Resources;
using Products.Complevo.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Products.Complevo.Service.Commands.Products
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public readonly Guid Id;

        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IValidator<DeleteProductCommand> _validator;
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IValidator<DeleteProductCommand> validator, IProductRepository repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            validationResult.ThrowExceptionIfNotValid();
            var product = await _repository.GetByIdAsync(request.Id);
            var countOfProductsDeleted = await _repository.DeleteAsync(product);
            if (countOfProductsDeleted == 0)
            {
                throw new ProductsComplevoValidationException(ProductsComplevoResource.NoRecordsDeleted);
            }
            return Unit.Value;
        }
    }
}
