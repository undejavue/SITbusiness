using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.ComponentModel;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Browser;

namespace Behaviors
{
    [System.ComponentModel.Description("Opens an HTML Window")]
    public class HTMLPopupWindow : TargetedTriggerAction<Button>, INotifyPropertyChanged
    {
        #region PopupURL
        public static readonly DependencyProperty PopupURLProperty = DependencyProperty.Register("PopupURL",
            typeof(string), typeof(HTMLPopupWindow), null);
        public string PopupURL
        {
            get
            {
                return (string)base.GetValue(PopupURLProperty);
            }
            set
            {
                base.SetValue(PopupURLProperty, value);
                this.NotifyPropertyChanged("PopupURL");
            }
        }
        #endregion

        #region PopupWidth
        public static readonly DependencyProperty PopupWidthProperty = DependencyProperty.Register("PopupWidth",
            typeof(int), typeof(HTMLPopupWindow), null);
        public int PopupWidth
        {
            get
            {
                return (int)base.GetValue(PopupWidthProperty);
            }
            set
            {
                base.SetValue(PopupWidthProperty, value);
                this.NotifyPropertyChanged("PopupWidth");
            }
        }
        #endregion

        #region PopupHeight
        public static readonly DependencyProperty PopupHeightProperty = DependencyProperty.Register("PopupHeight",
            typeof(int), typeof(HTMLPopupWindow), null);
        public int PopupHeight
        {
            get
            {
                return (int)base.GetValue(PopupHeightProperty);
            }
            set
            {
                base.SetValue(PopupHeightProperty, value);
                this.NotifyPropertyChanged("PopupHeight");
            }
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        protected override void Invoke(object parameter)
        {
            // From: http://sushantp.wordpress.com/2009/11/25/silverlight-reporting-support-for-ssrs-reports-problem-and-possible-solutions/
            if (true == HtmlPage.IsPopupWindowAllowed)
            {
                System.Text.StringBuilder codeToRun = new System.Text.StringBuilder();
                codeToRun.Append("window.open(");
                codeToRun.Append("\"");
                codeToRun.Append(string.Format("{0}", PopupURL));
                codeToRun.Append("\",");
                codeToRun.Append("\"");
                codeToRun.Append("\",");
                codeToRun.Append("\"");
                codeToRun.Append("width=" + PopupWidth.ToString() + ",height=" + PopupHeight.ToString());
                codeToRun.Append(",scrollbars=yes,menubar=no,toolbar=no,resizable=yes");
                codeToRun.Append("\");");
                try
                {
                    HtmlPage.Window.Eval(codeToRun.ToString());
                }
                catch
                {
                    MessageBox.Show("You must enable popups to view reports. Safari browser is not supported.",
                        "Error", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("You must enable popups to view reports. Safari browser is not supported.",
                    "Error", MessageBoxButton.OK);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        // Utility

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
