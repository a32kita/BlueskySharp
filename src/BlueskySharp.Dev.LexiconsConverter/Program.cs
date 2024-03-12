using BlueskySharp.Dev.LexiconReaderCore;

namespace BlueskySharp.Dev.LexiconsConverter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Lexicon JSON Conveter");

            fileSpc:
            var jsonPath = String.Empty;
            if (args.Length == 0 )
            {
                Console.WriteLine("Please specify source JSON file;");
                Console.Write("> ");
                jsonPath = Console.ReadLine()?.Trim('"').Trim();
            }
            else
            {
                jsonPath = args[0];
            }

            if (String.IsNullOrEmpty(jsonPath) || File.Exists(jsonPath) == false)
                goto fileSpc;

            using (var fs = File.OpenRead(jsonPath))
            {
                var epDef = await LexiconLoader.LoadEndpointDefinition(fs);
                Console.WriteLine();
            }
        }
    }
}
