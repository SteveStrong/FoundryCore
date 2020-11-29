using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace FoundryCore
{
    public class ComputeX { 
        public bool getvalue(FoComponent root) { 
            var depthProp = new FoReference("depth"); 
            var depth = depthProp.GetValue<double>(root);
            var widthProp = new FoReference("width"); 
            var width = widthProp.GetValue<double>(root);
            var heightProp = new FoReference("height"); 
            var height = heightProp.GetValue<double>(root);
            var volumeProp = new FoReference("volume"); 
            var volume = volumeProp.GetValue<double>(root);
            var areaProp = new FoReference("area"); 
            var area = areaProp.GetValue<double>(root);

            var result = depth * ( width * height ) == volume && width * height == area;

            return result; 
        } 
    } 
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

            root.Properties.AddList(props);

            var parser = new Parser("depth * (width * height) == volume and width * height == area");

            var formula = parser.ReadFormula();

            var list = new List<Operator>();
            formula.CollectionAll(list);

            var rawData = list
                        .GroupBy(item => item.Name)
                        .Select ( item => item.First())
                        .Where( item => item is ReferenceOperator)
                        .Select( item => $"{((ReferenceOperator)item).ToReferenceStatement()} ")
                        .ToList<string>();
                        
            var data = String.Join("\n", rawData);

            var json = formula.toJson();
            var cSharp = formula.AsCSharp();
            var decompile = formula.Decompile();

            string code = $"{data}\n\nvar result = {cSharp};\n\n return result;";
            // Console.WriteLine($"code: {code}");


            string nameSpace = "Apprentice";
            string className = "Compute";
            string typeName = "bool";
            string methodName = "ComputeValue";
            string args = "(FoComponent root)";
       
            var trans = new Transpiler();
            var body = trans.WrapInClassMethod(nameSpace, className, typeName, methodName, args, code);
            
            Console.WriteLine($"===========================");
            
            //Console.WriteLine($"BODY: {body}");

            Assembly assembly;
            var compile = trans.Compile(body);
            if (trans.LoadAssembly(compile, out assembly))
            {

                string target = $"{nameSpace}.{className}";
                Type type = assembly.GetType(target);
                var obj = Activator.CreateInstance(type);
                var thanks = type.InvokeMember(methodName,
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    new object[] { root });

                Console.WriteLine($"It Works if the answer is true");
                 Console.WriteLine($"{methodName} => {thanks}");
            }

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
