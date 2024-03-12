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

                    var inputDefValue = defValue.GetProperty("input");
                    //var inputDefinition = new SchemaDefinition();
                    //inputDefinition.Encoding = inputDefValue.GetProperty("encoding").GetString();

                    //var inputSchemaDefValue = inputDefValue.GetProperty("schema");
                    //inputDefinition.Type = inputSchemaDefValue.GetProperty("type").GetString();
                    //inputDefinition.Required = inputSchemaDefValue.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ToArray();

                    //var inputPropertiesDefValue = inputSchemaDefValue.GetProperty("properties");
                    //var inputPropertiesDefinition = new List<PropertyDefinition>();
                    //foreach (var pdef in inputPropertiesDefValue.EnumerateObject())
                    //{
                    //    inputPropertiesDefinition.Add(new PropertyDefinition()
                    //    {
                    //        Name = pdef.Name,
                    //        Type = pdef.Value.GetProperty("type").GetString(),
                    //        Description = pdef.Value.GetPropertyStringOrDefault("description"),
                    //    });
                    //}

                    //inputDefinition.Properties = inputPropertiesDefinition.ToArray();
                    //procedureDefinition.InputSchema = inputDefinition;

                    procedureDefinition.InputSchema = s_loadSchemaDefition(inputDefValue);

                    // Output Schema

                    //var outputDefValue = defValue.GetProperty("output");
                    //var outputDefinition = new SchemaDefinition();
                    //outputDefinition.Encoding = outputDefValue.GetProperty("encoding").GetString();

                    //var outputSchemaDefValue = outputDefValue.GetProperty("schema");
                    //outputDefinition.Type = outputDefValue.GetProperty("type").GetString();
                    //outputDefinition.Required = outputSchemaDefValue.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ToArray();




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
            }

            return endpointDefinition;
        }

        private static SchemaDefinition s_loadSchemaDefition(JsonElement schemaDefValue)
        {
            var schemaDefinition = new SchemaDefinition();
            schemaDefinition.Encoding = schemaDefValue.GetProperty("encoding").GetString();

            var schemaSchemaDefValue = schemaDefValue.GetProperty("schema");
            schemaDefinition.Type = schemaSchemaDefValue.GetProperty("type").GetString();
            schemaDefinition.Required = schemaSchemaDefValue.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ToArray();

            var schemaPropertiesDefValue = schemaSchemaDefValue.GetProperty("properties");
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
            return schemaDefinition;
        }
    }
}
