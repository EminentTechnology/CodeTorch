using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public class CompilerOptions
    {
        public string RootNamespace { get; set; }

        public string ConfigurationFolder { get; set; }

        public string Company { get; set; }

        public string Copyright { get; set; }

        public string FileVersion { get; set; }

        public string Version { get; set; }

        public string Product { get; set; }

        public string Title { get; set; }

        public string Trademark { get; set; }

        List<string> _outputLocations = new List<string>();

        public List<string> OutputLocations
        {
            get { return _outputLocations; }
            set { _outputLocations = value; }
        }

    }

    public class Compiler
    {
        public static CompilerResults GenerateConfigurationAssembly(string configurationPath, string rootNamespace, List<string> outputLocations)
        {
            CompilerOptions options = new CompilerOptions();

            options.ConfigurationFolder = configurationPath;
            options.RootNamespace = rootNamespace;
            options.OutputLocations = outputLocations;

            return GenerateConfigurationAssembly(options);
        }
        public static CompilerResults GenerateConfigurationAssembly(CompilerOptions options)
        {
            CompilerResults retVal = null;

            var tempFolderPath = String.Format("{0}{1}", Path.GetTempPath(), Guid.NewGuid().ToString());


            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            //Copy all the files
            foreach (string file in Directory.GetFiles(options.ConfigurationFolder, "*.*",
                SearchOption.AllDirectories))
            {
                if (!
                    (
                        file.ToLower().Contains("backups\\") ||
                        file.ToLower().Contains("bin\\") ||
                        file.ToLower().Contains("obj\\") ||
                        file.ToLower().Contains("obj\\") ||
                        file.ToLower().EndsWith(".csproj") ||
                        file.ToLower().EndsWith(".vbproj") ||
                        file.ToLower().EndsWith(".config") ||
                        file.ToLower().EndsWith(".user") ||
                        file.ToLower().EndsWith(".vb") ||
                        file.ToLower().EndsWith(".cs") ||
                        file.ToLower().Contains("svn\\")
                    )
                   )
                {
                    string destinationFileName = file.Replace(options.ConfigurationFolder, String.Empty).Replace("\\", ".");

                    if (destinationFileName.StartsWith("."))
                        destinationFileName = destinationFileName.Substring(1);

                    destinationFileName = String.Format("{0}\\{1}.{2}", tempFolderPath, options.RootNamespace, destinationFileName);

                    File.Copy(file, destinationFileName);
                }
            }


            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters compilerParameters = new CompilerParameters();

            compilerParameters.OutputAssembly = String.Format("{0}\\{1}.dll", tempFolderPath, options.RootNamespace);
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = false;
            compilerParameters.IncludeDebugInformation = false;
            compilerParameters.WarningLevel = 3;
            compilerParameters.TreatWarningsAsErrors = false;
            compilerParameters.CompilerOptions = "/optimize";

            //add default references
            compilerParameters.ReferencedAssemblies.Add("system.dll"); //includes
            compilerParameters.ReferencedAssemblies.Add("system.core.dll"); //includes



            foreach (string file in Directory.GetFiles(tempFolderPath))
            {
                compilerParameters.EmbeddedResources.Add(file);
            }



            retVal = provider.CompileAssemblyFromFile(compilerParameters);

            if (retVal.Errors.Count == 0)
            {
                foreach (string s in options.OutputLocations)
                {
                    File.Copy(retVal.PathToAssembly, String.Format("{0}\\{1}.dll", s, options.RootNamespace), true);
                }

                Directory.Delete(tempFolderPath, true);
            }

            return retVal;
        }
    }
}
