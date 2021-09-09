using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeebViewer
{
    public partial class BeebPalette : UserControl
    {
        private const int MAXCOLOURS = 16;

        public event EventHandler ColourChanged;

        /// <summary>
        ///
        /// </summary>
        public enum Colour
        {
            Black = 0,
            Red = 1,
            Green = 2,
            Yellow = 3,
            Blue = 4,
            Magenta = 5,
            Cyan = 6,
            White = 7
        }

        /// <summary>
        ///
        /// </summary>
        private int numColours;

        /// <summary>
        ///
        /// </summary>
        ///
        [DefaultValue(16)]
        public int NumColours
        {
            get
            {
                return numColours;
            }
            set
            {
                numColours = value;
                SetColourMode();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Colour[] Palette;

        /// <summary>
        ///
        /// </summary>
        public BeebPalette()
        {
            InitializeComponent();

            numColours = MAXCOLOURS;

            // Default max 16 colours
            Palette = new Colour[MAXCOLOURS];

            SetColourMode();
        }

        /// <summary>
        ///
        /// </summary>
        public void SetColourMode()
        {
            for (int i = 0; i < MAXCOLOURS; i++) Controls[i].Show();

            switch (numColours)
            {
                case 16:
                    Palette[0] = Colour.Black;
                    Palette[1] = Colour.Red;
                    Palette[2] = Colour.Green;
                    Palette[3] = Colour.Yellow;
                    Palette[4] = Colour.Blue;
                    Palette[5] = Colour.Magenta;
                    Palette[6] = Colour.Cyan;
                    Palette[7] = Colour.White;
                    Palette[8] = Colour.Black;
                    Palette[9] = Colour.Red;
                    Palette[10] = Colour.Green;
                    Palette[11] = Colour.Yellow;
                    Palette[12] = Colour.Blue;
                    Palette[13] = Colour.Magenta;
                    Palette[14] = Colour.Cyan;
                    Palette[15] = Colour.White;
                    break;

                case 4:
                    Palette[0] = Colour.Black;
                    Palette[1] = Colour.Red;
                    Palette[2] = Colour.Yellow;
                    Palette[3] = Colour.White;

                    for (int i = 4; i < MAXCOLOURS; i++)
                        Controls[i].Hide();

                    break;

                case 2:
                    Palette[0] = Colour.Black;
                    Palette[1] = Colour.White;

                    for (int i = 2; i < MAXCOLOURS; i++)
                        Controls[i].Hide();

                    break;

                default: throw new Exception("Unknown number of colour");
            }

            for (int i = 0; i < numColours; i++)
            {
                Controls[i].BackColor = GetWindowsColour(Palette[i]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="beebColour"></param>
        /// <returns></returns>
        public static Color GetWindowsColour(Colour beebColour)
        {
            switch (beebColour)
            {
                case Colour.Black: return Color.FromArgb(0, 0, 0);
                case Colour.Red: return Color.FromArgb(255, 0, 0);
                case Colour.Green: return Color.FromArgb(0, 255, 0);
                case Colour.Yellow: return Color.FromArgb(255, 255, 0);
                case Colour.Blue: return Color.FromArgb(0, 0, 255);
                case Colour.Magenta: return Color.FromArgb(255, 0, 255);
                case Colour.Cyan: return Color.FromArgb(0, 255, 255);
                case Colour.White: return Color.FromArgb(255, 255, 255);
                default: throw new Exception("Unknown colour");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Color[] GetPalette()
        {
            Color[] pal = new Color[MAXCOLOURS];

            for (int i = 0; i < numColours; i++)
            {
                pal[i] = GetWindowsColour(Palette[i]);
            }

            return pal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Colour GetNextColour(Colour colour)
        {
            return (Colour)(((int)colour + 1) & 7);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Colour GetPreviousColour(Colour colour)
        {
            return (Colour)(((int)colour - 1) & 7);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        private void ChangePalette(int index, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:

                    Palette[index] = GetNextColour(Palette[index]);
                    break;

                case MouseButtons.Right:

                    Palette[index] = GetPreviousColour(Palette[index]);
                    break;
            }

            Controls[index].BackColor = GetWindowsColour(Palette[index]);
            Controls[index].Invalidate();

            ColourChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button0_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(0, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(1, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(2, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(3, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(4, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(5, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(6, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(7, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(8, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(9, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(10, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(11, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(12, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(13, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(14, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette(15, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            SetColourMode();
            Invalidate();
            ColourChanged?.Invoke(this, new EventArgs());
        }
    }
}