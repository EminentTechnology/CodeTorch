using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public enum WorkflowSelectionMode
    {
        Static,
        DataCommand
    }

    [Serializable]
    public class WorkflowStatusControl: BaseControl
    {
        bool _ReloadAfterStatusChange = true;
        ScreenInputType _EntityInputType = ScreenInputType.QueryString;

        [TypeConverter("CodeTorch.Core.Design.WorkflowTypeConverter,CodeTorch.Core.Design")]
        public string WorkflowCode { get; set; }

        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string WorkflowSelectionCommandName { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string WorkflowSelectionFieldName { get; set; }

        public WorkflowSelectionMode WorkflowSelectionMode { get; set; }


        [Category("Entity")]
        public string EntityID { get; set; }



        [Category("Entity")]
        public ScreenInputType EntityInputType
        {
            get { return _EntityInputType; }
            set { _EntityInputType = value; }
        }

        

        public bool ReloadAfterStatusChange
        {
            get { return _ReloadAfterStatusChange; }
            set { _ReloadAfterStatusChange = value; }
        }

        public override string Type
        {
            get
            {
                return "WorkflowStatus";
            }
            set
            {
                base.Type = value;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, WorkflowStatusControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "WorkflowChangeStatusButton.Label", "Change Status");




            return retVal;
        }

    }
}
