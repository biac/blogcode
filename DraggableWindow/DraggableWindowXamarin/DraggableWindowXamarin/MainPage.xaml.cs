using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace DraggableWindowXamarin
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();

      DependencyService.Get<ITitleBarExtender>().Extend();
    }
  }
}
