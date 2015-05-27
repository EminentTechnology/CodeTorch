using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class GridSection: Section
    {
        public override string Type
        {
            get
            {
                return "Grid";
            }
            set
            {
                base.Type = value;
            }
        }

        bool _LoadDataOnPageLoad = true;
        ScreenActionLink _ActionLink = new ScreenActionLink();
        Grid _Grid = null;

        [Category("Links")]
        public ScreenActionLink ActionLink { get { return _ActionLink; } set { _ActionLink = value; } }


        [Category("Data")]
        public bool LoadDataOnPageLoad { get { return _LoadDataOnPageLoad; } set { _LoadDataOnPageLoad = value; } }



        [Category("Grid")]
        public Grid Grid
        {
            get
            {
                if (_Grid == null)
                {
                    _Grid = new Grid();
                    //_Grid.Parent = this;
                }

                return _Grid;
            }
            set
            {
                _Grid = value;
            }

        }

        [Browsable(false)]
        public override List<BaseControl> Controls
        {
            get
            {
                return base.Controls;
            }
            set
            {
                base.Controls = value;
            }
        }

        [Browsable(false)]
        public override string Name
        {
            get
            {
                string retVal = null;

                if (this.Grid != null)
                {
                    retVal = this.Grid.Name;
                }

                return retVal;
            }
            set
            {
                base.Name = value;
            }
        }

        [Browsable(false)]
        public override string SelectDataCommand
        {
            get
            {
                string retVal = null;

                if (this.Grid != null)
                {
                    retVal = this.Grid.SelectDataCommand;
                }

                return retVal;
            }
            set
            {
                base.SelectDataCommand = value;
            }
        }

        

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, GridSection Section, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            AddResourceKey(retVal, Screen, Section, Prefix, "Grid.HelpText", Section.Grid.HelpText);

            foreach (GridColumn column in Section.Grid.Columns)
            {
                string ResourceKeyPrefix = String.Format("{0}.{1}.Grid.Columns", Prefix, Section.Name);
                retVal.AddRange(GridColumn.GetResourceKeys(Screen, column, ResourceKeyPrefix));
            }

           

            return retVal;
        }

        
    }
}
