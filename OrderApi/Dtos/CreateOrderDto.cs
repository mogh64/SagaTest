using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Dtos
{
    public class CreateOrderDto
    {
        public CreateOrderDto()
        {            
        }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }        
    }
}
