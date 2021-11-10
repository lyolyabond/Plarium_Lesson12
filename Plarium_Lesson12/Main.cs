using System;
using System.IO;
using System.Threading;



//- Реализовать запросы к коллекциям сущностей по технологии LINQ используя синтаксис методов расширения. 

//4. Сувениры.В сущностях(типах) хранится информация о сувенирах и их производителях.
//Для сувениров необходимо хранить:
//— название;
//— реквизиты производителя;
//— дату выпуска;
//— цену.
//Для производителей необходимо хранить:
//— название;
//— страну.
// Упорядочить сущности по 2-3 различным критериям.
//Вывести информацию о сувенирах заданного производителя.
//Вывести информацию о сувенирах, произведенных в заданной стране.
//Вывести информацию о производителях, чьи цены на сувениры меньше заданной.
//Вывести информацию о производителях заданного сувенира, произведенного в заданном году.
//Удалить заданного производителя и его сувениры.


namespace Plarium_Lesson12
{
    class Program
    {
        public static string path = "Result.txt";
       
        static void Main(string[] args)
        {
            if (!File.Exists(Menu.databasePath)) 
                File.Create(Menu.databasePath).Close();
            else File.WriteAllText(Menu.databasePath, String.Empty);

            Serialization.Deserialization();
            //Создание файла для записи/чтения результатов запросов к коллекциям сущностей
            File.CreateText(path).Close();

            //Проверка коллекции на пустоту
            if (AddDelete.collectionClass.Length()<1)
            {
            Console.WriteLine("--Выберите действие: --");
            Console.WriteLine("1 - Не использовать объекты по умолчанию\n2 - Инициализировать по умолчанию");
           int choice = int.Parse(Console.ReadLine());
            
            switch (choice)
            {
                case 1:
                    Console.Clear();
                    //Вызов метода меню для консоли
                    Menu.ConsoleMenu();
                    break;
                case 2:
                    //Добавление в список сувениров и в словарь производителей
                    AddDelete.collectionClass.Add( new PromotionalSouvenir("Ручка","ПриватБанк", 2020, 15.5m));
                    AddDelete.AddManufacturer(new Manufacturer("UKR", "Украина"));
                    AddDelete.collectionClass.Add(new ThematicSouvenir("Статуэтка","Новый год", 1999, 1200m));
                    AddDelete.AddManufacturer(new Manufacturer("Global", "США"));
                    AddDelete.collectionClass.Add(new BusinessSouvenir("Ежедневник", "Plarium", 2021, 150m));
                    AddDelete.AddManufacturer(new Manufacturer("Assa", "Украина"));
                    AddDelete.collectionClass.Add(new VIPGift("Шкатулка", "Окончание проекта", 2019, 15000m));
                    AddDelete.AddManufacturer(new Manufacturer("Global", "США"));

                    Console.Clear();
                    //Вызов метода меню для консоли
                    break;
                default: break;
            }
            }
            Menu.ConsoleMenu();
        }
    }
}
