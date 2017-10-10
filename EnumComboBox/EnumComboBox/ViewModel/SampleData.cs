using System.Collections.Generic;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace EnumComboBox
{
  public class SampleData
  {
    public List<List<SampleEnum1Selection>> Data { get; }
      = new List<List<SampleEnum1Selection>>();
  }

  public class SampleEnum1Selection : BindableBase
  {
    private SampleEnum1 m_enum1;
    public SampleEnum1 SampleEnum1Data
    {
      get => m_enum1;
      set => base.SetProperty(ref m_enum1, value);
    }
  }
}
