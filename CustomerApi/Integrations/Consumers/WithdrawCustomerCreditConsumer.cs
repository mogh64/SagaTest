using CustomerApi.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Integrations.Consumers
{
    public class WithdrawCustomerCreditConsumer : IConsumer<WithdrawCustomerCreditMessage>
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<WithdrawCustomerCreditConsumer> logger;

        public WithdrawCustomerCreditConsumer(ICustomerService customerService, ILogger<WithdrawCustomerCreditConsumer> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<WithdrawCustomerCreditMessage> context)
        {
            try
            {
                logger.LogInformation($"Customer {context.Message.CustomerId} Withdraw credit {context.Message.Credit}");
                await customerService.WithdrawCredit(context.Message.Credit, context.Message.CustomerId);
                await context.Publish<OrderAcceptedEvent>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderId = context.Message.OrderId 
                });
            }
            catch(Exception ex)
            {
                await context.Publish<OrderRejectedEvent>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderId = context.Message.OrderId,
                    Reason = ex.Message
                });
            }
        }
    }
}
