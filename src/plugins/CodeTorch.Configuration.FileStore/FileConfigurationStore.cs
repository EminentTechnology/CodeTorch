using CodeTorch.Abstractions;
using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
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

        //public string Path { get; set; }
        string _Path = null;
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
                //TODO: Really need to remove this dependency
                //Also need to figure out a way to remove it from ConfigurationObjects in Core
                CodeTorch.Core.Configuration.GetInstance().ConfigurationPath = _Path;
            }
        }

        public FileConfigurationStore(ILogManager log)
        {
            Log = log.GetLogger(this.GetType());
            //set default codetorch path
            try
            {
                string dataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                Path = System.IO.Path.Combine(dataPath, "CodeTorch\\Web\\");
            }
            catch
            { }
        }

        public async Task<T> Add<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            //ensure folder exsists
            var folder = GetPath<T>(null);
            var t = typeof(T);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            //TODO - need a generic way of getting the manager
            var manager = ConfigurationObjectFactory.CreateConfigurationObject(t.Name) as IConfigurationObject2;

            System.Xml.Serialization.XmlSerializer x = null;
            Type[] extraTypes = null;
            //TODO: neeed way to set this for special types - eg screen

            if (extraTypes == null)
            {
                x = new System.Xml.Serialization.XmlSerializer(item.GetType());
            }
            else
            {
                x = new System.Xml.Serialization.XmlSerializer(item.GetType(), extraTypes);
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;

            var filePath = GetPath<T>(key);
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                x.Serialize(writer, item);
                writer.Close();
            }


            return await Task.FromResult(item);
        }

        public async Task Delete<T>(string key)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);

            
            var item = await GetItem<T>(key);

            var t = item.GetType();

            //TODO - need a generic way of getting the manager
            var manager = ConfigurationObjectFactory.CreateConfigurationObject(t.Name) as IConfigurationObject2;
            manager.Delete(item);

            await Task.FromResult(true);
        }

        public async Task<bool> Exists<T>(string key)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            T item = default(T);

            if (!String.IsNullOrEmpty(key))
            {
                key = ValidateKey(key);
                item = await GetItem<T>(key);
            }
           
            return (item != null);
        }

        public async Task<T> GetItem<T>(string key)
        {
            T item = default(T);

            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            if (!String.IsNullOrEmpty(key))
            {
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

        public async Task<T> Save<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            var exists = await Exists<T>(key);

            if (exists)
            {
                return await Add<T>(key, item);
            }
            else
            {
                return await Update<T>(key, item);
            }
        }

        public async Task<T> Update<T>(string key, T item)
        {
            if (String.IsNullOrEmpty(Path))
                throw new ArgumentNullException(nameof(Path));

            key = ValidateKey(key);
            var t = typeof(T);
            //ensure folder exsists
            var folder = GetPath<T>(null);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            //TODO - need a generic way of getting the manager
            var manager = ConfigurationObjectFactory.CreateConfigurationObject(t.Name) as IConfigurationObject2;
            manager.Save(item);

            return await Task.FromResult(item);
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
