using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace webcamapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            comboBox1.SelectedIndex = 0;

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
            //Thread.Sleep(5000);
            await System.Threading.Tasks.Task.Delay(10000);
            autoClick();
        }
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            eventArgs.Frame.RotateFlip(RotateFlipType.Rotate180FlipY);
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = video;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (FinalVideo.IsRunning == true)
            {
                FinalVideo.SignalToStop();
            }

        }

        private async void autoClick()
        {
            int j = 1;
            while (j!=999)
            {
                
                pictureBox1.Image.Save(@"F:\picture\" + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                await System.Threading.Tasks.Task.Delay(10000);
                j++;
            }
        }
    }
}
