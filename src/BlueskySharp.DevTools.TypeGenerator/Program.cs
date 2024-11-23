using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlueskySharp.DevTools.TypeGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("> ");

            // 処理するディレクトリのパス
            string directoryPath = Console.ReadLine();

            // ディレクトリを再帰的に検索してすべてのJSONファイルを取得
            var jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.AllDirectories);

            foreach (var jsonFile in jsonFiles)
            {
                // JSONファイルを読み込み
                var json = File.ReadAllText(jsonFile);
                var schema = JsonSerializer.Deserialize<LexiconSchema>(json);

                // 名前空間とクラス名を取得
                string namespaceName = schema.Namespace ?? "DefaultNamespace";
                string className = schema.Title;

                // プロパティを取得
                var properties = schema.Properties;
                var requiredProperties = schema.Required;

                if (schema.Properties == null)
                    continue;

                // C#クラスを生成
                var classDefinition = GenerateCSharpClass(namespaceName, className, properties, requiredProperties);

                // クラス定義をファイルに書き込み
                string outputDirectory = Path.Combine(directoryPath, namespaceName.Replace('.', '/'));
                Directory.CreateDirectory(outputDirectory);
                File.WriteAllText(Path.Combine(outputDirectory, $"{className}.cs"), classDefinition);

                Console.WriteLine($"{className}.cs generated successfully in namespace {namespaceName}!");
            }
        }

        private static string GenerateCSharpClass(string namespaceName, string className, Dictionary<string, JsonElement> properties, List<string> requiredProperties)
        {
            var classTemplate = $"namespace {namespaceName}\n{{\n";
            classTemplate += $"    public class {className}\n    {{\n";
            foreach (var property in properties)
            {
                string propName = property.Key;
                string propType = ConvertToCSharpType(property.Value.GetProperty("type").GetString());
                bool isRequired = requiredProperties.Contains(propName);

                classTemplate += $"        public {propType} {propName} {{ get; set; }}{(isRequired ? " // Required" : "")}\n";
            }
            classTemplate += "    }\n";
            classTemplate += "}";

            return classTemplate;
        }

        private static string ConvertToCSharpType(string jsonType)
        {
            return jsonType switch
            {
                "string" => "string",
                "integer" => "int",
                "boolean" => "bool",
                "array" => "List<object>", // 配列の型は必要に応じて変更
                "object" => "object",
                _ => "object"
            };
        }
    }
}