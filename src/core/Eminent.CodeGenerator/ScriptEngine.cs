using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;

using Microsoft.CSharp;
using Microsoft.VisualBasic;


namespace Eminent.CodeGenerator
{
	public enum ScriptEngineLanguage
	{
		CSharp,
		VB
	}


	/// <summary>
	/// Summary description for ScriptEngine.
	/// </summary>
	public class ScriptEngine
	{
		
		

		protected Assembly Assembly = null;
		protected ICodeCompiler Compiler = null;
		protected CompilerResults Results = null;
		protected CompilerParameters Parameters = null;
		protected string Namespaces = "";
		protected AppDomain AppDomain = null;
		protected string OutputAssembly = null;

		private bool IsFirstLoad = true;
		
		public bool HasError = false;

		public ScriptEngine()
		{
			this.Language = ScriptEngineLanguage.CSharp;
		}

		public ScriptEngine(ScriptEngineLanguage language)
		{
			this.Language = language;
		}

		private ScriptEngineLanguage _Language = ScriptEngineLanguage.CSharp;
		public ScriptEngineLanguage Language
		{
			get
			{
				return _Language;
			}
			set
			{
				_Language = value;

				switch(_Language)
				{
					case ScriptEngineLanguage.CSharp:
						this.Compiler = new CSharpCodeProvider().CreateCompiler();
						break;
					case ScriptEngineLanguage.VB:
						this.Compiler = new VBCodeProvider().CreateCompiler();
						break;
				}

				this.Parameters = new CompilerParameters();
				
			}
		}

		private object _ObjectReference = null;
		/// <summary>
		/// The object reference to the compiled object available after the first method call.
		/// You can use this method to call additional methods on the object.
		/// For example, you can use CallMethod and pass multiple methods of code each of
		/// which can be executed indirectly by using CallMethod() on this object reference.
		/// </summary>
		public object ObjectReference
		{
			get
			{
				return _ObjectReference;
			}
			set
			{
				_ObjectReference = value;
			}
		}

		private bool _IncludeDefaultAssemblies = true;
		/// <summary>
		/// Determines if default assemblies are added. System, System.IO, System.Reflection
		/// </summary>
		public bool IncludeDefaultAssemblies
		{
			get
			{
				return _IncludeDefaultAssemblies;
			}
			set
			{
				_IncludeDefaultAssemblies = value;
			}
		}

		private string _AssemblyNamespace = "Eminent";
		/// <summary>
		/// Namespace of the assembly created by the script processor. Determines
		/// how the class will be referenced and loaded.
		/// </summary>
		public string AssemblyNamespace
		{
			get
			{
				return _AssemblyNamespace;
			}
			set
			{
				_AssemblyNamespace = value;
			}
		}

		/// <summary>
		/// Name of the class created by the script processor. Script code becomes methods in the class.
		/// </summary>
		private string _ClassName = "CodeGeneratedScript";
		public string ClassName
		{
			get
			{
				return _ClassName;
			}
			set
			{
				_ClassName = value;
			}
		}

		private string _SourceCode = "";
		/// <summary>
		/// Contains the source code of the entired compiled assembly code.
		/// Note: this is not the code passed in, but the full fixed assembly code.
		/// 
		/// </summary>
		public string SourceCode
		{
			get
			{
				return _SourceCode;
			}
			set
			{
				_SourceCode = value;
			}
		}

		private string _ErrorMessage = "";
		public string ErrorMessage
		{
			get
			{
				return _ErrorMessage;
			}
			set
			{
				_ErrorMessage = value;
			}
		}

		private ScriptEngineResponse _Response = null;
		public ScriptEngineResponse Response
		{
			get
			{
				if(_Response == null)
				{
					_Response = new ScriptEngineResponse();
				}

				return _Response;
			}
			set
			{
				_Response = value;
			}
		}

		#region AddAssembly
		/// <summary>
		/// Adds an assembly to the compiled code
		/// </summary>
		/// <param name="AssemblyDll">DLL assembly file name</param>
		/// <param name="Namespace">Namespace to add if any. Pass null if no namespace is to be added</param>
		public void AddAssembly(string AssemblyDll,string Namespace) 
		{
			if (AssemblyDll==null && Namespace == null) 
			{
				// *** clear out assemblies and namespaces
				this.Parameters.ReferencedAssemblies.Clear();
				this.Namespaces = "";
				return;
			}
			
			if (AssemblyDll != null)
				this.Parameters.ReferencedAssemblies.Add(AssemblyDll);
		
			if (Namespace != null) 
			{
				switch(_Language)
				{
					case ScriptEngineLanguage.CSharp:
						this.Namespaces = this.Namespaces + "using " + Namespace + ";\r\n";
						break;
					case ScriptEngineLanguage.VB:
						this.Namespaces = this.Namespaces + "imports " + Namespace + "\r\n";
						break;
				}
			}
				
		}

