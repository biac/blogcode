using System;
using Windows.UI.Xaml.Data;

namespace EnumComboBox
{
  public sealed class EnumToIndexConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (value is Enum e)
        return Array.IndexOf(Enum.GetValues(e.GetType()), e);
      else
        return -1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      return (int)value;
    }
  }
}
