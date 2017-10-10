using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EnumComboBox
{
  // Getting change notifications from any dependency property in Windows Store Apps
  // https://blogs.msdn.microsoft.com/flaviencharlon/2012/12/07/getting-change-notifications-from-any-dependency-property-in-windows-store-apps/

  // And here is how to use it:
  //
  //private DependencyPropertyWatcher<string> watcher;
  //        
  //protected override void LoadState(Object navigationParameter, Dictionary pageState)
  //{
  //    watcher = new DependencyPropertyWatcher<string>(this.textBox, "Text");      //      
  //    watcher.PropertyChanged += OnTextChanged;
  //}
  //
  //private void OnTextChanged(object sender, EventArgs e)
  //{
  //    var text = watcher.Value;
  //    // ...
  //}

  public class DependencyPropertyWatcher<T> : DependencyObject, IDisposable
    {
      public static readonly DependencyProperty ValueProperty =
          DependencyProperty.Register(
              "Value",
              typeof(object),
              typeof(DependencyPropertyWatcher<T>),
              new PropertyMetadata(null, OnPropertyChanged));

      public event EventHandler PropertyChanged;

      public DependencyPropertyWatcher(DependencyObject target, string propertyPath)
      {
        this.Target = target;
        BindingOperations.SetBinding(
            this,
            ValueProperty,
            new Binding() { Source = target, Path = new PropertyPath(propertyPath), Mode = BindingMode.OneWay });
      }

      public DependencyObject Target { get; private set; }

      public T Value
      {
        get { return (T)this.GetValue(ValueProperty); }
      }

      public static void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
      {
        DependencyPropertyWatcher<T> source = (DependencyPropertyWatcher<T>)sender;

        source.PropertyChanged?.Invoke(source, EventArgs.Empty);
      }

      public void Dispose()
      {
        this.ClearValue(ValueProperty);
      }
    }
}
