using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Services;
using Shared.Contract.Events;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IPublishEndpoint publishEndpoint;

        public ProductController(IProductService productService, IPublishEndpoint publishEndpoint)
        {
            this.productService = productService;
            this.publishEndpoint = publishEndpoint;
        }
        public async Task<string> Get()
        {
            return "product";
        }
        [HttpPost("CreateProduct")]
        public async Task<int> CreateProduct([FromBody]ProductDto product)
        {
            var productId = await productService.CreateProduct(new Model.Product()
            {
                Title = product.Title,
                Price = product.Price,
                Count = product.Count
            });
            await publishEndpoint.Publish<ProductCreatedEvent>(new
            {
                ProductId = productId,
                product.Count,
                product.Price
            });
            return productId;
        }
        [HttpPost("AddProduct")]
        public async Task<int> AddProduct([FromBody] AddProductDto productDto)
        {
            var product = await productService.AddProduct(productDto.Id, productDto.Count);
            await publishEndpoint.Publish<ProductUpdatedEvent>(new
            {
                ProductId = productDto.Id,
                product.Count,
                product.Price
            });
            return product.Count;
        }
    }
}
