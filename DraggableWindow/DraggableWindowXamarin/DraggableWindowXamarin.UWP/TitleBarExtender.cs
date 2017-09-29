using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

[assembly: Xamarin.Forms.Dependency(typeof(DraggableWindowXamarin.UWP.TitleBarExtender))]
namespace DraggableWindowXamarin.UWP
{
  public class TitleBarExtender : ITitleBarExtender
  {
    public void Extend()
    {
      // 中身をタイトルバーにまで押し出し、[X] ボタン等の背景を透明にする。
      // 【参照】WinRT／Metro TIPS：タイトルバーにUIコントロールを配置するには？
      // http://www.atmarkit.co.jp/ait/articles/1510/14/news022.html
      CoreApplication.GetCurrentView()
        .TitleBar.ExtendViewIntoTitleBar = true;
      ApplicationView.GetForCurrentView()
        .TitleBar.ButtonBackgroundColor = Colors.Transparent;
      ApplicationView.GetForCurrentView()
        .TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
    }
  }
}
