using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections;
using System.Data;
using System.Reflection;
using CodeTorch.Core.Attributes;

namespace CodeTorch.Core
{
    public class Template
    {
        List<TemplateDataItem> _DataItems = new List<TemplateDataItem>();

        [ReadOnly(true)]
        public string Name { get; set; }

        
        [CustomEditor("MarkupEditor")]
        public string Content { get; set; }

        [XmlArray("DataItems")]
        [XmlArrayItem("DataItem")]
        [Description("List of data item placeholders used in template - correspond typically to a datacommand placeholder")]
        public List<TemplateDataItem> DataItems
        {
            get { return _DataItems; }
            set { _DataItems = value; }
        }



        public static Template Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Template));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Template item = null;

            try
            {
                item = (Template)serializer.Deserialize(reader);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Template", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(Template item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}Templates", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Templates", ConfigPath));
            }

            string filePath = String.Format("{0}Templates\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Templates.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Templates
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static Template GetByName(string Name)
        {
            Template item = Configuration.GetInstance().Templates
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }


        public static string RenderContent(Template template, Hashtable templateItems)
        {
            return RenderContent(template, template.Content, templateItems);
        }

        public static string RenderContent(Template template, string Content, Hashtable templateItems)
        {
            string retVal = String.Empty;
            //List<ContentDataItem> 

            Eminent.CodeGenerator.Template t = new Eminent.CodeGenerator.Template();

            t.ClassName = "ContentTemplate";
            t.Language = "C#";

            t.Assemblies.Add("System.Data");

            t.Namespaces.Add("System.Collections.Generic");
            t.Namespaces.Add("System.Data");
            t.Namespaces.Add("System.Linq");

            t.ParseTemplate(Content);



            foreach (TemplateDataItem item in template.DataItems)
            {
                Eminent.CodeGenerator.Property p = new Eminent.CodeGenerator.Property();
                p.Name = item.Name;
                p.Type = (item.Type == TemplateDataItemType.DataRow) ? "System.Data.DataRow" : "System.Data.DataRowCollection";

                t.Properties.Add(p);
            }

            Eminent.CodeGenerator.TemplateEngine engine = new Eminent.CodeGenerator.TemplateEngine();


            object templateObject = engine.Compile(t);

            if (engine.Errors.Count == 0)
            {
                //populate properties 
                foreach (TemplateDataItem item in template.DataItems)
                {
                    if ((templateItems.ContainsKey(item.Name)) && (templateItems[item.Name] != null))
                    {

                        if (templateItems[item.Name] is DataTable)
                        {
                            DataTable data = (DataTable)templateItems[item.Name];

                            if (item.Type == TemplateDataItemType.DataRow)
                            {

                                if (data.Rows.Count > 0)
                                {

                                    PropertyInfo itemProperty = templateObject.GetType().GetProperty(item.Name);
                                    itemProperty.SetValue(templateObject, data.Rows[0], null);

                                }
                            }
                            else
                            {
                                if (data.Rows.Count > 0)
                                {

                                    PropertyInfo itemProperty = templateObject.GetType().GetProperty(item.Name);
                                    itemProperty.SetValue(templateObject, data.Rows, null);

                                }
                            }
                        }
                    }
                }


                retVal = Eminent.CodeGenerator.TemplateEngine.GenerateCode(templateObject);

            }
            else
            {
                retVal = engine.GenerateCode(t);
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in engine.Errors)
                {
                    sb.AppendFormat("\r\n({0},{1}): {2} {3}: {4}", error.Line, error.Column, ((error.IsWarning) ? "warning" : "error"), error.ErrorNumber, error.ErrorText);
                }

                throw new ApplicationException(sb.ToString());
            }

            return retVal;
        }

    }
}
