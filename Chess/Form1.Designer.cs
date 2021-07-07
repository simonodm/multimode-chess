
namespace Chess
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chessBoardControl1 = new Chess.ChessBoardControl();
            this.SuspendLayout();
            // 
            // chessBoardControl1
            // 
            this.chessBoardControl1.Location = new System.Drawing.Point(12, 12);
            this.chessBoardControl1.Name = "chessBoardControl1";
            this.chessBoardControl1.Size = new System.Drawing.Size(578, 578);
            this.chessBoardControl1.TabIndex = 0;
            this.chessBoardControl1.Text = "chessBoardControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(900, 602);
            this.Controls.Add(this.chessBoardControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ChessBoardControl chessBoardControl1;
    }
}

