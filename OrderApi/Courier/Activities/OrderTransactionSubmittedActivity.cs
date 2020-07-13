using Automatonymous;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderApi.Saga.SubmitOrderSagas;
using OrderApi.Saga.SubmitOrderTransactionalSaga;
using Shared.Contract.Constants;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Courier.Activities
{
    public class OrderTransactionSubmittedActivity : 
        Activity<OrderTransactionState, OrderTransactionSubmittedEvent>
    {
        private readonly ILogger<OrderTransactionSubmittedActivity> logger;

        public OrderTransactionSubmittedActivity(ILogger<OrderTransactionSubmittedActivity> logger)
        {
            this.logger = logger;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async  Task Execute(BehaviorContext<OrderTransactionState, OrderTransactionSubmittedEvent> context, Behavior<OrderTransactionState, OrderTransactionSubmittedEvent> next)
        {
            var sendEndpoint = await context.GetSendEndpoint(QueueNames.GetMessageUri(nameof(FullfillOrderMessage)));
            logger.LogInformation($"Order Transaction activity for sendEndpoint {sendEndpoint} will be called");
            await sendEndpoint.Send<FullfillOrderMessage>(new
            {
                OrderId = context.Data.OrderId,
                Credit = context.Data.Credit,
                CustomerId = context.Data.CustomerId
            });             
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderTransactionState, OrderTransactionSubmittedEvent, TException> context, Behavior<OrderTransactionState, OrderTransactionSubmittedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submit-order");
        }
    }
}
