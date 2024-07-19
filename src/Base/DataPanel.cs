using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NtsLib.Utils;
using GameManager.UI;

namespace GameManager.Base {
  public class DataPanel {
    // *********************************************************************************************************************************************************************************************************************************************
    //
    // Provides an interface to simplify managing object editing
    //
    // To use, create a new DataPanel in your config form:
    //   class Whatever {
    //     private DataPanel dpSql;
    //
    // Initialize it during the form load.  Pass it the Panel with the controls and the item the data will come from.
    //   dpSql = new DataPanel(pnlSql, config);
    //
    // Add any parameters you need to add:
    //  dpSQL.Parameters.Add("default.SqlPort", "3306");
    //
    // Then load the form:
    //  dpSql.LoadForm(config);
    //
    // When you want to save, it will return whatever the source was.
    //  private void Save() {
    //    if (!dpSql.Validate()) return;
    //
    //    Configuration newcfg = (Configuration)dpSql.Save();
    //  }
    //
    // If you are wanting to edit a collection in an object, you do it like this.  Let's assume you have a combobox "sources" on your sources panel:
    //   dpSources = new DataPanel(pnlSources, config.Sources);
    //   dpSources.Parameters.Add("addfirst.sources", "[ New ]");
    //   dpSources.Parameters.Add("items.sources", config.GetSourceNames());
    //   dpSources.LoadItems(sources);
    // 
    // Be sure to mark your sources combobox as a master selector (*) and give it the object it will be loading.  In this case, my Tag was *,_Source.
    // Now add your SelectedIndexChanged event:
    //   private void sources_SelectedIndexChanged(object sender, EventArgs e) {
    //     if (sources.SelectedIndex == -1) {
    //       dpSources.ClearForm();
    //       return;
    //     }
    //  
    //     if (sources.SelectedIndex == 0) {
    //       dpSources.ClearForm(false);
    //       dpSources.NewEntry();
    //       dpSources.Source = new Source();
    //       ActiveControl = srcName;
    //       return;
    //     }
    //
    //     source = config.GetSource(sources.Text);
    //     if (source == null) {
    //       new Dialog("Could not load source!", "img:alert").Show();
    //       sources.SelectedIndex = -1;
    //       return;
    //     }
    //     dpSources.LoadForm(source);
    //     ActiveControl = srcName;
    //   }
    //  
    //   So if it is -1, it clears the form.  If it is 0, that means it is a new record since we used the addfirst parameter to add "[ New ]" as the first item.
    //   If it's new, clear form and pass false so it doesn't reset the selector (it needs to keep on the new so we know when we save it's new).  Call the
    //   DataPanel#NewEntry() method to tell it it's new, then set the source to a new instance of the object in question.  Set focus if desired and return.
    //  
    //   If it is > 0, then it means you are editing an existing object.  You need to first get an object from the text then call DataPanel#LoadForm() and pass
    //   that object to it.  Set focus if desired.
    //  
    // Now that the form has been loaded, we need to add typically three buttons to the panel for save/delete/cancel.  For the delete button, since we do not want 
    // it to enable when we create a new record set the tag to -new.  Create events for each:
    //   private void btnSrcSave_Click(object sender, EventArgs e) {
    //     if (!dpSources.Validate()) return;
    //   
    //     Source tmp = (Source)dpSources.Save();
    //     if (sources.SelectedIndex == 0) {
    //       if (config.GetSource(tmp.Name) != null) {
    //         new Dialog("A source with that name already exists!", "img:alert").Show();
    //         ActiveControl = srcName;
    //         return;
    //       }
    //   
    //       config.Sources.Add(tmp);
    //     } else {
    //       config.Sources[config.Sources.IndexOf(tmp)] = tmp;
    //     }
    //   
    //     dpSources.ClearForm();
    //     sources.Items.Clear();
    //     dpSources.Parameters["items.sources"] = config.GetSourceNames();
    //     dpSources.LoadItems(sources);
    //   }
    //
    //   For saving, we call the DataPanel#Validate() method to ensure the data is correct.  Then we create a new object based on the return of DataPanel#Save()
    //   Be sure you specified the object type in the combobox tag as I mentioned earlier or else the cast will fail.  
    //   If the master selector selectedindex is 0, it's a new record since we added new first to the items.  So check if the parent object collection already has
    //   the item and yell at the user if so.  If not, add it.
    //   If the mater selector is above 0, it's saving updates to an existing entry so just replace the item in the parent object collection with the new object.
    //   After that's all done, we need to call DataPanel#ClearForm to reset the form then we clear the master selector and reload its items parameter since now
    //   there will be a new item it needs to show.  Lastly you call DataPanel#LoadItems and pass the collection so it can refresh the combobox.
    //
    //   
    //   private void btnSrcDelete_Click(object sender, EventArgs e) {
    //     if (new Dialog("Are you sure?", "Yes,No").Show().Chose("No")) return;
    //   
    //     config.DeleteSource(sources.Text);
    //     dpSources.ClearForm();
    //     sources.Items.Clear();
    //     dpSources.Parameters["items.sources"] = config.GetSourceNames();
    //     dpSources.LoadItems(sources);
    //   }
    //
    //   For deleting, if they do not say no then we delete the object from the parent collection then call DataPanel#ClearForm() to reset the form.  We need to
    //   update the master selector now, so clear its items and update the items parameter with the current values then call DataPanel#LoadItems and pass it the
    //   updated collection to finish updating the combobox.
    //
    //   
    //   private void btnSrcCancel_Click(object sender, EventArgs e) {
    //     dpSources.ClearForm();
    //   }
    //
    //   For cancelling, it's pretty straight forward.  Just clear the form.
    //
    // *********************************************************************************************************************************************************************************************************************************************
    //
    // Control tag configuration structure.  Comma delimited.
    //
    // * = Designates control as the master selector
    // # = Value must be numeric.  Follow with value for what to use when field is empty
    // % = Value is encrypted
    // ! = Value must not be empty
    // $ = Value is in currency format
    // ^ = Value is a date/time
    // = = Value is a directory path.  Pass ctrl name after for +TAB target
    // _ = Prefixes display name for field (used in error messages)
    // ( = Property is for saving, not loading.  Display no value when loaded
    // - = Control should be disabled when performing action (e.g. -new, -load, etc)
    // ) = Control is a class (class name to follow).  If next char is *, load props
    // & = Do not save if value is empty
    // @ = Value should be an existing filename
    // ~ = Ignore this control, do nothing with it
    //
    // An entry that has no prefix is assumed to be a property of the source object
    //
    // Parameters:
    //
    // default.CTRL       Default value for control name
    // items.CTRL         List<string> of values to load in a ComboBox
    // addfirst.CTRL      String or List<string> of entry/ies to load first in a ComboBox
    // addlast.CTRL       String or List<string> of entry/ies to load last in a ComboBox
    //
    // *********************************************************************************************************************************************************************************************************************************************

