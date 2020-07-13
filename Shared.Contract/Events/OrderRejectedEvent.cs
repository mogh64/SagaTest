using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface OrderRejectedEvent
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
        string Reason { get; }
    }
}
