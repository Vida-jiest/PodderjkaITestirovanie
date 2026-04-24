using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Задание
{
    public partial class SaleWindow : Window
    {
        private string connectionString;
        private DataTable medicinesTable;
        private DataTable customersTable;
        private DataTable sellersTable;
        private DataTable prescriptionsTable;
        private decimal currentPrice;

        public SaleWindow(string connStr)
        {
            InitializeComponent();
            connectionString = connStr;
            cbPaymentMethod.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string medQuery = "SELECT ID_лекарства, Название, Цена, Количество FROM Лекарства WHERE Количество > 0 ORDER BY Название";
                    medicinesTable = new DataTable();
                    new SqlDataAdapter(medQuery, conn).Fill(medicinesTable);
                    cbMedicine.ItemsSource = medicinesTable.DefaultView;

                    string custQuery = "SELECT ID_покупателя, ФИО FROM Покупатели ORDER BY ФИО";
                    customersTable = new DataTable();
                    new SqlDataAdapter(custQuery, conn).Fill(customersTable);
                    cbCustomer.ItemsSource = customersTable.DefaultView;

                    string sellerQuery = "SELECT ID_продавца, ФИО FROM Продавцы ORDER BY ФИО";
                    sellersTable = new DataTable();
                    new SqlDataAdapter(sellerQuery, conn).Fill(sellersTable);
                    cbSeller.ItemsSource = sellersTable.DefaultView;

                    string presQuery = "SELECT ID_рецепта, Номер_рецепта FROM Рецепты WHERE Статус = 'Активен' ORDER BY Номер_рецепта";
                    prescriptionsTable = new DataTable();
                    new SqlDataAdapter(presQuery, conn).Fill(prescriptionsTable);
                    cbPrescription.ItemsSource = prescriptionsTable.DefaultView;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка загрузки: {ex.Message}"); }
        }

        private void cbMedicine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMedicine.SelectedItem != null)
            {
                DataRowView row = (DataRowView)cbMedicine.SelectedItem;
                currentPrice = Convert.ToDecimal(row["Цена"]);
                int stock = Convert.ToInt32(row["Количество"]);
                txtPrice.Text = currentPrice.ToString("N2");
                lblStock.Text = $"В наличии: {stock} шт.";
                CalculateTotal();
            }
        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e) => CalculateTotal();

        private void CalculateTotal()
        {
            if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(txtQuantity.Text, out int qty))
                txtTotal.Text = (price * qty).ToString("N2");
            else
                txtTotal.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbMedicine.SelectedItem == null || cbCustomer.SelectedItem == null || cbSeller.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Некорректное количество");
                return;
            }

            DataRowView medRow = (DataRowView)cbMedicine.SelectedItem;
            int stock = Convert.ToInt32(medRow["Количество"]);
            if (quantity > stock)
            {
                MessageBox.Show($"Недостаточно товара. В наличии: {stock} шт.");
                return;
            }

            int medicineId = Convert.ToInt32(medRow["ID_лекарства"]);
            int customerId = Convert.ToInt32(((DataRowView)cbCustomer.SelectedItem)["ID_покупателя"]);
            int sellerId = Convert.ToInt32(((DataRowView)cbSeller.SelectedItem)["ID_продавца"]);
            int? prescriptionId = (cbPrescription.SelectedItem != null) ? (int?)Convert.ToInt32(((DataRowView)cbPrescription.SelectedItem)["ID_рецепта"]) : null;
            string paymentMethod = (cbPaymentMethod.SelectedItem as ComboBoxItem)?.Content.ToString();
            string checkNumber = "ЧЕК-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertSale = @"INSERT INTO Продажи 
                        (Номер_чека, ID_лекарства, ID_покупателя, ID_продавца, ID_рецепта, Количество, Цена_продажи, Способ_оплаты, Дата_продажи)
                        VALUES (@check, @medId, @custId, @sellId, @prescId, @qty, @price, @pay, @date)";
                    using (SqlCommand cmd = new SqlCommand(insertSale, conn))
                    {
                        cmd.Parameters.AddWithValue("@check", checkNumber);
                        cmd.Parameters.AddWithValue("@medId", medicineId);
                        cmd.Parameters.AddWithValue("@custId", customerId);
                        cmd.Parameters.AddWithValue("@sellId", sellerId);
                        cmd.Parameters.AddWithValue("@prescId", prescriptionId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@qty", quantity);
                        cmd.Parameters.AddWithValue("@price", currentPrice);
                        cmd.Parameters.AddWithValue("@pay", paymentMethod);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    // обновить остаток
                    string updateStock = $"UPDATE Лекарства SET Количество = Количество - {quantity} WHERE ID_лекарства = {medicineId}";
                    using (SqlCommand cmd = new SqlCommand(updateStock, conn)) cmd.ExecuteNonQuery();

                    // если использован рецепт, пометить использованным
                    if (prescriptionId.HasValue)
                    {
                        string updatePresc = $"UPDATE Рецепты SET Статус = 'Использован' WHERE ID_рецепта = {prescriptionId}";
                        using (SqlCommand cmd = new SqlCommand(updatePresc, conn)) cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show($"Продажа оформлена!\nЧек: {checkNumber}", "Успех");
                DialogResult = true;
                Close();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}