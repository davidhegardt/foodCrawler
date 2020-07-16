using FoodCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCrawler.Mappers
{
    public static class FoodItemSeeder
    {
        public static List<FoodItem> GetDummyFoodItems()
        {
            return new List<FoodItem>()
            {
               new FoodItem()
               {
                   ID = 0,
                   Image = "https://d2rfo6yapuixuu.cloudfront.net/h1b/h39/8856420352030/7340083402241.jpg_master_axfood_300",
                   Manufacturer = "Garant",
                   Name = "Halloumi",
                   Price = 19.9,
                   Store = "Willys",
                   Vikt = "200g"
               },
               new FoodItem()
               {
                   ID = 1,
                   Image = "https://d2rfo6yapuixuu.cloudfront.net/hd9/h63/8918366683166/07340083454837.jpg_1492290753434_master_axfood_300",
                   Manufacturer = "Garant",
                   Name = "Halloumi Burgare",
                   Price = 19.9,
                   Store = "Willys",
                   Vikt = "200g"
               },new FoodItem()
               {
                   ID = 2,
                   Image = "https://d2rfo6yapuixuu.cloudfront.net/hd9/h63/8918366683166/07340083454837.jpg_1492290753434_master_axfood_300",
                   Manufacturer = "Garant",
                   Name = CorrectTitle("Burger Slices Chili&amp;röd Paprika Cheddar"),
                   Price = 9.9,
                   Store = "Willys",
                   Vikt = "200g"
               },new FoodItem()
               {
                   ID = 3,
                   Image = "https://d2rfo6yapuixuu.cloudfront.net/h42/h4c/9688830541854/2359762100007_1574418375926_master_axfood_300",
                   Manufacturer = "Garant",
                   Name = "Parmigiano Reggiano 22månader",
                   Price = 269,
                   Store = "Willys",
                   Vikt = "ca: 500g"
               },new FoodItem()
               {
                   ID = 4,
                   Image = "https://d2rfo6yapuixuu.cloudfront.net/hef/heb/9439470813214/5711953031212_1555512080280_master_axfood_300",
                   Manufacturer = "Apetina",
                   Name = "Vitost Herbs&amp;spices 29%",
                   Price = 27.5,
                   Store = "Willys",
                   Vikt = "265g"
               },new FoodItem()
               {
                   ID = 5,
                   Image = "https://d2rfo6yapuixuu.cloudfront.net/h20/hdc/8866149728286/07310861081487.jpg_1465304734148_master_axfood_300",
                   Manufacturer = "Västerbottens",
                   Name = "Västerbotten Riven Ost",
                   Price = 34,
                   Store = "Willys",
                   Vikt = "150g"
               },
               new FoodItem()
               {
                   ID = 6,
                   Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//313406.jpg",
                   Manufacturer = "Västerbottensost",
                   Name = "Västerbottensost Magnum ",
                   Price = 185.9,
                   Store = "Coop",
                   Vikt = "1100"
               },
               new FoodItem()
               {
                   ID = 7,
                   Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//354164.jpg",
                   Manufacturer = "Fjällbrynt",
                   Name = "Skinkost",
                   Price = 31.95,
                   Store = "Coop",
                   Vikt = "250"
               },
               new FoodItem()
               {
                   ID = 8,
                   Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//302631.jpg",
                   Manufacturer = "Castello®",
                   Name = "White Truffle 38 %",
                   Price = 26.95,
                   Store = "Coop",
                   Vikt = "150"
               },
               new FoodItem()
               {
                   ID = 9,
                   Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//276471.jpg",
                   Manufacturer = "Allerum",
                   Name = "Präst Ekologisk KRAV 12 mån 35%",
                   Price = 111.3,
                   Store = "Coop",
                   Vikt = "700"
               },
               new FoodItem()
               {
                   ID = 10,
                   Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//367534.jpg",
                   Manufacturer = "Coop Online Deli",
                   Name = "Ostbricka 4 Ostar",
                   Price = 119,
                   Store = "Coop",
                   Vikt = "500"
               },
               new FoodItem()
               {
                   ID = 8,
                   Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//302631.jpg",
                   Manufacturer = "Castello®",
                   Name = "White Truffle 38 %",
                   Price = 26.95,
                   Store = "Coop",
                   Vikt = "150"
               }
            };
        }

        public static List<FoodItem> GetDummyMeat()
        {
            return new List<FoodItem>()
            {
                new FoodItem()
                {
                    ID = 0,
                    Image = "https://d2rfo6yapuixuu.cloudfront.net/h6e/h01/9112928190494/7340083462207_1523912159311_master_axfood_300",
                    Manufacturer = "Garant",
                    Name = "Kyckling Ben Frysta",
                    Price = 38,
                    Vikt = "1kg",
                    Store = "Willys"
                },
                new FoodItem()
                {
                    ID = 1,
                    Image = "https://d2rfo6yapuixuu.cloudfront.net/hc9/h18/9112928845854/7340083462221_1523912178437_master_axfood_300",
                    Manufacturer = "Garant",
                    Name = "Kyckling Vingar Frysta",
                    Price = 38,
                    Vikt = "1kg",
                    Store = "Willys"
                },
                new FoodItem()
                {
                    ID = 2,
                    Image = "https://d2rfo6yapuixuu.cloudfront.net/h05/h6b/9112929173534/7340083462238_1523912181903_master_axfood_300",
                    Manufacturer = "Garant",
                    Name = "Kyckling Lår Frysta",
                    Price = 38,
                    Vikt = "1kg",
                    Store = "Willys"
                },
                new FoodItem()
                {
                    ID = 3,
                    Image = "https://d2rfo6yapuixuu.cloudfront.net/hba/hd7/8962017591326/07340083455858.jpg_1503560149443_master_axfood_300",
                    Manufacturer = "Garant",
                    Name = "Kycklingfärs Naturell",
                    Price = 29.9,
                    Vikt = "500g",
                    Store = "Willys"
                },
                new FoodItem()
                {
                    ID = 4,
                    Image = "https://d2rfo6yapuixuu.cloudfront.net/h1b/hcf/9284136665118/7311041056387_1542634828938_master_axfood_300",
                    Manufacturer = "Garant",
                    Name = "Köttbullar Sverige",
                    Price = 14.9,
                    Vikt = "350g",
                    Store = "Willys"
                },
                new FoodItem()
                {
                    ID = 5,
                    Image = "https://d2rfo6yapuixuu.cloudfront.net/h1b/hcf/9284136665118/7311041056387_1542634828938_master_axfood_300",
                    Manufacturer = "Garant",
                    Name = "Köttbullar Sverige",
                    Price = 14.9,
                    Vikt = "350g",
                    Store = "Willys"
                },
                new FoodItem()
                {
                    ID = 6,
                    Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//392326.jpg",
                    Manufacturer = "Guldfågeln",
                    Name = "Kycklingfilé Naturell Tvådelad",
                    Price = 59.4,
                    Vikt = "700",
                    Store = "Coop"
                },
                new FoodItem()
                {
                    ID = 7,
                    Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//387243.jpg",
                    Manufacturer = "Coop",
                    Name = "Ribs BBQ",
                    Price = 49.95,
                    Vikt = "",
                    Store = "Coop"
                },
                new FoodItem()
                {
                    ID = 8,
                    Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//142110.jpg",
                    Manufacturer = "Coop",
                    Name = "Nötfärs 12 % - Sverige",
                    Price = 87.95,
                    Vikt = "",
                    Store = "Coop"
                },
                new FoodItem()
                {
                    ID = 9,
                    Image = "https://res.cloudinary.com/coopsverige/image/upload/d_cooponline:missingimage:missing-image.png,fl_progressive,q_90,c_lpad,w_120,h_120/q_auto,f_auto//392335.jpg",
                    Manufacturer = "Guldfågeln",
                    Name = "Kycklinglårfilé",
                    Price = 133.2,
                    Vikt = "900",
                    Store = "Coop"
                }
            };
        }

        public static string CorrectTitle(string input)
        {
            return input.Replace("&amp;", " & ");
        }
    }
}
