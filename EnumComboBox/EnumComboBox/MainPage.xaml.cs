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
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    // この Page の DataContext にバインドするデータ
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
      int index = 1;
      m_sampleData.Data.Add(
        new List<SampleEnum1Selection> {
          new SampleEnum1Selection{
            SampleEnum1Data = SampleEnum1.One, Index=index++, },
        });
      m_sampleData.Data.Add(
        new List<SampleEnum1Selection> {
          new SampleEnum1Selection{
            SampleEnum1Data = SampleEnum1.One, Index=index++, },
          new SampleEnum1Selection{
            SampleEnum1Data = SampleEnum1.Two, Index=index++, },
        });
      m_sampleData.Data.Add(
        new List<SampleEnum1Selection> {
          new SampleEnum1Selection{
            SampleEnum1Data = SampleEnum1.Three, Index=index++, },
          new SampleEnum1Selection{
            SampleEnum1Data = SampleEnum1.Four, Index=index++, },
          new SampleEnum1Selection{
            SampleEnum1Data = SampleEnum1.Five, Index=index++, },
        });
    }



    // 2017/10/18 追加

    private int m_FocusedIndex {get;set;}

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      // フォーカスを受け取った TextBox にバインドされているデータを取り出す
      var textBox = sender as TextBox;
      var bindedData = textBox.DataContext as SampleEnum1Selection;
      
      // バインドされているデータのインデックスを画面右上に表示
      m_FocusedIndex = bindedData.Index;
      Bindings.Update();
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      m_FocusedIndex = 0;
      Bindings.Update();
    }
  }
}
