using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.System;

namespace BorderlessGaming.Forms
{
    public partial class Rainway : Form
    {
        public Rainway()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
        }

        private void Rainway_Load(object sender, EventArgs e)
        {

        }

        private void Rainway_Click(object sender, EventArgs e)
        {
            Tools.GotoSite("https://rainway.io/?ref=borderlessgaming2");
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox) sender;
            Config.Instance.AppSettings.ShowAdOnStart = checkbox.Checked != true;
        }

        private void Rainway_MouseEnter(object sender, EventArgs e)
        {
           Cursor = Cursors.Hand;
        }

        private void Rainway_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Instance.AppSettings.ShowAdOnStart = false;
            Config.Save();
        }
    }
}