		/// <summary>
		/// Adds an assembly to the compiled code.
		/// </summary>
		/// <param name="AssemblyDll">DLL assembly file name</param>
		public void AddAssembly(string AssemblyDll) 
		{
			this.AddAssembly(AssemblyDll,null);
		}

		public void AddNamespace(string Namespace)
		{
			this.AddAssembly(null,Namespace);
		}

		public void AddDefaultAssemblies()
		{
			this.AddAssembly("System.dll","System");
			this.AddNamespace("System.Reflection");
			this.AddNamespace("System.IO");
		}

		#endregion

		#region ExecuteMethod
		public object ExecuteMethod(string codeSnippet, string methodName, params object[] parameters) 
		{
			
			if (this.ObjectReference == null) 
			{
				if (this.IsFirstLoad)
				{
					if (this.IncludeDefaultAssemblies) 
					{
						this.AddDefaultAssemblies();
					}
					this.AddAssembly("OchuSolutions.CodeGenerator.RemoteAppDomainLoader.dll","OchuSolutions.CodeGenerator.RemoteAppDomainLoader");
					this.AddAssembly("OchuSolutions.CodeGenerator.dll","OchuSolutions.CodeGenerator");
					this.IsFirstLoad = false;
				}

				StringBuilder sb = new StringBuilder("");

				//*** Program lead in and class header
				sb.Append(this.Namespaces);
				sb.Append("\r\n");

				switch(_Language)
				{
					case ScriptEngineLanguage.CSharp:
						// *** Namespace headers and class definition
						sb.Append("namespace " + this.AssemblyNamespace + "{\r\npublic class " + this.ClassName + ":MarshalByRefObject,IRemoteInterface {\r\n");	
				
						// *** Generic Invoke method required for the remote call interface
						sb.Append(
							"public object Invoke(string methodName,object[] parms) {\r\n" + 
							"return this.GetType().InvokeMember(methodName,BindingFlags.InvokeMethod,null,this,parms );\r\n" +
							"}\r\n\r\n" );

						//*** The actual code to run in the form of a full method definition.
						sb.Append(codeSnippet);

						sb.Append("\r\n} }");  // Class and namespace closed
						break;
					case ScriptEngineLanguage.VB:
						// *** Namespace headers and class definition
						sb.Append("Namespace " + this.AssemblyNamespace + "\r\npublic class " + this.ClassName + "\r\n");
						sb.Append("Inherits MarshalByRefObject\r\nImplements IRemoteInterface\r\n\r\n");	
				
						// *** Generic Invoke method required for the remote call interface
						sb.Append(
							"Public Overridable Overloads Function Invoke(ByVal methodName As String, ByVal Parameters() As Object) As Object _\r\n" +
							"Implements IRemoteInterface.Invoke\r\n" + 
							"return me.GetType().InvokeMember(methodName,BindingFlags.InvokeMethod,nothing,me,Parameters)\r\n" +
							"End Function\r\n\r\n" );

						//*** The actual code to run in the form of a full method definition.
						sb.Append(codeSnippet);

						sb.Append("\r\n\r\nEnd Class\r\nEnd Namespace\r\n");  // Class and namespace closed
						break;
				}

				
				this.SourceCode = sb.ToString();

				if (!this.CompileAssembly(sb.ToString()) )
					return null;

				object tempobject = this.CreateInstance();
				if (tempobject == null)
					return null;
			}

			return this.CallMethod(this.ObjectReference,methodName,parameters);
		}

		/// <summary>
		///  Executes a snippet of code. Pass in a variable number of parameters
		///  (accessible via the parameters[0..n] array) and return an object parameter.
		///  Code should include:  return (object) SomeValue as the last line or return null
		/// </summary>
		/// <param name="codeSnippet"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public object ExecuteCode(string codeSnippet, params object[] parameters) 
		{
            object retVal = null;
			switch(_Language)
			{
				case ScriptEngineLanguage.CSharp:
					retVal= this.ExecuteMethod("public object ExecuteCode(params object[] Parameters) {" + 
						codeSnippet + 
						"}",
						"ExecuteCode",parameters);
					break;
				case ScriptEngineLanguage.VB:
					retVal= this.ExecuteMethod("public function ExecuteCode(ParamArray Parameters() As Object) as object\r\n" + 
						codeSnippet + 
						"\r\nend function\r\n",
						"ExecuteCode",parameters);
					break;
			}

            return retVal;
		}

