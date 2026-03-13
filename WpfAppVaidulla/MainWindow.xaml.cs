using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private AuthService _authService;

        public MainWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные от пользователя
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password; // Для PasswordBox используем .Password, а не .Text

            // Вызываем сервис аутентификации
            bool isAuthenticated = _authService.Authenticate(username, password);

            // Выводим результат
            if (isAuthenticated)
            {
                MessageTextBox.Text = "Успешный вход!";
                MessageTextBox.Foreground = System.Windows.Media.Brushes.Green;
                // MessageBox.Show("Успешный вход!"); // Можно и так, но в ТЗ используется TextBlock
            }
            else
            {
                MessageTextBox.Text = "Неверное имя или пароль.";
                MessageTextBox.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}