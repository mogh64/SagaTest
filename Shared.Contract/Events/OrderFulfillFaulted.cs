using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface OrderFulfillFaulted
    {
        int OrderId { get; }
    }
}
