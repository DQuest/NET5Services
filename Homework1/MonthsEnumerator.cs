namespace Homework1
{
    using System;
    using System.Collections;

    public class MonthsEnumerator : IEnumerator
    {
        private int _position = -1;

        private readonly string[] _months;

        public MonthsEnumerator(string[] months)
        {
            _months = months;
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _months.Length);
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current => Current;

        public string Current
        {
            get
            {
                try
                {
                    return _months[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}