    public object Source { get; set; } = null;
    public Panel SourcePanel { get; set; } = null;
    public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    public List<string> EnabledButtons { get; set; } = new List<string>();
    public TextBox report { get; set; } = null;

    public enum Prefix {
      Empty,
      Master,
      Numeric,
      Encrypted,
      NotEmpty,
      DisplayName,
      Currency,
      DateTime,
      NoDisplay,
      NoSaveEmpty,
      Disable,
      Class,
      LoadClassProperties,
      Filename,
      Property,
      DirectoryPath,
      DefaultValue,
      Ignore,
      NumericDefault
    }

    public DataPanel(Panel SourcePanel) {
      this.SourcePanel = SourcePanel;
    }

    public DataPanel(Panel SourcePanel, object Source) {
      this.SourcePanel = SourcePanel;
      this.Source = Source;
    }

    public void ClearForm(bool resetSelector = true) {
      foreach (Control ctrl in SourcePanel.Controls) {
        if (IsSelector(ctrl)) continue;
        if (ctrl is Label) continue;

        Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
        if (prefixes.ContainsKey(Prefix.Ignore)) continue;

        bool cb = ctrl is ComboBox;
        bool lb = ctrl is ListBox;
        bool tb = ctrl is TextBox;
        bool ck = ctrl is CheckBox;
        bool bgcolored = cb || lb || tb;
        bool multiitem = cb || lb;
        ctrl.Enabled = false;
        if (tb) ctrl.Text = "";
        if (lb) ((ListBox)ctrl).Items.Clear();
        if (ck) ((CheckBox)ctrl).Checked = false;
        if (cb) { ((ComboBox)ctrl).Items.Clear(); ((ComboBox)ctrl).SelectedIndex = -1; }
        if (bgcolored) ctrl.BackColor = Color.Gainsboro;
        if (multiitem && Parameters.ContainsKey("items." + ctrl.Name)) {
          foreach (string item in (List<string>)Parameters["items." + ctrl.Name]) {
            if (cb) ((ComboBox)ctrl).Items.Add(item);
            if (lb) ((ListBox)ctrl).Items.Add(item);
          }
        }
      }
      if (resetSelector) ResetSelector();
    }

