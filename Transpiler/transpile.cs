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
    //https://stackoverflow.com/questions/826398/is-it-possible-to-dynamically-compile-and-execute-c-sharp-code-fragments
    public class Transpiler
    {
        public SyntaxTree WrapInClass(string nameSpace, string className, string code){

            string classCode = $"public class {className} {Common.WrapCurly(code)}";
            string assemblyCode = $"namespace {nameSpace} {Common.WrapCurly(classCode)}";
            string body = $"using System;  {assemblyCode}";
            var syntaxTree = CSharpSyntaxTree.ParseText(body);
            return syntaxTree;
        }

        public CSharpCompilation Compile(SyntaxTree tree){
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
                syntaxTrees: new[] { tree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return compilation;
        }

        public Boolean LoadAssembly(CSharpCompilation compilation, out Assembly assembly)
        {
            using (var stream = new MemoryStream())
            {
                assembly = null;
                // write IL code into memory
                EmitResult result = compilation.Emit(stream);

                if (result.Success)
                {
                     // load this 'virtual' DLL so that we can use
                    stream.Seek(0, SeekOrigin.Begin);
                    assembly = Assembly.Load(stream.ToArray());
                    return true;
                }

                // handle exceptions
                var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    Console.Error.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                }
            }
            return false;
        } 
    }
}