using FoodCrawler.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodCrawler.Parsers
{
    public class CoopWebParser
    {
        private int _waitTime = 2;
        private int _productCount;
        public async Task<List<Product>> StartParser(string url)
        {
            var content = await GetUsableContent(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            var productItems = htmlDocument.DocumentNode.SelectNodes("//*[@data-click-gtm]");

            int foundProducts = productItems.Count;
            if (foundProducts < (_productCount - 2))
                await GetUsableContent(url);

            List<CoopProduct> coopProducts = new List<CoopProduct>();
            List<string> weights = new List<string>();
            List<string> imagePaths = new List<string>();

            foreach (var prodNode in productItems)
            {
                var unformatted = prodNode.Attributes["data-click-gtm"].Value;
                var formatted = FormatProductData(unformatted);
                var coopProduct = JsonConvert.DeserializeObject<CoopProduct>(formatted);
                coopProducts.Add(coopProduct);
            }
            var productSummaries = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'product-summary')]");
            foreach (var summaryNode in productSummaries)
            {
                var unformattedWeight = summaryNode.ChildNodes[2].InnerText;
                var formatted = FormatWeight(unformattedWeight);
                weights.Add(formatted);
            }
            var productImages = htmlDocument.DocumentNode.SelectNodes("//figure[contains(@class,'product-img')]");
            foreach (var imageNode in productImages)
            {
                var image = imageNode.ChildNodes.Where(child => child.Name.Equals("img")).FirstOrDefault();
                if (image != null)
                {
                    var path = image.Attributes["src"].Value;
                    imagePaths.Add(path);
                }
            }

            return CreateProducts(coopProducts, imagePaths, weights);

        }

        private List<Product> CreateProducts(List<CoopProduct> coopProducts, List<string> images, List<string> weights)
        {
            var products = new List<Product>();
            for(int i = 0; i < coopProducts.Count; i++)
            {
                var product = new Product()
                {
                    ImageURL = images[i],
                    Weight = weights[i],
                    Manufacturer = coopProducts[i].Brand,
                    Price = coopProducts[i].Price,
                    Title = coopProducts[i].Name
                };
                products.Add(product);
            }

            return products;
        }

        private async Task<string> GetUsableContent(string url)
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("headless");
            var driver = new ChromeDriver(options);
            var finished = false;
            int retryCounter = 0;

            while (!finished)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);

                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, _waitTime));

                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//span[@class='js-search-results-count']")));
                                        
                    var content = driver.PageSource;

                    _productCount = int.Parse(GetProductCount(content));

                    string completeURL = $"{url}?page=1&pageSize={_productCount}";

                    driver.Navigate().GoToUrl(completeURL);

                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//span[@class='js-search-results-count']")));

                    finished = true;
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException timeoutException)
                {
                    retryCounter++;
                    _waitTime++;
                    if (retryCounter > 3)
                        finished = true;

                    continue;
                }
            };

            var fullContent = driver.PageSource;

            return fullContent;
        }

        private string FormatProductData(string productData)
        {
            string test = "&quot;";
            string quote = @"""";
            return productData.Replace(test, quote);
        }

        private string FormatWeight(string productWeight)
        {
            var numeric = Regex.Match(productWeight, "\\d+").Value;
            return numeric;
        }

        private string GetProductCount(string content)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var productCount = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'js-search-results-count')]").First().InnerText;
            return productCount;
        }

    }
}
