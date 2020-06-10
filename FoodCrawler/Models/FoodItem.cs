using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCrawler.Models
{
    public class FoodItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Store { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public string Vikt { get; set; }
        public string Unit { get; set; }
    }
}
