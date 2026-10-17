using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // Added for advanced drawing paths
using System.Text;
using System.Windows.Forms;

namespace Jewels_app
{
    public partial class dahbaord : UserControl
    {
        public dahbaord()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int radius = 25; // កម្រិតកោងនៃជ្រុងប្រអប់
            int diameter = radius * 2;

            // ធ្វើឱ្យបន្ទាត់គូរចេញមកម៉ត់រលោង
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);                             // ជ្រុងលើ-ឆ្វេង
                path.AddArc(p.Width - diameter - 1, 0, diameter, diameter, 270, 90);         // ជ្រុងលើ-ស្តាំ
                path.AddArc(p.Width - diameter - 1, p.Height - diameter - 1, diameter, diameter, 0, 90); // ជ្រុងក្រោម-ស្តាំ
                path.AddArc(0, p.Height - diameter - 1, diameter, diameter, 90, 90);           // ជ្រុងក្រោម-ឆ្វេង
                path.CloseAllFigures();

                // កាត់ប្រអប់ឱ្យមានរាងមូលតាមជ្រុង
                p.Region = new Region(path);

                // បន្ថែមបន្ទាត់ស្រមោលស្តើងៗ និងព្រាលជុំវិញប្រអប់ (Elegant Soft Border instead of harsh shadow)
                using (Pen shadowPen = new Pen(Color.FromArgb(35, 0, 0, 0), 2))
                {
                    e.Graphics.DrawPath(shadowPen, path);
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int radius = 5; // កម្រិតកោងនៃជ្រុងប្រអប់
            int diameter = radius * 2;

            // ធ្វើឱ្យបន្ទាត់គូរចេញមកម៉ត់រលោង
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);                             // ជ្រុងលើ-ឆ្វេង
                path.AddArc(p.Width - diameter - 1, 0, diameter, diameter, 270, 90);         // ជ្រុងលើ-ស្តាំ
                path.AddArc(p.Width - diameter - 1, p.Height - diameter - 1, diameter, diameter, 0, 90); // ជ្រុងក្រោម-ស្តាំ
                path.AddArc(0, p.Height - diameter - 1, diameter, diameter, 90, 90);           // ជ្រុងក្រោម-ឆ្វេង
                path.CloseAllFigures();

                // កាត់ប្រអប់ឱ្យមានរាងមូលតាមជ្រុង
                p.Region = new Region(path);

                // បន្ថែមបន្ទាត់ស្រមោលស្តើងៗ និងព្រាលជុំវិញប្រអប់ (Elegant Soft Border instead of harsh shadow)
                using (Pen shadowPen = new Pen(Color.FromArgb(35, 0, 0, 0), 2))
                {
                    e.Graphics.DrawPath(shadowPen, path);
                }
            }
        }


        private void dahbaord_Load(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }
    }
}
