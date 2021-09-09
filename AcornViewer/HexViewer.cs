using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeebViewer
{
    internal class HexViewer : RichTextBox
    {
        private int addressSize;
        private int firstCharColumn;
        private int lineLength;
        private readonly char[] HexChars;
        private int bytesPerLine;

        /// <summary>
        ///
        /// </summary>
        public int BytesPerLine
        {
            get { return bytesPerLine; }
            set { bytesPerLine = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public HexViewer()
        {
            HexChars = "0123456789ABCDEF".ToCharArray();
            BytesPerLine = 16;

            this.ReadOnly = true;
            this.Font = new System.Drawing.Font("Courier New", 10);
            this.BackColor = Color.White;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        public void Load(int offset, byte[] data)
        {
            if (data == null) return;

            addressSize = 4 + 3;    // 4 + 3 spaces
            firstCharColumn = addressSize + bytesPerLine * 3 + (bytesPerLine - 1) / 8 + 2;
            lineLength = firstCharColumn + bytesPerLine + Environment.NewLine.Length;

            int bytesLength = data.Length;

            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();

            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(offset + i >> 12) & 0xF];
                line[1] = HexChars[(offset + i >> 8) & 0xF];
                line[2] = HexChars[(offset + i >> 4) & 0xF];
                line[3] = HexChars[(offset + i >> 0) & 0xF];

                int hexColumn = addressSize;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = data[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (b > 126) ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }

            this.Text = result.ToString();
        }
    }
}