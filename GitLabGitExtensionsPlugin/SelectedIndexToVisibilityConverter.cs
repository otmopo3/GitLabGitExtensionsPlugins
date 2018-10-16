using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitLabGitExtensionsPlugin
{
	class SelectedIndexToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var selectedIndex = (int)value;

			if (selectedIndex < 0)
				return Visibility.Collapsed;

			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
