using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Plarium_Lesson12
{
    class Function
    {
        static Thread keyMatching;
        /// <summary>
        /// Метод выводит информацию о сувенирах и их производителях
        /// </summary>
        public static void DisplayAllInformation()
        {
            //Проверка списка на пустоту
            if (AddDelete.collectionClass.Length() > 0)
            {
                using (var sw = new StreamWriter(Program.path, true))
                {
                    //Проход по элементам списка
                    foreach (Souvenir souvenir in AddDelete.collectionClass)
                    {
                        //Показ информации о сувенире
                        souvenir.DisplayInformationSouvenir();
                        //Выборка единственного элемента словаря, с которым совпадает ключ
                        var keyValue = AddDelete.Manufacturers.Single(manufacturer => manufacturer.Key == souvenir.ManufacturerRequisites);
                        //Показ информации о производителе 
                        AddDelete.Manufacturers[keyValue.Key].DisplayInformationManufacturer(); 
                    }
                }
            }
            //Если список пуст
            else Console.WriteLine("Нет информации о сувенирах!");
        }

        /// <summary>
        /// Метод записывает информацию о сувенире по ключу в файл
        /// </summary>
        /// <param name="obj"></param>
        static void KeyMatchingForList(object obj)
        {
           
            int key = (int)obj;
            //Выборка единственного элемента Souvenir, с которым совпадает ключ
              Souvenir value = AddDelete.collectionClass.Souvenirs.Single(souvenir => souvenir.ManufacturerRequisites == key);

                    using (var sw = new StreamWriter(Program.path, true))
                    {
                        value.WriteToFileInformationSouvenir(sw);
                    }

        }

        /// <summary>
        /// Метод записывает информацию о производителе по ключу в файл
        /// </summary>
        /// <param name="obj"></param>
        static void KeyMatchingForDictionary(object obj)
        {
            int key = (int)obj;
            //Выборка единственного элемента словаря, с которым совпадает ключ
            var keyValue = AddDelete.Manufacturers.Single(manufacturer => manufacturer.Key == key);
            using (var sw = new StreamWriter(Program.path, true))
                {
                    AddDelete.Manufacturers[key].WriteToFileInformationManufacturer(sw);
                }
        }

        /// <summary>
        /// Метод записывает в файл и выводит информацию о сувенирах по названию производителя
        /// </summary>
        public static void DisplayInformationByManufacturer()
        {
            string name = Input.InputManufacturerName();
            bool flag = false;
            string header = $"Информация о сувенирах производителя {name}:";
            WritingHeaderToFile(header);

            //Механизм обработки исключительных ситуаций(если нет производителя с таким названием)
            try
            {
                //Выборка элементов словаря, у которых совпадает название производителя с заданным
                var keysValue = AddDelete.Manufacturers.Where
                    (manufacturer => string.Equals(manufacturer.Value.ManufacturerName, name,StringComparison.OrdinalIgnoreCase));
                //Проход по элементам выборки
                    foreach (var keyValue in keysValue)
                    {
                    //Запись информации по ключу
                     keyMatching= new Thread(new ParameterizedThreadStart(KeyMatchingForList));
                     keyMatching.Start(keyValue.Key);
                     keyMatching.Join();
                        flag = true;
                     }
                if (!flag)
                    throw new Exception();
            }
            catch (Exception)
            {
                WritingHeaderToFile($"Названия производителя {name} нет в базе!");
            }
            finally
            {
                WriteEmptyLine();
                SearchTextInFile(header);
            }  
        }
        /// <summary>
        /// Метод записывает в файл и выводит информацию о сувенирах по названию страны производителя
        /// </summary>
        public static void DisplayInformationByCountry()
        {
            string country = Input.InputManufacturerCountry();
            bool flag = false;
            string header = $"Информация o сувенирах, произведенных в стране {country}:";
            WritingHeaderToFile(header);

            //Механизм обработки исключительных ситуаций(если нет страны с таким названием)
            try
            {
                //Выборка элементов словаря, у которых совпадает название страны производителя с заданным
                var keysValue = AddDelete.Manufacturers.Where
                   (manufacturer => string.Equals(manufacturer.Value.ManufacturerCountry, country, StringComparison.OrdinalIgnoreCase));
                
                //Проход по элементам выборки
                foreach (var keyValue in keysValue)
                {
                    //Запись информации по ключу
                     keyMatching= new Thread(new ParameterizedThreadStart(KeyMatchingForList));
                     keyMatching.Start(keyValue.Key);
                     keyMatching.Join();
                    flag = true;
                }

                if (!flag)
                    throw new Exception();
            }
            catch (Exception)
            {
                WritingHeaderToFile($"Названия страны {country} нет в базе!");
            }
            finally
            {
                WriteEmptyLine();
                SearchTextInFile(header);
            }
        }
        /// <summary>
        /// Метод записывает в файл и выводит информацию о производителях, чьи цены на сувениры меньше заданной
        /// </summary>
        public static void DisplayInformationByPrice()
        {
            decimal price = Input.InputPrice();
            bool flag = false;
            string header = $"Информация o производителях, чьи цены на сувениры меньше {price}:";
            WritingHeaderToFile(header);

            //Механизм обработки исключительных ситуаций(если нет цены на сувениры меньше заданной)
            try
            {
                //Выборка элементов списка, у которых цена на сувениры меньше заданной
                var Souvenirs = AddDelete.collectionClass.Souvenirs.Where(value => value.Price < price);
                //Проход по элементам выборки
                foreach (Souvenir souvenir in Souvenirs)
                {
                    if (souvenir.Price < price)
                    {
                        //Запись информации по ключу
                         keyMatching= new Thread(new ParameterizedThreadStart(KeyMatchingForDictionary));
                         keyMatching.Start(souvenir.ManufacturerRequisites);
                         keyMatching.Join();
                        flag = true;
                    }

                }
                if (!flag)
                    throw new Exception();
            }
            catch (Exception)
            {
                WritingHeaderToFile($"Сувенира с ценой, меньше чем {price} нет в базе!");
            }
            finally
            {
                WriteEmptyLine();
                SearchTextInFile(header);
            }
            
        }
        /// <summary>
        /// Метод записывает в файл и выводит информацию о производителях
        /// заданного сувенира, произведенного в заданном году
        /// </summary>
        public static void DisplayInformationByDate()
        {
            string souvenirName = Input.InputSouvenirName();
            int releaseDate = Input.InputReleaseDate();
            bool flag = false;
            string header = $"Информация о производителях сувенира {souvenirName}, произведенного в {releaseDate} году:";
            WritingHeaderToFile(header);

            //Механизм обработки исключительных ситуаций(если нет сувенира с заданным названием и датой)
            try
            {
                //Выборка элементов списка, у которых совпадает название сувенира и даты с заданными
                var Souvenirs = AddDelete.collectionClass.Souvenirs
                    .Where(value => string.Equals(value.SouvenirName, souvenirName, StringComparison.OrdinalIgnoreCase))
                    .Where(value => value.ReleaseDate == releaseDate);
                //Проход по элементам выборки
                foreach (var souvenir in Souvenirs)
                {
                    //Запись информации по ключу
                     keyMatching = new Thread(new ParameterizedThreadStart(KeyMatchingForDictionary));
                     keyMatching.Start(souvenir.ManufacturerRequisites);
                     keyMatching.Join();
                    flag = true;
                }
                if (!flag)
                    throw new Exception();
            }
            catch (Exception)
            {
                WritingHeaderToFile($"Сувенира с названием {souvenirName} и датой выпуска {releaseDate} нет в базе!");
            }
            finally
            {
                WriteEmptyLine();
                SearchTextInFile(header);    
            }
            
        }
        /// <summary>
        /// Метод меняет цену сувенира по ID
        /// </summary>
        /// <param name="flag"></param>
        public static void PriceChange(ref bool flag)
        {
            Console.Write("Введите ID сувенира: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Введите натуральное число: ");
            }

            //Механизм обработки исключительных ситуаций(если нет сувенира с заданным id)
            try
            {
                //Выборка единственного элемента списка, с которым совпадает id
                var souvenir = AddDelete.collectionClass.Souvenirs
                    .SingleOrDefault(value => value.ManufacturerRequisites == id);

               if(souvenir != null)
                { 
                        decimal price = Input.InputPrice();
                        if (souvenir.Price != price)
                        {
                        Thread threadChangePrice = new Thread(delegate () { Database.ChangePrice(souvenir, price); });
                        threadChangePrice.Start();
                         flag = true;
                        }
                        else
                        {
                            Console.WriteLine("Вы ввели старую цену!");
                            return;
                        }
                }
                if (!flag)
                    throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine($"Сувенира с ID {id} нет в базе!");
            }
        }
        /// <summary>
        /// Метод оповещает об изменении цены
        /// </summary>
        /// <param name="flag"></param>
        public static void PriceChangeNotification(ref bool flag)
        {
            if (flag)
            {
                Console.WriteLine($"Вы изменили цену!");
                //Файл очищается, чтобы не хранить некорректную информацию
                File.WriteAllText(Program.path, String.Empty);
            }
        }

        /// <summary>
        /// Метод сортирует список по параметру, в зависимости от переданного делегата
        /// </summary>
        /// <param name="sortDelegate"></param>
        public static void SortList(EventDelegate.SortDelegate sortDelegate)
        {
            if (AddDelete.collectionClass.Length() > 0)
            {
                sortDelegate();
                Console.WriteLine("Список отсортирован!");
            }
            else Console.WriteLine("Списк пуст!");
        }
        /// <summary>
        /// Метод сортирует словарь по названию производителя
        /// </summary>
        public static void SortDictionary()
        {
            //Упорядочивает элементы по алфавиту по названию производителя
            var manufacturers = AddDelete.Manufacturers.OrderBy(manufacturer => manufacturer.Value.ManufacturerName);
            foreach (var manufacturer in manufacturers)
                manufacturer.Value.DisplayInformationManufacturer();
        }
        /// <summary>
        /// Метод записывает заголовок запроса в файл
        /// </summary>
        /// <param name="header"></param>
        public static void WritingHeaderToFile(string header)
        { 
            using (var sw = new StreamWriter(Program.path, true))
            {
                sw.WriteLine(header);
            }
           
        }
        /// <summary>
        /// Метод ищет по заголовку нужную информацию в файле и выводи на консоль
        /// </summary>
        /// <param name="header"></param>
        public static void SearchTextInFile(string header)
        {
            string line;
            using (var sw = new StreamReader(Program.path))
            {
                while ((line = sw.ReadLine()) != null)
                {
                    if (String.Equals(header, line))
                    {
                        Console.WriteLine("\n" + line);
                        while (!String.IsNullOrEmpty((line = sw.ReadLine())))
                            Console.WriteLine(line);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Метод записывает в файл пустую строку
        /// </summary>
        public static void WriteEmptyLine()
        {
            using (var sw = new StreamWriter(Program.path, true))
            {
                sw.WriteLine();
            }
        }
        
    }
}

