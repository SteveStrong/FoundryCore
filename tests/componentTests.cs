using System;
using System.Collections.Generic;

namespace FoundryCore
{
    class ComponentTests
    {
        public static void test2()
        {
            IFoProperty[] props = {
                new FoProperty<string>("xxx", "Stephen R. Strong"),
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

            Console.WriteLine($"==========================");
            Console.WriteLine($"{root.toJson()}");
            Console.WriteLine($".........................."); 
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
            Console.WriteLine($"Component: {comp1.Name}");
            Console.WriteLine($"{comp1.toJson()}");
            Console.WriteLine($"..........................");

            cost.Value = 1;
            Console.WriteLine($"==========================");
            Console.WriteLine($"Component: {comp1.Name}");
            Console.WriteLine($"{comp1.toJson()}");
            Console.WriteLine($".........................."); 
        }
    }
}
