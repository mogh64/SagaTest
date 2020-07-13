using CustomerApi.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Integrations.Consumers
{
    public class ReturnCustomerCreditConsumer : IConsumer<ReturnCustomerCreditMessage>
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<ReturnCustomerCreditConsumer> logger;

        public ReturnCustomerCreditConsumer(ICustomerService customerService,
                                             ILogger<ReturnCustomerCreditConsumer> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ReturnCustomerCreditMessage> context)
        {            
            await customerService.AddCredit(context.Message.Credit, context.Message.CustomerId);
           logger.LogInformation($"Customer {context.Message.CustomerId} retuned credit {context.Message.Credit}");
        }
    }
}
