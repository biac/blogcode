using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
//using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(DraggableWindowXamarin.DraggableGrid),
                          typeof(DraggableWindowXamarin.UWP.DraggableGridRenderer))]
namespace DraggableWindowXamarin.UWP
{
  public  class DraggableGridRenderer : ViewRenderer<Xamarin.Forms.Grid, Grid>
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

          // draggableGrid をタイトルバー扱いにする
          // ⇒ ドラッグやダブルクリックや右クリックに反応するようになる
          Window.Current.SetTitleBar(Control);
          // ただし、透明でいいので Background は塗りつぶしておくこと
          Control.Background = new SolidColorBrush(Colors.Transparent);
        }
      }
    }
  }
}
