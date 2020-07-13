using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface ProductCreatedEvent
    {
        int ProductId { get;  }
        int Count { get; }
        decimal Price { get; }
    }
}
