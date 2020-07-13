using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface WithdrawCustomerCreditMessage
    {
        Guid CorrelationId { get; }
        decimal Credit { get; }
        int CustomerId { get;}
        int OrderId { get; }
    }
}
