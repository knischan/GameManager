using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameManager.Data_Objects;
using static NtsLib.Utils;
using static GameManager.Base.Utils;

namespace GameManager.UI {
  public class Dialog {
    // *******************************************************************************
    //
    // Provides a messaging and input gathering interface.
    //
    // Usage:
    //
    // Basic message display:
    //   new Dialog("Message to display").Show();
    //
    // Collect input:
    //   string name = new Dialog("What is your name?", "OK", true).Show().Input;
    //
    //   Show() returns a DialogReturn which has the following properties:
    //     Input = The text that was entered
    //     Button = The button that was clicked
    //     DialogResult = The form's DialogResult
    //
    // Optional parameters:
    //   img::resname                     Load an image from the resource file, using name specified
    //   imgfile::filename                Load an image from disk, using the filename specified
    //   width::500                       Set form width to the specified width, default 600
    //   height::500                      Set form height to the specified height.  Will grow beyond if required.  Not yet implemented
    //   input                            Collect input
    //   input.default::(value)           Default value for input box
    //   input.email                      If using input, it must be an email address
    //   input.email.error::(msg)         Error to display when input is required to be email but is not.  Default "Please enter a valid email!"
    //   input.numeric                    If using input, it must be numeric
    //   input.numeric.max::5             If using numeric input, it cannot be larger than the specified value
    //   input.numeric.min::0             If using numeric input, it cannot be less than the specified value
    //   input.numeric.error::(msg)       Error to display when input is required to be numeric but is not.  Default "Please enter a valid number!"
    //   input.numeric.max.error::(msg)   Error to display when input is required to be no more than a value but is not.  Default "That number is too large!"
    //   input.numeric.min.error::(msg)   Error to display when input is required to be no less than a value but is not.  Default "That number is too low!"
    //   input.phone                      If using input, it must be a phone number
    //   input.phone.error::(msg)         Error to display when input is required to be phone but is not.  Default "Please enter a valid phone number!"
    //   input.req                        Input is required
    //   input.req.error                  Error to display when required input not provided.  Default "You need to provide input!"
    //   input.url                        If using input, it must be a URL
    //   input.url.error::(msg)           Error to display when input is required to be url but is not.  Default "Please enter a valid URL!"
    //   input.width::#                   Set input textbox to specified width, default 200
    //   trim                             If using input, trim the result
    //   lcase                            If using input, force input to lowercase
    //   ucase                            If using input, force input to uppercase
    //   inicaps                          If using input, format with initial caps
    //   imgwidth::500                    If using an image, set width to specified pixels
    //   imgheight::500                   If using an image, set height to specified pixels
    //   imgmargin::20                    If using an image, set margin to specified pixels
    //   imgsizemode::sizemode            If using an image, set image size mode to specified ImageSizeMode
    //   debugfile::filename              Write generated form code to specified filename
    //
    // For any values displayed, you can tokenize dialog parameters as %p:(parameter%.  So to make an error for invalid max numeric for example:
    //   new Dialog("What is your age?", "input", "input.numeric.max:65", "input.numeric.max.error:You're too old; can't be older than %p:input.numeric.max%!").Show();
    //
    // Any sub parameters automatically set parents if not specified.  So in the prior example with age, even though input.numeric was not specified it will be
    // set because input.numeric.max was set.
    // *******************************************************************************

    private string prompt = "", buttons = "";
    private Dictionary<string, string> parameters = new Dictionary<string, string>();
    private Dictionary<string, string> defaults = new Dictionary<string, string>() {
      { "input.width", "200" },
      { "width", "600" },
      { "input.email.error", "Please enter a valid email!" },
      { "input.numeric.error", "Please enter a valid number!" },
      { "input.numeric.max.error", "That number is too large!" },
      { "input.numeric.min.error", "That number is too small!" },
      { "input.phone.error", "Please enter a valid phone number!" },
      { "input.req.error", "Please enter a value!" },
      { "input.url.error", "Please enter a valid URL!" },
      { "imgwidth", "100" },
      { "imgheight", "100" },
      { "imgmargin", "20" },
      { "imgsizemode", "Zoom" }
    };


    public Dialog(string prompt) {
      this.prompt = prompt;
      buttons = "OK";
      PrepParameters();
    }

    public Dialog(string prompt, string buttons, bool hasInput = false) {
      this.prompt = prompt;
      this.buttons = buttons;
      if (hasInput) {
        parameters.Add("input", "");
        parameters.Add("input.width", defaults["input.width"]);
      }
      PrepParameters();
    }

