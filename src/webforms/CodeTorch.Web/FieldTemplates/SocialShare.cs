using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CodeTorch.Web.Data;
using CodeTorch.Core;
using System.Drawing;
using Telerik.Web.UI;
using System.Net.Configuration;
using System.Configuration;


namespace CodeTorch.Web.FieldTemplates
{
    public class SocialShare : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadSocialShare ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadSocialShare();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        SocialShareControl _Me = null;
        public SocialShareControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (SocialShareControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public override string Value
        {
            get
            {
                return ctrl.UrlToShare;
            }
            set
            {
                ctrl.UrlToShare = value;
            }
        }

        public override string DisplayText
        {
            get
            {
                return Value;
            }

        }

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {



                if (!String.IsNullOrEmpty(Me.Height))
                {
                    ctrl.Height = new Unit(Me.Height);
                }

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                ctrl.CssClass = "form-control";
                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass += " " + Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                if (!String.IsNullOrEmpty(Me.Skin))
                {
                    ctrl.Skin = Me.Skin;
                }

                //ctrl.CompactButtons
                //ctrl.MainButtons

                if (!String.IsNullOrEmpty(Me.DialogHeight))
                {
                    ctrl.DialogHeight = new Unit(Me.DialogHeight);
                }

                if (!String.IsNullOrEmpty(Me.DialogLeft))
                {
                    ctrl.DialogLeft = new Unit(Me.DialogLeft);
                }

                if (!String.IsNullOrEmpty(Me.DialogTop))
                {
                    ctrl.DialogTop = new Unit(Me.DialogTop);
                }

                if (!String.IsNullOrEmpty(Me.DialogWidth))
                {
                    ctrl.DialogWidth = new Unit(Me.DialogWidth);
                }

                if (!String.IsNullOrEmpty(Me.FacebookAppId))
                {
                    ctrl.FacebookAppId = Me.FacebookAppId;
                }

                if (!String.IsNullOrEmpty(Me.GoogleAnalyticsUA))
                {
                    ctrl.GoogleAnalyticsUA = Me.GoogleAnalyticsUA;
                }

                if (!String.IsNullOrEmpty(Me.YammerAppId))
                {
                    ctrl.YammerAppId = Me.YammerAppId;
                }

                if (!String.IsNullOrEmpty(Me.ToolTip))
                {
                    ctrl.ToolTip = Me.ToolTip;
                }

               
              
                //?ctrl.EmailSettings.
                ctrl.HideIframesOnDialogMove = Me.HideIframesOnDialogMove;

                
               
                

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "<br/><span style='color:red'>ERROR - {0} - Control {1} ({2} - {3})</span>";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                //this.ctrl.Text = ErrorMessages;
                LiteralControl errorMessage = new LiteralControl(ErrorMessages);
                this.Controls.Add(errorMessage);

                this.ctrl.BackColor = Color.Red;

            }


        }

        public override void LoadControl(object sender, EventArgs e)
        {

            try
            {
                DataRow row = null;

                if ((this.RecordObject != null) && (this.RecordObject is DataRow))
                {
                    row = (DataRow)this.RecordObject;
                }

                //title to share
                if (String.IsNullOrEmpty(Me.TitleToShare))
                {
                    if (row != null)
                    {
                        
                        ctrl.TitleToShare = String.Format("{0}", row[Me.DataTitleToShareField]);
                    }
                }
                else
                {
                    ctrl.TitleToShare = Me.TitleToShare;
                }

                //url to share
                if (String.IsNullOrEmpty(Me.UrlToShare))
                {
                    if (row != null)
                    {
                        string format = "{0}";

                        if (!String.IsNullOrEmpty(Me.UrlToShareFormatString))
                            format = Me.UrlToShareFormatString;

                        ctrl.UrlToShare = String.Format(format, Value);
                    }
                }
                else
                {
                    ctrl.UrlToShare = Me.UrlToShare;
                }
                

                


                //ctrl.MainButtons.Clear();
                foreach (SocialNetwork network in Me.DisplayedNetworks)
                {
                    RadSocialButton b = new RadSocialButton();

                    b.SocialNetType = (SocialNetType)Enum.Parse(typeof(SocialNetType), network.NetworkType.ToString());

                    if (!String.IsNullOrEmpty(network.LabelText))
                    {
                        b.LabelText = network.LabelText;
                    }

                    string urlFormat = "{0}";
                    if (!String.IsNullOrEmpty(network.UrlToShareFormatString))
                    {
                        urlFormat = network.UrlToShareFormatString;
                    }

                    if (!String.IsNullOrEmpty(network.UrlToShare))
                    {
                        b.UrlToShare = String.Format(urlFormat, network.UrlToShare);
                    }
                    else
                    {
                        b.UrlToShare = String.Format(urlFormat, ctrl.UrlToShare); 
                    }

                    b.TitleToShare = ctrl.TitleToShare;

                    ctrl.MainButtons.Add(b);
                }

                //ctrl.CompactButtons.Clear();
                foreach (SocialNetwork network in Me.CompactNetworks)
                {
                    RadSocialButton b = new RadSocialButton();

                    b.SocialNetType = (SocialNetType)Enum.Parse(typeof(SocialNetType), network.NetworkType.ToString());

                    if (!String.IsNullOrEmpty(network.LabelText))
                    {
                        b.LabelText = network.LabelText;
                    }

                    string urlFormat = "{0}";
                    if (!String.IsNullOrEmpty(network.UrlToShareFormatString))
                    {
                        urlFormat = network.UrlToShareFormatString;
                    }

                    if (!String.IsNullOrEmpty(network.UrlToShare))
                    {
                        b.UrlToShare = String.Format(urlFormat, network.UrlToShare);
                    }
                    else
                    {
                        b.UrlToShare = String.Format(urlFormat, ctrl.UrlToShare);
                    }

                    b.TitleToShare = ctrl.TitleToShare;

                    ctrl.CompactButtons.Add(b);
                }


            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                //this.ctrl.Text = ErrorMessages;
                LiteralControl errorMessage = new LiteralControl(ErrorMessages);
                this.Controls.Add(errorMessage);

                this.ctrl.BackColor = Color.Red;

            }
        }
    }
}
