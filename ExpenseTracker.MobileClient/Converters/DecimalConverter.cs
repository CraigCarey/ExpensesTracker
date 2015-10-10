using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ExpenseTracker.MobileClient.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return decimal.Parse(value as string);
        }
    }
}
