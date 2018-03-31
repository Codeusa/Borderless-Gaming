using System;
using System.Windows.Forms;

namespace BorderlessGaming.Forms
{
    public partial class InputText : Form
    {
        public InputText()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => Text.Trim();
            set => Text = value.Trim();
        }

        public string Input
        {
            get => txtInput.Text.Trim();
            set => txtInput.Text = value.Trim();
        }

        public string Instructions
        {
            get => lblInstructions.Text.Trim();
            set => lblInstructions.Text = value.Trim();
        }

        private void frmInputText_Shown(object sender, EventArgs e)
        {
            txtInput.Focus();
        }
    }
}