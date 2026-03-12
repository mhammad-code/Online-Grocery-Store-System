using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace MY_DB_PROJECT
{
    public partial class MonthlyProfitForm : Form
    {
        // آپ کا connection string
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public MonthlyProfitForm()
        {
            InitializeComponent();
        }

        // Form Load Event
        private void MonthlyProfitForm_Load(object sender, EventArgs e)
        {
            LoadYearsComboBox();
            SetCurrentMonthYear();
            LoadMonthlyReport();
        }

        // Load years in combo box (last 5 years)
        private void LoadYearsComboBox()
        {
            try
            {
                cmbYear.Items.Clear();
                int currentYear = DateTime.Now.Year;

                for (int i = currentYear; i >= currentYear - 5; i--)
                {
                    cmbYear.Items.Add(i.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading years: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Set current month and year
        private void SetCurrentMonthYear()
        {
            try
            {
                int currentMonth = DateTime.Now.Month;
                string currentYear = DateTime.Now.Year.ToString();

                cmbMonth.SelectedIndex = currentMonth - 1; // Month index is 0-based

                // Find and select current year
                for (int i = 0; i < cmbYear.Items.Count; i++)
                {
                    if (cmbYear.Items[i].ToString() == currentYear)
                    {
                        cmbYear.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting current month/year: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Month selection changed
        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMonth.SelectedIndex >= 0 && cmbYear.SelectedIndex >= 0)
            {
                LoadMonthlyReport();
            }
        }

        // Year selection changed
        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMonth.SelectedIndex >= 0 && cmbYear.SelectedIndex >= 0)
            {
                LoadMonthlyReport();
            }
        }

        // Refresh button click
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMonthlyReport();
        }

        // Back button click
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Export button click
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        // Main method to load monthly report
        private void LoadMonthlyReport()
        {
            try
            {
                if (cmbMonth.SelectedIndex == -1 || cmbYear.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select both month and year", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedMonth = cmbMonth.SelectedIndex + 1;
                int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // =========================
                    // 1️⃣ Monthly Orders Data
                    // =========================
                    string ordersQuery = @"
                        SELECT 
                            o.OrderID,
                            o.OrderDate,
                            o.CustomerName,
                            o.TotalAmount AS Revenue,
                            o.OrderStatus,
                            o.PaymentMethod
                        FROM Orders o
                        WHERE MONTH(o.OrderDate) = @Month
                          AND YEAR(o.OrderDate) = @Year
                          AND o.OrderStatus IN ('Completed', 'Confirmed')
                        ORDER BY o.OrderDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(ordersQuery, con);
                    da.SelectCommand.Parameters.AddWithValue("@Month", selectedMonth);
                    da.SelectCommand.Parameters.AddWithValue("@Year", selectedYear);

                    DataTable dtOrders = new DataTable();
                    da.Fill(dtOrders);

                    dgvMonthlyData.DataSource = dtOrders;
                    FormatDataGridView();

                    // =========================
                    // 2️⃣ Monthly Revenue Calculation
                    // =========================
                    string revenueQuery = @"
                        SELECT 
                            ISNULL(SUM(TotalAmount), 0) AS MonthlyRevenue,
                            COUNT(*) AS OrderCount
                        FROM Orders
                        WHERE MONTH(OrderDate) = @Month
                          AND YEAR(OrderDate) = @Year
                          AND OrderStatus IN ('Completed', 'Confirmed')";

                    SqlCommand revenueCmd = new SqlCommand(revenueQuery, con);
                    revenueCmd.Parameters.AddWithValue("@Month", selectedMonth);
                    revenueCmd.Parameters.AddWithValue("@Year", selectedYear);

                    SqlDataReader revenueReader = revenueCmd.ExecuteReader();
                    decimal monthlyRevenue = 0;
                    int orderCount = 0;

                    if (revenueReader.Read())
                    {
                        monthlyRevenue = Convert.ToDecimal(revenueReader["MonthlyRevenue"]);
                        orderCount = Convert.ToInt32(revenueReader["OrderCount"]);
                    }
                    revenueReader.Close();

                    // =========================
                    // 3️⃣ Monthly Expenses Calculation
                    // =========================
                    decimal monthlyExpenses = 0;

                    // Try to get expenses from ProfitReports table
                    string expensesQuery = @"
                        SELECT ISNULL(SUM(Expenses), 0) AS MonthlyExpenses
                        FROM ProfitReports 
                        WHERE ReportType = 'Monthly' 
                        AND MONTH(ReportDate) = @Month 
                        AND YEAR(ReportDate) = @Year";

                    SqlCommand expensesCmd = new SqlCommand(expensesQuery, con);
                    expensesCmd.Parameters.AddWithValue("@Month", selectedMonth);
                    expensesCmd.Parameters.AddWithValue("@Year", selectedYear);

                    object expensesResult = expensesCmd.ExecuteScalar();
                    if (expensesResult != DBNull.Value && expensesResult != null)
                    {
                        monthlyExpenses = Convert.ToDecimal(expensesResult);
                    }

                    // If no expenses in ProfitReports, calculate estimated expenses (70% of revenue)
                    if (monthlyExpenses == 0 && monthlyRevenue > 0)
                    {
                        monthlyExpenses = monthlyRevenue * 0.7m; // 70% cost assumption
                    }

                    // =========================
                    // 4️⃣ Calculate Profit
                    // =========================
                    decimal monthlyProfit = monthlyRevenue - monthlyExpenses;

                    // =========================
                    // 5️⃣ Update Labels
                    // =========================
                    lblMonthlyRevenue.Text = $"PKR {monthlyRevenue:N2}";
                    lblMonthlyExpenses.Text = $"PKR {monthlyExpenses:N2}";
                    lblMonthlyProfit.Text = $"PKR {monthlyProfit:N2}";

                    // Update ProfitReports table
                    UpdateMonthlyProfitReport(selectedMonth, selectedYear, monthlyRevenue, monthlyExpenses, monthlyProfit);

                    // Update form title with month/year
                    label1.Text = $"📅 MONTHLY PROFIT - {cmbMonth.SelectedItem} {selectedYear} ({orderCount} Orders)";

                    con.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading monthly report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Format DataGridView
        private void FormatDataGridView()
        {
            if (dgvMonthlyData.Columns.Count > 0)
            {
                // Set column headers if they exist
                foreach (DataGridViewColumn column in dgvMonthlyData.Columns)
                {
                    switch (column.Name)
                    {
                        case "OrderID":
                            column.HeaderText = "Order ID";
                            break;
                        case "OrderDate":
                            column.HeaderText = "Order Date";
                            column.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                            break;
                        case "CustomerName":
                            column.HeaderText = "Customer";
                            break;
                        case "Revenue":
                            column.HeaderText = "Revenue (PKR)";
                            column.DefaultCellStyle.Format = "N2";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "OrderStatus":
                            column.HeaderText = "Status";
                            break;
                        case "PaymentMethod":
                            column.HeaderText = "Payment";
                            break;
                    }
                }

                // Auto size columns
                dgvMonthlyData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // Update or insert monthly profit report
        private void UpdateMonthlyProfitReport(int month, int year, decimal revenue, decimal expenses, decimal profit)
        {
            try
            {
                DateTime reportDate = new DateTime(year, month, 1);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if record exists
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM ProfitReports 
                        WHERE ReportType = 'Monthly' 
                        AND MONTH(ReportDate) = @Month 
                        AND YEAR(ReportDate) = @Year";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@Month", month);
                    checkCmd.Parameters.AddWithValue("@Year", year);

                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (exists > 0)
                    {
                        // Update existing record
                        string updateQuery = @"
                            UPDATE ProfitReports 
                            SET 
                                Revenue = @Revenue,
                                Expenses = @Expenses
                            WHERE ReportType = 'Monthly' 
                            AND MONTH(ReportDate) = @Month 
                            AND YEAR(ReportDate) = @Year";

                        SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                        updateCmd.Parameters.AddWithValue("@Revenue", revenue);
                        updateCmd.Parameters.AddWithValue("@Expenses", expenses);
                        updateCmd.Parameters.AddWithValue("@Month", month);
                        updateCmd.Parameters.AddWithValue("@Year", year);

                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insert new record
                        string insertQuery = @"
                            INSERT INTO ProfitReports 
                                (ReportType, ReportDate, Revenue, Expenses)
                            VALUES 
                                ('Monthly', @ReportDate, @Revenue, @Expenses)";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, con);
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
                System.Diagnostics.Debug.WriteLine($"Error updating monthly profit report: {ex.Message}");
            }
        }

        // Export to Excel/CSV
        private void ExportToExcel()
        {
            try
            {
                if (dgvMonthlyData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export!", "Export",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Save Monthly Report";
                saveFileDialog.FileName = $"Monthly_Profit_{cmbMonth.SelectedItem}_{cmbYear.SelectedItem}_{DateTime.Now:yyyyMMdd}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = (DataTable)dgvMonthlyData.DataSource;

                    if (saveFileDialog.FilterIndex == 1 || saveFileDialog.FileName.EndsWith(".csv")) // CSV
                    {
                        ExportToCSV(dt, saveFileDialog.FileName);
                    }
                    else // Excel (basic implementation)
                    {
                        ExportToExcelBasic(dt, saveFileDialog.FileName);
                    }

                    MessageBox.Show("Monthly report exported successfully!", "Export Complete",
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
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write report header
                sw.WriteLine($"Monthly Profit Report - {cmbMonth.SelectedItem} {cmbYear.SelectedItem}");
                sw.WriteLine($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}");
                sw.WriteLine($"Revenue: {lblMonthlyRevenue.Text}");
                sw.WriteLine($"Expenses: {lblMonthlyExpenses.Text}");
                sw.WriteLine($"Profit: {lblMonthlyProfit.Text}");
                sw.WriteLine(); // Empty line

                // Write data headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    sw.Write(dataTable.Columns[i].ColumnName);
                    if (i < dataTable.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.WriteLine();

                // Write data rows
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        string value = row[i].ToString();

                        // Handle commas and quotes in CSV
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

        // Basic Excel export (converts to CSV with .xlsx extension)
        private void ExportToExcelBasic(DataTable dataTable, string filePath)
        {
            // For Excel export, you might want to use a library like EPPlus
            // This is a simple CSV export with .xlsx extension
            if (filePath.EndsWith(".xlsx"))
            {
                filePath = filePath.Replace(".xlsx", ".csv");
            }
            ExportToCSV(dataTable, filePath);
        }

        // Get month name from month number (old style switch for compatibility)
        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return "Unknown";
            }
        }

        // Get month number from month name (old style switch for compatibility)
        private int GetMonthNumber(string monthName)
        {
            switch (monthName.ToLower())
            {
                case "january": return 1;
                case "february": return 2;
                case "march": return 3;
                case "april": return 4;
                case "may": return 5;
                case "june": return 6;
                case "july": return 7;
                case "august": return 8;
                case "september": return 9;
                case "october": return 10;
                case "november": return 11;
                case "december": return 12;
                default: return DateTime.Now.Month;
            }
        }

        // Generate sample data for testing (optional)
        private void GenerateSampleMonthlyData()
        {
            try
            {
                int month = cmbMonth.SelectedIndex + 1;
                int year = Convert.ToInt32(cmbYear.SelectedItem.ToString());
                DateTime reportDate = new DateTime(year, month, 1);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Insert sample profit report for selected month
                    string insertQuery = @"
                        IF NOT EXISTS (SELECT 1 FROM ProfitReports 
                                       WHERE ReportType='Monthly' 
                                       AND MONTH(ReportDate)=@Month 
                                       AND YEAR(ReportDate)=@Year)
                        BEGIN
                            INSERT INTO ProfitReports (ReportType, ReportDate, Revenue, Expenses)
                            VALUES ('Monthly', @ReportDate, 1500000.00, 900000.00);
                        END";

                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@ReportDate", reportDate);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Sample data added for {cmbMonth.SelectedItem} {year}!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh report
                    LoadMonthlyReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding sample data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Label click event
        private void label6_Click(object sender, EventArgs e)
        {
            // Optional: You can add functionality here
        }

        // Paint events (if needed)
        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom painting if needed
        }

        // DataGridView Cell Content Click
        private void dgvMonthlyData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click if needed
        }
    }
}