		#endregion

		#region CompileAssembly
		/// <summary>
		/// Compiles and runs the source code for a complete assembly.
		/// </summary>
		/// <param name="sourceCode"></param>
		/// <returns></returns>
		public bool CompileAssembly(string sourceCode) 
		{
			//this.oParameters.GenerateExecutable = false;

			if (this.AppDomain == null && this.OutputAssembly == null)
			{
				this.Parameters.GenerateInMemory = true;
			}
			else
			{
				if (this.AppDomain != null && this.OutputAssembly == null)
				{
					// *** Generate an assembly of the same name as the domain
					this.OutputAssembly = "ochucodegen_" + Guid.NewGuid().ToString() + ".dll";
					this.Parameters.OutputAssembly = this.OutputAssembly;
				}
				else 
				{
					this.Parameters.OutputAssembly = this.OutputAssembly;
				}
			}
		
			this.Results = this.Compiler.CompileAssemblyFromSource(this.Parameters,sourceCode);

			if (Results.Errors.HasErrors) 
			{
				this.HasError = true;

				// *** Create Error String
				this.ErrorMessage = Results.Errors.Count.ToString() + " Errors:";
				for (int x=0; x < Results.Errors.Count; x++) 
				{
					this.ErrorMessage = this.ErrorMessage  + "\r\nLine: " + Results.Errors[x].Line.ToString() + " - " + 
						Results.Errors[x].ErrorText;				
				}
				return false;
			}

			if (this.AppDomain == null)
				this.Assembly = Results.CompiledAssembly;
			
			return true;
		}
		#endregion

		#region CreateInstance
		public object CreateInstance() 
		{
			if (this.ObjectReference != null) 
			{
				return this.ObjectReference;
			}
			
			// *** Create an instance of the new object
			try 
			{
				if (this.AppDomain == null)
					try 
					{
						this.ObjectReference =  this.Assembly.CreateInstance(this.AssemblyNamespace + "." + this.ClassName);
						return this.ObjectReference;
					}
					catch(Exception ex) 
					{
						this.HasError = true;
						this.ErrorMessage = ex.Message;
						return null;
					}
				else 
				{
					// create the factory class in the secondary app-domain
					RemoteLoaderFactory factory = (RemoteLoaderFactory) this.AppDomain.CreateInstance(  "OchuSolutions.CodeGenerator.RemoteAppDomainLoader", "OchuSolutions.CodeGenerator.RemoteAppDomainLoader.RemoteLoaderFactory" ).Unwrap();

					// with the help of this factory, we can now create a real 'LiveClass' instance
					this.ObjectReference = factory.Create( this.OutputAssembly, this.AssemblyNamespace + "." + this.ClassName, null );

					return this.ObjectReference;			
				}	
			}
			catch(Exception ex) 
			{
				this.HasError = true;
				this.ErrorMessage = ex.Message;
				return null;
			}
				
		}
		#endregion

		#region CallMethod

		public object CallMethod(object obj,string methodName, params object[] parameters) 
		{
			// *** Try to run it
			try 
			{
				if (this.AppDomain == null)
					// *** Just invoke the method directly through Reflection
					return obj.GetType().InvokeMember(methodName,BindingFlags.InvokeMethod,null,obj,parameters );
				else 
				{
					// *** Invoke the method through the Remote interface and the Invoke method
					object oResult;
					try 
					{
						// *** Cast the object to the remote interface to avoid loading type info
						IRemoteInterface oRemote = (IRemoteInterface) obj;

						// *** Indirectly call the remote interface
						oResult = oRemote.Invoke(methodName,parameters);
					}
					catch(Exception ex) 
					{
						this.HasError = true;
						this.ErrorMessage = ex.Message;
						return null;
					}
					return oResult;
				}	
			}
			catch(Exception ex) 
			{
				this.HasError = true;
				this.ErrorMessage = ex.Message;
				
			}
			return null;
		}
		#endregion

