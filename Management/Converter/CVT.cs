using Fbwf.Library.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Fbwf.Management.Converter
{
    public class IsTrueToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (value is Visibility && ((Visibility)value) == Visibility.Visible);
        }
    }

    public class IsFalseToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            return (value is bool && (bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (value is Visibility && ((Visibility)value) == Visibility.Collapsed);
        }
    }

    public class IsTrueToDisable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return (value is bool && (bool)value) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (value is bool && (bool)value) ? false : true;
        }
    }

    public class FbwfStatusToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return (value is Status && (Status)value == Status.Enable);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return (value is bool && ((bool)value)) ? Status.Enable : Status.Disabled;
        }
    }

    public class FbwfSizeDisplayToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return (value is SizeDisplayMode && (SizeDisplayMode)value == SizeDisplayMode.Virtual);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return (value is bool && ((bool)value)) ? SizeDisplayMode.Virtual : SizeDisplayMode.Nominal;
        }
    }
}
