namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "<..>";
            do
            {
                Console.Clear();
                Console.Write("Указанный путь к директории: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{path}\n");
                Console.ResetColor();
                Console.WriteLine("Выберете действие:\n <1> Указать директорию\n <2> Вычислить размер целевой папки\n <3> Выйти из приложения ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1: // Задание директории

                        path = GetPath();
                        continue;

                    case ConsoleKey.D2: // Вычисление размера целевой папки

                        Console.WriteLine();
                        Console.WriteLine("Поиск неиспользуемых папок и файлов");
                        Console.WriteLine();

                        DirectoryInfo di = new DirectoryInfo(path);
                        if (di.Exists)
                        {
                            Console.WriteLine($"Размер папки: {GetFolderSize(di)} byte ({Math.Round((double)GetFolderSize(di) / 1048576, 2)} MB)");
                        }
                        else
                        {
                            Console.WriteLine($"Директории {di.FullName} не существует");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Нажмите любую клавишу для продолжения");
                        Console.ReadKey(true);

                        continue;

                    case ConsoleKey.D3: // Выход

                        break;

                    default:

                        continue;
                }
                break;
            }
            while (true);
        }

        static string GetPath()
        {
            string path = "<..>";

            Console.Write("Укажите директорию: ");
            try
            {
                path = Path.GetFullPath(Console.ReadLine());

                if (path == "" || !new DirectoryInfo(path).Exists)
                {
                    Console.WriteLine($"Директории {path} не существует");
                    path = "<..>";
                    //continue;
                }
                else
                {
                    Console.WriteLine($"Директория {new DirectoryInfo(path).FullName} найдена!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения");
            Console.ReadKey(true);
            return path;
        }

        static long GetFolderSize(DirectoryInfo directory)
        {
            long size = 0;
            try
            {
                DirectoryInfo[] folders = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                foreach(FileInfo file in files)
                {
                    try
                    {
                        size += file.Length;
                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.WriteLine($"Файл не найден. Ошибка: {ex.Message}");
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

                foreach(DirectoryInfo folder in folders)
                {
                    size += GetFolderSize(folder);
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
            return size;
        }
    }
}