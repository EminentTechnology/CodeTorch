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
    public class PageHelper
    {
        

        internal static void DefaultPageProperties(Page page, MobileScreen Me)
        {


            
              
             Device.OnPlatform(
                Android: () =>
                {
                    
                    bool DisplayNav =  OnPlatformBool.GetAndroidValue(Me.DisplayNavigationBar);
                    NavigationPage.SetHasNavigationBar (page,DisplayNav);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Title)))
                        page.Title = OnPlatformString.GetAndroidValue(Me.Title);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.BackgroundColor)))
                        page.BackgroundColor = Color.FromHex(OnPlatformString.GetAndroidValue(Me.BackgroundColor));
                },
                iOS: () =>
                {
                    NavigationPage.SetHasNavigationBar (page, OnPlatformBool.GetiOSValue(Me.DisplayNavigationBar));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Title)))
                        page.Title = OnPlatformString.GetiOSValue(Me.Title);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.BackgroundColor)))
                        page.BackgroundColor = Color.FromHex(OnPlatformString.GetiOSValue(Me.BackgroundColor));
                },
                WinPhone: () =>
                {
                    NavigationPage.SetHasNavigationBar (page, OnPlatformBool.GetWinPhoneValue(Me.DisplayNavigationBar));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Title)))
                        page.Title = OnPlatformString.GetWinPhoneValue(Me.Title);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.BackgroundColor)))
                        page.BackgroundColor = Color.FromHex(OnPlatformString.GetWinPhoneValue(Me.BackgroundColor));
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

        internal static void DisplayErrorAlert(IMobilePage mobilepage, string message)
        {
            Page page = mobilepage.GetPage();

            page.DisplayAlert("Error",message, "OK");
        }
    }
}
