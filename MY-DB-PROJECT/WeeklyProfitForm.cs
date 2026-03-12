using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class WeeklyProfitForm : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private DataTable weeklyData;
        private ToolTip toolTip1;

        public WeeklyProfitForm()
        {
            InitializeComponent();
            InitializeToolTips();
            SetupDataGridView();
            LoadWeekOptions();
            LoadCurrentWeekData();
        }

        private void InitializeToolTips()
        {
            toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            // Add tooltips
            toolTip1.SetToolTip(cmbWeek, "Select a week to view its profit data");
            toolTip1.SetToolTip(btnRefresh, "Refresh the current week's data");
            toolTip1.SetToolTip(btnExport, "Export data to Excel/CSV");
            toolTip1.SetToolTip(btnBack, "Return to previous screen");
            toolTip1.SetToolTip(guna2Button1, "Return to previous screen");
        }

        private void SetupDataGridView()
        {
            dgvWeeklyData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWeeklyData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWeeklyData.MultiSelect = false;
            dgvWeeklyData.ReadOnly = true;
            dgvWeeklyData.AllowUserToAddRows = false;
            dgvWeeklyData.AllowUserToDeleteRows = false;
            dgvWeeklyData.AllowUserToOrderColumns = false;
            dgvWeeklyData.RowHeadersVisible = false;
            dgvWeeklyData.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        }

        private void LoadWeekOptions()
        {
            // Generate week options for current month
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            cmbWeek.Items.Clear();

            int weekCount = 1;
            DateTime weekStart = firstDayOfMonth;

            while (weekStart <= lastDayOfMonth)
            {
                DateTime weekEnd = weekStart.AddDays(6);
                if (weekEnd > lastDayOfMonth)
                    weekEnd = lastDayOfMonth;

                string weekLabel = $"Week {weekCount} ({weekStart:dd MMM} - {weekEnd:dd MMM})";
                cmbWeek.Items.Add(weekLabel);

                weekStart = weekStart.AddDays(7);
                weekCount++;
            }

            // Select current week
            SelectCurrentWeek();
        }

        private void SelectCurrentWeek()
        {
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            int weekNumber = (today.Day - 1) / 7 + 1;

            if (weekNumber <= cmbWeek.Items.Count)
            {
                cmbWeek.SelectedIndex = weekNumber - 1;
            }
            else if (cmbWeek.Items.Count > 0)
            {
                cmbWeek.SelectedIndex = 0;
            }
        }

        private void LoadCurrentWeekData()
        {
            if (cmbWeek.SelectedIndex < 0)
                return;

            string selectedWeek = cmbWeek.SelectedItem.ToString();
            DateTime[] weekDates = ParseWeekDates(selectedWeek);

            if (weekDates != null)
            {
                LoadWeeklyData(weekDates[0], weekDates[1]);
            }
        }

        private DateTime[] ParseWeekDates(string weekLabel)
        {
            try
            {
                // Extract dates from week label like "Week 1 (01 Jan - 07 Jan)"
                string datePart = weekLabel.Split('(')[1].TrimEnd(')');
                string[] dateParts = datePart.Split('-');

                string startDateStr = dateParts[0].Trim() + " " + DateTime.Now.Year;
                string endDateStr = dateParts[1].Trim() + " " + DateTime.Now.Year;

                DateTime startDate = DateTime.ParseExact(startDateStr, "dd MMM yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(endDateStr, "dd MMM yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);

                return new DateTime[] { startDate, endDate };
            }
            catch
            {
                // If parsing fails, use current week
                DateTime today = DateTime.Today;
                DateTime startDate = today.AddDays(-(int)today.DayOfWeek + 1); // Monday
                DateTime endDate = startDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

                return new DateTime[] { startDate, endDate };
            }
        }

        private void LoadWeeklyData(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Get daily revenue from Orders
                    string revenueQuery = @"
                        SELECT 
                            CONVERT(date, OrderDate) as [Date],
                            SUM(TotalAmount) as Revenue,
                            0 as Expenses  -- We'll calculate expenses separately
                        FROM Orders 
                        WHERE OrderDate BETWEEN @StartDate AND @EndDate
                            AND OrderStatus = 'Completed'
                        GROUP BY CONVERT(date, OrderDate)
                        ORDER BY [Date]";

                    SqlCommand revenueCmd = new SqlCommand(revenueQuery, con);
                    revenueCmd.Parameters.AddWithValue("@StartDate", startDate);
                    revenueCmd.Parameters.AddWithValue("@EndDate", endDate);

                    SqlDataAdapter da = new SqlDataAdapter(revenueCmd);
                    weeklyData = new DataTable();
                    da.Fill(weeklyData);

                    // Calculate estimated expenses (30% of revenue for demo)
                    // In real scenario, you would have actual expenses data
                    foreach (DataRow row in weeklyData.Rows)
                    {
                        decimal revenue = Convert.ToDecimal(row["Revenue"]);
                        decimal expenses = revenue * 0.30m; // 30% expenses
                        row["Expenses"] = expenses;
                    }

                    // Add Profit column
                    weeklyData.Columns.Add("Profit", typeof(decimal));
                    weeklyData.Columns.Add("Profit Margin %", typeof(decimal));

                    foreach (DataRow row in weeklyData.Rows)
                    {
                        decimal revenue = Convert.ToDecimal(row["Revenue"]);
                        decimal expenses = Convert.ToDecimal(row["Expenses"]);
                        decimal profit = revenue - expenses;
                        decimal profitMargin = revenue > 0 ? (profit / revenue) * 100 : 0;

                        row["Profit"] = profit;
                        row["Profit Margin %"] = Math.Round(profitMargin, 2);
                    }

                    // Format date column
                    weeklyData.Columns["Date"].ColumnName = "📅 Date";
                    foreach (DataRow row in weeklyData.Rows)
                    {
                        DateTime date = Convert.ToDateTime(row["📅 Date"]);
                        row["📅 Date"] = date.ToString("ddd, dd MMM");
                    }

                    // Rename other columns
                    weeklyData.Columns["Revenue"].ColumnName = "💰 Revenue (PKR)";
                    weeklyData.Columns["Expenses"].ColumnName = "📉 Expenses (PKR)";
                    weeklyData.Columns["Profit"].ColumnName = "📈 Profit (PKR)";
                    weeklyData.Columns["Profit Margin %"].ColumnName = "📊 Profit Margin %";

                    // Bind to DataGridView
                    dgvWeeklyData.DataSource = weeklyData;

                    // Calculate weekly totals
                    CalculateWeeklyTotals();

                    // Apply cell formatting
                    ApplyCellFormatting();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}\n\n" +
                              "Note: This demo shows sample data.\n" +
                              "For real data, ensure Orders table has completed orders.",
                              "Database Info",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);

                // Load sample data for demo
                LoadSampleData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading weekly data: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            // Create sample data for demonstration
            weeklyData = new DataTable();
            weeklyData.Columns.Add("📅 Date", typeof(string));
            weeklyData.Columns.Add("💰 Revenue (PKR)", typeof(decimal));
            weeklyData.Columns.Add("📉 Expenses (PKR)", typeof(decimal));
            weeklyData.Columns.Add("📈 Profit (PKR)", typeof(decimal));
            weeklyData.Columns.Add("📊 Profit Margin %", typeof(decimal));

            string[] days = { "Mon, 01 Jan", "Tue, 02 Jan", "Wed, 03 Jan",
                            "Thu, 04 Jan", "Fri, 05 Jan", "Sat, 06 Jan", "Sun, 07 Jan" };

            Random rnd = new Random();

            for (int i = 0; i < days.Length; i++)
            {
                decimal revenue = rnd.Next(1500, 2500);
                decimal expenses = revenue * 0.30m;
                decimal profit = revenue - expenses;
                decimal margin = (profit / revenue) * 100;

                weeklyData.Rows.Add(days[i], revenue, expenses, profit, Math.Round(margin, 2));
            }

            dgvWeeklyData.DataSource = weeklyData;
            CalculateWeeklyTotals();
            ApplyCellFormatting();
        }

        private void CalculateWeeklyTotals()
        {
            if (weeklyData == null || weeklyData.Rows.Count == 0)
            {
                SetDefaultValues();
                return;
            }

            decimal totalRevenue = 0;
            decimal totalExpenses = 0;
            decimal totalProfit = 0;

            foreach (DataRow row in weeklyData.Rows)
            {
                totalRevenue += Convert.ToDecimal(row["💰 Revenue (PKR)"]);
                totalExpenses += Convert.ToDecimal(row["📉 Expenses (PKR)"]);
                totalProfit += Convert.ToDecimal(row["📈 Profit (PKR)"]);
            }

            // Update labels with formatted values
            lblWeeklyRevenue.Text = $"PKR {totalRevenue:N0}";
            lblWeeklyExpenses.Text = $"PKR {totalExpenses:N0}";
            lblWeeklyProfit.Text = $"PKR {totalProfit:N0}";

            // Calculate profit margin
            decimal profitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue) * 100 : 0;

            // Update week label with profit info
            if (cmbWeek.SelectedItem != null)
            {
                string weekText = cmbWeek.SelectedItem.ToString();
                label3.Text = $"📊 Weekly Profit ({Math.Round(profitMargin, 1)}% margin):";
            }

            // Color coding
            lblWeeklyRevenue.ForeColor = Color.DodgerBlue;
            lblWeeklyExpenses.ForeColor = Color.OrangeRed;
            lblWeeklyProfit.ForeColor = totalProfit >= 0 ? Color.LimeGreen : Color.Red;
        }

        private void SetDefaultValues()
        {
            lblWeeklyRevenue.Text = "PKR 0";
            lblWeeklyExpenses.Text = "PKR 0";
            lblWeeklyProfit.Text = "PKR 0";

            lblWeeklyRevenue.ForeColor = Color.Gray;
            lblWeeklyExpenses.ForeColor = Color.Gray;
            lblWeeklyProfit.ForeColor = Color.Gray;
        }

        private void ApplyCellFormatting()
        {
            foreach (DataGridViewRow row in dgvWeeklyData.Rows)
            {
                if (row.Cells["📈 Profit (PKR)"].Value != null)
                {
                    decimal profit = Convert.ToDecimal(row.Cells["📈 Profit (PKR)"].Value);

                    // Color code profit cells
                    if (profit > 0)
                    {
                        row.Cells["📈 Profit (PKR)"].Style.ForeColor = Color.LimeGreen;
                        row.Cells["📈 Profit (PKR)"].Style.Font = new Font(dgvWeeklyData.Font, FontStyle.Bold);
                    }
                    else if (profit < 0)
                    {
                        row.Cells["📈 Profit (PKR)"].Style.ForeColor = Color.Red;
                        row.Cells["📈 Profit (PKR)"].Style.Font = new Font(dgvWeeklyData.Font, FontStyle.Bold);
                    }

                    // Format revenue and expenses
                    if (row.Cells["💰 Revenue (PKR)"].Value != null)
                    {
                        decimal revenue = Convert.ToDecimal(row.Cells["💰 Revenue (PKR)"].Value);
                        row.Cells["💰 Revenue (PKR)"].Value = revenue.ToString("N0");
                    }

                    if (row.Cells["📉 Expenses (PKR)"].Value != null)
                    {
                        decimal expenses = Convert.ToDecimal(row.Cells["📉 Expenses (PKR)"].Value);
                        row.Cells["📉 Expenses (PKR)"].Value = expenses.ToString("N0");
                    }

                    if (row.Cells["📈 Profit (PKR)"].Value != null)
                    {
                        row.Cells["📈 Profit (PKR)"].Value = profit.ToString("N0");
                    }

                    if (row.Cells["📊 Profit Margin %"].Value != null)
                    {
                        decimal margin = Convert.ToDecimal(row.Cells["📊 Profit Margin %"].Value);
                        row.Cells["📊 Profit Margin %"].Value = margin.ToString("F1") + "%";

                        // Color code margin cells
                        if (margin >= 20)
                            row.Cells["📊 Profit Margin %"].Style.ForeColor = Color.LimeGreen;
                        else if (margin >= 10)
                            row.Cells["📊 Profit Margin %"].Style.ForeColor = Color.Orange;
                        else
                            row.Cells["📊 Profit Margin %"].Style.ForeColor = Color.OrangeRed;
                    }
                }
            }
        }

        private void cmbWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCurrentWeekData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Show refreshing animation
            btnRefresh.Text = "🔄 Refreshing...";
            btnRefresh.Enabled = false;
            Application.DoEvents();

            LoadCurrentWeekData();

            // Restore button
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Enabled = true;

            MessageBox.Show("Weekly profit data refreshed successfully!",
                          "Refresh Complete",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Export Weekly Profit Data";
                saveFileDialog.FileName = $"Weekly_Profit_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    if (filePath.EndsWith(".csv"))
                    {
                        ExportToCSV(filePath);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        ExportToExcelFile(filePath);
                    }

                    MessageBox.Show($"✅ Weekly profit data exported successfully!\n\n" +
                                  $"📁 File saved to:\n{filePath}",
                                  "Export Complete",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}",
                              "Export Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write header
                writer.WriteLine("Weekly Profit Report");
                writer.WriteLine($"Week: {cmbWeek.SelectedItem}");
                writer.WriteLine($"Generated: {DateTime.Now:dd-MMM-yyyy HH:mm}");
                writer.WriteLine();

                // Write data headers
                for (int i = 0; i < dgvWeeklyData.Columns.Count; i++)
                {
                    writer.Write(dgvWeeklyData.Columns[i].HeaderText);
                    if (i < dgvWeeklyData.Columns.Count - 1)
                        writer.Write(",");
                }
                writer.WriteLine();

                // Write data
                foreach (DataGridViewRow row in dgvWeeklyData.Rows)
                {
                    for (int i = 0; i < dgvWeeklyData.Columns.Count; i++)
                    {
                        var value = row.Cells[i].Value ?? "";
                        writer.Write($"\"{value.ToString().Replace("\"", "\"\"")}\"");
                        if (i < dgvWeeklyData.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                }

                // Write summary
                writer.WriteLine();
                writer.WriteLine("Weekly Summary:");
                writer.WriteLine($"Total Revenue: {lblWeeklyRevenue.Text}");
                writer.WriteLine($"Total Expenses: {lblWeeklyExpenses.Text}");
                writer.WriteLine($"Total Profit: {lblWeeklyProfit.Text}");
            }
        }

        private void ExportToExcelFile(string filePath)
        {
            // For Excel export, you would need to add a reference to Microsoft.Office.Interop.Excel
            // or use a library like EPPlus. Here's a CSV fallback:
            string csvPath = Path.ChangeExtension(filePath, ".csv");
            ExportToCSV(csvPath);

            MessageBox.Show($"Excel export requires additional setup.\n" +
                          $"Data has been saved as CSV instead:\n{csvPath}",
                          "Export Note",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ReturnToPrevious();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ReturnToPrevious();
        }

        private void ReturnToPrevious()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to go back?\n\n" +
                                                "Any unsaved changes will be lost.",
                                                "Confirm Back",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvWeeklyData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Center align numeric columns
                if (dgvWeeklyData.Columns[e.ColumnIndex].Name.Contains("PKR") ||
                    dgvWeeklyData.Columns[e.ColumnIndex].Name.Contains("%"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Center align date column
                if (dgvWeeklyData.Columns[e.ColumnIndex].Name.Contains("Date"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void dgvWeeklyData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvWeeklyData.Rows[e.RowIndex];

                string details = $"📅 Daily Profit Details\n\n" +
                               $"Date: {selectedRow.Cells["📅 Date"].Value}\n" +
                               $"Revenue: {selectedRow.Cells["💰 Revenue (PKR)"].Value}\n" +
                               $"Expenses: {selectedRow.Cells["📉 Expenses (PKR)"].Value}\n" +
                               $"Profit: {selectedRow.Cells["📈 Profit (PKR)"].Value}\n" +
                               $"Margin: {selectedRow.Cells["📊 Profit Margin %"].Value}";

                MessageBox.Show(details, "Daily Profit Details",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
        }

        private void WeeklyProfitForm_Load(object sender, EventArgs e)
        {
            // Add event handlers
            cmbWeek.SelectedIndexChanged += cmbWeek_SelectedIndexChanged;
            btnRefresh.Click += btnRefresh_Click;
            btnExport.Click += btnExport_Click;
            btnBack.Click += btnBack_Click;
            guna2Button1.Click += guna2Button1_Click;
            dgvWeeklyData.CellFormatting += dgvWeeklyData_CellFormatting;
            dgvWeeklyData.CellDoubleClick += dgvWeeklyData_CellDoubleClick;
        }

        private void WeeklyProfitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the Weekly Profit Report?",
                                                "Confirm Close",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        // Add keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnRefresh_Click(null, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.E))
            {
                btnExport_Click(null, null);
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                ReturnToPrevious();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Print functionality
        private void PrintWeeklyReport()
        {
            MessageBox.Show("Print functionality would be implemented here.\n" +
                          "You can use PrintDocument component for printing.",
                          "Print Report",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        // Generate PDF report
        private void GeneratePDFReport()
        {
            MessageBox.Show("PDF generation functionality would be implemented here.\n" +
                          "You can use libraries like iTextSharp or PDFSharp.",
                          "PDF Report",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        // Add right-click context menu
        private void dgvWeeklyData_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dgvWeeklyData.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dgvWeeklyData.ClearSelection();
                    dgvWeeklyData.Rows[currentMouseOverRow].Selected = true;

                    ContextMenuStrip menu = new ContextMenuStrip();

                    ToolStripMenuItem viewDetailsItem = new ToolStripMenuItem("👁️ View Details");
                    viewDetailsItem.Click += (s, ev) => dgvWeeklyData_CellDoubleClick(sender,
                        new DataGridViewCellEventArgs(0, currentMouseOverRow));

                    ToolStripMenuItem printItem = new ToolStripMenuItem("🖨️ Print Report");
                    printItem.Click += (s, ev) => PrintWeeklyReport();

                    ToolStripMenuItem pdfItem = new ToolStripMenuItem("📄 Generate PDF");
                    pdfItem.Click += (s, ev) => GeneratePDFReport();

                    menu.Items.Add(viewDetailsItem);
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add(printItem);
                    menu.Items.Add(pdfItem);

                    menu.Show(dgvWeeklyData, new Point(e.X, e.Y));
                }
            }
        }
    }
}