using NUnit.Framework;

[TestFixture]
public class StudentManagerTests
{
    private StudentManager studentManager;

    [SetUp]
    public void Setup()
    {
        studentManager = new StudentManager();
    }

    [Test]
    public void AddStudent_ShouldAddStudent()
    {
        var student = new Student("Alice", 1);
        studentManager.AddStudent(student);

        var result = studentManager.GetStudent(1);
        Assert.That(result.Name, Is.EqualTo("Alice")); // Исправлено
    }

    [Test]
    public void RemoveStudent_ShouldRemoveStudent()
    {
        var student = new Student("Bob", 2);
        studentManager.AddStudent(student);
        studentManager.RemoveStudent(2);

        var result = studentManager.GetStudent(2);
        Assert.That(result, Is.Null); // Исправлено
    }

    [Test]
    public void GetAllStudents_ShouldReturnAllStudents()
    {
        studentManager.AddStudent(new Student("Alice", 1));
        studentManager.AddStudent(new Student("Bob", 2));

        var students = studentManager.GetAllStudents();
        Assert.That(students.Count, Is.EqualTo(2)); // Исправлено
    }
}