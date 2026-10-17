using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // Added for advanced drawing paths
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Added for the Chart control

namespace Jewels_app
{
    public partial class dahbaord : UserControl
    {
        private Chart profitChart;
        // Place this inside your partial class main body variables
        private Panel customToolTipPanel;
        private Label lblToolTipDate;
        private Label lblToolTipCurrent;
        private Label lblToolTipLast;


        public dahbaord()
        {
            InitializeComponent();
        }

        private void dahbaord_Load(object sender, EventArgs e)
        {
            InitializeProfitChart();

        }

        private void InitializeProfitChart()
        {
            // Create Chart instance programmatically
            profitChart = new Chart();
            profitChart.Dock = DockStyle.Fill;
            profitChart.BackColor = Color.FromArgb(255, 247, 237); // Matches your panel13 background color
            profitChart.AntiAliasing = AntiAliasingStyles.All;

            // Create Chart Area
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.BackColor = Color.Transparent;

            // Style X and Y Axes to match Picture 1 (Clean, subtle grids)
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
            chartArea.AxisX.LineColor = Color.LightGray;
            chartArea.AxisY.LineColor = Color.Transparent; // Hide vertical axis line
            chartArea.AxisY.LabelStyle.Format = "{0}K"; // Formatting like 5K, 10K, 15K

            // Font styles for axis labels
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisX.LabelStyle.ForeColor = Color.Gray;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Gray;

            profitChart.ChartAreas.Add(chartArea);

            // 1. Setup "This Month" Series (Solid Blue Smooth Line)
            Series seriesCurrent = new Series("This Month");
            seriesCurrent.ChartType = SeriesChartType.Spline;
            seriesCurrent.Color = Color.FromArgb(24, 119, 242); // Vibrant Blue
            seriesCurrent.BorderWidth = 3;
            seriesCurrent.IsXValueIndexed = true;

            // 2. Setup "Last Month" Series (Dashed Gray Smooth Line)
            Series seriesLast = new Series("Last Month");
            seriesLast.ChartType = SeriesChartType.Spline;
            seriesLast.Color = Color.FromArgb(210, 215, 225); // Soft Gray-Blue
            seriesLast.BorderWidth = 2;
            seriesLast.BorderDashStyle = ChartDashStyle.Dash;
            seriesLast.IsXValueIndexed = true;

            // Add sample data points based on Picture 1
            seriesCurrent.Points.AddXY("1 Jan", 5.5);
            seriesCurrent.Points.AddXY("8 Jan", 5.0);
            seriesCurrent.Points.AddXY("15 Jan", 10.0);
            seriesCurrent.Points.AddXY("22 Jan", 7.5);
            seriesCurrent.Points.AddXY("29 Jan", 12.3);

            seriesLast.Points.AddXY("1 Jan", 4.0);
            seriesLast.Points.AddXY("8 Jan", 4.5);
            seriesLast.Points.AddXY("15 Jan", 6.0);
            seriesLast.Points.AddXY("22 Jan", 5.2);
            seriesLast.Points.AddXY("29 Jan", 5.5);

            // Dynamic Tooltip when hovering over nodes
            //seriesCurrent.ToolTip = "#VALX\nThis Month: $#VALK";
            //seriesLast.ToolTip = "Last Month: $#VALK";

            profitChart.Series.Add(seriesLast);
            profitChart.Series.Add(seriesCurrent);

            // Clear default legends
            profitChart.Legends.Clear();

            // Append chart inside your panel13
            panel13.Controls.Add(profitChart);

            // --- Create Custom Hover Popup Layout ---
            customToolTipPanel = new Panel();
            customToolTipPanel.Size = new Size(240, 200);
            customToolTipPanel.BackColor = Color.White;
            customToolTipPanel.Visible = false; // Hidden by default
            customToolTipPanel.BorderStyle = BorderStyle.None;
            customToolTipPanel.Paint += (s, pe) =>
            {
                Control PaintControl = (Control)s;
                using (Pen p = new Pen(Color.FromArgb(220, 224, 230), 1))
                {
                    pe.Graphics.DrawRectangle(p, 0, 0, PaintControl.Width - 1, PaintControl.Height - 1);
                }
            };


            // Add soft shadow border style using painting or a standard label layout
            lblToolTipDate = new Label { Location = new Point(10, 5), Size = new Size(140, 18), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50) };
            lblToolTipCurrent = new Label { Location = new Point(10, 25), Size = new Size(140, 18), Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(24, 119, 242) };
            lblToolTipLast = new Label { Location = new Point(10, 45), Size = new Size(140, 18), Font = new Font("Segoe UI", 9f), ForeColor = Color.Gray };

