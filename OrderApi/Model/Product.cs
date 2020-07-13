using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Model
{
    public class Product
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
