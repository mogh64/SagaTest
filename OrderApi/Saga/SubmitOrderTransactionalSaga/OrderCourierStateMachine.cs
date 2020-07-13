using Automatonymous;
using Microsoft.Extensions.Logging;
using OrderApi.Courier.Activities;
using OrderApi.Saga.SubmitOrderSagas;
using Shared.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Saga.SubmitOrderTransactionalSaga
{
    public class OrderCourierStateMachine:
        MassTransitStateMachine<OrderTransactionState>
    {
        private readonly ILogger<OrderCourierStateMachine> logger;

        public OrderCourierStateMachine(ILogger<OrderCourierStateMachine> logger)
        {
            this.logger = logger;
            this.InstanceState(x => x.CurrentState);
            this.ConfigureCorrelationIds();
            Initially(
               When(OrderSubmitted)
               .Then(x => x.Instance.OrderId = x.Data.OrderId)
               .Then(x => logger.LogInformation($"Order Transaction {x.Instance.OrderId} submitted"))
               .Activity(c =>c.OfType<OrderTransactionSubmittedActivity>())
               .TransitionTo(Submitted)
               );
        }
        private void ConfigureCorrelationIds()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(x => x.Message.CorrelationId)
                   .SelectId(c => c.Message.CorrelationId));

        }
        public State Submitted { get; private set; }       

        public Event<OrderTransactionSubmittedEvent> OrderSubmitted { get; private set; }

    }
}
