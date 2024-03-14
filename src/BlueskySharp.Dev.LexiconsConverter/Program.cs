using BlueskySharp.Dev.LexiconReaderCore;
using System.Text;

namespace BlueskySharp.Dev.LexiconsConverter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Lexicon JSON Conveter");

            fileSpc:
            var inputPath = String.Empty;
            if (args.Length == 0 )
            {
                Console.WriteLine("Please specify source JSON file;");
                Console.Write("> ");
                inputPath = Console.ReadLine()?.Trim('"').Trim();
            }
            else
            {
                inputPath = args[0];
            }

            if (String.IsNullOrEmpty(inputPath) || (File.Exists(inputPath) == false && Directory.Exists(inputPath) == false))
                goto fileSpc;

            var targetJsonFiles = new List<string>();
            if (File.Exists(inputPath))
            {
                targetJsonFiles.Add(inputPath);
            }
            else
            {
                targetJsonFiles.AddRange(Directory.GetFiles(inputPath, "*.json", SearchOption.TopDirectoryOnly));
            }

            var endpointDefinitions = new List<EndpointDefinition>();
            foreach (var jsonPath in targetJsonFiles)
            {
                using (var fs = File.OpenRead(jsonPath))
                {
                    var epDef = await LexiconLoader.LoadEndpointDefinition(fs);
                    endpointDefinitions.Add(epDef);
                }
            }

            Console.WriteLine();
            var prcEpds = endpointDefinitions.Where(epd => epd.Procedure != null);
            foreach (var epd in prcEpds)
            {
                Console.WriteLine("[PRC] {0} (Input={1}, Output={2})",
                    epd.Id,
                    epd.Procedure.Input?.Schema?.Properties?.Length ?? 0,
                    epd.Procedure.Output?.Schema?.Properties?.Length ?? 0);
            }

            var objEpds = endpointDefinitions.Where(epd => epd.Objects != null && epd.Objects.Count > 0);
            foreach (var epd in objEpds)
            {
                foreach (var obj in epd.Objects)
                {
                    Console.WriteLine("[OBJ] {0} {1} (Property={2})", epd.Id, obj.Key, obj.Value.Properties.Length);
                }
            }

            Console.WriteLine();

            var converted = ConvertedClass.GenerateClass(endpointDefinitions);
            converted.WriteCSharpCodeTo(Console.Out);

            using (var sw = new StreamWriter(File.OpenWrite(converted.FullName + ".cs"), Encoding.UTF8))
            {
                converted.WriteCSharpCodeTo(sw);
            }

            Console.WriteLine();
            Console.WriteLine("Please press [Enter] key to exit ...");
            Console.ReadLine();
        }
    }
}
