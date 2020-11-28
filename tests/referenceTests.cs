using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;

namespace FoundryCore
{
    class ReferenceTests
    {
        public static void test1()
        {

            var builder = new FoClass("FoComponent");
            var root = builder.ConstructInstanceCore("") as FoComponent;

            IFoProperty[] props = {
                new FoProperty<double>("width", 100),
                new FoProperty<double>("height", .33),
                new FoProperty<double>("area", () => {
                    var width = new FoReference("width");
                    var height = new FoReference("height");
                    var area = width.GetValue<double>(root) * height.GetValue<double>(root);
                    return area;
                })
            };
            root.Properties.AddList(props);


            //var root = new FoComponent("Comp1", props);

            Console.WriteLine($"===========================");
            Console.WriteLine($"{root.AsJson()}");
            Console.WriteLine($"..........................");
         }   
    }
}
