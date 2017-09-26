using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

[assembly: Xamarin.Forms.Dependency(typeof(TopMostWindowXamarin.UWP.ViewMode))]
namespace TopMostWindowXamarin.UWP
{
  public class ViewMode : TopMostWindowXamarin.IViewMode
  {
    public event EventHandler ViewModeChanged;

    public bool IsCompactOverlaySupported
      => ApplicationView.GetForCurrentView()
            .IsViewModeSupported(ApplicationViewMode.CompactOverlay);

    private ApplicationViewMode CurrentMode
      => ApplicationView.GetForCurrentView().ViewMode;

    public bool IsCompactOverlayMode
      => (CurrentMode == ApplicationViewMode.CompactOverlay);

    public bool IsDefaultMode
      => (CurrentMode == ApplicationViewMode.Default);

    public double LastWindowWidth => LastWindowSize.Width;
    public double LastWindowHeight => LastWindowSize.Height;
    private Size LastWindowSize { get; set; }
    private void SetLastWindowSize()
    {
      var rect = Window.Current.Bounds;
      LastWindowSize = new Size(rect.Width, rect.Height);
    }



    public ViewMode()
    {
      Window.Current.SizeChanged += (s, e) =>
      {
        if (CurrentMode == ApplicationViewMode.CompactOverlay)
          SetLastWindowSize();
      };
    }

    public async Task<bool> EnterCompactOverlayAsync(double desiredWidth = default(double), double desiredHeight = default(double))
    {
      if (!IsCompactOverlaySupported)
        return false;

      Size desiredSize = new Size(desiredWidth, desiredHeight);

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

    public async Task<bool> ExitCompactOverlayAsync()
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