		#region CreateAppDomain
		public bool CreateAppDomain(string appDomain) 
		{
			if (appDomain == null)
				appDomain = "GenericAppDomain";

			AppDomainSetup oSetup = new AppDomainSetup();
			oSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

			this.AppDomain = AppDomain.CreateDomain(appDomain,null,oSetup);
			return true;
		}
		#endregion

		#region UnloadAppDomain
		public bool UnloadAppDomain()
		{
			if (this.AppDomain != null)
				AppDomain.Unload(this.AppDomain);

			this.AppDomain = null;

			if (this.OutputAssembly != null) 
			{
				try 
				{
					File.Delete(this.OutputAssembly);
				}
				catch(Exception) {;}
			}

			return true;
		}
		#endregion


		#region ParseScript
		/// <summary>
		/// Parses a script into a compilable program
		/// </summary>
		/// <param name="code"></param
		public string ParseScript(string code)
		{
			if (code == null)
				return "";
			
			

			int lnLast = 0;
			int lnAt2 = 0;
			int lnAt = code.IndexOf("<%",0);
			if (lnAt == -1)
				return code;

			//TODO:vb portion
			// *** Create the Response object which is used to write output into the code generator
			this.Response.Append("ScriptEngineResponse Response = new ScriptEngineResponse();\r\n");

			while (lnAt > -1) 
			{
				if (lnAt > -1)    			 	
					// *** Catch the plain text write out to the Response Stream as is - fix up for quotes
					this.Response.Append("this.Response.Append(@\"" + code.Substring(lnLast,lnAt - lnLast).Replace("\"","\"\"") + "\" );\r\n\r\n");
				
				//*** Find end tag
				lnAt2 = code.IndexOf("%>",lnAt);
				if (lnAt2 < 0)
					break;

				string lcSnippet = code.Substring(lnAt,lnAt2-lnAt + 2);
				if (lcSnippet.Substring(2,1) == "=")
					// *** Write out an expression. 'Eval' inside of a Response.Write call
					this.Response.Append("this.Response.Append(" + lcSnippet.Substring(3,lcSnippet.Length-5).Trim() + ".ToString());\r\n");
				else if (lcSnippet.Substring(2,1) == "@") 
				{
					string lcAttribute = "";

					// *** Handle Directives
					lcAttribute = StrExtract(lcSnippet,"Assembly","=");
					if (lcAttribute.Length > 0) 
					{
						lcAttribute = StrExtract(lcSnippet,"\"","\"");
						if (lcAttribute.Length > 0)
							this.AddAssembly(lcAttribute);
					}		
					else 
					{
						lcAttribute = StrExtract(lcSnippet,"Import","=");
						if (lcAttribute.Length > 0) 
						{
							lcAttribute = StrExtract(lcSnippet,"\"","\"");
							if (lcAttribute.Length > 0)
								this.AddNamespace(lcAttribute);
						}		
					}
				}	
				else
					// *** Write out a line of code as is.
					this.Response.Append(lcSnippet.Substring(2,lcSnippet.Length - 4) + "\r\n");

				lnLast = lnAt2 + 2;
				lnAt = code.IndexOf("<%",lnLast);
				if (lnAt < 0)
					// *** Write out the final block of non-code text
					this.Response.Append("this.Response.Append(@\"" + code.Substring(lnLast,code.Length - lnLast).Replace("\"","\"\"") + "\" );\r\n\r\n");
			}

			this.Response.Append("return this.Response.ToString();");

			return this.Response.ToString();
		}
		#endregion

		#region Release
		public void Release() 
		{
			this.ObjectReference = null;
		}
		#endregion

		#region Dispose
		public void Dispose() 
		{
			this.Release();
			this.UnloadAppDomain();
		}
		#endregion



		~ScriptEngine() 
		{
			this.Dispose();
		}


		#region Helper Functions
		/// <summary>
		/// Searches one string into another string and replaces all occurences with
		/// a blank character.
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.StrTran("Joe Doe", "o");		//returns "J e D e" :)
		/// </pre>
		/// </summary>
		/// <param name="cSearchIn"> </param>
		/// <param name="cSearchFor"> </param>
		private static string StrTran(string cSearchIn, string cSearchFor)
		{
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(cSearchIn);
			
			//Call the Replace() method of the StringBuilder
			return sb.Replace(cSearchFor," ").ToString();
		}

