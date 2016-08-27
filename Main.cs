using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGauges.Win.Gauges;
using DevExpress.XtraGauges.Win.Gauges.Linear;
using DevExpress.XtraGauges.Win.Gauges.Circular;
using DevExpress.XtraGauges.Win.Gauges.Digital;
using DevExpress.XtraGauges.Core.Model;
using System.Timers;
using LBSoft.IndustrialCtrls.Leds;


namespace SSPC_DemoUI
{
    public delegate void SetParameterValueDelegate(string value);

    public partial class Main : Form
    {
        
        CommunicationFile Comm = new CommunicationFile();
      
        public static double[,] dataMatrix = new double[15,6];
        public static double[,] boardDataMatrix = new double[1, 2];
        
        public static double[,] dataMatrix2 = new double[15, 6];
        public static double[,] boardDataMatrix2 = new double[1, 2];
        private static System.Timers.Timer dataTimer;
       
        private static bool lefGn = true;
        private static bool rightGn = true;
        private static byte eventNo = 0;

        public Main()
        {
            
            InitializeComponent();
            dataTimer = new System.Timers.Timer(250);
            dataTimer.Elapsed += matrixUpdate;
           // this.FormBorderStyle = FormBorderStyle.None;
          //  this.WindowState = FormWindowState.Maximized;
            event_sequence(0, false);
 
        }

        private void Initialize_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            Control.Enabled = true;
            SysDown.Enabled = true;
            exit_out.Enabled = true;
            PrimaryControl.Enabled = true;
            SecondaryControl.Enabled = true;
            //groupBox3.Enabled = true;
            dataTimer.Enabled = true;
            Comm.Initializing();
            event_sequence(1, false);
            
            //dataTimer.Enabled = true;
        }

        delegate void setStat_Update(double[,] dataMatrix, double[,] dataMatrix2, double[,] boardDataMatrix, double[,] boardDataMatrix2);

