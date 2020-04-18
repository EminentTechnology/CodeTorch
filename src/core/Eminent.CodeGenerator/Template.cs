using System;
using System.IO;
using System.Collections.Specialized;
using System.Text;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for Template.
	/// </summary>
	public class Template
	{
		public Template()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
        public Template(FileInfo fi)
		{

			FileName = fi.Name;
			FileExtension = fi.Extension;
			DirectoryName = fi.DirectoryName;
			DateCreated = fi.CreationTime;
			DateModified = fi.LastWriteTime;
			FullFileName = fi.FullName;

			StreamReader fileSR = null;

			


			try
			{
				fileSR = File.OpenText(fi.FullName);
				EntireContents = fileSR.ReadToEnd();
				
			}
			catch
			{
				
			}
			finally
			{
				if(fileSR != null)
				{
					fileSR.Close();
				}
			}


        
			ParseTemplate(EntireContents);

		}

		public string FileName = "";
		public string BaseClass = "";
		public string FullFileName = "";
		public string FileExtension = "";
		public string Description = "";
		public string DirectoryName = "";
		private string _Language = "";
		public string TargetLanguage = "";
		public string OutputLocation = "";
		public DateTime DateCreated ;
		public DateTime DateModified ;
		public string EntireContents = "";

		public StringCollection Assemblies = new StringCollection();
		public StringCollection Namespaces = new StringCollection();
		public PropertyCollection Properties = new PropertyCollection();
		public StringBuilder CodeSnippet = new StringBuilder(); 

		private string CRLF = "\\r\\n";
		private string STMTTERM = ";";

		private void InitLanguageSettings()
		{
			switch(Language.ToLower().Trim())
			{
				case "vb":
					CRLF = "vbCRLF";
					STMTTERM = "";
					break;
				case "c#":
				default:
					CRLF = "\\r\\n";
					STMTTERM = ";";
					break;
			}
		}

        public string ClassName { get; set; }

		public string Language 
		{
			get 
			{
				return _Language;
			}
			set 
			{
				_Language = value;
				InitLanguageSettings();
			}
		}


		#region ParseTemplate
		/// <summary>
		/// Parses a script into a compilable program
		/// </summary>
		/// <param name="code"></param
		public void ParseTemplate(string code)
		{
			
			if (String.IsNullOrEmpty(code))
				return;
			
						

			int lnLast = 0;
			int lnAt2 = 0;
			int lnAt = code.IndexOf("<%",0);


			CodeSnippet.Append("\tresponseSB.Length = 0" + STMTTERM + "\r\n\r\n");

            if (lnAt > -1)
            {
                while (lnAt > -1)
                {

                    // *** Catch the plain text write out to the Response Stream as is - fix up for quotes
                    if (lnAt > -1)
                    {
                        if (code.Substring(lnLast, lnAt - lnLast).Replace("\"", "\"\"").Trim() != String.Empty)
                        {

                            string snip = code.Substring(lnLast, lnAt - lnLast).Replace("\"", "\\\"").Replace("\r\n", CRLF);
                            //string snip = code.Substring(lnLast,lnAt - lnLast).Replace("\"","\"\"").Replace("\r\n",CRLF);

                            //hack: quick fix..better algo can be used here
                            int maxCopy = 8000;
                            int lengthToCopy = snip.Length > maxCopy ? maxCopy : snip.Length;


                            CodeSnippet.Append("\tresponseSB.Append(\"" + snip.Substring(0, lengthToCopy) + "\" )" + STMTTERM + "\r\n\r\n");
                            snip = snip.Substring(lengthToCopy);

                            while (snip.Length > maxCopy)
                            {

                                CodeSnippet.Append("\tresponseSB.Append(\"" + snip.Substring(0, maxCopy) + "\" )" + STMTTERM + "\r\n\r\n");
                                snip = snip.Substring(maxCopy);
                            }
                            //CodeSnippet.Append("\tresponseSB.Append(\"" + code.Substring(lnLast,lnAt - lnLast).Replace("\"","\"\"").Replace("\r\n",CRLF) + "\" );\r\n\r\n");
                        }
                    }

                    //*** Find end tag
                    lnAt2 = code.IndexOf("%>", lnAt);
                    if (lnAt2 < 0)
                        break;

                    string lcSnippet = code.Substring(lnAt, lnAt2 - lnAt + 2);

                    // *** Write out an expression. 'Eval' inside of a Response.Write call
                    if (lcSnippet.Substring(2, 1) == "=")
                    {

                        CodeSnippet.Append("\tresponseSB.Append(" + lcSnippet.Substring(3, lcSnippet.Length - 5).Trim() + ".ToString())" + STMTTERM + "\r\n");

                    }
                    else if (lcSnippet.Substring(2, 1) == "@")
                    {
                        string lcAttribute = "";

                        // *** Handle Directives for Template/Assembly/Proprty/Import
                        lcAttribute = StrExtract(lcSnippet, "Property", "=");
                        if (lcAttribute.Length > 0)
                        {
                            Property p = new Property();
                            lcAttribute = StrExtract(lcSnippet, "Name=\"", "\"");
                            if (lcAttribute.Length > 0)
                                p.Name = lcAttribute;

                            lcAttribute = StrExtract(lcSnippet, "Type=\"", "\"");
                            if (lcAttribute.Length > 0)
                                p.Type = lcAttribute;

                            lcAttribute = StrExtract(lcSnippet, "Default=\"", "\"");
                            if (lcAttribute.Length > 0)
                                p.Default = lcAttribute;

                            lcAttribute = StrExtract(lcSnippet, "Category=\"", "\"");
                            if (lcAttribute.Length > 0)
                                p.Category = lcAttribute;

                            lcAttribute = StrExtract(lcSnippet, "Description=\"", "\"");
                            if (lcAttribute.Length > 0)
                                p.Description = lcAttribute;

                            Properties.Add(p);
                        }
                        else
                        {
                            lcAttribute = StrExtract(lcSnippet, "Assembly", "=");
                            if (lcAttribute.Length > 0)
                            {
                                lcAttribute = StrExtract(lcSnippet, "\"", "\"");
                                if (lcAttribute.Length > 0)
                                    Assemblies.Add(lcAttribute);
                            }

                            else
                            {
                                lcAttribute = StrExtract(lcSnippet, "Import", "=");
                                if (lcAttribute.Length > 0)
                                {
                                    lcAttribute = StrExtract(lcSnippet, "\"", "\"");
                                    if (lcAttribute.Length > 0)
                                        Namespaces.Add(lcAttribute);
                                }
                                else
                                {
                                    lcAttribute = StrExtract(lcSnippet, "Template", "=");

                                    if (lcAttribute.Length > 0)
                                    {


                                        lcAttribute = StrExtract(lcSnippet, "Language=\"", "\"");
                                        if (lcAttribute.Length > 0)
                                            Language = lcAttribute;

                                        lcAttribute = StrExtract(lcSnippet, "Inherits=\"", "\"");
                                        if (lcAttribute.Length > 0)
                                            BaseClass = lcAttribute;

                                        lcAttribute = StrExtract(lcSnippet, "TargetLanguage=\"", "\"");
                                        if (lcAttribute.Length > 0)
                                            TargetLanguage = lcAttribute;

                                        lcAttribute = StrExtract(lcSnippet, "Description=\"", "\"");
                                        if (lcAttribute.Length > 0)
                                            Description = lcAttribute;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // *** Write out a line of code as is.
                        CodeSnippet.Append("\t" + lcSnippet.Substring(2, lcSnippet.Length - 4) + "\r\n");
                    }

                    lnLast = lnAt2 + 2;
                    lnAt = code.IndexOf("<%", lnLast);
                    if (lnAt < 0)
                    {
                        // *** Write out the final block of non-code text
                        if (code.Substring(lnLast, code.Length - lnLast).Replace("\"", "\"\"").Trim() != String.Empty)
                        {
                            //orig code - does not seem to handle html with apostrophes well (") tryinh @ for verbatim string literal 
                            //CodeSnippet.Append("\tresponseSB.Append(\"" + code.Substring(lnLast, code.Length - lnLast).Replace("\"", "\"\"").Replace("\r\n", CRLF) + "\" )" + STMTTERM + "\r\n");
                            CodeSnippet.Append("\tresponseSB.Append(@\"" + code.Substring(lnLast, code.Length - lnLast).Replace("\"", "\"\"") + "\" )" + STMTTERM + "\r\n");
                        }
                    }
                }
            }
            else
            {
                //orig code - does not seem to handle html with apostrophes well (") tryinh @ for verbatim string literal 
                //CodeSnippet.Append("\tresponseSB.Append(\"" + code.Replace("\"", "\"\"").Replace("\r\n", CRLF) + "\" )" + STMTTERM + "\r\n");
                CodeSnippet.Append("\tresponseSB.Append(@\"" + code.Replace("\"", "\"\"") + "\" )" + STMTTERM + "\r\n");
            }

			CodeSnippet.Append("\treturn responseSB.ToString()");

		}
		#endregion
		

		#region StrExtract
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
		#endregion

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


		

	}
}
