using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace BotAliexpress
{
    class Program
    {
        //Url do carrinho de compras da Aliexpress
        private const string url = "https://shoppingcart.aliexpress.com/orders.htm?aeOrderFrom=main_shopcart&availableProductShopcartIds=81006878252174";

        static void Main(string[] args)
        {
            ChromeDriver chromeDriver = new ChromeDriver();
            chromeDriver.Navigate().GoToUrl(url);
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(chromeDriver.PageSource);

            string valorInicial = html.DocumentNode.SelectSingleNode("//div[@class='total-price']").InnerText;

            //Bot setado para rodar apenas a partir das 21:00:00
            DateTime horarioInicio = DateTime.Parse("21:00:00");

            bool valid = false;

            while (!valid)
            {
                while (DateTime.Parse(DateTime.Now.ToString("HH:mm:ss")) >= horarioInicio)
                {
                    try
                    {
                        string validacaoPagamento = html.DocumentNode.SelectSingleNode("//div[@class='pay-brief-info']").InnerText;
                    }
                    catch (NullReferenceException e)
                    {
                        chromeDriver.FindElementByXPath("//button[@class='next-btn next-medium next-btn-primary next-btn-text selected-payment-btn']").Click();
                        Thread.Sleep(500);
                        chromeDriver.FindElementByXPath("//div[@ae_object_value = 'select_payoption=BOLETO']").Click();
                        Thread.Sleep(500);
                        chromeDriver.FindElementByXPath("//div[@class='save']//button").Click();
                        Thread.Sleep(500);
                    }

                    //Inserir o cupom e finalizar a compra
                    while (true)
                    {
                        //Código do cupom de desconto
                        chromeDriver.FindElementByXPath("//div[@class='next-form-item-control']//input").SendKeys("ALIBR99");
                        Thread.Sleep(500);
                        chromeDriver.FindElementByXPath("//div[@class='next-form-item-control']//button").Click();
                        Thread.Sleep(500);
                        chromeDriver.FindElementByXPath("//div[@id='root']").Click();
                        Thread.Sleep(500);

                        html.LoadHtml(chromeDriver.PageSource);
                        string valorFinal = html.DocumentNode.SelectSingleNode("//div[@class='total-price']").InnerText;

                        //Verifica se o valor foi alterado
                        if (valorInicial != valorFinal)
                        {
                            break;
                        }
                    }
                    chromeDriver.FindElementByXPath("//div[@class='order-btn-holder']//button").Click();
                    valid = true;
                    break;
                }
            }
            Console.WriteLine("É NOIS QUE VOA BRUXO!!");
        }
    }
}
