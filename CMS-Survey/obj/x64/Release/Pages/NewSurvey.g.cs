﻿#pragma checksum "F:\downloads\CMS\CMS-Survey\UWPCMS\CMS-Survey\Pages\NewSurvey.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8A0D99151CD7ED135032A08B06D18293"
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
    partial class NewSurvey : 
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
                    this.ScrollBar = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                }
                break;
            case 2:
                {
                    this.progressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 3:
                {
                    this.mainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4:
                {
                    this.Previous = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 20 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Previous).Click += this.processPrevious;
                    #line default
                }
                break;
            case 5:
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.ProcessSave;
                    #line default
                }
                break;
            case 6:
                {
                    this.NextButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.NextButton).Click += this.processNext;
                    #line default
                }
                break;
            case 7:
                {
                    this.JumpButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 24 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.JumpButton).Loading += this.JumpButton_Loading;
                    #line 24 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.JumpButton).Loaded += this.JumpButton_Loaded;
                    #line default
                }
                break;
            case 8:
                {
                    this.HelpButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 55 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.HelpButton).Loading += this.JumpButton_Loading;
                    #line 55 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.HelpButton).Loaded += this.JumpButton_Loaded;
                    #line default
                }
                break;
            case 9:
                {
                    this.CommentsButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 58 "..\..\..\Pages\NewSurvey.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.CommentsButton).Click += this.CommentsButton_Click;
                    #line default
                }
                break;
            case 10:
                {
                    this.image = (global::Windows.UI.Xaml.Controls.Image)(target);
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

