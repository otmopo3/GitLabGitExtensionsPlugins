using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitLabGitExtensionsPlugin
{
	class ShowOnlyMyGroupsMrsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var isMyGroup = (bool)value;

			if (isMyGroup)
				return Visibility.Visible;

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
