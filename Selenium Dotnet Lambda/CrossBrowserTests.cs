using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Dotnet_Lambda
{
    [TestFixture("Chrome", "88.0", "Windows 10")]
    [TestFixture("MicrosoftEdge", "87.0", "macOS Sierra")]
    [TestFixture("Firefox", "82.0", "Windows 7")]
    [TestFixture("Internet Explorer", "11.0", "Windows 10")]
    [Parallelizable(ParallelScope.All)]
    public class CrossBrowserTests
    {
        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        private string browser;
        private string version;
        private string os;

        public CrossBrowserTests(string browser, string version, string os)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
        }


        [SetUp]
        public void SetupBrowser()
        {
            string username = "lambdaUserName";
            string accesskey = "lambdaUserPass";
            string gridURL = "@hub.lambdatest.com/wd/hub";


            Dictionary<string, object> ltOptions = new ();
            ltOptions.Add("username", username);
            ltOptions.Add("accessKey", accesskey);
            ltOptions.Add("visual", true);
            ltOptions.Add("video", true);
            ltOptions.Add("network", true);
            ltOptions.Add("platformName", os);
            ltOptions.Add("project", "Selenium Dotnet Lambda");
            ltOptions.Add("w3c", true);
            ltOptions.Add("name",String.Format("{0}:{1}",
                TestContext.CurrentContext.Test.ClassName,
                TestContext.CurrentContext.Test.MethodName));

            if (browser.Equals("Chrome"))
            {
                ChromeOptions chromeOptions = new();
                chromeOptions.BrowserVersion = version;
                chromeOptions.AddAdditionalOption("LT:Options", ltOptions);
                driver.Value = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL),
                    chromeOptions);
            }
            else if (browser.Equals("Firefox"))
            {
                FirefoxOptions firefoxOptions = new();
                firefoxOptions.BrowserVersion = version;
                firefoxOptions.AddAdditionalOption("LT:Options", ltOptions);
                driver.Value = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL),
                    firefoxOptions);
            }
            else if (browser.Equals("MicrosoftEdge"))
            {
                EdgeOptions edgeOptions = new();
                edgeOptions.BrowserVersion = version;
                edgeOptions.AddAdditionalOption("LT:Options", ltOptions);
                driver.Value = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL),
                    edgeOptions);
            }
            else if (browser.Equals("Internet Explorer"))
            {
                InternetExplorerOptions internetExplorerOptions = new();
                internetExplorerOptions.BrowserVersion = version;
                internetExplorerOptions.AddAdditionalOption("LT:Options", ltOptions);
                driver.Value = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL),
                    internetExplorerOptions);
            }

            Thread.Sleep(2000);

            
            // driver.Manage().Window.Maximize();
            // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test, Timeout(20000)]
        [Category("Test1")] 
        public void Test1()
        {
            // 1. Open LambdaTest’s Selenium Playground from https://www.lambdatest.com/selenium-playground
            driver.Value!.Url = "https://www.lambdatest.com/selenium-playground";
            System.Threading.Thread.Sleep(1000);

            // 2.Click “Simple Form Demo” under Input Forms.
            driver.Value!.FindElement(By.XPath("//a[contains(text(),'Simple Form Demo')]")).Click();
            System.Threading.Thread.Sleep(1000);

            // 3. Validate that the URL contains “simple-form-demo”.
            StringAssert.Contains("simple-form-demo", driver.Value!.Url, "Driver Url is Unexpected");

            // 4. Create a variable for a string value E.g: “Welcome to LambdaTest”.
            string message = "Welcome to LambdaTest";

            // 5. Use this variable to enter values in the “Enter Message” text box.
            driver.Value!.FindElement(By.CssSelector("input[id='user-message']")).SendKeys(message);

            // 6. Click “Get Checked Value”.
            driver.Value!.FindElement(By.CssSelector("button[id='showInput']")).Click();
            System.Threading.Thread.Sleep(1000);

            // 7. Validate whether the same text message is displayed in the right-hand panel under the “Your Message:” section.
            string actualText = driver.Value!.FindElement(By.CssSelector("p[id='message']")).Text;
            Assert.AreEqual(message, actualText);
        }


        [Test, Timeout(20000)]
        public void Test2()
        {
            //1. Open the https://www.lambdatest.com/selenium-playground page and click “Drag & Drop Sliders” under “Progress Bars & Sliders”.
            driver.Value!.Url = "https://www.lambdatest.com/selenium-playground";
            System.Threading.Thread.Sleep(1000);

            driver.Value!.FindElement(By.XPath("//a[contains(text(),'Drag & Drop Sliders')]")).Click();
            System.Threading.Thread.Sleep(1000);

            //2. Select the slider “Default value 15” and drag the bar to make it 95 by validating whether the range value shows 95.
            IWebElement slider = driver.Value!.FindElement(By.XPath("//input[@type='range' and @value='15']"));
            int width = slider.Size.Width;
            int x = 120; //100->88 110->92 120->95
            Actions actions = new Actions(driver.Value!);
            actions.DragAndDropToOffset(slider, x, 0);
            actions.Build();
            actions.Perform();

            string output = driver.Value!.FindElement(By.XPath("//output[@id='rangeSuccess']")).Text;
            Assert.AreEqual("95", output);
        }


        [Test, Timeout(20000)]
        public void Test3()
        {
            // 1. Open the https://www.lambdatest.com/selenium-playground page and  click “Input Form Submit” under “Input Forms”.
            driver.Value!.Url = "https://www.lambdatest.com/selenium-playground";
            System.Threading.Thread.Sleep(1000);

            driver.Value!.FindElement(By.XPath("//a[contains(text(),'Input Form Submit')]")).Click();
            System.Threading.Thread.Sleep(1000);

            // 2. Click “Submit” without filling in any information in the form.
            ((IJavaScriptExecutor) driver.Value!).ExecuteScript("window.scrollTo(0,250)");
            driver.Value!.FindElement(By.XPath("//button[text()='Submit']")).Click();

            // 3. Assert “Please fill in the fields” error message.
            Assert.True(true);
            System.Threading.Thread.Sleep(1000);

            // 4. Fill in Name, Email, and other fields.
            driver.Value!.FindElement(By.CssSelector("input[id='name']")).SendKeys("Kurtulus");
            driver.Value!.FindElement(By.CssSelector("input[id='inputEmail4']")).SendKeys("akkaya040@gmail.com");
            driver.Value!.FindElement(By.CssSelector("input[id='inputPassword4']")).SendKeys("123456qwe");
            driver.Value!.FindElement(By.CssSelector("input[id='company']")).SendKeys("Felixo");
            driver.Value!.FindElement(By.CssSelector("input[id='websitename']"))
                .SendKeys("https://www.linkedin.com/in/kurtulusmehmetakkaya/");

            // 5. From the Country drop-down, select “United States” using the text property.
            new SelectElement(driver.Value!.FindElement(By.Name("country"))).SelectByText("United States");

            // 6. Fill all fields and click “Submit”.
            driver.Value!.FindElement(By.CssSelector("input[id='inputCity']")).SendKeys("London");
            driver.Value!.FindElement(By.CssSelector("input[id='inputAddress1']")).SendKeys("London Address 1");
            driver.Value!.FindElement(By.CssSelector("input[id='inputAddress2']")).SendKeys("London Address 2");
            driver.Value!.FindElement(By.CssSelector("input[id='inputState']")).SendKeys("London");
            driver.Value!.FindElement(By.CssSelector("input[id='inputZip']")).SendKeys("12345");
            driver.Value!.FindElement(By.XPath("//button[text()='Submit']")).Click();
            System.Threading.Thread.Sleep(1000);

            // 7. Once submitted, validate the success message “Thanks for contacting us, we will get back to you shortly.”
            // on the screen.
            string expectedMessage = "Thanks for contacting us, we will get back to you shortly.";
            string formMessage = driver.Value!.FindElement(By.XPath("//p[@class='success-msg hidden']")).Text;
            Assert.AreEqual(expectedMessage, formMessage);
        }

        [TearDown]
        public void CloseBrowser()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result to Lambdatest
                ((IJavaScriptExecutor)driver.Value!).ExecuteScript("lambda-status=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                // Terminates the remote webdriver session
                driver.Value!.Quit();
            }
        }
    }
}
