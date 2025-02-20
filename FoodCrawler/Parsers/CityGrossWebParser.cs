﻿using FoodCrawler.Models;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodCrawler.Parsers
{
    public class CityGrossWebParser
    {
        private List<SpecialPrice> specialPriceList;
        private ChromeDriver driver;

        public CityGrossWebParser()
        {
            ChromeOptions options = new ChromeOptions();
            driver = new ChromeDriver();
        }
                
        public async Task<List<CityGrossProduct>> StartParser(string url)
        {
            //ChromeOptions options = new ChromeOptions();
            // TODO : Add headless option
            //options.AddArguments("headless");
            //var driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl(url);
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(ExpectedConditions.ElementExists((By.XPath("//*[contains(@class,'c-loadmore__status')]"))));
            var content = driver.PageSource;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            //driver.manage().timeouts().implicitlyWait(number_of_seconds, TimeUnit.SECONDS);
            

            var countTest = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'c-loadmore__status')]").First().InnerText;
            var countString = parseCount(countTest);
            var newUrl = CalculateUrl(countString, url);

            driver.Navigate().GoToUrl(newUrl);

            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            bool fortsatt = true;
            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(ExpectedConditions.ElementExists((By.XPath("//*[contains(@class,'product-card__lower-container')]"))));
            var fullContent = driver.PageSource;
            htmlDocument.LoadHtml(fullContent);

            var productInfoNodes =  htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'product-card__lower-container')]");
            //var imageNodes = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'c-progressive-picture__picture')]");
            var totalProducts = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class,'product-card__upper-container')]");

            var products = GetProducts(productInfoNodes);
            var brandTuple = GetManufacturerAndWeight(productInfoNodes);
            var brands = brandTuple.brands;
            var vikter = brandTuple.vikts;
            var priceTuple = GetPriceAndUnits(productInfoNodes);
            var priceList = priceTuple.priceList;
            var units = priceTuple.unitList;
            var imagePaths  = GetImageUrls(totalProducts);

            /*
            foreach (var product in productInfoNodes)
            {
                var manufacturerAndWeight = product.ChildNodes[0].ChildNodes[1].InnerText;
                var manWeightTuple = parseManufacturerWeight(manufacturerAndWeight);
                var productText = product.ChildNodes[0].ChildNodes[0].InnerText;
                var jmfrPris = product.ChildNodes[1];
                var priceTuple = parsePrice(jmfrPris);

                var manufacturer = manWeightTuple.Item1;
                var weight = manWeightTuple.Item2;

                var priceValue = priceTuple.Item1;
                var unit = priceTuple.Item2;
            }
            foreach(var upperProduct in totalProducts)
            {
                var links = upperProduct.ChildNodes[2];
                var imageUrl = parseImages(links);
            }
            */

            return SetupProducts(products, brands, vikter, priceList, units, imagePaths);

        }

        private List<CityGrossProduct> SetupProducts(List<string> titles,List<string> brandList, 
            List<string> weightList, List<double> priceList,List<string> unitList, List<string> imagePaths)
        {
            List<CityGrossProduct> productList = new List<CityGrossProduct>();
            for (int i = 0; i < titles.Count; i++)
            {
                var product = new CityGrossProduct()
                {
                    ProductID = i,
                    Title = titles[i],
                    Manufacturer = brandList[i],
                    ImageURL = imagePaths[i],
                    Price = priceList[i],
                    Weight = weightList[i],
                    Unit = unitList[i]
                };
                productList.Add(product);
            }

            foreach(var cityProduct in productList)
            {
                cityProduct.Specialprice = specialPriceList.FirstOrDefault(x => x.ProductId.Equals(cityProduct.ProductID));
            }

            return productList;
        }

        private (List<string> brands, List<string> vikts) GetManufacturerAndWeight(HtmlNodeCollection htmlNodes)
        {
            List<string> brands = new List<string>();
            List<string> vikts = new List<string>();
            foreach(var product in htmlNodes)
            {
                var manufacturerAndWeight = product.ChildNodes[0].ChildNodes[1].InnerText;
                var manWeightTuple = parseManufacturerWeight(manufacturerAndWeight);

                var manufacturer = manWeightTuple.Item1;
                var weight = manWeightTuple.Item2;
                brands.Add(manufacturer);
                vikts.Add(weight);
            }

            return (brands, vikts);
        }

        private List<string> GetProducts(HtmlNodeCollection htmlNodes)
        {
            List<string> productList = new List<string>();

            foreach(var product in htmlNodes)
            {
                var productText = product.ChildNodes[0].ChildNodes[0].InnerText;
                productList.Add(productText);
            }

            return productList;
        }

        private (List<double> priceList, List<string> unitList) GetPriceAndUnits(HtmlNodeCollection htmlNodes)
        {
            List<double> priceList = new List<double>();
            List<string> unitList = new List<string>();
            specialPriceList = new List<SpecialPrice>();

            string totalPrice = string.Empty;
            int productCount = 0;
            foreach (var product in htmlNodes)
            {
                try
                {
                    var jmfrPris = product.ChildNodes[1];
                    //product.SelectNodes(By.XPath(""))
                    var integerPrice = jmfrPris.SelectSingleNode(".//span[contains(@class,'integer')]").InnerText;
                    var unit = jmfrPris.SelectSingleNode(".//span[contains(@class,'unit')]");
                    if (unit != null)
                        unitList.Add(unit.InnerText);
                    else
                        unitList.Add("st");
                    //var singlePriceTest = jmfrPris.ChildNodes.Where(child => child.)
                    var decimalTest = jmfrPris.SelectNodes(".//span[contains(@class, 'fractions')]/text()");
                    if (decimalTest != null)
                        totalPrice = $"{integerPrice}.{decimalTest.FirstOrDefault().InnerText}";
                    else
                        totalPrice = integerPrice;
                    double outPrice;
                    double priceValue;
                    if (double.TryParse(totalPrice,NumberStyles.Number, CultureInfo.InvariantCulture, out outPrice))
                    {
                        priceValue = double.Parse(totalPrice, CultureInfo.InvariantCulture);
                    } else
                    {
                        var specialPrice = ParseExtraPrice(jmfrPris, productCount);
                        priceValue = specialPrice.OrdinaryPrice;
                        specialPriceList.Add(specialPrice);
                    }
                    
                    //var priceTuple = parsePrice(jmfrPris);
                    priceList.Add(priceValue);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }
                productCount++;
                //var priceValue = priceTuple.Item1;
                //priceList.Add(priceValue);
                //var unit = priceTuple.Item2;
                //unitList.Add(unit);
            }

            return (priceList, unitList);
        }

        private SpecialPrice ParseExtraPrice(HtmlNode node, int productID)
        {
            var extraPrice = new SpecialPrice();
            extraPrice.ProductId = productID;
            string quantityString = string.Empty;
            int quantity = 0;
            var extraQty = node.SelectSingleNode(".//p[contains(@class,'c-pricetag__effect-type')]");
            var ordinaryPrice = node.SelectSingleNode(".//span[contains(@class, 'bold ordinal')]");
            if (extraQty != null)
            {
                quantityString  = Regex.Match(extraQty.InnerText, @"\d+").Value;
                quantity = int.Parse(quantityString);
                extraPrice.Quantity = quantity;
            }
            if (ordinaryPrice != null)
            {                
                string[] priceParts = ordinaryPrice.InnerText.Split(',');
                var integerPart = new String(priceParts[0].Where(Char.IsDigit).ToArray());
                var decimalPart = new String(priceParts[1].Where(Char.IsDigit).ToArray());
                var ordinaryPriceString = $"{integerPart}.{decimalPart}";
                var priceValue = double.Parse(ordinaryPriceString, CultureInfo.InvariantCulture);
                extraPrice.OrdinaryPrice = priceValue;
            }
            var integerPriceParseFail = node.SelectSingleNode(".//span[contains(@class,'integer')]").InnerText;
            var integerPriceString = new String(integerPriceParseFail.Where(Char.IsDigit).ToArray());
            if (extraQty == null)
            {
                extraPrice.Extrapris = double.Parse(integerPriceString);
                //return double.Parse(integerPriceString);
            }
            //double newPrice = int.Parse(integerPriceString) / quantity;
            //return newPrice;
            return extraPrice;
        }

        private List<string> GetImageUrls(HtmlNodeCollection htmlNodes)
        {
            List<string> imageUrls = new List<string>();
            foreach (var upperProduct in htmlNodes)
            {
                var links = upperProduct.ChildNodes[2];
                var imageUrl = parseImages(links);
                imageUrls.Add(imageUrl);
            }

            return imageUrls;
        }

        private string parseImages(HtmlNode linkParentNode)
        {
            var href = linkParentNode.ChildNodes[0].Attributes["href"].Value;
            var imageURL = $"https://www.citygross.se/{href}";
            return imageURL;
        }

        private (double,string) parsePrice(HtmlNode priceNode)
        {
            var integerPrice = priceNode.ChildNodes[0].ChildNodes[0].FirstChild.FirstChild.InnerText;
            var decimalValue = priceNode.ChildNodes[0].ChildNodes[0].FirstChild.ChildNodes[1].ChildNodes[1].InnerText;
            var unit = priceNode.ChildNodes[0].ChildNodes[0].FirstChild.ChildNodes[1].ChildNodes[2].InnerText;
            var stringPrice = $"{integerPrice}.{decimalValue}";
            var priceValue = double.Parse(stringPrice, CultureInfo.InvariantCulture);
            return (priceValue, unit);
        }

        private string parseCount(string nodeContent)
        {
            string count = nodeContent.Substring(nodeContent.IndexOf("av"));
            var numericCount = new String(count.Where(Char.IsDigit).ToArray());            
            return numericCount;
        }

        private (string,string) parseManufacturerWeight(string manufacturerAndWeight)
        {
            var pieces = manufacturerAndWeight.Split(new[] { ',' }, 2);

            return (pieces[0].Trim(), pieces[1].Trim());
        }

        private string CalculateUrl(string count, string url)
        {
            int totalCount = int.Parse(count);

            var pageNumbers = totalCount / 20;
            var newUrl = $"{url}?page={pageNumbers}";
            return newUrl;
        }

        public static void ClickAndWaitForPageToLoad(By elementLocator, ChromeDriver Driver, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
                var element = Driver.FindElement(elementLocator);
                Actions action = new Actions(Driver);                
                //action.MoveToElement(element).Perform();
                //element.Click();
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
                ex.ExecuteScript("arguments[0].click();", element);
                //wait.Until(ExpectedConditions.StalenessOf(element));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }
    }
}
