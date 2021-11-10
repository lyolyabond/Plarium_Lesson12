using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Threading;


//Для хранения коллекций объектов предметной области использовать
//обобщенные коллекции (для одной из сущностей использовать коллекцию типа СЛОВАРЬ).

//Коллекцию сущностей представить в виде класса (коллекция - поле класса). 
//Реализовать индексаторы и итераторы по элементам коллекции.

namespace Plarium_Lesson12
{
    
    //Тип определяет контракт данных и может быть сериализован
    [DataContract]
    //Типы, которые следует включить при десериализации
    [KnownType(typeof(BusinessSouvenir))]
    [KnownType(typeof(PromotionalSouvenir))]
    [KnownType(typeof(ThematicSouvenir))]
    [KnownType(typeof(VIPGift))]
    //Коллекция сущностей(список) представлена в виде класса 
    class CollectionClass 
    {
        //Список объектов класса сувенира
        [DataMember]
        public List<Souvenir> Souvenirs { get; set; }
        public CollectionClass()
        {
            Souvenirs = new List<Souvenir>();
        }
        object locker = new object();

        /// <summary>
        /// Индексатор по элементам списка сувениров
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Souvenir this[int index]
        {
            get 
            {
                //Возвращает объект класса по индексу в списке(если не пустой)
                if (Souvenirs.Count > 0)
                   return Souvenirs[index];
                else return null;
            }
        }
        /// <summary>
        /// Итератор по элементам списка сувениров
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            for(int i = 0; i < Souvenirs.Count; i++)
            {
                yield return Souvenirs[i];
            }
        }

        /// <summary>
        /// Метод возвращает количество элементов в списке
        /// </summary>
        /// <returns></returns>
        public int Length()
        {
            return Souvenirs.Count;
        }
        /// <summary>
        /// Метод удаляет элемент списка по индексу
        /// </summary>
        /// <param name="key"></param>
        public void Remove(int key)
        {
            if(Souvenirs.Count > 0)
            Souvenirs.RemoveAt(key);
        }
        /// <summary>
        /// Метод добавляет объект в список
        /// </summary>
        /// <param name="souvenir"></param>
        public void Add(Souvenir souvenir)
        {
            Souvenirs.Add(souvenir);
            new Thread(Database.AddSouvenirToDatabase).Start();
        }
        /// <summary>
        /// Метод очищает список
        /// </summary>
        public void Clear()
        {
            Souvenirs.Clear();
        }
        /// <summary>
        /// Метод сортирует список по цене
        /// </summary>
        public void SortByPrice()
        {
            lock (locker)
            { 
                Souvenirs.Sort(new ComparerByPrice());
            }
        }
        /// <summary>
        /// Метод сортирует список по названию
        /// </summary>
        public void SortBySouvenirName()
        {
            lock (locker)
            {
                Souvenirs.Sort(new ComparerBySouvenirName());
            }
        }

        /// <summary>
        /// Метод при возникновении события удаления производителя, удаляет его сувениры
        /// Подписчик на событие ManufacturerRemoved - удаление производителя
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyEventArgs"></param>
        public void DeleteObjectsByKey (object source, EventDelegate.KeyEventArgs keyEventArgs)
        {
            bool flag = false;
            for (int i = 0; i < this.Length(); i++)
            {
                if (this[i].ManufacturerRequisites == keyEventArgs.Key)
                {
                    //Удаление элемента по индексу из списка сувениров
                    this.Remove(i);
                    new Thread(new ParameterizedThreadStart(Database.DeleteRecord)).Start(keyEventArgs.Key);
                    flag = true;
                }
            }
            if(flag)
            {
                Console.WriteLine($"Удаление сувенира с ID {keyEventArgs.Key} прошло успешно!");
            }
        }

    }
}