        private void status_Update(double[,] dataMatrix, double[,] dataMatrix2, double[,] boardDataMatrix, double[,] boardDataMatrix2)
        {
            double priCurr;
            double secCurr;
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new setStat_Update(status_Update), dataMatrix, dataMatrix2, boardDataMatrix, boardDataMatrix2);
                }
                catch (System.ObjectDisposedException)
                {
                }
            }
            else
            {
                if ((dataMatrix[2, 3] != 0) && (boardDataMatrix[0, 0] > 16))
                {
                    LeftGen.BackColor = Color.Blue;
                    LeftGen.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_on;
                }
                else
                {
                    if (dataMatrix[2, 4] != 0)
                    {
                        LeftGen.BackColor = Color.Red;
                        LeftGen.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_tripped;
                    }
                    else
                    {
                        LeftGen.BackColor = Color.Gray;
                        LeftGen.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_off;
                    }
                }

                if ((dataMatrix[3, 3] != 0) && (boardDataMatrix[0, 0] > 16)) //attach generator failure
                {
                    LftSSPC.BackColor = Color.Green;
                    if (dataMatrix[8, 3] != 0)          //8
                    {

                        L1PC.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix[8, 4] != 0)
                        {
                            L1PC.BackColor = Color.Red;
                        }
                        else
                        {
                            L1PC.BackColor = Color.Gray;
                        }
                    }

                    if (dataMatrix[9, 3] != 0)
                    {
                        L2PC.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix[9, 4] != 0)
                        {
                            L2PC.BackColor = Color.Red;
                        }
                        else
                        {
                            L2PC.BackColor = Color.Gray;
                        }
                    }



                    if (dataMatrix[10, 3] != 0)
                    {
                        L3PC.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix[10, 4] != 0)
                        {
                            L3PC.BackColor = Color.Red;
                        }
                        else
                        {
                            L3PC.BackColor = Color.Gray;
                        }
                    }

                    if (dataMatrix[11, 3] != 0)
                    {

                        L4PC.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix[11, 4] != 0)
                        {
                            L4PC.BackColor = Color.Red;
                        }
                        else
                        {
                            L4PC.BackColor = Color.Gray;
                        }
                    }

                    if (dataMatrix[5, 3] != 0)
                    {
                        L5PC.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix[5, 4] != 0)
                        {
                            L5PC.BackColor = Color.Red;
                        }
                        else
                        {
                            L5PC.BackColor = Color.Gray;
                        }
                    }
                    priCurr = dataMatrix[8, 1] + dataMatrix[9, 1] + dataMatrix[10, 1] + dataMatrix[11, 1] + dataMatrix[5, 1] + dataMatrix[0, 1];
                 
                }
                else
                {
                    priCurr = 0;
                    LftSSPC.BackColor = Color.Gray;
                    L1PC.BackColor = Color.Gray;
                    L2PC.BackColor = Color.Gray;
                    L3PC.BackColor = Color.Gray;
                    L4PC.BackColor = Color.Gray;
                    L5PC.BackColor = Color.Gray;
                }

                if ((dataMatrix2[2, 3] != 0) && (boardDataMatrix2[0, 0] > 16))
                {
                    RightGen.BackColor = Color.Blue;
                    RightGen.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_on; 
                }
                else
                {
                    if (dataMatrix2[2, 4] != 0)
                    {
                        RightGen.BackColor = Color.Red;
                        RightGen.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_tripped; 
                    }
                    else
                    {
                        RightGen.BackColor = Color.Gray;
                        RightGen.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_off; 
                    }
                }

                if ((dataMatrix2[3, 3] != 0) && (boardDataMatrix2[0, 0] > 16))
                {

                    RightSSPC.BackColor = Color.Green;              
                    if (dataMatrix2[8, 3] != 0)
                    {
          
                        RPC1.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix2[8, 4] != 0)
                        {
             
                            RPC1.BackColor = Color.Red;
                        }
                        else
                        {
         
                            RPC1.BackColor = Color.Gray;
                        }
                    }

                    if (dataMatrix2[9, 3] != 0)
                    {
                   
                        RPC2.BackColor = Color.Blue;
                    }
                    else
                    {
                        if (dataMatrix2[9, 4] != 0)
                        {
                       
                            RPC2.BackColor = Color.Red;
                        }
                        else
                        {
                         
                            RPC2.BackColor = Color.Gray;
                        }
                    }

                    if (dataMatrix2[10, 3] != 0)
                    {
                        RPC3.BackColor = Color.Blue;
                       
                    }
                    else
                    {
                        if (dataMatrix2[10, 4] != 0)
                        {
                            RPC3.BackColor = Color.Red;
                          
                        }
                        else
                        {
                            RPC3.BackColor = Color.Gray;
                         
                        }
                    }

                    if (dataMatrix2[11, 3] != 0)
                    {
                        RPC4.BackColor = Color.Blue;
                      
                    }
                    else
                    {
                        if (dataMatrix2[11, 4] != 0)
                        {
                            RPC4.BackColor = Color.Red;
                         
                        }
                        else
                        {
                            RPC4.BackColor = Color.Gray;
                          
                        }
                    }
                    if (dataMatrix2[5, 3] != 0)
                    {
                        RPC5.BackColor = Color.Blue;
                       
                    }
                    else
                    {
                        if (dataMatrix2[5, 4] != 0)
                        {
                            RPC5.BackColor = Color.Red;
                            
                        }
                        else
                        {
                            RPC5.BackColor = Color.Gray;
                       
                        }
                    }
                    secCurr = dataMatrix2[8, 1] + dataMatrix2[9, 1] + dataMatrix2[10, 1] + dataMatrix2[11, 1] + dataMatrix2[5, 1] + dataMatrix2[0, 1];
                }

                else
                {
                    secCurr = 0;
                  
                    RPC1.BackColor = Color.Gray;
                    RPC2.BackColor = Color.Gray;
                    RPC3.BackColor = Color.Gray;
                    RPC4.BackColor = Color.Gray;
                    RPC5.BackColor = Color.Gray;
                    RightSSPC.BackColor = Color.Gray;
                }


                if (dataMatrix[0, 3] != 0)
                {
                    LeftCont.BackColor = Color.Blue;
                    LL2.BackColor = Color.Blue;
                    LeftCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_on; 
                }
                else
                {
                    if (dataMatrix[0, 4] != 0)
                    {
                        LeftCont.BackColor = Color.Red;
                        LL2.BackColor = Color.Red;
                        LeftCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_tripped; 
                    }
                    else
                    {
                        LeftCont.BackColor = Color.Gray;
                        LL2.BackColor = Color.Gray;
                        LeftCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_off; 
                    }
                }

                if (dataMatrix2[0, 3] != 0)
                {
                    RightCont.BackColor = Color.Blue;
                    LR2.BackColor = Color.Blue;
                    RightCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_on;
                }
                else
                {
                    if (dataMatrix2[0, 4] != 0)
                    {
                        RightCont.BackColor = Color.Red;
                        LR2.BackColor = Color.Red;
                        RightCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_tripped;
                    }
                    else
                    {
                        RightCont.BackColor = Color.Gray;
                        LR2.BackColor = Color.Gray;
                        RightCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.combo_off;
                    }
                }


                if (((dataMatrix[1, 3] == 0) && (dataMatrix2[1, 3] == 1)) || ((dataMatrix[1, 3] == 1) && (dataMatrix2[1, 3] == 0)))
                {

                    BT.BackColor = Color.Blue;
                    BT.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_on;
                }
                else
                {

                    BT.BackColor = Color.Gray;
                    BT.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_off;
                }


                if (dataMatrix[3, 3] != 0)
                {
                    LSCont.BackColor = Color.Blue;
                    LSCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_on;
                }
                else
                {
                    if (dataMatrix[3, 4] != 0)
                    {
                        LSCont.BackColor = Color.Red;
                        LSCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_tripped;
                    }
                    else
                    {
                        LSCont.BackColor = Color.Gray;
                        LSCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_off;
                    }
                }

                if (dataMatrix2[3, 3] != 0)
                {
                    RSCont.BackColor = Color.Blue;
                    RSCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_on;
                }
                else
                {
                    if (dataMatrix2[3, 4] != 0)
                    {
                        RSCont.BackColor = Color.Red;
                        RSCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_tripped;
                    }
                    else
                    {
                        RSCont.BackColor = Color.Gray;
                        RSCont.BackgroundImage = SSPC_DemoUI.Properties.Resources.noHall_off;
                    }
                }


                if ((dataMatrix[3, 3] != 0) || (dataMatrix2[3, 3] != 0))
                {
                    if (priCurr < 0)
                    {
                        priCurr = 0;
                    }

                    if (secCurr < 0)
                    {
                        secCurr = 0;
                    }

                    if (dataMatrix[2, 3] == 0)
                    {
                        secCurr += priCurr;
                        priCurr = 0;
                    }

                    if (dataMatrix2[2, 3] == 0)
                    {
                        priCurr += secCurr;
                        secCurr = 0;
                    }

                    if ((priCurr + secCurr) < 10)
                    {
                        Math.Round(priCurr, 1);
                        Math.Round(secCurr, 1);
                        LeftDigitGauge.Text = priCurr.ToString("0.0");
                        rightDigitGauge.Text = secCurr.ToString("0.0");
                        linearScaleLevelComponent3.Value = Convert.ToSingle(priCurr + secCurr);
                    }
                }
                else
                {
                    LeftDigitGauge.Text = "0.0";
                    rightDigitGauge.Text = "0.0";                  
                    linearScaleLevelComponent3.Value = Convert.ToSingle(0.0);
                }
            }
        }

        private void matrixUpdate(Object source, ElapsedEventArgs e)
        { 
            Array.Copy(Comm.inComingData, dataMatrix, 90);
            Array.Copy(Comm.inComingBrdData, boardDataMatrix, 2);
            Array.Copy(Comm.inComingBrdData2, boardDataMatrix2, 2);
            Array.Copy(Comm.inComingData2, dataMatrix2, 90);
            dataMatrix[0, 1] = dataMatrix[0, 5];
            dataMatrix[2, 1] = dataMatrix[2, 5];
            dataMatrix2[0, 1] = dataMatrix2[0, 5];
            dataMatrix2[2, 1] = dataMatrix2[2, 5];

            status_Update(dataMatrix, dataMatrix2, boardDataMatrix, boardDataMatrix2);

           

           
        }
        
        private void SysStart_Click(object sender, EventArgs e)
        {
            SysStart.Enabled = false;
            Comm.primary_Start();
            Secondary_Sys.Enabled = true;
            SysDown.Enabled = true;
            load_manag_group.Enabled = true;
            ovr_crnt_group.Enabled = true;
            gen_failure.Enabled = true;
            event_sequence(2, false);
            normal_mode_Click_1(null, null);
            BT.Enabled = true;
            autoToggle_right.IsOn = true;
            autoToggle_Left.IsOn = true;
            manToggle_Left.IsOn = true;
            manualToggle_Right.IsOn = true;
        }

        private void SysUkn_Click(object sender, EventArgs e)
        {        
            Comm.secondary_Start();
            Load_Shedding.BackColor = Color.Cyan;
            I2T_Overload.BackColor = Color.Cyan;
            seci2t.BackColor = Color.Cyan;
            inst_trip.BackColor = Color.Cyan;
        }
        private void SysDown_Click(object sender, EventArgs e)
        { 
            Comm.system_Down();
            System.Threading.Thread.Sleep(100);
            Array.Clear(Comm.inComingData, 0, Comm.inComingData.Length);
            Array.Clear(Comm.inComingData2, 0, Comm.inComingData2.Length);
            SysStart.Enabled = true;
            load_manag_group.Enabled = false;
            
            gen_failure.Enabled = false;
           
            ovr_crnt_group.Enabled = false;
            load_manag_group.Enabled = false;
            BT.Enabled = false;
            event_sequence(1, true);
        }

        private void LeftGen_Click(object sender, EventArgs e)
        {
            
            event_sequence(2, false);
            Point pos;
            LeftGenForm LeftGenForm = new LeftGenForm();
            LeftGenForm.StartPosition = FormStartPosition.Manual;
            pos = MousePosition;
            pos.X += -50;
            pos.Y += -50;
            LeftGenForm.Show();
        }

        private void RightGen_Click(object sender, EventArgs e)
        {
            Point pos;
            RightGenForm RightGenForm = new RightGenForm();
            RightGenForm.StartPosition = FormStartPosition.Manual;
            pos = MousePosition;
            pos.X += -300;
            pos.Y += -100;
            RightGenForm.Show();
        }

        private void ControlGroup_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void LftSSPC_Click(object sender, EventArgs e)
        {
            if (dataMatrix[3, 3] == 1)
            {
                
                LeftGenForm LeftSSPCForm = new LeftGenForm();
                LeftSSPCForm.StartPosition = FormStartPosition.Manual;
                          
                LeftSSPCForm.Show();
            }
        }

        private void RightSSPC_Click(object sender, EventArgs e)
        {
            if (dataMatrix2[3, 3] == 1)
            {
                RightGenForm RightSSPCForm = new RightGenForm();
                RightSSPCForm.StartPosition = FormStartPosition.Manual;
                RightSSPCForm.Show();
            }
        }

        private void L1PC_Click(object sender, EventArgs e)
        {
            if (((dataMatrix[2, 3] == 1) || (dataMatrix[1, 3] == 1)) && (dataMatrix[3, 3] == 1))
            {
                Point pos;
                LeftLoad LeftLoadForm1 = new LeftLoad();
                LeftLoadForm1.Text = "Left Load 1";
                LeftLoadForm1.chnl_num(0x08);
                LeftLoadForm1.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                LeftLoadForm1.Show();
            }
        }

        public static DateTime PauseForMilliSeconds(int MillisecondsToPauseFor)
        {
            System.DateTime thisMoment = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 0, 0, MillisecondsToPauseFor);
            System.DateTime Afterwards = thisMoment.Add(duration);

            while (Afterwards >= thisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                thisMoment = System.DateTime.Now;
            }
            return System.DateTime.Now;
        }

        private void Load_Shedding_Click(object sender, EventArgs e)
        {
            Comm.load_Shedding();
            event_sequence(12, false);
        }

        private void I2T_Overload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Toggle Left Primary Switch for I2t trip");
            event_sequence(8, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Toggle Left Secondary Switch for Instant trip");
            event_sequence(9, false);
        }

        private void L2PC_Click(object sender, EventArgs e)
        {
            if (((dataMatrix[2, 3] == 1) || (dataMatrix[1, 3] == 1)) && (dataMatrix[3, 3] == 1))
            {
                Point pos;
                LeftLoad LeftLoadForm2 = new LeftLoad();
                LeftLoadForm2.chnl_num(0x09);
                LeftLoadForm2.StartPosition = FormStartPosition.Manual;
                LeftLoadForm2.Text = "Left Load 2";
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                //LeftLoadForm2.Location = pos;
                LeftLoadForm2.Show();
            }
        }

        private void L3PC_Click(object sender, EventArgs e)
        {
            if (((dataMatrix[2, 3] == 1) || (dataMatrix[1, 3] == 1)) && (dataMatrix[3, 3] == 1))
            {
                Point pos;
                LeftLoad LeftLoadForm3 = new LeftLoad();
                LeftLoadForm3.chnl_num(0x0A);
                LeftLoadForm3.StartPosition = FormStartPosition.Manual;
                LeftLoadForm3.Text = "Left Load 3";
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                //LeftLoadForm3.Location = pos;
                LeftLoadForm3.Show();
            }
        }

        private void L4PC_Click(object sender, EventArgs e)
        {
            if (((dataMatrix[2, 3] == 1) || (dataMatrix[1, 3] == 1)) && (dataMatrix[3, 3] == 1))
            {
                Point pos;
                LeftLoad LeftLoadForm4 = new LeftLoad();
                LeftLoadForm4.chnl_num(0x0B);
                LeftLoadForm4.StartPosition = FormStartPosition.Manual;
                LeftLoadForm4.Text = "Left Load 4";
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                //LeftLoadForm4.Location = pos;
                LeftLoadForm4.Show();
            }
        }

        private void L5PC_Click(object sender, EventArgs e)
        {
            if (((dataMatrix[2, 3] == 1) || (dataMatrix[1, 3] == 1)) && (dataMatrix[3, 3] == 1))
            {
                Point pos;
                LeftLoad LEftLoadForm5 = new LeftLoad();
                LEftLoadForm5.chnl_num(0x05);
                LEftLoadForm5.StartPosition = FormStartPosition.Manual;
                LEftLoadForm5.Text = "Left Load 5";
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                //LEftLoadForm5.Location = pos;
                LEftLoadForm5.Show();
            }
        }

        private void PrimaryControl_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void RPC1_Click(object sender, EventArgs e)
        {
            if (dataMatrix2[3, 3] == 1)
            {
                Point pos;
                RightLoad rightLoad1 = new RightLoad();
                rightLoad1.chnl_num(0x08);
                rightLoad1.Text = "Right Load 1";
                rightLoad1.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                rightLoad1.Show();
            }
        }

        private void RPC2_Click(object sender, EventArgs e)
        {
            if (dataMatrix2[3, 3] == 1)
            {
                Point pos;
                RightLoad rightLoad2 = new RightLoad();
                rightLoad2.chnl_num(0x09);
                rightLoad2.Text = "Right Load 2";
                rightLoad2.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.Y += -50;
                pos.X += -50;
                rightLoad2.Show();
            }
        }

        private void RPC3_Click(object sender, EventArgs e)
        {
            if (dataMatrix2[3, 3] == 1)
            {
                Point pos;
                RightLoad rightLoad3 = new RightLoad();
                rightLoad3.chnl_num(0x0A);
                rightLoad3.Text = "Right Load 3";
                rightLoad3.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                rightLoad3.Show();
            }
        }

        private void RPC4_Click(object sender, EventArgs e)
        {
            if (dataMatrix2[3, 3] == 1)
            {
                Point pos;
                RightLoad rightLoad4 = new RightLoad();
                rightLoad4.chnl_num(0x0B);
                rightLoad4.Text = "Right Load 4";
                rightLoad4.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
                rightLoad4.Show();
            }
        }

        private void RPC5_Click(object sender, EventArgs e)
        {
            if (dataMatrix2[3, 3] == 1)
            {
                Point pos;
                RightLoad rightLoad5 = new RightLoad();
                rightLoad5.chnl_num(0x05);
                rightLoad5.Text = "Right Load 5";
                rightLoad5.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
               
                rightLoad5.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((dataMatrix[2,3] == 1) || (dataMatrix[1,3] == 1))
            {
                Comm.secLftCont();
            }
        }

        private void RSCont_Click(object sender, EventArgs e)
        {
            if ((dataMatrix2[2, 3] == 1) || (dataMatrix2[1, 3] == 1))
            {
                Comm.secRghtCont();
            }
        }

        private void LeftCont_Click(object sender, EventArgs e)
        {
            if ((dataMatrix[2, 3] == 1) || (dataMatrix[1, 3] == 1))
            {
                event_sequence(3, false);
                Point pos;
                LeftLoad LeftContactor = new LeftLoad();
                LeftContactor.chnl_num(0x00);
                LeftContactor.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -50;
                pos.Y += -50;
               // LeftContactor.Location = pos;
                LeftContactor.Show();
            }
        }

        private void RightCont_Click(object sender, EventArgs e)
        {
            if ((dataMatrix2[2, 3] == 1) || (dataMatrix2[1, 3] == 1))
            {
                Point pos;
                RightLoad rightContactor = new RightLoad();
                rightContactor.chnl_num(0x00);
                rightContactor.StartPosition = FormStartPosition.Manual;
                pos = MousePosition;
                pos.X += -300;
                pos.Y += -100;
                rightContactor.Show();
            }
        }

        private void exit_out_Click(object sender, EventArgs e)
        {
            Comm.system_Down();
            Comm.appClosing();
            Application.Exit();
        }

        private void normal_mode_Click_1(object sender, EventArgs e)
        {
            byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05, 0x00};                   
            Comm.load_Management(shutoff_loads, 5, 0xFC, 3);
            byte[] norm_loads = {0x05, 0x0B, 0x08, 0x00};
            Comm.load_Management(norm_loads, 3, 0xFD, 3);
            event_sequence(7, false);
        }

        private void critical_mode_Click(object sender, EventArgs e)
        {
            byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05};
            Comm.load_Management(shutoff_loads, 4, 0xFC, 3);
            byte[] critical_loads = { 0x05, 0x0B, 0x00};
            Comm.load_Management(critical_loads, 2, 0xFD, 3);
            event_sequence(5, false);
        }

        private void noncrit_mode_Click(object sender, EventArgs e)
        {
            byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05};
            Comm.load_Management(shutoff_loads, 4, 0xFC, 3);
            byte[] nonCrit_loads = { 0x08, 0x09, 0x0A };
            Comm.load_Management(nonCrit_loads, 2, 0xFD, 3);
            event_sequence(6, false);
        }

        private void shutoff_mode_Click(object sender, EventArgs e)
        {
            byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05, 0x00};
            Comm.load_Management(shutoff_loads, 5, 0xFC, 3);
            event_sequence(4, false);
            
        }

        private void automatic_left_Click(object sender, EventArgs e)
        {
            lefGn = !lefGn;
            automatic_left.Enabled = false;
          
            if (lefGn == false)
            {
                automatic_left.Text = "TURN ON";
                automatic_left.BackColor = Color.Purple;
                manual_left.Enabled = false;
                automatic_right.Enabled = false;
                manual_right.Enabled = false;
                event_sequence(11, false);
            }
            else
            {
                automatic_left.Text = "AUTOMATIC";
                automatic_left.BackColor = Color.White;
                manual_left.Enabled = true;
                automatic_right.Enabled = true;
                manual_right.Enabled = true;
            }
           // Comm.autoMatic_leftGen(lefGn, );
            automatic_left.Enabled = true;
        }

        private void automatic_right_Click(object sender, EventArgs e)
        {
            rightGn = !rightGn;
            automatic_right.Enabled = false;

            if (rightGn == false)
            {
                automatic_right.Text = "TURN ON";
                automatic_right.BackColor = Color.Purple;
                manual_left.Enabled = false;
                automatic_left.Enabled = false;
                manual_right.Enabled = false;

            }
            else
            {
                automatic_right.Text = "AUTOMATIC";
                automatic_right.BackColor = Color.White;
                manual_left.Enabled = true;
                automatic_left.Enabled = true;
                manual_right.Enabled = true;
            }
            //Comm.autoMatic_rightGen(rightGn);
            automatic_right.Enabled = true;
        }

        private void manual_left_Click(object sender, EventArgs e)
        {
            lefGn = !lefGn;
            manual_left.Enabled = false;
            if (lefGn == false)
            {
                manual_left.Text = "TURN ON";
                manual_left.BackColor = Color.Purple;
                automatic_right.Enabled = false;
                automatic_left.Enabled = false;
                manual_right.Enabled = false;
            }
            else
            {
                manual_left.Text = "MANUAL";
                manual_left.BackColor = Color.White;
                automatic_right.Enabled = true;
                automatic_left.Enabled = true;
                manual_right.Enabled = true;
            }
            Comm.manual_LeftGen(lefGn);
            manual_left.Enabled = true;
        }

        private void BT_Click(object sender, EventArgs e)
        {
            if ((lefGn == false) && (rightGn == true))
            {      
                Comm.Left_busContactor();
                MessageBox.Show("Left Generator failed bus tie is on");
            }

            if ((rightGn == false) && (lefGn == true))
            {
                Comm.Right_busContactor();
                MessageBox.Show("Right Generator failed bus tie is on");
            }

            if ((lefGn == true) && (rightGn == true))
            {
                MessageBox.Show("Both Generators are on");
            }

            if ((lefGn == false) && (rightGn == false))
            {
                MessageBox.Show("Both Generators are off");
            }
            event_sequence(15, false);
        }

        private void manual_right_Click(object sender, EventArgs e)
        {
            rightGn = !rightGn;
            manual_right.Enabled = false;

            if (rightGn == false)
            {
                manual_right.Text = "TURN ON";
                manual_right.BackColor = Color.Purple;
                automatic_right.Enabled = false;
                automatic_left.Enabled = false;
                manual_left.Enabled = false;
            }
            else
            {
                manual_right.Text = "MANUAL";
                manual_right.BackColor = Color.White;
                automatic_right.Enabled = true;
                automatic_left.Enabled = true;
                manual_left.Enabled = true;
            }
            Comm.manual_RightGen(rightGn);
            manual_right.Enabled = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void buffeR_Clear_Click(object sender, EventArgs e)
        {
            Comm.buffeR_clear();
            MessageBox.Show("Buffer Cleared");
            
        }

        private void XMT_ERRr_Click(object sender, EventArgs e)
        {
       
        }

        private void inst_trip_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Toggle Right Secondary Switch for I2t trip");
            event_sequence(10, false);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Toggle Right Primary Switch for Instant Trip");
            event_sequence(11, false);
        }

        private void event_sequence(byte noOfEvent, bool reset)
        {
            string[] event_Seq = {" Click “INITIALIZE” button to establish communication with the Power Management system", 
                                     " Click “SYSTEM START” This initiates a controlled soft start of Primary Contactors and secondary distribution", 
                                     " Click  “Left Primary Contactor”. This window demonstrates the diagnostics information available from each Contactor or Secondary load", 
                                     " LOAD MANAGEMENT: Click ”SHUTOFF” to turn off all loads off", 
                                     " LOAD MANAGEMENT: Click on “CRITICAL” to Turn on all loads marked critical within the system", 
                                     " LOAD MANAGEMENT: Click “NON-CRITICAL” to turn on all loads marked non-critical within the system", 
                                     " LOAD MANAGEMENT: Click “NORMAL” to put the system in a default state with all normally running loads on",                                     
                                     " Click “I2T PRIMARY SWITCH”. This will initiate an 4x overcurrent fault on the Left Primary Load causing an I2t trip condition", 
                                     " Click “INSTANT SECONDARY SWITCH” This will initiate a 10x overcurrent fault on the Left Secondary load causing an Instant trip condition",
                                     " Click “I2T SECONDARY SWITCH” This will initiate a 4x overcurrent fault on the Right Secondary Load causing an I2t trip condition",
                                     " Click “INSTANT PRIMARY SWITCH” This will initiate a 10x overcurrent fault on the Right Primary Load causing an Instant trip condition",
                                     " Click “LOAD SHEDDING”. This will insert a fault where all loads are turned on causing a overload condition. The system will automatically detect this condition and shed all Non-Critical Loads ", 
                                     " Click Left generator Failure-Automatic. This will insert a Generator fault causing the Left generator contactor to go Offline. The system will detect this condition and automatically turn on the Bus Tie contactor to  provide power", 
                                     " Click “Left Generator-Manual”. This will insert a Generator fault causing the left generator contactor to go offline. The system will detect this condition and guide the operator through the reconfiguration process", 
                                     " Click on “BUS-TIE”. This contactor connects both sides, incase one side generator fails the bus tie will deliver power from the other side",
                                     " Click “System Shutdown” to initiate a sequenced shutdown of the system and reset ",                                
                                 };
 
            if ((eventNo == noOfEvent) || (reset == true))
            {
                if (noOfEvent == 0)
                {
                    Initialize.FlatAppearance.BorderColor = Color.Orange;
                    Initialize.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 1)
                {
                    Initialize.FlatAppearance.BorderColor = Color.Black;
                    SysDown.FlatAppearance.BorderColor = Color.Black;
                    SysStart.FlatAppearance.BorderColor = Color.Orange;
                    LeftCont.FlatAppearance.BorderColor = Color.Black;
                    shutoff_mode.FlatAppearance.BorderColor = Color.Black;
                    critical_mode.FlatAppearance.BorderColor = Color.Black;
                    noncrit_mode.FlatAppearance.BorderColor = Color.Black;
                    normal_mode.FlatAppearance.BorderColor = Color.Black;
                    I2T_Overload.FlatAppearance.BorderColor = Color.Black;
                    seci2t.FlatAppearance.BorderColor = Color.Black;
                    inst_trip.FlatAppearance.BorderColor = Color.Black;
                    Load_Shedding.FlatAppearance.BorderColor = Color.Black;
                    groupBox4.BackColor = SystemColors.ActiveCaption;

                    SysStart.FlatAppearance.BorderSize = 5;

                    Initialize.FlatAppearance.BorderSize = 2;
                    SysDown.FlatAppearance.BorderSize = 2;
                    LeftCont.FlatAppearance.BorderSize = 2;
                    shutoff_mode.FlatAppearance.BorderSize = 2;
                    critical_mode.FlatAppearance.BorderSize = 2;
                    noncrit_mode.FlatAppearance.BorderSize = 2;
                    normal_mode.FlatAppearance.BorderSize = 2;
                    I2T_Overload.FlatAppearance.BorderSize = 2;
                    seci2t.FlatAppearance.BorderSize = 2;
                    inst_trip.FlatAppearance.BorderSize = 2;
                    Load_Shedding.FlatAppearance.BorderSize = 2;
                    groupBox4.BackColor = SystemColors.ActiveCaption;
                }

                if (noOfEvent == 2)
                {
                    SysStart.FlatAppearance.BorderColor = Color.Black;
                    SysStart.FlatAppearance.BorderSize = 2;
                    LeftCont.FlatAppearance.BorderColor = Color.Orange;
                    LeftCont.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 3)
                {
                    LeftCont.FlatAppearance.BorderColor = Color.Black;
                    LeftCont.FlatAppearance.BorderSize = 2;
                    shutoff_mode.FlatAppearance.BorderColor = Color.Orange;
                    shutoff_mode.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 4)
                {
                    shutoff_mode.FlatAppearance.BorderColor = Color.Black;
                    shutoff_mode.FlatAppearance.BorderSize = 2;
                    critical_mode.FlatAppearance.BorderColor = Color.Orange;
                    critical_mode.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 5)
                {
                    critical_mode.FlatAppearance.BorderColor = Color.Black;
                    critical_mode.FlatAppearance.BorderSize = 2;
                    noncrit_mode.FlatAppearance.BorderColor = Color.Orange;
                    noncrit_mode.FlatAppearance.BorderSize = 5;
                }


                if (noOfEvent == 6)
                {
                    noncrit_mode.FlatAppearance.BorderColor = Color.Black;
                    noncrit_mode.FlatAppearance.BorderSize = 2;
                    normal_mode.FlatAppearance.BorderColor = Color.Orange;
                    normal_mode.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 7)
                {
                    normal_mode.FlatAppearance.BorderColor = Color.Black;
                    normal_mode.FlatAppearance.BorderSize = 2;
                    I2T_Overload.FlatAppearance.BorderColor = Color.Orange;
                    I2T_Overload.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 8)
                {
                    
                    I2T_Overload.FlatAppearance.BorderSize = 2;
                    I2T_Overload.FlatAppearance.BorderColor = Color.Black;
                    seci2t.FlatAppearance.BorderColor = Color.Orange;
                    seci2t.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 9)
                {
                   
                    seci2t.FlatAppearance.BorderColor = Color.Black;
                    seci2t.FlatAppearance.BorderSize = 2;
                    inst_trip.FlatAppearance.BorderColor = Color.Orange;
                    inst_trip.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 10)
                {                   
                    inst_trip.FlatAppearance.BorderColor = Color.Black;
                    inst_trip.FlatAppearance.BorderSize = 2;
                    inst_Primary.FlatAppearance.BorderColor = Color.Orange;
                    inst_Primary.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 11)
                {
                    
                    inst_Primary.FlatAppearance.BorderColor = Color.Black;
                    inst_Primary.FlatAppearance.BorderSize = 2;
                    Load_Shedding.FlatAppearance.BorderColor = Color.Orange;
                    Load_Shedding.FlatAppearance.BorderSize = 5;
                }

                if (noOfEvent == 12)
                {
                    Load_Shedding.FlatAppearance.BorderColor = Color.Black;
                    Load_Shedding.FlatAppearance.BorderSize = 2;
                    groupBox4.BackColor = Color.Orange;
                }

                if (noOfEvent == 14)
                {
                    groupBox4.BackColor = SystemColors.ActiveCaption;
                    BT.FlatAppearance.BorderSize = 5;
                    BT.FlatAppearance.BorderColor = Color.Orange;
                   
                }

                if (noOfEvent == 15)
                {
                    BT.FlatAppearance.BorderSize = 2;
                    BT.FlatAppearance.BorderColor = Color.Black;
                    SysDown.FlatAppearance.BorderColor = Color.Orange;
                    SysDown.FlatAppearance.BorderSize = 5;
                }
                
                events_Text.Text = event_Seq[noOfEvent];
                eventNo++;
            }

            if (eventNo == 16)
            {
                eventNo = 1;
            }

            if (reset == true)
            {
                eventNo = 2;
            }

        }

        private void events_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void autoToggle_Left_Toggled(object sender, EventArgs e)
        {
            if (autoToggle_Left.IsOn == false)
            {
                manToggle_Left.Enabled = false;
                manualToggle_Right.Enabled = false;
                autoToggle_right.Enabled = false;
                lefGn = false;
                byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05 };
                Comm.load_Management(shutoff_loads, 4, 0xFC, 1);

            }
            else
            {
                manToggle_Left.Enabled = true;
                manualToggle_Right.Enabled = true;
                autoToggle_right.Enabled = true;
                lefGn = true;
                normal_mode_Click_1(null, null);
            }
            event_sequence(13, false);
            Comm.autoMatic_leftGen(autoToggle_Left.IsOn, autoToggle_right.IsOn);
        }

        private void manToggle_Left_Toggled(object sender, EventArgs e)
        {
            if (manToggle_Left.IsOn == false)
            {
                autoToggle_Left.Enabled = false;
                manualToggle_Right.Enabled = false;
                autoToggle_right.Enabled = false;
                lefGn = false;
                byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05 };
                Comm.load_Management(shutoff_loads, 4, 0xFC, 1);
            }
            else
            {
                autoToggle_Left.Enabled = true;
                manualToggle_Right.Enabled = true;
                autoToggle_right.Enabled = true;
                lefGn = true;
                normal_mode_Click_1(null, null);
            }
            event_sequence(14, false);
            Comm.manual_LeftGen(manToggle_Left.IsOn);
        }

        private void autoToggle_right_Toggled(object sender, EventArgs e)
        {
            if (autoToggle_right.IsOn == false)
            {
                manualToggle_Right.Enabled = false;
                autoToggle_Left.Enabled = false;
                manToggle_Left.Enabled = false;
                rightGn = false;
                byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05 };
                Comm.load_Management(shutoff_loads, 4, 0xFC, 2);
            }
            else
            {
                manualToggle_Right.Enabled = true;
                autoToggle_Left.Enabled = true;
                manToggle_Left.Enabled = true;
                rightGn = true;
                normal_mode_Click_1(null, null);
            }
            Comm.autoMatic_rightGen(autoToggle_right.IsOn, autoToggle_Left.IsOn);
        }

        private void manualToggle_Right_Toggled(object sender, EventArgs e)
        {
            if (manualToggle_Right.IsOn == false)
            {
               
                autoToggle_Left.Enabled = false;
                manToggle_Left.Enabled = false;
                autoToggle_right.Enabled = false;
                rightGn = false;
                byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05 };
                Comm.load_Management(shutoff_loads, 4, 0xFC, 2);
            }
            else
            {
                autoToggle_Left.Enabled = true;
                manToggle_Left.Enabled = true;
                autoToggle_right.Enabled = true;
                rightGn = true;
                normal_mode_Click_1(null, null);
            }
            Comm.manual_RightGen(manualToggle_Right.IsOn);
        }

        private void leftPrim_Click(object sender, EventArgs e)
        {

        }

        private void gaugeControl1_Click(object sender, EventArgs e)
        {

        }      
    }
}

