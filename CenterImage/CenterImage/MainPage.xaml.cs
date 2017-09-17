using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace CenterImage
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    // バインドするデータ
    private StorageFile _file { get; set; }

    // バインディング パス内の関数 (Win10 1607 以降)
    private BitmapImage FileToBitmapImage(StorageFile file)
      => file is null ? null 
          : new BitmapImage(){ UriSource = new Uri(file.Path), };

    public MainPage()
    {
      this.InitializeComponent();

      Loaded += async (s, e) =>
      {
        _file = await Package.Current.InstalledLocation
                      .GetFileAsync("800px-Servals_Thoiry_19801.jpg");
        Bindings.Update();
      };
    }
  }
}
