using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface OrderSubmittedEvent
    {
        int OrderId { get; }
        Guid CorrelationId { get; }
        decimal Credit { get; }
        int CustomerId { get; }
    }
}
