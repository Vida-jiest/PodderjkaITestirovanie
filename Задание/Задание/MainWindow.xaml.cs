using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Задание
{
    public partial class MainWindow : Window
    {
        private string connectionString;
        private string currentUser;
        private string currentRole;

        // Конструктор по умолчанию (на всякий случай)
        public MainWindow()
        {
            InitializeComponent();
            connectionString = @"Data Source=DESKTOP-KUAK6NE\SQLEXPRESS;Initial Catalog=Аптека;Integrated Security=True";
            currentUser = "Гость";
            currentRole = "Пользователь";
            InitializeWindow();
        }

        // Основной конструктор, вызываемый из LoginWindow
        public MainWindow(string connStr, string user, string role)
        {
            InitializeComponent();
            connectionString = connStr;
            currentUser = user;
            currentRole = role;
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            lblUserInfo.Text = $"{currentUser} ({currentRole})";
            dpStartDate.SelectedDate = DateTime.Today.AddDays(-30);
            dpEndDate.SelectedDate = DateTime.Today;
            dpReportStart.SelectedDate = DateTime.Today.AddDays(-30);
            dpReportEnd.SelectedDate = DateTime.Today;
            LoadMedicines();
            LoadCustomers();
            LoadSales();
        }

        // ---------- Вспомогательные методы ----------
        private DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn)) { da.Fill(dt); }
                }
            }
            catch (Exception ex) { ShowError("Ошибка запроса", ex.Message); }
            return dt;
        }

        private int ExecuteNonQuery(string query)
        {
            int res = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn)) { res = cmd.ExecuteNonQuery(); }
                }
            }
            catch (Exception ex) { ShowError("Ошибка выполнения", ex.Message); }
            return res;
        }

        private void ShowError(string title, string msg)
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
            lblStatus.Text = $"Ошибка: {title}";
        }

        // ---------- Лекарства ----------
        private void LoadMedicines()
        {
            DataTable dt = ExecuteQuery("SELECT * FROM Лекарства ORDER BY Название");
            dgMedicines.ItemsSource = dt.DefaultView;
            lblStatus.Text = $"Лекарств: {dt.Rows.Count}";
        }
        private void SearchMedicines(string text)
        {
            string query = $@"SELECT * FROM Лекарства WHERE Название LIKE '%{text}%' OR Производитель LIKE '%{text}%' ORDER BY Название";
            DataTable dt = ExecuteQuery(query);
            dgMedicines.ItemsSource = dt.DefaultView;
            lblStatus.Text = $"Найдено: {dt.Rows.Count}";
        }
        private void txtSearchMedicine_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchMedicine.Text)) LoadMedicines();
            else SearchMedicines(txtSearchMedicine.Text);
        }
        private void btnSearchMedicine_Click(object sender, RoutedEventArgs e) => SearchMedicines(txtSearchMedicine.Text);
        private void btnClearSearchMedicine_Click(object sender, RoutedEventArgs e) { txtSearchMedicine.Text = ""; LoadMedicines(); }
        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            var win = new MedicineWindow(connectionString, null);
            if (win.ShowDialog() == true) { LoadMedicines(); lblStatus.Text = "Лекарство добавлено"; }
        }
        private void btnEditMedicine_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicines.SelectedItem == null) { MessageBox.Show("Выберите лекарство"); return; }
            var win = new MedicineWindow(connectionString, (DataRowView)dgMedicines.SelectedItem);
            if (win.ShowDialog() == true) { LoadMedicines(); lblStatus.Text = "Лекарство обновлено"; }
        }
        private void btnDeleteMedicine_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicines.SelectedItem == null) return;
            DataRowView row = (DataRowView)dgMedicines.SelectedItem;
            string name = row["Название"].ToString();
            if (MessageBox.Show($"Удалить '{name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ExecuteNonQuery($"DELETE FROM Лекарства WHERE ID_лекарства = {row["ID_лекарства"]}");
                LoadMedicines();
                lblStatus.Text = $"Удалено: {name}";
            }
        }
        private void btnRefreshMedicine_Click(object sender, RoutedEventArgs e) => LoadMedicines();

        // ---------- Покупатели ----------
        private void LoadCustomers()
        {
            DataTable dt = ExecuteQuery("SELECT * FROM Покупатели ORDER BY ФИО");
            dgCustomers.ItemsSource = dt.DefaultView;
        }
        private void SearchCustomers(string text)
        {
            string query = $@"SELECT * FROM Покупатели WHERE ФИО LIKE '%{text}%' OR Телефон LIKE '%{text}%' ORDER BY ФИО";
            dgCustomers.ItemsSource = ExecuteQuery(query).DefaultView;
        }
        private void txtSearchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchCustomer.Text)) LoadCustomers();
            else SearchCustomers(txtSearchCustomer.Text);
        }
        private void btnSearchCustomer_Click(object sender, RoutedEventArgs e) => SearchCustomers(txtSearchCustomer.Text);
        private void btnClearSearchCustomer_Click(object sender, RoutedEventArgs e) { txtSearchCustomer.Text = ""; LoadCustomers(); }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var win = new CustomerWindow(connectionString, null);
            if (win.ShowDialog() == true) { LoadCustomers(); lblStatus.Text = "Покупатель добавлен"; }
        }
        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem == null) { MessageBox.Show("Выберите покупателя"); return; }
            var win = new CustomerWindow(connectionString, (DataRowView)dgCustomers.SelectedItem);
            if (win.ShowDialog() == true) { LoadCustomers(); lblStatus.Text = "Покупатель обновлён"; }
        }
        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem == null) return;
            DataRowView row = (DataRowView)dgCustomers.SelectedItem;
            string name = row["ФИО"].ToString();
            if (MessageBox.Show($"Удалить '{name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ExecuteNonQuery($"DELETE FROM Покупатели WHERE ID_покупателя = {row["ID_покупателя"]}");
                LoadCustomers();
                lblStatus.Text = $"Удалён: {name}";
            }
        }
        private void btnRefreshCustomer_Click(object sender, RoutedEventArgs e) => LoadCustomers();

        // ---------- Продажи ----------
        private void LoadSales()
        {
            string query = @"SELECT пр.*, л.Название AS Лекарство, п.ФИО AS Покупатель, прод.ФИО AS Продавец,
                                    пр.Количество * пр.Цена_продажи AS Сумма
                            FROM Продажи пр
                            JOIN Лекарства л ON пр.ID_лекарства = л.ID_лекарства
                            JOIN Покупатели п ON пр.ID_покупателя = п.ID_покупателя
                            JOIN Продавцы прод ON пр.ID_продавца = прод.ID_продавца
                            ORDER BY пр.Дата_продажи DESC";
            dgSales.ItemsSource = ExecuteQuery(query).DefaultView;
        }
        private void btnFilterSales_Click(object sender, RoutedEventArgs e)
        {
            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null) { MessageBox.Show("Выберите период"); return; }
            string query = $@"SELECT пр.*, л.Название AS Лекарство, п.ФИО AS Покупатель, прод.ФИО AS Продавец,
                                    пр.Количество * пр.Цена_продажи AS Сумма
                            FROM Продажи пр
                            JOIN Лекарства л ON пр.ID_лекарства = л.ID_лекарства
                            JOIN Покупатели п ON пр.ID_покупателя = п.ID_покупателя
                            JOIN Продавцы прод ON пр.ID_продавца = прод.ID_продавца
                            WHERE CAST(пр.Дата_продажи AS DATE) BETWEEN '{dpStartDate.SelectedDate:yyyy-MM-dd}' AND '{dpEndDate.SelectedDate:yyyy-MM-dd}'
                            ORDER BY пр.Дата_продажи DESC";
            dgSales.ItemsSource = ExecuteQuery(query).DefaultView;
            lblStatus.Text = $"Период: {dpStartDate.SelectedDate:dd.MM.yyyy} - {dpEndDate.SelectedDate:dd.MM.yyyy}";
        }
        private void btnClearFilter_Click(object sender, RoutedEventArgs e) { dpStartDate.SelectedDate = DateTime.Today.AddDays(-30); dpEndDate.SelectedDate = DateTime.Today; LoadSales(); }
        private void btnNewSale_Click(object sender, RoutedEventArgs e)
        {
            var win = new SaleWindow(connectionString);
            if (win.ShowDialog() == true) { LoadSales(); LoadMedicines(); lblStatus.Text = "Продажа оформлена"; }
        }
        private void btnRefreshSales_Click(object sender, RoutedEventArgs e) => LoadSales();

        // ---------- Отчёты ----------
        private void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            if (dpReportStart.SelectedDate == null || dpReportEnd.SelectedDate == null) { MessageBox.Show("Выберите период"); return; }
            string query = $@"SELECT CAST(Дата_продажи AS DATE) AS Дата,
                                    COUNT(*) AS Количество_продаж,
                                    SUM(Количество) AS Продано_единиц,
                                    SUM(Количество * Цена_продажи) AS Выручка,
                                    AVG(Количество * Цена_продажи) AS Средний_чек,
                                    COUNT(DISTINCT ID_покупателя) AS Уникальные_покупатели
                            FROM Продажи
                            WHERE CAST(Дата_продажи AS DATE) BETWEEN '{dpReportStart.SelectedDate:yyyy-MM-dd}' AND '{dpReportEnd.SelectedDate:yyyy-MM-dd}'
                            GROUP BY CAST(Дата_продажи AS DATE) ORDER BY Дата DESC";
            DataTable dt = ExecuteQuery(query);
            dgReport.ItemsSource = dt.DefaultView;
            decimal total = 0;
            foreach (DataRow row in dt.Rows) total += Convert.ToDecimal(row["Выручка"]);
            lblReportTotal.Text = $"📊 ИТОГО за период: {dt.Rows.Count} дней, {total:N2} руб.";
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}