using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public interface ICustomerService
    {
        Task AddCredit(decimal credit, int customerId);
        Task WithdrawCredit(decimal credit, int customerId);
    }
}