    public void NewEntry() {
      foreach (Control ctrl in SourcePanel.Controls) {
        if (IsSelector(ctrl)) continue;

        Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
        ctrl.Enabled = prefixes.ContainsKey(Prefix.Disable) ? prefixes[Prefix.Disable].IndexOf("new") == -1 : true;
        if (ctrl is TextBox || ctrl is ComboBox || ctrl is ListBox) ctrl.BackColor = Color.Ivory;
      }
    }

    public void LoadForm(object source = null) {
      if (source == null && Source == null) return;
      if (source != null) Source = source;
      if (Source == null) Source = source;

      foreach (Control ctrl in SourcePanel.Controls) {
        if (IsSelector(ctrl)) continue;

        Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
        if (prefixes.ContainsKey(Prefix.Ignore)) continue;

        if (ctrl is TextBox || ctrl is ComboBox || ctrl is ListBox) ctrl.BackColor = Color.Ivory;
        ctrl.Enabled = prefixes.ContainsKey(Prefix.Disable) ? prefixes[Prefix.Disable].IndexOf("load") == -1 : true;

        LoadItems(ctrl);
        SetDefault(ctrl);
        if (prefixes.ContainsKey(Prefix.NoDisplay)) continue;
        if (ctrl.Tag == null) continue;

        SetValue(ctrl, GetValue(ctrl));
      }
    }

    public bool Validate() {
      foreach (Control ctrl in SourcePanel.Controls) {
        if (IsSelector(ctrl)) continue;

        Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
        if (prefixes.Count == 0) continue;
        if (prefixes.ContainsKey(Prefix.Ignore)) continue;

        string fieldName = prefixes.ContainsKey(Prefix.DisplayName) ? prefixes[Prefix.DisplayName] : prefixes.ContainsKey(Prefix.Property) ? prefixes[Prefix.Property] : ctrl.Name.SplitOnCaps();
        if (prefixes.ContainsKey(Prefix.NotEmpty)) {
          if ((ctrl is ListBox && ((ListBox)ctrl).SelectedIndices.Count == 0) ||
             ((ctrl is ComboBox || ctrl is TextBox) && ctrl.Text.Trim() == "")) {
            new Dialog(fieldName + " needs a value!").Show();
            return false;
          }
        }
        if (ctrl is TextBox && prefixes.ContainsKey(Prefix.Numeric)) {
          if (!IsNumeric(ctrl.Text.Trim() == "" ? (prefixes[Prefix.Numeric] == "" ? ctrl.Text : prefixes[Prefix.Numeric]) : ctrl.Text.Trim(), "p")) {
            new Dialog(fieldName + " must be a numeric value!").Show();
            return false;
          }
        }
        if (prefixes.ContainsKey(Prefix.Filename)) {
          if (ctrl.Text.Trim() == "") continue;
          if (File.Exists(ctrl.Text.Trim())) {
            new Dialog("The file specified in " + fieldName + " does not exist!").Show();
            return false;
          }
        }
      }
      return true;
    }

    public object Save() {
      foreach (Control ctrl in SourcePanel.Controls) {
        if (IsSelector(ctrl)) continue;

        Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
        if (!prefixes.ContainsKey(Prefix.Property)) continue;
        if (ctrl.Text.Trim() == "" && prefixes.ContainsKey(Prefix.NoSaveEmpty)) continue;
        if (prefixes.ContainsKey(Prefix.Ignore)) continue;

        string property = prefixes[Prefix.Property];
        if (ctrl is CheckBox) {
          SetProperty(Source, property, ((CheckBox)ctrl).Checked ? "true" : "false");
        } else {
          if (prefixes.ContainsKey(Prefix.Numeric)) {
            if (prefixes[Prefix.Numeric] != "" && ctrl.Text.Trim() == "") ctrl.Text = prefixes[Prefix.Numeric];
          }
          SetProperty(Source, property, ctrl.Text);
        }
      }
      return Source;
    }

