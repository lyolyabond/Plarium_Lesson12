using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Plarium_Lesson12
{
    class Database
    {
        static string pathTemporary = "Temporary.txt";
        public static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        
        /// <summary>
        /// Метод создаёт БД по указаному названию, если она ещё не существует
        /// </summary>
        public static void CreateDatabase()
        {
           
            if (File.Exists(Menu.databasePath))
            {
                Console.WriteLine("Файл уже существует!");
            }
            else
            {
                File.Create(Menu.databasePath).Close();
                Console.WriteLine($"Файл {Menu.databasePath} успешно создан!");
            }
        }
        /// <summary>
        /// Метод выводит в консоль информацию из БД
        /// </summary>
        public static void ReadDatabase()
        {
            if (File.Exists(Menu.databasePath))
            {
                _rw.EnterReadLock();
                using (var sr = new StreamReader(Menu.databasePath))
                {
                    if (new FileInfo(Menu.databasePath).Length > 0)
                    {
                        while (!sr.EndOfStream)
                        {
                            Console.WriteLine(sr.ReadLine());
                        }
                    }
                    else Console.WriteLine($"Файл пустой!");
                }
                _rw.ExitReadLock();
            }
            else Console.WriteLine("Такого файла не существует!");
        }
        /// <summary>
        /// Метод добавляет запись о сувенире в БД
        /// </summary>
        public static void AddSouvenirToDatabase()
        {
            int index = AddDelete.collectionClass.Length() - 1;
           
            if (AddDelete.collectionClass.Length() > 0 && File.Exists(Menu.databasePath))
            {   _rw.EnterWriteLock();
                using (var sw = new StreamWriter(Menu.databasePath, true))
                {
                    AddDelete.collectionClass[index].WriteToDatabase(sw);
                }
                _rw.ExitWriteLock();
            }
        }
        /// <summary>
        /// Метод добавляет запись о производителе в БД
        /// </summary>
        /// <param name="obj"></param>
        public static void AddManufacturerToDatabase(object obj)
        {
            int id = (int)obj;
            if (AddDelete.collectionClass.Length() > 0 && File.Exists(Menu.databasePath))
            {
                _rw.EnterWriteLock();
                using (var sw = new StreamWriter(Menu.databasePath, true))
                {
                    AddDelete.Manufacturers[id].WriteToDatabase(sw);
                }
                _rw.ExitWriteLock();
            }
        }
        /// <summary>
        /// Метод обновляет содержимое БД
        /// </summary>
        public static void UpdateDatabase()
        {
            if (AddDelete.collectionClass.Length() > 0 && File.Exists(Menu.databasePath))
            {
                _rw.EnterWriteLock();
                using (var sw = new StreamWriter(Menu.databasePath, false))
                {
                    foreach (Souvenir souvenir in AddDelete.collectionClass)
                    {
                        souvenir.WriteToDatabase(sw);
                        if (AddDelete.Manufacturers.ContainsKey(souvenir.ManufacturerRequisites))
                        {
                            AddDelete.Manufacturers[souvenir.ManufacturerRequisites].WriteToDatabase(sw);
                        }
                    }
                }
                _rw.ExitWriteLock();
                Console.WriteLine($"Файл обновлён!");
            }
            else Console.WriteLine("Такого файла не существует или список пуст!");
        }
        /// <summary>
        /// Метод меняет цену в БД 
        /// </summary>
        /// <param name="souvenir"></param>
        /// <param name="newPrice"></param>
        public static void ChangePrice(Souvenir souvenir, decimal newPrice)
        {
            if (File.Exists(Menu.databasePath))
            {
                string text;
                using (var sr = new StreamReader(Menu.databasePath))
                {
                    text = sr.ReadToEnd();
                }

                string oldRecord = souvenir.Record();
                
                if (text.Contains(oldRecord))
                {
                    souvenir.Price = newPrice;
                    string newText = text.Replace(oldRecord, souvenir.Record());
                   
                    using (var sw = new StreamWriter(pathTemporary))
                    {
                        sw.Write(newText);
                    }

                    File.Delete(Menu.databasePath);
                    File.Move(pathTemporary, Menu.databasePath);
                }

            }
        }
        /// <summary>
        ///  Метод удаляет запись из БД по ключу
        /// </summary>
        /// <param name="obj"></param>
        public static void DeleteRecord(object obj)
        {
            int key = (int)obj;
            
            if (File.Exists(Menu.databasePath))
            {
                _rw.EnterWriteLock();
                var lines = File.ReadLines(Menu.databasePath).Where(l => !l.Contains(";" + key.ToString() + ";"));
                 
                File.WriteAllLines(pathTemporary, lines);
                File.Delete(Menu.databasePath);
                File.Move(pathTemporary, Menu.databasePath);
                _rw.ExitWriteLock();
                }
            
        }
        /// <summary>
        /// Метод удаляет указанную БД, если она существует
        /// </summary>
        public static void DeleteDatabase()
        {
            if (File.Exists(Menu.databasePath))
            {
                File.Delete(Menu.databasePath);
                Console.WriteLine($"Файл удалён!");
            }
            else Console.WriteLine("Такого файла не существует!");
        }

    }
}
