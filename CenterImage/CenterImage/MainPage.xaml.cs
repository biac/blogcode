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


    public MainPage()
    {
      this.InitializeComponent();

      Loaded += async (s, e) =>
      {
        // インストール フォルダーにある画像
        // ⇒ これは StorageFile.Path を Image コントロールにバインドできる
        ImageFile = await Package.Current.InstalledLocation
                      .GetFileAsync("800px-Servals_Thoiry_19801.jpg");
        await SetBitmpAsync(ImageFile);
        Bindings.Update();
      };
    }

    private async Task SetBitmpAsync(StorageFile file)
    {
      var b = new BitmapImage();
      using (var s = await file.OpenReadAsync())
      {
        b.SetSource(s);
      }
      Bitmap = b;
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
      var ImageFile = await picker.PickSingleFileAsync();
      if (ImageFile is null)
        return;

      // 任意の場所にある画像
      // ⇒ これは StorageFile.Path を Image コントロールにバインドできない
      //    つまり、BitmapImage などへの変換が必須
      this.ImageFile = ImageFile;
      await SetBitmpAsync(this.ImageFile);
      Bindings.Update();
    }
  }
}
