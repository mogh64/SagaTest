using MassTransit.Courier.Exceptions;
using ProductApi.Infrastructure;
using ProductApi.Model;
using Shared.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext dbContext;

        public ProductService(ProductDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> AddProduct(int id, int count)
        {
            var product =  dbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                throw new Exception("Product Not Found!");
            }
            product.Count += count;
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<int> CreateProduct(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return product.Id;
        }

        public Task ReturnProducts(Dictionary<int, int> productCounts)
        {
            foreach (var productCount in productCounts)
            {
                var product = dbContext.Products.FirstOrDefault(x => x.Id == productCount.Key);
                if (product.Count < productCount.Value)
                {
                    throw new Exception("Product Count is Exeeded the maximum existance!");
                }
                product.Count += productCount.Value;
            }
            return dbContext.SaveChangesAsync();
        }

        public  Task TakeProduct(int count, int productId)
        {
            var product = dbContext.Products.FirstOrDefault(x => x.Id == productId);
            if (product.Count < count)
            {
                throw new Exception("Product Count is Exeeded the maximum existance!");
            }
            product.Count -= count;
            return dbContext.SaveChangesAsync();
        }

        public async Task<IList<Product>> TakeProducts(Dictionary<int, int> productCounts)
        {
            //if (productCounts.Count > 0)
            //{
            //    throw new RoutingSlipException("Product Failed!");
            //}
            IList<Product> products = new List<Product>();
            foreach (var productCount in productCounts)
            {
                var product = dbContext.Products.FirstOrDefault(x => x.Id == productCount.Key);
                if (product.Count < productCount.Value)
                {
                    throw new Exception("Product Count is Exeeded the maximum existance!");
                }
                product.Count -= productCount.Value;
                products.Add(product);
            }
            await dbContext.SaveChangesAsync();
            return products;
        }
    }
}
