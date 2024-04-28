using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

namespace Eminent.CodeGenerator
{
    public class TemplateEngine
    {
        private CompilerResults _compilerResults;
        public TemplateEngine()
        {
        }

        public object Compile(Template t)
        {
            string className = String.IsNullOrEmpty(t.ClassName) ? t.FileName.ToLower().Replace(".", "_") : t.ClassName;
            t.ClassName = className;

            CodeDomProvider provider = GetProvider(t.Language);

            CompilerParameters cp = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");

            foreach (string r in t.Assemblies)
            {
                cp.ReferencedAssemblies.Add(r + ".dll");
            }

            _compilerResults = provider.CompileAssemblyFromSource(cp, GenerateCode(t, provider));

            if (_compilerResults.Errors.Count == 0)
            {
                Assembly a = _compilerResults.CompiledAssembly;
                return a.CreateInstance("_Template." + t.ClassName);
            }

            return null;
        }

        private CodeDomProvider GetProvider(string language)
        {
            if (language.ToLower().Trim() == "vb")
            {
                return new VBCodeProvider();
            }
            else
            {
                return new CSharpCodeProvider();
            }
        }

        public string GenerateCode(Template t)
        {
            CodeDomProvider provider = GetProvider(t.Language);
            return GenerateCode(t, provider);
        }

        public string GenerateCode(Template t, CodeDomProvider provider)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            CodeNamespace _TemplateNS = new CodeNamespace("_Template");
            _TemplateNS.Imports.Add(new CodeNamespaceImport("System"));
            _TemplateNS.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
            _TemplateNS.Imports.Add(new CodeNamespaceImport("System.IO"));
            _TemplateNS.Imports.Add(new CodeNamespaceImport("System.Reflection"));

            CodeTypeDeclaration _templateClass = new CodeTypeDeclaration(t.ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public
            };

            if (!string.IsNullOrEmpty(t.BaseClass))
            {
                _templateClass.BaseTypes.Add(new CodeTypeReference(t.BaseClass));
            }

            _TemplateNS.Types.Add(_templateClass);

            // Other members: fields, properties, methods...
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            provider.GenerateCodeFromNamespace(_TemplateNS, sw, options);
            return sb.ToString();
        }

        public static string GenerateCode(object template)
        {
            string retVal = "";
            // *** Try to run it

            try
            {
                //check to make sure all public ref type properties are not null
                Type obj = template.GetType();
                PropertyInfo[] properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //TODO: take validation out of this function and put in another static method that throws err..so we can give better ui cues
                string errMsg = String.Empty;

                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = (PropertyInfo)properties[i];


                    if (property.PropertyType.IsClass)
                    {
                        if (property.CanWrite)
                        {
                            if (property.GetValue(template, null) == null)
                            {
                                errMsg += property.Name + " is required.";

                            }

                        }
                    }
                }

                if (errMsg != String.Empty)
                {
                    throw new ApplicationException(errMsg);
                }

                // *** Just invoke the method directly through Reflection
                retVal = (string)template.GetType().InvokeMember("GenerateCode", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, template, null);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }


        public CompilerErrorCollection Errors
        {
            get { return _compilerResults?.Errors; }
        }
    }
}
