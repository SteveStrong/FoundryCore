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
            init();
        }

        private void init() {

            this.Properties.AddArray( new[] {width, height, depth, area, volume} );

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
        public static void test4()
        {
            var prop = FoPropertyBuilder.Start()
                .SetName("volume");

            var build = FoComponentBuilder.Start()
                .SetName("Box")
                .AddProperty<string>("label")
                .AddProperty<int>("tag", 100)
                .AddProperty<double>("width")
                .AddProperty<double>("depth")
                .AddProperty<double>("height")
                
                .AddProperty(prop.Build<double>())
                .AddProperty(prop.SetName("title").Build<string>());

            var result1 = build.Build();
            Console.WriteLine($"{result1.AsJson()}");


            var result2 = build
                .SetName("Steve")
                .AddProperty<int>("tag2", 1234)
                .Build();

            Console.WriteLine($"{result2.AsJson()}");
        }

        public static void test3()
        {
            //http://www.stefanoricciardi.com/2010/04/14/a-fluent-builder-in-c/
            var emp = EmployeeBuilderDirector.NewEmployee
                .SetName("Steve Strong")
                .AtPosition("Software Developer")
                .WithSalary(3500)
                .Build();

            Console.WriteLine(emp);

            var me = EmployeeBuilderDirector
                .NewEmployee
                .SetName("steve")
                .Build();

            Console.WriteLine(me);
        }

        public static void test2()
        {

            Console.WriteLine($"===========================");

            var methodName = "ComputeValue";
            Type type = typeof(ModelComponent);
            var obj = Activator.CreateInstance(type,new object[] { "steve"}) as ModelComponent;

            var thanks = type.InvokeMember(methodName,
            BindingFlags.Default | BindingFlags.InvokeMethod,
            null,
            obj,
            new object[] { });

            Console.WriteLine($"It Works if the answer is true");
            Console.WriteLine($"{methodName} => {thanks}");
            
            //var volume = root.volume.Value;
            Console.WriteLine($"{obj.volume}");
            //Console.WriteLine($"{root.volume.AsJson()}");

            //var area = root.area.Value;
            Console.WriteLine($"{obj.area}");

            obj.width.Value = 0;
            Console.WriteLine($"{obj.area}");


            Console.WriteLine($"..........................");
         } 
        public static void test1()
        {
            var root = new ModelComponent("Component");

            Console.WriteLine($"===========================");


            Console.WriteLine($"It Works if the answer is true");
            Console.WriteLine($"ComputeValue => {root.ComputeValue()}");
            
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
