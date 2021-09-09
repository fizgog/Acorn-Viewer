using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeebViewer
{
    /// <summary>
    /// TODO
    /// </summary>
    public partial class AtomPalette : UserControl
    {
        private const int MAXCOLOURS = 4;

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
            White = 7,
            Orange = 8
        }

        /// <summary>
        ///
        /// </summary>
        private int numColours;

        /// <summary>
        ///
        /// </summary>
        ///
        [DefaultValue(MAXCOLOURS)]
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
        public Colour[] Palette1;

        /// <summary>
        ///
        /// </summary>
        public Colour[] Palette2;

        /// <summary>
        ///
        /// </summary>
        public AtomPalette()
        {
            InitializeComponent();

            numColours = MAXCOLOURS;

            // Colour Set 1
            Palette1 = new Colour[MAXCOLOURS];

            // Colour Set 2
            Palette2 = new Colour[MAXCOLOURS];

            SetColourMode();
        }

        /// <summary>
        ///
        /// </summary>
        public void SetColourMode()
        {
            for (int i = 0; i < MAXCOLOURS; i++)
            {
                Controls[i].Show();     // Colour Set 1
                Controls[i + 4].Show();   // Colour Set 2
            }

            switch (numColours)
            {
                case 4:
                    // Colour Set 1
                    Palette1[0] = Colour.Green;
                    Palette1[1] = Colour.Yellow;
                    Palette1[2] = Colour.Blue;
                    Palette1[3] = Colour.Red;
                    // Colour Set 2
                    Palette2[0] = Colour.White;
                    Palette2[1] = Colour.Cyan;
                    Palette2[2] = Colour.Magenta;
                    Palette2[3] = Colour.Orange;
                    break;

                case 2:
                    // Colour Set 1
                    Palette1[0] = Colour.Black;
                    Palette1[1] = Colour.Green;
                    // Colour Set 2
                    Palette2[0] = Colour.Green;
                    Palette2[1] = Colour.Yellow;

                    for (int i = 2; i < MAXCOLOURS; i++)
                    {
                        Controls[i].Hide();
                        Controls[i + 4].Hide();
                    }

                    break;

                default: throw new Exception("Unknown number of colours");
            }

            for (int i = 0; i < numColours; i++)
            {
                Controls[i].BackColor = GetWindowsColour(Palette1[i]);
                Controls[i + 4].BackColor = GetWindowsColour(Palette2[i]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="atomColour"></param>
        /// <returns></returns>
        public static Color GetWindowsColour(Colour atomColour)
        {
            switch (atomColour)
            {
                case Colour.Black: return Color.FromArgb(0, 0, 0);
                case Colour.Red: return Color.FromArgb(255, 0, 0);
                case Colour.Green: return Color.FromArgb(0, 255, 0);
                case Colour.Yellow: return Color.FromArgb(255, 255, 0);
                case Colour.Blue: return Color.FromArgb(0, 0, 255);
                case Colour.Magenta: return Color.FromArgb(255, 0, 255);
                case Colour.Cyan: return Color.FromArgb(0, 255, 255);
                case Colour.White: return Color.FromArgb(255, 255, 255);
                case Colour.Orange: return Color.FromArgb(255, 165, 0);

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
                if (radioButton1.Checked)
                    pal[i] = GetWindowsColour(Palette1[i]);
                else
                    pal[i] = GetWindowsColour(Palette2[i]);
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
            return (Colour)(((int)colour + 1) & 3);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Colour GetPreviousColour(Colour colour)
        {
            return (Colour)(((int)colour - 1) & 3);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        private void ChangePalette1(int index, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:

                    Palette1[index] = GetNextColour(Palette1[index]);
                    break;

                case MouseButtons.Right:

                    Palette1[index] = GetPreviousColour(Palette1[index]);
                    break;
            }

            Controls[index].BackColor = GetWindowsColour(Palette1[index]);
            Controls[index].Invalidate();

            ColourChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        private void ChangePalette2(int index, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:

                    Palette2[index] = GetNextColour(Palette2[index]);
                    break;

                case MouseButtons.Right:

                    Palette2[index] = GetPreviousColour(Palette2[index]);
                    break;
            }

            Controls[index + 4].BackColor = GetWindowsColour(Palette2[index]);
            Controls[index + 4].Invalidate();

            ColourChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button0_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette1(0, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette1(1, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette1(2, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette1(3, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette2(0, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette2(1, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette2(2, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            ChangePalette2(3, e.Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            SetColourMode();
            Invalidate();
            ColourChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            SetColourMode();
            Invalidate();
            ColourChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ColourChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ColourChanged?.Invoke(this, new EventArgs());
        }
    }
}