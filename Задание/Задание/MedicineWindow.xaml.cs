using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Задание
{
    public partial class MedicineWindow : Window
    {
        private string connectionString;
        private DataRowView medicineRow;
        private DataTable categoriesTable;
        private DataTable suppliersTable;

        public MedicineWindow(string connStr, DataRowView row)
        {
            InitializeComponent();
            connectionString = connStr;
            medicineRow = row;

            dpArrivalDate.SelectedDate = DateTime.Today;
            dpExpDate.SelectedDate = DateTime.Today.AddYears(2);

            LoadCategoriesAndSuppliers();

            if (row != null)
            {
                Title = "Редактирование лекарства";
                LoadMedicineData();
            }
            else
                Title = "Добавление лекарства";
        }

        private void LoadCategoriesAndSuppliers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string catQuery = "SELECT ID_категории, Название FROM Категории";
                    categoriesTable = new DataTable();
                    new SqlDataAdapter(catQuery, conn).Fill(categoriesTable);
                    cbCategory.ItemsSource = categoriesTable.DefaultView;

                    string supQuery = "SELECT ID_поставщика, Название FROM Поставщики";
                    suppliersTable = new DataTable();
                    new SqlDataAdapter(supQuery, conn).Fill(suppliersTable);
                    cbSupplier.ItemsSource = suppliersTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочников: {ex.Message}");
            }
        }

        private void LoadMedicineData()
        {
            if (medicineRow == null) return;
            txtName.Text = medicineRow["Название"].ToString();
            txtManufacturer.Text = medicineRow["Производитель"].ToString();
            txtPrice.Text = medicineRow["Цена"].ToString();
            txtQuantity.Text = medicineRow["Количество"].ToString();

            SetComboBoxValue(cbType, medicineRow["Тип"].ToString());
            SetComboBoxValue(cbKind, medicineRow["Вид"].ToString());

            if (medicineRow["ID_категории"] != DBNull.Value)
            {
                int catId = Convert.ToInt32(medicineRow["ID_категории"]);
                foreach (DataRowView r in cbCategory.Items)
                    if (Convert.ToInt32(r["ID_категории"]) == catId) { cbCategory.SelectedItem = r; break; }
            }
            if (medicineRow["ID_поставщика"] != DBNull.Value)
            {
                int supId = Convert.ToInt32(medicineRow["ID_поставщика"]);
                foreach (DataRowView r in cbSupplier.Items)
                    if (Convert.ToInt32(r["ID_поставщика"]) == supId) { cbSupplier.SelectedItem = r; break; }
            }

            if (medicineRow["Срок_годности"] != DBNull.Value)
                dpExpDate.SelectedDate = Convert.ToDateTime(medicineRow["Срок_годности"]);
            if (medicineRow["Дата_поступления"] != DBNull.Value)
                dpArrivalDate.SelectedDate = Convert.ToDateTime(medicineRow["Дата_поступления"]);
        }

        private void SetComboBoxValue(ComboBox cb, string value)
        {
            foreach (ComboBoxItem item in cb.Items)
                if (item.Content.ToString() == value) { cb.SelectedItem = item; break; }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Заполните обязательные поля (*)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            { MessageBox.Show("Некорректная цена"); return; }
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            { MessageBox.Show("Некорректное количество"); return; }

            string type = (cbType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string kind = (cbKind.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            int? catId = (cbCategory.SelectedItem != null) ? (int?)Convert.ToInt32(((DataRowView)cbCategory.SelectedItem)["ID_категории"]) : null;
            int? supId = (cbSupplier.SelectedItem != null) ? (int?)Convert.ToInt32(((DataRowView)cbSupplier.SelectedItem)["ID_поставщика"]) : null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (medicineRow == null)  // добавление
                    {
                        string query = @"INSERT INTO Лекарства 
                            (Название, Тип, Вид, Производитель, Цена, Количество, Срок_годности, Дата_поступления, ID_категории, ID_поставщика)
                            VALUES (@name, @type, @kind, @manufacturer, @price, @quantity, @expDate, @arrivalDate, @catId, @supId)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                            SetParams(cmd, type, kind, catId, supId, price, quantity);
                    }
                    else  // редактирование
                    {
                        string query = @"UPDATE Лекарства SET 
                            Название=@name, Тип=@type, Вид=@kind, Производитель=@manufacturer,
                            Цена=@price, Количество=@quantity, Срок_годности=@expDate,
                            Дата_поступления=@arrivalDate, ID_категории=@catId, ID_поставщика=@supId
                            WHERE ID_лекарства=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", medicineRow["ID_лекарства"]);
                            SetParams(cmd, type, kind, catId, supId, price, quantity);
                        }
                    }
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка сохранения: {ex.Message}"); }
        }

        private void SetParams(SqlCommand cmd, string type, string kind, int? catId, int? supId, decimal price, int quantity)
        {
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@kind", kind);
            cmd.Parameters.AddWithValue("@manufacturer", txtManufacturer.Text);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@expDate", dpExpDate.SelectedDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@arrivalDate", dpArrivalDate.SelectedDate ?? DateTime.Today);
            cmd.Parameters.AddWithValue("@catId", catId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@supId", supId ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}