    public void SetValue(Control ctrl, string value) {
      value = value.Trim();
      if (ctrl is TextBox) ctrl.Text = value.Trim() == "" ? (Parameters.ContainsKey("default." + ctrl.Name) ? Parameters["default." + ctrl.Name].ToString() : "") : value;
      if (ctrl is CheckBox) ((CheckBox)ctrl).Checked = value != "";
      if (ctrl is ComboBox) ((ComboBox)ctrl).SelectedIndex = ((ComboBox)ctrl).FindStringExact(value);
      if (ctrl is CheckBox) ((CheckBox)ctrl).Checked = GetBool(value);
      if (ctrl is ListBox) {
        foreach (string val in value.Split(',')) {
        }
      }
    }

    public string GetValue(Control ctrl) {
      if (ctrl == null || ctrl.Tag == null) return "";

      bool isEncrypted = false;
      foreach (string tagproperty in ctrl.Tag.ToString().Split(',')) {
        Prefix pfx = GetPrefixType(tagproperty);
        if (pfx == Prefix.Encrypted) isEncrypted = true;
        if (pfx != Prefix.Property) continue;

        object objvalue = GetProperty(Source, tagproperty);
        if (objvalue == null) continue;

        return isEncrypted ? DecryptString(objvalue.ToString()) : objvalue.ToString();
      }
      return "";
    }

    public void SetButton(bool status, params string[] buttons) {
      EnabledButtons.Clear();
      foreach (string button in buttons) EnabledButtons.Add(button.ToLower());
    }

    public Dictionary<Prefix, string> GetPrefixTypes(Control ctrl) {
      Dictionary<Prefix, string> ret = new Dictionary<Prefix, string>();
      if (ctrl == null || ctrl.Tag == null) return ret;

      foreach (string property in ctrl.Tag.ToString().Split(',')) {
        Prefix tmp = GetPrefixType(property);
        if (tmp == Prefix.Empty) continue;

        switch (tmp) {
        case Prefix.Property:
          ret.Add(tmp, property);
          break;

        case Prefix.Class:
          if (property.Trim().Length == 1) {
            new Dialog("** Corrupt control tag for " + ctrl.Name).Show();
            return new Dictionary<Prefix, string>();
          }

          if (property.Substring(1, 1) == "*") {
            if (property.Trim().Length == 2) {
              new Dialog("** Corrupt control tag for " + ctrl.Name).Show();
              return new Dictionary<Prefix, string>();
            }

            ret.Add(Prefix.LoadClassProperties, "");
            ret.Add(tmp, property.Substring(2, property.Length - 2));
            continue;
          }

          ret.Add(tmp, property.Substring(1, property.Length - 1));
          break;

        case Prefix.DisplayName:
          if (property.Trim().Length == 1) {
            new Dialog("** Corrupt control tag for " + ctrl.Name).Show();
            return new Dictionary<Prefix, string>();
          }

          ret.Add(tmp, property.Substring(1, property.Length - 1));
          break;

        case Prefix.Disable:
          if (property.Trim().Length == 1) {
            new Dialog("** Corrupt control tag for " + ctrl.Name).Show();
            return new Dictionary<Prefix, string>();
          }

          ret.Add(tmp, property.Substring(1, property.Length - 1));
          break;

        case Prefix.DirectoryPath:
          ret.Add(tmp, property.Trim().Length == 1 ? "" : property.Substring(1, property.Length - 1));
          break;

        case Prefix.Numeric:
          ret.Add(tmp, property.Trim().Length == 1 ? "" : property.Trim().Substring(1, property.Trim().Length - 1));
          break;

        default:
          ret.Add(tmp, "");
          break;
        }
      }
      return ret;
    }

