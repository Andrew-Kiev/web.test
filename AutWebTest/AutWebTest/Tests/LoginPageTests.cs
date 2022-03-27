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

        [TestCase("", "", "Login/Password cannot be enpty!", TestName = "Login With Empty Creds")]
        [TestCase("", "Somestring", "Login/Password cannot be enpty!", TestName = "Login With Empty Name and wrong password")]
        [TestCase("", "newyork1", "Login/Password cannot be enpty!", TestName = "Login With Empty Name and correct password")]
        [TestCase("Some*$#^$$*_+", "", "Login/Password cannot be enpty!", TestName = "Login With Empty Password and wrong login")]
        [TestCase("test", "", "Login/Password cannot be enpty!", TestName = "Login With Empty Password and correctlogin")]
        [TestCase("Some login", "Some string!1234", "Incorrect Login/Password!", TestName = "Login With Invalid Credentials")]
        [TestCase("Somelogin", "newyork1", "Incorrect Login/Password!", TestName = "Login With Invalid Name")]
        [TestCase("test", "Somestring!1234", "Incorrect Login/Password!", TestName = "Login With Invalid Password")]
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