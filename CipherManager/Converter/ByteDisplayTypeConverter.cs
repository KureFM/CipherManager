using CipherManager.View;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CipherManager.Converter
{
	public class ByteDisplayTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)((ByteDisplayType)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (ByteDisplayType)value;
		}
	}
}
