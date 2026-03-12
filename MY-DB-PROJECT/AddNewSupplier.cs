using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MY_DB_PROJECT
{
    public partial class AddNewSupplier : Form
    {
        // Database connection string for grocerystore database
        //private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        // Alternative connection strings if needed:
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public AddNewSupplier()
        {
            InitializeComponent();

            // Button event handlers
            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
            btnBack.Click += BtnBack_Click;

            // Input validation events
            txtPhone.KeyPress += TxtPhone_KeyPress;
            txtEmail.Leave += TxtEmail_Leave;

            // Form events
            this.KeyDown += AddNewSupplier_KeyDown;
            this.Load += AddNewSupplier_Load;
        }

        // Save supplier to database
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate form before saving
            if (!ValidateForm())
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if supplier already exists with same name
                    string checkQuery = "SELECT COUNT(*) FROM Suppliers WHERE SupplierName = @SupplierName";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                        int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existingCount > 0)
                        {
                            DialogResult result = MessageBox.Show($"Supplier '{txtSupplierName.Text.Trim()}' already exists.\nDo you want to update instead?",
                                                                "Supplier Exists",
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                UpdateSupplier(conn);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Please use a different supplier name.", "Duplicate Supplier",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtSupplierName.Focus();
                                return;
                            }
                        }
                    }

                    // Insert new supplier
                    string insertQuery = @"INSERT INTO Suppliers (SupplierName, Phone, Email, Address) 
                                         VALUES (@SupplierName, @Phone, @Email, @Address)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", CleanPhoneNumber(txtPhone.Text.Trim()));

                        // Handle nullable email
                        if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        else
                            cmd.Parameters.AddWithValue("@Email", DBNull.Value);

                        // Build address from city and notes
                        string address = BuildAddress();
                        if (!string.IsNullOrWhiteSpace(address))
                            cmd.Parameters.AddWithValue("@Address", address);
                        else
                            cmd.Parameters.AddWithValue("@Address", DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("✅ Supplier added successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Get the new supplier ID
                            int newSupplierID = GetLastSupplierID(conn);

                            // Show confirmation with details
                            ShowConfirmation(newSupplierID);

                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("❌ Failed to add supplier.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Message.Contains("Invalid object name"))
                {
                    MessageBox.Show("Suppliers table not found in database!\n\nPlease run the database setup script.",
                                  "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Update existing supplier
        private void UpdateSupplier(SqlConnection conn)
        {
            try
            {
                string updateQuery = @"UPDATE Suppliers 
                                      SET Phone = @Phone, 
                                          Email = @Email, 
                                          Address = @Address
                                      WHERE SupplierName = @SupplierName";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", CleanPhoneNumber(txtPhone.Text.Trim()));

                    if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    else
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);

                    string address = BuildAddress();
                    if (!string.IsNullOrWhiteSpace(address))
                        cmd.Parameters.AddWithValue("@Address", address);
                    else
                        cmd.Parameters.AddWithValue("@Address", DBNull.Value);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("✅ Supplier information updated successfully!", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Build address from city and notes
        private string BuildAddress()
        {
            string address = "";

            if (!string.IsNullOrWhiteSpace(txtCity.Text))
                address += txtCity.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtNotes.Text))
            {
                if (!string.IsNullOrEmpty(address))
                    address += ", " + txtNotes.Text.Trim();
                else
                    address = txtNotes.Text.Trim();
            }

            return address;
        }

        // Get the last inserted supplier ID
        private int GetLastSupplierID(SqlConnection conn)
        {
            try
            {
                string query = "SELECT IDENT_CURRENT('Suppliers')";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        // Show confirmation message with details
        private void ShowConfirmation(int supplierID)
        {
            string message = $"✅ Supplier Added Successfully!\n\n" +
                           $"📋 Supplier Details:\n" +
                           $"────────────────────\n" +
                           $"🆔 Supplier ID: {supplierID}\n" +
                           $"🏢 Name: {txtSupplierName.Text.Trim()}\n" +
                           $"📞 Phone: {txtPhone.Text.Trim()}\n";

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                message += $"📧 Email: {txtEmail.Text.Trim()}\n";

            if (!string.IsNullOrWhiteSpace(txtCity.Text) || !string.IsNullOrWhiteSpace(txtNotes.Text))
                message += $"📍 Address: {BuildAddress()}\n";

            message += $"────────────────────\n" +
                      $"Supplier has been saved to database.";

            MessageBox.Show(message, "Supplier Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Validate form inputs
        private bool ValidateForm()
        {
            // Check required fields
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Supplier Name is required!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSupplierName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number is required!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Validate phone number format
            string cleanPhone = CleanPhoneNumber(txtPhone.Text);
            if (cleanPhone.Length < 10 || cleanPhone.Length > 15)
            {
                MessageBox.Show("Please enter a valid phone number (10-15 digits)!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Validate email if provided
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address!", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        // Clean phone number (remove non-digits)
        private string CleanPhoneNumber(string phone)
        {
            return Regex.Replace(phone, @"[^\d]", "");
        }

        // Validate email format
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

        // Phone number input validation
        private void TxtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, backspace, plus, hyphen, and space
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '+' && e.KeyChar != '-' && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        // Email validation on leave
        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address!", "Invalid Email",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
            }
        }

        // Clear all input fields
        private void BtnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all fields?",
                                                "Confirm Clear",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearFields();
            }
        }

        private void ClearFields()
        {
            txtSupplierName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtCity.Clear();
            txtNotes.Clear();
            txtSupplierName.Focus();
        }

        // Go back to previous form
        private void BtnBack_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to go back?\nUnsaved changes will be lost.",
                                                "Confirm Exit",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Go back to Supplier Management form
                Supplier_Management supplierManagement = new Supplier_Management();
                supplierManagement.Show();
                this.Close();
            }
        }

        // Form load event
        private void AddNewSupplier_Load(object sender, EventArgs e)
        {
            // Set focus to first field
            txtSupplierName.Focus();

            // Optional: Load last saved data or defaults
            LoadLastSessionData();
        }

        // Load last session data (optional)
        private void LoadLastSessionData()
        {
            try
            {
                // You can load from settings or database if needed
                // For now, just set default city
                if (string.IsNullOrEmpty(txtCity.Text))
                {
                    txtCity.Text = "Karachi"; // Default city
                }
            }
            catch
            {
                // Silently fail
            }
        }

        // Keyboard shortcuts
        private void AddNewSupplier_KeyDown(object sender, KeyEventArgs e)
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
            else if (e.KeyCode == Keys.Delete)
            {
                BtnClear_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Move to next control on Enter key
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }

        // Auto-format phone number as user types
        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            // Optional: Auto-format phone number
            // Example: 03001234567 → 0300-1234567
            /*
            string phone = txtPhone.Text;
            if (phone.Length == 11 && phone.StartsWith("03") && !phone.Contains("-"))
            {
                txtPhone.Text = phone.Insert(4, "-");
                txtPhone.SelectionStart = txtPhone.Text.Length;
            }
            */
        }

        // Test database connection button (optional - if you have a test button)
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("✅ Database connection successful!\n\n" +
                                  $"Database: grocerystore\n" +
                                  $"Server: {conn.DataSource}",
                                  "Connection Test",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Connection failed: {ex.Message}\n\n" +
                              $"Connection String:\n{connectionString}",
                              "Connection Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}