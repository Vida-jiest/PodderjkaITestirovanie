using System;
using System.Collections.Generic;

public class TodoList
{
    private readonly List<string> _tasks;

    public TodoList()
    {
        _tasks = new List<string>();
    }

    public void AddTask(string task)
    {
        if (string.IsNullOrWhiteSpace(task))
            throw new ArgumentException("Задача не может быть пустой.");
        _tasks.Add(task);
    }

    public void RemoveTask(string task)
    {
        if (!_tasks.Remove(task))
            throw new InvalidOperationException("Задача не найдена.");
    }

    public List<string> GetTasks()
    {
        return new List<string>(_tasks);
    }

    // Добавьте этот метод для тестов
    public bool IsTaskExist(string task)
    {
        return _tasks.Contains(task);
    }
}