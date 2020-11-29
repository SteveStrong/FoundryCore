using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace FoundryCore
{

    public class ModelComponent : FoComponent
    {
        public FoProperty<double> width = new FoProperty<double>("width", 10);
        public FoProperty<double> height = new FoProperty<double>("height", .33);
        public FoProperty<double> depth = new FoProperty<double>("depth", 22);

        public FoProperty<double> area = new FoProperty<double>("Area");
        public FoProperty<double> volume = new FoProperty<double>("Volume");


        public ModelComponent(string name): base(name)
        {
            this.Properties.AddList( new[] {width, height, depth, area, volume} );

            area.Formula = () =>
            {
                return width.Value * height.Value;
            };

            volume.Formula = () =>
            {
                return area.Value * depth.Value;
            };
        }

        public bool ComputeValue() {
            return depth.Value * (width.Value * height.Value) == volume.Value && width.Value * height.Value == area.Value;
        }
    }
    class ModelTests
    {
         public static void test1()
        {
            var root = new ModelComponent("Component");

            Console.WriteLine($"===========================");

            var methodName = "ComputeValue";
            Type type = typeof(ModelComponent);
            var obj = Activator.CreateInstance(type,new object[] { "steve"});

            var thanks = type.InvokeMember(methodName,
            BindingFlags.Default | BindingFlags.InvokeMethod,
            null,
            obj,
            new object[] { });

            Console.WriteLine($"It Works if the answer is true");
            Console.WriteLine($"{methodName} => {thanks}");
            
            //var volume = root.volume.Value;
            Console.WriteLine($"{root.volume}");
            //Console.WriteLine($"{root.volume.AsJson()}");

            //var area = root.area.Value;
            Console.WriteLine($"{root.area}");

            root.width.Value = 0;
            Console.WriteLine($"{root.area}");


            Console.WriteLine($"..........................");
         }  

 
    }
}
