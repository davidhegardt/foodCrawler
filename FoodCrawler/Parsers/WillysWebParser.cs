using FoodCrawler.Models;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCrawler.Parsers
{
    public class WillysWebParser
    {
        private int waitTime = 1;
        private string totaltProductCount;
        public string SeleniumStart(string url)
        {
            //return minimumSelenium(url);

            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("headless");
            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);
            totaltProductCount = driver.FindElement(By.Id("selenium--searchResult-number-of-products")).Text;

            bool fortsatt = true;
            while (fortsatt)
            {
                try
                {

                    var span = driver.FindElement(By.XPath("//span[.='Visa fler']"));
                    Actions action = new Actions(driver);
                    action.MoveToElement(span).Perform();
                    span.Click();
                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, waitTime));

                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//span[.='Visa fler']")));

                }
                catch (OpenQA.Selenium.WebDriverTimeoutException exception)
                {
                    fortsatt = false;
                    continue;
                }
            }
            var content = driver.PageSource;
            return content;
        }

        private static string minimumSelenium(string url)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("headless");
            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);
            var span = driver.FindElement(By.XPath("//span[.='Visa fler']"));
            Actions action = new Actions(driver);
            action.MoveToElement(span).Perform();
            span.Click();
            var content = driver.PageSource;
            return content;
        }


        public List<Product> ParseProducts(string contentResponse)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(contentResponse);

            var price = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'ax-product-pricelabel-price layout-row layout-align-center-stretch')]");
            var productInfo = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'ax-product-info')]");
            var productImages = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'ax-product-puff-image selenium--product-puff-img layout-align-center')]");

            var lists = splitList(productInfo);
            var productManufacturerList = lists.Item1;            
            var priceList = GetPriceList(price);
            var imagePathList = GetProductImages(productImages);
            return SetupProducts(productManufacturerList, priceList, imagePathList);
        }

        private List<Product> SetupProducts(IEnumerable<HtmlNode> productManufacturerList, List<double> priceList, List<string> imagePathList)
        {
            var productList = new List<Product>();
            foreach (var node in productManufacturerList)
            {
                var product = new Product();
                var productTitle = node.ChildNodes.Where(child => child.Attributes["class"].Value.Contains("product-title")).FirstOrDefault().InnerText;
                product.Title = productTitle;
                var manufacturerInfo = node.ChildNodes.Where(child => child.Attributes["class"].Value.Contains("manufacturer")).FirstOrDefault();
                if (manufacturerInfo != null)
                {
                    var manufactorName = manufacturerInfo.ChildNodes[0].InnerText;
                    product.Manufacturer = manufactorName;
                    var weight = manufacturerInfo.ChildNodes[2].InnerText;
                    product.Weight = weight;
                }
                productList.Add(product);
            }

            for (int i = 0; i < imagePathList.Count; i++)
            {
                productList[i].ImageURL = imagePathList[i];
            }

            for (int i = 0; i < priceList.Count; i++)
            {
                productList[i].Price = priceList[i];
            }

            return productList;
        }

        private List<string> GetProductImages(HtmlNodeCollection productImages)
        {
            var imagePathList = new List<string>();
            foreach (var imageNode in productImages)
            {
                var path = imageNode.ChildNodes[0].Attributes["src"].Value;
                imagePathList.Add(path);
            }
            return imagePathList;
        }

        private List<double> GetPriceList(HtmlNodeCollection price)
        {
            var priceList = new List<double>();
            foreach (var priceNode in price)
            {
                var innerhtml = priceNode.InnerHtml;
                var searchString = "itemprop=\"price\" content=\"";
                var startIndex = innerhtml.LastIndexOf(searchString) + searchString.Length;
                var endIndex = innerhtml.IndexOf(">", startIndex);
                var charLenght = endIndex - startIndex - 1;
                var priceString = innerhtml.Substring(startIndex, charLenght);                
                var productPrice = double.Parse(priceString, CultureInfo.InvariantCulture);
                priceList.Add(productPrice);
            }

            return priceList;
        }


        private (IEnumerable<HtmlNode>, IEnumerable<HtmlNode>) splitList(HtmlNodeCollection nodes)
        {            
            var jmfPrisList = nodes.Where((x, i) => i % 2 != 0);
            var productManufacturerList = nodes.Where((x, i) => i % 2 == 0);
            return (productManufacturerList, jmfPrisList);
        }
    }
}
