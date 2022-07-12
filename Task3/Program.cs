namespace Task3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "<..>";
            long folderSize = 0;
            long newFolderSize = 0;
            do
            {
                Console.Clear();
                Console.Write("Указанный путь к директории: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{path}");
                Console.ResetColor();
                Console.WriteLine($"Исходный размер папки: {folderSize} байт");
                Console.WriteLine($"Текущий размер папки: {newFolderSize} байт\n");
                Console.WriteLine("Выберете действие:\n <1> Указать директорию\n <2> Удалить папки и файлы, неиспользуемые более 30 минут, в указанной директории\n <3> Показать все файлы и подпапки и их время доступа в указанной директории\n <4> Выйти из приложения ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1: // Задание директории

                        path = GetPath();
                        folderSize = GetFolderSize(new DirectoryInfo(path));
                        newFolderSize = GetFolderSize(new DirectoryInfo(path));
                        continue;

                    case ConsoleKey.D2: // Удаление папок и файлов

                        Console.WriteLine();
                        Console.WriteLine("Поиск неиспользуемых папок и файлов");
                        Console.WriteLine();

                        long freedSpace = 0;
                        int fileCount = 0;

                        DirectoryInfo diA = new DirectoryInfo(path);
                        if (diA.Exists)
                        {
                            if (diA.GetDirectories().Length == 0 && diA.GetFiles().Length == 0)
                            {
                                Console.WriteLine("Папка пуста");
                                //folderSize = GetFolderSize(diA);
                            }
                            else
                            {
                                newFolderSize = CleanFolder(diA, ref fileCount, ref freedSpace);
                                //Console.WriteLine($"Итого\n-----\n Удалено файлов: {fileCount}\n Освобождено места: {folderSize - newFolderSize} байт");
                                Console.WriteLine($"Итого\n-----\n Удалено файлов: {fileCount}\n Освобождено места: {freedSpace} байт");
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

        static long CleanFolder(DirectoryInfo directory, ref int fileCount, ref long freedSpace)
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
                            int temp = 0;
                            long subFolderSize = GetFolderSize(subfolder); // Получаем размер удаляемой подпапки
                            Console.WriteLine($"Удаление папки {subfolder.Name} размером {subFolderSize} байт!");
                            temp += GetFilesCount(subfolder); // Определяем количество удаляемых файлов в указанной директории 
                            SetFileAttributes(subfolder);
                            subfolder.Delete(true);
                            Console.WriteLine("Удаление завершено!\n");
                            freedSpace += subFolderSize;
                            fileCount += temp; // Определяем количество удаляемых файлов в указанной директории 
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"Папка {subfolder.Name} ещё используется!\n");
                            CleanFolder(subfolder, ref fileCount, ref freedSpace/*,ref folderSize*/);

                        }
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Console.WriteLine($"Директория не найдена. Ошибка: {ex.Message}\n");
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}\n");
                    }
                }

                foreach (FileInfo f in fi)
                {
                    try
                    {
                        if (DateTime.Now.Subtract(f.LastAccessTime) > TimeSpan.FromMinutes(30))
                        {
                            long fileSize = f.Length;
                            Console.WriteLine($"\tУдаление файла {f.Name} размером: {f.Length} байт!");
                            f.Delete();
                            Console.WriteLine("\tУдаление завершено!\n");
                            freedSpace += fileSize;
                            fileCount++;
                        }
                        else
                        {
                            Console.WriteLine($"\tФайл {f.Name} ещё используется!\n");
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.WriteLine($"Файл не найден. Ошибка: {ex.Message}\n");
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}\n");
                    }

                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
            return GetFolderSize(directory);
        }

        //static long CleanFolder(DirectoryInfo directory, ref int fileCount, ref long freedSpace)
        //{
        //    //long folderSize = 0;
        //    //long freedMemory = 0;
        //    // long subFolderSize = 0;
        //    //int delFileCount = 0;
        //    //fileCount = 0;

        //    try
        //    {
        //        DirectoryInfo[] di = directory.GetDirectories();
        //        FileInfo[] fi = directory.GetFiles();
        //        //folderSize = GetFolderSize(directory);

        //        foreach (var subfolder in di)
        //        {
        //            try
        //            {
        //                if (DateTime.Now.Subtract(subfolder.LastAccessTime) > TimeSpan.FromMinutes(6))
        //                {
        //                    long subFolderSize = GetFolderSize(subfolder); // Получаем размер удаляемой подпапки
        //                    Console.WriteLine($"Удаление папки {subfolder.Name} размером {subFolderSize} байт!\n");
        //                    fileCount += GetFilesCount(subfolder); // Определяем количество удаляемых файлов в указанной директории 
        //                    subfolder.Delete(true);
        //                    //freedSpace += subFolderSize;
        //                    //Console.WriteLine($"Освобождено памяти: {subFolderSize} байт\n");
        //                    //folderSize -= subFolderSize;
        //                    //folderSize = GetFolderSize(directory);
        //                    continue;
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"Папка {subfolder.Name} ещё используется!\n");
        //                    CleanFolder(subfolder, ref fileCount, ref freedSpace/*,ref folderSize*/);

        //                }
        //            }
        //            catch (DirectoryNotFoundException ex)
        //            {
        //                Console.WriteLine($"Директория не найдена. Ошибка: {ex.Message}");
        //            }
        //            catch (UnauthorizedAccessException ex)
        //            {
        //                Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Ошибка: {ex.Message}");
        //            }
        //        }
        //        //folderSize = GetFolderSize(directory);

        //        // Поиск файлов
        //        //if(directory.Exists)
        //        //{
        //        foreach (FileInfo f in fi)
        //        {
        //            try
        //            {
        //                if (DateTime.Now.Subtract(f.LastAccessTime) > TimeSpan.FromMinutes(3))
        //                {
        //                    Console.WriteLine($"\tУдаление файла {f.Name} размером: {f.Length} байт!\n");
        //                    //freedMemory += f.Length;
        //                    //freedSpace += f.Length;
        //                    f.Delete();
        //                    //delFileCount++;
        //                    fileCount++;
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"\tФайл {f.Name} ещё используется!\n");
        //                }
        //            }
        //            catch (FileNotFoundException ex)
        //            {
        //                Console.WriteLine($"Файл не найден. Ошибка: {ex.Message}");
        //            }
        //            catch (UnauthorizedAccessException ex)
        //            {
        //                Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Ошибка: {ex.Message}");
        //            }

        //        }
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Ошибка: {ex.Message}");
        //    }
        //    return GetFolderSize(directory);
        //}

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

        static long GetFolderSize(DirectoryInfo directory)
        {
            long size = 0;
            try
            {
                DirectoryInfo[] folders = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                foreach (FileInfo file in files)
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

                foreach (DirectoryInfo folder in folders)
                {
                    size += GetFolderSize(folder);
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
            return size;
        }

        static int GetFilesCount(DirectoryInfo directory)
        {
            int count = 0;

            try
            {
                DirectoryInfo[] folders = directory.GetDirectories();
                
                foreach (DirectoryInfo folder in folders)
                {
                    count = GetFilesCount(folder);
                    
                }
                count += directory.GetFiles().Length;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа. Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            return count;
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