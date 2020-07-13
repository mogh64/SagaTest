using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using Shared.Contract.Constants;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Courier.Activities
{
    public class PaymentActivity : IActivity<PaymentArgument, PaymentLog>
    {
        private readonly ILogger<PaymentActivity> logger;

        public PaymentActivity(ILogger<PaymentActivity> logger)
        {
            this.logger = logger;
        }
        public async Task<CompensationResult> Compensate(CompensateContext<PaymentLog> context)
        {
            logger.LogInformation($"Payment copmensated Courier called for customer {context.Log.CustomerId}");
            var uri = QueueNames.GetMessageUri(nameof(ReturnCustomerCreditMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<ReturnCustomerCreditMessage>(new
            {
                Credit = context.Log.Credit,
                CustomerId = context.Log.CustomerId                
            });
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<PaymentArgument> context)
        {
            logger.LogInformation($"Payment Courier called for order {context.Arguments.OrderId}");
            var uri = QueueNames.GetMessageUri(nameof(WithdrawCustomerCreditMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<WithdrawCustomerCreditMessage>(new
            {                
                Credit = context.Arguments.Credit,
                CustomerId = context.Arguments.CustomerId,
                OrderId = context.Arguments.OrderId
            });
            return context.Completed(new { CustomerId  = context.Arguments.CustomerId, Credit = context.Arguments.Credit });
        }
    }
    public interface PaymentArgument
    {
        int OrderId { get; }
        int CustomerId { get; }
        decimal Credit { get; }
    }
    public interface PaymentLog
    {
        int CustomerId { get; }
        decimal Credit { get; }
    }
}