            customToolTipPanel.Controls.AddRange(new Control[] { lblToolTipDate, lblToolTipCurrent, lblToolTipLast });
            panel13.Controls.Add(customToolTipPanel);
            customToolTipPanel.BringToFront(); // Force popup on top of chart line layers

            // Hook up the live tracking event handler
            profitChart.MouseMove += ProfitChart_MouseMove;

        }


        private void panel13_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int radius = 20; // Matches the elegant rounded curve layout of Picture 1
            int diameter = radius * 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(p.Width - diameter - 1, 0, diameter, diameter, 270, 90);
                path.AddArc(p.Width - diameter - 1, p.Height - diameter - 1, diameter, diameter, 0, 90);
                path.AddArc(0, p.Height - diameter - 1, diameter, diameter, 90, 90);
                path.CloseAllFigures();

                p.Region = new Region(path);

                // Elegant Soft Border around panel13
                using (Pen shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 2))
                {
                    e.Graphics.DrawPath(shadowPen, path);
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int radius = 25; // កម្រិតកោងនៃជ្រុងប្រអប់
            int diameter = radius * 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(p.Width - diameter - 1, 0, diameter, diameter, 270, 90);
                path.AddArc(p.Width - diameter - 1, p.Height - diameter - 1, diameter, diameter, 0, 90);
                path.AddArc(0, p.Height - diameter - 1, diameter, diameter, 90, 90);
                path.CloseAllFigures();

                p.Region = new Region(path);

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

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(p.Width - diameter - 1, 0, diameter, diameter, 270, 90);
                path.AddArc(p.Width - diameter - 1, p.Height - diameter - 1, diameter, diameter, 0, 90);
                path.AddArc(0, p.Height - diameter - 1, diameter, diameter, 90, 90);
                path.CloseAllFigures();

                p.Region = new Region(path);

                using (Pen shadowPen = new Pen(Color.FromArgb(35, 0, 0, 0), 2))
                {
                    e.Graphics.DrawPath(shadowPen, path);
                }
            }
        }
        private void ProfitChart_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult result = profitChart.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel)
            {
                int pointIndex = result.PointIndex;
                if (pointIndex >= 0)
                {
                    var pointCurrent = profitChart.Series["This Month"].Points[pointIndex];
                    var pointLast = profitChart.Series["Last Month"].Points[pointIndex];

                    // 1. Clean up the Date Text (Fixes the "Jan 22 Jan" duplicate bug)
                    lblToolTipDate.Text = pointCurrent.AxisLabel.Contains("Jan") ? pointCurrent.AxisLabel : "Jan " + pointCurrent.AxisLabel;

                    // 2. Format values cleanly as currency
                    lblToolTipCurrent.Text = $"—  ${pointCurrent.YValues[0] * 1000:N0} this month";
                    lblToolTipLast.Text = $"---  ${pointLast.YValues[0] * 1000:N0} last month";

                    // 3. Make the box wide enough so text doesn't cut off
                    customToolTipPanel.Size = new Size(180, 75);
                    lblToolTipDate.Width = 160;
                    lblToolTipCurrent.Width = 160;
                    lblToolTipLast.Width = 160;

                    // 4. Position it beautifully right next to your mouse cursor
                    customToolTipPanel.Location = new Point(e.X + 15, e.Y - 20);
                    customToolTipPanel.Visible = true;
                }
            }
            else
            {
                customToolTipPanel.Visible = false;
            }
        }



        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click_1(object sender, EventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
