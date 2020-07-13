using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface OrderItemAddedEvent
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
