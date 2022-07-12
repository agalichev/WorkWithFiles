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
                Console.WriteLine("Выберете действие:\n <1> Указать директорию\n <2> Удалить папки и файлы, неиспользуемые более 30 минут, в указанной директории\n <3> Показать все файлы и подпапки и их время доступа в указанной директории\n <4> Выйти из приложения ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1: // Задание директории

                        path = GetPath();
                        continue;

                    case ConsoleKey.D2: // Удаление папок и файлов

                        Console.WriteLine();
                        Console.WriteLine("Поиск неиспользуемых папок и файлов");
                        Console.WriteLine();

                        DirectoryInfo diA = new DirectoryInfo(path);
                        //if (diA.Exists)
                        //{
                        //    if (diA.GetDirectories().Length != 0)
                        //    {
                        //        CleanDirectory(diA);
                        //    }
                        //    else if (diA.GetDirectories().Length == 0 && diA.GetFiles().Length == 0)
                        //    {
                        //        Console.WriteLine("Папка пуста");
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine($"Директории {diA.FullName} не существует");
                        //}
                        if (diA.Exists)
                        {
                            if (diA.GetDirectories().Length == 0 && diA.GetFiles().Length == 0)
                            {
                                Console.WriteLine("Папка пуста");
                                //folderSize = GetFolderSize(diA);
                            }
                            else
                            {
                                CleanFolder(diA);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Директории {diA.FullName} не существует");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Нажмите любую клавишу для продолжения");
                        Console.ReadKey(true);
                        continue;

                    case ConsoleKey.D3: // Время доступа всех файлов и папок в указанной директории, в том числе и подпапок

                        Console.WriteLine();
                        Console.WriteLine("Иерархия целевой папки:");
                        Console.WriteLine();

                        DirectoryInfo di = new DirectoryInfo(path);
                        //if (di.Exists)
                        //{
                        //    if (di.GetDirectories().Length != 0)
                        //    {
                        //        ShowLastAccessTime(di);
                        //    }
                        //    else if (di.GetDirectories().Length == 0 && di.GetFiles().Length == 0)
                        //    {
                        //        Console.WriteLine("Папка пуста");
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine($"Директории {di.FullName} не существует");
                        //}
                        if (di.Exists)
                        {
                            if (di.GetDirectories().Length == 0 && di.GetFiles().Length == 0)
                            {
                                Console.WriteLine("Папка пуста");
                            }
                            else
                            {

                                ShowLastAccessTime(di);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Директории {di.FullName} не существует");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Нажмите любую клавишу для продолжения");
                        Console.ReadKey(true);
                        continue;

                    case ConsoleKey.D4: // Выход

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

        static void CleanFolder(DirectoryInfo directory)
        {
            try
            {
                DirectoryInfo[] di = directory.GetDirectories();
                FileInfo[] fi = directory.GetFiles();

                foreach (var subfolder in di)
                {
                    try
                    {
                        if (DateTime.Now.Subtract(subfolder.LastAccessTime) > TimeSpan.FromMinutes(30))
                        {
                            Console.WriteLine($"Удаление папки {subfolder.Name}!\n");
                            SetFileAttributes(subfolder);
                            subfolder.Delete(true);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"Папка {subfolder.Name} ещё используется!\n");
                            CleanFolder(subfolder);
                        }
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Console.WriteLine($"Директория не найдена. Ошибка: {ex.Message}");
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

                // Поиск файлов
                foreach (FileInfo f in fi)
                {
                    try
                    {
                        if (DateTime.Now.Subtract(f.LastAccessTime) > TimeSpan.FromMinutes(30))
                        {
                            Console.WriteLine($"Удаление файла {f.Name}!\n");
                            f.Delete();
                        }
                        else
                        {
                            Console.WriteLine($"Файл {f.Name} ещё используется!\n");
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

        static void ShowLastAccessTime(DirectoryInfo directory)
        {
            try
            {
                var folders = directory.GetDirectories();
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    Console.WriteLine($"{file.Name}: {file.LastAccessTime}");
                }
                Console.WriteLine();
                foreach (var folder in folders)
                {
                    Console.WriteLine($"{folder.Name}: {folder.LastAccessTime}");
                    ShowLastAccessTime(folder);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Директория не существует. Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // Метод для условия: при удалении каталога не должно возникнуть проблем (так я это понимаю)
        static void SetFileAttributes(DirectoryInfo directory)
        {
            try
            {
                var folders = directory.GetDirectories();
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    var filePath = file.FullName;
                    FileAttributes attributes = File.GetAttributes(filePath);
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        Console.WriteLine($"Установка атрибутов по умолчанию файлу {file.Name}");
                        File.SetAttributes(filePath, FileAttributes.Normal);
                    }               
                }
                Console.WriteLine();
                foreach (var folder in folders)
                {
                    SetFileAttributes(folder);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Директория не существует. Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}