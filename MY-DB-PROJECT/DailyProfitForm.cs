using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class DailyProfitForm : Form
    {
        // Connection string - اپنے سرور کے مطابق تبدیل کریں
        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=grocerystore;Trusted_Connection=True;";
        // یا remote server: @"Server=YOUR_SERVER_NAME;Database=grocerystore;User Id=USERNAME;Password=PASSWORD;";

        public DailyProfitForm()
        {
            InitializeComponent();
        }

        // Form Load Event
        private void DailyProfitForm_Load(object sender, EventArgs e)
        {
            LoadDailyData(dtpDate.Value.Date);
        }

        // Date Changed Event
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDailyData(dtpDate.Value.Date);
        }

        // Refresh Button Click
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDailyData(dtpDate.Value.Date);
        }

        // Back Button Click
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Export Button Click
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        // Main method to load daily data
        private void LoadDailyData(DateTime selectedDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to get daily orders with customer information
                    string ordersQuery = @"
                        SELECT 
                            o.OrderID,
                            o.OrderDate,
                            o.CustomerName,
                            o.TotalAmount AS Revenue,
                            o.OrderStatus,
                            o.PaymentMethod
                        FROM Orders o
                        WHERE CAST(o.OrderDate AS DATE) = @ReportDate
                        ORDER BY o.OrderDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(ordersQuery, conn);
                    da.SelectCommand.Parameters.AddWithValue("@ReportDate", selectedDate);

                    DataTable dtOrders = new DataTable();
                    da.Fill(dtOrders);

                    // Bind to DataGridView
                    dgvDailyData.DataSource = dtOrders;

                    // Format DataGridView columns
                    FormatDataGridView();

                    // Calculate total revenue from completed orders
                    string revenueQuery = @"
                        SELECT 
                            ISNULL(SUM(TotalAmount), 0) AS DailyRevenue,
                            COUNT(*) AS OrderCount
                        FROM Orders 
                        WHERE CAST(OrderDate AS DATE) = @ReportDate 
                        AND OrderStatus IN ('Completed', 'Confirmed')";

                    SqlCommand revenueCmd = new SqlCommand(revenueQuery, conn);
                    revenueCmd.Parameters.AddWithValue("@ReportDate", selectedDate);

                    SqlDataReader revenueReader = revenueCmd.ExecuteReader();
                    decimal dailyRevenue = 0;

                    if (revenueReader.Read())
                    {
                        dailyRevenue = Convert.ToDecimal(revenueReader["DailyRevenue"]);
                        // Note: lblOrderCount is not available in your design, so we removed it
                    }
                    revenueReader.Close();

                    // Get expenses from ProfitReports table
                    string expensesQuery = @"
                        SELECT ISNULL(SUM(Expenses), 0) AS DailyExpenses
                        FROM ProfitReports 
                        WHERE ReportType = 'Daily' 
                        AND CAST(ReportDate AS DATE) = @ReportDate";

                    decimal dailyExpenses = 0;
                    SqlCommand expensesCmd = new SqlCommand(expensesQuery, conn);
                    expensesCmd.Parameters.AddWithValue("@ReportDate", selectedDate);

                    object expensesResult = expensesCmd.ExecuteScalar();
                    if (expensesResult != DBNull.Value && expensesResult != null)
                    {
                        dailyExpenses = Convert.ToDecimal(expensesResult);
                    }

                    // If no expenses in ProfitReports, calculate estimated expenses (70% of revenue)
                    if (dailyExpenses == 0 && dailyRevenue > 0)
                    {
                        dailyExpenses = dailyRevenue * 0.7m; // 70% cost assumption
                    }

                    // Calculate profit
                    decimal dailyProfit = dailyRevenue - dailyExpenses;

                    // Update labels with formatted values
                    lblDailyRevenue.Text = $"PKR {dailyRevenue:N2}";
                    lblDailyExpenses.Text = $"PKR {dailyExpenses:N2}";
                    lblDailyProfit.Text = $"PKR {dailyProfit:N2}";

                    // Update ProfitReports table
                    UpdateProfitReport(selectedDate, dailyRevenue, dailyExpenses, dailyProfit);

                    // Update date in title (if you want to show it)
                    // lblReportDate is not available in your design, so we removed it

                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading daily data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Format DataGridView columns
        private void FormatDataGridView()
        {
            if (dgvDailyData.Columns.Count > 0)
            {
                // Set column headers
                if (dgvDailyData.Columns.Contains("OrderID"))
                    dgvDailyData.Columns["OrderID"].HeaderText = "Order ID";

                if (dgvDailyData.Columns.Contains("OrderDate"))
                {
                    dgvDailyData.Columns["OrderDate"].HeaderText = "Order Date";
                    dgvDailyData.Columns["OrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                if (dgvDailyData.Columns.Contains("CustomerName"))
                    dgvDailyData.Columns["CustomerName"].HeaderText = "Customer";

                if (dgvDailyData.Columns.Contains("Revenue"))
                {
                    dgvDailyData.Columns["Revenue"].HeaderText = "Revenue (PKR)";
                    dgvDailyData.Columns["Revenue"].DefaultCellStyle.Format = "N2";
                    dgvDailyData.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvDailyData.Columns.Contains("OrderStatus"))
                    dgvDailyData.Columns["OrderStatus"].HeaderText = "Status";

                if (dgvDailyData.Columns.Contains("PaymentMethod"))
                    dgvDailyData.Columns["PaymentMethod"].HeaderText = "Payment";

                // Auto size columns
                dgvDailyData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Set row height
                dgvDailyData.RowTemplate.Height = 30;
            }
        }

        // Update ProfitReports table
        private void UpdateProfitReport(DateTime reportDate, decimal revenue, decimal expenses, decimal profit)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if record exists
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM ProfitReports 
                        WHERE ReportType = 'Daily' 
                        AND CAST(ReportDate AS DATE) = @ReportDate";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@ReportDate", reportDate);

                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (exists > 0)
                    {
                        // Update existing record
                        string updateQuery = @"
                            UPDATE ProfitReports 
                            SET 
                                Revenue = @Revenue,
                                Expenses = @Expenses
                            WHERE ReportType = 'Daily' 
                            AND CAST(ReportDate AS DATE) = @ReportDate";

                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@Revenue", revenue);
                        updateCmd.Parameters.AddWithValue("@Expenses", expenses);
                        updateCmd.Parameters.AddWithValue("@ReportDate", reportDate);

                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insert new record
                        string insertQuery = @"
                            INSERT INTO ProfitReports 
                                (ReportType, ReportDate, Revenue, Expenses)
                            VALUES 
                                ('Daily', @ReportDate, @Revenue, @Expenses)";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@ReportDate", reportDate);
                        insertCmd.Parameters.AddWithValue("@Revenue", revenue);
                        insertCmd.Parameters.AddWithValue("@Expenses", expenses);

                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show to user
                Console.WriteLine($"Error updating profit report: {ex.Message}");
            }
        }

        // Export to CSV/Excel
        private void ExportToExcel()
        {
            try
            {
                if (dgvDailyData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export!", "Export",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV file (*.csv)|*.csv",
                    Title = "Save Daily Report",
                    FileName = $"Daily_Profit_Report_{DateTime.Now:yyyyMMdd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = (DataTable)dgvDailyData.DataSource;
                    ExportToCSV(dt, saveFileDialog.FileName);

                    MessageBox.Show("Report exported successfully!", "Export Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Export to CSV
        private void ExportToCSV(DataTable dataTable, string filePath)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Write headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    sw.Write(dataTable.Columns[i].ColumnName);
                    if (i < dataTable.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.WriteLine();

                // Write rows
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        string value = row[i].ToString();

                        // Handle special characters for CSV
                        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }

                        sw.Write(value);
                        if (i < dataTable.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();
                }
            }
        }

        // Paint event for shadow panel (if needed)
        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom painting code if needed
        }

        // Add event handlers to designer code (if not already added)
        // یہ event handlers ڈیزائنر کوڈ میں شامل کریں اگر پہلے سے نہیں ہیں
        private void InitializeEventHandlers()
        {
            this.Load += new EventHandler(DailyProfitForm_Load);
            this.dtpDate.ValueChanged += new EventHandler(dtpDate_ValueChanged);
            this.btnRefresh.Click += new EventHandler(btnRefresh_Click);
            this.btnExport.Click += new EventHandler(btnExport_Click);
            this.btnBack.Click += new EventHandler(btnBack_Click);
        }
    }
}