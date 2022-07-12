using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadBinaryFile();
            Console.ReadLine();
        }


        static void ReadBinaryFile()
        {
            int ID = 0;
            //string sourceFile = @"C:\Users\lexga\OneDrive\Рабочий стол\Новая папка\Students.dat";
            string sourceFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students.dat";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students"; 
            DirectoryInfo dir = new DirectoryInfo(path);

            //if (!Directory.Exists(path))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(path);
            //    }
            //    catch (UnauthorizedAccessException ex)
            //    {
            //        Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Ошибка: {ex.Message}");
            //    }
            //}
            if (!dir.Exists)
            {
                try
                {
                    Console.WriteLine($"Создание папки {dir.Name}");
                    dir.Create();
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    Console.WriteLine($"Очистка папки {dir.Name} от файлов!");
                    FileInfo[] files = dir.GetFiles();
                    try
                    {
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                catch(UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            if (File.Exists(sourceFilePath))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (var fs = new FileStream(sourceFilePath, FileMode.Open))
                    {
                        var students = (Student[])formatter.Deserialize(fs);
                        Console.WriteLine("Десериализация завершена успешно!\nСодержимое файла:\n-----------------");
                        
                        List<string> groups = new List<string>();

                        // Выводим содержимое файла отдельно для удобства (можно и в одном цикле foreach, но так показалось удобнее)
                        foreach (var student in students)
                        {
                            Console.WriteLine($"{ID + 1}  Имя: {student.Name}  Группа: {student.Group}  Дата рождения: {student.DateOfBirth}");

                            FileInfo groupFile = new FileInfo(Path.Combine(path, student.Group + ".txt"));

                            if (!groups.Contains(student.Group))
                            {
                                groups.Add(student.Group);
                            }
                            ID++;
                        }

                        Console.WriteLine();

                        foreach (var group in groups)
                        {
                            FileInfo groupFile = new FileInfo(Path.Combine(path, group + ".txt"));
                            Console.WriteLine($"Создаём файл {groupFile.Name}");

                            foreach(var student in students)
                            {
                                if (student.Group == group)
                                {
                                    using (StreamWriter sw = groupFile.AppendText())
                                    {
                                        Console.WriteLine($"Добавляем запись в файл {groupFile.Name}!");
                                        sw.WriteLine($"{student.Name} {student.DateOfBirth}");
                                    }
                                }
                            }
                        }


                        //foreach (var student in students)
                        //{
                        //    foreach (var group in groups)
                        //    {
                        //        FileInfo groupFile = new FileInfo(Path.Combine(path, group + ".txt"));/*Path.Combine(path, group + ".txt");*/

                        //        if (!groupFile.Exists)
                        //        {
                        //            Console.WriteLine($"Создаём файл {groupFile.Name} и добавляем в него запись!");
                        //            using (StreamWriter sw = groupFile.CreateText())
                        //            {
                        //                sw.WriteLine($"{student.Name} {student.DateOfBirth}");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            using (StreamWriter sw = groupFile.AppendText())
                        //            {
                        //                Console.WriteLine($"Добавляем запись в файл {groupFile.Name}!");
                        //                sw.WriteLine($"{student.Name} {student.DateOfBirth}");
                        //            }
                        //        }


                        //        //else if (groupFile.Exists)
                        //        //{
                        //        //    Console.WriteLine($"Удаляем старый файл {groupFile.Name} и создаём новый!");
                        //        //    try
                        //        //    {
                        //        //        groupFile.Delete();
                        //        //    }
                        //        //    catch (Exception ex)
                        //        //    {
                        //        //        Console.WriteLine($"Ошибка: {ex.Message}");
                        //        //    }
                        //        //}
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Такого файла не существует");
            }
        }
    }

    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Student(string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }
    }
   
}