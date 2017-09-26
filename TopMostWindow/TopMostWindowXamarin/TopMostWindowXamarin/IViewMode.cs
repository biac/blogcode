using System;
using System.Threading.Tasks;

namespace TopMostWindowXamarin
{
  public interface IViewMode
  {
    bool IsCompactOverlayMode { get; }
    bool IsCompactOverlaySupported { get; }
    bool IsDefaultMode { get; }
    double LastWindowHeight { get; }
    double LastWindowWidth { get; }

    event EventHandler ViewModeChanged;

    Task<bool> EnterCompactOverlayAsync(double desiredWidth = 0, double desiredHeight = 0);
    Task<bool> ExitCompactOverlayAsync();
  }
}