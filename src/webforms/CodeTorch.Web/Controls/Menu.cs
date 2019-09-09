using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using System.Web.Security;
using Telerik.Web.UI;
using CodeTorch.Core;
using CodeTorch.Web.Templates;
using System.Web.UI.HtmlControls;

namespace CodeTorch.Web.Controls
{
    public class Menu: Control, INamingContainer
    {
        HtmlGenericControl control;
        App app;


        protected override void CreateChildControls()
        {
            control = new HtmlGenericControl("ul");
            Controls.Add(control);
        }


        public string MenuName {get;set;}
        public string MenuType { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EnsureChildControls();
            app = CodeTorch.Core.Configuration.GetInstance().App;

            CodeTorch.Core.Menu menuobject = null;

            string menuToSelect = null;

            if (!String.IsNullOrEmpty(this.MenuType))
            {
                switch (this.MenuType.ToLower())
                {
                    case "secondary":
                        if (this.Page is BasePage)
                        {
                            menuToSelect = ((BasePage)this.Page).Screen.Menu.Name;
                        }
                        break;
                    default:
                        //handles primary - currently for root menu
                        menuToSelect = String.IsNullOrEmpty(MenuName) ? app.DefaultMenu : MenuName;
                        break;
                }

               
            }

            if (!String.IsNullOrEmpty(menuToSelect))
            {
                menuobject = CodeTorch.Core.Menu.GetMenu(menuToSelect);
            }

            if (menuobject != null)
            {
                bool renderMenu = false;



                if (
                        ((menuobject.RequiresAuthentication) && (this.Page.User.Identity.IsAuthenticated)) ||
                        (!menuobject.RequiresAuthentication)
                    )
                {
                    
                    renderMenu = true;
                }


                if ((this.Page.Request.QueryString["DisplayMenu"] != null) && (this.Page.Request.QueryString["DisplayMenu"] == "0"))
                {
                    renderMenu = false;
                }

                if (renderMenu)
                {
                    PopulateTopLevelNodes(menuobject);
                
                }
            }

            //if(control != null)
            //    this.Widgets.Add(control);

            
        }



        private void PopulateTopLevelNodes(CodeTorch.Core.Menu menuobject)
        {

            //control = new HtmlGenericControl("ul");
            

            CodeTorch.Core.Menu menu = PopulateDynamicMenuItems(menuobject);
            
            if (menu != null)
            {
                if (!String.IsNullOrEmpty(menu.CssClass))
                {
                    control.Attributes.Add("class",menu.CssClass);
                }

                foreach (CodeTorch.Core.MenuItem item in menu.Items)
                {
                    bool displayItem = true;
                    string resourceSet = String.Format("Menu.{0}", menu.Name);
                    string resourceKeyPrefix = String.Format("{0}", item.Code);

                    if (item.Permission != null)
                    {
                        if ((item.Permission.CheckPermission) && (!String.IsNullOrEmpty(item.Permission.Name)))
                        {
                            displayItem = Common.HasPermission(item.Permission.Name);
                        }
                    }

                    if (displayItem)
                    {
                        HtmlGenericControl menuItem = buildMenuItem(resourceSet, resourceKeyPrefix, item);

                        PopulateChildrenMenuItems(menuItem, item, resourceSet, resourceKeyPrefix);
                        this.control.Controls.Add(menuItem);
                    }
                }
            }

        }

        private bool IsMenuItemSelected(string url)
        {
            bool retVal = false;

            string urlPath = url;

           

            //if (urlPath.IndexOf('?') >= 0)
            //{
            //    urlPath = urlPath.Substring(0, urlPath.IndexOf('?'));
            //}

            if (!String.IsNullOrEmpty(urlPath))
            {
                if (urlPath.StartsWith("~"))
                {
                    urlPath = urlPath.Substring(1);
                }

                if (this.Page.Request.Url.PathAndQuery.ToLower().Contains(urlPath.ToLower()))
                {
                    retVal = true;
                }
                
            }
            

            return retVal;
        }
        

