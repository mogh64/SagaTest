using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Events
{
    public interface ProductUpdatedEvent
    {
        int ProductId { get; }
        int Count { get; }
        decimal Price { get; }
    }
    public interface ProductsUpdatedEvent
    {
        IList<ProductUpdatedEvent> ProductUpdatedEvents {get ;}
    }
}
