using System;
using System.Collections.Generic;

namespace FoundryCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var prop1 = new FoProperty<string>("xxx", "Stephen R. Strong");
            prop1.toJson();

            var prop2a = new FoProperty<double>("cost", 100);
            prop2a.toJson();

            var prop2b = new FoProperty<double>("tax", .33);
            prop2b.toJson();

            var prop2c = new FoProperty<double>("total", prop2a.Value * prop2b.Value);
            prop2c.toJson();

            string[] stuff = { "one", "two", "three", "5" };
            var prop3 = new FoCollection<string>("count",stuff);

            double[] data = { 1, 2, 3, 4, 5, 6, 7 };
            var prop4 = new FoCollection<double>("number", data);
            var prop5 = new FoCollection<double>("number");

            var comp1 = new FoComponent("my Comp");
            comp1.propx = prop1;

            // Console.WriteLine($" to string {prop1}");
            // Console.WriteLine($" to string {prop2}");
            // Console.WriteLine($" to string {prop3}");
            // Console.WriteLine($" to string {prop4}");
            // Console.WriteLine($" to string {prop5}");

            comp1.Add(prop1);
            comp1.Add(prop2a);
            comp1.Add(prop2b);
            comp1.Add(prop2c);
            Console.WriteLine($" to string {comp1}");

            comp1.toJson();

            
            //prop4.asJson(prop4);
            //prop5.asJson(prop5);
        }
    }
}
