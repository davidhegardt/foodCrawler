using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FoodCrawler.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using FoodCrawler.Parsers;

namespace FoodCrawler
{
    public static class FoodService
    {
        public static async Task<List<Product>> GetStoreProducts(string storeName)
        {
            switch (storeName)
            {
                case "Willys": return await GetWillysProducts();
                case "Coop": return await GetCoopProducts();
                case "CityGross": return await GetCityGrossProducts();
            }

            return null;
        }

        private static async Task<List<Product>> GetWillysProducts()
        {
            var url = GetMainUrl("Willys");
            var category = GetCategoryUrl("Willys", "Cheese");

            url = $"{url}{category}";

            WillysWebParser willys = new WillysWebParser();

            var content = willys.SeleniumStart(url);
            return willys.ParseProducts(content);
        }

        private static async Task<List<Product>> GetCoopProducts()
        {
            var url = GetMainUrl("Coop");
            var category = GetCategoryUrl("Coop", "Cheese");

            url = $"{url}{category}";

            CoopWebParser coop = new CoopWebParser();
            return await coop.StartParser(url);
        }

        private static async Task<List<Product>> GetCityGrossProducts()
        {
            var url = "https://www.citygross.se/matvaror/";
            var categories = GetCityGrossCategoryUrl("Cheese");

            var firstUrl = $"{url}{categories[0]}";
            var secondUrl = $"{url}{categories[1]}";
            var thirdUrl = $"{url}{categories[2]}";

            CityGrossWebParser cityGross = new CityGrossWebParser();
            return await cityGross.StartParser(firstUrl);
        }

        private static string[] GetCityGrossCategoryUrl(string category)
        {
            switch (category)
            {
                case "Cheese": return new string[] { "mejeri-ost-och-agg/hardost", "mejeri-ost-och-agg/dessertost", "mejeri-ost-och-agg/mjukost" };
                default: return null;
            }
        }

        private static string GetMainUrl(string storeName)
        {
            switch (storeName)
            {
                case "Willys": return "https://www.willys.se/sortiment/";
                case "Coop": return "https://www.coop.se/handla/varor/";
                default: return string.Empty;
            }
        }

        private static string GetCategoryUrl(string storeName,string category)
        {
            switch (storeName)
            {
                case "Willys": return GetWillysCategoryUrl(category);
                case "Coop": return GetCoopCategoryUrl(category);                
                default: return string.Empty;
            }
        }

        private static string GetCoopCategoryUrl(string category)
        {
            switch (category)
            {
                case "Cheese": return "ost";
                case "Diary": return "mejeri-agg";
                case "Meat": return "kott-fagel";
                default: return string.Empty;
            }
        }

        private static string GetWillysCategoryUrl(string category)
        {
            switch (category)
            {
                case "Diary": return "Mejeri-ost-och-agg";
                case "Cheese": return "Mejeri-ost-och-agg/Ost";
                case "Meat": return "Kott-chark-och-fagel";
                default: return string.Empty;
            }
        }
    }
}
