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
        public async Task<IEnumerable<FoodItem>> Get()

        {
            //var test = await FoodService.GetStoreProducts("Willys");
            
            var willysProducts = await FoodService.GetStoreProducts("Willys");
            var willysFoodItems = FoodItemMapper.MapProductsToFoodItems(willysProducts, "Willys");

            var coopProducts = await FoodService.GetStoreProducts("Coop");
            var coopFoodItems = FoodItemMapper.MapProductsToFoodItems(coopProducts, "Coop", willysFoodItems.Count);

            var allFoodItems = new List<FoodItem>();

            allFoodItems.AddRange(willysFoodItems);
            allFoodItems.AddRange(coopFoodItems);

            return allFoodItems;
            
            return null;
        }
    }
}