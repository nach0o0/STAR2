using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WpfClient.Helpers
{
    public static class AutoScrollHelper
    {
        public static readonly DependencyProperty AutoScrollToTopOnChangeProperty =
            DependencyProperty.RegisterAttached(
                "AutoScrollToTopOnChange",
                typeof(object),
                typeof(AutoScrollHelper),
                new PropertyMetadata(null, OnAutoScrollToTopOnChangeChanged));

        public static object GetAutoScrollToTopOnChange(DependencyObject obj)
        {
            return obj.GetValue(AutoScrollToTopOnChangeProperty);
        }

        public static void SetAutoScrollToTopOnChange(DependencyObject obj, object value)
        {
            obj.SetValue(AutoScrollToTopOnChangeProperty, value);
        }

        private static void OnAutoScrollToTopOnChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToTop();
            }
        }
    }
}
