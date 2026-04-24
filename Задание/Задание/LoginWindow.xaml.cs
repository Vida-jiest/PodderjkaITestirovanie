using System;
using System.Data.SqlClient;
using System.Windows;

namespace Задание
{
    public partial class LoginWindow : Window
    {
        private string connectionString = @"Data Source=DESKTOP-KUAK6NE\SQLEXPRESS;Initial Catalog=Аптека;Integrated Security=True";

        public LoginWindow() => InitializeComponent();

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            { lblStatus.Text = "Введите логин и пароль"; return; }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ФИО, Роль FROM Пользователи WHERE Логин=@login AND Пароль=@password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fio = reader["ФИО"].ToString();
                                string role = reader["Роль"].ToString();
                                MainWindow main = new MainWindow(connectionString, fio, role);
                                main.Show();
                                this.Close();
                            }
                            else lblStatus.Text = "Неверный логин или пароль";
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}