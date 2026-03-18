using System.Collections.Generic;

public class Student
{
    public string Name { get; set; }
    public int Id { get; set; }

    public Student(string name, int id)
    {
        Name = name;
        Id = id;
    }
}

public class StudentManager
{
    private List<Student> students = new List<Student>();

    public void AddStudent(Student student)
    {
        students.Add(student);
    }

    public void RemoveStudent(int id)
    {
        var student = students.Find(s => s.Id == id);
        if (student != null)
        {
            students.Remove(student);
        }
    }

    public Student GetStudent(int id)
    {
        return students.Find(s => s.Id == id);
    }

    public List<Student> GetAllStudents()
    {
        return new List<Student>(students);
    }
}