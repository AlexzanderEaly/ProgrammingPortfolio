﻿#pragma checksum "..\..\..\AddModifyUser.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FFB99170E3E793B8EB0D011EC91C3111A182D35D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using webSystemGUI;


namespace webSystemGUI {
    
    
    /// <summary>
    /// AddModifyUser
    /// </summary>
    public partial class AddModifyUser : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\AddModifyUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox userEmailTxt;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\AddModifyUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox firstNameTxt;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\AddModifyUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox lastNameTxt;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\AddModifyUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addUserBtn;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\AddModifyUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button modfiyUserBtn;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\AddModifyUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button lookupUserBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/webSystemGUI;component/addmodifyuser.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AddModifyUser.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.userEmailTxt = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.firstNameTxt = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.lastNameTxt = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.addUserBtn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\AddModifyUser.xaml"
            this.addUserBtn.Click += new System.Windows.RoutedEventHandler(this.addUserBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.modfiyUserBtn = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\AddModifyUser.xaml"
            this.modfiyUserBtn.Click += new System.Windows.RoutedEventHandler(this.modfiyUserBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lookupUserBtn = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\AddModifyUser.xaml"
            this.lookupUserBtn.Click += new System.Windows.RoutedEventHandler(this.lookupUserBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

