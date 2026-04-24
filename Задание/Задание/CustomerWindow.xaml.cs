using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Задание
{
    public partial class CustomerWindow : Window
    {
        private string connectionString;
        private DataRowView customerRow;

        public CustomerWindow(string connStr, DataRowView row)
        {
            InitializeComponent();
            connectionString = connStr;
            customerRow = row;
            if (row != null)
            {
                Title = "Редактирование покупателя";
                LoadCustomerData();
            }
            else Title = "Добавление покупателя";
        }

        private void LoadCustomerData()
        {
            if (customerRow == null) return;
            txtFullName.Text = customerRow["ФИО"].ToString();
            txtPhone.Text = customerRow["Телефон"].ToString();
            txtEmail.Text = customerRow["Email"].ToString();
            txtAddress.Text = customerRow["Адрес"].ToString();
            txtCity.Text = customerRow["Город"].ToString();
            if (customerRow["Дата_рождения"] != DBNull.Value)
                dpBirthDate.SelectedDate = Convert.ToDateTime(customerRow["Дата_рождения"]);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Заполните ФИО и телефон", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (customerRow == null)  // добавление
                    {
                        string query = @"INSERT INTO Покупатели (ФИО, Телефон, Email, Адрес, Город, Дата_рождения)
                                         VALUES (@fio, @phone, @email, @address, @city, @birth)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                            SetParams(cmd);
                    }
                    else  // редактирование
                    {
                        string query = @"UPDATE Покупатели SET ФИО=@fio, Телефон=@phone, Email=@email,
                                         Адрес=@address, Город=@city, Дата_рождения=@birth
                                         WHERE ID_покупателя=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", customerRow["ID_покупателя"]);
                            SetParams(cmd);
                        }
                    }
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void SetParams(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@fio", txtFullName.Text);
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@city", txtCity.Text);
            cmd.Parameters.AddWithValue("@birth", dpBirthDate.SelectedDate ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}