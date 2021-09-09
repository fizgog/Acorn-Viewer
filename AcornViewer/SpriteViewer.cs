using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BeebViewer
{
    public partial class SpriteViewer : ScrollableControl
    {
        public event EventHandler StartAddressChanged;

        private Machine machine;

        private byte[] m_Data;
        public Color[] palette;

        private Image m_Image;
        private Decimal m_ZoomValue = 4;

        public int StartAddress { get; set; }
        public int OffsetAddress { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int BitsPerByte { get; set; }

        /// <summary>
        ///
        /// </summary>
        public SpriteViewer()
        {
            InitializeComponent();

            //mode = 0;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            this.HorizontalScroll.SmallChange = (int)m_ZoomValue;
            this.VerticalScroll.SmallChange = (int)m_ZoomValue;
        }

        /// <summary>
        ///
        /// </summary>
        public Machine Machine
        {
            get { return machine; }
            set { machine = value; DrawImage(); }
        }

        /// <summary>
        ///
        /// </summary>
        public Decimal Zoom
        {
            get { return m_ZoomValue; }
            set
            {
                m_ZoomValue = value;

                this.HorizontalScroll.SmallChange = (int)m_ZoomValue;
                this.VerticalScroll.SmallChange = (int)m_ZoomValue;

                Invalidate();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Image Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        public void Load(int offset, byte[] data)
        {
            if (data == null) return;

            StartAddress = offset;

            m_Data = data;

            DrawImage();
        }

        /// <summary>
        ///
        /// </summary>
        public void DrawImage()
        {
            if (m_Data == null) return;

            int nx = 0;
            int ny = 0;

            int i, j, k;

            int bytes = 8;

            int width = machine.Width;
            int height = (m_Data.Length / width) * bytes;

            int columns = machine.Width / machine.BitsPerByte;
            int rows = (m_Data.Length / bytes) / columns;

            Bitmap canvas = new Bitmap(width, height);

            int address = 0;

            for (k = 0; k < rows; k++)
            {
                for (j = 0; j < columns; j++)
                {
                    for (i = 0; i < bytes; i++)
                    {
                        int pixel = m_Data[address];

                        List<int> bitsArray;

                        bitsArray = (machine.MachineType == "Acorn Atom") ? Unpack_Atom_Byte(pixel) : Unpack_BBC_Byte(pixel);

                        for (int bit = 0; bit < bitsArray.Count; bit++)
                        {
                            canvas.SetPixel(nx + bit, ny + i, GetColour(bitsArray[bit]));
                        }
                        address++;
                    }
                    nx += machine.BitsPerByte;
                }
                nx = 0;
                ny += bytes;
            }

            this.Image = canvas;

            Invalidate();
        }

        /// <summary>
        ///
        /// </summary>
        public new void Invalidate()
        {
            if (m_Image is object)
            {
                this.AutoScrollMinSize = new Size((int)(this.Image.Width * m_ZoomValue), (int)(this.Image.Height * m_ZoomValue));

                OffsetAddress = StartAddress + (int)(this.HorizontalScroll.Value / m_ZoomValue) + ((int)(this.VerticalScroll.Value / m_ZoomValue) * this.Image.Width);

                StartAddressChanged?.Invoke(this, new EventArgs());
            }

            base.Invalidate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="se"></param>
        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            if (m_Image != null)
            {
                Image img;

                img = new Bitmap(m_Image);

                int width = img.Width;
                int height = img.Height;

                int x = 0;
                int y = 0;

                if (this.AutoScroll)
                {
                    if (this.HScroll) x = this.AutoScrollPosition.X;
                    if (this.VScroll) y = this.AutoScrollPosition.Y;
                }

                var rect = new Rectangle(x, y, (int)(width * m_ZoomValue), (int)(height * m_ZoomValue));

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                e.Graphics.DrawImage(img, rect);
            }
            base.OnPaint(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="charByte"></param>
        /// <returns></returns>
        private List<int> Unpack_BBC_Byte(int charByte)
        {
            List<int> bits = new List<int>();

            // 8 bits
            if (machine.Mode == "0" || machine.Mode == "4")
            {
                bits.Add((charByte >> 7) & 0x01);
                bits.Add((charByte >> 6) & 0x01);
                bits.Add((charByte >> 5) & 0x01);
                bits.Add((charByte >> 4) & 0x01);
                bits.Add((charByte >> 3) & 0x01);
                bits.Add((charByte >> 2) & 0x01);
                bits.Add((charByte >> 1) & 0x01);
                bits.Add((charByte) & 0x01);
            }

            // 4 bits
            else if (machine.Mode == "1" || machine.Mode == "5")
            {
                bits.Add(((charByte >> 6) & 0x02) | ((charByte >> 3) & 0x01));
                bits.Add(((charByte >> 5) & 0x02) | ((charByte >> 2) & 0x01));
                bits.Add(((charByte >> 4) & 0x02) | ((charByte >> 1) & 0x01));
                bits.Add(((charByte >> 3) & 0x02) | ((charByte) & 0x01));
            }

            // 2 bitd
            else if (machine.Mode == "2")
            {
                bits.Add(((charByte >> 4) & 0x08) | ((charByte >> 3) & 0x04) | ((charByte >> 2) & 0x02) | ((charByte >> 1) & 0x01));
                bits.Add(((charByte >> 3) & 0x08) | ((charByte >> 2) & 0x04) | ((charByte >> 1) & 0x02) | (charByte & 0x01));
            }

            return bits;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="charByte"></param>
        /// <returns></returns>
        private List<int> Unpack_Atom_Byte(int charByte)
        {
            List<int> bits = new List<int>();

            if (machine.BitsPerByte == 8)
            {
                bits.Add((charByte >> 7) & 0x01);
                bits.Add((charByte >> 6) & 0x01);
                bits.Add((charByte >> 5) & 0x01);
                bits.Add((charByte >> 4) & 0x01);
                bits.Add((charByte >> 3) & 0x01);
                bits.Add((charByte >> 2) & 0x01);
                bits.Add((charByte >> 1) & 0x01);
                bits.Add((charByte) & 0x01);
            }
            else if (machine.BitsPerByte == 4)
            {
                bits.Add((charByte >> 6) & 0x03);
                bits.Add((charByte >> 4) & 0x03);
                bits.Add((charByte >> 2) & 0x03);
                bits.Add((charByte) & 0x03);
            }

            return bits;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Color GetColour(int index)
        {
            return palette[index];
        }
    }
}