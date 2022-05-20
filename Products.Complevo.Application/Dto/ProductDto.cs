using System.ComponentModel.DataAnnotations;

namespace Products.Complevo.Application.Core.Dto
{
    /// <summary>
    /// Contract of product
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Product code
        /// </summary>
        [Required]
        public uint ProductCode { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Product Category
        /// </summary>
        [Required]
        public string Category { get; set; }
        /// <summary>
        /// Product price
        /// </summary>
        [Required]
        public double Price { get; set; }
    }
}
