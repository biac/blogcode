using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace EnumComboBox
{
  public class SampleEnum1ToSolidColorBrushConverter : IValueConverter
  {
    private static Dictionary<SampleEnum1, Color> m_colorDictionary
      = new Dictionary<SampleEnum1, Color> {
            { SampleEnum1.One, Color.FromArgb(0xff,0xb0,0xb0,0xff)}, //青
            { SampleEnum1.Two, Color.FromArgb(0xff,0xff,0xa0,0xa0)}, //赤
            { SampleEnum1.Three, Color.FromArgb(0xff,0xff,0xff,0x00)}, //黄
            { SampleEnum1.Four, Colors.White}, //白
            { SampleEnum1.Five, Color.FromArgb(0xff,0xe0,0xa0,0xff)}, //紫
        };

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (value is SampleEnum1 data)
      {
        return new SolidColorBrush(m_colorDictionary[data]);
      }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
      => DependencyProperty.UnsetValue;
  }
}
