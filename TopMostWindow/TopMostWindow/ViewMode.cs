using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace TopMostWindow
{
  public class ViewMode
  {
    public static event EventHandler ViewModeChanged;

    public static bool IsCompactOverlaySupported
      => ApplicationView.GetForCurrentView()
            .IsViewModeSupported(ApplicationViewMode.CompactOverlay);

    public static ApplicationViewMode CurrentMode
      => ApplicationView.GetForCurrentView().ViewMode;
    public static bool IsCompactOverlayMode
      => (CurrentMode == ApplicationViewMode.CompactOverlay);
    public static bool IsDefaultMode
      => (CurrentMode == ApplicationViewMode.Default);

    public static Size LastWindowSize { get; private set; }
    private static void SetLastWindowSize()
    {
      var rect = Window.Current.Bounds;
      LastWindowSize = new Size(rect.Width, rect.Height);
    }



    static ViewMode()
    {
      Window.Current.SizeChanged += (s, e) => 
      {
        if (CurrentMode == ApplicationViewMode.CompactOverlay)
          SetLastWindowSize();
      };
    }

    public static async Task<bool> EnterCompactOverlayAsync(Size desiredSize = default(Size))
    {
      if (!IsCompactOverlaySupported)
        return false;

      ViewModePreferences compactOptions
        = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
      if (desiredSize != default(Size))
        compactOptions.CustomSize = desiredSize;

      bool modeSwitched 
        = await ApplicationView.GetForCurrentView()
                  .TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);

      if (modeSwitched)
        ViewModeChanged?.Invoke(ApplicationView.GetForCurrentView(), EventArgs.Empty);

      return modeSwitched;
    }

    public static async Task<bool> ExitCompactOverlayAsync()
    {
      if (!IsCompactOverlaySupported)
        return false;

      SetLastWindowSize();

      bool modeSwitched 
        = await ApplicationView.GetForCurrentView()
                  .TryEnterViewModeAsync(ApplicationViewMode.Default);

      if (modeSwitched)
        ViewModeChanged?.Invoke(ApplicationView.GetForCurrentView(), EventArgs.Empty);

      return modeSwitched;
    }
  }
}
