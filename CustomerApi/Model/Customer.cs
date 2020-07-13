using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal Credit { get; set; }
    }
}
