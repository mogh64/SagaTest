using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Dtos
{
    public class SubmitOrderDto
    {
        public int OrderId { get; set; }
        public decimal Credit { get; set; }
    }
}
