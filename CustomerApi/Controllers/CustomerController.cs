using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Dtos;
using CustomerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        public async Task<string> Get()
        {
            return "customer";
        }
        [HttpPost("AddCredit")]
        public  Task AddCustomerCredit([FromBody] AddCreditDto addCreditDto)
        {

            return customerService.AddCredit(addCreditDto.Credit, addCreditDto.CustomerId);
        }
        [HttpPost("WithdrawCredit")]
        public Task WithdrawCredit([FromBody] WithdrawCreditDto withdrawCreditDto)
        {

            return customerService.WithdrawCredit(withdrawCreditDto.Credit, withdrawCreditDto.CustomerId);
        }
    }
}
