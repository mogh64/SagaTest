using CustomerApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
{
    public interface ICustomerRepository
    {
        Task<Customer> Get(int id);
        Task<IEnumerable<Customer>> Get();    
        Task<Customer> Insert(Customer customer);
        Task Delete(int id);
        Task Update(Customer customer);
    }
}