		/// <summary>
		/// Searches one string into another string and replaces all occurences with
		/// a third string.
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.StrTran("Joe Doe", "o", "ak");		//returns "Jake Dake" 
		/// </pre>
		/// </summary>
		/// <param name="cSearchIn"> </param>
		/// <param name="cSearchFor"> </param>
		/// <param name="cReplaceWith"> </param>
		private static string StrTran(string cSearchIn, string cSearchFor, string cReplaceWith)
		{
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(cSearchIn);

			//There is a bug in the replace method of the StringBuilder
			sb.Replace(cSearchFor, cReplaceWith);

			//Call the Replace() method of the StringBuilder and specify the string to replace with
			return sb.Replace(cSearchFor, cReplaceWith).ToString();
		}

		/// Searches one string into another string and replaces each occurences with
		/// a third string. The fourth parameter specifies the starting occurence and the 
		/// number of times it should be replaced
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.StrTran("Joe Doe", "o", "ak", 2, 1);		//returns "Joe Dake" 
		/// </pre>
		private static string StrTran(string cSearchIn, string cSearchFor, string cReplaceWith, int nStartoccurence, int nCount)
		{
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(cSearchIn);

			//There is a bug in the replace method of the StringBuilder
			sb.Replace(cSearchFor, cReplaceWith);

			//Call the Replace() method of the StringBuilder specifying the replace with string, occurence and count
			return sb.Replace(cSearchFor, cReplaceWith, nStartoccurence, nCount).ToString();
		}
 		
		/// <summary>
		/// Receives a string along with starting and ending delimiters and returns the 
		/// part of the string between the delimiters. Receives a beginning occurence
		/// to begin the extraction from and also receives a flag (0/1) where 1 indicates
		/// that the search should be case insensitive.
		/// <pre>
		/// Example:
		/// string cExpression = "JoeDoeJoeDoe";
		/// VFPToolkit.strings.StrExtract(cExpression, "o", "eJ", 1, 0);		//returns "eDo"
		/// </pre>
		/// </summary>
		private static string StrExtract(string cSearchExpression, string cBeginDelim, string cEndDelim, int nBeginOccurence, int nFlags)
		{

			string cstring = cSearchExpression;
			string cb = cBeginDelim;
			string ce = cEndDelim;
			string lcRetVal = "";

			if (nFlags == 1) 
			{
				cb = cb.ToLower();
				ce = ce.ToLower();
				cstring = cstring.ToLower();
			}

			int lnAt = cSearchExpression.IndexOf(cb,0);
			if (lnAt < 0)
				return "";

			int lnAtCut = lnAt + cb.Length ;

			int lnAt2  = cSearchExpression.IndexOf(ce,lnAtCut);
			if (lnAt2 < 0)
				return "";

			return cSearchExpression.Substring(lnAtCut,lnAt2 - lnAtCut);
		}

		/// <summary>
		/// Receives a string and a delimiter as parameters and returns a string starting 
		/// from the position after the delimiter
		/// <pre>
		/// Example:
		/// string cExpression = "JoeDoeJoeDoe";
		/// VFPToolkit.strings.StrExtract(cExpression, "o");		//returns "eDoeJoeDoe"
		/// </pre>
		/// </summary>
		/// <param name="cSearchExpression"> </param>
		/// <param name="cBeginDelim"> </param>
		private static string StrExtract(string cSearchExpression, string cBeginDelim)
		{
			int nbpos = At(cBeginDelim, cSearchExpression);
			return cSearchExpression.Substring(nbpos + cBeginDelim.Length - 1);
		}

		/// <summary>
		/// Receives a string along with starting and ending delimiters and returns the 
		/// part of the string between the delimiters
		/// <pre>
		/// Example:
		/// string cExpression = "JoeDoeJoeDoe";
		/// VFPToolkit.strings.StrExtract(cExpression, "o", "eJ");		//returns "eDo"
		/// </pre>
		/// </summary>
		/// <param name="cSearchExpression"> </param>
		/// <param name="cBeginDelim"> </param>
		/// <param name="cEndDelim"> </param>
		private static string StrExtract(string cSearchExpression, string cBeginDelim, string cEndDelim)
		{
			return StrExtract(cSearchExpression, cBeginDelim, cEndDelim, 1, 0);
		}

