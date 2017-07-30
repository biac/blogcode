using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
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
    }

    private void SetPointerCursor(CoreCursorType cursorType, uint resourceId = 0)
      => Window.Current.CoreWindow.PointerCursor = new CoreCursor(cursorType, resourceId);

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);

      // Acrylic Effect
      InitializeAcrylicBrush();

      // マウスカーソルのデフォルトは「✋」にする（上手く行かない… orz）
      SetPointerCursor(CoreCursorType.Hand);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
      base.OnNavigatingFrom(e);

      _hostSprite?.Dispose();
    }



    // Acrylic Effect

    private static readonly bool IsAcrylicSupported
      = ApiInformation.IsMethodPresent(
          "Windows.UI.Composition.Compositor", "CreateHostBackdropBrush")
        &&
        AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop";

    Windows.UI.Composition.SpriteVisual _hostSprite;

    private void InitializeAcrylicBrush()
    {
      if (!IsAcrylicSupported || _hostSprite != null)
        return;

      var compositor
        = ElementCompositionPreview.GetElementVisual(draggableGrid).Compositor;
      _hostSprite = compositor.CreateSpriteVisual();
      _hostSprite.Brush = compositor.CreateHostBackdropBrush();
      ElementCompositionPreview.SetElementChildVisual(draggableGrid, _hostSprite);

      draggableGrid.SizeChanged += (s, e) =>
      {
        _hostSprite.Size
          = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
      };
    }

  }
}
