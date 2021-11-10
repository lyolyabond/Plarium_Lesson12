using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Plarium_Lesson12
{
    class AddDelete
    {
        //Переменная, которая хранит последний ключ
        public static int ID = 0;
        //Коллекция, которая хранит ключи и объекты класса производителя
        public static Dictionary<int, Manufacturer> Manufacturers = new Dictionary<int, Manufacturer>();
        //Объект класса для работы со списком сувениров
        public static CollectionClass collectionClass = new CollectionClass();

        /// <summary>
        /// Метод добавляет объект производителя в словарь
        /// </summary>
        /// <param name="manufacturer"></param>
        public static void AddManufacturer(Manufacturer manufacturer)
        {
            Manufacturers.Add(ID, manufacturer);
            //Файл очищается, чтобы не хранить некорректную информацию
            File.WriteAllText(Program.path, String.Empty);
            Thread threadManufacturer = new Thread(new ParameterizedThreadStart(Database.AddManufacturerToDatabase));
            threadManufacturer.Start(ID);
            threadManufacturer.Join();
        }
        /// <summary>
        /// Метод удаляет объект из словаря по заданному названию производителя
        /// </summary>
        /// <param name="eventDelete"></param>
        public static void DeleteItemByManufacturer(Manufacturer eventDelete)
        {
            string name = Input.InputManufacturerName();
            bool flag = false;

            //Механизм обработки исключительных ситуаций(если нет сувенира с заданным названием производителя)
            try
            {
                //Выборка элементов коллекции, в которых совпадает название производителя
                var keysValue = AddDelete.Manufacturers.Where
                    (manufacturer => string.Equals(manufacturer.Value.ManufacturerName, name, StringComparison.OrdinalIgnoreCase));
                //Проход по элементам выборки
                foreach (var keyValue in keysValue)
                {   
                        //Удаление элемента по ключу из словаря производителей
                        Manufacturers.Remove(keyValue.Key);
                        //Метод, который вызывает событие
                        eventDelete.RemoveManufacturer(keyValue.Key);
                        //Файл очищается, чтобы не хранить некорректную информацию
                        File.WriteAllText(Program.path, String.Empty);
                        flag = true;
                }
                if(!flag)
                 throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine($"Производителя с названием {name} нет в базе!");
            }
    
        }
        /// <summary>
        /// Метод очищает коллекции
        /// </summary>
        public static void ClearCollections()
        {
            collectionClass.Clear();
            Manufacturers.Clear();
            Console.WriteLine("Коллекции очищены.");
            //Файл очищается, чтобы не хранить некорректную информацию
            File.WriteAllText(Program.path, String.Empty);
            File.WriteAllText(Menu.databasePath, String.Empty);
        }
    }   
}
