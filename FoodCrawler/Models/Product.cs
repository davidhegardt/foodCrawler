using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCrawler.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Manufacturer { get; set; }
        public string Weight { get; set; }
        public string ImageURL { get; set; }
        public string Unit { get; set; }
    }
}
