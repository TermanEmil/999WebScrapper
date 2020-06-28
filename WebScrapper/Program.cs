using System;
using System.Text;
using OpenQA.Selenium.Chrome;

namespace SeleniumSandbox
{
    public class Program
    {
        public static void Main(string[] _)
        {
            Console.OutputEncoding = Encoding.UTF8;

            using var webDriver = new ChromeDriver();
            var webScrapper = new Site999MdWebScrapper(webDriver);

            try
            {
                foreach (var car in webScrapper.ExtractGoods("car"))
                    Console.WriteLine(car);
            }
            finally
            {
                webDriver.Quit();
            }
        }
    }
}
