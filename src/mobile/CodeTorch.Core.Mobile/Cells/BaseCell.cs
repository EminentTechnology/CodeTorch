using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class BaseCell
    {
        public string Name { get; set; }
        public virtual string Type { get; set; }

        OnPlatformString _ClassID = new OnPlatformString();
        public OnPlatformString ClassID
        {
            get
            {
                return this._ClassID;
            }
            set
            {
                this._ClassID = value;
            }
        }

        OnPlatformString _StyleID = new OnPlatformString();
        public OnPlatformString StyleID
        {
            get
            {
                return this._StyleID;
            }
            set
            {
                this._StyleID = value;
            }
        }

        private bool _Visible = true;

        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        private bool _Enabled = true;

        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
            }
        }

        OnPlatformDouble _Height = new OnPlatformDouble();
        public OnPlatformDouble Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                this._Height = value;
            }
        }

       

        private Action _OnAppearing = new Action();
        public Action OnAppearing
        {
            get
            {
                return _OnAppearing;
            }
            set
            {
                _OnAppearing = value;
            }

        }

        private Action _OnDisappearing = new Action();
        public Action OnDisappearing
        {
            get
            {
                return _OnDisappearing;
            }
            set
            {
                _OnDisappearing = value;
            }

        }

        private Action _OnLongPressed = new Action();
        public Action OnLongPressed
        {
            get
            {
                return _OnLongPressed;
            }
            set
            {
                _OnLongPressed = value;
            }

        }

        private Action _OnTapped = new Action();
        public Action OnTapped
        {
            get
            {
                return _OnTapped;
            }
            set
            {
                _OnTapped = value;
            }

        }
    }
}
