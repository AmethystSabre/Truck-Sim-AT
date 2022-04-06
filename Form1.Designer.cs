namespace ETS2_DualSenseAT_Mod
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusLbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.JobLbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AcceleratorFeedback = new System.Windows.Forms.CheckBox();
            this.BrakeFeedback = new System.Windows.Forms.CheckBox();
            this.LedAlarmsOnSpeedLimit = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TruckLightsIndicator = new System.Windows.Forms.CheckBox();
            this.BrakeRedLights = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.ForeColor = System.Drawing.Color.White;
            this.statusLbl.Location = new System.Drawing.Point(12, 165);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(89, 13);
            this.statusLbl.TabIndex = 0;
            this.statusLbl.Text = "Status: Unknown";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ETS2_DualSenseAT_Mod.Properties.Resources.capsule_616x353;
            this.pictureBox1.Location = new System.Drawing.Point(15, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(299, 147);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // JobLbl
            // 
            this.JobLbl.AutoSize = true;
            this.JobLbl.ForeColor = System.Drawing.Color.White;
            this.JobLbl.Location = new System.Drawing.Point(165, 165);
            this.JobLbl.Name = "JobLbl";
            this.JobLbl.Size = new System.Drawing.Size(63, 13);
            this.JobLbl.TabIndex = 2;
            this.JobLbl.Text = "Job Status: ";
            this.JobLbl.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BrakeRedLights);
            this.groupBox1.Controls.Add(this.TruckLightsIndicator);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LedAlarmsOnSpeedLimit);
            this.groupBox1.Controls.Add(this.BrakeFeedback);
            this.groupBox1.Controls.Add(this.AcceleratorFeedback);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(15, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 111);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mapped Features";
            // 
            // AcceleratorFeedback
            // 
            this.AcceleratorFeedback.AutoSize = true;
            this.AcceleratorFeedback.Checked = true;
            this.AcceleratorFeedback.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AcceleratorFeedback.Location = new System.Drawing.Point(6, 19);
            this.AcceleratorFeedback.Name = "AcceleratorFeedback";
            this.AcceleratorFeedback.Size = new System.Drawing.Size(146, 17);
            this.AcceleratorFeedback.TabIndex = 0;
            this.AcceleratorFeedback.Text = "Feedback on Accelerator";
            this.AcceleratorFeedback.UseVisualStyleBackColor = true;
            // 
            // BrakeFeedback
            // 
            this.BrakeFeedback.AutoSize = true;
            this.BrakeFeedback.Checked = true;
            this.BrakeFeedback.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BrakeFeedback.Location = new System.Drawing.Point(6, 36);
            this.BrakeFeedback.Name = "BrakeFeedback";
            this.BrakeFeedback.Size = new System.Drawing.Size(120, 17);
            this.BrakeFeedback.TabIndex = 1;
            this.BrakeFeedback.Text = "Feedback on Brake";
            this.BrakeFeedback.UseVisualStyleBackColor = true;
            // 
            // LedAlarmsOnSpeedLimit
            // 
            this.LedAlarmsOnSpeedLimit.AutoSize = true;
            this.LedAlarmsOnSpeedLimit.Checked = true;
            this.LedAlarmsOnSpeedLimit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LedAlarmsOnSpeedLimit.Location = new System.Drawing.Point(6, 53);
            this.LedAlarmsOnSpeedLimit.Name = "LedAlarmsOnSpeedLimit";
            this.LedAlarmsOnSpeedLimit.Size = new System.Drawing.Size(207, 17);
            this.LedAlarmsOnSpeedLimit.TabIndex = 2;
            this.LedAlarmsOnSpeedLimit.Text = "Alarm when exceeding highway speed";
            this.LedAlarmsOnSpeedLimit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(163, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Lights -> ";
            this.label1.Visible = false;
            // 
            // TruckLightsIndicator
            // 
            this.TruckLightsIndicator.AutoSize = true;
            this.TruckLightsIndicator.Checked = true;
            this.TruckLightsIndicator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TruckLightsIndicator.Location = new System.Drawing.Point(6, 70);
            this.TruckLightsIndicator.Name = "TruckLightsIndicator";
            this.TruckLightsIndicator.Size = new System.Drawing.Size(129, 17);
            this.TruckLightsIndicator.TabIndex = 5;
            this.TruckLightsIndicator.Text = "Truck Lights Indicator";
            this.TruckLightsIndicator.UseVisualStyleBackColor = true;
            // 
            // BrakeRedLights
            // 
            this.BrakeRedLights.AutoSize = true;
            this.BrakeRedLights.Checked = true;
            this.BrakeRedLights.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BrakeRedLights.Location = new System.Drawing.Point(6, 87);
            this.BrakeRedLights.Name = "BrakeRedLights";
            this.BrakeRedLights.Size = new System.Drawing.Size(103, 17);
            this.BrakeRedLights.TabIndex = 6;
            this.BrakeRedLights.Text = "Brake Red Light";
            this.BrakeRedLights.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(326, 304);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.JobLbl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusLbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(342, 223);
            this.Name = "Form1";
            this.Text = "Euro Truck Simulator 2 | DualSense AT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label JobLbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox AcceleratorFeedback;
        private System.Windows.Forms.CheckBox BrakeFeedback;
        private System.Windows.Forms.CheckBox LedAlarmsOnSpeedLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox TruckLightsIndicator;
        private System.Windows.Forms.CheckBox BrakeRedLights;
    }
}

