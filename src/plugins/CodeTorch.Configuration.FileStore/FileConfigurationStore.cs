using CodeTorch.Abstractions;
using CodeTorch.Abstractions.Services;
using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Configuration.FileStore
{
    public class FileConfigurationStore : IConfigurationStore
    {
        private readonly ILog Log;
        public string Path { get; set; }

        public FileConfigurationStore(ILog log)
        {
            Log = log;
            //set default codetorch path
            try
            {
                string dataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                Path = System.IO.Path.Combine(dataPath, "CodeTorch\\Web\\");
            }
            catch
            { }
        }

        public Task<T> Add<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            throw new NotImplementedException();
        }

        public Task<T> Delete<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            throw new NotImplementedException();
        }

        public Task<bool> Exists<T>(string key)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            throw new NotImplementedException();
        }

        public async Task<T> GetItem<T>(string key)
        {
            T item = default(T);

            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);
            var file = GetPath<T>(key);

            var t = typeof(T);

            try
            {
                //TODO - need a generic way of getting the manager
                var manager = ConfigurationObjectFactory.CreateConfigurationObject(t.Name) as IConfigurationManager<T>;
                XDocument doc = XDocument.Load(file);
                item = manager.Load(doc, file);

            }
            catch (Exception ex)
            {
                Log.Debug(String.Format("Error during deserialization of {0} - {1}", t.Name, key), ex);
            }

            return await Task.FromResult(item);
        }

        

        public async Task<List<T>> GetItems<T>()
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            List<T> retVal = new List<T>();
            var t = typeof(T);

            var folder = GetPath<T>();

            List<T> list = GetItems<T>( t, folder);
            retVal.AddRange(list);

            return await Task.FromResult(retVal);
        }

        private List<T> GetItems<T>( Type t, string path)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            List<T> retVal = new List<T>();

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.xml");

                string[] childfolders = Directory.GetDirectories(path);

                

                foreach (string dir in childfolders)
                {
                    List<T> list = GetItems<T>(t, dir);
                    retVal.AddRange(list);
                }

                foreach (string file in files)
                {

                    try
                    {
                        
                        //TODO - need a generic way of getting the manager
                        var manager = ConfigurationObjectFactory.CreateConfigurationObject(t.Name) as IConfigurationManager<T>;
                        XDocument doc = XDocument.Load(file);
                        T item = manager.Load(doc, file);
                        
                        retVal.Add(item);

                    }
                    catch(Exception ex)
                    {
                        Log.Debug(String.Format("Error during deserialization of {0} - {1}", t.Name, file), ex);
                    }
                    
                }

            }

            return retVal;
        }

        public Task<T> Save<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            throw new NotImplementedException();
        }

        public Task<T> Update<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            throw new NotImplementedException();
        }


        private string GetPath<T>(string key = null)
        {
            var t = typeof(T);
            string typeName = t.Name;

            if (t.Name.ToLower().EndsWith("y"))
            {
                typeName = t.Name.Substring(0, t.Name.Length - 1) + "ies";
            }
            else
            {
                if (!t.Name.ToLower().EndsWith("s"))
                {
                    typeName += "s";
                }
            }
            
            if (String.IsNullOrEmpty(key))
            {
                return System.IO.Path.Combine(Path, typeName);
            }
            else
            {
                return System.IO.Path.Combine(Path, typeName, key);
            }


        }

        private string ValidateKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!key.ToLower().EndsWith(".xml"))
                key += ".xml";

            return key;
        }

        //private  void LoadDirectoryItems(string FolderPath, string entityName, bool ClearCollection)
        //{

        //    string[] files = Directory.GetFiles(FolderPath, "*.xml");

        //    string[] childfolders = Directory.GetDirectories(FolderPath);

        //    if (ClearCollection)
        //    {
        //        IConfigurationObject config = ConfigurationObjectFactory.CreateConfigurationObject(entityName);
        //        config.ClearAll();
        //    }

        //    foreach (string dir in childfolders)
        //    {
        //        LoadDirectoryItems(dir, entityName, false);
        //    }

        //    foreach (string file in files)
        //    {
        //        LoadFile(entityName, file);
        //    }

        //}
    }
}
