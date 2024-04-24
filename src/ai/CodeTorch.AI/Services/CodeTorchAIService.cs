using CodeTorch.AI.Abstractions;
using CodeTorch.AI.Models;
using CodeTorch.Core;
using Microsoft.SemanticKernel;
using System;
using System.Threading.Tasks;

namespace CodeTorch.AI.Services
{
    public class CodeTorchAIService: ICodeTorchAIService
    {
        const string modelName = "gpt-4-turbo";

        public Kernel GetKernel()
        {
            var openAiKey = Environment.GetEnvironmentVariable("OPEN_AI_KEY");

            if (string.IsNullOrEmpty(openAiKey))
            {
                throw new Exception("OpenAI key not found");
            }

            var builder = Kernel.CreateBuilder();
            builder.Services.AddOpenAIChatCompletion(modelName, openAiKey);

            Kernel kernel = builder.Build();

            return kernel;
        }

        public KernelPlugin GetPluginsFromPromptsDirectory(Kernel kernel)
        {
            var assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var promptsPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyPath), "prompts");

            var prompts = kernel.CreatePluginFromPromptDirectory(promptsPath);
            return prompts;
        }

        public async Task<AnalyzeRequestResponse> GetRecommendationsForRestService(string request, string existingRestServicesJson)
        {
            try
            {
                var arguments = new KernelArguments
                {
                    { "request", request },
                    { "existingRestServices", existingRestServicesJson },
                };

                var kernel = GetKernel();
                var prompts = GetPluginsFromPromptsDirectory(kernel);
                var function = prompts["RecommendRestService"];
                var functionResult = await kernel.InvokeAsync(function, arguments);
                var resultJson = functionResult.ToString();
            
                // remove the first and last line if it starts with `
                if (resultJson.StartsWith("```json"))
                {
                    resultJson = resultJson.Substring(7, resultJson.Length - 10);
                }

                var recommendations = Newtonsoft.Json.JsonConvert.DeserializeObject<AnalyzeRequestResponse>(resultJson);

                return recommendations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new AnalyzeRequestResponse
                {
                    RecommendNewRestService = true,
                    Recommendations = new System.Collections.Generic.List<NewRestServiceRecommendation>(),
                    Reason = $"Error Processing - {ex.Message}"
                };
            }
        }

        public Task<string> GenerateRestServiceDescription(RestService service)
        {
            return Task.FromResult(service.Description);
        }

        public async Task<DataCommand> UpdateDataCommandDescription(DataCommand command, string DatabaseProjectFolder = null)
        {
            try
            {
                //serialize data command to json
                var dataCommandJson = Newtonsoft.Json.JsonConvert.SerializeObject(command);

                string sql = command.Text;
                if (command.Type.ToString().ToLower() == "storedprocedure")
                {
                    //look in database project folder recursively for file with same name as sql
                    if (DatabaseProjectFolder != null)
                    {
                        var files = System.IO.Directory.GetFiles(DatabaseProjectFolder, $"{command.Name}.sql", System.IO.SearchOption.AllDirectories);
                        if (files.Length > 0)
                        {
                            sql = System.IO.File.ReadAllText(files[0]);
                        }
                    }
                }

                var arguments = new KernelArguments
                {
                    { "datacommandJson", dataCommandJson },
                    { "sql", sql },
                };

                var kernel = GetKernel();
                var prompts = GetPluginsFromPromptsDirectory(kernel);
                var function = prompts["UpdateDataCommandDescription"];
                var functionResult = await kernel.InvokeAsync(function, arguments);
                var resultJson = functionResult.ToString();

                // remove the first and last line if it starts with `
                if (resultJson.StartsWith("```json"))
                {
                    resultJson = resultJson.Substring(7, resultJson.Length - 10);
                }

                var updatedDataCommand = Newtonsoft.Json.JsonConvert.DeserializeObject<DataCommand>(resultJson);

                //update the description
                command.Description = updatedDataCommand.Description ?? command.Description;
                command.Tables = updatedDataCommand.Tables ?? command.Tables;

                //update the  parameters descriptions
                foreach (var parameter in command.Parameters)
                {
                    var updatedParameter = updatedDataCommand.Parameters.Find(x => x.Name == parameter.Name);
                    if (updatedParameter != null)
                    {
                        parameter.Description = updatedParameter.Description ?? parameter.Description;
                    }
                }

                //update the  columns descriptions
                foreach (var column in command.Columns)
                {
                    var updatedColumn = updatedDataCommand.Columns.Find(x => x.Name == column.Name);
                    if (updatedColumn != null)
                    {
                        column.Description = updatedColumn.Description ?? column.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw new Exception($"Error Processing - {ex.Message}", ex);
            }

            return command;
        }
    }
}
