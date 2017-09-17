using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Win2DColorMatrixEffect
{
  public sealed partial class ColorChangeableCanvas : UserControl
  {
    private CanvasBitmap _bitmap;
    private Color _foreground;

    public ColorChangeableCanvas()
    {
      this.InitializeComponent();

      var watcher = new DependencyPropertyWatcher<Brush>(this, "Foreground");
      watcher.PropertyChanged += (s, e) =>
      {
        if (s is DependencyPropertyWatcher<Brush> w)
          ForegroundPropertyChanged(w.Value);
      };

      win2DCanvas.CreateResources += (s, e) =>
      {
        // s is CanvasControl
        // e is CanvasCreateResourcesEventArgs
        e.TrackAsyncAction(CreateResourcesAsync(s).AsAsyncAction());
      };

      win2DCanvas.Draw += (s, e) =>
      {
        // s is CanvasControl
        // e is CanvasDrawEventArgs
        if (_bitmap is null)
          return;
        if (IsLoadInProgress())
          return;

        // ビットマップにエフェクトを掛ける
        Color c = _foreground; //colorPicker.Color;
        float r = c.R / 255.0f;
        float g = c.G / 255.0f;
        float b = c.B / 255.0f;
        var colorMatrixEffect = new ColorMatrixEffect
        {
          Source = _bitmap,
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

        // ※ ここでは、Win2D Canvas を UserControl の中に入れているため、表示サイズの調整が必要。
        this.Width = _bitmap.Size.Width;
        this.Height = _bitmap.Size.Height;
      };
    }

    private void ForegroundPropertyChanged(Brush newBrush)
    {
      if (newBrush is SolidColorBrush scb)
        UpdateForegroundColor(scb.Color);
    }



    // 表示する画像ファイルのパス (依存関係プロパティ)
    public string Source
    {
      get => (string)GetValue(SourceProperty);
      set => SetValue(SourceProperty, value);
    }
    public static readonly DependencyProperty SourceProperty =
      DependencyProperty.Register(
        "Source", typeof(string), typeof(ColorChangeableCanvas),
        new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged))
      );
    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is ColorChangeableCanvas theInstance
            && e.NewValue is string newSource)
        theInstance.OnSourceChanged(e.OldValue as string, newSource);
      // ↑ e.OldValue は、最初は null (is not string)
    }

    private async void OnSourceChanged(string oldSource, string newSource)
    {
      if (string.Equals(oldSource, newSource))
        return;
#if DEBUG
      if (DesignMode.DesignModeEnabled)
        return;
#endif
      Reload();
      while (levelLoadTask != null && !levelLoadTask.IsCompleted)
        await Task.Delay(50);
      win2DCanvas.Invalidate();
    }

    // 表示色 (依存関係プロパティ)
    public Color ForegroundColor
    {
      get => (Color)GetValue(SourceProperty);
      set => SetValue(SourceProperty, value);
    }
    public static readonly DependencyProperty ForegroundColorProperty =
      DependencyProperty.Register(
        "ForegroundColor", typeof(Color), typeof(ColorChangeableCanvas),
        new PropertyMetadata(default(Color), new PropertyChangedCallback(OnForegroundColorChanged))
      );
    private static void OnForegroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is ColorChangeableCanvas theInstance
            && e.OldValue is Color oldColor
            && e.NewValue is Color newColor)
        theInstance.OnForegroundColorChanged(oldColor, newColor);
    }

    private void OnForegroundColorChanged(Color oldColor, Color newColor)
    {
      if (newColor != oldColor)
        UpdateForegroundColor(newColor);
    }

    private void UpdateForegroundColor(Color c)
    {
      _foreground = c;
      win2DCanvas.Invalidate();
    }





    #region "Loading resources outside of CreateResources"
    // https://microsoft.github.io/Win2D/html/LoadingResourcesOutsideCreateResources.htm

    private Task levelLoadTask;

    async Task CreateResourcesAsync(CanvasControl sender)
    {
      // If there is a previous load in progress, stop it, and
      // swallow any stale errors. This implements requirement #3.
      if (levelLoadTask != null)
      {
        levelLoadTask.AsAsyncAction().Cancel();
        try { await levelLoadTask; } catch { }
        levelLoadTask = null;
      }

      // Unload resources used by the previous level here.
      _bitmap?.Dispose();
      _bitmap = null;

      // Asynchronous resource loading, for globally-required resources goes here:

      // If we are already in a level, reload its per-level resources.
      // This implements requirement #4.
      if(!string.IsNullOrEmpty(Source))
      {
        Reload();
      }
    }

    public void Reload()
    {
#if DEBUG
      System.Diagnostics.Debug.Assert(levelLoadTask == null);
#endif
      levelLoadTask = LoadResourcesForLevelAsync();
    }

    private async Task LoadResourcesForLevelAsync()
    {
      _bitmap = await CanvasBitmap.LoadAsync(win2DCanvas, Source);
    }

    // Because of how this is designed to throw an exception, this must only 
    // ever be called from a Win2D event handler.
    private bool IsLoadInProgress()
    {
      // No loading task?
      if (levelLoadTask == null)
        return false;

      // Loading task is still running?
      if (!levelLoadTask.IsCompleted)
        return true;

      // Query the load task results and re-throw any exceptions
      // so Win2D can see them. This implements requirement #2.
      try
      {
        levelLoadTask.Wait();
      }
      catch (AggregateException aggregateException)
      {
        // .NET async tasks wrap all errors in an AggregateException.
        // We unpack this so Win2D can directly see any lost device errors.
        aggregateException.Handle(exception => { throw exception; });
      }
      finally
      {
        levelLoadTask = null;
      }

      return false;
    }
#endregion
  }
}
