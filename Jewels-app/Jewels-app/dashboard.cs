using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Jewels_app
{
    public partial class home : Form
    {
        private Panel activeMenuPanel = null;
        private Color activeMenuColor = Color.Gold;
        private Color inactiveMenuColor = Color.Yellow;

        public home()
        {
            InitializeComponent();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {
            // Add click event handlers to all menu panels
            panel4.Click += MenuPanel_Click;
            panel5.Click += MenuPanel_Click;
            panel6.Click += MenuPanel_Click;
            panel7.Click += MenuPanel_Click;
            panel8.Click += MenuPanel_Click;
            panel9.Click += MenuPanel_Click;

            // Make child controls also trigger the parent panel click
            AttachClickHandlerToChildren(panel4);
            AttachClickHandlerToChildren(panel5);
            AttachClickHandlerToChildren(panel6);
            AttachClickHandlerToChildren(panel7);
            AttachClickHandlerToChildren(panel8);
            AttachClickHandlerToChildren(panel9);

            // Set Dashboard as the default active menu
            SelectMenuPanel(panel4);
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
            // Get the parent panel if a child control is clicked
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
            // Reset previous active panel to inactive color
            if (activeMenuPanel != null)
            {
                activeMenuPanel.BackColor = inactiveMenuColor;
            }

            // Set the new active panel
            activeMenuPanel = menuPanel;
            activeMenuPanel.BackColor = activeMenuColor;

            // Handle navigation based on which panel was clicked
            HandleMenuNavigation(menuPanel);
        }

        private void HandleMenuNavigation(Panel menuPanel)
        {
            // Clear the layout panel
            Layout.Controls.Clear();

            // Load appropriate user control based on menu selection
            if (menuPanel == panel4)
            {
                // Dashboard menu item clicked
                dahbaord dashboardControl = new dahbaord();
                dashboardControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(dashboardControl);
            }
            else if (menuPanel == panel5)
            {
                // Product menu item clicked
                supplier productControl = new supplier();
                productControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(productControl);
            }
            else if (menuPanel == panel6)
            {
                // Customer menu item clicked
                customer customerControl = new customer();
                customerControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(customerControl);
            }
            else if (menuPanel == panel7)
            {
                // Customer Order menu item clicked
                customerOrder orderControl = new customerOrder();
                orderControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(orderControl);
            }
            else if (menuPanel == panel8)
            {
                // Report menu item clicked
                report reportControl = new report();
                reportControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(reportControl);
            }
            else if (menuPanel == panel9)
            {
                // Settings menu item clicked
                setting settingControl = new setting();
                settingControl.Dock = DockStyle.Fill;
                Layout.Controls.Add(settingControl);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Layout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
