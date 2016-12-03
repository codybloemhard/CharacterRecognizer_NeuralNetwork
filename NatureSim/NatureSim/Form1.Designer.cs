namespace NatureSim
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
            this.ok = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.Panel();
            this.pcCanvas = new System.Windows.Forms.Panel();
            this.clear = new System.Windows.Forms.Button();
            this.Test = new System.Windows.Forms.Button();
            this.mission = new System.Windows.Forms.Label();
            this.learn = new System.Windows.Forms.Button();
            this.progressbar = new System.Windows.Forms.Panel();
            this.view = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok.Location = new System.Drawing.Point(0, 0);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(240, 100);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(0, 100);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(960, 940);
            this.canvas.TabIndex = 1;
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // pcCanvas
            // 
            this.pcCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pcCanvas.Location = new System.Drawing.Point(960, 100);
            this.pcCanvas.Name = "pcCanvas";
            this.pcCanvas.Size = new System.Drawing.Size(960, 940);
            this.pcCanvas.TabIndex = 2;
            this.pcCanvas.UseWaitCursor = true;
            // 
            // clear
            // 
            this.clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear.Location = new System.Drawing.Point(240, 0);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(240, 100);
            this.clear.TabIndex = 3;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // Test
            // 
            this.Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Test.Location = new System.Drawing.Point(960, 0);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(240, 100);
            this.Test.TabIndex = 4;
            this.Test.Text = "Test";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // mission
            // 
            this.mission.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mission.Location = new System.Drawing.Point(480, 0);
            this.mission.Name = "mission";
            this.mission.Size = new System.Drawing.Size(240, 100);
            this.mission.TabIndex = 5;
            this.mission.Text = "Please write: ";
            this.mission.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // learn
            // 
            this.learn.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.learn.Location = new System.Drawing.Point(720, 0);
            this.learn.Name = "learn";
            this.learn.Size = new System.Drawing.Size(240, 100);
            this.learn.TabIndex = 7;
            this.learn.Text = "Learn";
            this.learn.UseVisualStyleBackColor = true;
            this.learn.Click += new System.EventHandler(this.learn_Click);
            // 
            // progressbar
            // 
            this.progressbar.Location = new System.Drawing.Point(1440, 0);
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(480, 100);
            this.progressbar.TabIndex = 6;
            // 
            // view
            // 
            this.view.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.view.Location = new System.Drawing.Point(1200, 0);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(240, 100);
            this.view.TabIndex = 8;
            this.view.Text = "View Network";
            this.view.UseVisualStyleBackColor = true;
            this.view.Click += new System.EventHandler(this.view_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1041);
            this.Controls.Add(this.view);
            this.Controls.Add(this.learn);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.mission);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.pcCanvas);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.ok);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Panel canvas;
        private System.Windows.Forms.Panel pcCanvas;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Label mission;
        private System.Windows.Forms.Button learn;
        private System.Windows.Forms.Panel progressbar;
        private System.Windows.Forms.Button view;
    }
}

