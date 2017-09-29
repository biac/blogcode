using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
//using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(DraggableWindowXamarin.AcrylicGrid),
                          typeof(DraggableWindowXamarin.UWP.AcrylicGridRenderer))]
namespace DraggableWindowXamarin.UWP
{
  public  class AcrylicGridRenderer : ViewRenderer<Xamarin.Forms.Grid, Grid>
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Grid> e)
    {
      base.OnElementChanged(e);

      if (e.OldElement != null)
      {
        // (void)
      }
      if (e.NewElement != null)
      {
        if (Control == null)
        {
          SetNativeControl(new Grid());

          Control.Loaded += (s1, e1) => 
          {
            InitializeAcrylicBrush();
          };
        }
      }
    }



    UISettings _uiSettings { get; } = new UISettings();
    CoreWindowActivationState _activationState;

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

    private void SetBackgroundBrush()
    {
      var accentColor = _uiSettings.GetColorValue(UIColorType.AccentDark1);

      if (_uiSettings.AdvancedEffectsEnabled
          && _activationState != CoreWindowActivationState.Deactivated)
      {
        Control.Background
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
        Control.Background = new SolidColorBrush(accentColor);
      }
    }
  }
}