    public Prefix GetPrefixType(string field) {
      if (field.Trim() == "") return Prefix.Empty;
      switch (field.Trim().Substring(0, 1)) {
      case "*":
        return Prefix.Master;
      case "#":
        return Prefix.Numeric;
      case "%":
        return Prefix.Encrypted;
      case "$":
        return Prefix.Currency;
      case "^":
        return Prefix.DateTime;
      case "_":
        return Prefix.DisplayName;
      case "!":
        return Prefix.NotEmpty;
      case "(":
        return Prefix.NoDisplay;
      case ")":
        return Prefix.Class;
      case "-":
        return Prefix.Disable;
      case "&":
        return Prefix.NoSaveEmpty;
      case "=":
        return Prefix.DirectoryPath;
      case "@":
        return Prefix.Filename;
      case "~":
        return Prefix.Ignore;
      default:
        return Prefix.Property;
      }
    }

    public void ToggleButtons(bool enabling = true) {
      foreach (Control ctrl in SourcePanel.Controls) {
        if (!(ctrl is Button)) continue;

      }
    }

    public void SetDefault(Control ctrl) {
      if (Parameters.ContainsKey("default." + ctrl.Name)) {
        if (ctrl is TextBox) ctrl.Text = Parameters["default." + ctrl.Name].ToString();
        if (ctrl is ComboBox) ((ComboBox)ctrl).SelectedIndex = ((ComboBox)ctrl).FindStringExact(Parameters["default." + ctrl.Name].ToString());
        if (ctrl is CheckBox) ((CheckBox)ctrl).Checked = true;
        if (ctrl is ListBox) {
          foreach (string lbdefault in (List<string>)Parameters["default." + ctrl.Name]) {
            ((ListBox)ctrl).SetSelected(((ListBox)ctrl).FindStringExact(Parameters["default." + ctrl.Name].ToString()), true);
          }
        }
      }
    }

    public void LoadItems(Control ctrl) {
      if (Parameters.ContainsKey("items." + ctrl.Name)) ((ComboBox)ctrl).Items.Clear();
      foreach (string key in new string[] { "addfirst.", "items.", "addlast." }) {
        if (!Parameters.ContainsKey(key + ctrl.Name)) continue;

        if (Parameters[key + ctrl.Name] is List<string>) {
          foreach (string item in (List<string>)Parameters[key + ctrl.Name]) AddToControl(ctrl, item);
          continue;
        }

        AddToControl(ctrl, Parameters[key + ctrl.Name]);
      }

      Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
      if (prefixes.ContainsKey(Prefix.LoadClassProperties) && prefixes.ContainsKey(Prefix.Class)) {
        foreach (KeyValuePair<string, int> vals in GetClassValues("GameManager", prefixes[Prefix.Class])) {
          AddToControl(ctrl, vals.Key);
        }
      }
    }

    public void AddToControl(Control ctrl, object value) {
      if (ctrl is ComboBox) ((ComboBox)ctrl).Items.Add(value.ToString());
      if (ctrl is ListBox) ((ListBox)ctrl).Items.Add(value.ToString());
    }

    public bool IsSelector(Control ctrl) {
      Dictionary<Prefix, string> prefixes = GetPrefixTypes(ctrl);
      return prefixes.ContainsKey(Prefix.Master);
    }

    public void ResetSelector() {
      foreach (Control ctrl in SourcePanel.Controls) {
        if (!IsSelector(ctrl)) continue;
        if (ctrl is ComboBox) ((ComboBox)ctrl).SelectedIndex = -1;
        if (ctrl is ListBox) ((ListBox)ctrl).SelectedIndices.Clear();
      }
    }

    public Dictionary<TextBox, Control> GetDirectoryPaths() {
      Dictionary<TextBox, Control> ret = new Dictionary<TextBox, Control>();
      foreach (Control ctrl in SourcePanel.Controls) {
        if (ctrl.Tag == null) continue;
        if (!(ctrl is TextBox)) continue;

        foreach (string tagproperty in ctrl.Tag.ToString().Split(',')) {
          Dictionary<Prefix, string> pfx = GetPrefixTypes(ctrl);
          if (!pfx.ContainsKey(Prefix.DirectoryPath)) continue;

          Control target = pfx[Prefix.DirectoryPath] == "" ? null : SourcePanel.Controls.Find(pfx[Prefix.DirectoryPath], true).FirstOrDefault();
          ret.Add((TextBox)ctrl, target);
          break;
        }
      }
      return ret;
    }
  }
}
