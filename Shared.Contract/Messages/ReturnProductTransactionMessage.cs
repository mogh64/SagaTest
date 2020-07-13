using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface ReturnProductTransactionMessage
    {
        IList<ProductBasket> ProductBaskets { get; }
    }
}
