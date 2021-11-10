using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;


namespace Plarium_Lesson12
{
    class Serialization
    {
       public static string pathManufacturers = "Manufacturers.json";
       public static string pathSouvenirs = "Souvenirs.json";

        /// <summary>
        /// Метод сериализует словарь, где хранится информация о производителях
        /// </summary>
        public static void ManufacturersSerialization()
        {
            if(AddDelete.Manufacturers.Count>0)
            { 
            var jsonFormatter = new DataContractJsonSerializer(typeof(Dictionary<int, Manufacturer>));
            using(var file = new FileStream(pathManufacturers, FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(file, AddDelete.Manufacturers);
            }
            }
        }
        /// <summary>
        /// Метод сериализует класс списка, где хранится информация о сувенирах
        /// </summary>
        public static void SouvenirsSerialization()
        {
            if(AddDelete.collectionClass.Length() > 0)
            { 
            var jsonFormatter = new DataContractJsonSerializer(typeof(CollectionClass));
            using (var file = new FileStream(pathSouvenirs, FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(file, AddDelete.collectionClass);
            }
            }
        }
        /// <summary>
        /// Метод десериализует строку json в объект типа словаря 
        /// </summary>
        /// <returns></returns>
        static Dictionary<int, Manufacturer> ManufacturersDeserialization()
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(Dictionary<int, Manufacturer>));
           
            using (var file = new FileStream(pathManufacturers, FileMode.Open))
            {
                return  jsonFormatter.ReadObject(file) as Dictionary<int, Manufacturer>;   
            } 
        }
        /// <summary>
        /// Метод десериализует строку json в объект типа класса списка 
        /// </summary>
        /// <returns></returns>
        static CollectionClass SouvenirsDeserialization()
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(CollectionClass));
            
            using (var file = new FileStream(pathSouvenirs, FileMode.Open))
            {
                return jsonFormatter.ReadObject(file) as CollectionClass;   
            }
        }
        /// <summary>
        /// Метод проверяет существуют ли файлы для десериализации и вызывает методы
        /// </summary>
        public static void Deserialization()
        {
            if(File.Exists(pathManufacturers) && File.Exists(pathSouvenirs))
            {
                AddDelete.collectionClass = SouvenirsDeserialization();
                File.Delete(pathSouvenirs);
                AddDelete.Manufacturers = ManufacturersDeserialization();
                File.Delete(pathManufacturers);
            }
        }
    }
}
