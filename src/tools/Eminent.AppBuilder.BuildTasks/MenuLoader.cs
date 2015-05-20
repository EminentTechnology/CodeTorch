using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Eminent.AppBuilder.BuildTasks
{
    [TaskName("menuLoader")]
    public  class MenuLoader: Task
    {
        [TaskAttribute("filePath", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string filePath { get; set; }

        [TaskAttribute("connectionString", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string connectionString { get; set; }



        protected override void ExecuteTask()
        {
            LoadMenu();


        }

        public void LoadMenu()
        {
           
            //load file
           // load xml document from file path
            XDocument doc = XDocument.Load(filePath);

            Database db = null;
            if (String.IsNullOrEmpty(connectionString))
            {
                db = DatabaseFactory.CreateDatabase();
            }
            
            using (DbConnection conn = DataLayerBase.GetConnection(db, connectionString))
            {
                conn.Open();

                DbTransaction tran = conn.BeginTransaction();

                try
                {
                    MenuDB menu = new MenuDB();
                    DataTable dtMenu = menu.GetMenus(db, tran);
                    DataView dvMenu = dtMenu.DefaultView;

                    foreach (XElement m in doc.Root.Elements())
                    {
                       
                            int menudbVersion = -1;
                            int menufileVersion = 0;
                            bool isRootMenu = false;
                            string menuName = m.Attribute("name").Value;

                            if (m.Attribute("version") != null)
                            {
                                if (!String.IsNullOrEmpty(m.Attribute("version").Value))
                                {
                                    menufileVersion = Convert.ToInt32(m.Attribute("version").Value);
                                }
                            }

                            if (m.Attribute("isRootMenu") != null)
                            {
                                if (!String.IsNullOrEmpty(m.Attribute("isRootMenu").Value))
                                {
                                    isRootMenu = Convert.ToBoolean(m.Attribute("isRootMenu").Value);
                                }
                            }

                            dvMenu.RowFilter = String.Format("MenuName = '{0}'", menuName);
                            if (dvMenu.Count == 1)
                            {
                                if (dvMenu[0]["version"] != DBNull.Value)
                                    menudbVersion = Convert.ToInt32(dvMenu[0]["version"]);
                            }

                            if (menudbVersion < menufileVersion)
                            {
                                //delete menu
                                menu.DeleteMenu(db, tran, menuName);

                                //insert menu
                                int menuID = 0;
                                menu.InsertMenu(db, tran, out menuID, menuName, menufileVersion, isRootMenu);

                                //loop through menu objects and add menu item
                                int Sequence = 0;
                                foreach (XElement item in m.Elements("Items").Elements())
                                {
                                    //insert menu item
                                    Sequence++;
                                    AddMenuItem(db, tran, menuID, null, ref Sequence, item);
                                }
                            
                        }

                        
                    }


                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void AddMenuItem(Database db, DbTransaction tran, int menuID, int? parentID, ref int Sequence, XElement menuitem)
        {
            int menuItemID = 0;
            //add menu item

            MenuDB menu = new MenuDB();

            string name = null;
            string code = null;
            string permission = null;
            string location = null;
            string context = null;
            string cssClass = null;
            string target = null;

            if (menuitem.Attribute("name") != null)
                name = menuitem.Attribute("name").Value;

            if(menuitem.Attribute("code") != null)
                code = menuitem.Attribute("code").Value;

            if (menuitem.Attribute("permission") != null)
                permission = menuitem.Attribute("permission").Value;

            if (menuitem.Attribute("location") != null)
                location = menuitem.Attribute("location").Value;

            if (menuitem.Attribute("context") != null)
                context = menuitem.Attribute("context").Value;

            if (menuitem.Attribute("cssClass") != null)
                cssClass = menuitem.Attribute("cssClass").Value;

            if (menuitem.Attribute("target") != null)
                target = menuitem.Attribute("target").Value;

            menu.InsertMenuItem(db, tran, out menuItemID, parentID, menuID, 
                Sequence,
                name,
                code,
                permission,
                location,
                context,
                cssClass,
                target

                );



            //if it has children loop through and add menu items
            if (menuitem.HasElements)
            {
                foreach (XElement item in menuitem.Elements("Items").Elements())
                {
                    Sequence++;
                    //insert menu item
                    AddMenuItem(db, tran, menuID, menuItemID, ref Sequence, item);
                }
            }
        }
    }
}
