using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeTorch.Mobile.Settings
{
    public class SettingsProvider: ISettings
    {
        ISettings service = null;
        public void Initialize(string settings)
        {

            service = DependencyService.Get<ISettings>();
        }

       

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            return service.GetValue<T>(key, defaultValue, roaming);
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            return service.AddOrUpdateValue<T>(key, value, roaming);
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            return service.DeleteValue(key, roaming);
        }

        public bool Contains(string key, bool roaming = false)
        {
            return service.Contains(key, roaming);
        }

        public bool ClearAllValues(bool roaming = false)
        {
            return service.ClearAllValues(roaming);
        }
    }
}
