using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TopMostWindowXamarin
{
  public partial class MainPage : ContentPage
  {
    private IViewMode m_ViewMode;

    public MainPage()
    {
      InitializeComponent();

      if(Device.RuntimePlatform == Device.Windows)
          m_ViewMode = DependencyService.Get<IViewMode>();

      SizeChanged += (s, e) =>
      {
        LabelWidth.Text = Width.ToString("0"); ;
        LabelHeight.Text = Height.ToString("0"); ;
      };
        
    }

    private async void Switch_Toggled(object sender, ToggledEventArgs e)
    {
      if (Device.RuntimePlatform != Device.Windows)
        return;

      if (e.Value)
      {
        if(m_ViewMode.LastWindowWidth > 0.0 && m_ViewMode.LastWindowHeight > 0.0)
          await m_ViewMode.EnterCompactOverlayAsync(m_ViewMode.LastWindowWidth, m_ViewMode.LastWindowHeight);
        else
          await m_ViewMode.EnterCompactOverlayAsync();
      }
      else
      {
        await m_ViewMode.ExitCompactOverlayAsync();
      }
    }
  }
}
