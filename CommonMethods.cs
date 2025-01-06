using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Reflection;
using System.IO;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions; 
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using SampleCshrapProgram.Pages;
using Newtonsoft.Json;
using System.Linq;

namespace SampleCshrapProgram.Support
{
    public class CommonMethods
    {
        public static string filePath= " C:\\Users\\Rahulk\\Desktop\\AssignmenTcap\\SampleCshrapProgram\\Testdata\\TestData.json";
        public IWebDriver driver;
        public string TestCaseID;
        private IWebElement element;
        public bool bStatus;
        public void
            OpenBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.google.com/");


        }
        public IWebElement FindLocator(string locator)
        {
            if (locator.StartsWith("//") || locator.StartsWith("/html") || locator.StartsWith("xpath="))
            {
                if (locator.StartsWith("XPath="))
                {

                    element = driver.FindElement(By.XPath(locator.Substring("XPath=".Length)));
                }

                else
                {

                    element = driver.FindElement(By.XPath(locator));
                }

            }
            else if (locator.StartsWith("@"))
            {
                if (locator.StartsWith("@="))
                    element = driver.FindElement(By.XPath("//*[@text='" + locator.Substring("@=".Length) + "']"));
                else if (locator.StartsWith("@content-desc="))
                    element = driver.FindElement(By.XPath("//*[contains(@content-desc, '" + locator.Substring("@content-desc=".Length) + "')]"));
                else
                    element = driver.FindElement(By.XPath("//*[contains(@text, '" + locator.Substring("@".Length) + "')]"));
            }
            else if (locator.StartsWith("LinkText="))
            {
                element = driver.FindElement(By.LinkText(locator.Substring("LinkText=".Length)));
            }
            else if (locator.StartsWith("PartialLinkText="))
            {
                element = driver.FindElement(By.PartialLinkText(locator.Substring("PartialLinkText=".Length)));
            }
            else if (locator.StartsWith("Class="))
            {
                element = driver.FindElement(By.ClassName(locator.Substring("Class=".Length)));
            }
            else if (locator.StartsWith("Css="))
            {
                element = driver.FindElement(By.CssSelector(locator.Substring("Css=".Length)));
            }
            else
            {
                element = driver.FindElement(By.Id(locator));
            }
            return element;
        }

        public IWebElement GetElement(string locator)
        {
            try
            {
                if (FindLocator(locator).Displayed)
                    element = FindLocator(locator);
                WaitForElementVisiable(locator, Variables.longer);
                Assert.IsTrue(FindLocator(locator).Displayed);
            }
            catch (NoSuchElementException)
            {
                throw new ArgumentException("Failed to Find Element: " + locator);
            }
            return element;
        }


        public void WaitAndClick(string loctor, int timeout)
        {
            element = GetElement(loctor);

            bStatus = WaitForElementVisiable(loctor, timeout);
            if (bStatus)
            {
                element.Click();

            }


        }
        public bool WaitForElementVisiable(string locator, int timewait)
        {

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timewait));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));

            }
            catch (Exception e)
            {

                return false;
            }

            return true;

        }

        public void EnterText(String locator, String value)
        {
            element = GetElement(locator);

            bStatus = WaitForElementVisiable(locator, Variables.longer);
            if (bStatus)
            {
                element.Clear();
                element.SendKeys(value);

            }


        }
        public void valiadtelinks()

        {
            bStatus = driver.FindElement(By.XPath(GoogleSearchPage.txt_Avivalinks)).Displayed;
            if (!bStatus)
            {
                Assert.Fail("links are not visiable");
            }
            else
            {
                IList<IWebElement> elements = new List<IWebElement>();
                elements = driver.FindElements(By.XPath(GoogleSearchPage.txt_Avivalinks));

                int count = elements.Count;
                string link = elements[5].Text;
                Assert.That(count, Is.GreaterThanOrEqualTo(6));

                Console.WriteLine("Fifth link of Aviva is " + link + "");
            }

        }


        public void Quit()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }


        public Dictionary<string, string> ReadTestDataFromJSONFile(string FilePath, string TestCaseName)

        {

            TestCaseID = null;

            TestCaseID = TestCaseName;

            string jKey = null;

            string jValue = null;

            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            using (var reader = new StreamReader(FilePath))

            {

                var strResultjson = File.ReadAllText(FilePath);

                var dictionary = JsonConvert.DeserializeObject<IDictionary>(strResultjson);

                foreach (DictionaryEntry entry in dictionary)

                {

                    string key = null;

                    string value = null;

                    key = entry.Key.ToString();

                    if (key.Equals(TestCaseName))

                    {

                        value = entry.Value.ToString().Replace("{", "").Replace("}", "").Replace("\"", "").TrimEnd('\n');

                        string[] items = value.Split('\n');

                        foreach (string item in items.Skip(1))

                        {

                            string[] keyValue = item.Split(':');

                            jKey = keyValue[0].Trim();

                            jValue = keyValue[1].Trim().TrimEnd(',');

                            if (jValue != "null")

                            {

                                jsonData[jKey] = jValue;

                            }

                        }

                    }

                }

            }

            return jsonData;

        }




        public string CaptureScreenshot(string testName = "screen")

        {
            string projectPath = Path.GetDirectoryName(GetTestAssemblyFolder());

            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

            string fileLocation = $"{projectPath}/{testName}.png";

            ss.SaveAsFile(fileLocation, ScreenshotImageFormat.Png);

            return fileLocation;

        }

        private string GetTestAssemblyFolder()

        {

            return Assembly.GetExecutingAssembly().Location;

        }

    }



    }

