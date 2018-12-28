using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using Xunit;

namespace CV_2_HR.UITests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var driver = new ChromeDriver(Directory.GetCurrentDirectory())
            {
                Url = "https://google.com"
            };
            driver.Close();
        }
    }
}
