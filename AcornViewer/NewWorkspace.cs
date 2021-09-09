using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BeebViewer
{
    public partial class NewWorkspaceForm : Form
    {
        public string Workspace { get; set; }

        /// <summary>
        ///
        /// </summary>
        public NewWorkspaceForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Workspace = comboBox1.SelectedItem.ToString();
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Workspace = string.Empty;
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mList"></param>
        public new void Load(List<Machine> mList)
        {
            // Get all machine types
            List<string> distinctList = mList.Select(x => x.MachineType).Distinct().ToList();

            // populate combobox with machine types
            foreach (var itm in distinctList)
            {
                comboBox1.Items.Add(itm);
            }

            comboBox1.SelectedIndex = 0;
        }
    }
}