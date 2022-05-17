using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace Azure.Functions.Tracing.Internal
{
    internal class FunctionsConfig
    {
        public static string AssemblyFromFilename(string filename)
        {
            return Path.GetFileNameWithoutExtension(filename);
        }

        public static string AssemblyFromEntrypoint(string? entrypoint)
        {
            if (entrypoint == null) throw new ArgumentNullException("AssemblyFromEntrypoint requires a valid entrypoint string");
            return entrypoint.Substring(0, entrypoint.IndexOf('.'));
        }

        public static string ClassFromEntrypoint(string? entrypoint)
        {
            if (entrypoint == null) throw new ArgumentNullException("AssemblyFromEntrypoint requires a valid entrypoint string");
            return entrypoint.Substring(0, entrypoint.LastIndexOf('.'));
        }

        public static Dictionary<string, List<string>> Read()
        {

            var func = new Dictionary<string, List<string>>();
            var rootDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".."));

            foreach (var dir in Directory.GetDirectories(rootDirectory))
            {
                var configFile = Path.Combine(dir, "function.json");
                if (dir != "bin" && File.Exists(configFile))
                {
                    using (var file = new StreamReader(configFile))
                    {
                        JsonDocument root = JsonDocument.Parse(file.ReadToEnd());

                        var asm = AssemblyFromEntrypoint(root.RootElement.GetProperty("entryPoint").GetString());
                        var cls = ClassFromEntrypoint(root.RootElement.GetProperty("entryPoint").GetString());
                        if (!func.ContainsKey(asm))
                            func.Add(asm, new List<string>(new string[] { cls }));
                        else if (!func[asm].Contains(cls))
                            func[asm].Add(cls);
                    }
                }
            }
            return func;
        }


    }
}
