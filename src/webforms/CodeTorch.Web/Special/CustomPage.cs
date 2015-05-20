using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;

namespace CodeTorch.Web.Templates
{
    public class CustomPage: BasePage
    {
        CustomScreen _Me = null;
        public CustomScreen Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = new CustomScreen();
                    this.Screen = _Me;
                }
                return _Me;
            }
        }

        public string Menu
        {
            get 
            {
                return Me.Menu.Name;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    Me.Menu.DisplayMenu = false;
                }
                else
                {
                    Me.Menu.Name = value;
                    Me.Menu.DisplayMenu = true;
                }
            }
        }

        public string Permission
        {
            get
            {
                return Me.ScreenPermission.Name;
            }
            set
            {
               Me.ScreenPermission.Name = value;
            }
        }

        public bool CheckPagePermission
        {
            get
            {
                return Me.ScreenPermission.CheckPermission;
            }
            set
            {
                Me.ScreenPermission.CheckPermission = value;
            }
        }

    }
}
