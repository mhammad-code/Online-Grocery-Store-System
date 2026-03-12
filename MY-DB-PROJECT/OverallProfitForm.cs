using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace MY_DB_PROJECT
{
    public partial class OverallProfitForm : Form
    {
        // Updated connection string to match your database
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public OverallProfitForm()
        {
            InitializeComponent();
            LoadProfitData();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            // Set up column headers and formatting
            dgvProfitData.Columns.Clear();

            // Add columns with proper formatting
            dgvProfitData.Columns.Add("ReportID", "Report ID");
            dgvProfitData.Columns.Add("ReportType", "Report Type");
            dgvProfitData.Columns.Add("ReportDate", "Report Date");
            dgvProfitData.Columns.Add("Revenue", "Revenue (PKR)");
            dgvProfitData.Columns.Add("Expenses", "Expenses (PKR)");
            dgvProfitData.Columns.Add("Profit", "Profit (PKR)");

            // Set column widths
            dgvProfitData.Columns["ReportID"].Width = 80;
            dgvProfitData.Columns["ReportType"].Width = 120;
            dgvProfitData.Columns["ReportDate"].Width = 150;
            dgvProfitData.Columns["Revenue"].Width = 150;
            dgvProfitData.Columns["Expenses"].Width = 150;
            dgvProfitData.Columns["Profit"].Width = 150;

            // Format currency columns
            dgvProfitData.Columns["Revenue"].DefaultCellStyle.Format = "N2";
            dgvProfitData.Columns["Expenses"].DefaultCellStyle.Format = "N2";
            dgvProfitData.Columns["Profit"].DefaultCellStyle.Format = "N2";
            dgvProfitData.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProfitData.Columns["Expenses"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProfitData.Columns["Profit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void LoadProfitData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Query to get all profit reports from your ProfitReports table
                    string query = @"
                        SELECT 
                            ReportID,
                            ReportType,
                            CONVERT(varchar, ReportDate, 103) + ' ' + CONVERT(varchar, ReportDate, 108) as ReportDate,
                            Revenue,
                            Expenses,
                            Profit
                        FROM ProfitReports 
                        ORDER BY ReportDate DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Clear existing rows
                    dgvProfitData.Rows.Clear();

                    // Populate DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvProfitData.Rows.Add();
                        dgvProfitData.Rows[rowIndex].Cells["ReportID"].Value = row["ReportID"];
                        dgvProfitData.Rows[rowIndex].Cells["ReportType"].Value = row["ReportType"];
                        dgvProfitData.Rows[rowIndex].Cells["ReportDate"].Value = row["ReportDate"];
                        dgvProfitData.Rows[rowIndex].Cells["Revenue"].Value = Convert.ToDecimal(row["Revenue"]);
                        dgvProfitData.Rows[rowIndex].Cells["Expenses"].Value = Convert.ToDecimal(row["Expenses"]);
                        dgvProfitData.Rows[rowIndex].Cells["Profit"].Value = Convert.ToDecimal(row["Profit"]);

                        // Color code based on profit
                        decimal profit = Convert.ToDecimal(row["Profit"]);
                        if (profit < 0)
                        {
                            dgvProfitData.Rows[rowIndex].Cells["Profit"].Style.ForeColor = Color.Red;
                            dgvProfitData.Rows[rowIndex].Cells["Profit"].Style.Font = new Font(dgvProfitData.Font, FontStyle.Bold);
                        }
                        else if (profit > 0)
                        {
                            dgvProfitData.Rows[rowIndex].Cells["Profit"].Style.ForeColor = Color.LimeGreen;
                            dgvProfitData.Rows[rowIndex].Cells["Profit"].Style.Font = new Font(dgvProfitData.Font, FontStyle.Bold);
                        }
                    }

                    // Calculate and display totals
                    CalculateTotals(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profit data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTotals(DataTable dt)
        {
            try
            {
                decimal totalRevenue = 0;
                decimal totalExpenses = 0;
                decimal totalProfit = 0;

                foreach (DataRow row in dt.Rows)
                {
                    totalRevenue += Convert.ToDecimal(row["Revenue"]);
                    totalExpenses += Convert.ToDecimal(row["Expenses"]);
                    totalProfit += Convert.ToDecimal(row["Profit"]);
                }

                // Update labels with formatted values
                lblTotalRevenue.Text = $"PKR {totalRevenue:N2}";
                lblTotalExpenses.Text = $"PKR {totalExpenses:N2}";
                lblTotalProfit.Text = $"PKR {totalProfit:N2}";

                // Color code total profit
                if (totalProfit < 0)
                {
                    lblTotalProfit.ForeColor = Color.Red;
                }
                else if (totalProfit > 0)
                {
                    lblTotalProfit.ForeColor = Color.LimeGreen;
                }
                else
                {
                    lblTotalProfit.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating totals: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Save Profit Report";
                sfd.FileName = $"ProfitReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (ExcelPackage package = new ExcelPackage())
                        {
                            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Profit Report");

                            // Title
                            ws.Cells[1, 1].Value = "Overall Profit Report";
                            ws.Cells[1, 1].Style.Font.Bold = true;
                            ws.Cells[1, 1].Style.Font.Size = 16;
                            ws.Cells[1, 1, 1, 6].Merge = true;
                            ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            // Date stamp
                            ws.Cells[2, 1].Value = $"Generated on: {DateTime.Now:dd-MMM-yyyy HH:mm}";
                            ws.Cells[2, 1, 2, 6].Merge = true;
                            ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[2, 1].Style.Font.Italic = true;

                            // Summary section
                            ws.Cells[4, 1].Value = "SUMMARY";
                            ws.Cells[4, 1].Style.Font.Bold = true;
                            ws.Cells[4, 1].Style.Font.Size = 14;
                            ws.Cells[4, 1, 4, 2].Merge = true;

                            ws.Cells[5, 1].Value = "Total Revenue:";
                            ws.Cells[5, 2].Value = lblTotalRevenue.Text;
                            ws.Cells[5, 2].Style.Font.Bold = true;
                            ws.Cells[5, 2].Style.Font.Color.SetColor(Color.DodgerBlue);

                            ws.Cells[6, 1].Value = "Total Expenses:";
                            ws.Cells[6, 2].Value = lblTotalExpenses.Text;
                            ws.Cells[6, 2].Style.Font.Bold = true;
                            ws.Cells[6, 2].Style.Font.Color.SetColor(Color.OrangeRed);

                            ws.Cells[7, 1].Value = "Total Profit:";
                            ws.Cells[7, 2].Value = lblTotalProfit.Text;
                            ws.Cells[7, 2].Style.Font.Bold = true;

                            // Data header
                            int startRow = 10;
                            ws.Cells[startRow, 1].Value = "Report ID";
                            ws.Cells[startRow, 2].Value = "Report Type";
                            ws.Cells[startRow, 3].Value = "Report Date";
                            ws.Cells[startRow, 4].Value = "Revenue (PKR)";
                            ws.Cells[startRow, 5].Value = "Expenses (PKR)";
                            ws.Cells[startRow, 6].Value = "Profit (PKR)";

                            // Format header
                            using (var range = ws.Cells[startRow, 1, startRow, 6])
                            {
                                range.Style.Font.Bold = true;
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(100, 88, 255));
                                range.Style.Font.Color.SetColor(Color.White);
                                range.Style.Border.BorderAround(ExcelBorderStyle.Thick);
                            }

                            // Add data rows
                            for (int i = 0; i < dgvProfitData.Rows.Count; i++)
                            {
                                int row = startRow + i + 1;
                                ws.Cells[row, 1].Value = dgvProfitData.Rows[i].Cells["ReportID"].Value;
                                ws.Cells[row, 2].Value = dgvProfitData.Rows[i].Cells["ReportType"].Value;
                                ws.Cells[row, 3].Value = dgvProfitData.Rows[i].Cells["ReportDate"].Value;
                                ws.Cells[row, 4].Value = dgvProfitData.Rows[i].Cells["Revenue"].Value;
                                ws.Cells[row, 5].Value = dgvProfitData.Rows[i].Cells["Expenses"].Value;
                                ws.Cells[row, 6].Value = dgvProfitData.Rows[i].Cells["Profit"].Value;

                                // Format currency cells
                                ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";

                                // Color profit cells
                                decimal profit = Convert.ToDecimal(dgvProfitData.Rows[i].Cells["Profit"].Value ?? 0);
                                if (profit < 0)
                                {
                                    ws.Cells[row, 6].Style.Font.Color.SetColor(Color.Red);
                                    ws.Cells[row, 6].Style.Font.Bold = true;
                                }
                                else if (profit > 0)
                                {
                                    ws.Cells[row, 6].Style.Font.Color.SetColor(Color.Green);
                                    ws.Cells[row, 6].Style.Font.Bold = true;
                                }

                                // Add borders
                                using (var range = ws.Cells[row, 1, row, 6])
                                {
                                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    range.Style.Border.Bottom.Color.SetColor(Color.LightGray);
                                }
                            }

                            // Auto-fit columns
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            // Save file
                            File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());

                            MessageBox.Show($"Profit report exported successfully!\nFile saved at: {sfd.FileName}",
                                "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Optional: Refresh button event handler if you add one
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProfitData();
            MessageBox.Show("Profit data refreshed successfully!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}