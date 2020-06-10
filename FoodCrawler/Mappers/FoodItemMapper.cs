using FoodCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCrawler.Mappers
{
    public static class FoodItemMapper
    {
        public static List<FoodItem> MapProductsToFoodItems(List<Product> prodList, string storeName, int startIndex = 0)
        {
            var foodItems = new List<FoodItem>();
            for(int i = 0; i < prodList.Count; i++)
            {
                foodItems.Add(MapProductToFoodItem(prodList[i], i + startIndex, storeName));
            }

            return foodItems;
        }

        private static FoodItem MapProductToFoodItem(Product product, int index, string storeName)
        {
            return new FoodItem()
            {
                ID = index,
                Name = product.Title,
                Price = product.Price,
                Store = storeName,
                Manufacturer = product.Manufacturer,
                Image = product.ImageURL,
                Vikt = product.Weight
            };
        }
    }
}
