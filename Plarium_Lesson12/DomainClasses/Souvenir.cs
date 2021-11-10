using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Plarium_Lesson12
{
    
    //Тип определяет контракт данных и может быть сериализован
    [DataContract]
    //Класс сувенира
     abstract class Souvenir
    {
        //Инициализирующий конструктор
        protected Souvenir(string souvenirName, int releaseDate, decimal price)
        {
            SouvenirName = souvenirName;
            ReleaseDate = releaseDate;
            Price = price;
        }
        //Конструктор по умолчанию
        protected Souvenir()
        { }
        //Абстрактное свойство
        [DataMember]
        public abstract string KindOfSouvenir { get; set; }
        [DataMember]
        public string SouvenirName { get; set; }
        //Установка ID
        [DataMember]
        public int ManufacturerRequisites { get; set; } = ++AddDelete.ID;
        [DataMember]
        public int ReleaseDate { get;  set; }
        [DataMember]
        public decimal Price { get; set; }
        /// <summary>
        /// Метод для записи информации в файл
        /// </summary>
        /// <param name="sw"></param>
        public virtual void WriteToFileInformationSouvenir(StreamWriter sw)
        {
            sw.WriteLine($"Вид сувенира: {KindOfSouvenir}");
            sw.WriteLine($"Название сувенира: {SouvenirName}");
            sw.WriteLine($"Реквизиты производителя(ID): {ManufacturerRequisites}");
            sw.WriteLine($"Год выпуска: {ReleaseDate}");
            sw.WriteLine($"Цена: {Price}");
        }
        /// <summary>
        /// Метод для получения строки-записи в БД о сувенире
        /// </summary>
        /// <returns></returns>
        public virtual string Record()
        {
            return $"{this.KindOfSouvenir};{this.SouvenirName};{this.ReleaseDate};{this.Price};{this.ManufacturerRequisites};";
        }
        /// <summary>
        /// Метод для записи информации в БД
        /// </summary>
        /// <param name="sw"></param>
        public virtual void WriteToDatabase(StreamWriter sw) => sw.Write(Record());
        /// <summary>
        /// Метод для вывода информации в консоль
        /// </summary>
        public virtual void  DisplayInformationSouvenir()
        {
            Console.WriteLine($"Вид сувенира: {KindOfSouvenir}");
            Console.WriteLine($"Название сувенира: {SouvenirName}");
            Console.WriteLine($"Реквизиты производителя(ID): {ManufacturerRequisites}");
            Console.WriteLine($"Год выпуска: {ReleaseDate}");
            Console.WriteLine($"Цена: {Price}");
        }
    }
}

