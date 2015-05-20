using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{

	public class SettingService
	{
		static readonly SettingService instance = new SettingService();

		public static SettingService GetInstance()
		{
			return instance;
		}

		private SettingService()
		{
		}

		ISettings provider = null;

		public ISettings Provider
		{
			get
			{
				if (provider == null)
				{
					provider = GetProvider();
				}
				return provider;
			}

		}

		private ISettings GetProvider()
		{
			ISettings retVal = null;
			App app = Configuration.GetInstance().App;

			string assemblyName = app.SettingProviderAssembly;
			string className = app.SettingProviderClass;
			string config = app.SettingProviderConfig;


			if (!String.IsNullOrEmpty(assemblyName) && !String.IsNullOrEmpty(className))
			{

				try
				{
					retVal = Common.CreateInstance(assemblyName, className) as ISettings;
					retVal.Initialize(config);
				}
				catch(Exception ex)
				{
                    Common.LogException(ex);
				}
			}

			if (retVal == null)
			{
				throw new Exception(String.Format("No valid settings provider was found"));
			}

			return retVal;
		}
	}
}
