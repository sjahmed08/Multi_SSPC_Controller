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
using DevExpress.XtraGauges.Win.Gauges.Linear;
using System.Timers;
namespace SSPC_DemoUI
{
    public partial class LeftGenForm : Form
    {
        bool pinButton = false;
        byte formCloseCounter = 0;
        private static System.Timers.Timer dataTimer;
        public LeftGenForm()
        {
            InitializeComponent();
            this.Opacity = 0.99;
            dataTimer = new System.Timers.Timer(100);
            dataTimer.Elapsed += displayGauge;
            dataTimer.Enabled = true;
            this.CenterToParent();
            this.TopMost = true;
            this.TopLevel = true;                              
        }

        delegate void statUpdate(double[,] bordData);

        public void stat_update(double[,] boardData)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new statUpdate(stat_update), boardData);
                }
                catch (System.ObjectDisposedException)
                {
                }
            }
            else
            {
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


        public void displayGauge(Object source, ElapsedEventArgs e)
        {
            try
            {
                stat_update(Main.boardDataMatrix);
                arcScaleNeedleComponent2.ArcScale.Value = Convert.ToUInt16(Main.boardDataMatrix[0, 0]);
                arcScaleNeedleComponent3.ArcScale.Value = Convert.ToUInt16(Main.dataMatrix[2, 1]);
                linearScaleLevelComponent1.Value = Convert.ToUInt16(Main.boardDataMatrix[0, 1]);
               
            }
            catch (System.NullReferenceException)
            {
            }
            catch (System.OverflowException)
            {
            }                
        }

        private void LeftGenForm_Load(object sender, EventArgs e)
        {

        }

        private void pin_button_Click(object sender, EventArgs e)
        {
            pinButton = true;
            this.TopLevel = true;
            this.TopMost = true;
        }

        private void gaugeControl1_Click(object sender, EventArgs e)
        {

        }
    
    }
}
