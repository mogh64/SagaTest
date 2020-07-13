using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface OrderCompletedEvent
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}
