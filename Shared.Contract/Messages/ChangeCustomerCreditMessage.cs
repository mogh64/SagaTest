using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface ChangeCustomerCreditMessage
    {
        int CostomerId { get; }
        decimal Credit { get; }
    }
}
