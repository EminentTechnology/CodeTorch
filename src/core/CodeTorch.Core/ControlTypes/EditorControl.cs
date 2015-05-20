using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    public enum EditorToolbarMode
    {
        Default = 1,
        Floating = 2,
        PageTop = 4,
        ShowOnFocus = 8,
        RibbonBar = 16,
        RibbonBarFloating = 32,
        RibbonBarPageTop = 64,
        RibbonBarShowOnFocus = 128,
    }

    [Serializable]
    public class EditorControl : BaseControl
    {
        bool _EnableDesignMode = true;
        bool _EnablePreviewMode = true;
        bool _EnableHtmlMode = true;
        EditorToolbarMode _ToolbarMode = EditorToolbarMode.Default;

        public override string Type
        {
            get
            {
                return "Editor";
            }
            set
            {
                base.Type = value;
            }
        }

        [Category("Appearance")]
        public string Skin { get; set; }

        [Category("Appearance")]
        public string Height { get; set; }

        [Category("Behavior")]
        public bool AutoResizeHeight { get; set; }

        [Category("Behavior")]
        public int MaxHtmlLength { get; set; }

        [Category("Behavior")]
        public int MaxTextLength { get; set; }

        public string EnabledContentFilters { get; set; }
        public string DisabledContentFilters { get; set; }

        public bool EnableDesignMode
        {
            get { return _EnableDesignMode; }
            set { _EnableDesignMode = value; }
        }

        public bool EnablePreviewMode
        {
            get { return _EnablePreviewMode; }
            set { _EnablePreviewMode = value; }
        }

        public bool EnableHtmlMode
        {
            get { return _EnableHtmlMode; }
            set { _EnableHtmlMode = value; }
        }

        

        public EditorToolbarMode ToolbarMode
        {
            get { return _ToolbarMode; }
            set { _ToolbarMode = value; }
        }
    }
}