		/// <summary>
		/// Receives a string along with starting and ending delimiters and returns the 
		/// part of the string between the delimiters. It also receives a beginning occurence
		/// to begin the extraction from.
		/// <pre>
		/// Example:
		/// string cExpression = "JoeDoeJoeDoe";
		/// VFPToolkit.strings.StrExtract(cExpression, "o", "eJ", 2);		//returns ""
		/// </pre>
		/// </summary>
		/// <param name="cSearchExpression"> </param>
		/// <param name="cBeginDelim"> </param>
		/// <param name="cEndDelim"> </param>
		/// <param name="nBeginOccurence"> </param>
		private static string StrExtract(string cSearchExpression, string cBeginDelim, string cEndDelim, int nBeginOccurence)
		{
			return StrExtract(cSearchExpression, cBeginDelim, cEndDelim, nBeginOccurence, 0);
		}


		/// Private Implementation: This is the actual implementation of the At() and RAt() functions. 
		/// Receives two strings, the expression in which search is performed and the expression to search for. 
		/// Also receives an occurence position and the mode (1 or 0) that specifies whether it is a search
		/// from Left to Right (for At() function)  or from Right to Left (for RAt() function)
		private static int __at(string cSearchFor, string cSearchIn, int nOccurence, int nMode)
		{
			//In this case we actually have to locate the occurence
			int i = 0;
			int nOccured = 0;
			int nPos = 0;
			if (nMode == 1) {nPos = 0;}
			else {nPos = cSearchIn.Length;}

			//Loop through the string and get the position of the requiref occurence
			for (i=1;i<=nOccurence;i++)
			{
				if(nMode == 1) {nPos = cSearchIn.IndexOf(cSearchFor,nPos);}
				else {nPos = cSearchIn.LastIndexOf(cSearchFor,nPos);}

				if (nPos < 0)
				{
					//This means that we did not find the item
					break;
				}
				else
				{
					//Increment the occured counter based on the current mode we are in
					nOccured++;

					//Check if this is the occurence we are looking for
					if (nOccured == nOccurence)
					{
						return nPos + 1;
					}
					else
					{
						if(nMode == 1) {nPos++;}
						else {nPos--;}

					}
				}
			}
			//We never found our guy if we reached here
			return 0;
		}
		/// <summary>
		/// Receives two strings as parameters and searches for one string within another. 
		/// If found, returns the beginning numeric position otherwise returns 0
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.At("D", "Joe Doe");	//returns 5
		/// </pre>
		/// </summary>
		/// <param name="cSearchFor"> </param>
		/// <param name="cSearchIn"> </param>
		private static int At(string cSearchFor, string cSearchIn)
		{
			return cSearchIn.IndexOf(cSearchFor) + 1;
		}

		/// <summary>
		/// Receives two strings and an occurence position (1st, 2nd etc) as parameters and 
		/// searches for one string within another for that position. 
		/// If found, returns the beginning numeric position otherwise returns 0
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.At("o", "Joe Doe", 1);	//returns 2
		/// VFPToolkit.strings.At("o", "Joe Doe", 2);	//returns 6
		/// </pre>
		/// </summary>
		/// <param name="cSearchFor"> </param>
		/// <param name="cSearchIn"> </param>
		/// <param name="nOccurence"> </param>
		private static int At(string cSearchFor, string cSearchIn, int nOccurence)
		{
			return __at(cSearchFor, cSearchIn, nOccurence, 1);
		}


		/// <summary>
		/// Receives two strings as parameters and searches for one string within another. 
		/// This function ignores the case and if found, returns the beginning numeric position 
		/// otherwise returns 0
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.AtC("d", "Joe Doe");	//returns 5
		/// </pre>
		/// </summary>
		/// <param name="cSearchFor"> </param>
		/// <param name="cSearchIn"> </param>
		private static int AtC(string cSearchFor, string cSearchIn)
		{
			return cSearchIn.ToLower().IndexOf(cSearchFor.ToLower()) + 1;
		}

		/// <summary>
		/// Receives two strings and an occurence position (1st, 2nd etc) as parameters and 
		/// searches for one string within another for that position. This function ignores the
		/// case of both the strings and if found, returns the beginning numeric position 
		/// otherwise returns 0.
		/// <pre>
		/// Example:
		/// VFPToolkit.strings.AtC("d", "Joe Doe", 1);	//returns 5
		/// VFPToolkit.strings.AtC("O", "Joe Doe", 2);	//returns 6
		/// </pre>
		/// </summary>
		/// <param name="cSearchFor"> </param>
		/// <param name="cSearchIn"> </param>
		/// <param name="nOccurence"> </param>
		private static int AtC(string cSearchFor, string cSearchIn, int nOccurence)
		{
			return __at(cSearchFor.ToLower(), cSearchIn.ToLower(), nOccurence, 1);
		}
		#endregion
		
	}
}
