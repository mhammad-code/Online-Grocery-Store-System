using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class ViewAllStaff : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public ViewAllStaff()
        {
            InitializeComponent();
            LoadAllStaff();
            LoadStaffSummary();
            SetupDataGridView();
            LoadFilterOptions();
        }

        private void SetupDataGridView()
        {
            // Configure DataGridView appearance
            dgvAllStaff.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAllStaff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAllStaff.MultiSelect = false;
            dgvAllStaff.ReadOnly = true;
            dgvAllStaff.AllowUserToAddRows = false;
            dgvAllStaff.AllowUserToDeleteRows = false;
            dgvAllStaff.AllowUserToOrderColumns = false;
            dgvAllStaff.RowHeadersVisible = false;
        }

        private void LoadAllStaff()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"
                        SELECT 
                            StaffID,
                            FullName,
                            Role,
                            Email,
                            Phone,
                            Address,
                            CreatedAt,
                            'Active' as Status  -- Adding a default status column since your table doesn't have Status
                        FROM Staff 
                        ORDER BY StaffID DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Format the CreatedAt column
                    if (dt.Columns.Contains("CreatedAt"))
                    {
                        dt.Columns["CreatedAt"].ColumnName = "Join Date";
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["Join Date"] != DBNull.Value)
                            {
                                DateTime joinDate = Convert.ToDateTime(row["Join Date"]);
                                row["Join Date"] = joinDate.ToString("dd-MMM-yyyy");
                            }
                        }
                    }

                    // Rename columns for better display
                    dt.Columns["FullName"].ColumnName = "Full Name";
                    dt.Columns["StaffID"].ColumnName = "ID";

                    dgvAllStaff.DataSource = dt;

                    // Apply row coloring based on Role
                    ApplyRowColoring();

                    // Format columns
                    FormatDataGridViewColumns();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyRowColoring()
        {
            foreach (DataGridViewRow row in dgvAllStaff.Rows)
            {
                if (row.Cells["Role"].Value != null)
                {
                    string role = row.Cells["Role"].Value.ToString().ToLower();

                    if (role.Contains("manager") || role.Contains("admin"))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCyan;
                        row.DefaultCellStyle.Font = new Font(dgvAllStaff.Font, FontStyle.Bold);
                    }
                    else if (role.Contains("cashier"))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            }
        }

        private void FormatDataGridViewColumns()
        {
            if (dgvAllStaff.Columns.Count > 0)
            {
                // ID column
                if (dgvAllStaff.Columns.Contains("ID"))
                {
                    dgvAllStaff.Columns["ID"].Width = 50;
                    dgvAllStaff.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Full Name column
                if (dgvAllStaff.Columns.Contains("Full Name"))
                {
                    dgvAllStaff.Columns["Full Name"].Width = 150;
                }

                // Role column
                if (dgvAllStaff.Columns.Contains("Role"))
                {
                    dgvAllStaff.Columns["Role"].Width = 120;
                }

                // Email column
                if (dgvAllStaff.Columns.Contains("Email"))
                {
                    dgvAllStaff.Columns["Email"].Width = 180;
                }

                // Phone column
                if (dgvAllStaff.Columns.Contains("Phone"))
                {
                    dgvAllStaff.Columns["Phone"].Width = 120;
                    dgvAllStaff.Columns["Phone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Join Date column
                if (dgvAllStaff.Columns.Contains("Join Date"))
                {
                    dgvAllStaff.Columns["Join Date"].Width = 100;
                    dgvAllStaff.Columns["Join Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Address column
                if (dgvAllStaff.Columns.Contains("Address"))
                {
                    dgvAllStaff.Columns["Address"].Width = 200;
                }
            }
        }

        private void LoadStaffSummary()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Total Staff Count
                    SqlCommand cmdTotal = new SqlCommand("SELECT COUNT(*) FROM Staff", con);
                    int totalStaff = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    lblTotalStaff.Text = totalStaff.ToString();
                    lblTotalStaff.ForeColor = Color.FromArgb(33, 150, 243); // Blue

                    // Active Staff (Considering all as active for now)
                    lblActiveStaff.Text = totalStaff.ToString();
                    lblActiveStaff.ForeColor = Color.FromArgb(76, 175, 80); // Green

                    // Inactive Staff (0 since we don't have status column)
                    lblInactiveStaff.Text = "0";
                    lblInactiveStaff.ForeColor = Color.FromArgb(244, 67, 54); // Red

                    // On Leave Staff (0 since we don't have leave status)
                    lblOnLeave.Text = "0";
                    lblOnLeave.ForeColor = Color.FromArgb(255, 152, 0); // Orange

                    // Add tooltips
                    toolTip1.SetToolTip(lblTotalStaff, "Total number of staff members");
                    toolTip1.SetToolTip(lblActiveStaff, "Currently active staff members");
                    toolTip1.SetToolTip(lblInactiveStaff, "Inactive staff members");
                    toolTip1.SetToolTip(lblOnLeave, "Staff members currently on leave");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff summary: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFilterOptions()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Load unique roles for department filter
                    string roleQuery = "SELECT DISTINCT Role FROM Staff ORDER BY Role";
                    SqlCommand roleCmd = new SqlCommand(roleQuery, con);
                    SqlDataReader roleReader = roleCmd.ExecuteReader();

                    cmbDepartment.Items.Clear();
                    cmbDepartment.Items.Add("All Departments");

                    while (roleReader.Read())
                    {
                        string role = roleReader["Role"].ToString();
                        if (!string.IsNullOrEmpty(role))
                        {
                            cmbDepartment.Items.Add(role);
                        }
                    }
                    roleReader.Close();

                    // Set default selections
                    cmbDepartment.SelectedIndex = 0;
                    cmbStatus.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading filter options: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string selectedDepartment = cmbDepartment.SelectedItem?.ToString();
                    string selectedStatus = cmbStatus.SelectedItem?.ToString();

                    string query = @"
                        SELECT 
                            StaffID,
                            FullName,
                            Role,
                            Email,
                            Phone,
                            Address,
                            CreatedAt,
                            'Active' as Status
                        FROM Staff 
                        WHERE 1=1";

                    // Add department filter
                    if (!string.IsNullOrEmpty(selectedDepartment) && selectedDepartment != "All Departments")
                    {
                        query += " AND Role = @Role";
                    }

                    // Note: Status filter is commented out since we don't have Status column
                    // if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All Status")
                    // {
                    //     query += " AND Status = @Status";
                    // }

                    query += " ORDER BY StaffID DESC";

                    SqlCommand cmd = new SqlCommand(query, con);

                    if (!string.IsNullOrEmpty(selectedDepartment) && selectedDepartment != "All Departments")
                    {
                        cmd.Parameters.AddWithValue("@Role", selectedDepartment);
                    }

                    // if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All Status")
                    // {
                    //     cmd.Parameters.AddWithValue("@Status", selectedStatus);
                    // }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Format the CreatedAt column
                    if (dt.Columns.Contains("CreatedAt"))
                    {
                        dt.Columns["CreatedAt"].ColumnName = "Join Date";
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["Join Date"] != DBNull.Value)
                            {
                                DateTime joinDate = Convert.ToDateTime(row["Join Date"]);
                                row["Join Date"] = joinDate.ToString("dd-MMM-yyyy");
                            }
                        }
                    }

                    // Rename columns for better display
                    dt.Columns["FullName"].ColumnName = "Full Name";
                    dt.Columns["StaffID"].ColumnName = "ID";

                    dgvAllStaff.DataSource = dt;

                    // Apply row coloring
                    ApplyRowColoring();

                    // Format columns
                    FormatDataGridViewColumns();

                    // Show search results count
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No staff members found matching your criteria.",
                            "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching staff: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllStaff();
            LoadStaffSummary();
            cmbDepartment.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            // Refresh animation
            btnRefresh.Text = "🔄 Refreshing...";
            Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            btnRefresh.Text = "🔄 Refresh All";

            MessageBox.Show("Staff data refreshed successfully!", "Refresh Complete",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvAllStaff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow selectedRow = dgvAllStaff.Rows[e.RowIndex];
                    int staffID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    // Show staff details in a message box
                    string details = $"👤 Staff Details:\n\n" +
                                    $"🆔 ID: {staffID}\n" +
                                    $"👨‍💼 Name: {selectedRow.Cells["Full Name"].Value}\n" +
                                    $"🎭 Role: {selectedRow.Cells["Role"].Value}\n" +
                                    $"📧 Email: {selectedRow.Cells["Email"].Value}\n" +
                                    $"📞 Phone: {selectedRow.Cells["Phone"].Value}\n" +
                                    $"📍 Address: {selectedRow.Cells["Address"].Value}\n" +
                                    $"📅 Join Date: {selectedRow.Cells["Join Date"].Value}";

                    MessageBox.Show(details, "Staff Details",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error displaying details: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvAllStaff_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Make email cells blue and underlined
                if (dgvAllStaff.Columns[e.ColumnIndex].Name == "Email" && e.Value != null)
                {
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new Font(dgvAllStaff.Font, FontStyle.Underline);
                }

                // Center align numeric and date columns
                if (dgvAllStaff.Columns[e.ColumnIndex].Name == "ID" ||
                    dgvAllStaff.Columns[e.ColumnIndex].Name == "Phone" ||
                    dgvAllStaff.Columns[e.ColumnIndex].Name == "Join Date")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void ViewAllStaff_Load(object sender, EventArgs e)
        {
            // Initialize tooltip
            toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            // Add event handlers
            btnSearch.Click += btnSearch_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnBack.Click += btnBack_Click;
            dgvAllStaff.CellDoubleClick += dgvAllStaff_CellDoubleClick;
            dgvAllStaff.CellFormatting += dgvAllStaff_CellFormatting;

            // Add Enter key functionality for search
            cmbDepartment.KeyPress += (s, ev) =>
            {
                if (ev.KeyChar == (char)Keys.Enter)
                {
                    btnSearch_Click(s, ev);
                    ev.Handled = true;
                }
            };

            cmbStatus.KeyPress += (s, ev) =>
            {
                if (ev.KeyChar == (char)Keys.Enter)
                {
                    btnSearch_Click(s, ev);
                    ev.Handled = true;
                }
            };
        }

        private void ViewAllStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the Staff View?",
                "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        // Add export to CSV functionality
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Staff Data";
                saveFileDialog.FileName = $"Staff_Data_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        // Write headers
                        for (int i = 0; i < dgvAllStaff.Columns.Count; i++)
                        {
                            writer.Write(dgvAllStaff.Columns[i].HeaderText);
                            if (i < dgvAllStaff.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                        // Write data
                        foreach (DataGridViewRow row in dgvAllStaff.Rows)
                        {
                            for (int i = 0; i < dgvAllStaff.Columns.Count; i++)
                            {
                                var value = row.Cells[i].Value ?? "";
                                writer.Write($"\"{value.ToString().Replace("\"", "\"\"")}\"");
                                if (i < dgvAllStaff.Columns.Count - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show($"Staff data exported successfully to:\n{saveFileDialog.FileName}",
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add print functionality
        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print functionality would be implemented here.\n" +
                          "You can add a PrintDocument component for printing.",
                          "Print Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Add delete selected staff member functionality
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAllStaff.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvAllStaff.SelectedRows[0];
                int staffID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                string staffName = selectedRow.Cells["Full Name"].Value.ToString();

                DialogResult confirm = MessageBox.Show($"Are you sure you want to delete staff member:\n\n" +
                                                     $"ID: {staffID}\n" +
                                                     $"Name: {staffName}\n\n" +
                                                     "This action cannot be undone!",
                                                     "Confirm Deletion",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            string deleteQuery = "DELETE FROM Staff WHERE StaffID = @StaffID";
                            SqlCommand cmd = new SqlCommand(deleteQuery, con);
                            cmd.Parameters.AddWithValue("@StaffID", staffID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Staff member '{staffName}' deleted successfully!",
                                    "Deletion Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAllStaff();
                                LoadStaffSummary();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting staff member: {ex.Message}",
                            "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to delete.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ToolTip component - add this to your form
        private ToolTip toolTip1;
    }
}