    public Dialog(string prompt, string buttons, bool hasInput = false, params string[] parms) {
      this.prompt = prompt;
      this.buttons = buttons;
      if (hasInput) {
        parameters.Add("input", "");
        parameters.Add("input.width", defaults["input.width"]);
      }
      PrepParameters(parms);
    }

    public Dialog(string prompt, string buttons, params string[] parms) {
      this.prompt = prompt;
      this.buttons = buttons;
      PrepParameters(parms);
    }

    public DialogReturn Show() {
      string buf = "";
      int formWidth = GetInt(GetOpt("width", defaults["width"]), 0);
      while (true) {
        DialogReturn ret = new DialogReturn();
        int formheight = 20;

        Form dialog = new Form();
        dialog.Width = formWidth;
        dialog.Font = new Font("Microsoft Sans Serif", 12F);
        dialog.StartPosition = FormStartPosition.CenterScreen;

        Image img = null;
        try {
          if (HasOpt("img")) {
            img = (Bitmap)Properties.Resources.ResourceManager.GetObject(GetOpt("img"));

          }
          if (HasOpt("imgfile")) {
            img = Image.FromFile(GetOpt("imgfile"));
          }
        } catch {
        }
        if (img != null) {
          int imgWidth = GetInt(GetOpt("imgwidth", defaults["imgwidth"]), 0);
          int imgHeight = GetInt(GetOpt("imgheight", defaults["imgheight"]), 0);
          int topMargin = GetInt(GetOpt("imgmargin", defaults["imgmargin"]), 0);

          PictureBox pb = new PictureBox();
          pb.Size = new Size(imgWidth, imgHeight);
          pb.Location = new Point((formWidth - imgWidth) / 2, topMargin);
          pb.SizeMode = GetPictureboxSizeMode(GetOpt("imgsizemode", defaults["imgsizemode"]), PictureBoxSizeMode.Zoom);
          pb.Image = img;
          dialog.Controls.Add(pb);

          buf += "PictureBox pb = new PictureBox();" + Environment.NewLine;
          buf += "pb.BorderStyle = BorderStyle.FixedSingle;" + Environment.NewLine;
          buf += "pb.Size = new Size(100, 100);" + Environment.NewLine;
          buf += "pb.Location = new Point((" + formWidth + " - 100) / 2, 20);" + Environment.NewLine;
          buf += "dialog.Controls.Add(pb);" + Environment.NewLine;
          formheight += imgHeight + topMargin;
        }

        buf += "Form dialog = new Form();" + Environment.NewLine;
        buf += "dialog.Width = " + formWidth + "; " + Environment.NewLine;
        buf += "dialog.Font = new Font(\"Microsoft Sans Serif\", 12F);" + Environment.NewLine;
        buf += "dialog.StartPosition = FormStartPosition.CenterScreen;" + Environment.NewLine;

        Label label = new Label();
        label.Name = "label";
        label.UseMnemonic = false;
        label.AutoSize = true;
        label.MaximumSize = new Size(540, 0);
        label.Text = TokenizedString(prompt);
        label.Font = new Font("Microsoft Sans Serif", 12F);
        label.TextAlign = ContentAlignment.BottomCenter;
        dialog.Controls.Add(label);
        label.Location = new Point((dialog.Width - label.Width) / 2, formheight);
        formheight += label.Height + 20;

        buf += "Label label = new Label();" + Environment.NewLine;
        buf += "label.Name = \"label\";" + Environment.NewLine;
        buf += "label.UseMnemonic = false;" + Environment.NewLine;
        buf += "label.AutoSize = true;" + Environment.NewLine;
        buf += "label.MaximumSize = new Size(540, 0);" + Environment.NewLine;
        buf += "label.Text = \"" + TokenizedString(prompt) + "\";" + Environment.NewLine;
        buf += "label.Font = new Font(\"Microsoft Sans Serif\", 12F);" + Environment.NewLine;
        buf += "label.TextAlign = ContentAlignment.BottomCenter;" + Environment.NewLine;
        buf += "dialog.Controls.Add(label);" + Environment.NewLine;
        buf += "label.Location = new Point(" + ((dialog.Width - label.Width) / 2) + ", 20);" + Environment.NewLine;

        TextBox inputControl = new TextBox();
        buf += "TextBox inputControl = new TextBox();" + Environment.NewLine;
        if (HasOpt("input")) {
          inputControl.BackColor = Color.Ivory;
          inputControl.Name = "input";
          inputControl.Font = new Font("Microsoft Sans Serif", 12F);
          inputControl.Size = new Size(GetInt(GetOpt("input.width", defaults["input.width"])), 26);
          inputControl.Location = new Point((formWidth - inputControl.Width) / 2, formheight);
          inputControl.Text = TokenizedString(GetOrDefaultOpt("input.default", ""));
          inputControl.KeyDown += (sender, e) => {
            if (e.KeyCode == Keys.Enter) {
              e.SuppressKeyPress = true;
              dialog.Close();
            }
          };
          dialog.Controls.Add(inputControl);
          formheight += inputControl.Height + 20;
        }
        buf += "inputControl.BackColor = Color.Ivory;" + Environment.NewLine;
        buf += "inputControl.Name = \"input\";" + Environment.NewLine;
        buf += "inputControl.Font = new Font(\"Microsoft Sans Serif\", 12F);" + Environment.NewLine;
        buf += "inputControl.Size = new Size(" + GetInt(GetOpt("input.width")) + ", 26);" + Environment.NewLine;
        buf += "inputControl.Location = new Point(" + (formWidth - inputControl.Width) / 2 + ", " + formheight + ");" + Environment.NewLine;
        buf += "inputControl.Text = TokenizedString(GetOrDefaultOpt(\"input.default\", \"\"));" + Environment.NewLine;
        buf += "inputControl.KeyDown += (sender, e) => {" + Environment.NewLine;
        buf += "  if (e.KeyCode == Keys.Enter) {" + Environment.NewLine;
        buf += "    e.SuppressKeyPress = true;" + Environment.NewLine;
        buf += "    dialog.Close();" + Environment.NewLine;
        buf += "  }" + Environment.NewLine;
        buf += "};" + Environment.NewLine;
        buf += "dialog.Controls.Add(inputControl);" + Environment.NewLine;

        if (buttons.Trim() != "") {
          FlowLayoutPanel flp = new FlowLayoutPanel();
          flp.Location = new Point(10, formheight);
          int buttonwidth = 0, buttonheight = 0;
          string[] buttonarray = buttons.Split(',');
          buf += "FlowLayoutPanel flp = new FlowLayoutPanel();" + Environment.NewLine;
          buf += "flp.Location = new Point(10, " + formheight + ");" + Environment.NewLine;
          for (int x = 0; x < buttonarray.Length; x++) {
            Button button = new Button();
            button.Text = TokenizedString(buttonarray[x].Trim());
            button.AutoSize = true;
            button.Click += (sender, e) => {
              ret.Button = button.Text.Trim().ToLower();
              dialog.Close();
            };
            buttonwidth += button.Width + (x < buttonarray.Length - 1 ? 6 : 0);
            buttonheight = button.Height;
            flp.Controls.Add(button);
            buf += "  Button button" + x + " = new Button();" + Environment.NewLine;
            buf += "  button" + x + ".Text = \"" + TokenizedString(buttonarray[x].Trim()) + "\";" + Environment.NewLine;
            buf += "  button" + x + ".AutoSize = true;" + Environment.NewLine;
            buf += "  button" + x + ".Click += (sender, e) => {" + Environment.NewLine;
            buf += "    ret.Button = button" + x + ".Text.Trim().ToLower();" + Environment.NewLine;
            buf += "    dialog.Close();" + Environment.NewLine;
            buf += "  };" + Environment.NewLine;
            buf += "  flp.Controls.Add(button" + x + ");" + Environment.NewLine;
          }

          flp.Size = new Size(buttonwidth + 10, buttonheight + 10);
          flp.Location = new Point((dialog.Width - flp.Width) / 2, formheight);
          dialog.Controls.Add(flp);
          buf += "flp.Size = new Size(" + (buttonwidth + 10) + ", " + (buttonheight + 10) + ");" + Environment.NewLine;
          buf += "flp.Location = new Point(" + ((dialog.Width - flp.Width) / 2) + ", " + formheight + ");" + Environment.NewLine;
          buf += "dialog.Controls.Add(flp);" + Environment.NewLine;
        }

        dialog.Size = new Size(formWidth, formheight + 100);
        buf += "dialog.Size = new Size(" + formWidth + ", " + (formheight + 100) + ");" + Environment.NewLine;
        if (HasOpt("debugfile")) File.WriteAllText(GetOpt("debugfile"), buf);
        dialog.ShowDialog();

        if (HasOpt("input.req") && inputControl.Text.Trim() == "" && ret.Button.ToLower().Trim() != "cancel") {
          new Dialog(TokenizedString(GetOrDefaultOpt("input.req.error", defaults["input.req.error"])), "OK").Show();
          continue;
        }

        if (HasOpt("input.phone") && HasOpt("input") && !IsPhone(inputControl.Text.Trim())) {
          new Dialog(TokenizedString(GetOrDefaultOpt("input.phone.error", defaults["input.phone.error"])), "OK").Show();
          continue;
        }

        if (HasOpt("input.phone") && HasOpt("input") && !IsEmail(inputControl.Text.Trim())) {
          new Dialog(TokenizedString(GetOrDefaultOpt("input.phone.error", defaults["input.phone.error"])), "OK").Show();
          continue;
        }

        if (HasOpt("input.url") && HasOpt("input") && !IsUrl(inputControl.Text.Trim())) {
          new Dialog(TokenizedString(GetOrDefaultOpt("input.url.error", defaults["input.url.error"])), "OK").Show();
          continue;
        }

        if (HasOpt("input.numeric")) {
          if (!IsNumeric(inputControl.Text)) {
            new Dialog(TokenizedString(GetOrDefaultOpt("input.numeric.error", defaults["input.numeric.error"])), "OK").Show();
            continue;
          }

          int num = GetInt(inputControl.Text);
          if (HasOpt("input.numeric.max") && num > GetInt(GetOpt("input.numeric.max"))) {
            new Dialog(TokenizedString(GetOrDefaultOpt("input.numeric.max.error", defaults["input.numeric.max"])), "OK").Show();
            continue;
          }

          if (HasOpt("input.numeric.min") && num < GetInt(GetOpt("input.numeric.min"))) {
            new Dialog(TokenizedString(GetOrDefaultOpt("input.numeric.min.error", defaults["input.numeric.min"])), "OK").Show();
            continue;
          }
        }

        ret.Input = inputControl.Text;
        return ret;
      }
    }

