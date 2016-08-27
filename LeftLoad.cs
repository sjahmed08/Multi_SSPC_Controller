using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGauges.Win.Gauges.Circular;
using System.Timers;

namespace SSPC_DemoUI
{
    public partial class LeftLoad : Form
    {
        byte chnl_Num;
       // public double[,] disPlayMat2 = new double[15, 5];
        private System.Timers.Timer dataTimer;
        private System.Timers.Timer formClosetimer;
        bool pinButton = false;
        byte closeFormCounter = 0;
        CommunicationFile Comm = new CommunicationFile();

        public LeftLoad()
        {
            
            InitializeComponent();
            dataTimer = new System.Timers.Timer(100);
            dataTimer.Elapsed += displayGauge;
            dataTimer.Enabled = true;
            this.Opacity = .99;
        //    this.FormBorderStyle = FormBorderStyle.None;
            
            formClosetimer = new System.Timers.Timer(2000);
            formClosetimer.Enabled = true;
            formClosetimer.Elapsed += timer_Tick;
            this.CenterToParent();
            this.TopMost = true;
            this.TopLevel = true;
        }         

        void timer_Tick(object sender, EventArgs e)
        {
          
        }

        delegate void setStat_Update(double[,] displayMat1, byte chnl_Num);

        public void status_Update(double[,] disPlayMat1, byte chnl_Num)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new setStat_Update(status_Update), disPlayMat1, chnl_Num);
                }
                catch (System.ObjectDisposedException)
                {
                }
            }
            else
            {
                textBox2.Text = Convert.ToString(disPlayMat1[chnl_Num, 3]);
                textBox1.Text = Convert.ToString(disPlayMat1[chnl_Num, 4]);
                textBox3.Text = Convert.ToString(chnl_Num);

                if (disPlayMat1[chnl_Num, 3] != 0)
                {
                    Channel_Switch.Text = "Turn Off Channel";
                    Channel_Switch.BackColor = Color.Blue;
                }
                else
                {
                    if (disPlayMat1[chnl_Num, 4] == 1)
                    {
                        Channel_Switch.Text = "Reset Channel";
                        Channel_Switch.BackColor = Color.Red;
                    }
                    else
                    {
                        Channel_Switch.Text = "Turn On Channel";
                        Channel_Switch.BackColor = Color.Gray;
                    }
                }

                Point pos = Control.MousePosition;
                bool inForm = pos.X >= Left && pos.Y >= Top && pos.X < Right && pos.Y < Bottom;
                this.Opacity = inForm ? 0.99 : 0.80;
                if (Opacity == .80)
                {
                    closeFormCounter++;
                }
                else
                {
                    closeFormCounter = 0;
                }

                if ((closeFormCounter == 50) && (pinButton == false))
                {
                    Close();
                }
            }
        }

        public void chnl_num(byte num)
        {
            chnl_Num = num;
        }

        private void displayGauge(Object source, ElapsedEventArgs e)
        {
            try
            {
                status_Update(Main.dataMatrix, chnl_Num);
                arcScaleNeedleComponent3.ArcScale.Value = Convert.ToSingle(Main.dataMatrix[chnl_Num, 1]);

                arcScaleNeedleComponent1.ArcScale.Value = Convert.ToSingle(Main.dataMatrix[chnl_Num, 2]);
            }

            catch (System.NullReferenceException)
            {
            }
            catch (System.OverflowException)
            {
            }


        }

        private void Channel_Switch_Click(object sender, EventArgs e)
        {
            //CommunicationFile.leftChannel_sw(chnl_num, true);
            if ((Main.dataMatrix[chnl_Num, 3] == 0 && (Main.dataMatrix[chnl_Num, 4] != 1)))
            {
                Comm.leftChannel_sw(chnl_Num, true);
            }

            else
            {              
                Comm.leftChannel_sw(chnl_Num, false);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pinButton = true;
            this.TopLevel = true;
            this.TopMost = true;
        }

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {

        }

        private void LeftLoad_Load(object sender, EventArgs e)
        {

        }

        private void gaugeControl1_Click(object sender, EventArgs e)
        {

        }

    }
}
    