using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class EditStaff : Form
    {
        // Database connection string - آپ کے DB کے مطابق اپ ڈیٹ کریں
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public EditStaff()
        {
            InitializeComponent();
            LoadDepartmentsAndPositions();
            AttachEventHandlers();
        }

        // Form Load Event
        private void EditStaff_Load(object sender, EventArgs e)
        {
            LoadDepartmentsAndPositions();
        }

        // Event handlers جوڑنے کا method
        private void AttachEventHandlers()
        {
            this.Load += new EventHandler(EditStaff_Load);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnBack.Click += new EventHandler(btnBack_Click);
        }

        // Departments اور Positions ڈراپ ڈاؤن میں load کرنے کا method
        private void LoadDepartmentsAndPositions()
        {
            try
            {
                // آپ کے DB میں Department اور Position کالموں کے لیے options
                // اگر آپ چاہیں تو DB سے بھی load کر سکتے ہیں

                // Departments (آپ کے DB کے مطابق)
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.AddRange(new object[] {
                    "Management",
                    "Cashier",
                    "Store Operations",
                    "Inventory",
                    "Customer Service",
                    "Security",
                    "Cleaning"
                });

                // Positions (آپ کے DB کے مطابق)
                cmbPosition.Items.Clear();
                cmbPosition.Items.AddRange(new object[] {
                    "Manager",
                    "Assistant Manager",
                    "Senior Cashier",
                    "Cashier",
                    "Store Assistant",
                    "Stock Clerk",
                    "Security Guard",
                    "Cleaner"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SEARCH button click
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text))
            {
                MessageBox.Show("Please enter Employee ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmployeeID.Focus();
                return;
            }

            if (!int.TryParse(txtEmployeeID.Text.Trim(), out int employeeID))
            {
                MessageBox.Show("Please enter a valid numeric Employee ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmployeeID.Focus();
                return;
            }

            SearchEmployee(employeeID);
        }

        // Employee search method
        private void SearchEmployee(int employeeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // آپ کے DB میں Staff ٹیبل کے کالم: StaffID, FullName, Role, Email, Phone, Address, CreatedAt
                    // لیکن آپ کے ڈیزائن میں: FirstName, LastName, CNIC, Phone, Email, Department, Position
                    // اس لیے میں FullName کو FirstName + LastName میں split کروں گا

                    string query = "SELECT * FROM Staff WHERE StaffID = @EmployeeID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // StaffID
                        txtEmployeeID.Text = reader["StaffID"].ToString();

                        // FullName کو FirstName اور LastName میں split کریں
                        string fullName = reader["FullName"].ToString();
                        string[] nameParts = fullName.Split(' ');

                        if (nameParts.Length >= 2)
                        {
                            txtFirstName.Text = nameParts[0];
                            txtLastName.Text = string.Join(" ", nameParts, 1, nameParts.Length - 1);
                        }
                        else
                        {
                            txtFirstName.Text = fullName;
                            txtLastName.Text = "";
                        }

                        // Role کو Department اور Position میں تقسیم کریں
                        // Note: آپ کے DB میں صرف Role ہے، اس لیے میں اسے Department اور Position کے طور پر استعمال کر رہا ہوں
                        string role = reader["Role"].ToString();
                        cmbDepartment.Text = role; // Temporary: role کو department کے طور پر استعمال کریں
                        cmbPosition.Text = role;   // Temporary: role کو position کے طور پر استعمال کریں

                        // باقی fields
                        txtEmail.Text = reader["Email"].ToString();
                        txtPhone.Text = reader["Phone"].ToString();

                        // Address کو CNIC کے طور پر استعمال کریں (آپ کے ڈیزائن میں CNIC field ہے)
                        // Note: اصل میں CNIC کا کالم نہیں ہے، اس لیے Address استعمال کر رہے ہیں
                        txtCNIC.Text = reader["Address"].ToString();

                        // Update button enable کریں
                        btnUpdate.Enabled = true;

                        // Status message
                        label5.Text = "Employee found. You can now edit details.";
                        label5.ForeColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        MessageBox.Show($"Employee with ID {employeeID} not found!", "Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }

                    reader.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching employee: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE button click
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text))
            {
                MessageBox.Show("Please search for an employee first", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbDepartment.Text))
            {
                MessageBox.Show("Department is required", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbDepartment.Focus();
                return;
            }

            if (!int.TryParse(txtEmployeeID.Text.Trim(), out int employeeID))
            {
                MessageBox.Show("Invalid Employee ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm update
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to update Employee ID: {employeeID}?",
                "Confirm Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // آپ کے DB میں Staff ٹیبل کا ڈیزائن:
                    // StaffID, FullName, Role, Email, Phone, Address, CreatedAt

                    // FullName بنائیں
                    string fullName = txtFirstName.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(txtLastName.Text.Trim()))
                    {
                        fullName += " " + txtLastName.Text.Trim();
                    }

                    // Role کا تعین: Department + Position ملا کر یا صرف Department
                    string role = cmbDepartment.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(cmbPosition.Text.Trim()))
                    {
                        role = cmbPosition.Text.Trim(); // Position کو priority دیں
                    }

                    string query = @"UPDATE Staff SET 
                                    FullName = @FullName, 
                                    Role = @Role, 
                                    Email = @Email, 
                                    Phone = @Phone, 
                                    Address = @Address
                                    WHERE StaffID = @EmployeeID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                    // Address میں CNIC ڈالیں (آپ کے ڈیزائن میں CNIC field ہے)
                    cmd.Parameters.AddWithValue("@Address", txtCNIC.Text.Trim());

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show($"Employee ID {employeeID} updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Audit log (optional)
                        LogEmployeeUpdate(employeeID, "Updated employee information");

                        // Status message update
                        label5.Text = $"Employee ID {employeeID} updated successfully!";
                        label5.ForeColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        MessageBox.Show("Employee not found or no changes made!", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Unique constraint violation (email duplicate)
                {
                    MessageBox.Show("Email already exists. Please use a different email.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                }
                else if (sqlEx.Number == 547) // Foreign key constraint
                {
                    MessageBox.Show("Cannot update due to referential integrity constraints.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CANCEL button click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // BACK button click
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Fields clear کرنے کا method
        private void ClearFields()
        {
            txtEmployeeID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCNIC.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;

            btnUpdate.Enabled = false;

            // Reset status message
            label5.Text = "Step 1: Enter Employee ID and Click Search";
            label5.ForeColor = System.Drawing.Color.White;

            txtEmployeeID.Focus();
        }

        // Paint event
        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom painting if needed
        }

        // Employee update کا log رکھنے کا method (optional)
        private void LogEmployeeUpdate(int employeeID, string action)
        {
            try
            {
                // اگر آپ AuditLog ٹیبل بنانا چاہیں تو یہ SQL استعمال کریں:
                /*
                CREATE TABLE EmployeeAuditLog (
                    LogID INT IDENTITY(1,1) PRIMARY KEY,
                    EmployeeID INT NOT NULL,
                    Action NVARCHAR(200),
                    ActionBy NVARCHAR(100),
                    ActionDate DATETIME DEFAULT GETDATE()
                )
                */

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Check if AuditLog table exists
                    string checkTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmployeeAuditLog')
                        BEGIN
                            CREATE TABLE EmployeeAuditLog (
                                LogID INT IDENTITY(1,1) PRIMARY KEY,
                                EmployeeID INT NOT NULL,
                                Action NVARCHAR(200),
                                ActionBy NVARCHAR(100),
                                ActionDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    SqlCommand checkCmd = new SqlCommand(checkTableQuery, con);
                    con.Open();
                    checkCmd.ExecuteNonQuery();

                    // Insert audit log
                    string insertQuery = @"
                        INSERT INTO EmployeeAuditLog (EmployeeID, Action, ActionBy)
                        VALUES (@EmployeeID, @Action, @ActionBy)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    insertCmd.Parameters.AddWithValue("@Action", action);
                    insertCmd.Parameters.AddWithValue("@ActionBy", Environment.UserName);

                    insertCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail if audit logging fails
            }
        }

        // Key events for better UX
        private void txtEmployeeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Enter key to trigger search
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, plus, hyphen, and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '+' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void txtCNIC_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and hyphens for CNIC format
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        // Form Closing Event
        private void EditStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            // کوئی cleanup اگر چاہیں تو
        }

        // Add new employee feature (optional - اگر آپ چاہیں تو)
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            // یہ بٹن آپ کے ڈیزائن میں نہیں ہے، لیکن اگر چاہیں تو شامل کر سکتے ہیں
            ClearFields();
            txtEmployeeID.Text = "NEW";
            txtEmployeeID.Enabled = false;
            txtFirstName.Focus();
            btnUpdate.Text = "Add Employee";
            label5.Text = "Adding new employee. Fill all details.";
            label5.ForeColor = System.Drawing.Color.Yellow;
        }

        // Delete employee (optional - اگر آپ چاہیں تو)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) || txtEmployeeID.Text == "NEW")
            {
                MessageBox.Show("Please select an employee to delete", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtEmployeeID.Text.Trim(), out int employeeID))
            {
                MessageBox.Show("Invalid Employee ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete Employee ID: {employeeID}?\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Staff WHERE StaffID = @EmployeeID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show($"Employee ID {employeeID} deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogEmployeeUpdate(employeeID, "Deleted employee");
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Employee not found!", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547) // Foreign key constraint
                {
                    MessageBox.Show("Cannot delete employee because they have related records in other tables.",
                        "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}