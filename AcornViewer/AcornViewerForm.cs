using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BeebViewer
{
    public partial class Form1 : Form
    {
        private int offset;
        private string workspace;
        private byte[] buffer;

        private readonly List<Machine> machineList;

        /// <summary>
        ///
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // Load json file into machine array
            using (StreamReader r = new StreamReader("Machine.json"))
            {
                string json = r.ReadToEnd();
                machineList = JsonConvert.DeserializeObject<List<Machine>>(json);
            }

            openToolStripMenuItem.Enabled = false;
            openRawFileToolStripMenuItem.Enabled = false;
            spriteViewerToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem.Enabled = false;
            openRawFileToolStripMenuItem.Enabled = false;
            spriteViewerToolStripMenuItem.Enabled = false;

            NewWorkspaceForm frm = new NewWorkspaceForm();
            frm.Load(machineList);
            frm.ShowDialog();

            if (frm.Workspace != string.Empty)
            {
                openToolStripMenuItem.Enabled = true;
                openRawFileToolStripMenuItem.Enabled = true;

                workspace = frm.Workspace;
                hexViewer1.Clear();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openRawFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Raw files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Load address
                LoadAddressForm frm = new LoadAddressForm();
                frm.ShowDialog();

                if (frm.LoadAddress != string.Empty)
                {
                    // Load data into HexViewer
                    offset = Convert.ToInt32(frm.LoadAddress, 16);

                    buffer = File.ReadAllBytes(openFileDialog1.FileName);
                    hexViewer1.Load(offset, buffer);

                    spriteViewerToolStripMenuItem.Enabled = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spriteViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buffer != null)
            {
                if (workspace == "Acorn Atom")
                {
                    // Load data into Atom SpriteViewer
                    AtomSpriteViewerForm frm = new AtomSpriteViewerForm();

                    frm.Load(GetMachineList(), offset, buffer);
                    frm.ShowDialog();
                }
                else if (workspace == "Acorn BBC")
                {
                    // Load data into BBC SpriteViewer
                    BeebSpriteViewerForm frm = new BeebSpriteViewerForm();

                    frm.Load(GetMachineList(), offset, buffer);
                    frm.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Get machine types data based from the workspace
        /// </summary>
        /// <returns></returns>
        public List<Machine> GetMachineList()
        {
            List<Machine> list = new List<Machine>();

            foreach (Machine itm in machineList)
            {
                if (itm.MachineType == workspace) list.Add(itm);
            }
            return list;
        }
    }
}