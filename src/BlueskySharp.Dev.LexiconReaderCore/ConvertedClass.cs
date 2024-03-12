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

        public ConvertedEntity[] Entities
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

        private static string s_getDotnetTypeName(PropertyDefinition propertyDefinition)
        {
            switch (propertyDefinition.Type)
            {
                case "string":
                    return "string";
                case "integer":
                    return "int";
                default:
                    return "Object";
            }
        }

        public static ConvertedClass GenerateClass(IEnumerable<EndpointDefinition> endpoints)
        {
            var result = new ConvertedClass();

            var firstEndpoint = endpoints.First();
            var firstEndpointIdSplitted = firstEndpoint.Id.Split('.');
            result.FullName = s_convertToPascalCase(String.Join(".", firstEndpointIdSplitted.Take(firstEndpointIdSplitted.Length - 1)));

            var knownEntities = new List<ConvertedEntity>();

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

                        //switch (inputProperty.Type)
                        //{
                        //    case "string":
                        //        inputParameter.Type = "string";
                        //        break;
                        //    case "integer":
                        //        inputParameter.Type = "int";
                        //        break;
                        //    default:
                        //        inputParameter.Type = "Object";
                        //        break;
                        //}

                        inputParameter.Type = s_getDotnetTypeName(inputProperty);

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

                        var returnValueProperties = new List<ConvertedProperty>();
                        foreach (var outputProperty in procedureOutputSchema.Properties)
                        {
                            var rvProperty = new ConvertedProperty();
                            rvProperty.Name = s_convertToPascalCase(outputProperty.Name);
                            rvProperty.Required = procedureOutputSchema.Required?.Contains(outputProperty.Name) ?? false;
                            rvProperty.Type = s_getDotnetTypeName(outputProperty);

                            //switch (outputProperty.Type)
                            //{
                            //    case "string":
                            //        rvProperty.Type = "string";
                            //        break;
                            //    case "integer":
                            //        rvProperty.Type = "int";
                            //        break;
                            //    default:
                            //        rvProperty.Type = "Object";
                            //        break;
                            //}

                            returnValueProperties.Add(rvProperty);
                        }

                        returnValue.Properties = returnValueProperties.ToArray();
                    }

                    knownEntities.Add(returnValue);
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

            var objectEndpoints = endpoints.Where(edps => edps.Objects != null && edps.Objects.Count > 0);
            foreach (var oedp in objectEndpoints)
            {
                foreach (var objDefKvp in oedp.Objects)
                {
                    // 先にコレクションに追加しておくか、既存のものを取得
                    var convEntity = new ConvertedEntity();
                    var convEntityAddeds = knownEntities.Where(item => item.TypeName == s_convertToPascalCase(objDefKvp.Key));
                    if (convEntityAddeds.Count() > 0)
                        convEntity = convEntityAddeds.Single();
                    else
                    {
                        convEntity.TypeName = s_convertToPascalCase(objDefKvp.Key);
                        knownEntities.Add(convEntity);
                    }

                    var convertedProperties = new List<ConvertedProperty>();
                    foreach (var propDef in objDefKvp.Value.Properties)
                    {
                        var cvProperty = new ConvertedProperty();
                        cvProperty.Name = s_convertToPascalCase(propDef.Name);
                        cvProperty.Required = objDefKvp.Value.Required?.Contains(propDef.Name) ?? false;
                        cvProperty.Type = s_getDotnetTypeName(propDef);

                        convertedProperties.Add(cvProperty);
                    }

                    convEntity.Properties = convertedProperties.ToArray();
                }
            }

            result.Methods = convertedMethods.ToArray();
            result.Entities = knownEntities.ToArray();

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

            foreach (var entity in this.Entities)
            {
                textWriter.WriteLine("        /// <summary>");
                textWriter.WriteLine("        /// Entity");
                textWriter.WriteLine("        /// </summary>");

                textWriter.WriteLine("        public class {0}", entity.TypeName);
                textWriter.WriteLine("        {");

                if (entity.Properties != null)
                {
                    foreach (var property in entity.Properties)
                    {
                        textWriter.WriteLine("            /// <summary>");
                        textWriter.WriteLine("            /// {0}", property.Summary);
                        textWriter.WriteLine("            /// </summary>");
                        textWriter.WriteLine("            public {0} {1}", property.Type, property.Name);
                        textWriter.WriteLine("            {");
                        textWriter.WriteLine("                get; set;");
                        textWriter.WriteLine("            }");
                    }
                }
                else
                {
                    // ERROR
                    textWriter.WriteLine("            // ERROR: No member definition detected.");
                }

                textWriter.WriteLine("        }");
                textWriter.WriteLine();
            }

            textWriter.WriteLine("    }");
            textWriter.WriteLine("}");
            textWriter.WriteLine();
        }
    }
}
