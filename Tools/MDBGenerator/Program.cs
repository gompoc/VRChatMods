using System.IO;
using Mono.Cecil;
using Mono.Cecil.Mdb;
using Mono.Cecil.Pdb;

namespace MDBGenerator
{
    internal static class Program
    {
        static int Main(string[] args)
        {
            var assemblyToProcess = args[0];

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(assemblyToProcess));

            var readerParams = new ReaderParameters {
                AssemblyResolver = assemblyResolver,
                ReadSymbols = true,
                ReadWrite = true,
                SymbolReaderProvider = new PdbReaderProvider(),
            };

            using var assembly = AssemblyDefinition.ReadAssembly(new FileStream(args[0], FileMode.Open, FileAccess.ReadWrite), readerParams);
            
            assembly.Write(new WriterParameters {SymbolWriterProvider = new MdbWriterProvider(), WriteSymbols = true, });

            return 0;
        }
    }
}