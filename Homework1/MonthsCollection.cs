namespace Homework1
{
    using System.Collections;

    public class MonthsCollection : IEnumerable
    {
        private readonly string[] _months =
        {
            "January", "February", "March",
            "April", "May", "June",
            "Jule", "August", "September",
            "October", "November", "December"
        };

        /// <summary>
        /// Индексатор.
        /// </summary>
        /// <param name="index">Индекс</param>
        public string this[int index]
        {
            get => _months[index];
            set => _months[index] = value;
        }

        /// <summary>
        /// Перечислитель.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _months.GetEnumerator();
        }
    }
}