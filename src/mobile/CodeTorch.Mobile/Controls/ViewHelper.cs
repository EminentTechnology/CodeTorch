using CodeTorch.Core;
using CodeTorch.Core.Platform;
using CodeTorch.Mobile.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{
    public class ViewHelper
    {
        public static IView GetView(Page page, MobileScreen screen, BaseControl control)
        {
            IView retVal = null;

            switch (control.Type.ToLower().ToString())
            {
                case "boxview":
                    retVal = new CodeTorch.Mobile.BoxView();
                    break;
                case "button":
                    retVal = new CodeTorch.Mobile.Button();
                    break;
                case "entry":
                    retVal = new CodeTorch.Mobile.Entry();
                    break;
                case "image":
                    retVal = new CodeTorch.Mobile.Image();
                    break;

                case "label":
                    retVal = new CodeTorch.Mobile.Label();
                    break;
                case "listview":
                    retVal = new CodeTorch.Mobile.ListView();
                    break;
                case "stacklayout":
                    retVal = new CodeTorch.Mobile.StackLayout();

                    break;
                case "scrollview":
                    retVal = new CodeTorch.Mobile.ScrollView();
                    break;
                case "tableview":
                    retVal = new CodeTorch.Mobile.TableView();
                    break;
            }

            retVal.Page = page;
            retVal.BaseControl = control;
            retVal.Screen = screen;

            return retVal;
        }

        internal static void SetupView(IMobilePage page, MobileScreen screen, BaseControl control, IView view)
        {
            if (!String.IsNullOrEmpty(control.Name))
            {
                if (page.Elements.ContainsKey(control.Name))
                    {
                        throw new Exception(String.Format("Screen {0} - cannot have controls with duplicate names - {1}", screen.Name, control.Name));
                    }

                 page.Elements.Add(control.Name, view.GetView());
            }
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
    }
}
