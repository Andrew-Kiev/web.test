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

        [TestCase("", "", "User not found!", TestName = "Login With Empty Creds")]
        [TestCase("", "Somestring", "User not found!", TestName = "Login With Empty Name")]
        [TestCase("Some*$#^$$*_+", "", "User not found!", TestName = "Login With Empty Password")]
        [TestCase("Some login", "Some string!1234", "User not found!", TestName = "Login With Invalid Credentials")]
        [TestCase("Somelogin", "newyork1", "Incorrect user name!", TestName = "Login With Invalid Name")]
        [TestCase("test", "Somestring!1234", "Incorrect password!", TestName = "Login With Invalid Password")]
        public void InvalidOrEmptyCreds(string login, string password, string expectedError)
        {
            loginPage.Login(login, password);
            Assert.AreEqual(expectedError, loginPage.ErrorMessage);
        }

        [Test]
        public void LoginWithValidCredentials()
        {
            loginPage.Login("test", "newyork1");
            Assert.AreEqual("https://localhost:5001/Calculator", driver.Url);
        }
    }
}