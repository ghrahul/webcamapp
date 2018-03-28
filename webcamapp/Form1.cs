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
        private VideoCaptureDevice FinalVideo1;
        int stop = 0; int j = 1;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
                comboBox2.Items.Add(VideoCaptureDevice.Name);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            textBox1.Visible = false;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            //CAMERA 1
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
            //CAMERA 2
            FinalVideo1 = new VideoCaptureDevice(VideoCaptureDevices[comboBox2.SelectedIndex].MonikerString);
            FinalVideo1.NewFrame += new NewFrameEventHandler(FinalVideo1_NewFrame);
            FinalVideo1.Start();
            await System.Threading.Tasks.Task.Delay(5000);
            stop = 0;
            autoClick1();//first page
            textBox1.Visible = false;
            await System.Threading.Tasks.Task.Delay(2000);
            autoClick();//camera auto
        }
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            eventArgs.Frame.RotateFlip(RotateFlipType.Rotate180FlipY);
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = video;
        }
        void FinalVideo1_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            eventArgs.Frame.RotateFlip(RotateFlipType.Rotate180FlipY);
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox2.Image = video;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (FinalVideo.IsRunning == true)
                FinalVideo.SignalToStop();
            if (FinalVideo1.IsRunning == true)
                FinalVideo1.SignalToStop();
            stop = 1;
            autoClick2();//last page
        }
        private async void autoClick()
        {
           
            while (stop==0)
            {
                textBox1.Visible = true;
                pictureBox1.Image.Save(@"F:\picture\" + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                j++;
                textBox1.Visible = true;
                pictureBox2.Image.Save(@"F:\picture\" + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                j++;
                textBox1.Visible = false;
                await System.Threading.Tasks.Task.Delay(2000);
            }
        }  
        private void autoClick1()//first_page
        {
            textBox1.Visible = true;
            pictureBox2.Image.Save(@"F:\picture\" + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            j++;
        }
        private void autoClick2()//last_page
        {
            textBox1.Visible = true;
            pictureBox1.Image.Save(@"F:\picture\" + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            j++;
        }
    }
}
