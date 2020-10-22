namespace Homework1
{
    using System.Collections;

    public class MonthsCollection : IEnumerable
    {
        private int _position = -1;

        private readonly string[] _months =
        {
            "Январь", "Февраль", "Март",
            "Апрель", "Май", "Июнь",
            "Июль", "Август", "Сентябрь",
            "Октябрь", "Ноябрь", "Декабрь"
        };

        /// <summary>
        /// Индексатор.
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        public string this[int index]
        {
            get => _months[index];
            set => _months[index] = value;
        }

        /// <summary>
        /// Перечислитель.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();

            // Yield return
            // for (var i = 0; i < _months.Length; i++)
            // {
            //     yield return _months[i];
            // }

            // LINQ-expression
            // return _months.GetEnumerator();
        }

        public MonthsEnumerator GetEnumerator()
        {
            return new MonthsEnumerator(_months);
        }
    }
}