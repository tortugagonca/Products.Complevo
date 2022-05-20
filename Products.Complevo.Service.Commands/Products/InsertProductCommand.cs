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
    public class InsertProductCommand : ProductDto, IRequest<Guid>
    {
        public InsertProductCommand(uint productCode, string name, string category, double price)
        {
            ProductCode = productCode;
            Name = name;
            Category = category;
            Price = price;
        }
    }
    public class InsertProductHandler : IRequestHandler<InsertProductCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<InsertProductCommand> _validator;
        private readonly IProductRepository _repository;

        public InsertProductHandler(
            IMapper mapper, 
            IValidator<InsertProductCommand> validator, 
            IProductRepository repository)
        {
            _mapper = mapper;
            _validator = validator;
            _repository = repository;
        }

        public async Task<Guid> Handle(InsertProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            validationResult.ThrowExceptionIfNotValid();

            var product = _mapper.Map<Product>(request);

            var entityPersisted = await _repository.InsertAsync(product);
            if (entityPersisted == null)
            {
                throw new ProductsComplevoValidationException(ProductsComplevoResource.NoProductGenerated);
            }

            var countOfRegistered= await _repository.SaveChangesAsync();
            if (countOfRegistered == 0)
            {
                throw new ProductsComplevoValidationException(ProductsComplevoResource.NoRecordsPersisted);
            }
            return entityPersisted.Id;
        }
    }
}
