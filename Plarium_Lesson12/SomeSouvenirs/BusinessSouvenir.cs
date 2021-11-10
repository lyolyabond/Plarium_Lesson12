using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;


namespace Plarium_Lesson12
{
    //Тип определяет контракт данных и может быть сериализован
    [DataContract]
    class BusinessSouvenir : Souvenir
    {
        //Реализация абстрактного свойства

        public override string KindOfSouvenir { get; set; } = "Бизнес-сувенир";
        [DataMember]
        public string CompanyName { get; set; }
        public BusinessSouvenir(string souvenirName, string companyName, int releaseDate, decimal price)
            : base(souvenirName, releaseDate, price)
        {
            CompanyName = companyName;
        }
        public BusinessSouvenir()
        { }

        /// <summary>
        /// Переопределённый метод для записи информации в файл
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteToFileInformationSouvenir(StreamWriter sw)
        {
            base.WriteToFileInformationSouvenir(sw);
            sw.WriteLine($"Название компании бизнес-сувенира: {CompanyName}");
            sw.WriteLine("--------------------------");
        }
        /// <summary>
        /// Переопределённый метод для получения строки-записи в БД о сувенире
        /// </summary>
        /// <returns></returns>
        public override string Record() => base.Record() + $"{this.CompanyName};";
        /// <summary>
        /// Переопределённый метод для вывода информации в консоль
        /// </summary>
        public override void DisplayInformationSouvenir()
        {
            base.DisplayInformationSouvenir();
            Console.WriteLine($"Название компании бизнес-сувенира: {CompanyName}");
            Console.WriteLine("--------------------------");
        }
    }
}
