using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface ReturnCustomerCreditMessage
    {
        int CustomerId { get; }
        decimal Credit { get; }
    }
}
