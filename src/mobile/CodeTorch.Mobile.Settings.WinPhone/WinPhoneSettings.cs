using System.Collections.Generic;
using CodeTorch.Core.Interfaces;
using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CodeTorch.Mobile.Settings.WinPhone;

[assembly: Xamarin.Forms.Dependency (typeof (WinPhoneSettings))]
namespace CodeTorch.Mobile.Settings.WinPhone
{
    public class WinPhoneSettings: ISettings
    {

        private static IsolatedStorageSettings IsolatedStorageSettings
        {
            get { return IsolatedStorageSettings.ApplicationSettings; }
        }

        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
            
        }

        public void Initialize(string settings)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (IsolatedStorageSettings.Contains(key))
                return (T)IsolatedStorageSettings[key];

            //Mvx.Trace(MvxTraceLevel.Warning, "Key: {0} was not in IsolatedStorageSettings", key);
            return defaultValue;
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (IsolatedStorageSettings.Contains(key))
            {
                if (IsolatedStorageSettings[key].Equals(value)) return false;
                IsolatedStorageSettings[key] = value;
                IsolatedStorageSettings.Save();
                return true;
            }

            IsolatedStorageSettings.Add(key, value);
            IsolatedStorageSettings.Save();
            return true;
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            if (!IsolatedStorageSettings.Contains(key)) return false;

            IsolatedStorageSettings.Remove(key);
            IsolatedStorageSettings.Save();
            return true;
        }

        public bool Contains(string key, bool roaming = false)
        {
            return IsolatedStorageSettings.Contains(key);
        }

        public bool ClearAllValues(bool roaming = false)
        {
            IsolatedStorageSettings.Clear();
            IsolatedStorageSettings.Save();
            return true;
        }
    }
}
