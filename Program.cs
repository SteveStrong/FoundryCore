using System;
using System.Collections.Generic;

namespace FoundryCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var prop1 = new FoProperty<string>("Stephen R. Strong");
            prop1.asJson(prop1);

            var prop2 = new FoProperty<double>(100);
            //prop2.asJson(prop2);

            string[] stuff = { "one", "two", "three", "5" };
            var prop3 = new FoCollection<string>(stuff);

            double[] data = { 1, 2, 3, 4, 5, 6, 7 };
            var prop4 = new FoCollection<double>(data);

            var comp1 = new FoComponent();
            comp1.Name = "my Comp";
            comp1.propx = prop1;

            Console.WriteLine($" to string {prop1}");
            Console.WriteLine($" to string {prop2}");
            Console.WriteLine($" to string {prop3}");
            Console.WriteLine($" to string {prop4}");


            comp1.Add("prop1", prop1);
            //comp1.Add("prop2", prop2);
            Console.WriteLine($" to string {comp1}");

            comp1.asJson(comp1);

            
            //prop4.asJson(prop4);
        }
    }
}
