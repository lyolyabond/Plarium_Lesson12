using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Plarium_Lesson12
{
    //Тип определяет контракт данных и может быть сериализован
    [DataContract]
    //Класс производителя сувениров
    class Manufacturer
    {
        public event EventHandler <EventDelegate.KeyEventArgs> ManufacturerRemoved;
        //Инициализирующий конструктор
        public Manufacturer(string manufacturerName, string manufacturerCountry)
        {
            ManufacturerName = manufacturerName;
            ManufacturerCountry = manufacturerCountry;
        }
        //Конструктор по умолчанию
        public Manufacturer()
        { }
        [DataMember]
        public string ManufacturerName { get; set; }
        [DataMember]
        public string ManufacturerCountry { get; set; }
        /// <summary>
        /// Метод для записи информации в файл
        /// </summary>
        /// <param name="sw"></param>
        public void WriteToFileInformationManufacturer(StreamWriter sw)
        {
            sw.WriteLine($"Название производителя: {ManufacturerName}");
            sw.WriteLine($"Страна производителя: {ManufacturerCountry}");
            sw.WriteLine("--------------------------");
        }
        /// <summary>
        /// Метод для вывода информации в консоль
        /// </summary>
        public void DisplayInformationManufacturer()
        {
            Console.WriteLine($"Название производителя: {ManufacturerName}");
            Console.WriteLine($"Страна производителя: {ManufacturerCountry}");
            Console.WriteLine("--------------------------");
        }
        /// <summary>
        /// Метод для получения строки-записи в БД о производителе
        /// </summary>
        /// <returns></returns>
        public string Record() => $"{this.ManufacturerName};{this.ManufacturerCountry}\n";
        /// <summary>
        /// Метод записывает информацию в БД
        /// </summary>
        /// <param name="sw"></param>
        public void WriteToDatabase(StreamWriter sw) => sw.Write(Record());

        /// <summary>
        /// Метод для вызова события удаления производителя
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnManufacturerRemoved(EventDelegate.KeyEventArgs e)
        {
            var temp = ManufacturerRemoved;
            //Вызов события, проверка на null перед вызовом
            temp?.Invoke(this, e);
        }

        public void RemoveManufacturer(int key)
        {
            var e = new EventDelegate.KeyEventArgs(key);
            OnManufacturerRemoved(e);
        }

    }
}
