using Shared.Contract.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public  interface OrderAcceptedEvent
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}
