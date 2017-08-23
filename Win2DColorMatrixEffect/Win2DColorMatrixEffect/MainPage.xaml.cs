using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください


// NuGet パッケージ:
// Win2D.uwp https://www.nuget.org/packages/Win2D.uwp
// Coding4Fun Toolkit https://www.nuget.org/packages/Coding4Fun.Toolkit.Controls/

namespace Win2DColorMatrixEffect
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();

      // 1. Win2D のビットマップ (ここに画像を読み込ませる)
      CanvasBitmap bitmap = null;

      // 2. Win2D Canvas の CreateResources イベントで画像を読み込む
      win2DCanvas.CreateResources += (s, e) => 
      {
        // s is CanvasControl
        // e is CanvasCreateResourcesEventArgs
        e.TrackAsyncAction(LoadImageAsync(s).AsAsyncAction());

        async Task LoadImageAsync(CanvasControl canvas)
        {
          bitmap = await CanvasBitmap.LoadAsync(canvas, "Assets/ClockPanel.png");
          // ここでは 1 個だけだが、必要なだけ何個でも!
        }
      };

      // 3. Win2D Canvas の Draw イベントで bitmap を表示する
      win2DCanvas.Draw += (s, e) =>
      {
        // s is CanvasControl
        // e is CanvasDrawEventArgs

        // 単にビットマップをそのまま表示するなら、↓これだけ
        //e.DrawingSession.DrawImage(bitmap, 0, 0);

        // ビットマップにエフェクトを掛ける
        Color c = colorPicker.Color;
        float r = c.R / 255.0f;
        float g = c.G / 255.0f;
        float b = c.B / 255.0f;
        var colorMatrixEffect = new ColorMatrixEffect
        {
          Source = bitmap,
          ColorMatrix = new Matrix5x4
          {
            M11 = r, M12 = 0, M13 = 0, M14 = 0,
            M21 = 0, M22 = g, M23 = 0, M24 = 0,
            M31 = 0, M32 = 0, M33 = b, M34 = 0,
            M41 = 0, M42 = 0, M43 = 0, M44 = 1,
            M51 = 0, M52 = 0, M53 = 0, M54 = 0
          },
        };

        // エフェクトを掛けた結果を表示する
        e.DrawingSession.DrawImage(colorMatrixEffect, 0, 0);


        // ※ ここでは、Win2D Canvas を ViewBox の中に入れているため、表示サイズの調整が必要。
        // (Win2D Canvas を固定スケールで表示しているなら、つまり、普通に使っているなら、不要)
        canvasGrid.Width = bitmap.Size.Width;
        canvasGrid.Height = bitmap.Size.Height;
      };

      // 4. 必要に応じて Win2D Canvas を再描画
      colorPicker.ColorChanged += (s, e) =>
      {
        win2DCanvas.Invalidate();
      };


      SizeChanged += (s, e) => 
      {
        double edgeLength = Math.Min(ActualWidth, ActualHeight);
        viewBox1.Width = viewBox1.Height = edgeLength;
      };
    }
  }
}
