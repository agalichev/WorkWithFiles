using FinalTask;  
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ReadValues();
            Console.ReadLine();
        }

        static void ReadValues()
        {
            string stringValue;
            int ID = 0;
            string filePath = @"C:\Users\lexga\OneDrive\Рабочий стол\Новая папка\Students.dat";
            List<Student> students = new List<Student>();

            if (File.Exists(filePath))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        var Student = (Student[])formatter.Deserialize(fs);
                        Console.WriteLine("Объект десериализован!");
                        foreach (var student in students)
                        {
                            Console.WriteLine($"{ID + 1}  Имя: {student.Name}  Группа: {student.Group}  Дата рождения: {student.DateOfBirth}");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                    //while (fs.Position < fs.Length)
                    //{
                    //    students.Add((Student) formatter.Deserialize(fs));
                    //}
            }

                //foreach (var student in students)
                //{
                //    Console.WriteLine($"{ID + 1}  Имя: {student.Name}  Группа: {student.Group}  Дата рождения: {student.DateOfBirth}");
                //}
            else
            {
                Console.WriteLine("Такого файла не существует");
            }
        }
    }

    //[Serializable]
    //public class Student
    //{
    //    public string Name { get; set; }
    //    public string Group { get; set; }
    //    public DateTime DateOfBirth { get; set; }

    //    public Student(string name, string group, DateTime dateOfBirth)
    //    {
    //        Name = name;
    //        Group = group;
    //        DateOfBirth = dateOfBirth;
    //    }
    //}
}