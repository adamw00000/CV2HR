using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Moq;
using Shouldly;

namespace CV2HR.UITests
{
    public class JobOfferUITests
    {
        [Fact]
        public void MainPageRedirectsToJobOfferIndex()
        {
            var driver = new ChromeDriver(Directory.GetCurrentDirectory());
            EnterJobOfferIndex(driver);

            var title = driver.FindElement(By.XPath("//*[@id=\"vuePage\"]/div/h2"));
            var list = driver.FindElement(By.XPath("//*[@id=\"vuePage\"]/div/table[2]"));

            title.Text.ShouldBe("Job offer list:");
            list.Displayed.ShouldBe(true);

            driver.Close();
        }

        private static void EnterJobOfferIndex(ChromeDriver driver)
        {
            driver.Url = "https://localhost:44310/";
            driver.FindElement(By.XPath("//*[@id=\"cookieConsent\"]/div/div[2]/div/button")).Click();
            driver.FindElement(By.LinkText("Job Offers")).Click();
        }

        [Fact]
        public void SearchButtonRedirectsToResults()
        {
            var driver = new ChromeDriver(Directory.GetCurrentDirectory());
            EnterJobOfferIndex(driver);

            driver.FindElement(By.XPath("//*[@id=\"search\"]")).SendKeys("Developer");
            driver.FindElement(By.XPath("//*[@id=\"searchButton\"]")).Click();

            var title = driver.FindElement(By.XPath("//*[@id=\"vuePage\"]/div/h2"));
            var list = driver.FindElement(By.XPath("//*[@id=\"vuePage\"]/div/table[2]"));

            title.Text.ShouldBe("Job offer list:");
            list.Displayed.ShouldBe(true);

            driver.Close();
        }

        [Fact]
        public void ClickingOfferNameRedirectsToOfferDetails()
        {
            var driver = new ChromeDriver(Directory.GetCurrentDirectory());
            EnterJobOfferIndex(driver);

            var link = driver.FindElement(By.XPath("//*[@id=\"offers\"]/tr[1]/td[1]/a"));
            var jobTitle = link.Text;
            link.Click();

            var title = driver.FindElement(By.XPath("/html/body/div/div[1]/h1/span"));
            var applyButton = driver.FindElement(By.XPath("/html/body/div/div[1]/div[2]/div/form/div/button"));

            title.Text.ShouldBe(jobTitle);
            applyButton.Displayed.ShouldBe(true);

            driver.Close();
        }
    }
}
