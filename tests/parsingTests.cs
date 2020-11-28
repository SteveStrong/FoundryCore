using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;

namespace FoundryCore
{
    //https://github.com/dotnet/roslyn/issues/49498
    class ParsingTests
    {
         public static void test3()
         {
            string code1 = @"public void Write(string message, int count)
                            {
                                Console.WriteLine(message);
                                    for(int i = 0; i < count; i++)
                                    {    
                                        var xxx = i * 2 * 5 + 100;
                                        var str = $""{message} => {xxx}"";
                                        Console.WriteLine(str);
                                    }
                            }";

            string code2 = @"public double Math(double a, double b)
                            {
                                var c = a * b;
                                var str = $""{a} * {b} => {c}"";
                                Console.WriteLine(str);
                                return c;
                            }";

            string code = $"{code1} {code2}";

            string nameSpace = "Apprentice";
            string className = "Writer";
         
            var trans = new Transpiler();
            var body = trans.WrapInClass(nameSpace, className, code);
            
            // Console.WriteLine($"BODY: {body}");
            var compile = trans.Compile(body);

            Assembly assembly;
            if ( trans.LoadAssembly(compile, out assembly) ) {

                string target = $"{nameSpace}.{className}";
                Type type = assembly.GetType(target);
                var obj = Activator.CreateInstance(type);
                type.InvokeMember("Write",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    new object[] { "Hello: World", 3 });

                var result = type.InvokeMember("Math",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { 17.0, 3.0 });

                    var str = $" result => {result}";
                    Console.WriteLine(str);

            }

        }
        //https://stackoverflow.com/questions/826398/is-it-possible-to-dynamically-compile-and-execute-c-sharp-code-fragments
        public static void test2()
        {
            // define source code, then parse it (to the type used for compilation)
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace Apprentice
                {
                    public class Writer
                    {
                        public void Write(string message, int count)
                        {
                            Console.WriteLine(message);
                              for(int i = 0; i < count; i++)
                                {    
                                    var xxx = i * 2 * 5 + 100;
                                    var str = $""{message} => {xxx}"";
                                    Console.WriteLine(str);
                                }
                        }

                        public double Math(double a, double b)
                        {
                            var c = a * b;
                            var str = $""{a} * {b} => {c}"";
                            Console.WriteLine(str);
                            return c;
                        }
                    }
                }");

            // define other necessary objects for compilation
            string assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(Object).Assembly.Location),
            };

            //https://github.com/dotnet/roslyn/issues/49498
            Assembly.GetEntryAssembly().GetReferencedAssemblies()
                .ToList()
                .ForEach(a => references.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

            // analyse and generate IL code from syntax tree
            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var stream = new MemoryStream())
            {
                // write IL code into memory
                EmitResult result = compilation.Emit(stream);

                if (!result.Success)
                {
                    // handle exceptions
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    // load this 'virtual' DLL so that we can use
                    stream.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(stream.ToArray());

                    // create instance of the desired class and call the desired function
                    Type type = assembly.GetType("Apprentice.Writer");
                    var obj = Activator.CreateInstance(type);
                    type.InvokeMember("Write",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { "Hello: World", 3 });

                    var resultx = type.InvokeMember("Math",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { 17.0, 3.0 });

                    var str = $" resultx => {resultx}";
                    Console.WriteLine(str);
                }
            }

            Console.ReadLine();
        }
    


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