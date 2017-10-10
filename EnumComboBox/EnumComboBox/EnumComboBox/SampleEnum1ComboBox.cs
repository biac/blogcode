using System;
using System.Collections.Generic;

namespace EnumComboBox
{
  public class SampleEnum1ComboBox : EnumComboBox<SampleEnum1>
  {
    private Dictionary<SampleEnum1, string> m_nameDictionary
      = new Dictionary<SampleEnum1, string> {
        { SampleEnum1.One, "1: 選択肢-A1"},
        { SampleEnum1.Two, "2: 選択肢-A2"},
        { SampleEnum1.Three, "3: 選択肢-A3"},
        { SampleEnum1.Four, "4: 選択肢-A4"},
        { SampleEnum1.Five, "5: 選択肢-A5"},
      };

    protected override string GetName(SampleEnum1 value)
    {
      return Enum.IsDefined(typeof(SampleEnum1), value)
                  ? m_nameDictionary[value]
                  : null;
    }
  }
}
