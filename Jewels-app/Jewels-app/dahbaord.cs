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
        // ---------------- Theme ----------------
        private static readonly Color CardColor = Color.FromArgb(255, 247, 237);
        private static readonly Color TitleColor = Color.FromArgb(60, 60, 60);
        private static readonly Color SelectionColor = Color.FromArgb(245, 235, 220);
        private static readonly Color Blue = Color.FromArgb(24, 119, 242);
        private static readonly Color LightGray = Color.FromArgb(210, 215, 225);

        // ---------------- Fields ----------------
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
            DoubleBuffered = true; // reduces flicker / repaint glitches
        }

        private void dahbaord_Load(object sender, EventArgs e)
        {
            if (profitChart == null)
                InitializeProfitChart();

            LoadBestSellingProducts();
            FillCategoryCard(panel16);
            BuildRecentOrdersCard();
        }

        // =====================================================================
        //  Shared helpers
        // =====================================================================
        private static Label CreateCardTitle(string text) => new Label
        {
            Text = text,
            Location = new Point(18, 14),
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = TitleColor,
            BackColor = Color.Transparent
        };

        private static void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(245, 245, 245);
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grid.DefaultCellStyle.SelectionBackColor = SelectionColor;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.DarkGray;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        }

        /// <summary>Sets every row height, clears the selection and shrinks the grid to its rows.</summary>
        private static void FitGridToRows(DataGridView grid, int rowHeight)
        {
            foreach (DataGridViewRow row in grid.Rows)
                row.Height = rowHeight;

            grid.ClearSelection();
            grid.Height = grid.ColumnHeadersHeight + grid.Rows.Count * rowHeight + 2;
        }

        // =====================================================================
        //  Best selling products
        // =====================================================================
        private void LoadBestSellingProducts()
        {
            var products = new List<ProductItem>
            {
                new ProductItem { ID = "#83009", Name = "Diamond Solitaire Ring",  Sold = "2,310 sold", Revenue = "$124,839", Rating = "⭐ (5.0)" },
                new ProductItem { ID = "#83001", Name = "18K Gold Necklace",       Sold = "1,230 sold", Revenue = "$92,662",  Rating = "⭐ (4.8)" },
                new ProductItem { ID = "#83004", Name = "Pearl Drop Earrings",     Sold = "812 sold",   Revenue = "$74,048",  Rating = "⭐ (4.7)" },
                new ProductItem { ID = "#83002", Name = "Silver Charm Bracelet",   Sold = "645 sold",   Revenue = "$62,820",  Rating = "⭐ (4.5)" },
                new ProductItem { ID = "#83005", Name = "Jade Pendant",            Sold = "572 sold",   Revenue = "$48,724",  Rating = "⭐ (4.5)" }
            };

            dataGridView1.DataBindingComplete -= DataGridView1_DataBindingComplete;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = products;

            StyleGrid(dataGridView1);
            dataGridView1.Columns["ID"].FillWeight = 70;
            dataGridView1.Columns["Name"].FillWeight = 200;
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
            => FitGridToRows(dataGridView1, 36);

        // =====================================================================
        //  Sales by category (panel16)
        // =====================================================================
        private void Panel16_Paint(object sender, PaintEventArgs e)
            => PaintRoundedPanel((Panel)sender, e, 20, 30);

        private void FillCategoryCard(Panel card)
        {
            card.Controls.Clear();
            card.BackColor = CardColor;

            card.Paint -= Panel16_Paint;
            card.Paint += Panel16_Paint;

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AntiAliasing = AntiAliasingStyles.All,
                BorderlineDashStyle = ChartDashStyle.NotSet,
                BorderlineWidth = 0
            };

            chart.ChartAreas.Add(new ChartArea("Donut")
            {
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent,
                BorderDashStyle = ChartDashStyle.NotSet,
                BorderWidth = 0
            });

            Series donut = new Series("Category")
            {
                ChartType = SeriesChartType.Doughnut,
                IsValueShownAsLabel = true,
                Label = "#PERCENT{P0}",       // percentage on the slice
                LegendText = "#VALX",         // name in the legend
                LabelForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            donut["DoughnutRadius"] = "55";

            string[] names = { "Rings", "Necklaces", "Earrings", "Bracelets", "Others" };
            double[] values = { 38, 24, 18, 12, 8 };
            Color[] colors =
            {
                Color.FromArgb(176, 141, 87),
                Color.FromArgb(214, 186, 140),
                Color.FromArgb(230, 100, 100),
                Blue,
                LightGray
            };

            for (int i = 0; i < names.Length; i++)
            {
                int idx = donut.Points.AddXY(names[i], values[i]);
                donut.Points[idx].Color = colors[i];
            }

            // Light slices need dark text to stay readable
            donut.Points[1].LabelForeColor = TitleColor;
            donut.Points[4].LabelForeColor = TitleColor;

            chart.Series.Add(donut);
            chart.Legends.Add(new Legend("L")
            {
                Docking = Docking.Bottom,
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f)
            });

            // Host panel leaves room for the title
            Panel host = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 40, 10, 10),
                BackColor = Color.Transparent
            };
            host.Controls.Add(chart);

            Label title = CreateCardTitle("Sales by Category");

            card.Controls.Add(host);
            card.Controls.Add(title);
            title.BringToFront();
        }

        // =====================================================================
        //  Recent orders (under the donut)
        // =====================================================================
        private void BuildRecentOrdersCard()
        {
            if (ordersCard != null) return; // avoid duplicates if Load fires again

            Control host = panel16.Parent;
            int top = panel16.Bottom + 20;

            // Line the bottom up with the Best Selling card when possible
            int height = 260;
            Control bestCard = dataGridView1.Parent;
            if (bestCard != null && bestCard.Parent == host)
                height = Math.Max(200, bestCard.Bottom - top);

            ordersCard = new Panel
            {
                Bounds = new Rectangle(panel16.Left, top, panel16.Width, height),
                BackColor = CardColor
            };
            ordersCard.Paint += (s, pe) => PaintRoundedPanel((Panel)s, pe, 20, 30);

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
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleGrid(grid);

            // Columns only exist after binding, so set widths here
            grid.DataBindingComplete += (s, ev) =>
            {
                grid.Columns["Order"].FillWeight = 60;
                grid.Columns["Customer"].FillWeight = 110;
                grid.Columns["Item"].FillWeight = 130;
                grid.Columns["Total"].FillWeight = 70;
                grid.Columns["Status"].FillWeight = 70;

                FitGridToRows(grid, 32);
            };

            // Color the Status column
            grid.CellFormatting += (s, ev) =>
            {
                if (ev.Value == null || grid.Columns[ev.ColumnIndex].Name != "Status") return;

                switch (ev.Value.ToString())
                {
                    case "Paid": ev.CellStyle.ForeColor = Color.SeaGreen; break;
                    case "Pending": ev.CellStyle.ForeColor = Color.DarkOrange; break;
                    default: ev.CellStyle.ForeColor = Blue; break;
                }
            };

            grid.DataSource = dt; // set last, after the events are hooked up

            ordersCard.Controls.Add(CreateCardTitle("Recent Orders"));
            ordersCard.Controls.Add(grid);
            host.Controls.Add(ordersCard);
        }

        // =====================================================================
        //  Profit chart (panel13)
        // =====================================================================
        private void InitializeProfitChart()
        {
            profitChart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = CardColor,
                AntiAliasing = AntiAliasingStyles.All
            };

            ChartArea area = new ChartArea("MainArea") { BackColor = Color.Transparent };
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
            area.AxisX.LineColor = Color.LightGray;
            area.AxisY.LineColor = Color.Transparent;
            area.AxisY.LabelStyle.Format = "{0}K";
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            area.AxisX.LabelStyle.ForeColor = Color.Gray;
            area.AxisY.LabelStyle.ForeColor = Color.Gray;
            profitChart.ChartAreas.Add(area);

            Series seriesCurrent = new Series("This Month")
            {
                ChartType = SeriesChartType.Spline,
                Color = Blue,
                BorderWidth = 3,
                IsXValueIndexed = true
            };

            Series seriesLast = new Series("Last Month")
            {
                ChartType = SeriesChartType.Spline,
                Color = LightGray,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash,
                IsXValueIndexed = true
            };

            string[] days = { "1 Jan", "8 Jan", "15 Jan", "22 Jan", "29 Jan" };
            double[] current = { 5.5, 5.0, 10.0, 7.5, 12.3 };
            double[] last = { 4.0, 4.5, 6.0, 5.2, 5.5 };

            for (int i = 0; i < days.Length; i++)
            {
                seriesCurrent.Points.AddXY(days[i], current[i]);
                seriesLast.Points.AddXY(days[i], last[i]);
            }

            profitChart.Series.Add(seriesLast);
            profitChart.Series.Add(seriesCurrent);
            profitChart.Legends.Clear();

            panel13.Controls.Add(profitChart);
            CreateChartTooltip();

            profitChart.MouseMove += ProfitChart_MouseMove;
            profitChart.MouseLeave += (s, e) => customToolTipPanel.Visible = false;
        }

        private void CreateChartTooltip()
        {
            customToolTipPanel = new Panel
            {
                Size = new Size(180, 75),
                BackColor = Color.White,
                Visible = false
            };
            customToolTipPanel.Paint += (s, pe) =>
            {
                Control c = (Control)s;
                using (Pen p = new Pen(Color.FromArgb(220, 224, 230), 1))
                    pe.Graphics.DrawRectangle(p, 0, 0, c.Width - 1, c.Height - 1);
            };

            lblToolTipDate = new Label { Location = new Point(10, 5), Size = new Size(160, 18), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50) };
            lblToolTipCurrent = new Label { Location = new Point(10, 25), Size = new Size(160, 18), Font = new Font("Segoe UI", 9f), ForeColor = Blue };
            lblToolTipLast = new Label { Location = new Point(10, 45), Size = new Size(160, 18), Font = new Font("Segoe UI", 9f), ForeColor = Color.Gray };

            customToolTipPanel.Controls.AddRange(new Control[] { lblToolTipDate, lblToolTipCurrent, lblToolTipLast });
            panel13.Controls.Add(customToolTipPanel);
            customToolTipPanel.BringToFront();
        }

        private void ProfitChart_MouseMove(object sender, MouseEventArgs e)
        {
            Series current = profitChart.Series["This Month"];
            Series last = profitChart.Series["Last Month"];
            ChartArea area = profitChart.ChartAreas["MainArea"];

            int index;
            try
            {
                index = (int)Math.Round(area.AxisX.PixelPositionToValue(e.X)) - 1;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                // Chart has not finished its first draw yet, so there is nothing to show
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

            // Keep the popup inside panel13
            int x = e.X + 15;
            int y = e.Y - 20;
            if (x + customToolTipPanel.Width > panel13.ClientSize.Width)
                x = e.X - customToolTipPanel.Width - 15;
            y = Math.Max(0, Math.Min(y, panel13.ClientSize.Height - customToolTipPanel.Height));

            customToolTipPanel.Location = new Point(x, y);
            customToolTipPanel.Visible = true;
            customToolTipPanel.BringToFront();
        }

        // =====================================================================
        //  Rounded panels
        // =====================================================================
        // Designer-wired Paint handlers (keep the names)
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
                // Rebuild the Region only when the size changed
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

        // =====================================================================
        //  Empty handlers: the Designer may still reference these.
        //  Delete one only if the build still succeeds without it.
        // =====================================================================
        private void label20_Click(object sender, EventArgs e) { }
        private void label20_Click_1(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
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