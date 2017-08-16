using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace TopMostWindow
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private static bool IsMobile
      => (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile");

    // 画面に表示するデータ
    private Size PageSize { get; set; }

    public MainPage()
    {
      this.InitializeComponent();

      if (IsMobile || !ViewMode.IsCompactOverlaySupported)
      {
        viewModeToggle.Visibility = Visibility.Collapsed;
      }

      bool isFirstActivate = true;
      Window.Current.Activated += async (s, e) => 
      {
        if (e.WindowActivationState == CoreWindowActivationState.CodeActivated
            && isFirstActivate)
        {
          isFirstActivate = false;
          if (LocalSettings.PreviousViewMode == ApplicationViewMode.CompactOverlay)
          {
            // ウインドウが初めてアクティブになったとき、
            // 前回終了時が CompactOverlay だったら、
            // CompactOverlay にする。
            await ViewMode.EnterCompactOverlayAsync(LocalSettings.CompactOverlaySize);
          }
        }
      };

      SizeChanged += (s, e) => 
      {
        // ウインドウのサイズが変わったとき:
        // ウインドウのサイズを取得し、画面を更新する。
        PageSize = e.NewSize;
        this.Bindings.Update();
      };

      ViewMode.ViewModeChanged += (s, e) =>
      {
        // ViewMode が切り替わったときに、
        // やりたいことがあれば、ここで。

        LocalSettings.PreviousViewMode = ViewMode.CurrentMode;
        viewModeToggle.IsChecked = ViewMode.IsCompactOverlayMode;
      };

      Application.Current.EnteredBackground += (s, e) => 
      {
        // アプリがバックグラウンドに移されるとき:
        // 必要なデータを保存する
        // ※ 1607 以前では Application.Current.Suspending でやっていた処理

        LocalSettings.CompactOverlaySize = ViewMode.LastWindowSize;
      };
    }


    private async void viewModeToggle_Click(object sender, RoutedEventArgs e)
    {
      viewModeToggle.IsEnabled = false;

      if (viewModeToggle.IsChecked ?? false)
        await EnterTopMostAsync();
      else
        await ExitTopMostAsync();

      viewModeToggle.IsEnabled = true;
    }

    private async Task EnterTopMostAsync()
    {
      var previousSize = LocalSettings.CompactOverlaySize;
      await ViewMode.EnterCompactOverlayAsync(previousSize);
    }

    private async Task ExitTopMostAsync()
    {
      LocalSettings.CompactOverlaySize = this.PageSize;
      await ViewMode.ExitCompactOverlayAsync();
    }
  }
}
