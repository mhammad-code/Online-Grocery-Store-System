using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class ViewAllSuppliers : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private DataTable dtSuppliers;
        private ToolTip toolTip1;

        public ViewAllSuppliers()
        {
            InitializeComponent();
            InitializeToolTips();
            LoadAllSuppliers();
            SetupDataGridView();
            SetupSearchEvents();
        }

        private void InitializeToolTips()
        {
            toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            // Add tooltips
            toolTip1.SetToolTip(txtSearch, "Search suppliers by name, phone, email, or address");
            toolTip1.SetToolTip(btnRefresh, "Refresh suppliers list and clear search");
            toolTip1.SetToolTip(btnBack, "Return to previous screen");
            toolTip1.SetToolTip(dgvAllSuppliers, "Double-click a row to view supplier details");
        }

        private void SetupDataGridView()
        {
            dgvAllSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAllSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAllSuppliers.MultiSelect = false;
            dgvAllSuppliers.ReadOnly = true;
            dgvAllSuppliers.AllowUserToAddRows = false;
            dgvAllSuppliers.AllowUserToDeleteRows = false;
            dgvAllSuppliers.AllowUserToOrderColumns = false;
            dgvAllSuppliers.RowHeadersVisible = false;
            dgvAllSuppliers.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        }

        private void SetupSearchEvents()
        {
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
        }

        // Load all suppliers from database
        private void LoadAllSuppliers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"
                        SELECT 
                            SupplierID,
                            SupplierName,
                            Phone,
                            Email,
                            Address,
                            'Active' as Status  -- Adding status column
                        FROM Suppliers 
                        ORDER BY SupplierID DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    dtSuppliers = new DataTable();
                    da.Fill(dtSuppliers);

                    // Rename columns for better display
                    dtSuppliers.Columns["SupplierID"].ColumnName = "ID";
                    dtSuppliers.Columns["SupplierName"].ColumnName = "Supplier Name";
                    dtSuppliers.Columns["Phone"].ColumnName = "Contact No";

                    dgvAllSuppliers.DataSource = dtSuppliers;
                    lblTotalRecords.Text = $"📊 Total Suppliers: {dtSuppliers.Rows.Count}";
                    lblTotalRecords.ForeColor = Color.FromArgb(33, 150, 243); // Blue

                    // Apply row styling
                    ApplyRowColoring();

                    // Format columns
                    FormatDataGridViewColumns();

                    // Add row numbers if needed
                    AddRowNumbers();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}\n\n" +
                              "Please ensure:\n" +
                              "1. Database connection is correct\n" +
                              "2. Suppliers table exists\n" +
                              "3. SQL Server is running",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ApplyRowColoring()
        {
            foreach (DataGridViewRow row in dgvAllSuppliers.Rows)
            {
                // Alternate row colors for better readability
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                }

                // Highlight suppliers with email
                if (row.Cells["Email"].Value != null &&
                    !string.IsNullOrWhiteSpace(row.Cells["Email"].Value.ToString()))
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.DefaultCellStyle.Font = new Font(dgvAllSuppliers.Font, FontStyle.Bold);
                }
            }
        }

        private void FormatDataGridViewColumns()
        {
            if (dgvAllSuppliers.Columns.Count > 0)
            {
                // ID column
                if (dgvAllSuppliers.Columns.Contains("ID"))
                {
                    dgvAllSuppliers.Columns["ID"].Width = 60;
                    dgvAllSuppliers.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvAllSuppliers.Columns["ID"].HeaderText = "🆔 ID";
                }

                // Supplier Name column
                if (dgvAllSuppliers.Columns.Contains("Supplier Name"))
                {
                    dgvAllSuppliers.Columns["Supplier Name"].Width = 200;
                    dgvAllSuppliers.Columns["Supplier Name"].HeaderText = "🏢 Supplier Name";
                }

                // Contact No column
                if (dgvAllSuppliers.Columns.Contains("Contact No"))
                {
                    dgvAllSuppliers.Columns["Contact No"].Width = 150;
                    dgvAllSuppliers.Columns["Contact No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvAllSuppliers.Columns["Contact No"].HeaderText = "📞 Contact No";
                }

                // Email column
                if (dgvAllSuppliers.Columns.Contains("Email"))
                {
                    dgvAllSuppliers.Columns["Email"].Width = 220;
                    dgvAllSuppliers.Columns["Email"].HeaderText = "📧 Email";
                }

                // Address column
                if (dgvAllSuppliers.Columns.Contains("Address"))
                {
                    dgvAllSuppliers.Columns["Address"].Width = 300;
                    dgvAllSuppliers.Columns["Address"].HeaderText = "📍 Address";
                }

                // Status column
                if (dgvAllSuppliers.Columns.Contains("Status"))
                {
                    dgvAllSuppliers.Columns["Status"].Width = 100;
                    dgvAllSuppliers.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvAllSuppliers.Columns["Status"].HeaderText = "✅ Status";
                }
            }
        }

        private void AddRowNumbers()
        {
            foreach (DataGridViewRow row in dgvAllSuppliers.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        // Refresh button click
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Show refreshing animation
            btnRefresh.Text = "🔄 Refreshing...";
            btnRefresh.Enabled = false;
            Application.DoEvents();

            txtSearch.Clear();
            LoadAllSuppliers();

            // Restore button
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Enabled = true;

            MessageBox.Show("Suppliers list refreshed successfully!",
                          "Refresh Complete",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        // Back button click
        private void btnBack_Click(object sender, EventArgs e)
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

        // Search text changed event
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dtSuppliers == null)
                return;

            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                dgvAllSuppliers.DataSource = dtSuppliers;
                lblTotalRecords.Text = $"📊 Total Suppliers: {dtSuppliers.Rows.Count}";
                return;
            }

            // Create a filtered view
            DataView dv = dtSuppliers.DefaultView;

            // Build filter for all searchable columns
            string filter = string.Format(
                "([Supplier Name] LIKE '%{0}%' OR " +
                "[Contact No] LIKE '%{0}%' OR " +
                "[Email] LIKE '%{0}%' OR " +
                "[Address] LIKE '%{0}%')",
                searchText.Replace("'", "''"));

            dv.RowFilter = filter;
            dgvAllSuppliers.DataSource = dv;

            // Update count with search results
            int resultCount = dv.Count;
            lblTotalRecords.Text = $"🔍 Found: {resultCount} supplier(s)";

            if (resultCount == 0)
            {
                lblTotalRecords.ForeColor = Color.Red;
            }
            else
            {
                lblTotalRecords.ForeColor = Color.Green;
            }

            // Apply coloring to filtered results
            ApplyRowColoring();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtSearch.Clear();
                txtSearch.Focus();
            }
            else if (e.KeyCode == Keys.Enter && dgvAllSuppliers.Rows.Count > 0)
            {
                dgvAllSuppliers.Focus();
                if (dgvAllSuppliers.Rows.Count > 0)
                {
                    dgvAllSuppliers.Rows[0].Selected = true;
                }
            }
        }

        // Double-click to view supplier details
        private void dgvAllSuppliers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow selectedRow = dgvAllSuppliers.Rows[e.RowIndex];
                    int supplierID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    // Show supplier details
                    ShowSupplierDetails(selectedRow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error displaying details: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void ShowSupplierDetails(DataGridViewRow row)
        {
            string details = $"🏢 SUPPLIER DETAILS\n\n" +
                            $"🆔 ID: {row.Cells["ID"].Value}\n" +
                            $"🏢 Name: {row.Cells["Supplier Name"].Value}\n" +
                            $"📞 Contact: {row.Cells["Contact No"].Value}\n" +
                            $"📧 Email: {row.Cells["Email"].Value}\n" +
                            $"📍 Address: {row.Cells["Address"].Value}\n" +
                            $"✅ Status: {row.Cells["Status"].Value}\n\n" +
                            $"📅 Last Updated: {DateTime.Now:dd-MMM-yyyy HH:mm}";

            using (Form detailForm = new Form())
            {
                detailForm.Text = "Supplier Details";
                detailForm.Size = new Size(500, 300);
                detailForm.StartPosition = FormStartPosition.CenterParent;
                detailForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                detailForm.MaximizeBox = false;
                detailForm.MinimizeBox = false;

                TextBox txtDetails = new TextBox();
                txtDetails.Multiline = true;
                txtDetails.ReadOnly = true;
                txtDetails.Text = details;
                txtDetails.Font = new Font("Segoe UI", 10F);
                txtDetails.Dock = DockStyle.Fill;
                txtDetails.ScrollBars = ScrollBars.Vertical;
                txtDetails.BackColor = Color.White;

                Button btnClose = new Button();
                btnClose.Text = "Close";
                btnClose.Dock = DockStyle.Bottom;
                btnClose.Height = 40;
                btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                btnClose.BackColor = Color.FromArgb(33, 150, 243);
                btnClose.ForeColor = Color.White;
                btnClose.Click += (s, e) => detailForm.Close();

                detailForm.Controls.Add(txtDetails);
                detailForm.Controls.Add(btnClose);
                detailForm.ShowDialog();
            }
        }

        // Export to CSV functionality
        private void ExportToCSV()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Suppliers Data";
                saveFileDialog.FileName = $"Suppliers_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        // Write headers
                        for (int i = 0; i < dgvAllSuppliers.Columns.Count; i++)
                        {
                            writer.Write(dgvAllSuppliers.Columns[i].HeaderText.Replace(",", ""));
                            if (i < dgvAllSuppliers.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                        // Write data
                        foreach (DataGridViewRow row in dgvAllSuppliers.Rows)
                        {
                            for (int i = 0; i < dgvAllSuppliers.Columns.Count; i++)
                            {
                                var value = row.Cells[i].Value ?? "";
                                writer.Write($"\"{value.ToString().Replace("\"", "\"\"")}\"");
                                if (i < dgvAllSuppliers.Columns.Count - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show($"✅ Export successful!\n\n" +
                                  $"File saved to:\n{saveFileDialog.FileName}",
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

        // Context menu for right-click operations
        private void dgvAllSuppliers_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dgvAllSuppliers.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dgvAllSuppliers.ClearSelection();
                    dgvAllSuppliers.Rows[currentMouseOverRow].Selected = true;

                    ContextMenuStrip menu = new ContextMenuStrip();

                    ToolStripMenuItem viewDetailsItem = new ToolStripMenuItem("👁️ View Details");
                    viewDetailsItem.Click += (s, ev) => dgvAllSuppliers_CellDoubleClick(sender,
                        new DataGridViewCellEventArgs(0, currentMouseOverRow));

                    ToolStripMenuItem exportItem = new ToolStripMenuItem("📤 Export to CSV");
                    exportItem.Click += (s, ev) => ExportToCSV();

                    ToolStripMenuItem refreshItem = new ToolStripMenuItem("🔄 Refresh");
                    refreshItem.Click += btnRefresh_Click;

                    menu.Items.Add(viewDetailsItem);
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add(exportItem);
                    menu.Items.Add(refreshItem);

                    menu.Show(dgvAllSuppliers, new Point(e.X, e.Y));
                }
            }
        }

        // Form load event
        private void ViewAllSuppliers_Load(object sender, EventArgs e)
        {
            // Add event handlers
            dgvAllSuppliers.CellDoubleClick += dgvAllSuppliers_CellDoubleClick;
            dgvAllSuppliers.MouseClick += dgvAllSuppliers_MouseClick;
            btnRefresh.Click += btnRefresh_Click;
            btnBack.Click += btnBack_Click;

            // Set focus to search box
            txtSearch.Focus();
        }

        // Form closing event
        private void ViewAllSuppliers_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the Suppliers View?",
                                                "Confirm Close",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        // Cell formatting for better display
        private void dgvAllSuppliers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Email cells in blue
                if (dgvAllSuppliers.Columns[e.ColumnIndex].Name == "Email" && e.Value != null)
                {
                    string email = e.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        e.CellStyle.ForeColor = Color.Blue;
                        e.CellStyle.Font = new Font(dgvAllSuppliers.Font, FontStyle.Underline);
                    }
                }

                // Center align ID and Contact columns
                if (dgvAllSuppliers.Columns[e.ColumnIndex].Name == "ID" ||
                    dgvAllSuppliers.Columns[e.ColumnIndex].Name == "Contact No" ||
                    dgvAllSuppliers.Columns[e.ColumnIndex].Name == "Status")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
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
            else if (keyData == (Keys.Control | Keys.F))
            {
                txtSearch.Focus();
                txtSearch.SelectAll();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}