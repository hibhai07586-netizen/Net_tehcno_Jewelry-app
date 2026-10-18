using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Jewels_app
{
    // IMPORTANT: the UserControl must be the FIRST class in this file
    public partial class dahbaord : UserControl
    {
        private Chart profitChart;
        private Panel customToolTipPanel;
        private Label lblToolTipDate;
        private Label lblToolTipCurrent;
        private Label lblToolTipLast;
        private Panel ordersCard;

        private readonly Dictionary<Panel, Size> _regionSizes = new Dictionary<Panel, Size>();

        public dahbaord()
        {
            InitializeComponent();
        }

        private void dahbaord_Load(object sender, EventArgs e)
        {
            if (profitChart == null)
                InitializeProfitChart();

            LoadBestSellingProducts();
            FillCategoryCard(panel16);
            BuildRecentOrdersCard();
        }

        // ---------------- KPI cards (pink / orange / yellow) ----------------
        private void FillKpiCards()
        {
            FillKpiCard(panel4, "Total Revenue", "$124,839", "12.5%", true);   // pink
            FillKpiCard(panel5, "Total Orders", "1,284", "8.2%", true);        // orange
            FillKpiCard(panel6, "New Customers", "96", "3.1%", false);         // yellow
        }

        private void FillKpiCard(Panel card, string title, string value, string change, bool isUp)
        {
            card.Controls.Clear();

            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(20, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 70),
                BackColor = Color.Transparent
            };

            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(20, 38),
                AutoSize = true,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                BackColor = Color.Transparent
            };

            Label lblChange = new Label
            {
                Text = (isUp ? "▲ " : "▼ ") + change + " vs last month",
                Location = new Point(20, 82),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = isUp ? Color.SeaGreen : Color.Firebrick,
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblChange);
        }

        // ---------------- Sales by category (panel16) ----------------
        private void Panel16_Paint(object sender, PaintEventArgs e)
            => PaintRoundedPanel((Panel)sender, e, 20, 30);

        private void FillCategoryCard(Panel card)
        {
            card.Controls.Clear();

            // Rounded corners (safe to call more than once)
            card.Paint -= Panel16_Paint;
            card.Paint += Panel16_Paint;

            Label lbl = new Label
            {
                Text = "Sales by Category",
                Location = new Point(18, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                BackColor = Color.Transparent
            };

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AntiAliasing = AntiAliasingStyles.All
            };
            chart.ChartAreas.Add(new ChartArea("Donut")
            {
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent
            });

            Series donut = new Series("Category") { ChartType = SeriesChartType.Doughnut };
            donut["DoughnutRadius"] = "55";
            donut.IsValueShownAsLabel = true;
            donut.Label = "#PERCENT{P0}";          // show 38% instead of the name
            donut.LabelForeColor = Color.White;
            donut.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            string[] names = { "Rings", "Necklaces", "Earrings", "Bracelets", "Others" };
            double[] values = { 38, 24, 18, 12, 8 };
            Color[] colors =
            {
                Color.FromArgb(176, 141, 87),
                Color.FromArgb(214, 186, 140),
                Color.FromArgb(230, 100, 100),
                Color.FromArgb(24, 119, 242),
                Color.FromArgb(210, 215, 225)
            };
            for (int i = 0; i < names.Length; i++)
            {
                int idx = donut.Points.AddXY(names[i], values[i]);
                donut.Points[idx].Color = colors[i];
            }
            chart.Series.Add(donut);
            chart.Legends.Add(new Legend("L")
            {
                Docking = Docking.Bottom,
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f)
            });

            // Host panel leaves room for the title
            Panel host = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 40, 10, 10), BackColor = Color.Transparent };
            host.Controls.Add(chart);

            card.Controls.Add(host);
            card.Controls.Add(lbl);
            lbl.BringToFront();
        }

        // ---------------- Recent orders (under the donut) ----------------
        private void BuildRecentOrdersCard()
        {
            if (ordersCard != null) return; // avoid duplicates if Load fires again

            Control host = panel16.Parent;
            int top = panel16.Bottom + 20;

            // Try to line the bottom up with the Best Selling card; fall back to a fixed height
            int height = 260;
            Control bestCard = dataGridView1.Parent;
            if (bestCard != null && bestCard.Parent == host)
                height = Math.Max(200, bestCard.Bottom - top);

            ordersCard = new Panel
            {
                Bounds = new Rectangle(panel16.Left, top, panel16.Width, height),
                BackColor = Color.FromArgb(255, 247, 237)
            };
            ordersCard.Paint += (s, pe) => PaintRoundedPanel((Panel)s, pe, 20, 30);

            Label lbl = new Label
            {
                Text = "Recent Orders",
                Location = new Point(18, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                BackColor = Color.Transparent
            };

            DataTable dt = new DataTable();
            dt.Columns.Add("Order");
            dt.Columns.Add("Customer");
            dt.Columns.Add("Item");
            dt.Columns.Add("Total");
            dt.Columns.Add("Status");
            dt.Rows.Add("#1042", "Sokha Chan", "Diamond Ring", "$1,250", "Paid");
            dt.Rows.Add("#1041", "Dara Pich", "Pearl Earrings", "$840", "Pending");
            dt.Rows.Add("#1040", "Maly Sok", "Gold Necklace", "$2,300", "Paid");
            dt.Rows.Add("#1039", "Vannak Heng", "Silver Bracelet", "$410", "Shipped");
            dt.Rows.Add("#1038", "Srey Neang", "Jade Pendant", "$960", "Paid");

            DataGridView grid = new DataGridView
            {
                Location = new Point(14, 44),
                Size = new Size(ordersCard.Width - 28, ordersCard.Height - 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                DataSource = dt,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(245, 245, 245),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.DarkGray;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 235, 220);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.DataBindingComplete += (s, ev) =>
            {
                foreach (DataGridViewRow r in grid.Rows) r.Height = 32;
                grid.ClearSelection();
            };

            // Color the Status column
            grid.CellFormatting += (s, ev) =>
            {
                if (ev.ColumnIndex == 4 && ev.Value != null)
                {
                    string st = ev.Value.ToString();
                    ev.CellStyle.ForeColor =
                        st == "Paid" ? Color.SeaGreen :
                        st == "Pending" ? Color.DarkOrange :
                                          Color.FromArgb(24, 119, 242);
                }
            };

            ordersCard.Controls.Add(lbl);
            ordersCard.Controls.Add(grid);
            host.Controls.Add(ordersCard);
        }

        // ---------------- Best selling products grid ----------------
        private void LoadBestSellingProducts()
        {
            List<ProductItem> products = new List<ProductItem>()
            {
                new ProductItem { ID = "#83009", Name = "Hybrid Active Noise Cancelling Headphones", Sold = "2,310 sold", Revenue = "$124,839", Rating = "⭐ (5.0)" },
                new ProductItem { ID = "#83001", Name = "Casio G-Shock Shock Resistant Watch",       Sold = "1,230 sold", Revenue = "$92,662",  Rating = "⭐ (4.8)" },
                new ProductItem { ID = "#83004", Name = "SAMSUNG Galaxy S25 Ultra",                  Sold = "812 sold",   Revenue = "$74,048",  Rating = "⭐ (4.7)" },
                new ProductItem { ID = "#83002", Name = "Xbox Wireless Gaming Controller",           Sold = "645 sold",   Revenue = "$62,820",  Rating = "⭐ (4.5)" },
                new ProductItem { ID = "#83005", Name = "Timex Men's Easy Reader Watch",             Sold = "572 sold",   Revenue = "$48,724",  Rating = "⭐ (4.5)" }
            };

            dataGridView1.DataBindingComplete -= DataGridView1_DataBindingComplete;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = products;
            StyleProductGrid();
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                row.Height = 36;

            dataGridView1.ClearSelection();
        }

        private void StyleProductGrid()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = Color.FromArgb(245, 245, 245);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 235, 220);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridView1.Columns["ID"].FillWeight = 70;
            dataGridView1.Columns["Name"].FillWeight = 200;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.DarkGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        }

        // ---------------- Profit chart ----------------
        private void InitializeProfitChart()
        {
            profitChart = new Chart();
            profitChart.Dock = DockStyle.Fill;
            profitChart.BackColor = Color.FromArgb(255, 247, 237);
            profitChart.AntiAliasing = AntiAliasingStyles.All;

            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
            chartArea.AxisX.LineColor = Color.LightGray;
            chartArea.AxisY.LineColor = Color.Transparent;
            chartArea.AxisY.LabelStyle.Format = "{0}K";
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisX.LabelStyle.ForeColor = Color.Gray;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Gray;
            profitChart.ChartAreas.Add(chartArea);

            Series seriesCurrent = new Series("This Month")
            {
                ChartType = SeriesChartType.Spline,
                Color = Color.FromArgb(24, 119, 242),
                BorderWidth = 3,
                IsXValueIndexed = true
            };

            Series seriesLast = new Series("Last Month")
            {
                ChartType = SeriesChartType.Spline,
                Color = Color.FromArgb(210, 215, 225),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash,
                IsXValueIndexed = true
            };

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

            profitChart.Series.Add(seriesLast);
            profitChart.Series.Add(seriesCurrent);
            profitChart.Legends.Clear();

            panel13.Controls.Add(profitChart);

            // Custom hover popup
            customToolTipPanel = new Panel
            {
                Size = new Size(180, 75),
                BackColor = Color.White,
                Visible = false,
                BorderStyle = BorderStyle.None
            };
            customToolTipPanel.Paint += (s, pe) =>
            {
                Control c = (Control)s;
                using (Pen p = new Pen(Color.FromArgb(220, 224, 230), 1))
                    pe.Graphics.DrawRectangle(p, 0, 0, c.Width - 1, c.Height - 1);
            };

            lblToolTipDate = new Label { Location = new Point(10, 5), Size = new Size(160, 18), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50) };
            lblToolTipCurrent = new Label { Location = new Point(10, 25), Size = new Size(160, 18), Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(24, 119, 242) };
            lblToolTipLast = new Label { Location = new Point(10, 45), Size = new Size(160, 18), Font = new Font("Segoe UI", 9f), ForeColor = Color.Gray };

            customToolTipPanel.Controls.AddRange(new Control[] { lblToolTipDate, lblToolTipCurrent, lblToolTipLast });
            panel13.Controls.Add(customToolTipPanel);
            customToolTipPanel.BringToFront();

            profitChart.MouseMove += ProfitChart_MouseMove;
            profitChart.MouseLeave += (s, e) => customToolTipPanel.Visible = false;
        }

        private void ProfitChart_MouseMove(object sender, MouseEventArgs e)
        {
            Series current = profitChart.Series["This Month"];
            Series last = profitChart.Series["Last Month"];
            ChartArea area = profitChart.ChartAreas["MainArea"];

            int index;
            try
            {
                double xValue = area.AxisX.PixelPositionToValue(e.X);
                index = (int)Math.Round(xValue) - 1;
            }
            catch (ArgumentException)
            {
                customToolTipPanel.Visible = false;
                return;
            }

            if (index < 0 || index >= current.Points.Count || index >= last.Points.Count)
            {
                customToolTipPanel.Visible = false;
                return;
            }

            DataPoint pCurrent = current.Points[index];
            DataPoint pLast = last.Points[index];

            lblToolTipDate.Text = pCurrent.AxisLabel;
            lblToolTipCurrent.Text = $"—  ${pCurrent.YValues[0] * 1000:N0} this month";
            lblToolTipLast.Text = $"---  ${pLast.YValues[0] * 1000:N0} last month";

            int x = e.X + 15;
            int y = e.Y - 20;
            if (x + customToolTipPanel.Width > panel13.ClientSize.Width)
                x = e.X - customToolTipPanel.Width - 15;
            if (y < 0) y = 0;
            if (y + customToolTipPanel.Height > panel13.ClientSize.Height)
                y = panel13.ClientSize.Height - customToolTipPanel.Height;

            customToolTipPanel.Location = new Point(x, y);
            customToolTipPanel.Visible = true;
            customToolTipPanel.BringToFront();
        }

        // ---------------- Rounded panels ----------------
        private void panel13_Paint(object sender, PaintEventArgs e) => PaintRoundedPanel((Panel)sender, e, 20, 30);
        private void panel2_Paint(object sender, PaintEventArgs e) => PaintRoundedPanel((Panel)sender, e, 25, 35);
        private void panel3_Paint(object sender, PaintEventArgs e) => PaintRoundedPanel((Panel)sender, e, 5, 35);

        private static GraphicsPath CreateRoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void PaintRoundedPanel(Panel p, PaintEventArgs e, int radius, int borderAlpha)
        {
            if (p.Width < radius * 2 || p.Height < radius * 2) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, p.Width - 1, p.Height - 1), radius))
            {
                if (!_regionSizes.TryGetValue(p, out Size lastSize) || lastSize != p.Size)
                {
                    Region old = p.Region;
                    p.Region = new Region(path);
                    old?.Dispose();
                    _regionSizes[p] = p.Size;
                }

                using (Pen borderPen = new Pen(Color.FromArgb(borderAlpha, 0, 0, 0), 2))
                    e.Graphics.DrawPath(borderPen, path);
            }
        }

        // Empty handlers kept because the Designer may reference them
        private void label20_Click(object sender, EventArgs e) { }
        private void label20_Click_1(object sender, EventArgs e) { }
        private void panel14_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
    }

    // Data model: keep this AFTER the dahbaord class, in the same file
    public class ProductItem
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sold { get; set; } = string.Empty;
        public string Revenue { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
    }
}