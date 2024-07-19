using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameManager.Utils;

namespace GameManager.Data_Objects {
  public class User {
    public int UserId { get; set; } = 0;
    public long ACL { get; set; } = 0;
    public long Flags { get; set; } = 0;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";

    public User() {

    }
  }
}
