namespace Jewels_app
{
    partial class customer
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
            supplier1 = new supplier();
            setting1 = new setting();
            report1 = new report();
            dahbaord1 = new dahbaord();
            dahbaord2 = new dahbaord();
            customerOrder1 = new customerOrder();
            dahbaord3 = new dahbaord();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(386, 242);
            label1.Name = "label1";
            label1.Size = new Size(89, 25);
            label1.TabIndex = 0;
            label1.Text = "Customer";
            // 
            // supplier1
            // 
            supplier1.Location = new Point(0, 0);
            supplier1.Name = "supplier1";
            supplier1.Size = new Size(1245, 716);
            supplier1.TabIndex = 1;
            // 
            // setting1
            // 
            setting1.Location = new Point(0, 0);
            setting1.Name = "setting1";
            setting1.Size = new Size(1282, 750);
            setting1.TabIndex = 2;
            // 
            // report1
            // 
            report1.Location = new Point(0, 0);
            report1.Name = "report1";
            report1.Size = new Size(998, 690);
            report1.TabIndex = 3;
            // 
            // dahbaord1
            // 
            dahbaord1.Location = new Point(0, 0);
            dahbaord1.Name = "dahbaord1";
            dahbaord1.Size = new Size(1900, 792);
            dahbaord1.TabIndex = 4;
            // 
            // dahbaord2
            // 
            dahbaord2.Location = new Point(0, 0);
            dahbaord2.Name = "dahbaord2";
            dahbaord2.Size = new Size(1900, 792);
            dahbaord2.TabIndex = 5;
            // 
            // customerOrder1
            // 
            customerOrder1.Location = new Point(0, 0);
            customerOrder1.Name = "customerOrder1";
            customerOrder1.Size = new Size(1168, 699);
            customerOrder1.TabIndex = 6;
            // 
            // dahbaord3
            // 
            dahbaord3.Location = new Point(0, 0);
            dahbaord3.Name = "dahbaord3";
            dahbaord3.Size = new Size(1900, 792);
            dahbaord3.TabIndex = 7;
            // 
            // customer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dahbaord3);
            Controls.Add(customerOrder1);
            Controls.Add(dahbaord2);
            Controls.Add(dahbaord1);
            Controls.Add(report1);
            Controls.Add(setting1);
            Controls.Add(supplier1);
            Controls.Add(label1);
            Name = "customer";
            Size = new Size(865, 475);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private supplier supplier1;
        private setting setting1;
        private report report1;
        private dahbaord dahbaord1;
        private dahbaord dahbaord2;
        private customerOrder customerOrder1;
        private dahbaord dahbaord3;
    }
}
