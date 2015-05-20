using System;
using System.Reflection;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Interface that can be run over the remote AppDomain boundary.
	/// </summary>
	public interface IRemoteInterface
	{
		object Invoke(string lcMethod,object[] Parameters);
	}
}
