using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Reflection; // to get the extension method for GetTypeInfo()
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Linq;

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






    // Enum 値を入力するための ComboBox (ジェネリック クラス)
    public class EnumComboBox<TEnum> : ComboBox
    where TEnum : struct, IComparable, IFormattable, IConvertible
    // 注意: where TEnum : Enum …とは書けない。
    //       しかたがないので Enum クラスの継承元を列挙して代用する。
  {
    private Dictionary<TEnum, string> m_items = new Dictionary<TEnum, string>();

    public EnumComboBox()
    {
#if DEBUG
      // TEnum が確かに Enum 型であることのチェック
      if (!typeof(TEnum).GetTypeInfo().IsEnum)
        throw new ArgumentException("TEnum must be an enumerated type");
#endif

      // Enum 値とその名前をペアにした Dictionary を作る
      var values = (TEnum[])Enum.GetValues(typeof(TEnum));
      foreach (var v in values)
        m_items.Add(v, GetName(v));

      // 上で作った Dictionary を、この ComboBox に表示する
      this.ItemsSource = m_items;
      this.DisplayMemberPath = "Value"; // 表示するもの (名前)
      this.SelectedValuePath = "Key"; // 選択されたもの (Enum 値)

      //this.Loaded += (s, e) =>
      //{
        m_watcher = new DependencyPropertyWatcher<object>(this, nameof(SelectedValue));
        m_watcher.PropertyChanged += OnSelectedValueChanged;
      //};
    }

    // Enum 値から名前を得るメソッド
    // この既定の実装は、Enum 値の名前をそのまま返す
    // (必要に応じて継承先で上書きする)
    protected virtual string GetName(TEnum value)
      => Enum.GetName(typeof(TEnum), value);



    private DependencyPropertyWatcher<object> m_watcher;

    private void OnSelectedValueChanged(object sender, EventArgs e)
    {
      if (m_watcher.Value is TEnum value)
      {
        int index = Array.IndexOf(Enum.GetValues(typeof(TEnum)), value);
        //base.SelectedIndex = index;
        this.SetValue(SelectedIndexProperty, index);

        //var item = m_items.First(kv => value.Equals(kv.Key));
        //this.SelectedItem = item;
      }
    }




    // 継承したプロパティのうち、見せたくないものを隠す
    private new object ItemsSource
    {
      get => base.ItemsSource;
      set => base.ItemsSource = value;
    }
    private new string DisplayMemberPath
    {
      get => base.DisplayMemberPath;
      set => base.DisplayMemberPath = value;
    }
    private new string SelectedValuePath
    {
      get => base.SelectedValuePath;
      set => base.SelectedValuePath = value;
    }
  }
}
