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
            endpointDefinition.Objects = new Dictionary<string, SchemaDefinition>();

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
                    var inputDef = defValue.GetPropertyElementOrNull("input");
                    if (inputDef != null)
                        procedureDefinition.Input = s_loadIODefition(inputDef.Value);

                    // Output Schema
                    var outputDef = defValue.GetPropertyElementOrNull("output");
                    if (outputDef != null)
                        procedureDefinition.Output = s_loadIODefition(outputDef.Value);

                    // Errors
                    var errorsDefValue = defValue.GetPropertyElementOrNull("errors");
                    if (errorsDefValue != null)
                    {
                        var errorsDefinition = new List<ErrorDefinition>();
                        foreach (var errorDef in errorsDefValue.Value.EnumerateArray())
                        {
                            errorsDefinition.Add(new ErrorDefinition()
                            {
                                Name = errorDef.GetProperty("name").GetString(),
                            });
                        }
                        procedureDefinition.Errors = errorsDefinition.ToArray();
                    }

                    endpointDefinition.Procedure = procedureDefinition;
                }
                else if (defValue.GetProperty("type").GetString() == "object")
                {
                    endpointDefinition.Objects.Add(def.Name, s_loadSchemaDefinition(def.Value));
                }
            }

            return endpointDefinition;
        }

        private static ProcedureIODefinition s_loadIODefition(JsonElement ioDefValue)
        {
            var ioDefinition = new ProcedureIODefinition();
            ioDefinition.Encoding = ioDefValue.GetProperty("encoding").GetString();
            ioDefinition.Schema = s_loadSchemaDefinition(ioDefValue.GetProperty("schema"));

            return ioDefinition;
        }

        private static SchemaDefinition s_loadSchemaDefinition(JsonElement schemaDefValue)
        {
            var schemaDefinition = new SchemaDefinition();
            schemaDefinition.Type = schemaDefValue.GetProperty("type").GetString();

            if (schemaDefinition.Type == "ref")
            {
                schemaDefinition.Ref = schemaDefValue.GetProperty("ref").GetString();
            }

            var requiredDef = schemaDefValue.GetPropertyElementOrNull("required");
            if (requiredDef != null)
                schemaDefinition.Required = requiredDef.Value.EnumerateArray().Select(e => e.GetString()).ToArray();

            var schemaPropertiesDefValue = schemaDefValue.GetPropertyElementOrNull("properties");
            if (schemaPropertiesDefValue != null)
            {
                var schemaPropertiesDefinition = new List<PropertyDefinition>();
                foreach (var pdef in schemaPropertiesDefValue.Value.EnumerateObject())
                {
                    schemaPropertiesDefinition.Add(new PropertyDefinition()
                    {
                        Name = pdef.Name,
                        Type = pdef.Value.GetProperty("type").GetString(),
                        Description = pdef.Value.GetPropertyStringOrDefault("description"),
                    });
                }

                schemaDefinition.Properties = schemaPropertiesDefinition.ToArray();
            }

            return schemaDefinition;
        }
    }
}
