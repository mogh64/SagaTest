using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Messages
{
    public interface ChangeCustomerCreditResponse
    {
        decimal TotalCredit { get; }
    }
}
