using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using BarcodeLib.BarcodeReader;

namespace UsarCamera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private FilterInfoCollection Dispositivos;

        private VideoCaptureDevice Video;

        private void Form1_Load(object sender, EventArgs e)
        {
            // lista dispositio com entrada e video

            Dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach(FilterInfo x in Dispositivos)
            {
                comboBox1.Items.Add(x.Name);
            }

            comboBox1.SelectedIndex = 0; 
        }

        private void leitura_Click(object sender, EventArgs e)
        {

            timer1.Enabled = true;
            Video = new VideoCaptureDevice(Dispositivos[comboBox1.SelectedIndex].MonikerString);
            videoSourcePlayer1.VideoSource = Video;
            videoSourcePlayer1.Start();

        }

        private void parar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            videoSourcePlayer1.SignalToStop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (videoSourcePlayer1.GetCurrentVideoFrame() != null)
            {
                Bitmap img = new Bitmap(videoSourcePlayer1.GetCurrentVideoFrame());
                string[] resultado = BarcodeReader.read(img, BarcodeReader.QRCODE);
                img.Dispose();

                if(resultado!= null && resultado.Count() > 0)
                {
                    listBox1.Items.Add(resultado[0]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.listBox1.DataSource = null;
        }
    }
}
