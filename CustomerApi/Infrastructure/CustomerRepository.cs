using CustomerApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly CustomerDbContext dbContext;

        public CustomerRepository(CustomerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            var customer = await this.dbContext.Customers.FindAsync(id);
            this.dbContext.Customers.Remove(customer);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Customer> Get(int id)
        {
            return await this.dbContext.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            return await this.dbContext.Customers.ToListAsync();
        }

        public async Task<Customer> Insert(Customer customer)
        {
            this.dbContext.Customers.Add(customer);
            await this.dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task Update(Customer customer)
        {
            this.dbContext.Customers.Update(customer);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
