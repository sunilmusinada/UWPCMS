﻿#pragma checksum "F:\downloads\CMS\CMS-Survey\UWPCMS\CMS-Survey\Views\AssignSurvey.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "79952A5840F8629BA7B17034548EDDC2"
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
    partial class AssignSurvey : 
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
                    this.UserDataGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.Submit = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 20 "..\..\..\Views\AssignSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Submit).Click += this.Submit_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.ResetButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\Views\AssignSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.ResetButton).Click += this.ResetButto_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.progressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 5:
                {
                    this.Edit = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\Views\AssignSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Edit).Click += this.Edit_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.SurveyKey = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.Provider = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.SurveyKeyText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.ProviderKeyText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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

