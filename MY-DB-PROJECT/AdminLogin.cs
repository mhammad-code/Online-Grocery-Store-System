using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace MY_DB_PROJECT
{
    public partial class AdminLogin : Form
    {
        // Database connection string for grocerystore database
       // private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        // Alternative connection strings if needed:
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public AdminLogin()
        {
            InitializeComponent();

            // Set focus to username field
            txtUsername.Focus();

            // Load default credentials (for testing)
            LoadDefaultCredentials();

            // Test database connection on startup
            TestDatabaseConnection();
        }

        // Load default credentials for testing
        private void LoadDefaultCredentials()
        {
            // Uncomment for testing - pre-fill with default admin credentials
            // txtUsername.Text = "admin";
            // txtPassword.Text = "admin123";
        }

        // Test database connection on startup
        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if Users table exists
                    string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users'";
                    SqlCommand checkCmd = new SqlCommand(checkTableQuery, con);
                    int tableExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (tableExists == 0)
                    {
                        MessageBox.Show("⚠️ Warning: Users table not found in database!\n\nPlease run the database setup script.",
                                      "Database Warning",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Check if admin user exists
                        string checkAdminQuery = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";
                        SqlCommand adminCmd = new SqlCommand(checkAdminQuery, con);
                        int adminCount = Convert.ToInt32(adminCmd.ExecuteScalar());

                        if (adminCount == 0)
                        {
                            MessageBox.Show("⚠️ No admin users found in database.\n\nDefault admin credentials:\nUsername: admin\nPassword: admin123",
                                          "Admin Warning",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Don't show error on startup, just log it
                Console.WriteLine($"Database test failed: {ex.Message}");
            }
        }

        // Login button click
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                ShowValidationError("Please enter username!", txtUsername);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowValidationError("Please enter password!", txtPassword);
                return;
            }

            // Show loading indicator
            ShowLoading(true);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check admin credentials
                    string query = @"SELECT UserID, Username, Email, Phone FROM Users
                                     WHERE Username = @Username 
                                     AND PasswordHash = @Password 
                                     AND Role = 'Admin'";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int adminID = Convert.ToInt32(reader["UserID"]);
                            string loggedInAdmin = reader["Username"].ToString();
                            string email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                            string phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";

                            // Log login activity
                            LogLoginActivity(adminID, username);

                            // Show success message
                            ShowLoginSuccess(loggedInAdmin, email, phone);

                            // Open AdminDashboard form
                            AdminDashboard dashboard = new AdminDashboard(adminID);
                            dashboard.Show();
                            this.Hide();
                        }
                        else
                        {
                            ShowLoginFailed();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                HandleDatabaseError(sqlEx);
            }
            catch (Exception ex)
            {
                HandleGeneralError(ex);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        // Show validation error
        private void ShowValidationError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl.Focus();
            focusControl.BackColor = Color.LightPink;

            // Reset color after 2 seconds
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += (s, e) =>
            {
                focusControl.BackColor = Color.White;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        // Show loading indicator
        private void ShowLoading(bool isLoading)
        {
            if (isLoading)
            {
                guna2Button3.Text = "🔐 Logging in...";
                guna2Button3.Enabled = false;
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                guna2Button3.Text = "🔐 Login";
                guna2Button3.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        // Show login success message
        private void ShowLoginSuccess(string username, string email, string phone)
        {
            string message = $"✅ Welcome back, {username}!";

            if (!string.IsNullOrEmpty(email))
                message += $"\n📧 Email: {email}";

            if (!string.IsNullOrEmpty(phone))
                message += $"\n📞 Phone: {phone}";

            message += $"\n\n⏰ Login Time: {DateTime.Now.ToString("hh:mm tt")}";

            MessageBox.Show(message, "Login Successful",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Show login failed message
        private void ShowLoginFailed()
        {
            // Try to provide helpful suggestions
            string suggestions = "";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if username exists but password is wrong
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    SqlCommand checkCmd = new SqlCommand(checkUserQuery, con);
                    checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        // Check if user is not admin
                        string checkRoleQuery = "SELECT Role FROM Users WHERE Username = @Username";
                        SqlCommand roleCmd = new SqlCommand(checkRoleQuery, con);
                        roleCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        string role = roleCmd.ExecuteScalar()?.ToString();

                        if (role == "User")
                        {
                            suggestions = "\n\nThis account is a User account, not Admin.\nUse the User Login instead.";
                        }
                        else
                        {
                            suggestions = "\n\nIncorrect password. Try: admin123";
                        }
                    }
                    else
                    {
                        suggestions = "\n\nUsername not found. Try: admin";
                    }
                }
            }
            catch
            {
                suggestions = "\n\nTry using:\nUsername: admin\nPassword: admin123";
            }

            MessageBox.Show($"❌ Invalid admin credentials!{suggestions}",
                            "Login Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

            txtPassword.Clear();
            txtPassword.Focus();
        }

        // Handle database errors
        private void HandleDatabaseError(SqlException sqlEx)
        {
            string errorMessage = sqlEx.Message;
            string suggestion = "";

            if (errorMessage.Contains("Cannot open database"))
            {
                suggestion = "\n\nPlease check:\n1. Database 'grocerystore' exists\n2. SQL Server is running";
            }
            else if (errorMessage.Contains("Login failed"))
            {
                suggestion = "\n\nPlease check your SQL Server authentication settings.";
            }
            else if (errorMessage.Contains("Invalid object name"))
            {
                suggestion = "\n\nDatabase tables not found. Please run the setup script.";
            }

            MessageBox.Show($"Database Error: {errorMessage}{suggestion}",
                            "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Handle general errors
        private void HandleGeneralError(Exception ex)
        {
            MessageBox.Show($"Application Error: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Log login activity
        private void LogLoginActivity(int userID, string username)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Insert into login log table (create this table if needed)
                    string logQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LoginLog')
                        BEGIN
                            CREATE TABLE LoginLog (
                                LogID INT IDENTITY(1,1) PRIMARY KEY,
                                UserID INT,
                                Username NVARCHAR(50),
                                LoginTime DATETIME DEFAULT GETDATE(),
                                IPAddress NVARCHAR(50) DEFAULT 'N/A'
                            )
                        END
                        
                        INSERT INTO LoginLog (UserID, Username) 
                        VALUES (@UserID, @Username)";

                    SqlCommand cmd = new SqlCommand(logQuery, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Username", username);

                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail - logging is optional
            }
        }

        // Enter key in password field
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                guna2Button3_Click(sender, e);
                e.Handled = true;
            }
        }

        // Enter key in username field
        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        // Clear button click
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        // Exit button click
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?",
                                                "Confirm Exit",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        // Form load event
        private void AdminLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        // Register button click
        private void btnRegister_Click(object sender, EventArgs e)
        {
            USER_REGISTRATION regForm = new USER_REGISTRATION();
            regForm.Show();
            this.Hide();
        }

        // Test database connection button (if you have one)
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string message = $"✅ Database Connection Successful!\n\n" +
                                   $"📊 Database: grocerystore\n" +
                                   $"🖥️ Server: {con.DataSource}\n" +
                                   $"🔗 State: {con.State}\n\n" +
                                   $"📋 Checking tables...\n";

                    // Check important tables
                    string[] tables = { "Users", "Products", "Orders", "Suppliers" };
                    foreach (string table in tables)
                    {
                        string checkQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}'";
                        SqlCommand cmd = new SqlCommand(checkQuery, con);
                        int exists = Convert.ToInt32(cmd.ExecuteScalar());
                        message += $"   {(exists > 0 ? "✅" : "❌")} {table} table\n";
                    }

                    MessageBox.Show(message, "Connection Test",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Connection Failed!\n\nError: {ex.Message}\n\n" +
                              $"Connection String:\n{connectionString}",
                              "Connection Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Forgot password link (if you have one)
        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact system administrator to reset your password.\n\n" +
                          "Default Admin Credentials:\nUsername: admin\nPassword: admin123",
                          "Forgot Password",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Show/hide password checkbox (if you have one)
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // If you have a show password checkbox
            // txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        // Form closing event
        private void AdminLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                                                    "Exit Application",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        // Keyboard shortcuts
        private void AdminLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.L)
            {
                guna2Button3_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnExit_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F1)
            {
                btnTestConnection_Click(sender, e);
                e.Handled = true;
            }
        }

        private void AdminLogin_Load_1(object sender, EventArgs e)
        {

        }
    }
}