namespace Jewels_app
{
    partial class dahbaord
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
            label1 = new Label();
            panel1 = new Panel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Historic", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(304, 240);
            label1.Name = "label1";
            label1.Size = new Size(311, 65);
            label1.TabIndex = 0;
            label1.Text = "DASHBAORD";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Lime;
            panel1.Location = new Point(225, 57);
            panel1.Name = "panel1";
            panel1.Size = new Size(300, 150);
            panel1.TabIndex = 1;
            // 
            // dahbaord
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "dahbaord";
            Size = new Size(1267, 528);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
    }
}
