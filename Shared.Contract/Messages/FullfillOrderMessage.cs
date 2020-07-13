using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface FullfillOrderMessage
    {
        int OrderId { get; }
        decimal Credit { get; }
        int CustomerId { get; }
    }
}
