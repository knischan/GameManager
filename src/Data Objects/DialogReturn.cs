using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameManager.Data_Objects {
  public class DialogReturn {
    public string Input { get; set; } = "";
    public string Button { get; set; } = "";
    public DialogResult Result { get; set; } = DialogResult.OK;

    public bool Chose(string value) {
      return value.ToLower().Trim() == Button.ToLower().Trim();
    }
  }
}
