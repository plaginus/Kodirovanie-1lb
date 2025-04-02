using System.Drawing;

namespace LabaKod1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private PixelYUV[,] YUVmap;
        private PixelYUV[,] newYUVmap;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                pictureBox1.ImageLocation = filePath;
                button2.Enabled = true;

                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RGBToYUV();
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            YImage();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UImage();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            VImage();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RGBImage();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Image_4_2_0();
            RMSEEstimation();
        }


        /// <summary>
        /// 
        /// </summary>
        private void RGBToYUV()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            PixelYUV[,] YUVmap = new PixelYUV[bitmap.Width, bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);

                    YUVmap[x, y].Y = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.144 * pixel.B);
                    YUVmap[x, y].U = (int)(-0.14713 * pixel.R - 0.28886 * pixel.G + 0.436 * pixel.B + 128);
                    YUVmap[x, y].V = (int)(0.615 * pixel.R - 0.51499 * pixel.G - 0.10001 * pixel.B + 128);
                }
            }
            this.YUVmap = YUVmap;
        }

        private void YImage()
        {
            int W = pictureBox1.Image.Width;
            int H = pictureBox1.Image.Height;

            Bitmap bitmap = new Bitmap(W, H);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte color = YUVmap[x, y].Aligning(YUVmap[x, y].Y);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                }
            }
            pictureBox2.Image = bitmap;
        }

        private void UImage()
        {
            int W = pictureBox1.Image.Width;
            int H = pictureBox1.Image.Height;

            Bitmap bitmap = new Bitmap(W, H);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte color = YUVmap[x, y].Aligning(YUVmap[x, y].U);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                }
            }
            pictureBox2.Image = bitmap;
        }

        private void VImage()
        {
            int W = pictureBox1.Image.Width;
            int H = pictureBox1.Image.Height;

            Bitmap bitmap = new Bitmap(W, H);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte color = YUVmap[x, y].Aligning(YUVmap[x, y].V);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                }
            }
            pictureBox2.Image = bitmap;
        }

        private void RGBImage() 
        {
            int W = pictureBox1.Image.Width;
            int H = pictureBox1.Image.Height;

            Bitmap bitmap = new Bitmap(W, H);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(255, YUVmap[x, y].ToR(), YUVmap[x, y].ToG(), YUVmap[x, y].ToB()));
                }
            }
            pictureBox2.Image = bitmap;
        }

        private void Image_4_2_0()
        {
            int W = pictureBox1.Image.Width;
            int H = pictureBox1.Image.Height;

            Bitmap bitmap = new Bitmap(W, H);
            PixelYUV[,] result = new PixelYUV[W, H];

            for (int x = 1; x < bitmap.Width; x += 2)
            {
                for (int y = 1; y < bitmap.Height; y += 2)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(255, YUVmap[x, y].ToR(), YUVmap[x, y].ToG(), YUVmap[x, y].ToB()));
                    result[x, y].Y = YUVmap[x, y].Y;
                    result[x, y].U = YUVmap[x, y].U;
                    result[x, y].V = YUVmap[x, y].V;

                    bitmap.SetPixel(x, y - 1, Color.FromArgb(255, YUVmap[x, y].ToR(), YUVmap[x, y].ToG(), YUVmap[x, y].ToB()));
                    result[x, y-1].Y = YUVmap[x, y-1].Y;
                    result[x, y - 1].U = YUVmap[x, y].U;
                    result[x, y - 1].V = YUVmap[x, y].V;

                    bitmap.SetPixel(x - 1, y, Color.FromArgb(255, YUVmap[x, y].ToR(), YUVmap[x, y].ToG(), YUVmap[x, y].ToB()));
                    result[x-1, y].Y = YUVmap[x-1, y].Y;
                    result[x - 1, y].U = YUVmap[x, y].U;
                    result[x - 1, y].V = YUVmap[x, y].V;

                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(255, YUVmap[x, y].ToR(), YUVmap[x, y].ToG(), YUVmap[x, y].ToB()));
                    result[x-1, y-1].Y = YUVmap[x-1, y-1].Y;
                    result[x - 1, y - 1].U = YUVmap[x, y].U;
                    result[x - 1, y - 1].V = YUVmap[x, y].V;
                }
            }
            newYUVmap = result;
            pictureBox2.Image = bitmap;
        }

        private void RMSEEstimation()
        {
            int W = pictureBox1.Image.Width;
            int H = pictureBox1.Image.Height;

            double k = 1.0 / (W * H);
            double sum = 0;

            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    sum += Math.Sqrt(Math.Pow(YUVmap[x, y].ToR() - newYUVmap[x, y].ToR(), 2) + Math.Pow(YUVmap[x, y].ToG() - newYUVmap[x, y].ToG(), 2) + Math.Pow(YUVmap[x, y].ToB() - newYUVmap[x, y].ToB(), 2));
                }
            }
            textBox1.Text = $"СКО RGB сигналов: {(k * sum):f1}";
        }
    }
}
