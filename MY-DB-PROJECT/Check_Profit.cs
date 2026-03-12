using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class Check_Profit : Form
    {
        // Database connection string for grocerystore database
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        private string currentPeriod = "Overall"; // Track current period

        public Check_Profit()
        {
            InitializeComponent();
            SetupDataGridView();
            AttachButtonEvents();
        }

        private void SetupDataGridView()
        {
            // Configure DataGridView appearance
            dgvProfitData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProfitData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProfitData.ReadOnly = true;
            dgvProfitData.AllowUserToAddRows = false;
            dgvProfitData.AllowUserToDeleteRows = false;

            // Set column headers style
            dgvProfitData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProfitData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 50);
            dgvProfitData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProfitData.ColumnHeadersHeight = 40;
        }

        private void AttachButtonEvents()
        {
            // Attach click events to buttons
            btnOverallProfit.Click += (s, e) => { LoadProfitData("Overall"); HighlightButton(btnOverallProfit); };
            btnDailyProfit.Click += (s, e) => { LoadProfitData("Daily"); HighlightButton(btnDailyProfit); };
            btnWeeklyProfit.Click += (s, e) => { LoadProfitData("Weekly"); HighlightButton(btnWeeklyProfit); };
            btnMonthlyProfit.Click += (s, e) => { LoadProfitData("Monthly"); HighlightButton(btnMonthlyProfit); };

            // Attach back button event
            btnBack.Click += BtnBack_Click;
        }

        private void HighlightButton(Guna.UI2.WinForms.Guna2Button activeButton)
        {
            // Reset all buttons to original colors
            btnOverallProfit.FillColor = Color.FromArgb(33, 150, 243);   // Blue
            btnDailyProfit.FillColor = Color.FromArgb(244, 67, 54);     // Red
            btnWeeklyProfit.FillColor = Color.FromArgb(255, 152, 0);    // Orange
            btnMonthlyProfit.FillColor = Color.FromArgb(156, 39, 176);  // Purple

            // Highlight active button (make it darker)
            activeButton.FillColor = ControlPaint.Dark(activeButton.FillColor, 0.2f);
        }

        private void Check_Profit_Load(object sender, EventArgs e)
        {
            try
            {
                // Highlight Overall Profit button by default
                HighlightButton(btnOverallProfit);

                // Load overall profit data
                LoadProfitData("Overall");

                // Set form title with current date
                this.Text = $"Profit Report - {DateTime.Now.ToString("dd MMM yyyy")}";

                // Center form on screen
                this.CenterToScreen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profit form: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProfitData(string period)
        {
            currentPeriod = period;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Calculate profit based on selected period
                    CalculateProfit(con, period);

                    // Load detailed data for DataGridView
                    LoadDetailedData(con, period);
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Message.Contains("Invalid object name"))
                {
                    MessageBox.Show("Profit data tables not found!\n\nUsing sample data for demonstration.",
                                  "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSampleData();
                }
                else
                {
                    MessageBox.Show($"Database Error: {sqlEx.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateProfit(SqlConnection con, string period)
        {
            string periodCondition = GetPeriodCondition(period);

            // Calculate revenue from completed orders for the selected period
            string revenueQuery = $@"
                SELECT 
                    ISNULL(SUM(O.TotalAmount), 0) AS TotalRevenue
                FROM Orders O
                WHERE O.OrderStatus IN ('Completed', 'Confirmed')
                {periodCondition}";

            SqlCommand revenueCmd = new SqlCommand(revenueQuery, con);
            decimal revenue = Convert.ToDecimal(revenueCmd.ExecuteScalar());

            // Calculate expenses (60% of revenue as cost + fixed expenses)
            decimal expenses = (revenue * 0.6m) + 10000; // 60% cost + PKR 10,000 fixed

            // Calculate net profit and margin
            decimal netProfit = revenue - expenses;
            decimal profitMargin = revenue > 0 ? (netProfit / revenue) * 100 : 0;

            // Update labels with proper formatting
            lblRevenue.Text = $"PKR {revenue.ToString("N2")}";
            lblExpenses.Text = $"PKR {expenses.ToString("N2")}";
            lblNetProfit.Text = $"PKR {netProfit.ToString("N2")}";
            lblProfitMargin.Text = $"{profitMargin.ToString("0.00")}%";

            // Color code profit
            lblNetProfit.ForeColor = netProfit >= 0 ? Color.Green : Color.Red;
            lblProfitMargin.ForeColor = profitMargin >= 0 ? Color.Green : Color.Red;
        }

        private string GetPeriodCondition(string period)
        {
            switch (period)
            {
                case "Daily":
                    return "AND CONVERT(DATE, O.OrderDate) = CONVERT(DATE, GETDATE())";
                case "Weekly":
                    return "AND DATEPART(WEEK, O.OrderDate) = DATEPART(WEEK, GETDATE()) " +
                           "AND DATEPART(YEAR, O.OrderDate) = DATEPART(YEAR, GETDATE())";
                case "Monthly":
                    return "AND MONTH(O.OrderDate) = MONTH(GETDATE()) " +
                           "AND YEAR(O.OrderDate) = YEAR(GETDATE())";
                case "Overall":
                default:
                    return "";
            }
        }

        private void LoadDetailedData(SqlConnection con, string period)
        {
            try
            {
                string query = "";

                switch (period)
                {
                    case "Daily":
                        query = @"
                            SELECT 
                                CONVERT(VARCHAR, O.OrderDate, 108) AS Time,
                                O.OrderID,
                                O.TotalAmount AS Revenue,
                                O.TotalAmount * 0.6 AS Expenses,
                                O.TotalAmount * 0.4 AS Profit
                            FROM Orders O
                            WHERE O.OrderStatus IN ('Completed', 'Confirmed')
                            AND CONVERT(DATE, O.OrderDate) = CONVERT(DATE, GETDATE())
                            ORDER BY O.OrderDate DESC";
                        break;

                    case "Weekly":
                        query = @"
                            SELECT 
                                DATENAME(WEEKDAY, O.OrderDate) AS Day,
                                COUNT(O.OrderID) AS Orders,
                                SUM(O.TotalAmount) AS Revenue,
                                SUM(O.TotalAmount * 0.6) AS Expenses,
                                SUM(O.TotalAmount * 0.4) AS Profit
                            FROM Orders O
                            WHERE O.OrderStatus IN ('Completed', 'Confirmed')
                            AND DATEPART(WEEK, O.OrderDate) = DATEPART(WEEK, GETDATE())
                            AND DATEPART(YEAR, O.OrderDate) = DATEPART(YEAR, GETDATE())
                            GROUP BY DATENAME(WEEKDAY, O.OrderDate), DATEPART(WEEKDAY, O.OrderDate)
                            ORDER BY DATEPART(WEEKDAY, O.OrderDate)";
                        break;

                    case "Monthly":
                        query = @"
                            SELECT 
                                DAY(O.OrderDate) AS Day,
                                COUNT(O.OrderID) AS Orders,
                                SUM(O.TotalAmount) AS Revenue,
                                SUM(O.TotalAmount * 0.6) AS Expenses,
                                SUM(O.TotalAmount * 0.4) AS Profit
                            FROM Orders O
                            WHERE O.OrderStatus IN ('Completed', 'Confirmed')
                            AND MONTH(O.OrderDate) = MONTH(GETDATE())
                            AND YEAR(O.OrderDate) = YEAR(GETDATE())
                            GROUP BY DAY(O.OrderDate)
                            ORDER BY DAY(O.OrderDate)";
                        break;

                    case "Overall":
                    default:
                        query = @"
                            SELECT 
                                FORMAT(O.OrderDate, 'MMM yyyy') AS Month,
                                COUNT(O.OrderID) AS Orders,
                                SUM(O.TotalAmount) AS Revenue,
                                SUM(O.TotalAmount * 0.6) AS Expenses,
                                SUM(O.TotalAmount * 0.4) AS Profit
                            FROM Orders O
                            WHERE O.OrderStatus IN ('Completed', 'Confirmed')
                            GROUP BY FORMAT(O.OrderDate, 'yyyy-MM'), FORMAT(O.OrderDate, 'MMM yyyy')
                            ORDER BY FORMAT(O.OrderDate, 'yyyy-MM') DESC";
                        break;
                }

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProfitData.DataSource = dt;

                // Format columns
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading detailed data: {ex.Message}");
                GenerateSampleData(period);
            }
        }

        private void FormatDataGridView()
        {
            // Format currency columns
            string[] currencyColumns = { "Revenue", "Expenses", "Profit", "TotalAmount" };

            foreach (DataGridViewColumn column in dgvProfitData.Columns)
            {
                if (currencyColumns.Contains(column.Name))
                {
                    column.DefaultCellStyle.Format = "N2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    if (column.Name == "Profit")
                    {
                        column.DefaultCellStyle.ForeColor = Color.Green;
                        column.DefaultCellStyle.Font = new Font(dgvProfitData.Font, FontStyle.Bold);
                    }
                }

                // Center align other columns
                if (!currencyColumns.Contains(column.Name))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void GenerateSampleData(string period)
        {
            DataTable dt = new DataTable();
            Random rnd = new Random();

            switch (period)
            {
                case "Daily":
                    dt.Columns.Add("Time", typeof(string));
                    dt.Columns.Add("Revenue", typeof(decimal));
                    dt.Columns.Add("Profit", typeof(decimal));

                    for (int i = 0; i < 8; i++)
                    {
                        decimal revenue = rnd.Next(5000, 20000);
                        decimal profit = revenue * 0.4m;
                        dt.Rows.Add($"{9 + i}:00", revenue, profit);
                    }
                    break;

                case "Weekly":
                    dt.Columns.Add("Day", typeof(string));
                    dt.Columns.Add("Orders", typeof(int));
                    dt.Columns.Add("Revenue", typeof(decimal));
                    dt.Columns.Add("Profit", typeof(decimal));

                    string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                    foreach (string day in days)
                    {
                        int orders = rnd.Next(5, 20);
                        decimal revenue = orders * rnd.Next(1000, 5000);
                        decimal profit = revenue * 0.4m;
                        dt.Rows.Add(day, orders, revenue, profit);
                    }
                    break;

                case "Monthly":
                    dt.Columns.Add("Day", typeof(int));
                    dt.Columns.Add("Orders", typeof(int));
                    dt.Columns.Add("Revenue", typeof(decimal));
                    dt.Columns.Add("Profit", typeof(decimal));

                    for (int day = 1; day <= 30; day += 3)
                    {
                        int orders = rnd.Next(10, 30);
                        decimal revenue = orders * rnd.Next(800, 3000);
                        decimal profit = revenue * 0.4m;
                        dt.Rows.Add(day, orders, revenue, profit);
                    }
                    break;

                case "Overall":
                default:
                    dt.Columns.Add("Month", typeof(string));
                    dt.Columns.Add("Orders", typeof(int));
                    dt.Columns.Add("Revenue", typeof(decimal));
                    dt.Columns.Add("Profit", typeof(decimal));

                    for (int i = 5; i >= 0; i--)
                    {
                        DateTime month = DateTime.Now.AddMonths(-i);
                        int orders = rnd.Next(100, 300);
                        decimal revenue = orders * rnd.Next(1000, 4000);
                        decimal profit = revenue * 0.4m;
                        dt.Rows.Add(month.ToString("MMM yyyy"), orders, revenue, profit);
                    }
                    break;
            }

            dgvProfitData.DataSource = dt;
            FormatDataGridView();

            // Show info message only once
            if (currentPeriod == "Overall")
            {
                MessageBox.Show("Using sample profit data. Real profit data will appear when you have completed orders.",
                              "Sample Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadSampleData()
        {
            // Set sample values for labels
            lblRevenue.Text = "PKR 150,000.00";
            lblExpenses.Text = "PKR 90,000.00";
            lblNetProfit.Text = "PKR 60,000.00";
            lblProfitMargin.Text = "40.00%";

            lblNetProfit.ForeColor = Color.Green;
            lblProfitMargin.ForeColor = Color.Green;

            GenerateSampleData("Overall");
        }

        // Daily Profit button click (already attached in designer)
        private void btnDailyProfit_Click(object sender, EventArgs e)
        {
            // This event is already handled by AttachButtonEvents
        }

        // Refresh data (if you add a refresh button)
        private void RefreshData()
        {
            LoadProfitData(currentPeriod);
            MessageBox.Show($"Profit data refreshed for {currentPeriod} period!", "Refreshed",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Back button
        private void BtnBack_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Return to Admin Dashboard?", "Confirm Exit",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.Show();
                this.Close();
            }
        }

        // Keyboard shortcuts
        private void Check_Profit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                BtnBack_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                RefreshData();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                LoadProfitData("Overall");
                HighlightButton(btnOverallProfit);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                LoadProfitData("Daily");
                HighlightButton(btnDailyProfit);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                LoadProfitData("Weekly");
                HighlightButton(btnWeeklyProfit);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
            {
                LoadProfitData("Monthly");
                HighlightButton(btnMonthlyProfit);
                e.Handled = true;
            }
        }

        // Show detailed view when double-clicking a row
        private void dgvProfitData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvProfitData.Rows.Count)
            {
                DataGridViewRow row = dgvProfitData.Rows[e.RowIndex];

                string details = $"📊 Detailed View ({currentPeriod})\n\n";

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        details += $"{dgvProfitData.Columns[cell.ColumnIndex].HeaderText}: {cell.Value}\n";
                    }
                }

                MessageBox.Show(details, "Profit Details",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Export to Excel/PDF (if you add export button)
        private void ExportToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Export Profit Report";
                saveFileDialog.FileName = $"Profit_Report_{currentPeriod}_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Simple CSV export
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        // Write headers
                        for (int i = 0; i < dgvProfitData.Columns.Count; i++)
                        {
                            writer.Write(dgvProfitData.Columns[i].HeaderText);
                            if (i < dgvProfitData.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                        // Write data
                        foreach (DataGridViewRow row in dgvProfitData.Rows)
                        {
                            for (int i = 0; i < dgvProfitData.Columns.Count; i++)
                            {
                                writer.Write(row.Cells[i].Value?.ToString() ?? "");
                                if (i < dgvProfitData.Columns.Count - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show($"Report exported to: {saveFileDialog.FileName}", "Export Successful",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}