using System;
using CodeTorch.Core.Interfaces;
using MonoTouch.Foundation;
using System.Collections.Generic;
using CodeTorch.Mobile.Settings.iOS;

[assembly: Xamarin.Forms.Dependency (typeof (iOSSettings))]
namespace CodeTorch.Mobile.Settings.iOS
{
	public class iOSSettings: ISettings
	{
		public void Initialize(string settings)
		{

		}

		public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");

			var type = typeof(T);
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}

			object returnVal;
			var defaults = NSUserDefaults.StandardUserDefaults;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				returnVal = defaults.BoolForKey(key);
				break;
			case TypeCode.Int64:
				var savedval = defaults.StringForKey(key);
				returnVal = Convert.ToInt64(savedval);
				break;
			case TypeCode.Double:
				returnVal = defaults.DoubleForKey(key);
				break;
			case TypeCode.Int32:
				returnVal = defaults.IntForKey(key);
				break;
			case TypeCode.Single:
				returnVal = defaults.FloatForKey(key);
				break;
			case TypeCode.String:
				returnVal = defaults.StringForKey(key);
				break;
			default:
				returnVal = defaultValue;
				break;
			}

			if (Equals(default(T), returnVal))
			{
				returnVal = defaultValue;
			}

			return (T)returnVal;
		}

		public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");

			var type = value.GetType();
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			var defaults = NSUserDefaults.StandardUserDefaults;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				defaults.SetBool(Convert.ToBoolean(value), key);
				break;
			case TypeCode.Int64:
				defaults.SetString(Convert.ToString(value), key);
				break;
			case TypeCode.Double:
				defaults.SetDouble(Convert.ToDouble(value), key);
				break;
			case TypeCode.Int32:
				defaults.SetInt(Convert.ToInt32(value), key);
				break;
			case TypeCode.Single:
				defaults.SetFloat(Convert.ToSingle(value), key);
				break;
			case TypeCode.String:
				defaults.SetString(Convert.ToString(value), key);
				break;
			default:
				throw new ArgumentException(string.Format("Type {0} is not supported", type), "value");
			}
			return defaults.Synchronize();
		}

		public bool Contains(string key, bool roaming = false)
		{
			var defaults = NSUserDefaults.StandardUserDefaults;
			try
			{
				var stuff = defaults[key];
				return stuff != null;
			}
			catch
			{
				return false;
			}
		}

		public bool DeleteValue(string key, bool roaming = false)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Key must have a value", "key");

			var defaults = NSUserDefaults.StandardUserDefaults;
			defaults.RemoveObject(key);
			return defaults.Synchronize();
		}

		public bool ClearAllValues(bool roaming = false)
		{
			var defaults = NSUserDefaults.StandardUserDefaults;
			defaults.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
			return defaults.Synchronize();
		}
	}
}

