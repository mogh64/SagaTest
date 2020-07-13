using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Dtos
{
    public class WithdrawCreditDto
    {
        public int CustomerId { get; set; }
        public decimal Credit { get; set; }
    }
}
