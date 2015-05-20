using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core.Interfaces;
using Android.Content;
using Android.App;
using Android.Preferences;
using CodeTorch.Mobile.Settings.Android;

[assembly: Xamarin.Forms.Dependency (typeof (AndroidSettings))]
namespace CodeTorch.Mobile.Settings.Android
{
    public class AndroidSettings: ISettings
    {
        public AndroidSettings()
        { 
        
        }

        public AndroidSettings(string settingsesFileName) 
        { 
            _settingsFileName = settingsesFileName; 
        }

        public void Initialize(string settings)
        {
           
        }

        private static string _settingsFileName;

        private static ISharedPreferences _sharedPreferences;
        private static ISharedPreferences SharedPreferences
        {
            get
            {
                if(_sharedPreferences != null) return _sharedPreferences;

                var context = Application.Context;

                //If file name is empty use defaults
                if(string.IsNullOrEmpty(_settingsFileName))
                {
                    _sharedPreferences =
                        PreferenceManager.GetDefaultSharedPreferences(context);
                }
                else
                {
                    _sharedPreferences =
                        context.ApplicationContext.GetSharedPreferences(_settingsFileName,
                            FileCreationMode.Append);
                }

                return _sharedPreferences;
            }
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
            switch(Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    returnVal = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
                    break;
                case TypeCode.Int64:
                    returnVal = SharedPreferences.GetLong(key, Convert.ToInt64(defaultValue));
                    break;
                case TypeCode.Int32:
                    returnVal = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue));
                    break;
                case TypeCode.Single:
                    returnVal = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue));
                    break;
                case TypeCode.String:
                    returnVal = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
                    break;
                case TypeCode.DateTime:
                    var ticks = SharedPreferences.GetLong(key, -1);
                    if (ticks == -1)
                        returnVal = defaultValue;
                    else
                        returnVal = new DateTime(ticks);
                    break;
                default:
                    returnVal = defaultValue;
                    break;
            }
            return (T)returnVal;
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var editor = SharedPreferences.Edit();
            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            switch(Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    editor.PutBoolean(key, Convert.ToBoolean(value));
                    break;
                case TypeCode.Int64:
                    editor.PutLong(key, Convert.ToInt64(value));
                    break;
                case TypeCode.Int32:
                    editor.PutInt(key, Convert.ToInt32(value));
                    break;
                case TypeCode.Single:
                    editor.PutFloat(key, Convert.ToSingle(value));
                    break;
                case TypeCode.String:
                    editor.PutString(key, Convert.ToString(value));
                    break;
                case TypeCode.DateTime:
                    editor.PutLong(key, ((DateTime)(object)value).Ticks);
                    break;
                default:
                    throw new ArgumentException(string.Format("Type {0} is not supported", type), "value");
            }
            return editor.Commit();
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var editor = SharedPreferences.Edit();
            editor.Remove(key);
            return editor.Commit();
        }

        public bool Contains(string key, bool roaming = false)
        {
            return SharedPreferences.Contains(key);
        }

        public bool ClearAllValues(bool roaming = false)
        {
            var editor = SharedPreferences.Edit();
            editor.Clear();
            return editor.Commit();
        }
    }
}
