using AutWebTest.Helpers;
using AutWebTest.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutWebTest.Tests
{
    public class LoginPageTests
    {
        private IWebDriver driver;
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions { AcceptInsecureCertificates = true };  // needed to open HTTPS pages without certificate
            options.AddArgument("log-level=3");                                     // only log errors in github pipelines
            options.AddArgument("--silent");                                        // less info messages in github pipelines

            driver = new ChromeDriver(options);
            driver.Url = "https://localhost:5001/";
            loginPage = new LoginPage(driver);
        }

        [TearDown]
        public void TearDown() => driver.Dispose();

        [Test]
        public void LoginWithEmptyField()
        {
            loginPage.Login(string.Empty, string.Empty);
            Assert.AreEqual("User not found!", loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithEmptyName()
        {

            loginPage.Login(Helper.RandomString(10), string.Empty);
            Assert.AreEqual("User not found!", loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithEmptyPass()
        {
            loginPage.Login(string.Empty, Helper.RandomString(10));
            Assert.AreEqual("User not found!", loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithInvalidCredentials()
        {
            loginPage.Login(Helper.RandomString(10), Helper.RandomString(10));
            Assert.AreEqual("User not found!", loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithInvalidName()
        {
            loginPage.Login(Helper.RandomString(10), "newyork1");
            Assert.AreEqual("Incorrect user name!", loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithInvalidPass()
        {
            loginPage.Login("test", Helper.RandomString(10));
            Assert.AreEqual("Incorrect password!", loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithValidCredentials()
        {
            loginPage.Login("test", "newyork1");
            Assert.AreEqual("https://localhost:5001/Calculator", driver.Url);
        }
    }
}