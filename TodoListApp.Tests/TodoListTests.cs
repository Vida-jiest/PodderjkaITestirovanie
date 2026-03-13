using NUnit.Framework;
using System;

namespace TodoListApp.Tests
{
    [TestFixture]
    public class TodoListTests
    {
        private TodoList _todoList;

        [SetUp]
        public void Setup()
        {
            _todoList = new TodoList();
        }

        [Test]
        public void AddTask_ShouldAddTask_WhenValidTaskIsProvided()
        {
           
            string task = "Купить молоко";

            _todoList.AddTask(task);
            var tasks = _todoList.GetTasks();

            Assert.That(tasks, Does.Contain(task));
        }

        [Test]
        public void AddTask_ShouldThrowArgumentException_WhenTaskIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _todoList.AddTask(""));
        }

        [Test]
        public void RemoveTask_ShouldRemoveTask_WhenTaskExists()
        {
            
            string task = "Купить молоко";
            _todoList.AddTask(task);

            _todoList.RemoveTask(task);
            var tasks = _todoList.GetTasks();

            Assert.That(tasks, Does.Not.Contain(task));
          
        }

        [Test]
        public void RemoveTask_ShouldThrowInvalidOperationException_WhenTaskDoesNotExist()
        {
          
            _todoList.AddTask("Купить молоко");

            Assert.Throws<InvalidOperationException>(() => _todoList.RemoveTask("Несуществующая задача"));
        }

        [Test]
        public void GetTasks_ShouldReturnAllTasks()
        {
           
            _todoList.AddTask("Купить молоко");
            _todoList.AddTask("Сделать домашку");

            var tasks = _todoList.GetTasks();

            Assert.That(tasks.Count, Is.EqualTo(2));
        }
    }
}