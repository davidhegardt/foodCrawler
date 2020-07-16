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
        public static async Task<List<Product>> GetStoreProducts(Store storeEnum, Category categoryEnum)
        {
            switch (storeEnum)
            {
                case Store.Coop: return await GetCoopProducts(categoryEnum);
                //case Store.CityGross: return await GetCityGrossProducts();
                case Store.Willys: return await GetWillysProducts(categoryEnum);
            }

            return null;
        }

        private static async Task<List<Product>> GetWillysProducts(Category categoryEnum)
        {
            var url = GetMainUrl(Store.Willys);
            var category = GetCategoryUrl(Store.Willys, categoryEnum);

            url = $"{url}{category}";

            WillysWebParser willys = new WillysWebParser();

            var content = willys.SeleniumStart(url);
            return willys.ParseProducts(content);
        }

        private static async Task<List<Product>> GetCoopProducts(Category categoryEnum)
        {
            var url = GetMainUrl(Store.Coop);
            var category = GetCategoryUrl(Store.Coop, categoryEnum);

            url = $"{url}{category}";

            CoopWebParser coop = new CoopWebParser();
            return await coop.StartParser(url);
        }

        public static async Task<List<CityGrossProduct>> GetCityGrossProducts()
        {
            var startUrl = "https://www.citygross.se/matvaror/";
            var categories = GetCityGrossCategoryUrl("Cheese");
            var allProducts = new List<CityGrossProduct>();
            CityGrossWebParser cityGross = new CityGrossWebParser();

            foreach (var category in categories)
            {
                var url = $"{startUrl}{category}";
                var products = await cityGross.StartParser(url);
                allProducts.AddRange(products);
            }
            return allProducts;
        }

        private static string[] GetCityGrossCategoryUrl(string category)
        {
            switch (category)
            {
                case "Cheese": return new string[] { "mejeri-ost-och-agg/hardost", "mejeri-ost-och-agg/dessertost", "mejeri-ost-och-agg/mjukost" };
                default: return null;
            }
        }

        private static string GetMainUrl(Store storeName)
        {
            switch (storeName)
            {
                case Store.Willys: return "https://www.willys.se/sortiment/";
                case Store.Coop: return "https://www.coop.se/handla/varor/";
                default: return string.Empty;
            }
        }

        private static string GetCategoryUrl(Store storeName,Category category)
        {
            switch (storeName)
            {
                case Store.Willys: return GetWillysCategoryUrl(category);
                case Store.Coop: return GetCoopCategoryUrl(category);
                default: return string.Empty;
            }
        }

        private static string GetCoopCategoryUrl(Category category)
        {
            switch (category)
            {
                case Category.Cheese: return "ost";
                case Category.Diary: return "mejeri-agg";
                case Category.Meat: return "kott-fagel";
                default: return string.Empty;
            }
        }

        private static string GetWillysCategoryUrl(Category category)
        {
            switch (category)
            {
                case Category.Diary: return "Mejeri-ost-och-agg";
                case Category.Cheese: return "Mejeri-ost-och-agg/Ost";
                case Category.Meat: return "Kott-chark-och-fagel";
                default: return string.Empty;
            }
        }
    }
}
