namespace Task1
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
                Console.WriteLine("Выберете действие:\n <F1> Указать директорию\n <F2> Удалить папки и файлы, неиспользуемые более 30 минут, в указанной директории\n <F3> Показать все файлы и подпапки и их время доступа в указанной директории\n <F4> Выйти из приложения ");
                
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.F1: // Задание директории

                        path = GetPath();
                        continue;

                    case ConsoleKey.F2: // Удаление папок и файлов

                        Console.WriteLine();
                        Console.WriteLine("Поиск неиспользуемых папок и файлов");
                        Console.WriteLine();

                        try
                        {
                            DirectoryInfo diA = new DirectoryInfo(path);
                            CleanDirectory(diA);
                        }
                        catch(DirectoryNotFoundException ex)
                        {
                            Console.WriteLine($"Директория не найдена. Ошибка: {ex.Message}");
                        }
                        catch(UnauthorizedAccessException ex)
                        {
                            Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        Console.WriteLine("Нажмите любую клавишу для продолжения");
                        Console.ReadKey(true);
                        continue;

                    case ConsoleKey.F3: // Время доступа всех файлов и папок в указанной директории, в том числе и подпапок

                        Console.WriteLine();
                        Console.WriteLine("Подкаталоги указанной директории:");
                        Console.WriteLine();

                        DirectoryInfo di = new DirectoryInfo(path);
                        ShowLastAccessTime(di);
                        Console.WriteLine("Нажмите любую клавишу для продолжения");
                        Console.ReadKey(true);
                        continue;

                    case ConsoleKey.F4: // Выход

                        break;

                    default:

                        continue;
                }
                break;
            }
            while(true);
        }

        static string GetPath()
        {
            string path = "<..>";

            Console.Write("Укажите директорию: ");
            //try
            //{
            path = Path.GetFullPath(Console.ReadLine()); 

            if (path == "" || !new DirectoryInfo(path).Exists)
            {
                Console.WriteLine("Несуществующий путь к каталогу");
                path = "<..>";
                    //continue;
            }
            else
            {
                //DirectoryInfo dirInfo = new DirectoryInfo(path);
                Console.WriteLine($"Директория найдена: {new DirectoryInfo(path).FullName}");
            }
               
            //}
            //catch(DirectoryNotFoundException ex)
            //{
            //    Console.WriteLine($"Директория не найдена. Ошибка: {ex.Message}");
            //}
            //catch(UnauthorizedAccessException ex)
            //{
            //    Console.WriteLine($"Отстствует доступ. Ошибка: {ex.Message}");
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine($"Произошла ошибка: {ex.Message}");
            //}

            Console.WriteLine("Нажмите любую клавишу для продолжения");
            Console.ReadKey(true);
            return path;
        }

        static void CleanDirectory(DirectoryInfo directory)
        {
            DirectoryInfo[] di = directory.GetDirectories();
            FileInfo[] fi = directory.GetFiles();
            
            // За счёт рекурсии сначала происходит обнаружение и проверка всех файлов в указанной папке и всех подпапках

            foreach (FileInfo f in fi)
            {
                try
                {
                    if (DateTime.Now.Subtract(File.GetLastAccessTime(f.FullName)) > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine($"Файл {f.Name} удалён!");
                        f.Delete();
                    }
                    else
                    {
                        Console.WriteLine($"Файл {f.Name} используется!");
                    }
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

            // После завершения рекурсии происходит проверка всех подпапок

            foreach(var subfolder in di)
            {
                CleanDirectory(subfolder);

                try
                {
                    if (DateTime.Now.Subtract(Directory.GetLastAccessTime(subfolder.FullName)) > TimeSpan.FromMinutes(30) && subfolder.GetDirectories().Length == 0 && subfolder.GetFiles().Length == 0)
                    {
                        Console.WriteLine($"Каталог {subfolder.Name} удалён!");
                        subfolder.Delete();
                    }
                    else
                    {
                        Console.WriteLine($"Папка {subfolder.Name} ещё используется!");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void ShowLastAccessTime(DirectoryInfo directory)
        {
            var folders = directory.GetDirectories();
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                Console.WriteLine($"{file.Name}: {file.LastAccessTime}");
            }

            foreach (var folder in folders)
            {
                //Console.WriteLine();
                Console.WriteLine($"\n{folder.Name}: {folder.LastAccessTime}\n");
                ShowLastAccessTime(folder);
            }
        }
    }
}