using BlueskySharp.Dev.LexiconReaderCore.InternalExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public static class LexiconLoader
    {
        public static async Task<EndpointDefinition> LoadEndpointDefinition(Stream lexiconJsonStream)
        {
            var endpointDefinition = new EndpointDefinition();
            var jsonDocument = await JsonDocument.ParseAsync(lexiconJsonStream);
            var jsonRootElement = jsonDocument.RootElement;

            endpointDefinition.Lexicon = jsonRootElement.GetProperty("lexicon").GetUInt32();
            endpointDefinition.Id = jsonRootElement.GetProperty("id").GetString();

            var defs = jsonRootElement.GetProperty("defs");
            foreach (var def in defs.EnumerateObject())
            {
                var defValue = def.Value;

                if (defValue.GetProperty("type").GetString() == "procedure")
                {
                    var procedureDefinition = new ProcedureDefinition();
                    procedureDefinition.Description = defValue.GetProperty("description").GetString();

                    var inputDefValue = defValue.GetProperty("input");
                    var inputDefinition = new SchemaDefinition();
                    inputDefinition.Encoding = inputDefValue.GetProperty("encoding").GetString();

                    var schemaDefValue = inputDefValue.GetProperty("schema");
                    inputDefinition.Type = schemaDefValue.GetProperty("type").GetString();
                    inputDefinition.Required = schemaDefValue.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ToArray();

                    var propertiesDefValue = schemaDefValue.GetProperty("properties");
                    var propertiesDefinition = new List<PropertyDefinition>();
                    foreach (var pdef in propertiesDefValue.EnumerateObject())
                    {
                        //propertiesDefinition.Add(new PropertyDefinition()
                        //{
                        //    Name = pdef.Name,
                        //    Type = pdef.Value.GetProperty("type").GetString(),
                        //    Description = pdef.Value.GetProperty("description").GetString(),
                        //});

                        var propertyDefinition = new PropertyDefinition();
                        propertyDefinition.Name = pdef.Name;
                        propertyDefinition.Type = pdef.Value.GetProperty("type").GetString();

                        //var descriptionJsonElement = new JsonElement();
                        //if (pdef.Value.TryGetProperty("description", out descriptionJsonElement))
                        //    propertyDefinition.Description = descriptionJsonElement.GetString();

                        propertyDefinition.Description = pdef.Value.GetPropertyStringOrDefault("description");

                        propertiesDefinition.Add(propertyDefinition);
                    }

                    inputDefinition.Properties = propertiesDefinition.ToArray();
                    procedureDefinition.InputSchema = inputDefinition;

                    endpointDefinition.Procedure = procedureDefinition;
                }
            }

            return endpointDefinition;
        }
    }
}
