using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using CodeTorch.Core;
using System.IO;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class RestService
    {
        [ReadOnly(true)]
        public string Name { get; set; }

        public string EntityName { get; set; }
        public string EntityCollectionName { get; set; }

        public string Resource { get; set; }

        List<BaseRestServiceMethod> methods = new List<BaseRestServiceMethod>();

        [XmlArray("Methods")]
        [Description("List all rest methods")]
        [XmlArrayItem(ElementName = "Get", Type = typeof(GetRestServiceMethod))]
        [XmlArrayItem(ElementName = "Post", Type = typeof(PostRestServiceMethod))]
        [XmlArrayItem(ElementName = "Put", Type = typeof(PutRestServiceMethod))]
        [XmlArrayItem(ElementName = "Delete", Type = typeof(DeleteRestServiceMethod))]
        [Editor("CodeTorch.Core.Design.RestServiceMethodCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
        public virtual List<BaseRestServiceMethod> Methods
        {
            get
            {
                return methods;
            }
            set
            {
                methods = value;
            }

        }

        

        public static RestService Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RestService));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            RestService item = null;

            try
            {
                item = (RestService)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing RestService - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(RestService item)
        {
            string configPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}RestServices", configPath)))
            {
                Directory.CreateDirectory(String.Format("{0}RestServices", configPath));
            }

            string filePath = String.Format("{0}RestServices\\{1}.xml", configPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().RestServices.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().RestServices
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static RestService GetByName(string Name)
        {
            RestService item = Configuration.GetInstance().RestServices
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }

        //public static RestService GetByResourceCollectionUrl(string Entity)
        //{
        //    RestService item = Configuration.GetInstance().RestServices
        //                    .Where(i =>
        //                        (
        //                            (i.Resource.ToLower() == Entity.ToLower())
        //                        )
        //                    )
        //                    .SingleOrDefault();

        //    return item;
        //}

        //public static RestService GetByResourceEntityUrl(string Entity)
        //{
        //    RestService item = Configuration.GetInstance().RestServices
        //                    .Where(i =>
        //                        (
        //                            (i.Resource.ToLower().StartsWith(Entity.ToLower())) &&
        //                            (i.Resource.Split('/').Length == 2)
        //                        )
        //                    )
        //                    .SingleOrDefault();

        //    return item;
        //}

        //public static RestService GetByResourceEntitySubCollectionUrl(string Entity, string Collection)
        //{
        //    RestService item = Configuration.GetInstance().RestServices
        //                    .Where(i =>
        //                        (
        //                            (i.Resource.ToLower().StartsWith(Entity.ToLower())) &&
        //                            (i.Resource.Split('/').Length == 3) &&
        //                            (i.Resource.ToLower().EndsWith(Collection.ToLower())) 
        //                        )
        //                    )
        //                    .SingleOrDefault();

        //    return item;
        //}
    }
}
