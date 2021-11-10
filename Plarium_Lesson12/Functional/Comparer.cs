using System;
using System.Collections.Generic;

namespace Plarium_Lesson12
{
    //Компараторы объектов Souvenir
    class ComparerByPrice : IComparer<Souvenir>
    {
        /// <summary>
        /// Сравнивает объекты в зависимости от цены (значения свойства Price)
        /// </summary>
        /// <param name="souvenir1"></param>
        /// <param name="souvenir2"></param>
        /// <returns>int</returns>
        public int Compare(Souvenir souvenir1, Souvenir souvenir2) => souvenir1.Price.CompareTo(souvenir2.Price);
    }
    class ComparerBySouvenirName : IComparer<Souvenir>
    {

        /// <summary>
        /// Сравнивает объекты в зависимости от названия сувенира(чтобы отсортировать в алфавитном порядке)
        /// </summary>
        /// <param name="souvenir1"></param>
        /// <param name="souvenir2"></param>
        /// <returns></returns>
        public int Compare(Souvenir souvenir1, Souvenir souvenir2) => 
            String.Compare(souvenir1.SouvenirName, souvenir2.SouvenirName, StringComparison.CurrentCultureIgnoreCase);
    }
}
