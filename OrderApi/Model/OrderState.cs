using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Model
{
    public enum OrderState
    {
        Created,
        Submitted,
        Rejected,
        Closed
    }
}
