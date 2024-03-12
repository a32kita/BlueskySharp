using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class ConvertedClass
    {
        public string FullName
        {
            get;
            set;
        }

        public string Name
        {
            get => this.FullName.Split('.').Last();
        }

        public string NameSpace
        {
            get
            {
                var classFullNameSplitted = this.FullName.Split('.');
                return String.Join(".", classFullNameSplitted.Take(classFullNameSplitted.Length - 1));
            }
        }

        public ConvertedMethod[] Methods
        {
            get;
            set;
        }

        public SchemaDefinition[] Entities
        {
            get;
            set;
        }

        private static string s_convertToPascalCase(string input)
        {
            string[] parts = input.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }
            return string.Join(".", parts);
        }

        public static ConvertedClass GenerateClass(IEnumerable<EndpointDefinition> endpoints)
        {
            var result = new ConvertedClass();

            var firstEndpoint = endpoints.First();
            var firstEndpointIdSplitted = firstEndpoint.Id.Split('.');
            result.FullName = s_convertToPascalCase(String.Join(".", firstEndpointIdSplitted.Take(firstEndpointIdSplitted.Length - 1)));

            var convertedMethods = new List<ConvertedMethod>();
            var procedureEndpoints = endpoints.Where(epd => epd.Procedure != null);
            foreach (var pepds in procedureEndpoints)
            {
                var methodName = s_convertToPascalCase(pepds.Id.Split('.').Last());

                var inputParameters = new List<ConvertedParameter>();
                if (pepds.Procedure.Input != null)
                {
                    var schemaProperties = pepds.Procedure.Input.Schema.Properties;
                    if (pepds.Procedure.Input.Schema.Type == "ref")
                    {
                        // TODO
                        // →該当なし？
                        schemaProperties = new PropertyDefinition[0];
                    }

                    foreach (var inputProperty in schemaProperties)
                    {
                        var inputParameter = new ConvertedParameter();
                        inputParameter.Name = s_convertToPascalCase(inputProperty.Name);
                        inputParameter.Required = pepds.Procedure.Input.Schema.Required?.Contains(inputProperty.Name) ?? false;
                        inputParameter.Summary = inputParameter.Summary ?? String.Empty;

                        switch (inputProperty.Type)
                        {
                            case "string":
                                inputParameter.Type = "string";
                                break;
                            case "integer":
                                inputParameter.Type = "int";
                                break;
                            default:
                                inputParameter.Type = "Object";
                                break;
                        }

                        inputParameters.Add(inputParameter);
                    }
                }

                var returnValue = new ConvertedEntity() { TypeName = "void" };
                if (pepds.Procedure.Output?.Schema != null)
                {
                    var procedureOutputSchema = pepds.Procedure.Output.Schema;
                    if (procedureOutputSchema.Type == "ref")
                    {
                        returnValue.TypeName = s_convertToPascalCase(procedureOutputSchema.Ref.TrimStart('#'));
                    }
                    else
                    {
                        returnValue.TypeName = methodName + "Result";
                    }
                }

                convertedMethods.Add(new ConvertedMethod()
                {
                    Name = methodName,
                    Summary = pepds.Procedure.Description ?? String.Empty,
                    EndpointName = pepds.Id,
                    Parameters = inputParameters.ToArray(),
                    ReturnValue = returnValue,
                });
            }

            result.Methods = convertedMethods.ToArray();

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textWriter"></param>
        public void WriteCSharpCodeTo(TextWriter textWriter)
        {
            textWriter.WriteLine("namespace {0}", this.NameSpace);
            textWriter.WriteLine("{");
            textWriter.WriteLine("    public class {0}", this.Name);
            textWriter.WriteLine("    {");

            foreach (var method in this.Methods)
            {
                textWriter.WriteLine("        /// <summary>");
                textWriter.WriteLine("        /// {0}", method.Summary);
                textWriter.WriteLine("        /// ({0})", method.EndpointName);
                textWriter.WriteLine("        /// </summary>");
                foreach (var parameter in method.Parameters)
                {
                    textWriter.WriteLine("        /// <param name=\"{0}\">{1}</param>", parameter.Name, parameter.Summary);
                }

                textWriter.Write("        public {0} {1}", method.ReturnValue.TypeName, method.Name);
                textWriter.Write("({0})", String.Join(", ", method.Parameters.Select(p => p.Type + " " + p.Name + (p.Required ? "" : " = default(" + p.Type + ")"))));
                textWriter.WriteLine();
                textWriter.WriteLine("        {");


                textWriter.WriteLine("        }");
                textWriter.WriteLine();
            }

            textWriter.WriteLine("    }");
            textWriter.WriteLine("}");
            textWriter.WriteLine();
        }
    }
}