        public void PopulateChildrenMenuItems(HtmlGenericControl parentMenuItem, CodeTorch.Core.MenuItem parentItem, string ResourceSet, string resourceKeyPrefix)
        {
            if (parentItem.Items.Count > 0)
            {
                HtmlGenericControl list = new HtmlGenericControl("ul");

                parentMenuItem.Controls.Add(list);

                foreach (CodeTorch.Core.MenuItem item in parentItem.Items)
                {
                    bool displayItem = true;

                    if (item.Permission != null)
                    {
                        if ((item.Permission.CheckPermission) && (!String.IsNullOrEmpty(item.Permission.Name)))
                        {
                            displayItem = Common.HasPermission(item.Permission.Name);
                        }
                    }

                    if (displayItem)
                    {
                        if (!item.UseCommand)
                        {
                            HtmlGenericControl menuItem = buildMenuItem(ResourceSet, resourceKeyPrefix, item);
                            
                            string newResourceKeyPrefix = resourceKeyPrefix + "." + item.Code;
                            PopulateChildrenMenuItems(menuItem, item, ResourceSet, newResourceKeyPrefix);
                            list.Controls.Add(menuItem);
                        }
                        else
                        {
                            //SHOULD REALLY NEVER GET HERE - as DATA DRIVEN HAS ALREADY RUN FROM TOP LEVEL
                            //CLEAN UP LATER AFTER VERIFIED - OR KEEP IF WE WANT TO SUPPORT MULTI REQUEST DB ITEMS
                            //DataCommandDB dataCommandDB = new DataCommandDB();
                            //PageDB pageDB = new PageDB();
                            //List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
                            //DataCommand cmd = DataCommand.GetDataCommand(item.CommandName);

                            //foreach (DataCommandParameter cmdParam in cmd.Parameters)
                            //{
                            //    ScreenDataCommandParameter p = new ScreenDataCommandParameter();
                            //    //TODO: need to support context with multiple items - presently assumes one item
                            //    if (cmdParam.Name.ToLower().EndsWith(item.Context.ToLower()))
                            //    {
                            //        p.Name = cmdParam.Name;
                            //        p.InputType = ScreenInputType.QueryString;
                            //        p.InputKey = item.Context;
                            //        p.Value = this.Page.Request.QueryString[item.Context];
                            //    }

                            //    if (cmdParam.Name.ToLower().EndsWith("username"))
                            //    {
                            //        p.Name = cmdParam.Name;
                            //        p.InputType = ScreenInputType.Special;
                            //        p.InputKey = "UserName";
                            //        p.Value = Common.UserName;
                            //    }
                            //    parameters.Add(p);
                            //}


                            //DataTable dt = dataCommandDB.GetDataForDataCommand(item.CommandName, parameters);

                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    HtmlGenericControl newMenuItem = new HtmlGenericControl("li");
                            //    HyperLink newLink = new HyperLink();
                            //    string Url = row["Location"].ToString();
                            //    string Context = row["Context"].ToString();

                            //    if (Url != String.Empty)
                            //    {

                            //        newLink.NavigateUrl = Common.CreateUrlWithQueryStringContext(Url, Context);
                            //    }

                            //    //newLink.Text = MenuFunctions.GetMenuItemText(app, this.Page, ResourceSet, resourceKeyPrefix, row["MenuItemName"].ToString());
                            //    newMenuItem.Widgets.Add(newLink);


                            //    list.Widgets.Add(newMenuItem);

                                
                            //}
                        }
                    }
                }
            }
            
        }

        private HtmlGenericControl buildMenuItem(string ResourceSet, string resourceKeyPrefix, CodeTorch.Core.MenuItem item)
        {
            HtmlGenericControl menuItem = new HtmlGenericControl("li");
            HyperLink link = new HyperLink();
            string url = item.Location;
            string context = item.Context;

            if (url != String.Empty)
            {

                link.NavigateUrl = Common.CreateUrlWithQueryStringContext(url, context);
            }

            bool isSelected = IsMenuItemSelected(url);
            if (!String.IsNullOrEmpty(item.CssClass))
            {
                if (isSelected)
                {
                    menuItem.Attributes.Add("class", String.Format("{0} selected", item.CssClass));
                }
                else
                {
                    menuItem.Attributes.Add("class", item.CssClass);
                }
            }
            else
            {
                if (isSelected)
                {
                    menuItem.Attributes.Add("class", "selected");
                }
            }

            string linkText = MenuFunctions.GetMenuItemText(app, this.Page, ResourceSet, resourceKeyPrefix, item.Code, item.Name);
            string linkFormat = item.NameFormatString;

            if (String.IsNullOrEmpty(linkFormat))
                linkFormat = "{0}";

            link.Text = String.Format(linkFormat, linkText);


            if (!String.IsNullOrEmpty(item.LinkCssClass))
            {
                link.CssClass = item.LinkCssClass;
                if (isSelected)
                {
                    link.CssClass += " active";
                }
            }
            else
            {
                if (isSelected)
                {
                    link.CssClass = "active";
                }
            }

            menuItem.Controls.Add(link);
            return menuItem;
        }