    private void PrepParameters(string[] parms = null) {
      if (parms != null) {
        foreach (string parm in parms) {
          string[] tmp = Split(parm, "::");
          string key = tmp[0].ToLower().Trim();
          string value = tmp.Length == 1 ? "" : tmp[1];
          if (parameters.ContainsKey(key)) {
            parameters[key] = value;
            continue;
          }

          parameters.Add(key, value);
        }
      }

      Dictionary<string, string> tmpparms = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> parm in parameters) tmpparms.Add(parm.Key, parm.Value);
      foreach (KeyValuePair<string, string> parm in tmpparms) {
        string[] tmp = Split(parm.Key, ".");
        switch (tmp.Length) {
        case 1:
          switch (tmp[0]) {
          case "trim":
          case "lcase":
          case "ucase":
          case "inicaps":
            SetOpt("input");
            break;
          }
          break;

        case 2:
          if (tmp[0] == "input") SetOpt("input");
          break;

        case 3:
          if (tmp[0] == "input") {
            SetOpt("input");
            if (tmp[1] == "numeric") SetOpt("input.numeric");
            if (tmp[1] == "email") SetOpt("input.email");
            if (tmp[1] == "phone") SetOpt("input.phone");
            if (tmp[1] == "url") SetOpt("input.url");
          }
          break;

        default:
          if (tmp[0] == "input") {
            SetOpt("input");
            if (tmp[1] == "numeric") SetOpt("input.numeric");
          }
          break;
        }
      }

