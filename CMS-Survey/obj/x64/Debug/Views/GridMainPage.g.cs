﻿#pragma checksum "F:\downloads\CMS\CMS-Survey\UWPCMS\CMS-Survey\Views\GridMainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6026D5414A24839D014B06E38C18AB1"
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
    partial class GridMainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_MyToolkit_Controls_DataGrid_ItemsSource(global::MyToolkit.Controls.DataGrid obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
        };

        private class GridMainPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IGridMainPage_Bindings
        {
            private global::CMS_Survey.Views.GridMainPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::MyToolkit.Controls.DataGrid obj10;
            private global::Windows.UI.Xaml.Controls.AppBarButton obj15;

            private GridMainPage_obj1_BindingsTracking bindingsTracking;

            public GridMainPage_obj1_Bindings()
            {
                this.bindingsTracking = new GridMainPage_obj1_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 10:
                        this.obj10 = (global::MyToolkit.Controls.DataGrid)target;
                        break;
                    case 15:
                        this.obj15 = (global::Windows.UI.Xaml.Controls.AppBarButton)target;
                        ((global::Windows.UI.Xaml.Controls.AppBarButton)target).Click += (global::System.Object param0, global::Windows.UI.Xaml.RoutedEventArgs param1) =>
                        {
                        this.dataRoot.ViewModel.GotoSettings();
                        };
                        break;
                    default:
                        break;
                }
            }

            // IGridMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // GridMainPage_obj1_Bindings

            public void SetDataRoot(global::CMS_Survey.Views.GridMainPage newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::CMS_Survey.Views.GridMainPage obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_FilteredUsersurveys(obj.FilteredUsersurveys, phase);
                    }
                }
            }
            private void Update_FilteredUsersurveys(global::System.Collections.ObjectModel.ObservableCollection<global::CMS_Survey.Models.UserSurvey> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_MyToolkit_Controls_DataGrid_ItemsSource(this.obj10, obj, null);
                }
            }

            private class GridMainPage_obj1_BindingsTracking
            {
                global::System.WeakReference<GridMainPage_obj1_Bindings> WeakRefToBindingObj; 

                public GridMainPage_obj1_BindingsTracking(GridMainPage_obj1_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<GridMainPage_obj1_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_(null);
                }

                public void PropertyChanged_(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    GridMainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::CMS_Survey.Views.GridMainPage obj = sender as global::CMS_Survey.Views.GridMainPage;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_FilteredUsersurveys(obj.FilteredUsersurveys, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "FilteredUsersurveys":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_FilteredUsersurveys(obj.FilteredUsersurveys, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                public void UpdateChildListeners_(global::CMS_Survey.Views.GridMainPage obj)
                {
                    GridMainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        if (bindings.dataRoot != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)bindings.dataRoot).PropertyChanged -= PropertyChanged_;
                        }
                        if (obj != null)
                        {
                            bindings.dataRoot = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_;
                        }
                    }
                }
                public void PropertyChanged_FilteredUsersurveys(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    GridMainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::System.Collections.ObjectModel.ObservableCollection<global::CMS_Survey.Models.UserSurvey> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::CMS_Survey.Models.UserSurvey>;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                        }
                        else
                        {
                            switch (propName)
                            {
                                default:
                                    break;
                            }
                        }
                    }
                }
                public void CollectionChanged_FilteredUsersurveys(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    GridMainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        global::System.Collections.ObjectModel.ObservableCollection<global::CMS_Survey.Models.UserSurvey> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::CMS_Survey.Models.UserSurvey>;
                    }
                }
            }
        }
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
                    this.GridMainPage1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    #line 13 "..\..\..\Views\GridMainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Page)this.GridMainPage1).Loaded += this.GridMainPage1_Loaded;
                    #line default
                }
                break;
            case 2:
                {
                    this.ViewModel = (global::CMS_Survey.ViewModels.MainPageViewModel)(target);
                }
                break;
            case 3:
                {
                    this.AdaptiveVisualStateGroup = (global::Windows.UI.Xaml.VisualStateGroup)(target);
                }
                break;
            case 4:
                {
                    this.VisualStateNarrow = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 5:
                {
                    this.VisualStateNormal = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 6:
                {
                    this.VisualStateWide = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 7:
                {
                    this.pageHeader = (global::Template10.Controls.PageHeader)(target);
                }
                break;
            case 8:
                {
                    this.image = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 9:
                {
                    this.ActualGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 10:
                {
                    this.SurveyGrid = (global::MyToolkit.Controls.DataGrid)(target);
                    #line 97 "..\..\..\Views\GridMainPage.xaml"
                    ((global::MyToolkit.Controls.DataGrid)this.SurveyGrid).SelectionChanged += this.DataGrid_SelectionChanged;
                    #line default
                }
                break;
            case 11:
                {
                    this.IDFilter = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 85 "..\..\..\Views\GridMainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.IDFilter).TextChanged += this.Filter_TextChanged;
                    #line default
                }
                break;
            case 12:
                {
                    this.ProviderFilter = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 87 "..\..\..\Views\GridMainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.ProviderFilter).TextChanged += this.Filter_TextChanged;
                    #line default
                }
                break;
            case 13:
                {
                    this.EndDateFilter = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 89 "..\..\..\Views\GridMainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.EndDateFilter).TextChanged += this.Filter_TextChanged;
                    #line default
                }
                break;
            case 14:
                {
                    this.ActionDateFilter = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 91 "..\..\..\Views\GridMainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.ActionDateFilter).TextChanged += this.Filter_TextChanged;
                    #line default
                }
                break;
            case 15:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element15 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
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
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    GridMainPage_obj1_Bindings bindings = new GridMainPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

