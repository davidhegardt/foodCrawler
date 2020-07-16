using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCrawler.Models
{
    public class CityGrossProduct : Product
    {
        public SpecialPrice Specialprice { get; set; }
    }

    public class SpecialPrice
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Extrapris { get; set; }
        public double OrdinaryPrice { get; set; }
    }
}
