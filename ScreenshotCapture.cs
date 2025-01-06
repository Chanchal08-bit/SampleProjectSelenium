using System.IO;
using OpenQA.Selenium;
using System;
using System.Reflection;
namespace SampleCshrapProgram.Support
{
    public class ScreenshotCapture
    {

        public ScreenshotCapture()
        {

        }

        public string ScreenCapture(IWebDriver driver, string testName = "screen")

        {
            Console.WriteLine("driver second instance is "+driver+"");
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