        private CodeTorch.Core.Menu PopulateDynamicMenuItems(CodeTorch.Core.Menu menuObject)
        {
            CodeTorch.Core.Menu retVal = null;

            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();

            var items = from item in menuObject.Items
                        where item.UseCommand == true
                        select item;

            if (items.Count() > 0)
            {
                retVal = new Core.Menu();
                retVal.Name = menuObject.Name;
                retVal.CssClass = menuObject.CssClass;



                foreach (Core.MenuItem item in menuObject.Items)
                {

                    if (item.UseCommand == false)
                    {
                        Core.MenuItem copiedItem = ObjectCopier.Clone<Core.MenuItem>(item);
                        retVal.Items.Add(copiedItem);
                    }
                    else
                    {

                        DataCommand cmd = DataCommand.GetDataCommand(item.CommandName);

                        foreach (DataCommandParameter cmdParam in cmd.Parameters)
                        {
                            ScreenDataCommandParameter p = new ScreenDataCommandParameter();
                            //TODO: need to support context with multiple items - presently assumes one item
                            if (!String.IsNullOrEmpty(item.Context))
                            {
                                if (cmdParam.Name.ToLower().EndsWith(item.Context.ToLower()))
                                {
                                    p.Name = cmdParam.Name;
                                    p.InputType = ScreenInputType.QueryString;
                                    p.InputKey = item.Context;
                                    p.Value = this.Page.Request.QueryString[item.Context];
                                }
                            }

                            if (cmdParam.Name.ToLower().EndsWith("username"))
                            {
                                p.Name = cmdParam.Name;
                                p.InputType = ScreenInputType.Special;
                                p.InputKey = "UserName";
                                p.Value = Common.UserName;
                            }

                            if (cmdParam.Name.ToLower().EndsWith("hostheader"))
                            {
                                p.Name = cmdParam.Name;
                                p.InputType = ScreenInputType.Special;
                                p.InputKey = "HostHeader";
                                p.Value = Common.HostHeader;
                            }

                            parameters.Add(p);
                        }


                        DataTable dt = dataCommandDB.GetDataForDataCommand(item.CommandName, parameters);
                        DataRow[] rows = null;

                        bool containsParentID = dt.Columns.Contains("ParentID");

                        if (containsParentID)
                        {
                            rows = dt.Select("ParentID IS NULL");
                        }
                        else 
                        {
                            rows = dt.Select();
                        }

                        AddMenuItems(retVal, dt, item, rows, true);

                        

                    }
                }
            }
            else
            {
                retVal = menuObject;
            }

            return retVal;
        }

        

        private void AddMenuItems(CodeTorch.Core.Menu menu, DataTable dt, CodeTorch.Core.MenuItem menuItem, DataRow[] rows, bool addToMenu)
        {
            bool containsContext = dt.Columns.Contains("Context");
            bool containsLocation = dt.Columns.Contains("Location");
            bool containsMenuItemName = dt.Columns.Contains("MenuItemName");
            bool containsCheckPermission = dt.Columns.Contains("CheckPermission");
            bool containsPermissionName = dt.Columns.Contains("PermissionName");
            bool containsCssClass = dt.Columns.Contains("CssClass");
            bool containsTarget = dt.Columns.Contains("Target");
            bool containsCode = dt.Columns.Contains("Code");

            foreach (DataRow row in rows)
            {
                CodeTorch.Core.MenuItem newItem = new CodeTorch.Core.MenuItem();

                if (containsCode)
                {
                    newItem.Code = row["Code"].ToString();
                }

                if (containsContext)
                {
                    newItem.Context = row["Context"].ToString();
                }

                if (containsLocation)
                {
                    newItem.Location = row["Location"].ToString();
                }

                if (containsMenuItemName)
                {
                    newItem.Name = row["MenuItemName"].ToString();
                }

                if (containsCheckPermission)
                {
                    newItem.Permission.CheckPermission = Convert.ToBoolean(row["CheckPermission"]);
                }

                if (containsPermissionName)
                {
                    newItem.Permission.Name = row["PermissionName"].ToString();
                }

                if (containsCssClass)
                {
                    newItem.CssClass = row["CssClass"].ToString();
                }

                if (containsTarget)
                {
                    if (row["Target"] != DBNull.Value)
                    {
                        newItem.Target = row["Target"].ToString();
                    }
                }

                newItem.UseCommand = false;

                PopulateChildItems(menu, dt, row, newItem, false);

                if (addToMenu)
                {
                    menu.Items.Add(newItem);
                }
                else
                {
                    menuItem.Items.Add(newItem);
                }

            }
        }

        private void PopulateChildItems(Core.Menu menu, DataTable dt, DataRow currentRow, Core.MenuItem menuItem, bool addToMenu)
        {
            bool containsParentID = dt.Columns.Contains("ParentID");
            
            if (containsParentID)
            {
                DataRow[] rows = dt.Select(String.Format("ParentID = {0}", currentRow["MenuItemID"]));

                AddMenuItems(menu, dt, menuItem, rows, addToMenu);
            }
        }
    }
}
