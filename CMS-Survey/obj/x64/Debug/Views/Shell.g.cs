﻿#pragma checksum "F:\downloads\CMS\CMS-Survey\UWPCMS\CMS-Survey\Views\Shell.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "36AA6E6416CCE743D5221AB4EF86E330"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS_Survey.Views
{
    partial class Shell : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.RootGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.MyHamburgerMenu = (global::Template10.Controls.HamburgerMenu)(target);
                }
                break;
            case 3:
                {
                    this.LoginModal = (global::Template10.Controls.ModalDialog)(target);
                }
                break;
            case 4:
                {
                    this.loginPart = (global::CMS_Survey.Pages.LoginPart)(target);
                    #line 75 "..\..\..\Views\Shell.xaml"
                    ((global::CMS_Survey.Pages.LoginPart)this.loginPart).HideRequested += this.LoginHide;
                    #line 76 "..\..\..\Views\Shell.xaml"
                    ((global::CMS_Survey.Pages.LoginPart)this.loginPart).LoggedIn += this.LoginLoggedIn;
                    #line default
                }
                break;
            case 5:
                {
                    this.SettingsButton = (global::Template10.Controls.HamburgerButtonInfo)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

