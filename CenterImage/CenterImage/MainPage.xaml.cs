using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace CenterImage
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    // バインドするデータ
    private StorageFile ImageFile { get; set; }
    private BitmapImage Bitmap { get; set; }

    private async Task UpdateImageAsync(StorageFile file)
    {
      ImageFile = file;

      var b = new BitmapImage();
      using (var s = await file.OpenReadAsync())
      {
        b.SetSource(s);
      }
      Bitmap = b;

      Bindings.Update();
    }



    public MainPage()
    {
      this.InitializeComponent();

      Loaded += async (s, e) =>
      {
        // インストール フォルダーにある画像
        // ⇒ これは StorageFile.Path を Image コントロールにバインドできる
        var file = await Package.Current.InstalledLocation
                      .GetFileAsync("800px-Servals_Thoiry_19801.jpg");
        await UpdateImageAsync(file);
      };
    }

    private async void FileOpenButton_Click(object sender, RoutedEventArgs e)
    {
      var picker = new FileOpenPicker
      {
        ViewMode = PickerViewMode.Thumbnail,
        SuggestedStartLocation = PickerLocationId.PicturesLibrary,
      };
      picker.FileTypeFilter.Add(".png");
      picker.FileTypeFilter.Add(".jpg");
      picker.FileTypeFilter.Add(".jpeg");
      picker.FileTypeFilter.Add(".gif");
      var file = await picker.PickSingleFileAsync();
      if (file is null)
        return;

      // 任意の場所にある画像
      // ⇒ これは StorageFile.Path を Image コントロールにバインドできない
      //    つまり、BitmapImage などへの変換が必須
      await UpdateImageAsync(file);
    }
  }
}
