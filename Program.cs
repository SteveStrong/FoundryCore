using System;
using System.Collections.Generic;

namespace FoundryCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var prop1 = new FoProperty<string>
            {
                Value = "Stephen R. Strong"
            };
            var prop2 = new FoProperty<double>(100);

            string[] stuff = { "one", "two", "three" };
            var prop3 = new FoCollection<string>(stuff);


            var prop4 = new FoCollection<string>
            {
                Value = new List<string> { "one", "two" }
            };

            Console.WriteLine($" to string {prop1}");
            Console.WriteLine($" to string {prop2}");
            Console.WriteLine($" to string {prop3}");
            Console.WriteLine($" to string {prop4}");
        }
    }
}
