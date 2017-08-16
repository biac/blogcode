using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace TopMostWindow
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public event EventHandler ViewModeChanged;

    private static bool IsMobile
      => (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile");

    private static bool IsCompactOverlaySupported
      => ApplicationView.GetForCurrentView()
            .IsViewModeSupported(ApplicationViewMode.CompactOverlay);

    private static bool InCompactOverlayMode
      => (ApplicationView.GetForCurrentView().ViewMode
            == ApplicationViewMode.CompactOverlay);

    // 画面に表示するデータ
    private Size PageSize { get; set; }

    public MainPage()
    {
      this.InitializeComponent();

      if (IsMobile || !IsCompactOverlaySupported)
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
            await EnterTopMostAsync();
            viewModeToggle.IsChecked = true;
          }
        }
      };

      //Size lastCompactOverlaySize = Size.Empty;
      SizeChanged += (s, e) => 
      {
        PageSize = e.NewSize;
        this.Bindings.Update();

        //if (InCompactOverlayMode)
        //  lastCompactOverlaySize = e.NewSize;
      };

      ViewModeChanged += (s, e) =>
      {
        var winRect = Window.Current.Bounds;
        PageSize = new Size(winRect.Width, winRect.Height);
        this.Bindings.Update();
      };

      Application.Current.EnteredBackground += (s, e) => 
      {
        //if(lastCompactOverlaySize != Size.Empty)
        //  LocalSettings.CompactOverlaySize = lastCompactOverlaySize;
        if(viewModeToggle.IsChecked??false)
          LocalSettings.CompactOverlaySize = this.PageSize;
      };
    }


    private async void viewModeToggle_Click(object sender, RoutedEventArgs e)
    {
      viewModeToggle.IsEnabled = false;

      if (viewModeToggle.IsChecked ?? false)
        await EnterTopMostAsync();
      else
        await ExitTopMostAsync();

      viewModeToggle.IsChecked = InCompactOverlayMode;
      viewModeToggle.IsEnabled = true;
    }

    private async Task EnterTopMostAsync()
    {
      var previousSize = LocalSettings.CompactOverlaySize;

      ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
      if(previousSize != Size.Empty)
        compactOptions.CustomSize = previousSize;
      bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);

      if (modeSwitched)
      {
        //await this.Dispatcher.RunAsync
        // (CoreDispatcherPriority.Normal, () =>
        // {
           ViewModeChanged?.Invoke(this, EventArgs.Empty);
           LocalSettings.PreviousViewMode = ApplicationViewMode.CompactOverlay;
         //});
      }
    }

    private async Task ExitTopMostAsync()
    {
      LocalSettings.CompactOverlaySize = this.PageSize;

      bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);

      if (modeSwitched)
      {
        //await this.Dispatcher.RunAsync
        // (CoreDispatcherPriority.Normal, () =>
        // {
           ViewModeChanged?.Invoke(this, EventArgs.Empty);
           LocalSettings.PreviousViewMode = ApplicationViewMode.Default;
         //});
      }
    }

  }
}
