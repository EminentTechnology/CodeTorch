using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CodeTorch.Core;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;


namespace CodeTorch.Web.FieldTemplates
{
    public class DatePicker : BaseFieldTemplate
    {
        protected RadDatePicker ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new RadDatePicker();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        DatePickerControl _Me = null;
        public DatePickerControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (DatePickerControl)this.Widget;
                }
                return _Me;
            }
        }

        public override void InitControl(object sender, EventArgs e)
        {
  
            base.InitControl(sender, e);

            try
            {
                ctrl.EnableTyping = Me.EnableTyping;
                ctrl.MinDate = Me.MinDate;
                ctrl.DateInput.DateFormat = Me.DateFormat;
                ctrl.DateInput.DisplayDateFormat = Me.DisplayDateFormat;

                ctrl.Calendar.ShowRowHeaders = false;
                ctrl.Calendar.RangeMinDate = Me.MinDate;
                ctrl.Calendar.RangeMaxDate = Me.MaxDate;


                RadCalendarDay today = new RadCalendarDay();
                today.Repeatable = Telerik.Web.UI.Calendar.RecurringEvents.Today;
                today.ItemStyle.CssClass = "rcToday";

                ctrl.Calendar.SpecialDays.Add(today);

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

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

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.ToolTip = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }

            
            
           
        }

        public override string Value
        {
            get
            {
                string retVal = String.Empty;

                DateTime? d = ctrl.SelectedDate;

                if (d.HasValue)
                {
                    if (d.Value.ToShortDateString() == "1/1/0001")
                    {
                        retVal = null;
                    }
                    else
                    {
                       retVal = d.Value.ToString(ctrl.DateInput.DateFormat);
                    }

                    
                }


                return retVal;
            }
            set
            {
                DateTime? d = null;

                try
                {
                    d = DateTime.Parse(value);
                }
                catch
                { }

                ctrl.SelectedDate = d;
            }
        }

        public override string DisplayText
        {
            get
            {
                string retVal = String.Empty;

                DateTime? d = ctrl.SelectedDate;

                if (d.HasValue)
                {
                    if (d.Value.CompareTo(DateTime.MinValue) == 0)
                    {
                        retVal = null;
                    }
                    else
                    {
                        retVal = d.Value.ToString(ctrl.DateInput.DisplayDateFormat);
                    }


                }


                return retVal;
            }

        }

        //public override string GetValidationControlIDSuffix()
        //{
        //    return "$ctrl";
        //}

    }
}