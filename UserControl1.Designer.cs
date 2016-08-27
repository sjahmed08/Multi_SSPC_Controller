namespace SSPC_DemoUI
{
    partial class UserControl1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.primary_Current = new DevExpress.XtraGauges.Win.GaugeControl();
            this.digitalGauge1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge();
            this.digitalBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent();
            this.LSCont = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.LftSSPC = new System.Windows.Forms.Button();
            this.LeftGen = new System.Windows.Forms.Button();
            this.LeftCont = new System.Windows.Forms.Button();
            this.L3PC = new System.Windows.Forms.Button();
            this.L5PC = new System.Windows.Forms.Button();
            this.L4PC = new System.Windows.Forms.Button();
            this.L2PC = new System.Windows.Forms.Button();
            this.L1PC = new System.Windows.Forms.Button();
            this.lbLed1 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed5 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed3 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed2 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed4 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.LL2 = new System.Windows.Forms.Button();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.editFormUserControl1 = new DevExpress.XtraGrid.Views.Grid.EditFormUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.digitalGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // primary_Current
            // 
            this.primary_Current.AutoLayout = false;
            this.primary_Current.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.editFormUserControl1.SetBoundPropertyName(this.primary_Current, "");
            this.primary_Current.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.digitalGauge1});
            this.primary_Current.Location = new System.Drawing.Point(39, 37);
            this.primary_Current.Name = "primary_Current";
            this.primary_Current.Size = new System.Drawing.Size(248, 91);
            this.primary_Current.TabIndex = 83;
            // 
            // digitalGauge1
            // 
            this.digitalGauge1.AppearanceOff.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#00FFFFFF");
            this.digitalGauge1.AppearanceOn.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:WhiteSmoke");
            this.digitalGauge1.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent[] {
            this.digitalBackgroundLayerComponent1});
            this.digitalGauge1.Bounds = new System.Drawing.Rectangle(-2, 0, 252, 91);
            this.digitalGauge1.DigitCount = 5;
            this.digitalGauge1.Name = "digitalGauge1";
            this.digitalGauge1.Text = "00,000";
            // 
            // digitalBackgroundLayerComponent1
            // 
            this.digitalBackgroundLayerComponent1.BottomRight = new DevExpress.XtraGauges.Core.Base.PointF2D(259.8125F, 99.9625F);
            this.digitalBackgroundLayerComponent1.Name = "digitalBackgroundLayerComponent6";
            this.digitalBackgroundLayerComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.DigitalBackgroundShapeSetType.Style6;
            this.digitalBackgroundLayerComponent1.TopLeft = new DevExpress.XtraGauges.Core.Base.PointF2D(20F, 0F);
            this.digitalBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // LSCont
            // 
            this.LSCont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LSCont.Location = new System.Drawing.Point(105, 166);
            this.LSCont.Name = "LSCont";
            this.LSCont.Size = new System.Drawing.Size(126, 64);
            this.LSCont.TabIndex = 88;
            this.LSCont.Text = "LEFT SECONDARY CONTACTOR";
            this.LSCont.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 13);
            this.label10.TabIndex = 87;
            this.label10.Text = "LEFT CURRENT (A)";
            // 
            // LftSSPC
            // 
            this.LftSSPC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LftSSPC.BackgroundImage")));
            this.LftSSPC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.editFormUserControl1.SetBoundPropertyName(this.LftSSPC, "");
            this.LftSSPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LftSSPC.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LftSSPC.Location = new System.Drawing.Point(16, 246);
            this.LftSSPC.Name = "LftSSPC";
            this.LftSSPC.Size = new System.Drawing.Size(336, 135);
            this.LftSSPC.TabIndex = 62;
            this.LftSSPC.UseVisualStyleBackColor = true;
            // 
            // LeftGen
            // 
            this.LeftGen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LeftGen.BackgroundImage")));
            this.LeftGen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.LeftGen.Location = new System.Drawing.Point(34, 33);
            this.LeftGen.Name = "LeftGen";
            this.LeftGen.Size = new System.Drawing.Size(154, 109);
            this.LeftGen.TabIndex = 55;
            this.LeftGen.UseVisualStyleBackColor = true;
            // 
            // LeftCont
            // 
            this.LeftCont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftCont.Image = ((System.Drawing.Image)(resources.GetObject("LeftCont.Image")));
            this.LeftCont.Location = new System.Drawing.Point(43, 283);
            this.LeftCont.Name = "LeftCont";
            this.LeftCont.Size = new System.Drawing.Size(125, 100);
            this.LeftCont.TabIndex = 54;
            this.LeftCont.UseVisualStyleBackColor = true;
            // 
            // L3PC
            // 
            this.L3PC.Location = new System.Drawing.Point(155, 402);
            this.L3PC.Name = "L3PC";
            this.L3PC.Size = new System.Drawing.Size(63, 66);
            this.L3PC.TabIndex = 52;
            this.L3PC.Text = "10";
            this.L3PC.UseVisualStyleBackColor = true;
            // 
            // L5PC
            // 
            this.L5PC.Location = new System.Drawing.Point(289, 402);
            this.L5PC.Name = "L5PC";
            this.L5PC.Size = new System.Drawing.Size(63, 66);
            this.L5PC.TabIndex = 51;
            this.L5PC.Text = "5";
            this.L5PC.UseVisualStyleBackColor = true;
            // 
            // L4PC
            // 
            this.L4PC.Location = new System.Drawing.Point(224, 402);
            this.L4PC.Name = "L4PC";
            this.L4PC.Size = new System.Drawing.Size(63, 66);
            this.L4PC.TabIndex = 50;
            this.L4PC.Text = "11";
            this.L4PC.UseVisualStyleBackColor = true;
            // 
            // L2PC
            // 
            this.L2PC.Location = new System.Drawing.Point(86, 402);
            this.L2PC.Name = "L2PC";
            this.L2PC.Size = new System.Drawing.Size(63, 66);
            this.L2PC.TabIndex = 49;
            this.L2PC.Text = "9";
            this.L2PC.UseVisualStyleBackColor = true;
            // 
            // L1PC
            // 
            this.L1PC.Location = new System.Drawing.Point(16, 402);
            this.L1PC.Name = "L1PC";
            this.L1PC.Size = new System.Drawing.Size(64, 66);
            this.L1PC.TabIndex = 48;
            this.L1PC.Text = "8";
            this.L1PC.UseVisualStyleBackColor = true;
            // 
            // lbLed1
            // 
            this.lbLed1.BackColor = System.Drawing.Color.Transparent;
            this.lbLed1.BlinkInterval = 500;
            this.lbLed1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLed1.ForeColor = System.Drawing.Color.Black;
            this.lbLed1.Label = "";
            this.lbLed1.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Bottom;
            this.lbLed1.LedColor = System.Drawing.Color.Red;
            this.lbLed1.LedSize = new System.Drawing.SizeF(20F, 20F);
            this.lbLed1.Location = new System.Drawing.Point(39, 476);
            this.lbLed1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.lbLed1.Name = "lbLed1";
            this.lbLed1.Renderer = null;
            this.lbLed1.Size = new System.Drawing.Size(23, 25);
            this.lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed1.TabIndex = 89;
            // 
            // lbLed5
            // 
            this.lbLed5.BackColor = System.Drawing.Color.Transparent;
            this.lbLed5.BlinkInterval = 500;
            this.lbLed5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLed5.ForeColor = System.Drawing.Color.Black;
            this.lbLed5.Label = "";
            this.lbLed5.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Bottom;
            this.lbLed5.LedColor = System.Drawing.Color.Red;
            this.lbLed5.LedSize = new System.Drawing.SizeF(20F, 20F);
            this.lbLed5.Location = new System.Drawing.Point(308, 476);
            this.lbLed5.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.lbLed5.Name = "lbLed5";
            this.lbLed5.Renderer = null;
            this.lbLed5.Size = new System.Drawing.Size(23, 25);
            this.lbLed5.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed5.TabIndex = 93;
            // 
            // lbLed3
            // 
            this.lbLed3.BackColor = System.Drawing.Color.Transparent;
            this.lbLed3.BlinkInterval = 500;
            this.lbLed3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLed3.ForeColor = System.Drawing.Color.Black;
            this.lbLed3.Label = "";
            this.lbLed3.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Bottom;
            this.lbLed3.LedColor = System.Drawing.Color.Red;
            this.lbLed3.LedSize = new System.Drawing.SizeF(20F, 20F);
            this.lbLed3.Location = new System.Drawing.Point(172, 475);
            this.lbLed3.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.lbLed3.Name = "lbLed3";
            this.lbLed3.Renderer = null;
            this.lbLed3.Size = new System.Drawing.Size(23, 24);
            this.lbLed3.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed3.TabIndex = 91;
            // 
            // lbLed2
            // 
            this.lbLed2.BackColor = System.Drawing.Color.Transparent;
            this.lbLed2.BlinkInterval = 500;
            this.lbLed2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLed2.ForeColor = System.Drawing.Color.Black;
            this.lbLed2.Label = "";
            this.lbLed2.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Bottom;
            this.lbLed2.LedColor = System.Drawing.Color.Red;
            this.lbLed2.LedSize = new System.Drawing.SizeF(20F, 20F);
            this.lbLed2.Location = new System.Drawing.Point(105, 476);
            this.lbLed2.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.lbLed2.Name = "lbLed2";
            this.lbLed2.Renderer = null;
            this.lbLed2.Size = new System.Drawing.Size(23, 25);
            this.lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed2.TabIndex = 90;
            // 
            // lbLed4
            // 
            this.lbLed4.BackColor = System.Drawing.Color.Transparent;
            this.lbLed4.BlinkInterval = 500;
            this.lbLed4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLed4.ForeColor = System.Drawing.Color.Black;
            this.lbLed4.Label = "";
            this.lbLed4.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Bottom;
            this.lbLed4.LedColor = System.Drawing.Color.Red;
            this.lbLed4.LedSize = new System.Drawing.SizeF(20F, 20F);
            this.lbLed4.Location = new System.Drawing.Point(240, 475);
            this.lbLed4.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.lbLed4.Name = "lbLed4";
            this.lbLed4.Renderer = null;
            this.lbLed4.Size = new System.Drawing.Size(23, 25);
            this.lbLed4.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed4.TabIndex = 92;
            // 
            // LL2
            // 
            this.LL2.Location = new System.Drawing.Point(67, 411);
            this.LL2.Name = "LL2";
            this.LL2.Size = new System.Drawing.Size(64, 66);
            this.LL2.TabIndex = 46;
            this.LL2.Text = "LL2";
            this.LL2.UseVisualStyleBackColor = true;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.editFormUserControl1.SetBoundPropertyName(this.panelControl1, "");
            this.panelControl1.Controls.Add(this.editFormUserControl1);
            this.panelControl1.Controls.Add(this.LeftGen);
            this.panelControl1.Controls.Add(this.LeftCont);
            this.panelControl1.Controls.Add(this.LL2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(200, 525);
            this.panelControl1.TabIndex = 94;
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.LSCont);
            this.panelControl2.Controls.Add(this.L2PC);
            this.panelControl2.Controls.Add(this.label10);
            this.panelControl2.Controls.Add(this.primary_Current);
            this.panelControl2.Controls.Add(this.L1PC);
            this.panelControl2.Controls.Add(this.L4PC);
            this.panelControl2.Controls.Add(this.LftSSPC);
            this.panelControl2.Controls.Add(this.lbLed1);
            this.panelControl2.Controls.Add(this.L5PC);
            this.panelControl2.Controls.Add(this.lbLed4);
            this.panelControl2.Controls.Add(this.lbLed5);
            this.panelControl2.Controls.Add(this.lbLed2);
            this.panelControl2.Controls.Add(this.L3PC);
            this.panelControl2.Controls.Add(this.lbLed3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl2.Location = new System.Drawing.Point(190, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(371, 525);
            this.panelControl2.TabIndex = 95;
            // 
            // editFormUserControl1
            // 
            this.editFormUserControl1.Location = new System.Drawing.Point(143, 215);
            this.editFormUserControl1.Name = "editFormUserControl1";
            this.editFormUserControl1.Size = new System.Drawing.Size(928, 395);
            this.editFormUserControl1.TabIndex = 56;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.editFormUserControl1.SetBoundPropertyName(this, "");
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(561, 525);
            ((System.ComponentModel.ISupportInitialize)(this.digitalGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGauges.Win.GaugeControl primary_Current;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge digitalGauge1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent digitalBackgroundLayerComponent1;
        private System.Windows.Forms.Button LSCont;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button LftSSPC;
        private System.Windows.Forms.Button LeftGen;
        private System.Windows.Forms.Button LeftCont;
        private System.Windows.Forms.Button L3PC;
        private System.Windows.Forms.Button L5PC;
        private System.Windows.Forms.Button L4PC;
        private System.Windows.Forms.Button L2PC;
        private System.Windows.Forms.Button L1PC;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed5;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed3;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed2;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed4;
        private System.Windows.Forms.Button LL2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.Views.Grid.EditFormUserControl editFormUserControl1;

    }
}
