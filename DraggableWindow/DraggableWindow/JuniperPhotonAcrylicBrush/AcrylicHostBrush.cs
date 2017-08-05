using System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MyerSplash.Common.Brush
{
  public class AcrylicHostBrush : AcrylicBrushBase
  {
    public AcrylicHostBrush()
    {

    }

    protected override BackdropBrushType GetBrushType()
    {
      return BackdropBrushType.HostBackdrop;
    }
  }
}