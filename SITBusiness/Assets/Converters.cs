using System;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Browser;

namespace SITBusiness.Assets
{

    //--- Конвертеры ------------------------------------------------------
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return DependencyProperty.UnsetValue;
        }
    }

    public class Formatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                string formatterString = parameter.ToString();

                if (!string.IsNullOrEmpty(formatterString))
                {
                    return string.Format(culture, formatterString, value);
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
    

    public class BoolInvertor : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !Convert.ToBoolean(value);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class VisibilityTripple : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return Visibility.Collapsed;
                if (Convert.ToBoolean(value.ToString())) return System.Windows.Visibility.Visible;
            }
            catch { }
            return System.Windows.Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class VisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (Convert.ToBoolean(value.ToString())) return System.Windows.Visibility.Visible;
            }
            catch { }
            return System.Windows.Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class invVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (Convert.ToBoolean(value.ToString())) return System.Windows.Visibility.Collapsed;
            }
            catch { }
            return System.Windows.Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ImagePathConverter : IValueConverter
    {
        private string _rootUri;
        public string RootUri
        {
            get { return _rootUri; }
            set { _rootUri = value; }
        }

        public ImagePathConverter()
        {
            //string uri = HtmlPage.Document.DocumentUri.ToString();

            //_rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));
            //uri = _rootUri;
            //_rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));
        }

        public object Convert(object value, Type targetType, object parametr, CultureInfo culture)
        {
            string uri = HtmlPage.Document.DocumentUri.ToString();

            _rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));
            uri = _rootUri;
            _rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));

            string imagePath = RootUri + "/items_pics/" + (string)value;
            return new BitmapImage(new Uri(imagePath));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return null;
            throw new NotSupportedException();
        }
    }
}
