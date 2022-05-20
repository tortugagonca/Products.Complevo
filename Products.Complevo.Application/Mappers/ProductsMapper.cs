using AutoMapper;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Domain.Models;

namespace Products.Complevo.Application.Core.Mappers
{
    public class ProductsMapper : Profile
    {
        public ProductsMapper()
        {
            CreateMap<ProductDto, Product>();
        } 
    }
}
