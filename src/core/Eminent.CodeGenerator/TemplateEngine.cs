using System;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Runtime.Remoting;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Linq;



namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for TemplateEngine.
	/// </summary>
	public class TemplateEngine
	{
		private CompilerResults _compilerResults;
		public TemplateEngine()
		{

		}

		public object Compile(Template t)
		{
			object retVal = null;
            string className = String.IsNullOrEmpty(t.ClassName) ? t.FileName.ToLower().Replace(".", "_") : t.ClassName;

            t.ClassName = className;

			ICodeCompiler cc = new CSharpCodeProvider().CreateCompiler();

			CompilerParameters cp = new CompilerParameters();

			//add default references
			cp.ReferencedAssemblies.Add("system.dll"); //includes
            cp.ReferencedAssemblies.Add("system.core.dll"); //includes

			//add aditional references
			foreach(string r in t.Assemblies)
			{
				cp.ReferencedAssemblies.Add(r + ".dll");
			}

			cp.GenerateExecutable = false; //generate executable
			cp.GenerateInMemory = true;

			_compilerResults = cc.CompileAssemblyFromSource(cp, this.GenerateCode(t));

			if(_compilerResults.Errors.Count == 0)
			{
				Assembly a = _compilerResults.CompiledAssembly;
				retVal = a.CreateInstance("_Template." + t.ClassName);
			}

			return retVal;
		}

		public string GenerateCode(Template t)
		{
			StringBuilder retVal = new StringBuilder();

			//TODO: comment code section
            string className = String.IsNullOrEmpty(t.ClassName) ? t.FileName.ToLower().Replace(".", "_") : t.ClassName;


			ICodeGenerator codeGenerator;
			CodeDomProvider codeProvider;
			switch(t.Language.ToLower().Trim())
			{
				case "vb":
					codeGenerator = new VBCodeProvider().CreateGenerator();
					codeProvider = new VBCodeProvider();
					break;
				case "c#":
				default:
					codeGenerator = new CSharpCodeProvider().CreateGenerator();
					codeProvider = new CSharpCodeProvider();
					break;
			}
			
			codeGenerator = codeProvider.CreateGenerator();

			//create namespace for class
			CodeNamespace _TemplateNS = new CodeNamespace("_Template");
			
			//add required namespaces
			_TemplateNS.Imports.Add(new CodeNamespaceImport("System"));
			_TemplateNS.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
			_TemplateNS.Imports.Add(new CodeNamespaceImport("System.IO"));
			_TemplateNS.Imports.Add(new CodeNamespaceImport("System.Reflection"));

			//add additional namesspaces
			foreach(string ns in t.Namespaces)
			{
				_TemplateNS.Imports.Add(new CodeNamespaceImport(ns));
			}

			//create class
            
            CodeTypeDeclaration _templateClass = new CodeTypeDeclaration(className);
				
			_TemplateNS.Types.Add(_templateClass);
			_templateClass.IsClass = true;

			//add base class if any
			if(t.BaseClass.Trim() != string.Empty)
			{
				_templateClass.BaseTypes.Add(new CodeTypeReference(t.BaseClass));
			}

			//create constructor
			CodeConstructor _constructor = new CodeConstructor();
			_constructor.Attributes = MemberAttributes.Public;
			_templateClass.Members.Add(_constructor);

			//add private response stringbuider field
			CodeMemberField responsefield = new CodeMemberField();
			responsefield.Name = "responseSB";
			responsefield.Type = new CodeTypeReference("System.Text.StringBuilder");
			responsefield.Attributes = MemberAttributes.Private;
			CodeObjectCreateExpression  responseSBCreateExpression = new CodeObjectCreateExpression ();
			responseSBCreateExpression.CreateType = responsefield.Type;
			responsefield.InitExpression = responseSBCreateExpression;
			_templateClass.Members.Add(responsefield);

			
			//add private variables - properties
			foreach(Property p in t.Properties)
			{
				CodeMemberField field = new CodeMemberField();
				field.Name = "_" + p.Name.ToLower();
				field.Type = new CodeTypeReference(p.Type);
				field.Attributes = MemberAttributes.Private;

				if(p.Default.Trim() != String.Empty)
				{
					
					field.InitExpression = new CodePrimitiveExpression( System.Convert.ChangeType(  p.Default, System.Type.GetType(p.Type)));
				}
				else
				{
					if(System.Type.GetType(p.Type) == typeof(string))
					{
						field.InitExpression = new CodePrimitiveExpression( System.Convert.ChangeType(  p.Default, System.Type.GetType(p.Type)));
					}
				}

				_templateClass.Members.Add(field);
			}

			//add properties - take care of attributes for property grid
			foreach(Property p in t.Properties)
			{
				CodeMemberProperty prop = new CodeMemberProperty();
				prop.Name = p.Name;
				prop.Type = new CodeTypeReference(p.Type);
				prop.Attributes = MemberAttributes.Public;
				
				prop.HasGet = true;
				prop.HasSet = true;

				CodeFieldReferenceExpression reference = new CodeFieldReferenceExpression();
				reference.FieldName = "_" + p.Name.ToLower();
				prop.GetStatements.Add(new CodeMethodReturnStatement(reference));

				CodeAssignStatement assignment = new CodeAssignStatement();
				assignment.Left = reference;
				assignment.Right = new CodeArgumentReferenceExpression("value");
				prop.SetStatements.Add(assignment);
				assignment = null;

				//add attributes
				if(p.Description.Trim() != string.Empty)
				{
					CodeAttributeDeclaration attrib = new CodeAttributeDeclaration("System.ComponentModel.DescriptionAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(p.Description)));
					prop.CustomAttributes.Add(attrib);
				}

				if(p.Category.Trim() != string.Empty)
				{
					CodeAttributeDeclaration attrib = new CodeAttributeDeclaration("System.ComponentModel.CategoryAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(p.Category)));
					prop.CustomAttributes.Add(attrib);
				}


				_templateClass.Members.Add(prop);

			}
	
    

 


			CodeMemberMethod WriteMethod = new CodeMemberMethod();
			WriteMethod.Parameters.Add( new CodeParameterDeclarationExpression( new CodeTypeReference("System.String") ,"text"));
			WriteMethod.ReturnType = new CodeTypeReference("System.Text.StringBuilder");
			WriteMethod.Name = "Write";
			WriteMethod.Attributes = MemberAttributes.Public;
			WriteMethod.Statements.Add( new CodeSnippetExpression(" return responseSB.Append(text)"));
			_templateClass.Members.Add(WriteMethod);



			//add method with actual code in it
			// create a public method named "GenerateCode"
			CodeMemberMethod GenerateCodeMethod = new CodeMemberMethod();
			GenerateCodeMethod.Name = "GenerateCode";
			GenerateCodeMethod.Attributes = MemberAttributes.Public;
	   		GenerateCodeMethod.ReturnType = new CodeTypeReference("System.String");
	   		GenerateCodeMethod.Statements.Add( new CodeSnippetExpression(t.CodeSnippet.ToString()));
			_templateClass.Members.Add(GenerateCodeMethod);

         
			
			//text writer to write the code
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.BracingStyle = "C";

			TextWriter _writer = new StringWriter(retVal);
			codeGenerator.GenerateCodeFromNamespace(_TemplateNS, _writer, options);

			return retVal.ToString();

			
			
			
		}

		public CompilerErrorCollection Errors
		{
			get
			{
				return _compilerResults.Errors;
			}
			
		}


		public static string GenerateCode(object template) 
		{
			string retVal = "";
			// *** Try to run it
			
			try 
			{
				//check to make sure all public ref type properties are not null
				Type obj = template.GetType();
				PropertyInfo[] properties =  obj.GetProperties(BindingFlags.Public|BindingFlags.Instance);


				//TODO: take validation out of this function and put in another static method that throws err..so we can give better ui cues
				string errMsg = String.Empty;

				for(int i=0;i<properties.Length;i++)
				{
					PropertyInfo property = (PropertyInfo)properties[i];
					
					
					if(property.PropertyType.IsClass)
					{
						if(property.CanWrite)
						{							
							if(property.GetValue(template,null) == null)
							{
								errMsg += property.Name + " is required.";
								
							}
							
						}
					}
				}

				if(errMsg != String.Empty)
                    throw new ApplicationException(errMsg);




				// *** Just invoke the method directly through Reflection
				retVal =  (string) template.GetType().InvokeMember("GenerateCode", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, template, null);
				//retVal = retVal.Replace("\\r\\n","\r\n");

				
			}
			catch(Exception ex) 
			{
				throw ex;
			}

			return retVal;
		}

		

		
		
	}
}
