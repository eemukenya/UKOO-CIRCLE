﻿#pragma checksum "C:\Users\gabes\Desktop\Ukoo Circle II\Ukoo Circle\Pages\UserDetails\ChildrenListPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7B4507FB287823AAE563E228FD050264"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Ukoo_Circle.user_details_views {
    
    
    public partial class ChildrenListPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar progressbar;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox allusers;
        
        internal System.Windows.Controls.Canvas appbar_add;
        
        internal System.Windows.Shapes.Path canvAdd;
        
        internal System.Windows.Controls.Image image_settings;
        
        internal System.Windows.Controls.TextBlock textBlock_settings;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ukoo%20Circle;component/Pages/UserDetails/ChildrenListPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.progressbar = ((System.Windows.Controls.ProgressBar)(this.FindName("progressbar")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.allusers = ((System.Windows.Controls.ListBox)(this.FindName("allusers")));
            this.appbar_add = ((System.Windows.Controls.Canvas)(this.FindName("appbar_add")));
            this.canvAdd = ((System.Windows.Shapes.Path)(this.FindName("canvAdd")));
            this.image_settings = ((System.Windows.Controls.Image)(this.FindName("image_settings")));
            this.textBlock_settings = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock_settings")));
        }
    }
}

