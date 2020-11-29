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
         public static void test2()
        {
            var root = new FoComponent("Component");
            IFoProperty[] props = {
                new FoProperty<double>("width", 100),
                new FoProperty<double>("height", .33),
                new FoProperty<double>("depth", 22),
                new FoProperty<double>("area", () => {
                    var width = new FoReference("width");
                    var height = new FoReference("height");
                    var depth = new FoReference("depth");
                    var area = width.GetValue<double>(root) * height.GetValue<double>(root);
                    var volume = area * depth.GetValue<double>(root);
                    return area;
                }),
                new FoProperty<double>("volume", () => {
                    var area = new FoReference("area");
                    var depth = new FoReference("depth");
                    var volume = area.GetValue<double>(root) * depth.GetValue<double>(root);
                    return volume;
                })
            };

            var parser = new Parser("depth * (width * height) ");

            var formula = parser.ReadFormula();
            var json = formula.toJson();
            var cSharp = formula.AsCSharp();
            var decompile = formula.Decompile();

            Console.WriteLine($"===========================");
            Console.WriteLine($"C#: {cSharp}");
            Console.WriteLine($"code: {decompile}");
            //Console.WriteLine($"{json}");
            //Console.WriteLine($"{root.AsJson()}");
            Console.WriteLine($"..........................");
         }  

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
