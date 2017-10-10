﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnumComboBox
{
  public class BindableBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    // PropertyChangedイベントを発火させるメソッド
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this,
        new PropertyChangedEventArgs(propertyName));
    }

    // メンバ変数を変更し、PropertyChangedイベントを発火させるための、
    // ジェネリックなメソッド
    protected bool SetProperty<T>(ref T storage, T value,
                                 [CallerMemberName] string propertyName = null)
    {
      if (Equals(storage, value))
        return false;
      storage = value;
      OnPropertyChanged(propertyName);
      return true;
    }

  }
}
