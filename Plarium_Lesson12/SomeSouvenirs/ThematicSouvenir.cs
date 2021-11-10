using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;


namespace Plarium_Lesson12
{
    //Тип определяет контракт данных и может быть сериализован
    [DataContract]
    class ThematicSouvenir : Souvenir
    {
        //Реализация абстрактного свойства

        public override string KindOfSouvenir { get; set; } = "Тематический сувенир";
        [DataMember]
        public string SubjectMatter { get; set; }
        public ThematicSouvenir(string souvenirName, string subjectMatter,  int releaseDate, decimal price)
            : base(souvenirName, releaseDate, price)
        {
            SubjectMatter = subjectMatter;
        }
        public ThematicSouvenir()
        { }

        /// <summary>
        /// Переопределённый метод для записи информации в файл
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteToFileInformationSouvenir(StreamWriter sw)
        {
            base.WriteToFileInformationSouvenir(sw);
            sw.WriteLine($"Название тематики сувенира: {SubjectMatter}");
            sw.WriteLine("--------------------------");
        }
        /// <summary>
        /// Переопределённый метод для получения строки-записи в БД о сувенире
        /// </summary>
        /// <returns></returns>
        public override string Record() => base.Record() + $"{this.SubjectMatter};";
        /// <summary>
        /// Переопределённый метод для вывода информации в консоль
        /// </summary>
        public override void DisplayInformationSouvenir()
        {
            base.DisplayInformationSouvenir();
            Console.WriteLine($"Название тематики сувенира: {SubjectMatter}");
            Console.WriteLine("--------------------------");
        }
    }
}
