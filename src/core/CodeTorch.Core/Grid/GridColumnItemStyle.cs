using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GridColumnItemStyle
    {
        bool _Wrap = true;
        string _CssClass = String.Empty;

        [Category("Appearance")]
        public string CssClass
        {
            get { return _CssClass; }
            set { _CssClass = value; }
        }


        [Category("Appearance")]
        public HorizontalAlignment HorizonalAlign { get; set; }

        [Category("Appearance")]
        public VerticalAlignment VerticalAlign { get; set; }

        

        
        [Category("Appearance")]
        public bool Wrap
        {
            get { return _Wrap; }
            set { _Wrap = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(CssClass))
            {
                sb.AppendFormat("CSS:{0};", CssClass);
            }

            if (HorizonalAlign != HorizontalAlignment.NotSet)
            {
                sb.AppendFormat("Horizonal:{0};", HorizonalAlign);
            }

            if (VerticalAlign != VerticalAlignment.NotSet)
            {
                sb.AppendFormat("Vertical:{0};", VerticalAlign);
            }

            sb.AppendFormat("Wrap:{0};", Wrap);
            

            return sb.ToString();
        }
        
    }
}
