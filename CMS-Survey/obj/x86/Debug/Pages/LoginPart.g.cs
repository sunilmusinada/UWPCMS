﻿#pragma checksum "F:\CMS\CMS-Survey\UWPCMS\CMS-Survey\Pages\LoginPart.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FDD9D688FF93111FB6AB39A8C3BAF6C2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS_Survey.Pages
{
    partial class LoginPart : 
        global::Windows.UI.Xaml.Controls.UserControl, 
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
                    this.ThisPage = (global::Windows.UI.Xaml.Controls.UserControl)(target);
                    #line 9 "..\..\..\Pages\LoginPart.xaml"
                    ((global::Windows.UI.Xaml.Controls.UserControl)this.ThisPage).Loaded += this.ThisPage_Loaded;
                    #line default
                }
                break;
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.Button element2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\Pages\LoginPart.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element2).Click += this.CloseClicked;
                    #line default
                }
                break;
            case 3:
                {
                    this.UserName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4:
                {
                    this.Password = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 5:
                {
                    this.Login = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\Pages\LoginPart.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Login).Click += this.LoginClicked;
                    #line default
                }
                break;
            case 6:
                {
                    this.ErrorMessage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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

