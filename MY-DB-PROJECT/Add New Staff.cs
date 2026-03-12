using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class AddNewStaff : Form
    {
        // Database connection string
       // private string connectionString = @"Data Source=.\SQLEXPRES;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        // Alternative connection strings (try if above doesn't work):
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        private Random random = new Random();

        public AddNewStaff()
        {
            InitializeComponent();
            btnGenerateID.Click += BtnGenerateID_Click;
            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
            btnBack.Click += BtnBack_Click;

            // Set default values for dropdowns
            LoadFormDefaults();
        }

        private void LoadFormDefaults()
        {
            try
            {
                // Set default department and position
                cmbDepartment.SelectedIndex = 0;
                cmbPosition.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading defaults: {ex.Message}");
            }
        }

        // Event: Generate a random Employee ID
        private void BtnGenerateID_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate a unique staff ID
                string newID = "STAFF" + random.Next(10000, 99999);

                // Check if ID already exists in database
                if (!IsStaffIDExists(newID))
                {
                    txtEmployeeID.Text = newID;
                    MessageBox.Show($"Employee ID Generated: {newID}", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Try again if ID exists
                    BtnGenerateID_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating ID: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Check if Staff ID already exists in database
        private bool IsStaffIDExists(string staffID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Staff WHERE FullName LIKE @StaffIDPattern";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@StaffIDPattern", $"%{staffID}%");

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Event: Save staff information to database
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (!ValidateForm())
                return;

            try
            {
                // Save to database
                bool success = SaveStaffToDatabase();

                if (success)
                {
                    MessageBox.Show("✅ Staff member saved successfully to database!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear form after successful save
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("❌ Failed to save staff member!", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}\n\nPlease check:\n1. Database connection\n2. Staff table exists",
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validate form inputs
        private bool ValidateForm()
        {
            // Check required fields
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text))
            {
                MessageBox.Show("Please generate an Employee ID first!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnGenerateID.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last Name is required!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbDepartment.Text))
            {
                MessageBox.Show("Please select a Department!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbPosition.Text))
            {
                MessageBox.Show("Please select a Position!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPosition.Focus();
                return false;
            }

            // Validate email format
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validate phone number
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Please enter a valid phone number (10-15 digits)!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        // Simple email validation
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Simple phone validation
        private bool IsValidPhone(string phone)
        {
            // Remove all non-digit characters
            string digitsOnly = System.Text.RegularExpressions.Regex.Replace(phone, @"[^\d]", "");
            return digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
        }

        // Save staff to database
        private bool SaveStaffToDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Create full name from first and last name
                    string fullName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";

                    // Prepare phone number (remove non-digits)
                    string phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null :
                                 System.Text.RegularExpressions.Regex.Replace(txtPhone.Text, @"[^\d]", "");

                    // Prepare CNIC
                    string cnic = string.IsNullOrWhiteSpace(txtCNIC.Text) ? null : txtCNIC.Text.Trim();

                    // Prepare email
                    string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();

                    // Insert query for Staff table
                    string query = @"
                        INSERT INTO Staff (FullName, Role, Email, Phone, Address, CreatedAt)
                        VALUES (@FullName, @Role, @Email, @Phone, @Address, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Role", cmbPosition.Text.Trim());

                    // Handle nullable parameters
                    if (email != null)
                        cmd.Parameters.AddWithValue("@Email", email);
                    else
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);

                    if (phone != null)
                        cmd.Parameters.AddWithValue("@Phone", phone);
                    else
                        cmd.Parameters.AddWithValue("@Phone", DBNull.Value);

                    // Address - using CNIC as address if provided, otherwise NULL
                    if (cnic != null)
                        cmd.Parameters.AddWithValue("@Address", $"CNIC: {cnic}");
                    else
                        cmd.Parameters.AddWithValue("@Address", DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Also save to a backup or additional table if needed
                    SaveToAdditionalTable(fullName, phone, email, cnic);

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Save Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Optional: Save to additional table for record keeping
        private void SaveToAdditionalTable(string fullName, string phone, string email, string cnic)
        {
            try
            {
                // This is optional - you can create an EmployeeDetails table if needed
                // For now, we're just using the Staff table

                // If you want to track employee IDs separately, you could create another table:
                /*
                CREATE TABLE EmployeeDetails (
                    EmployeeDetailID INT IDENTITY(1,1) PRIMARY KEY,
                    StaffID INT,
                    EmployeeID NVARCHAR(50),
                    FirstName NVARCHAR(50),
                    LastName NVARCHAR(50),
                    CNIC NVARCHAR(15),
                    Department NVARCHAR(100),
                    CreatedAt DATETIME DEFAULT GETDATE(),
                    FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
                )
                */
            }
            catch (Exception)
            {
                // Silently fail - this is optional
            }
        }

        // Event: Clear all input fields
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all fields?",
                                                "Confirm Clear",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                txtEmployeeID.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtCNIC.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
                cmbDepartment.SelectedIndex = 0;
                cmbPosition.SelectedIndex = 0;

                txtFirstName.Focus();
            }
        }

        // Event: Go back to previous form
        private void BtnBack_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to go back?\nUnsaved changes will be lost.",
                                                "Confirm Exit",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Go back to Staff Management form
                Staff_Management staffManagementForm = new Staff_Management();
                staffManagementForm.Show();
                this.Close();
            }
        }

        // Form load event
        private void AddNewStaff_Load(object sender, EventArgs e)
        {
            // Focus on first field
            txtFirstName.Focus();
        }

        // Keyboard shortcuts
        private void AddNewStaff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                BtnSave_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnBack_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                BtnGenerateID_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                BtnClear_Click(sender, e);
                e.Handled = true;
            }
        }

        // Input validation events
        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, backspace, and plus sign
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '+')
            {
                e.Handled = true;
            }
        }

        private void txtCNIC_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and hyphens for CNIC
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            // Validate email format when leaving field
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address!", "Invalid Email",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
            }
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            // Validate phone format when leaving field
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Please enter a valid phone number (10-15 digits)!", "Invalid Phone",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
            }
        }

        // Auto-format phone number
        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add auto-formatting for phone numbers
            // This can format as user types: 03001234567 → 0300-1234567
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}