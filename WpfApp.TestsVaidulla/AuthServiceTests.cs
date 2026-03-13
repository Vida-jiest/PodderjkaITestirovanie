using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfApp.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private AuthService _authService = null!;
        
        [TestInitialize]
        public void Setup()
        {
            _authService = new AuthService();
        }

        [TestMethod]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            string username = "user";
            string password = "pass";

            bool result = _authService.Authenticate(username, password);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            string username = "invalidUser";
            string password = "invalidPass";

            bool result = _authService.Authenticate(username, password);

            Assert.IsFalse(result);
        }
    }
}