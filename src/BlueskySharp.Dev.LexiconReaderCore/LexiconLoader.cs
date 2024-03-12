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
                    procedureDefinition.Type = defValue.GetProperty("type").GetString();

                    // Input Schema
                    procedureDefinition.Input = s_loadIODefition(defValue.GetProperty("input"));

                    // Output Schema
                    procedureDefinition.Output = s_loadIODefition(defValue.GetProperty("output"));

                    // Errors
                    var errorsDefValue = defValue.GetProperty("errors");
                    var errorsDefinition = new List<ErrorDefinition>();
                    foreach (var errorDef in errorsDefValue.EnumerateArray())
                    {
                        errorsDefinition.Add(new ErrorDefinition()
                        {
                            Name = errorDef.GetProperty("name").GetString(),
                        });
                    }

                    procedureDefinition.Errors = errorsDefinition.ToArray();

                    endpointDefinition.Procedure = procedureDefinition;
                }
                else if (defValue.GetProperty("type").GetString() == "object")
                {

                }
            }

            return endpointDefinition;
        }

        private static ProcedureIODefinition s_loadIODefition(JsonElement ioDefValue)
        {
            var ioDefinition = new ProcedureIODefinition();
            ioDefinition.Encoding = ioDefValue.GetProperty("encoding").GetString();

            var schemaDefValue = ioDefValue.GetProperty("schema");
            var schemaDefinition = new SchemaDefinition();
            schemaDefinition.Type = schemaDefValue.GetProperty("type").GetString();
            schemaDefinition.Required = schemaDefValue.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ToArray();

            var schemaPropertiesDefValue = schemaDefValue.GetProperty("properties");
            var schemaPropertiesDefinition = new List<PropertyDefinition>();
            foreach (var pdef in schemaPropertiesDefValue.EnumerateObject())
            {
                schemaPropertiesDefinition.Add(new PropertyDefinition()
                {
                    Name = pdef.Name,
                    Type = pdef.Value.GetProperty("type").GetString(),
                    Description = pdef.Value.GetPropertyStringOrDefault("description"),
                });
            }

            schemaDefinition.Properties = schemaPropertiesDefinition.ToArray();

            ioDefinition.Schema = schemaDefinition;
            return ioDefinition;
        }
    }
}
