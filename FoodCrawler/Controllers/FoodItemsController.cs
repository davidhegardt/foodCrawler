using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodCrawler.Mappers;
using FoodCrawler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodCrawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        [HttpGet]
        [Route("Cheese")]
        public async Task<IEnumerable<FoodItem>> GetCheese()
        {
            //var test = await FoodService.GetStoreProducts("Willys");
            return FoodItemSeeder.GetDummyFoodItems();
            
            var willysProducts = await FoodService.GetStoreProducts(Store.Willys, Category.Cheese);
            var willysFoodItems = FoodItemMapper.MapProductsToFoodItems(willysProducts, "Willys");
            //return willysFoodItems;

            var coopProducts = await FoodService.GetStoreProducts(Store.Coop, Category.Cheese);
            var coopFoodItems = FoodItemMapper.MapProductsToFoodItems(coopProducts, "Coop", willysFoodItems.Count);

            var allFoodItems = new List<FoodItem>();

            allFoodItems.AddRange(willysFoodItems);
            allFoodItems.AddRange(coopFoodItems);

            return allFoodItems;

            //return willysFoodItems;
        }

        [HttpGet]
        [Route("Meat")]
        public async Task<IEnumerable<FoodItem>> GetMeat()
        {
            return FoodItemSeeder.GetDummyMeat();
            var willys = await FoodService.GetStoreProducts(Store.Coop, Category.Meat);
            var willysFoodItems = FoodItemMapper.MapProductsToFoodItems(willys, "Coop");

            return willysFoodItems;
        }

        [HttpGet]
        [Route("CityGross")]
        public async Task<IEnumerable<FoodItem>> GetCityGrossProducts()
        {
            var cityGrossProducts = await FoodService.GetCityGrossProducts();
            //var cityGrossProducts = new List<CityGrossProduct>()
            //{
            //    new CityGrossProduct()
            //    {
            //        ProductID = 0,
            //        Title = "Herrgårdsost 28%",
            //        Price = 74.95,
            //        Unit = "/kg",
            //        Weight = "ca 700g",
            //        Manufacturer = "Favorit",
            //        ImageURL = "https://www.citygross.se//images/products/02340388600002_C1N1-600.jpg"
            //    }
            //};

            var foodItems = FoodItemMapper.MapCityGrossProductsToFoodItems(cityGrossProducts,"CityGross");
            return foodItems;
        }
    }
}