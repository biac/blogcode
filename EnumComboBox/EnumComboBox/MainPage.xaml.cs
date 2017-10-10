using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace EnumComboBox
{
  public class SampleEnum1Selection : BindableBase
  {
    private SampleEnum1 m_enum1;
    public SampleEnum1 SampleEnum1Data
    {
      get => m_enum1;
      set => base.SetProperty(ref m_enum1, value);
    }
  }

  public class SampleData
  {
    public List<List<SampleEnum1Selection>> Data { get; }
      = new List<List<SampleEnum1Selection>>();
  }




  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    // バインドするデータ
    SampleData m_sampleData = new SampleData();

    public MainPage()
    {
      PrepareResource();
      this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      PrepareData();
      //Bindings.Update();
    }



    private void PrepareResource()
    {
      // ComboBox に表示する Enum 値と名前の Dictionary
      Dictionary<SampleEnum1, string> nameDictionary
        = new Dictionary<SampleEnum1, string> {
              { SampleEnum1.One, "1: 選択肢-A1"},
              { SampleEnum1.Two, "2: 選択肢-A2"},
              { SampleEnum1.Three, "3: 選択肢-A3"},
              { SampleEnum1.Four, "4: 選択肢-A4"},
              { SampleEnum1.Five, "5: 選択肢-A5"},
          };
      this.Resources.Add("SampleEnum1Dictionary", nameDictionary);
    }

    private void PrepareData()
    {
      m_sampleData.Data.Add(
        new List<SampleEnum1Selection> {
          new SampleEnum1Selection{ SampleEnum1Data = SampleEnum1.One, },
        });
      m_sampleData.Data.Add(
        new List<SampleEnum1Selection> {
          new SampleEnum1Selection{ SampleEnum1Data = SampleEnum1.One, },
          new SampleEnum1Selection{ SampleEnum1Data = SampleEnum1.Two, },
        });
      m_sampleData.Data.Add(
        new List<SampleEnum1Selection> {
          new SampleEnum1Selection{ SampleEnum1Data = SampleEnum1.Three, },
          new SampleEnum1Selection{ SampleEnum1Data = SampleEnum1.Four, },
          new SampleEnum1Selection{ SampleEnum1Data = SampleEnum1.Five, },
        });
    }
  }
}
