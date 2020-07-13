using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Dtos
{
    public class ProductDto
    {        
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
