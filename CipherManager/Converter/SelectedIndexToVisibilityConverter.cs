using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CipherManager.Converter
{
	internal class SelectedIndexToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = System.Convert.ToInt32(parameter);
			int num2 = System.Convert.ToInt32(value);
			if (num == num2)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
