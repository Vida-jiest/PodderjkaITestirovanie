using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TaskManager_Light
{
    public partial class Form1 : Form
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public Form1()
        {
            InitializeComponent();
            SetupEventHandlers();
            // НЕ делаем поле ReadOnly!
        }

        private void SetupEventHandlers()
        {
            // Обработка нажатия Enter в поле ввода
            txtTask.KeyDown += TxtTask_KeyDown;
        }

        private void TxtTask_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddNewTask();
            }
        }

        // Добавление задачи
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddNewTask();
        }

        private void AddNewTask()
        {
            string taskText = txtTask.Text.Trim();

            // Проверка на пустую задачу
            if (string.IsNullOrWhiteSpace(taskText))
            {
                UpdateStatus("❌ Ошибка: Нельзя добавить пустую задачу!", Color.Red);
                MessageBox.Show("Введите текст задачи!", "Некорректный ввод",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на длину
            if (taskText.Length > 100)
            {
                UpdateStatus("❌ Ошибка: Задача не должна превышать 100 символов!", Color.Red);
                MessageBox.Show("Задача слишком длинная (макс. 100 символов)!", "Некорректный ввод",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Добавляем задачу
            TaskItem newTask = new TaskItem(taskText);
            tasks.Add(newTask);
            RefreshTaskList();
            txtTask.Clear();

            UpdateStatus($"✓ Задача добавлена: {taskText}", Color.Green);
            txtTask.Focus();
        }

        // Отметка выполнения
        private void BtnComplete_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedItem == null)
            {
                UpdateStatus("⚠️ Ошибка: Выберите задачу для отметки!", Color.Orange);
                MessageBox.Show("Сначала выберите задачу из списка!", "Нет выбора",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TaskItem selectedTask = listTasks.SelectedItem as TaskItem;
            if (!selectedTask.IsCompleted)
            {
                selectedTask.MarkAsCompleted();
                RefreshTaskList();
                UpdateStatus($"✓ Задача отмечена как выполненная: {selectedTask.Text}", Color.Green);
            }
            else
            {
                UpdateStatus("ℹ️ Задача уже была выполнена ранее", Color.Blue);
            }
        }

        // Удаление задачи
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedItem == null)
            {
                UpdateStatus("⚠️ Ошибка: Выберите задачу для удаления!", Color.Orange);
                MessageBox.Show("Сначала выберите задачу из списка!", "Нет выбора",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TaskItem selectedTask = listTasks.SelectedItem as TaskItem;
            string taskText = selectedTask.Text;

            // Подтверждение удаления
            DialogResult result = MessageBox.Show($"Удалить задачу:\n\"{taskText}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                tasks.Remove(selectedTask);
                RefreshTaskList();
                UpdateStatus($"✓ Задача удалена: {taskText}", Color.Green);
            }
            else
            {
                UpdateStatus("Удаление отменено", Color.Gray);
            }
        }

        // Обновление списка задач
        private void RefreshTaskList()
        {
            listTasks.Items.Clear();
            foreach (var task in tasks)
            {
                listTasks.Items.Add(task);
            }
        }

        // Обновление статусной строки
        private void UpdateStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
        }
    }

    // Класс задачи
    public class TaskItem
    {
        public string Text { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime? CompletionTime { get; private set; }

        public TaskItem(string text)
        {
            Text = text;
            IsCompleted = false;
            CreationTime = DateTime.Now;
            CompletionTime = null;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletionTime = DateTime.Now;
        }

        // Для отображения в ListBox
        public override string ToString()
        {
            string status = IsCompleted ? "✓ " : "○ ";
            string text = Text;

            if (IsCompleted && CompletionTime.HasValue)
            {
                text = Text + $" (Выполнено: {CompletionTime.Value:HH:mm dd.MM.yyyy})";
            }

            return status + text;
        }
    }
}