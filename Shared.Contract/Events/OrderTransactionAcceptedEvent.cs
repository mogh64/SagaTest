using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface OrderTransactionAcceptedEvent
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}
