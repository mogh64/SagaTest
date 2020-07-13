using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface TakeProductTransactionMessage
    {
        IList<ProductBasket> ProductBaskets { get; }
    }
    public interface ProductBasket
    {
        int ProductId { get; }
        int Count { get; }
    }
}
