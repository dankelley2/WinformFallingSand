namespace Sand
{
    partial class SandboxForm
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
            this.Panel_Container = new System.Windows.Forms.Panel();
            this.ImgSandbox = new Sand.PictureBoxPixels();
            this.lblParticleCount = new System.Windows.Forms.Label();
            this.lblMsPerFrame = new System.Windows.Forms.Label();
            this.Panel_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgSandbox)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_Container
            // 
            this.Panel_Container.Controls.Add(this.ImgSandbox);
            this.Panel_Container.Location = new System.Drawing.Point(43, 58);
            this.Panel_Container.Name = "Panel_Container";
            this.Panel_Container.Size = new System.Drawing.Size(512, 512);
            this.Panel_Container.TabIndex = 0;
            // 
            // ImgSandbox
            // 
            this.ImgSandbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImgSandbox.Location = new System.Drawing.Point(0, 0);
            this.ImgSandbox.Name = "ImgSandbox";
            this.ImgSandbox.Size = new System.Drawing.Size(512, 512);
            this.ImgSandbox.TabIndex = 0;
            this.ImgSandbox.TabStop = false;
            this.ImgSandbox.MouseEnter += new System.EventHandler(this.ImgSandbox_MouseEnter);
            this.ImgSandbox.MouseLeave += new System.EventHandler(this.ImgSandbox_MouseLeave);
            this.ImgSandbox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImgSandbox_MouseMove);
            this.ImgSandbox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImgSandbox_MouseClick);
            this.ImgSandbox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImgSandbox_MouseUp);
            // 
            // lblParticleCount
            // 
            this.lblParticleCount.AutoSize = true;
            this.lblParticleCount.Location = new System.Drawing.Point(9, 13);
            this.lblParticleCount.Name = "lblParticleCount";
            this.lblParticleCount.Size = new System.Drawing.Size(100, 13);
            this.lblParticleCount.TabIndex = 1;
            this.lblParticleCount.Text = "Active Particles: {0}";
            // 
            // lblMsPerFrame
            // 
            this.lblMsPerFrame.AutoSize = true;
            this.lblMsPerFrame.Location = new System.Drawing.Point(121, 13);
            this.lblMsPerFrame.Name = "lblMsPerFrame";
            this.lblMsPerFrame.Size = new System.Drawing.Size(91, 13);
            this.lblMsPerFrame.TabIndex = 3;
            this.lblMsPerFrame.Text = "ms Per Frame: {0}";
            // 
            // SandboxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.lblMsPerFrame);
            this.Controls.Add(this.lblParticleCount);
            this.Controls.Add(this.Panel_Container);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "SandboxForm";
            this.Text = "Sandblockz";
            this.Load += new System.EventHandler(this.SandboxForm_Load);
            this.Panel_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImgSandbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Container;
        private Sand.PictureBoxPixels ImgSandbox;
        private System.Windows.Forms.Label lblParticleCount;
        private System.Windows.Forms.Label lblMsPerFrame;
    }
}

