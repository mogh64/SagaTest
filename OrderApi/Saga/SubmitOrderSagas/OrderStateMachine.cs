using Automatonymous;
using Microsoft.Extensions.Logging;
using Shared.Contract.Constants;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System.Threading.Tasks;

namespace OrderApi.Saga.SubmitOrderSagas
{
    public class OrderStateMachine :
        MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            this.InstanceState(x => x.CurrentState);
            this.ConfigureCorrelationIds();
            Initially(
                When(OrderSubmitted)
                .Then(x => x.Instance.OrderId = x.Data.OrderId)
                .Then(x => logger.LogInformation($"Order {x.Instance.OrderId} submitted"))
                .ThenAsync(c => WithdrawCustomerCreditCommand(c))
                .TransitionTo(Submitted)
                );
            During(Submitted,
                 When(OrderAccepted)
                 .Then(x => logger.LogInformation($"Order {x.Instance.OrderId} accepted"))
                 .ThenAsync(c => TakeProductCommand(c))
                 .TransitionTo(Accepted));
            DuringAny(
                   When(OrderRejected)
                   .Then(x => logger.LogInformation($"Order {x.Instance.OrderId} rejected! because {x.Data.Reason}"))
                   .TransitionTo(Rejected)
                   .Finalize());
            During(Accepted,
                  When(OrderCompleted)
                  .Then(x => logger.LogInformation($"Order {x.Instance.OrderId} completed"))
                  .TransitionTo(Completed)
                  .Finalize());

        }

        private async Task TakeProductCommand(BehaviorContext<OrderState, OrderAcceptedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(TakeProductMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<TakeProductMessage>(new
            {
                CorrelationId = context.Data.CorrelationId,
                OrderId = context.Data.OrderId
            });
        }

        private async Task WithdrawCustomerCreditCommand(BehaviorContext<OrderState, OrderSubmittedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(WithdrawCustomerCreditMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<WithdrawCustomerCreditMessage>(new
            {
                CorrelationId = context.Data.CorrelationId,
                Credit = context.Data.Credit,
                CustomerId = context.Data.CustomerId,
                OrderId = context.Data.OrderId
            });
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(x => x.Message.CorrelationId)
                   .SelectId(c => c.Message.CorrelationId));
            Event(() => OrderAccepted, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OrderRejected, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OrderCompleted, x => x.CorrelateById(x => x.Message.CorrelationId));
        }
        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        public State Rejected { get; private set; }
        public State Completed { get; private set; }

        public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; }
        public Event<OrderAcceptedEvent> OrderAccepted { get; private set; }
        public Event<OrderRejectedEvent> OrderRejected { get; private set; }
        public Event<OrderCompletedEvent> OrderCompleted { get; private set; }

    }
}
