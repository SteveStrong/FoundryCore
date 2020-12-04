using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace FoundryCore
{
    public static class ConstructionTests
    {
        public static void test1()
        {
            var root = new ModelComponent("Component");

            Console.WriteLine($"===========================");


            Console.WriteLine($"{root.AsJson(WritingDetails.DETAILS)}");


            Console.WriteLine($"..........................");
         }  

 
    }
}
