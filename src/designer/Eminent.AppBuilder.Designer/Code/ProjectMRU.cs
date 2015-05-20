using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CodeTorch.Designer.Code
{
    [Serializable]
    public class ProjectMRU
    {

        RecentSet<ProjectMRUItem> _items = new RecentSet<ProjectMRUItem>(25);

        public RecentSet<ProjectMRUItem> Items
        {
            get 
            {
                return _items;
            }
            set 
            {
                _items = value;
            }
        }
    
        public static void Save(ProjectMRU mru)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, mru);
                    ms.Position = 0;
                    byte[] buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    Properties.Settings.Default.ProjectMRU = Convert.ToBase64String(buffer);
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static ProjectMRU Get()
        {
            ProjectMRU retVal = null;

            if (!String.IsNullOrEmpty(Properties.Settings.Default.ProjectMRU))
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.ProjectMRU)))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    retVal = (ProjectMRU)bf.Deserialize(ms);
                }
            }

            return retVal;
        }
    }

    


   
}
