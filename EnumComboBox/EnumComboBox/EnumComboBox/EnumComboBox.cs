using System;
using System.Collections.Generic;
using System.Reflection; // to get the extension method for GetTypeInfo()
using Windows.UI.Xaml.Controls;

namespace EnumComboBox
{
  // Enum 値を入力するための ComboBox (ジェネリック クラス)
  public class EnumComboBox<TEnum> : ComboBox
    where TEnum : struct, IComparable, IFormattable, IConvertible
    // 注意: where TEnum : Enum …とは書けない。
    //       しかたがないので Enum クラスの継承元を列挙して代用する。
  {
    public EnumComboBox()
    {
#if DEBUG
      // TEnum が確かに Enum 型であることのチェック
      if (!typeof(TEnum).GetTypeInfo().IsEnum)
        throw new ArgumentException("TEnum must be an enumerated type");
#endif

      // Enum 値とその名前をペアにした Dictionary を作る
      var items = new Dictionary<TEnum, string>();
      var values = (TEnum[])Enum.GetValues(typeof(TEnum));
      foreach (var v in values)
        items.Add(v, GetName(v));

      // 上で作った Dictionary を、この ComboBox に表示する
      this.ItemsSource = items;
      this.DisplayMemberPath = "Value"; // 表示するもの (名前)
      this.SelectedValuePath = "Key"; // 選択されたもの (Enum 値)

      // SelectedValue プロパティの変化を監視する
      m_watcher = new DependencyPropertyWatcher<object>(this, nameof(SelectedValue));
      m_watcher.PropertyChanged += OnSelectedValueChanged;
    }

    // Enum 値から名前を得るメソッド
    // この既定の実装は、Enum 値の名前をそのまま返す
    // (必要に応じて継承先で上書きする)
    protected virtual string GetName(TEnum value)
      => Enum.GetName(typeof(TEnum), value);



    private DependencyPropertyWatcher<object> m_watcher;
    private int m_lastSelectedIndex = -1;
    private void OnSelectedValueChanged(object sender, EventArgs e)
    {
      // 【UWP の ComboBox の不具合対応】
      // Enum 値を SelectedValue にバインドしているとき、
      // バインディングソース側で値が変化すると、
      // 1回は正常な値が来るが、続けて2回目として null が来る。
      // そこで 1回目のときに SelectedIndex を記憶しておき、
      // 2回目の null が来たときに復元する。
      if (m_watcher.Value is TEnum value)
        m_lastSelectedIndex = Array.IndexOf(Enum.GetValues(typeof(TEnum)), value);
      else
        this.SelectedIndex = m_lastSelectedIndex;
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
