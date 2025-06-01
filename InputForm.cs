using System;
using System.Windows.Forms;

namespace dz3
{
    public partial class InputForm : Form
    {
        public string InputText { get; private set; }

        public InputForm()
        {
            InitializeComponent();
        }

        public InputForm(string title, string prompt, string defaultText = "") : this()
        {
            this.Text = title;
            lblPrompt.Text = prompt;
            txtInput.Text = defaultText;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            InputText = txtInput.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(sender, e);
            }
        }
    }
}