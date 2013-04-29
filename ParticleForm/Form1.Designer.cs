namespace ParticleForm
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.particleSimulation = new DeflickerPanel();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // particleSimulation
            // 
            this.particleSimulation.BackColor = System.Drawing.Color.Black;
            this.particleSimulation.Location = new System.Drawing.Point(400, 0);
            this.particleSimulation.Name = "particleSimulation";
            this.particleSimulation.Size = new System.Drawing.Size(600, 600);
            this.particleSimulation.TabIndex = 0;
            this.particleSimulation.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.particleSimulation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.particleSimulation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.particleSimulation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1000, 599);
            this.Controls.Add(this.particleSimulation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private DeflickerPanel particleSimulation;
    }
}

