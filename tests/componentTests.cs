using System;
using System.Linq;
using System.Collections.Generic;

namespace FoundryCore
{
    class ComponentTests
    {
        public static void test4()
        {
            var body = JSONExtensions.readFromFile(@"data/create_root.json");
            var name = body.GetProperty("MyName");
            var root = new FoComponent(name.ToString());
            root.exportAsJson(@"data/test4.json");

            var handle = Activator.CreateInstance(root.GetType()) as FoComponent;
            handle.MyName = "ffrfrf";

            Console.WriteLine($"{handle.toJson()}");
            Console.WriteLine($"===========================");
            Console.WriteLine($"{root.toJson()}");
            Console.WriteLine($"..........................");
         }
 
        public static void test3()
        {
            IFoProperty[] props = {
                new FoProperty<string>("proper name", "Stephen R. Strong"),
            };


            var root = new FoComponent("Root", props);

            Console.WriteLine($"===========================");
            // Console.WriteLine($"{root.Properties.toJson()}");
            Console.WriteLine($"{root.toJson()}");
            Console.WriteLine($"..........................");

            root.saveToFile(@"data/test3.json");
         }
 
        public static void test2()
        {
            IFoProperty[] props = {
                new FoProperty<string>("proper name", "Stephen R. Strong"),
                new FoProperty<double>("cost", 100),
                new FoProperty<double>("tax", .33),
                new FoProperty<double>("total", () => { return 1000; })
            };

            IFoComponent[] comps = {
                new FoComponent("Comp1",props ),
                new FoComponent("Comp2",props ),
                new FoComponent("Comp3",props ),
            };

            var root = new FoComponent("Root", comps);
            var total = new FoProperty<double>("total cost", () => {
                double result = root.SumOver("cost");
                return result;
            });
            root.Properties.Add(total);

            Console.WriteLine($"===========================");
            // Console.WriteLine($"{root.Properties.toJson()}");
            Console.WriteLine($"{root.toJson()}");
            Console.WriteLine($"..........................");

            root.saveToFile(@"data/test2.json");
         }
 
        public static void test1()
        {
            IFoProperty[] props = {
                new FoProperty<string>("xxx", "Stephen R. Strong"),
                new FoProperty<double>("cost", 100),
                new FoProperty<double>("tax", .33),
                new FoProperty<double>("total", () => { return 1000; })
            };


            var comp1 = new FoComponent("Comp1");
            comp1.Properties.AddList(props);
            var cost = comp1.Reference<FoProperty<double>>("cost");
            var tax = comp1.Reference<FoProperty<double>>("tax");
            var total = comp1.Reference<FoProperty<double>>("total");
            total.Formula = () => { return 1000 * cost.Value * tax.Value; };


            Console.WriteLine($"==========================");
            Console.WriteLine($"Component: {comp1.MyName}");
            Console.WriteLine($"{comp1.toJson()}");
            Console.WriteLine($"..........................");

            cost.Value = 1;
            Console.WriteLine($"==========================");
            Console.WriteLine($"Component: {comp1.MyName}");
            Console.WriteLine($"{comp1.toJson()}");
            Console.WriteLine($".........................."); 
        }
    }
}
