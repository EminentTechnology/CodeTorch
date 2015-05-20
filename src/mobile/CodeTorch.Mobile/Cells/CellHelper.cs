using CodeTorch.Core;
using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{
    public class CellHelper
    {
        public static ICell GetCell(Page page, BaseControl control, BaseSection section, BaseCell cell)
        {
            ICell retVal = null;

            switch (cell.Type.ToLower().ToString())
            {
                case "entry":
                    retVal = new CodeTorch.Mobile.EntryCell();
                    break;
                case "image":
                    retVal = new CodeTorch.Mobile.ImageCell();
                    break;
                case "switch":
                    retVal = new CodeTorch.Mobile.SwitchCell();
                    break;
                case "text":
                    retVal = new CodeTorch.Mobile.TextCell();
                    break;

                
            }

            retVal.Page = page;
            retVal.BaseControl = control;
            retVal.Cell = cell;
            retVal.Section = section;

            return retVal;
        }

        internal static void DefaultViewProperties(IView view)
        {
            View formView = view.GetView();

            BaseControl me = view.BaseControl;

              
            Device.OnPlatform(
                Android: () =>
                {

                    if (OnPlatformDouble.GetAndroidValue(me.HeightRequest) != 0)
                        formView.HeightRequest = OnPlatformDouble.GetAndroidValue(me.HeightRequest);
                   
                    if (OnPlatformDouble.GetAndroidValue(me.WidthRequest) != 0)
                        formView.WidthRequest = OnPlatformDouble.GetAndroidValue(me.WidthRequest);


                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(me.BackgroundColor)))
                        formView.BackgroundColor = Color.FromHex( OnPlatformString.GetAndroidValue(me.BackgroundColor));

                    if(OnPlatformLayoutOptions.GetAndroidValue(me.HorizontalOptions) != FormLayoutOptions.NotSet)
                        formView.HorizontalOptions = GetLayoutOptions(me.HorizontalOptions);

                    if(OnPlatformLayoutOptions.GetAndroidValue(me.VerticalOptions) != FormLayoutOptions.NotSet)
                        formView.VerticalOptions = GetLayoutOptions(me.VerticalOptions);
                    
                },
                iOS: () =>
                {
                  if (OnPlatformDouble.GetiOSValue(me.HeightRequest) != 0)
                        formView.HeightRequest = OnPlatformDouble.GetiOSValue(me.HeightRequest);

                     if (OnPlatformDouble.GetiOSValue(me.WidthRequest) != 0)
                        formView.WidthRequest = OnPlatformDouble.GetiOSValue(me.WidthRequest);


                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(me.BackgroundColor)))
                        formView.BackgroundColor = Color.FromHex( OnPlatformString.GetiOSValue(me.BackgroundColor));

                    if(OnPlatformLayoutOptions.GetiOSValue(me.HorizontalOptions) != FormLayoutOptions.NotSet)
                        formView.HorizontalOptions = GetLayoutOptions(me.HorizontalOptions);

                    if(OnPlatformLayoutOptions.GetiOSValue(me.VerticalOptions) != FormLayoutOptions.NotSet)
                        formView.VerticalOptions = GetLayoutOptions(me.VerticalOptions);

                },
                WinPhone: () =>
                {
                   if (OnPlatformDouble.GetWinPhoneValue(me.HeightRequest) != 0)
                        formView.HeightRequest = OnPlatformDouble.GetWinPhoneValue(me.HeightRequest);

                     if (OnPlatformDouble.GetWinPhoneValue(me.WidthRequest) != 0)
                        formView.WidthRequest = OnPlatformDouble.GetWinPhoneValue(me.WidthRequest);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(me.BackgroundColor)))
                        formView.BackgroundColor = Color.FromHex( OnPlatformString.GetWinPhoneValue(me.BackgroundColor));

                    if(OnPlatformLayoutOptions.GetWinPhoneValue(me.HorizontalOptions) != FormLayoutOptions.NotSet)
                        formView.HorizontalOptions = GetLayoutOptions(me.HorizontalOptions);

                    if(OnPlatformLayoutOptions.GetWinPhoneValue(me.VerticalOptions) != FormLayoutOptions.NotSet)
                        formView.VerticalOptions = GetLayoutOptions(me.VerticalOptions);

                }
          );

            
            
        }

        private static LayoutOptions GetLayoutOptions(OnPlatformLayoutOptions options)
        {
            LayoutOptions retVal = LayoutOptions.Fill;

            FormLayoutOptions option = FormLayoutOptions.NotSet;

            Device.OnPlatform(
                Android: () =>
                {
                    option = OnPlatformLayoutOptions.GetAndroidValue(options);
                },
                iOS: () =>
                {
                    option = OnPlatformLayoutOptions.GetiOSValue(options);
                },
                WinPhone: () =>
                {
                    option = OnPlatformLayoutOptions.GetWinPhoneValue(options);
                }
            );

            
            switch (option)
            {
                case FormLayoutOptions.Center:
                    retVal = LayoutOptions.Center;
                    break;
                case FormLayoutOptions.CenterAndExpand:
                    retVal = LayoutOptions.CenterAndExpand;
                    break;
                case FormLayoutOptions.End:
                    retVal = LayoutOptions.End;
                    break;
                case FormLayoutOptions.EndAndExpand:
                    retVal = LayoutOptions.EndAndExpand;
                    break;
                case FormLayoutOptions.Fill:
                    retVal = LayoutOptions.Fill;
                    break;
                case FormLayoutOptions.FillAndExpand:
                    retVal = LayoutOptions.FillAndExpand;
                    break;
                case FormLayoutOptions.Start:
                    retVal = LayoutOptions.Start;
                    break;
                case FormLayoutOptions.StartAndExpand:
                    retVal = LayoutOptions.StartAndExpand;
                    break;
            }

            return retVal;
        }

        internal static Keyboard GetKeyboard(OnPlatformKeyboard platformKeyboard)
        {
            Keyboard retVal = Keyboard.Default;

            KeyboardType keyboardType = KeyboardType.NotSet;

            Device.OnPlatform(
                Android: () =>
                {
                    keyboardType = OnPlatformKeyboard.GetAndroidValue(platformKeyboard);
                },
                iOS: () =>
                {
                    keyboardType = OnPlatformKeyboard.GetiOSValue(platformKeyboard);
                },
                WinPhone: () =>
                {
                    keyboardType = OnPlatformKeyboard.GetWinPhoneValue(platformKeyboard);
                }
            );

            
            switch (keyboardType)
            {
                case KeyboardType.Default:
                    retVal = Keyboard.Default;
                    break;
                case KeyboardType.Chat:
                    retVal = Keyboard.Chat;
                    break;
                case KeyboardType.Email:
                    retVal = Keyboard.Email;
                    break;
                case KeyboardType.Numeric:
                    retVal = Keyboard.Numeric;
                    break;
                case KeyboardType.Telephone:
                    retVal = Keyboard.Telephone;
                    break;
                case KeyboardType.Text:
                    retVal = Keyboard.Text;
                    break;
                case KeyboardType.Url:
                    retVal = Keyboard.Url;
                    break;
            }

            return retVal;
        }

        internal static TextAlignment GetTextAlignment(OnPlatformTextAlignment platformTextAlignment)
        {
            TextAlignment retVal = TextAlignment.Start;

            TextAlignmentType textAlignmentType = TextAlignmentType.NotSet;

            Device.OnPlatform(
                Android: () =>
                {
                    textAlignmentType = OnPlatformTextAlignment.GetAndroidValue(platformTextAlignment);
                },
                iOS: () =>
                {
                    textAlignmentType = OnPlatformTextAlignment.GetiOSValue(platformTextAlignment);
                },
                WinPhone: () =>
                {
                    textAlignmentType = OnPlatformTextAlignment.GetWinPhoneValue(platformTextAlignment);
                }
            );

            
            switch (textAlignmentType)
            {
                case TextAlignmentType.Start:
                    retVal = TextAlignment.Start;
                    break;
                case TextAlignmentType.Center:
                    retVal = TextAlignment.Center;
                    break;
                case TextAlignmentType.End:
                    retVal = TextAlignment.End;
                    break;
            }

            return retVal;
        }

        internal static void DefaultViewProperties(ICell icell)
        {
            Cell cell = icell.GetCell();
            BaseCell Me = icell.Cell;
            
            cell.IsEnabled = Me.Enabled;
            

            Device.OnPlatform(
                Android: () =>
                {
                    if (OnPlatformDouble.GetAndroidValue(Me.Height) != 0)
                        cell.Height = OnPlatformDouble.GetAndroidValue(Me.Height);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.ClassID)))
                        cell.ClassId =  OnPlatformString.GetAndroidValue(Me.ClassID);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.StyleID)))
                        cell.StyleId =  OnPlatformString.GetAndroidValue(Me.StyleID);

                },
                iOS: () =>
                {
                    if (OnPlatformDouble.GetiOSValue(Me.Height) != 0)
                        cell.Height = OnPlatformDouble.GetiOSValue(Me.Height);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.ClassID)))
                        cell.ClassId =  OnPlatformString.GetiOSValue(Me.ClassID);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.StyleID)))
                        cell.StyleId =  OnPlatformString.GetiOSValue(Me.StyleID);

                },
                WinPhone: () =>
                {
                    if (OnPlatformDouble.GetWinPhoneValue(Me.Height) != 0)
                        cell.Height = OnPlatformDouble.GetWinPhoneValue(Me.Height);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.ClassID)))
                        cell.ClassId =  OnPlatformString.GetWinPhoneValue(Me.ClassID);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.StyleID)))
                        cell.StyleId =  OnPlatformString.GetWinPhoneValue(Me.StyleID);

                }
            );

            
        }

        

    }
}
