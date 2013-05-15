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
            this.glControl1 = new OpenTK.GLControl();
            this.particleSimulation = new ParticleForm.DeflickerPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(400, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(600, 600);
            this.glControl1.TabIndex = 1;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // particleSimulation
            // 
            this.particleSimulation.BackColor = System.Drawing.Color.Black;
            this.particleSimulation.Location = new System.Drawing.Point(400, 0);
            this.particleSimulation.Name = "particleSimulation";
            this.particleSimulation.Size = new System.Drawing.Size(600, 600);
            this.particleSimulation.TabIndex = 0;
            this.particleSimulation.Visible = false;
            this.particleSimulation.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.particleSimulation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.particleSimulation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.particleSimulation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 191);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1000, 599);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.particleSimulation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private DeflickerPanel particleSimulation;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

