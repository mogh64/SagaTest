using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface OrderTransactionCompleted
    {
        int OrderId { get; }
    }
}
