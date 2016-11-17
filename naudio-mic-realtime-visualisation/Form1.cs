using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NAudio.Wave;
using NAudio;

namespace naudio_mic_realtime_visualisation
{
    public partial class Form1 : Form
    {
        int n = 4000; // number of x-axis pints
        //Stopwatch time = new Stopwatch();
        WaveIn wi;
        Queue<double> myQ;
        public Form1()
        {
            InitializeComponent();
            myQ = new Queue<double>(Enumerable.Repeat(0.0, n).ToList()); // fill myQ w/ zeros
            chart1.ChartAreas[0].AxisY.Minimum = -10000;
            chart1.ChartAreas[0].AxisY.Maximum = 10000;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            wi = new WaveIn();
            wi.StartRecording();
            wi.WaveFormat = new WaveFormat(4, 16, 1); // (44100, 16, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            timer1.Enabled = true;
            //time.Start();
        }


        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                myQ.Enqueue(BitConverter.ToInt16(e.Buffer, i));
                myQ.Dequeue();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                chart1.Series["Series1"].Points.DataBindY(myQ);
                //chart1.ResetAutoValues();
            }
            catch
            {
                Console.WriteLine("No bytes recorded");
            }
        }
    }
}