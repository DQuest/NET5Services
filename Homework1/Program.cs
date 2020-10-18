namespace Homework1
{
    using System;

    class Program
    {
        static void Main()
        {
            var months = new MonthsCollection();

            for (var i = 0; i < 12; i += 2)
            {
                Console.WriteLine(months[i]);
            }

            Console.WriteLine("********");

            foreach (var month in months)
            {
                Console.WriteLine(month);
            }
        }
    }
}