using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DraggableWindow
{
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();

      // draggableGrid をタイトルバー扱いにする
      // ⇒ ドラッグやダブルクリックや右クリックに反応するようになる
      Window.Current.SetTitleBar(draggableGrid);

      // 中身をタイトルバーにまで押し出し、[X] ボタン等の背景を透明にする。
      // 【参照】WinRT／Metro TIPS：タイトルバーにUIコントロールを配置するには？
      // http://www.atmarkit.co.jp/ait/articles/1510/14/news022.html
      CoreApplication.GetCurrentView()
        .TitleBar.ExtendViewIntoTitleBar = true;
      ApplicationView.GetForCurrentView()
        .TitleBar.ButtonBackgroundColor = Colors.Transparent;
      ApplicationView.GetForCurrentView()
        .TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

      // contentGrid 配下のマウスカーソルをキャプチャするパーツでは、
      // マウスカーソルが入ってきたら「↖」カーソル
      // マウスカーソルが出ていったら「✋」カーソル
      contentGrid.PointerEntered += (s, e) =>
      {
        SetPointerCursor(CoreCursorType.Arrow);
        e.Handled = true;
      };
      contentGrid.PointerExited += (s, e) =>
      {
        SetPointerCursor(CoreCursorType.Hand);
        e.Handled = true;
      };

      // Acrylic Effect
      InitializeAcrylicBrush();
    }

    private void SetPointerCursor(CoreCursorType cursorType, uint resourceId = 0)
      => Window.Current.CoreWindow.PointerCursor = new CoreCursor(cursorType, resourceId);

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      // マウスカーソルのデフォルトは「✋」にする（上手く行かない… orz）
      SetPointerCursor(CoreCursorType.Hand);
    }



    // Acrylic Effect

    UISettings _uiSettings = new UISettings();
    CoreWindowActivationState _activationState;

    private void SetBackgroundBrush()
    {
      var accentColor = _uiSettings.GetColorValue(UIColorType.AccentDark1);

      if (_uiSettings.AdvancedEffectsEnabled 
          && _activationState != CoreWindowActivationState.Deactivated)
      {
        backgroundGrid.Background 
          = new MyerSplash.Common.Brush.AcrylicHostBrush()
            {
              TintColor = accentColor,
              TintColorFactor = 0.4,
              BackdropFactor = 0.6,
              BlurAmount = 20,
            };
      }
      else
      {
        backgroundGrid.Background = new SolidColorBrush(accentColor);
      }
    }

    private void InitializeAcrylicBrush()
    {
      Window.Current.Activated
        += (sender, e) =>
        {
          _activationState = e.WindowActivationState;
          SetBackgroundBrush();
        };

      // 別スレッドで叩かれる!
      _uiSettings.AdvancedEffectsEnabledChanged
        += (setting, o) => runAsync_SetBackgroundBrush();

      // 別スレッドで叩かれる!
      _uiSettings.ColorValuesChanged
        += (setting, o) => runAsync_SetBackgroundBrush();

      async void runAsync_SetBackgroundBrush()
      {
        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        {
          SetBackgroundBrush();
        });
      }
    }
  }
}
