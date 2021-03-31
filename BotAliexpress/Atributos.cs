using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;

namespace BotAliexpress
{
    public class Atributos
    {
        protected static readonly ChromeDriver chromeDriver = new ChromeDriver();
        protected static readonly HtmlDocument html = new HtmlDocument();
    }
}
