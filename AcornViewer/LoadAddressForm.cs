using System;
using System.Windows.Forms;

namespace BeebViewer
{
    public partial class LoadAddressForm : Form
    {
        public string LoadAddress { get; set; }

        public LoadAddressForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty) textBox1.Text = "0000";

            LoadAddress = this.textBox1.Text;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            LoadAddress = string.Empty;
            this.Close();
        }
    }
}