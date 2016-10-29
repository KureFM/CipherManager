using System;
using System.Globalization;
using System.Windows.Data;

namespace CipherManager.Converter
{
	internal class SelectedIndexToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = System.Convert.ToInt32(value);
			if (num >= 0)
			{
				return true;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
