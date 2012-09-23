﻿using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WeekNumber.Common
{
    public class CurrentWeekColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as bool?;
            if (boolValue != null)
                return (bool) boolValue ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);

            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
