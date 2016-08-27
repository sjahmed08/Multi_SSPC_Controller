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
    public partial class RightGenForm : Form
    {
        private static System.Timers.Timer dataTimer;
        private static bool PinButton = false;
        byte formCloseCounter = 0;
        public RightGenForm()
        {
            InitializeComponent();
          //  this.FormBorderStyle = FormBorderStyle.None;
            dataTimer = new System.Timers.Timer(100);
            dataTimer.Elapsed += displayGauge;
            dataTimer.Enabled = true;
            this.CenterToParent();
            this.TopMost = true;
            this.TopLevel = true;           
        }
        delegate void setStat_Update(double[,] boardData2);

        public void stat_update(double[,] boardData2)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new setStat_Update(stat_update), boardData2);
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


                if ((formCloseCounter == 50) && (PinButton == false))
                {
                    Close();
                }
            }
        }

        public void displayGauge(Object source, ElapsedEventArgs e)
        {
            try
                {
                    stat_update(Main.dataMatrix2);
                    arcScaleNeedleComponent3.ArcScale.Value = Convert.ToUInt16(Main.dataMatrix2[2, 1]);
                    arcScaleNeedleComponent4.ArcScale.Value = Convert.ToUInt16(Main.boardDataMatrix2[0, 0]);
                    linearScaleLevelComponent2.Value = Convert.ToUInt16(Main.boardDataMatrix2[0, 1]);
                }
            catch (System.NullReferenceException)
                {
                }
            catch (System.OverflowException)
                {

                }
        }

        private void Pinbutton_Click(object sender, EventArgs e)
        {
            PinButton = true;
            this.TopLevel = true;
            this.TopMost = true;
        }

        private void RightGenForm_Load(object sender, EventArgs e)
        {

        }
    }
}
