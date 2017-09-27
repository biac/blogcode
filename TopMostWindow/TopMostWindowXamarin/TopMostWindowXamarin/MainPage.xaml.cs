using Xamarin.Forms;

namespace TopMostWindowXamarin
{
  public partial class MainPage : ContentPage
  {
    private IViewMode m_ViewMode;
    private bool OnWindows { get; } = (Device.RuntimePlatform == Device.Windows);

    public MainPage()
    {
      InitializeComponent();

      if (OnWindows)
        m_ViewMode = DependencyService.Get<IViewMode>();

      SizeChanged += (s, e) =>
      {
        LabelWidth.Text = Width.ToString("0"); ;
        LabelHeight.Text = Height.ToString("0"); ;
      };
    }

    private async void Switch_Toggled(object sender, ToggledEventArgs e)
    {
      if (!OnWindows)
        return;

      if (e.Value)
        await m_ViewMode.EnterCompactOverlayAsync(m_ViewMode.LastWindowWidth, m_ViewMode.LastWindowHeight);
      else
        await m_ViewMode.ExitCompactOverlayAsync();
    }
  }
}
