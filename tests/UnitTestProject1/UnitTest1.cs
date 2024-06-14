using CodeTorch.AI.Services;
using CodeTorch.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public const string ConfigurationPath = @"C:\Sandbox\evisitor-backend\src\eVisitor.Services.Customer.Config";



        //create  test setup method 

        [TestInitialize]
        public void TestInitialize()
        {
            ConfigurationLoader.LoadFromConfigurationFolder(ConfigurationPath);
        }

        [TestMethod]
        public void TestMethod1()
        {

            //get all data commands
            var commands = CodeTorch.Core.Configuration.GetInstance().DataCommands;
            //get all rest services
            var restservices = CodeTorch.Core.Configuration.GetInstance().RestServices;

            var ServiceHeader = restservices.Select(x => new
            {
                x.Name,
                x.Folder,
                x.Resource,
                x.Description,
                x.SupportJSON,
                x.SupportXML
            }).ToList();

            // serialize package to json
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ServiceHeader);

            Console.WriteLine($"{commands.Count} data commands");
            Console.WriteLine($"{restservices.Count} rest services");
            Console.WriteLine($"combined size as text {json.Length} characters");
            Console.WriteLine();
            Console.WriteLine(json);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var a = new CodeTorchAIService();

            var command = CodeTorch.Core.Configuration.GetInstance().DataCommands.Find(x=> x.Name == "Customer_AddLocationDevice");

            var result = await a.UpdateDataCommandDescription(command, @"C:\Sandbox\evisitor-backend\src\eVisitor.DB");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
        }
    }
}
