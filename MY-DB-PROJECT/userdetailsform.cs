using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class UserDetailsForm : Form
    {
        private int userID;
        private string username;
        private string email;
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        // Constructor modified to accept username and email
        public UserDetailsForm(int userID, string username, string email)
        {
            InitializeComponent();
            this.userID = userID;
            this.username = username;
            this.email = email;
            LoadUserDetails();
        }

        private void LoadUserDetails()
        {
            try
            {
                // Auto-fill username and email from login
                txtName.Text = username;
                TxtEmail.Text = email;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            Phone,
                            Address
                        FROM Users 
                        WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Load phone and address if available
                            txtPhone.Text = dr["Phone"] != DBNull.Value ? dr["Phone"].ToString() : "";
                            txtAddress.Text = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user details: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Validations - Username and Email are pre-filled, user can edit them
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter your name!", "Required Field",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                MessageBox.Show("Please enter your email!", "Required Field",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtEmail.Focus();
                return;
            }

            // Email validation
            if (!IsValidEmail(TxtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address!", "Invalid Email",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtEmail.Focus();
                TxtEmail.SelectAll();
                return;
            }

            // Phone number validation (if provided)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!IsValidPhoneNumber(txtPhone.Text))
                {
                    MessageBox.Show("Please enter a valid phone number (11 digits starting with 03)!",
                                  "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    txtPhone.SelectAll();
                    return;
                }
            }

            // Update user details in database
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string updateQuery = @"
                        UPDATE Users 
                        SET 
                            Username = @Username,
                            Email = @Email,
                            Phone = @Phone,
                            Address = @Address
                        WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@Username", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Success message
                        MessageBox.Show($"✅ Details saved successfully!\n\n" +
                                      $"👤 Name: {txtName.Text}\n" +
                                      $"📧 Email: {TxtEmail.Text}" +
                                      (string.IsNullOrWhiteSpace(txtPhone.Text) ? "" : $"\n📞 Phone: {txtPhone.Text}") +
                                      (string.IsNullOrWhiteSpace(txtAddress.Text) ? "" : $"\n📍 Address: {txtAddress.Text}"),
                                      "Success",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update details.", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Unique constraint violation
                {
                    if (sqlEx.Message.Contains("Email"))
                    {
                        MessageBox.Show("Email already exists! Please use a different email.",
                                      "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TxtEmail.Focus();
                        TxtEmail.SelectAll();
                    }
                    else if (sqlEx.Message.Contains("Username"))
                    {
                        MessageBox.Show("Username already exists! Please use a different name.",
                                      "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtName.Focus();
                        txtName.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show($"Database Error: {sqlEx.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving details: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Pakistani phone number validation (03xx-xxxxxxx)
            if (string.IsNullOrWhiteSpace(phone))
                return true; // Phone is optional, so empty is valid

            // Remove spaces and dashes
            phone = phone.Replace(" ", "").Replace("-", "");

            // Check if it's 11 digits and starts with 03
            return phone.Length == 11 && phone.StartsWith("03") && long.TryParse(phone, out _);
        }

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

        private void btnEditCart_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to edit your cart?\n\nAny unsaved changes will be lost.",
                                                "Edit Cart",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void UserDetailsForm_Load(object sender, EventArgs e)
        {
            txtName.Focus();

            // Set maximum lengths according to database schema
            txtName.MaxLength = 50;      // Users.Username NVARCHAR(50)
            txtPhone.MaxLength = 15;     // Users.Phone NVARCHAR(15)
            txtAddress.MaxLength = 255;  // Users.Address NVARCHAR(255)
            TxtEmail.MaxLength = 100;    // Users.Email NVARCHAR(100)
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, backspace, and dash
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            // Auto-format phone number: 03xx-xxxxxxx
            if (txtPhone.Text.Length == 4 && !txtPhone.Text.Contains("-"))
            {
                txtPhone.Text = txtPhone.Text.Insert(4, "-");
                txtPhone.SelectionStart = txtPhone.Text.Length;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow Enter key to move to next field
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPhone.Focus();
                e.Handled = true;
            }
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            // Allow Enter key to move to next field
            if (e.KeyCode == Keys.Enter)
            {
                txtAddress.Focus();
                e.Handled = true;
            }
        }

        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow Enter key to move to next field
            if (e.KeyChar == (char)Keys.Enter)
            {
                TxtEmail.Focus();
                e.Handled = true;
            }
        }

        private void TxtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow Enter key to trigger Next button
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnNext_Click(sender, e);
                e.Handled = true;
            }
        }

        private void UserDetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ask for confirmation if user tries to close without saving
            if (this.DialogResult == DialogResult.None)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit?\n\nAny unsaved changes will be lost.",
                                                    "Exit Confirmation",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void UserDetailsForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}