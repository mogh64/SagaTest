using CustomerApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
{
    public class CustomerDbContext:DbContext
    {
        public CustomerDbContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
        { }
        public DbSet<Customer> Customers { get; set; }
    }
}
