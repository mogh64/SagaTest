using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Saga.SubmitOrderSagas
{
    public class OrderState :
        SagaStateMachineInstance
    {
        public Guid CorrelationId { get ; set ; }
        public State CurrentState { get; set; }
        public int OrderId { get; set; }
    }
}
