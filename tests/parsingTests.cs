using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;

namespace FoundryCore
{
    class ParsingTests
    {
        public static void test1()
        {
            var root = new FoComponent("Component");

            var parser = new Parser("1.0 + 2 * (3 - 55.5 )");

            var formula = parser.ReadFormula();
            var json = formula.toJson();

            Console.WriteLine($"===========================");
            Console.WriteLine($"{json}");
            //Console.WriteLine($"{root.AsJson()}");
            Console.WriteLine($"..........................");
         }  
    }
}