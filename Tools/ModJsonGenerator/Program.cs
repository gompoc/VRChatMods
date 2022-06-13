using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mono.Cecil;
using Newtonsoft.Json;

namespace ModJsonGenerator
{
    internal static class Program
    {
        // Scuffed program to auto generate mod jsons so I can submit them easily to the vrcmg
        static int Main(string[] args)
        {
            using var assembly = AssemblyDefinition.ReadAssembly(new FileStream(args[0], FileMode.Open, FileAccess.ReadWrite));
            
            if (args.Length != 5)
            {
                goto cleanup;
            }
            
            var modType = assembly.MainModule.Types.SingleOrDefault(it => it.BaseType?.Name == "MelonMod");

            bool isMod = true;
            
            if (modType == null)
            {
                var pluginType = assembly.MainModule.Types.SingleOrDefault(it => it.BaseType?.Name == "MelonPlugin");

                if (pluginType == null)
                {
                    Console.Error.WriteLine("MelonPlugin or MelonMod wasn't found");
                    return 1;
                }
                isMod = false;
            }

            
            var modJsonAttribute = assembly.CustomAttributes.SingleOrDefault(attribute =>
                attribute.AttributeType.Name.Equals("ModJsonInfoAttribute"));

            if (modJsonAttribute == null)
            {
                Console.Error.WriteLine("ModJsonInfoAttribute not found");
                return 1;
            }
            
            var melonInfoAttribute = assembly.CustomAttributes.SingleOrDefault(attribute =>
                attribute.AttributeType.Name.Equals("MelonInfoAttribute"));
                

            if (melonInfoAttribute == null)
            {
                Console.Error.WriteLine("MelonInfoAttribute not found");
                return 1;
            }

            Mod ourMod = new Mod
            {
                _id = (int)modJsonAttribute.ConstructorArguments[0].Value,
                description = (string) modJsonAttribute.ConstructorArguments[1].Value,
                searchtags = ((CustomAttributeArgument[]) modJsonAttribute.ConstructorArguments[2].Value).ToStringArray(),
                requirements = ((CustomAttributeArgument[]) modJsonAttribute.ConstructorArguments[3].Value).ToStringArray(),
                changelog = (string) modJsonAttribute.ConstructorArguments[4].Value,
                embedcolor = (string) modJsonAttribute.ConstructorArguments[5].Value,
                name = (string) melonInfoAttribute.ConstructorArguments[1].Value,
                modversion = (string) melonInfoAttribute.ConstructorArguments[2].Value,
                author = (string) melonInfoAttribute.ConstructorArguments[3].Value,
                loaderversion = assembly.MainModule.AssemblyReferences.SingleOrDefault(a => a.Name.Equals("MelonLoader")).Version.ToString()
            };


            var globalGameManagersPath = Path.Combine(args[1], "VRChat_Data", "globalgamemanagers");
            Match match = ParseRegex.Match(Encoding.ASCII.GetString(File.ReadAllBytes(globalGameManagersPath)));

            ourMod.vrchatversion = $"Build {match.Groups[1]}";

            ourMod.modType = isMod ? "Mod" : "Plugin";

            ourMod.sourcelink = $"{args[3]}/tree/master/{ourMod.name}";

            var currentDate = DateTime.Now;

            ourMod.downloadlink = $"{args[3]}/releases/download/{currentDate.Year}-{currentDate.Month:00}-{currentDate.Day:00}/{ourMod.name}.dll";

            var generatedJson = JsonConvert.SerializeObject(ourMod, Formatting.Indented);

            var modJsonFolder = Path.Combine(args[2], "ModJsons");
            
            Directory.CreateDirectory(modJsonFolder);
            
            File.WriteAllText(Path.Combine(modJsonFolder, $"{ourMod.name}.json"), generatedJson);

        cleanup:
            var attributeToNuke = assembly.CustomAttributes.SingleOrDefault(attribute =>
                attribute.AttributeType.Name.Equals("ModJsonInfoAttribute"));
            if (attributeToNuke != null)
                assembly.CustomAttributes.Remove(attributeToNuke);

            var assemblyRefToNuke = assembly.MainModule.AssemblyReferences.SingleOrDefault(a => a.Name.Equals("ModJsonGenerator"));
            if (assemblyRefToNuke != null)
                assembly.MainModule.AssemblyReferences.Remove(assemblyRefToNuke);
            
            assembly.Write();
            
            return 0;
        }

        
        private static readonly Regex ParseRegex = new (@"(?:\d|.)-(\d+)-");
        private static string[] ToStringArray(this IReadOnlyList<CustomAttributeArgument> customAttributeArguments)
        {
            if (customAttributeArguments == null)
                return null;
            
            var arr = new string[customAttributeArguments.Count];
            for (var i = 0; i < customAttributeArguments.Count; i++)
            {
                arr[i] = (string)customAttributeArguments[i].Value;
            }
            return arr;
        }
    }
}