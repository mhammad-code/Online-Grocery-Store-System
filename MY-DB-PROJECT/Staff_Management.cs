using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class Staff_Management : Form
    {
        // Updated connection string to match your database
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public Staff_Management()
        {
            InitializeComponent();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            // Clear existing columns
            dgvStaff.Columns.Clear();

            // Add columns based on your Staff table schema
            dgvStaff.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StaffID",
                HeaderText = "ID",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            });

            dgvStaff.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "Full Name",
                Width = 180,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 10)
                }
            });

            dgvStaff.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Role",
                HeaderText = "Role/Position",
                Width = 150,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });

            dgvStaff.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                Width = 180,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });

            dgvStaff.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                HeaderText = "Phone",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvStaff.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Joined On",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "dd/MM/yyyy"
                }
            });

            dgvStaff.RowTemplate.Height = 35;
            dgvStaff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStaff.AllowUserToAddRows = false;
            dgvStaff.ReadOnly = true;
            dgvStaff.MultiSelect = false;
        }

        private void Staff_Management_Load(object sender, EventArgs e)
        {
            LoadStaffStats();
            LoadStaffData();
            ApplyDataGridViewStyling();
        }

        private void ApplyDataGridViewStyling()
        {
            // Header styling
            dgvStaff.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvStaff.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStaff.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvStaff.ColumnHeadersHeight = 40;
            dgvStaff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row styling
            dgvStaff.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvStaff.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvStaff.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvStaff.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Grid lines
            dgvStaff.GridColor = Color.FromArgb(224, 224, 224);
            dgvStaff.BorderStyle = BorderStyle.None;
            dgvStaff.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        private void LoadStaffStats()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Total Staff - your Staff table doesn't have Status column, so count all
                    SqlCommand cmdTotal = new SqlCommand("SELECT COUNT(*) FROM Staff", con);
                    int totalStaff = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    lblTotalStaff.Text = totalStaff.ToString();

                    // Since your Staff table doesn't have Status column, we'll simulate
                    // Active Staff (all are considered active in your schema)
                    lblActiveStaff.Text = totalStaff.ToString();

                    // Inactive Staff (none in your schema)
                    lblInactiveStaff.Text = "0";

                    // On Leave (none in your schema, but we can add logic if needed)
                    lblOnLeave.Text = "0";

                    // Update form title
                    this.Text = $"Staff Management - {totalStaff} Staff Members";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff statistics: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void LoadStaffData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Query matches your Staff table schema
                    string query = @"
                        SELECT 
                            StaffID,
                            FullName,
                            Role,
                            Email,
                            Phone,
                            CreatedAt
                        FROM Staff 
                        ORDER BY FullName";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Clear existing rows
                    dgvStaff.Rows.Clear();

                    // Populate DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvStaff.Rows.Add();
                        DataGridViewRow dataRow = dgvStaff.Rows[rowIndex];

                        dataRow.Cells["StaffID"].Value = row["StaffID"];
                        dataRow.Cells["FullName"].Value = row["FullName"];
                        dataRow.Cells["Role"].Value = row["Role"];
                        dataRow.Cells["Email"].Value = row["Email"];
                        dataRow.Cells["Phone"].Value = row["Phone"] != DBNull.Value ? row["Phone"] : "N/A";

                        if (row["CreatedAt"] != DBNull.Value)
                        {
                            DateTime joinDate = Convert.ToDateTime(row["CreatedAt"]);
                            dataRow.Cells["CreatedAt"].Value = joinDate;
                        }
                        else
                        {
                            dataRow.Cells["CreatedAt"].Value = "N/A";
                        }

                        // Color code based on role
                        string role = row["Role"].ToString().ToLower();
                        if (role.Contains("manager") || role.Contains("admin"))
                        {
                            dataRow.Cells["Role"].Style.ForeColor = Color.FromArgb(33, 150, 243);
                            dataRow.Cells["Role"].Style.Font = new Font(dgvStaff.Font, FontStyle.Bold);
                        }
                        else if (role.Contains("cashier"))
                        {
                            dataRow.Cells["Role"].Style.ForeColor = Color.FromArgb(76, 175, 80);
                            dataRow.Cells["Role"].Style.Font = new Font(dgvStaff.Font, FontStyle.Bold);
                        }
                        else
                        {
                            dataRow.Cells["Role"].Style.ForeColor = Color.FromArgb(255, 152, 0);
                            dataRow.Cells["Role"].Style.Font = new Font(dgvStaff.Font, FontStyle.Bold);
                        }
                    }

                    // Update count label
                    label7.Text = $"👥 Staff Members ({dgvStaff.Rows.Count})";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff data: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void btnViewStaff_Click(object sender, EventArgs e)
        {
            LoadStaffData();
            MessageBox.Show("Staff list refreshed successfully!",
                          "Refresh Complete",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            // Create a simple dialog for adding staff
            using (AddStaffDialog addStaffDialog = new AddStaffDialog())
            {
                if (addStaffDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();

                            string query = @"
                                INSERT INTO Staff (FullName, Role, Email, Phone, Address, CreatedAt)
                                VALUES (@FullName, @Role, @Email, @Phone, @Address, GETDATE())";

                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@FullName", addStaffDialog.StaffName);
                            cmd.Parameters.AddWithValue("@Role", addStaffDialog.Role);
                            cmd.Parameters.AddWithValue("@Email", addStaffDialog.Email);
                            cmd.Parameters.AddWithValue("@Phone", addStaffDialog.Phone);
                            cmd.Parameters.AddWithValue("@Address", addStaffDialog.Address);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Staff member '{addStaffDialog.StaffName}' added successfully!",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                LoadStaffData();
                                LoadStaffStats();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding staff: {ex.Message}",
                                      "Database Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditStaff_Click(object sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a staff member to edit.",
                              "No Selection",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvStaff.SelectedRows[0];
            int staffID = Convert.ToInt32(selectedRow.Cells["StaffID"].Value);
            string staffName = selectedRow.Cells["FullName"].Value.ToString();
            string role = selectedRow.Cells["Role"].Value.ToString();
            string email = selectedRow.Cells["Email"].Value.ToString();
            string phone = selectedRow.Cells["Phone"].Value.ToString();

            // Create a simple edit dialog
            using (EditStaffDialog editDialog = new EditStaffDialog(staffName, role, email, phone))
            {
                if (editDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();

                            string query = @"
                                UPDATE Staff 
                                SET FullName = @FullName,
                                    Role = @Role,
                                    Email = @Email,
                                    Phone = @Phone
                                WHERE StaffID = @StaffID";

                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@FullName", editDialog.StaffName);
                            cmd.Parameters.AddWithValue("@Role", editDialog.Role);
                            cmd.Parameters.AddWithValue("@Email", editDialog.Email);
                            cmd.Parameters.AddWithValue("@Phone", editDialog.Phone);
                            cmd.Parameters.AddWithValue("@StaffID", staffID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Staff member '{editDialog.StaffName}' updated successfully!",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                LoadStaffData();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating staff: {ex.Message}",
                                      "Database Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Double-click to view details
        private void dgvStaff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvStaff.Rows.Count)
            {
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];

                string staffID = row.Cells["StaffID"].Value?.ToString() ?? "N/A";
                string fullName = row.Cells["FullName"].Value?.ToString() ?? "N/A";
                string role = row.Cells["Role"].Value?.ToString() ?? "N/A";
                string email = row.Cells["Email"].Value?.ToString() ?? "N/A";
                string phone = row.Cells["Phone"].Value?.ToString() ?? "N/A";
                string joinDate = row.Cells["CreatedAt"].Value?.ToString() ?? "N/A";

                string details = "👤 STAFF DETAILS\n";
                details += "═══════════════════════\n\n";
                details += $"🆔 Staff ID: {staffID}\n";
                details += $"👨‍💼 Name: {fullName}\n";
                details += $"💼 Role: {role}\n";
                details += $"📧 Email: {email}\n";
                details += $"📞 Phone: {phone}\n";
                details += $"📅 Joined: {joinDate}\n\n";
                details += "═══════════════════════\n";

                MessageBox.Show(details,
                              "Staff Information",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
        }

        // Right-click context menu
        private void dgvStaff_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvStaff.ClearSelection();
                dgvStaff.Rows[e.RowIndex].Selected = true;

                ContextMenuStrip contextMenu = new ContextMenuStrip();

                ToolStripMenuItem viewDetailsItem = new ToolStripMenuItem("View Details");
                ToolStripMenuItem editItem = new ToolStripMenuItem("Edit Staff");
                ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete Staff");
                ToolStripSeparator separator = new ToolStripSeparator();
                ToolStripMenuItem refreshItem = new ToolStripMenuItem("Refresh");

                viewDetailsItem.Click += (s, ev) => dgvStaff_CellDoubleClick(sender,
                    new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));

                editItem.Click += (s, ev) => btnEditStaff_Click(sender, e);

                deleteItem.Click += (s, ev) =>
                {
                    if (dgvStaff.SelectedRows.Count > 0)
                    {
                        DataGridViewRow selectedRow = dgvStaff.SelectedRows[0];
                        string staffName = selectedRow.Cells["FullName"].Value.ToString();

                        DialogResult result = MessageBox.Show($"Are you sure you want to delete {staffName}?\nThis action cannot be undone.",
                                                            "Confirm Delete",
                                                            MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            DeleteStaff(Convert.ToInt32(selectedRow.Cells["StaffID"].Value));
                        }
                    }
                };

                refreshItem.Click += (s, ev) => LoadStaffData();

                contextMenu.Items.Add(viewDetailsItem);
                contextMenu.Items.Add(editItem);
                contextMenu.Items.Add(deleteItem);
                contextMenu.Items.Add(separator);
                contextMenu.Items.Add(refreshItem);

                contextMenu.Show(dgvStaff, e.Location);
            }
        }

        private void DeleteStaff(int staffID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "DELETE FROM Staff WHERE StaffID = @StaffID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@StaffID", staffID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Staff member deleted successfully!",
                                      "Deleted",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        LoadStaffData();
                        LoadStaffStats();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting staff: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        // Keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                LoadStaffData();
                return true;
            }
            else if (keyData == Keys.Delete && dgvStaff.Focused)
            {
                if (dgvStaff.SelectedRows.Count > 0)
                {
                    btnEditStaff_Click(null, null);
                    return true;
                }
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    // Simple dialog classes (you can create separate files for these)
    public class AddStaffDialog : Form
    {
        private TextBox txtName;
        private TextBox txtRole;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private Button btnOK;
        private Button btnCancel;

        public string StaffName => txtName.Text;
        public string Role => txtRole.Text;
        public string Email => txtEmail.Text;
        public string Phone => txtPhone.Text;
        public string Address => txtAddress.Text;

        public AddStaffDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Add New Staff";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblName = new Label { Text = "Full Name:", Location = new Point(20, 20), Width = 100 };
            txtName = new TextBox { Location = new Point(130, 20), Width = 230 };

            Label lblRole = new Label { Text = "Role:", Location = new Point(20, 60), Width = 100 };
            txtRole = new TextBox { Location = new Point(130, 60), Width = 230 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), Width = 100 };
            txtEmail = new TextBox { Location = new Point(130, 100), Width = 230 };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 140), Width = 100 };
            txtPhone = new TextBox { Location = new Point(130, 140), Width = 230 };

            Label lblAddress = new Label { Text = "Address:", Location = new Point(20, 180), Width = 100 };
            txtAddress = new TextBox { Location = new Point(130, 180), Width = 230, Multiline = true, Height = 60 };

            btnOK = new Button { Text = "OK", Location = new Point(130, 260), Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", Location = new Point(220, 260), Width = 80, DialogResult = DialogResult.Cancel };

            this.Controls.AddRange(new Control[] { lblName, txtName, lblRole, txtRole, lblEmail, txtEmail,
                                                   lblPhone, txtPhone, lblAddress, txtAddress, btnOK, btnCancel });

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }

    public class EditStaffDialog : Form
    {
        private TextBox txtName;
        private TextBox txtRole;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private Button btnOK;
        private Button btnCancel;

        public string StaffName => txtName.Text;
        public string Role => txtRole.Text;
        public string Email => txtEmail.Text;
        public string Phone => txtPhone.Text;

        public EditStaffDialog(string name, string role, string email, string phone)
        {
            InitializeComponent();
            txtName.Text = name;
            txtRole.Text = role;
            txtEmail.Text = email;
            txtPhone.Text = phone;
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Staff";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblName = new Label { Text = "Full Name:", Location = new Point(20, 20), Width = 100 };
            txtName = new TextBox { Location = new Point(130, 20), Width = 230 };

            Label lblRole = new Label { Text = "Role:", Location = new Point(20, 60), Width = 100 };
            txtRole = new TextBox { Location = new Point(130, 60), Width = 230 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), Width = 100 };
            txtEmail = new TextBox { Location = new Point(130, 100), Width = 230 };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 140), Width = 100 };
            txtPhone = new TextBox { Location = new Point(130, 140), Width = 230 };

            btnOK = new Button { Text = "OK", Location = new Point(130, 180), Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", Location = new Point(220, 180), Width = 80, DialogResult = DialogResult.Cancel };

            this.Controls.AddRange(new Control[] { lblName, txtName, lblRole, txtRole, lblEmail, txtEmail,
                                                   lblPhone, txtPhone, btnOK, btnCancel });

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }
}