      if (HasOpt("input") && !HasOpt("input.width")) SetOpt("input.width", defaults["input.width"]);
      if (HasOpt("input.width") && !HasOpt("input")) SetOpt("input");
      //string p = "";
      //foreach (KeyValuePair<string, string> a in parameters) p += a.Key + " = " + a.Value + Environment.NewLine;
      //MessageBox.Show(p);
    }

    private string TokenizedString(string value) {
      string ret = value.Tokenize();
      foreach (KeyValuePair<string, string> parm in parameters) ret = ret.Replace("%p:" + parm.Key + "%", parm.Value);
      return ret;
    }

    private bool HasOpt(string opt) {
      return parameters.ContainsKey(opt.ToLower().Trim());
    }

    private string GetOpt(string opt, string defaultValue = "") {
      opt = opt.ToLower().Trim();
      return parameters.ContainsKey(opt) ? parameters[opt] : defaultValue;
    }

    private void DelOpt(string opt) {
      opt = opt.ToLower().Trim();
      if (!parameters.ContainsKey(opt)) return;

      parameters.Remove(opt);
    }

    private void SetOpt(string opt, string value = "") {
      opt = opt.ToLower().Trim();
      if (HasOpt(opt)) DelOpt(opt);

      parameters.Add(opt, value);
    }

    private string GetOrDefaultOpt(string opt, string defaultValue) {
      opt = opt.ToLower().Trim();
      string value = GetOpt(opt);
      return value.Trim() == "" ? defaultValue : value;
    }
  }
}
