using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Abstractions
{
    public interface IConfigurationStore
    {
        Task<List<T>> GetItems<T>();
        Task<T> GetItem<T>(string key);
        Task<bool> Exists<T>(string key);
        Task<T> Add<T>(string key, T item);
        Task<T> Update<T>(string key, T item);
        Task Delete<T>(string key);
        Task<T> Save<T>(string key, T item);
    }
}
