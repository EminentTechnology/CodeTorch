using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.SectionControls;
using CodeTorch.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class SetScreenObjectsDataCommandActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        SetScreenObjectsDataCommand Me = null;
        


        public void ExecuteCommand()
        {


            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();
            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(this.GetType());

            try
            {
                if (Command != null)
                {
                    Me = (SetScreenObjectsDataCommand)Command;
                }


                if (String.IsNullOrEmpty(Me.DataCommand))
                {
                    throw new ApplicationException(String.Format("Command {0} - SetScreenObjects - DataCommand is invalid", Me.Name));
                }
                log.DebugFormat("DataCommand:{0}", Me.DataCommand);

                DataCommand dataCommand = DataCommand.GetDataCommand(Me.DataCommand);
                if (dataCommand == null)
                {
                    throw new ApplicationException(String.Format("SetScreenObjects Data Command - {0} - does not exist in configuration", Me.DataCommand));
                }

                if (dataCommand.ReturnType != DataCommandReturnType.DataTable)
                {
                    throw new ApplicationException(String.Format("SetScreenObjects Data Command - {0} - invalid return type - return type must be Data Table", Me.DataCommand));
                }

                List<ScreenDataCommandParameter> parameters = null;
                parameters = pageDB.GetPopulatedCommandParameters(Me.DataCommand, Page);
                DataTable dt = dataCommandDB.GetDataForDataCommand(Me.DataCommand, parameters);

                foreach (DataRow row in dt.Rows)
                {
                    switch (row["ObjectType"].ToString().ToLower())
                    {
                        case "control":
                            SetPageControl(
                                row["Name"].ToString(),
                                row["MemberType"].ToString(),
                                row["Member"].ToString(),
                                row["Value"].ToString()
                                );
                            break;
                        case "section":
                            SetPageSection(
                                row["Name"].ToString(),
                                row["MemberType"].ToString(),
                                row["Member"].ToString(),
                                row["Value"].ToString()
                                );
                            break;
                    }


                }



            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
            
        }

        private void SetPageControl(string ControlName, string MemberType, string Member, string Value)
        {

            CodeTorch.Web.FieldTemplates.BaseFieldTemplate f = Page.FindFieldRecursive(ControlName);

            if (f != null)
            {
                f.Update(ControlName, MemberType, Member, Value);

            }
            else
            {
                SetViaReflection(ControlName, Member, Value);
            }
        }

        private void SetViaReflection(string ControlName, string Member, string Value)
        {
            try
            {
                //first find control
                object control = Page.FindControlRecursive(ControlName);

                //then attempt to set propery
                if (control != null)
                {
                    PropertyInfo propertyInfo = control.GetType().GetProperty(Member);

                    if (propertyInfo != null)
                    {
                        object controlValue = Convert.ChangeType(Value, propertyInfo.PropertyType);

                        propertyInfo.SetValue(control, controlValue);
                    }

                }
            }
            catch { }
        }

        private void SetPageSection(string SectionID, string MemberType, string Member, string Value)
        {
            BaseSectionControl s = Page.FindSection(SectionID);

            if (s != null)
            {
                s.Update(SectionID, MemberType, Member, Value);

            }
            else
            {
                SetViaReflection(SectionID, Member, Value);
            }
        }

        
    }
}
