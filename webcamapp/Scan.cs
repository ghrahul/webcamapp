using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace webcamapp
{
    public partial class Scan : Form
    {
        public string path;
        public Scan()
        {
            InitializeComponent();
            button1.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            label1.Visible = false;
            comboBox3.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
        }

        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private VideoCaptureDevice FinalVideo1;
        int stop = 0; int j = 1;
        int interval=2000;

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

            //CAMERA 1
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);

            //CAMERA 2
            FinalVideo1 = new VideoCaptureDevice(VideoCaptureDevices[comboBox2.SelectedIndex].MonikerString);

            for (int i = 0; i < FinalVideo.VideoCapabilities.Length; i++)
            {
                string resolution_size = FinalVideo.VideoCapabilities[i].FrameSize.ToString();
                comboBox4.Items.Add(resolution_size);
            }
            //CAMERA 1

            for (int i = 0; i < FinalVideo1.VideoCapabilities.Length; i++)
            {
                string resolution_size = FinalVideo1.VideoCapabilities[i].FrameSize.ToString();
                comboBox5.Items.Add(resolution_size);
            }
            //CAMERA 2
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            await System.Threading.Tasks.Task.Delay(5000);
            stop = 0;
            autoClick1();//first page
            textBox1.Visible = false;
            await System.Threading.Tasks.Task.Delay(interval);
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
            stop = 1;
            autoClick2();//last page
        }
        private async void autoClick()
        {
            Console.WriteLine("PATH: " + path);
            while (stop==0)
            {
                textBox1.Visible = true;
                pictureBox1.Image.Save(path + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                j++;
                textBox1.Visible = true;
                pictureBox2.Image.Save(path + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                j++;
                textBox1.Visible = false;
                await System.Threading.Tasks.Task.Delay(interval);
            }
        }  
        private void autoClick1()//first_page
        {
            textBox1.Visible = true;
            pictureBox2.Image.Save(path + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            j++;
        }
        private void autoClick2()//last_page
        {
            textBox1.Visible = true;
            pictureBox1.Image.Save(path + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            j++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            pictureBox1.Image.Save(path + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //Console.WriteLine("PATH: " + path);
            j++;
            pictureBox2.Image.Save(path + j.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            j++;
            textBox1.Visible = false;
        }
        private void radioButton1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button3.Visible = false;
            button4.Visible = true;
            label1.Visible = false;
            comboBox3.Visible = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button3.Visible = true ;
            button4.Visible = false;
            label1.Visible = true;
            comboBox3.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath + "\\";
                label3.Text = path;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int x;
            String S = comboBox3.Text;
            int.TryParse(S,out x);
            interval = x * 1000;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {          
            FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[comboBox4.SelectedIndex];
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            label9.Visible = false;
            FinalVideo.Start();
            comboBox4.Visible = false;
            label10.Text = comboBox4.SelectedItem.ToString();
            label10.Visible = true;
            if(label10.Visible == true && label11.Visible == true)
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
            }
            
        }//CAMERA 1

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            FinalVideo1.VideoResolution = FinalVideo1.VideoCapabilities[comboBox5.SelectedIndex];
            FinalVideo1.NewFrame += new NewFrameEventHandler(FinalVideo1_NewFrame);
            label8.Visible = false;
            FinalVideo1.Start();
            comboBox5.Visible = false;
            label11.Text = comboBox5.SelectedItem.ToString();
            label11.Visible = true;
            if (label10.Visible == true && label11.Visible == true)
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
            }
        }//CAMERA 2

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            
            if (FinalVideo.IsRunning == true)
                FinalVideo.SignalToStop();
            if (FinalVideo1.IsRunning == true)
                FinalVideo1.SignalToStop();
            
            

        }
    }
}
