using System;
using System.Text;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for ScriptEngineResponse.
	/// </summary>
	public class ScriptEngineResponse
	{
		private StringBuilder oSb;

		public ScriptEngineResponse()
		{
			this.oSb = new StringBuilder();
		}

		
		public void Write(string lcString) 
		{
			this.oSb.Append(lcString);
		}
		public void Append(string lcString)
		{
			this.oSb.Append(lcString);
		}
		public override string ToString() 
		{
			return this.oSb.ToString();
		}
	}
}


