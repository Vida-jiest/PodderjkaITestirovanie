using System;
using System.Windows.Forms;

namespace TodoListApp
{
    public partial class Form1 : Form
    {
        private readonly TodoList _todoList;

        public Form1()
        {
            InitializeComponent();
            _todoList = new TodoList();

            // Для удобства - фокус на поле ввода при запуске
            this.Load += (s, e) => txtTask.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string taskText = txtTask.Text.Trim();
                _todoList.AddTask(taskText);
                UpdateTaskList();
                txtTask.Clear();
                txtTask.Focus();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTask.Focus();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, выбран ли элемент в списке
                if (listBoxTasks.SelectedItem == null)
                {
                    MessageBox.Show("Выберите задачу для удаления.",
                        "Внимание",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Получаем выбранную задачу
                string selectedTask = listBoxTasks.SelectedItem.ToString();

                // Удаляем задачу
                _todoList.RemoveTask(selectedTask);

                // Обновляем список
                UpdateTaskList();

                // Если остались задачи, снимаем выделение
                if (listBoxTasks.Items.Count > 0)
                {
                    listBoxTasks.SelectedIndex = -1;
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateTaskList()
        {
            // Сохраняем выбранный элемент (если есть)
            string selectedTask = listBoxTasks.SelectedItem?.ToString();

            // Очищаем и обновляем список
            listBoxTasks.Items.Clear();
            var tasks = _todoList.GetTasks();

            foreach (var task in tasks)
            {
                listBoxTasks.Items.Add(task);
            }

            // Восстанавливаем выделение, если задача все еще существует
            if (!string.IsNullOrEmpty(selectedTask) && tasks.Contains(selectedTask))
            {
                listBoxTasks.SelectedItem = selectedTask;
            }
        }

        // Добавляем обработчик клавиши Enter
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && txtTask.Focused)
            {
                btnAdd_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}