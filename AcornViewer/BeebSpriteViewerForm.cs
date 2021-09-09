using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BeebViewer
{
    public partial class BeebSpriteViewerForm : Form
    {
        private List<Machine> machineList;

        /// <summary>
        ///
        /// </summary>
        public BeebSpriteViewerForm()
        {
            InitializeComponent();

            spriteViewer1.StartAddressChanged += new EventHandler(SpriteViewer_StartAddressChanged);
            beebPalette1.ColourChanged += new EventHandler(SpriteViewer_ColourChanged);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SpriteViewer_StartAddressChanged(object sender, EventArgs e)
        {
            textBox1.Text = "0x" + ((SpriteViewer)sender).OffsetAddress.ToString("X4");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SpriteViewer_ColourChanged(object sender, EventArgs e)
        {
            spriteViewer1.palette = beebPalette1.GetPalette();
            spriteViewer1.DrawImage();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        public new void Load(List<Machine> mList, int offset, byte[] data)
        {
            machineList = mList;

            foreach (var itm in machineList)
            {
                comboBox1.Items.Add(itm.Description);
            }

            comboBox1.SelectedIndex = 0;

            textBox1.Text = "0x" + offset.ToString("X4");
            spriteViewer1.palette = beebPalette1.GetPalette();
            spriteViewer1.Load(offset, data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            spriteViewer1.Zoom = trackBar1.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = "Sprite Viewer (" + machineList[comboBox1.SelectedIndex].ShortDesc + ")";

            beebPalette1.NumColours = machineList[comboBox1.SelectedIndex].NumColours;
            spriteViewer1.palette = beebPalette1.GetPalette();
            spriteViewer1.Machine = machineList[comboBox1.SelectedIndex];
        }
    }
}