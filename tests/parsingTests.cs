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
        //https://stackoverflow.com/questions/826398/is-it-possible-to-dynamically-compile-and-execute-c-sharp-code-fragments
        public static void test2()
        {
         // define source code, then parse it (to the type used for compilation)
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }");

            // define other necessary objects for compilation
            string assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(Object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            //https://github.com/dotnet/roslyn/issues/49498
            Assembly.GetEntryAssembly().GetReferencedAssemblies()
                .ToList()
                .ForEach(a => references.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

            // analyse and generate IL code from syntax tree
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                // write IL code into memory
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    // handle exceptions
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
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
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    // create instance of the desired class and call the desired function
                    Type type = assembly.GetType("RoslynCompileSample.Writer");
                    object obj = Activator.CreateInstance(type);
                    type.InvokeMember("Write",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { "Hello World" });
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