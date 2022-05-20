using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Service.Commands.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 
namespace Products.Complevo.Application.Controllers.V0
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Insert a Product on database
        /// </summary>
        /// <param name="product">Product to be inserted</param>
        /// <returns>Id of the product inserted</returns>
        /// <response code="200">Sucessfull insertion of a product.</response>
        /// <response code="400">Invalid parameters</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto product)

            => Ok(await _mediator.Send(new InsertProductCommand(
                product.ProductCode,
                product.Name,
                product.Category,
                product.Price
                )));

        /// <summary>
        /// Update a Product on database
        /// </summary>
        /// <param name="id">Id of the product to be updated</param>
        /// <param name="product">Product to be updated</param> 
        /// <response code="200">Sucessfull update of product.</response>
        /// <response code="400">Invalid parameters</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ProductDto product)

            => Ok(await _mediator.Send(new UpdateProductCommand(
                id,
                product.ProductCode,
                product.Name,
                product.Category,
                product.Price
                )));

        /// <summary>
        /// Delete product from database
        /// </summary>
        /// <param name="id">Id of product to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Sucessfull deletion of product</response>
        /// <response code="400">Invalid operation</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _mediator.Send(new DeleteProductCommand(id)));
    }
}
