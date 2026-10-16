using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // បន្ថែមសម្រាប់ប្រើប្រាស់ GraphicsPath កាត់ជ្រុងមូល
using System.Text;
using System.Windows.Forms;

namespace Jewels_app
{
    public partial class home : Form
    {
        private Panel activeMenuPanel = null;

        // 🎨 ពណ៌លំនាំប្រណីត (Luxury Theme)
        private Color activeMenuColor = Color.FromArgb(197, 168, 128);   // ពណ៌មាសប្រណីតសម្រាប់ប៊ូតុងដែលចុច
        private Color inactiveMenuColor = Color.FromArgb(253, 251, 247); // ពណ៌សគ្រីមស្រទន់សម្រាប់ប៊ូតុងធម្មតា

        public home()
        {
            InitializeComponent();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {
            // បន្ថែម click event ទៅលើ panel ម៉ឺនុយទាំងអស់ (រួមទាំង panel16)
            panel4.Click += MenuPanel_Click;
            panel5.Click += MenuPanel_Click;
            panel6.Click += MenuPanel_Click;
            panel7.Click += MenuPanel_Click;
            panel8.Click += MenuPanel_Click;
            panel9.Click += MenuPanel_Click;
            panel16.Click += MenuPanel_Click; // ភ្ជាប់ Click Event រួចរាល់

            // ភ្ជាប់មុខងារកាត់ជ្រុងមូលទៅកាន់ម៉ឺនុយប៊ូតុងនីមួយៗ (រួមទាំង panel16)
            panel4.Paint += MenuPanel_Paint;
            panel5.Paint += MenuPanel_Paint;
            panel6.Paint += MenuPanel_Paint;
            panel7.Paint += MenuPanel_Paint;
            panel8.Paint += MenuPanel_Paint;
            panel9.Paint += MenuPanel_Paint;
            panel16.Paint += MenuPanel_Paint; // 💡 បន្ថែមត្រង់នេះដើម្បីឱ្យ Supplier កាច់ជ្រុងមូលដែរ

            // ធ្វើឱ្យ controls ខាងក្នុង panel ដើរតួជាអ្នកចុច panel ដែរ (រួមទាំង panel16)
            AttachClickHandlerToChildren(panel4);
            AttachClickHandlerToChildren(panel5);
            AttachClickHandlerToChildren(panel6);
            AttachClickHandlerToChildren(panel7);
            AttachClickHandlerToChildren(panel8);
            AttachClickHandlerToChildren(panel9);
            AttachClickHandlerToChildren(panel16); // 💡 បន្ថែមត្រង់នេះដើម្បីឱ្យចុចចំរូប ឬអក្សរ Supplier ក៏ដើរដែរ

            // កំណត់យក Dashboard ជាម៉ឺនុយដំបូងគេបង្អស់
            SelectMenuPanel(panel4);
        }

        // មុខងារគូរ និងកាត់ជ្រុងមូលស្វ័យប្រវត្តិសម្រាប់ម៉ឺនុយនៅលើ Sidebar
        private void MenuPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int radius = 12; // កម្រិតកោងនៃប៊ូតុងម៉ឺនុយ
            int diameter = radius * 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);                             // លើ-ឆ្វេង
                path.AddArc(p.Width - diameter - 1, 0, diameter, diameter, 270, 90);         // លើ-ស្តាំ
                path.AddArc(p.Width - diameter - 1, p.Height - diameter - 1, diameter, diameter, 0, 90); // ក្រោម-ស្តាំ
                path.AddArc(0, p.Height - diameter - 1, diameter, diameter, 90, 90);           // ក្រោម-ឆ្វេង
                path.CloseAllFigures();

                // កាត់ panel ឱ្យទៅជាជ្រុងមូល
                p.Region = new Region(path);
            }
        }

        private void AttachClickHandlerToChildren(Panel panel)
        {
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.Click += MenuPanel_Click;
            }
        }

        private void MenuPanel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            if (clickedPanel == null)
            {
                Control ctrl = sender as Control;
                if (ctrl != null && ctrl.Parent is Panel parent)
                {
                    clickedPanel = parent;
                }
            }

            if (clickedPanel != null)
            {
                SelectMenuPanel(clickedPanel);
            }
        }

        private void SelectMenuPanel(Panel menuPanel)
        {
            if (activeMenuPanel != null)
            {
                activeMenuPanel.BackColor = inactiveMenuColor;
                SetChildrenForeColor(activeMenuPanel, Color.Black);
                activeMenuPanel.Invalidate(); // បង្ខំឱ្យវាគូរផ្ទៃឡើងវិញដើម្បីកុំឱ្យជាប់ពណ៌ចាស់
            }

            activeMenuPanel = menuPanel;
            activeMenuPanel.BackColor = activeMenuColor;
            SetChildrenForeColor(activeMenuPanel, Color.White);
            activeMenuPanel.Invalidate(); // បង្ខំឱ្យវាគូរផ្ទៃឡើងវិញជាពណ៌មាសជ្រុងមូល

            HandleMenuNavigation(menuPanel);
        }

        private void SetChildrenForeColor(Panel panel, Color color)
        {
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.ForeColor = color;
            }
        }

        private void HandleMenuNavigation(Panel menuPanel)
        {
            Layout.Controls.Clear();

            if (menuPanel == panel4)
            {
                dahbaord dashboardControl = new dahbaord();
                dashboardControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(dashboardControl);
            }
            else if (menuPanel == panel5)
            {
                product productControl = new product();
                productControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(productControl);
            }
            else if (menuPanel == panel6)
            {
                customer customerControl = new customer();
                customerControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(customerControl);
            }
            else if (menuPanel == panel7)
            {
                customerOrder orderControl = new customerOrder();
                orderControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(orderControl);
            }
            else if (menuPanel == panel16) // 💡 រៀបចំលំដាប់លំដោយតាមរូបរាងអេក្រង់ (Supplier នៅពីលើ Report)
            {
                supplier supplierControl = new supplier();
                supplierControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(supplierControl);
            }
            else if (menuPanel == panel8)
            {
                report reportControl = new report();
                reportControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(reportControl);
            }
            else if (menuPanel == panel9)
            {
                setting settingControl = new setting();
                settingControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(settingControl);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void timer1_Tick(object sender, EventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void Layout_Paint(object sender, PaintEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel16_Paint(object sender, PaintEventArgs e) { } // ទុកចោលធម្មតា លែងមាន Error
    }
}
