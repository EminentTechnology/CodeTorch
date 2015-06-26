using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAnt.Core;
using NAnt.Core.Attributes;
using CodeTorch.Core;
using System.CodeDom.Compiler;

namespace Eminent.AppBuilder.BuildTasks
{
    [TaskName("appbuilderCompiler")]
    public class AppBuilderCompiler: Task
    {
        [TaskAttribute("rootNamespace", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string RootNamespace { get; set; }

        [TaskAttribute("configurationFolder", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string ConfigurationFolder { get; set; }

        [TaskAttribute("outputLocation", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string OutputLocation { get; set; }

        [TaskAttribute("company", Required = false)]
        public string Company { get; set; }

        [TaskAttribute("copyright", Required = false)]
        public string Copyright { get; set; }

        [TaskAttribute("fileVersion", Required = false)]
        public string FileVersion { get; set; }

        [TaskAttribute("version", Required = false)]
        public string Version { get; set; }

        [TaskAttribute("product", Required = false)]
        public string Product { get; set; }

        [TaskAttribute("title", Required = false)]
        public string Title { get; set; }

        [TaskAttribute("trademark", Required = false)]
        public string Trademark { get; set; }

        protected override void ExecuteTask()
        {
            CompilerOptions options = new CompilerOptions();

            options.RootNamespace = this.RootNamespace;
            options.ConfigurationFolder = this.ConfigurationFolder;
            options.Company = this.Company;
            options.Copyright = this.Copyright;
            options.FileVersion = this.FileVersion;
            options.Version = this.Version;
            options.Product = this.Product;
            options.Title = this.Title;
            options.Trademark = this.Trademark;
            options.OutputLocations.Add(this.OutputLocation);

            CompilerResults results = Compiler.GenerateConfigurationAssembly(options);

            if (results.Errors.Count == 0)
            {
                Project.Log(Level.Info, "Build successful");
            }
            else
            {
                Project.Log(Level.Info, "Build failed");

                

                foreach (CompilerError e in results.Errors)
                {
                    string type = e.IsWarning ? "Warning" : "Error";
                    Project.Log(Level.Debug, "{0} {1} - {2} - in {3} [{4},{5}]", type, e.ErrorNumber, e.ErrorText, e.FileName, e.Line, e.Column);
                }
            }
        }
    }
}
