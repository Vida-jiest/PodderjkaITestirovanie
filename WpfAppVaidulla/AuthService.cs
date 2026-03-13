namespace WpfApp
{
    public class AuthService
    {
        // Простой метод проверки логина и пароля
        public bool Authenticate(string username, string password)
        {
            return username == "user" && password == "pass";
        }
    }
}