using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface TakeProductMessage
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
    
}
