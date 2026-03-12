using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MY_DB_PROJECT
{
    public partial class USER_REGISTRATION : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public USER_REGISTRATION()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Validations (same as your code)
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Username validation
            if (username.Length < 3 || username.Length > 50)
            {
                MessageBox.Show("Username must be between 3 and 50 characters!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                txtUsername.SelectAll();
                return;
            }

            // Password validation
            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                txtPassword.SelectAll();
                return;
            }

            // Email validation
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check duplicate username
                    string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    SqlCommand checkUsernameCmd = new SqlCommand(checkUsernameQuery, con);
                    checkUsernameCmd.Parameters.AddWithValue("@username", username);
                    int usernameExists = (int)checkUsernameCmd.ExecuteScalar();

                    if (usernameExists > 0)
                    {
                        MessageBox.Show("Username already exists! Please choose a different username.",
                                      "Registration Failed",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                        return;
                    }

                    // Check duplicate email
                    string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @email";
                    SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, con);
                    checkEmailCmd.Parameters.AddWithValue("@email", email);
                    int emailExists = (int)checkEmailCmd.ExecuteScalar();

                    if (emailExists > 0)
                    {
                        MessageBox.Show("Email already registered! Please use a different email.",
                                      "Registration Failed",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                        return;
                    }

                    // 🔴 **یہیں تبدیلی کی گئی ہے** 🔴
                    // صرف Users table میں insert کریں، UserDetails table میں نہیں
                    string insertQuery = @"INSERT INTO Users (Username, Email, PasswordHash, Role, Phone, Address, CreatedAt)
                                           VALUES (@username, @email, @password, @role, @phone, @address, GETDATE());
                                           
                                           SELECT SCOPE_IDENTITY();"; // Get the UserID of newly inserted user

                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@role", "User");

                    // Phone اور Address کے لیے NULL values
                    cmd.Parameters.AddWithValue("@phone", DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", DBNull.Value);

                    // Get the new UserID
                    int newUserID = Convert.ToInt32(cmd.ExecuteScalar());

                    if (newUserID > 0)
                    {
                        // 🔴 **UserDetails table میں insert کرنے والا حصہ ہٹا دیا گیا ہے** 🔴

                        MessageBox.Show("🎉 Registration Successful!\n\n" +
                                      "✅ Account created successfully\n" +
                                      "✅ You can now login with your credentials\n" +
                                      "✅ User ID: " + newUserID,
                                      "Success",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        // Clear form
                        ClearForm();

                        // Option 1: Go to login form
                        Userlogin loginForm = new Userlogin();
                        loginForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed! Please try again.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void HandleSqlException(SqlException sqlEx)
        {
            if (sqlEx.Number == 2627) // Unique constraint violation
            {
                if (sqlEx.Message.Contains("Username"))
                {
                    MessageBox.Show("Username already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                }
                else if (sqlEx.Message.Contains("Email"))
                {
                    MessageBox.Show("Email already registered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                }
            }
            else if (sqlEx.Number == 18456) // Login failed
            {
                MessageBox.Show("Database connection failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (sqlEx.Message.Contains("Invalid object name"))
            {
                // 🔴 **یہاں درست کیا گیا ہے** 🔴
                if (sqlEx.Message.Contains("UserDetails"))
                {
                    // صرف Users table کا چیک کریں
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users'";
                            SqlCommand cmd = new SqlCommand(checkTableQuery, con);
                            int tableExists = (int)cmd.ExecuteScalar();

                            if (tableExists == 0)
                            {
                                MessageBox.Show("Users table not found!\n\nPlease create Users table first.",
                                              "Database Error",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("Registration successful! (UserDetails table not found but account created in Users table)",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Database Error: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Database tables not found!\n\nPlease run the database setup script first.",
                                  "Database Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            else if (sqlEx.Number == 4060) // Cannot open database
            {
                // 🔴 **Database نام درست کیا گیا ہے** 🔴
                MessageBox.Show("Cannot connect to database 'onlinestore'.\n\nPlease ensure:\n1. Database exists\n2. SQL Server is running\n3. Connection string is correct",
                              "Database Connection Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Database Error: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtUsername.Focus();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Go to Login form
            Userlogin login = new Userlogin();
            login.Show();
            this.Hide();
        }

        private void USER_REGISTRATION_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();

            // Set maximum lengths according to database schema
            txtUsername.MaxLength = 50;  // Based on Users table Username NVARCHAR(50)
            txtPassword.MaxLength = 255; // Based on Users table PasswordHash NVARCHAR(255)
            txtEmail.MaxLength = 100;    // Based on Users table Email NVARCHAR(100)

            // Test database connection
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if Users table exists
                    string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users'";
                    SqlCommand cmd = new SqlCommand(checkTableQuery, con);
                    int tableExists = (int)cmd.ExecuteScalar();

                    if (tableExists == 0)
                    {
                        MessageBox.Show("⚠ Warning: Users table not found in database.\n\nPlease run the database setup script first.",
                                      "Database Setup Required",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠ Database connection test failed:\n" + ex.Message,
                              "Connection Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            // Remove spaces from username
            if (txtUsername.Text.Contains(" "))
            {
                txtUsername.Text = txtUsername.Text.Replace(" ", "");
                txtUsername.SelectionStart = txtUsername.Text.Length;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            // Optional: Real-time email validation
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.ForeColor = IsValidEmail(txtEmail.Text) ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Show password strength
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                if (txtPassword.Text.Length < 6)
                {
                    txtPassword.ForeColor = System.Drawing.Color.Red;
                }
                else if (txtPassword.Text.Length < 10)
                {
                    txtPassword.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    txtPassword.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        // Optional: Add Enter key functionality
        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtEmail.Focus();
                e.Handled = true;
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                guna2Button1_Click(sender, e); // Trigger Register button
                e.Handled = true;
            }
        }

        // Optional: Add show/hide password functionality
        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            // You can add a button for this functionality
            txtPassword.PasswordChar = txtPassword.PasswordChar == '*' ? '\0' : '*';
        }
    }
}