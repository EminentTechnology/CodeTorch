using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace CodeTorch.Designer.Code
{
    class ObjectTreeNode: TreeNode
    {
        public object Object;

        public override string ToString()
        {
            return Object.ToString();
        }
    }

    public class SolutionTreeNode : RadTreeNode
    {
        public object Object;

        public string PluralTitle { get; set; }
        public string SingleType { get; set; }
        public string EntityType { get; set; }

        public string ConfigFolderPath { get; set; }

        public override string ToString()
        {
            return Object.ToString();
        }

        public string Key
        {
            get 
            {
                string retVal = null;

                if (Object != null)
                {
                    if (EntityType == Constants.ENTITY_TYPE_SCREEN)
                    {
                        CodeTorch.Core.Screen s = (CodeTorch.Core.Screen)Object;
                        retVal = String.Format("SCREEN|{0}|{1}", s.Folder, s.Name);
                    }
                    else
                    {
                        retVal = String.Format("{0}|{1}", EntityType.ToUpper(), Text);
                    }
                }

                return retVal;
            }
        }
    }
}
