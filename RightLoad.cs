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
    public partial class RightLoad : Form
    {
        byte chnl_Num;
        bool pinButton = false;
        byte formCloseCounter = 0;
        CommunicationFile Comm = new CommunicationFile();
        public double[,] disPlayMat2 = new double[15, 5];
        private static System.Timers.Timer dataTimer;
        public RightLoad()
        {
            InitializeComponent();
            this.Opacity = .10;
            dataTimer = new System.Timers.Timer(100);
            dataTimer.Elapsed += displayGauge;
            dataTimer.Enabled = true;
            this.CenterToParent();
            this.TopMost = true;
            this.TopLevel = true;
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        delegate void setStat_Update(double[,] displayMat2, byte chnl_Num);

        public void status_Update(double[,] disPlayMat2, byte chnl_Num)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new setStat_Update(status_Update), disPlayMat2, chnl_Num);
                }
                catch (System.ObjectDisposedException)
                {
                }
            }
            else
            {
                On_OffBox.Text = Convert.ToString(disPlayMat2[chnl_Num, 3]);
                tripBox.Text = Convert.ToString(disPlayMat2[chnl_Num, 4]);
                ChnlBox.Text = Convert.ToString(chnl_Num);

                if (disPlayMat2[chnl_Num, 3] != 0)
                {
                    Channel_Switch.Text = "Turn Off Channel";
                    Channel_Switch.BackColor = Color.Blue;
                }
                else
                {
                    if (disPlayMat2[chnl_Num, 4] == 1)
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
                    formCloseCounter++;
                }
                else
                {
                    formCloseCounter = 0;
                }

                if ((formCloseCounter == 50) && (pinButton == false))
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
                status_Update(Main.dataMatrix2, chnl_Num);
                arcScaleNeedleComponent1.ArcScale.Value = Convert.ToSingle(Main.dataMatrix2[chnl_Num, 1]);
                arcScaleNeedleComponent2.ArcScale.Value = Convert.ToSingle(Main.dataMatrix2[chnl_Num, 2]);
            }
            
            catch (System.NullReferenceException)
            {
            }
            catch(System.OverflowException)
            {
            }


    }

        private void Channel_Switch_Click(object sender, EventArgs e)
        {
            if ((Main.dataMatrix2[chnl_Num, 3] == 0) && (Main.dataMatrix2[chnl_Num, 4] != 1))
            {
                Comm.rightChannel_sw(chnl_Num, true);
            }

            else
            {
                Comm.rightChannel_sw(chnl_Num, false);
            }
        }

        private void pin_button_Click(object sender, EventArgs e)
        {
            pinButton = true;
            this.TopLevel = true;
            this.TopMost = true;
        }

        private void RightLoad_Load(object sender, EventArgs e)
        {

        }
    }
}