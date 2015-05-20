using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Platform
{
    public class OnPlatformBool
    {
        public OnPlatformBool()
        { }

        public OnPlatformBool(bool d)
        {
            this._Default = d;
            //this._Android = d;
            //this._iOS = d;
            //this._WinPhone = d;
        }

        bool _Default;
         public bool Default
        {
            get { return _Default; }
            set { _Default = value; _DefaultIsSet = true; }
        }

         bool _DefaultIsSet;

         public bool DefaultIsSet
         {
             get { return _DefaultIsSet; }
         }

       
        
        bool _Android;
        bool _AndroidIsSet;

        public bool AndroidIsSet
        {
            get { return _AndroidIsSet; }
        }

        public bool Android
        {
            get { return _Android; }
            set { _Android = value; _AndroidIsSet = true;}
        }
        
        bool _iOS;
        bool _iOSIsSet;

        public bool iOSIsSet
        {
            get { return _iOSIsSet; }
        }

        public bool iOS
        {
            get { return _iOS; }
            set { _iOS = value; _iOSIsSet = true; }
        }
        
        bool _WinPhone;
        bool _WinPhoneIsSet;

        public bool WinPhoneIsSet
        {
            get { return _WinPhoneIsSet; }
        }

        public bool WinPhone
        {
            get { return _WinPhone; }
            set { _WinPhone = value; _WinPhoneIsSet = true; }
        }

        public static bool GetAndroidValue(OnPlatformBool p)
        {
            
            return (p.AndroidIsSet) ? p.Android : p.Default;
        }

        public static bool GetiOSValue(OnPlatformBool p)
        {
            return (p.iOSIsSet) ? p.iOS : p.Default;
        }

        public static bool GetWinPhoneValue(OnPlatformBool p)
        {
           return (p.WinPhoneIsSet) ? p.WinPhone : p.Default;
        }
    }
}
