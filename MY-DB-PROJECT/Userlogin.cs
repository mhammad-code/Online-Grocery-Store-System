using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using WindowsFormsApp2;

namespace MY_DB_PROJECT
{
    public partial class Userlogin : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public Userlogin()
        {
            InitializeComponent();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter username", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Additional validations
            if (username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long",
                              "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                txtUsername.SelectAll();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long",
                              "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                txtPassword.SelectAll();
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // 🔴 **یہاں تبدیلی کی گئی ہے - IsActive = 1 condition ہٹائی گئی ہے** 🔴
                    string query = @"SELECT 
                                        UserID, 
                                        Username, 
                                        Email, 
                                        Role,
                                        CreatedAt
                                     FROM Users 
                                     WHERE Username = @Username 
                                     AND PasswordHash = @Password";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userID = Convert.ToInt32(reader["UserID"]);
                            string loggedInUsername = reader["Username"].ToString();
                            string email = reader["Email"].ToString();
                            string role = reader["Role"].ToString();
                            DateTime createdAt = Convert.ToDateTime(reader["CreatedAt"]);

                            // Log successful login attempt
                            LogLoginAttempt(username, "Success", con);

                            MessageBox.Show($"🎉 Welcome back, {loggedInUsername}!\n\n" +
                                          $"📧 Email: {email}\n" +
                                          $"👤 Role: {role}\n" +
                                          $"📅 Member since: {createdAt:dd-MMM-yyyy}",
                                          "Login Successful",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);

                            // Clear password field for security
                            txtPassword.Clear();

                            // Open dashboard based on role
                            this.Hide();

                            if (role == "Admin")
                            {
                                AdminDashboard adminDashboard = new AdminDashboard(userID);
                                adminDashboard.Closed += (s, args) => this.Close();
                                adminDashboard.Show();
                            }
                            else
                            {
                                user_dashboard userDashboard = new user_dashboard(userID);
                                userDashboard.Closed += (s, args) => this.Close();
                                userDashboard.Show();
                            }
                        }
                        else
                        {
                            // Log failed login attempt
                            LogLoginAttempt(username, "Failed - Invalid credentials", con);

                            MessageBox.Show("❌ Invalid username or password!\n\n" +
                                          "🔍 Please check:\n" +
                                          "• Username spelling\n" +
                                          "• Caps Lock status\n" +
                                          "• Password accuracy\n\n" +
                                          "📋 Try these demo credentials:\n" +
                                          "👑 Admin: username=admin, password=admin123\n" +
                                          "👤 User: username=user1, password=user123",
                                          "Login Failed",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);

                            txtPassword.Clear();
                            txtPassword.Focus();

                            // Shake effect for wrong credentials
                            ShakeForm();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠ Application Error: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void LogLoginAttempt(string username, string status, SqlConnection con)
        {
            try
            {
                string logQuery = @"INSERT INTO LoginLogs (Username, LoginTime, Status, IPAddress)
                                   VALUES (@Username, GETDATE(), @Status, @IPAddress)";

                // Check if LoginLogs table exists
                string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LoginLogs'";
                SqlCommand checkCmd = new SqlCommand(checkTableQuery, con);
                int tableExists = (int)checkCmd.ExecuteScalar();

                if (tableExists > 0)
                {
                    SqlCommand logCmd = new SqlCommand(logQuery, con);
                    logCmd.Parameters.AddWithValue("@Username", username);
                    logCmd.Parameters.AddWithValue("@Status", status);
                    logCmd.Parameters.AddWithValue("@IPAddress", GetIPAddress());
                    logCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }

        private string GetIPAddress()
        {
            try
            {
                string hostName = System.Net.Dns.GetHostName();
                var addresses = System.Net.Dns.GetHostAddresses(hostName);
                foreach (var ip in addresses)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        private void HandleSqlException(SqlException sqlEx)
        {
            // 🔴 **یہاں تبدیلی کی گئی ہے - Database نام درست کیا گیا ہے** 🔴
            if (sqlEx.Message.Contains("Invalid object name 'Users'"))
            {
                MessageBox.Show("⚠ Database Configuration Error!\n\n" +
                              "The Users table was not found.\n" +
                              "Please ensure:\n" +
                              "1. Database 'onlinestore' exists\n" + 
                              "2. Database setup script has been run\n" +
                              "3. Tables are properly created",
                              "System Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (sqlEx.Number == 18456)
            {
                MessageBox.Show("⚠ Database Connection Failed!\n\n" +
                              "Cannot connect to SQL Server.\n" +
                              "Please ensure:\n" +
                              "1. SQL Server is running\n" +
                              "2. Instance name is correct\n" +
                              "3. Windows Authentication is enabled",
                              "Connection Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (sqlEx.Number == 4060)
            {
                
                MessageBox.Show("⚠ Database Not Found!\n\n" +
                              "Database 'onlinestore' does not exist.\n" + 
                              "Please create the database first.",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (sqlEx.Message.Contains("invalid column name"))
            {
                if (sqlEx.Message.Contains("IsActive"))
                {
                    // خاص طور پر IsActive column not found والی error کے لیے
                    MessageBox.Show("⚠ Login successful but IsActive column not found.\n" +
                                  "Please remove IsActive condition from login query.",
                                  "Schema Warning",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("⚠ Database Schema Error!\n\n" +
                                  "Database tables are missing required columns.\n" +
                                  "Please run the database setup script again.",
                                  "Schema Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"⚠ Database Error ({sqlEx.Number}):\n{sqlEx.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ShakeForm()
        {
            var original = this.Location;
            var rnd = new Random(1337);
            const int shakeAmplitude = 10;

            for (int i = 0; i < 10; i++)
            {
                this.Location = new System.Drawing.Point(
                    original.X + rnd.Next(-shakeAmplitude, shakeAmplitude),
                    original.Y + rnd.Next(-shakeAmplitude, shakeAmplitude)
                );
                System.Threading.Thread.Sleep(20);
            }
            this.Location = original;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                guna2Button3_Click(sender, e);
                e.Handled = true;
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        // Method to handle Clear button (if you add one)
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        // Method to handle Exit button (if you add one)
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?",
                                                "Confirm Exit",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Method to handle Register button (if you add one)
        private void btnRegister_Click(object sender, EventArgs e)
        {
            USER_REGISTRATION regForm = new USER_REGISTRATION();
            regForm.Show();
            this.Hide();
        }

        private void Userlogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();

            // Set maximum lengths according to database schema
            txtUsername.MaxLength = 50;  // NVARCHAR(50)
            txtPassword.MaxLength = 255; // NVARCHAR(255)

            // Test database connection on load
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
                        MessageBox.Show("⚠ Warning: Users table not found!\n\n" +
                                      "Please run the database setup script first.\n" +
                                      "Using demo credentials may not work.",
                                      "Database Setup Required",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Check if there are any users
                        string checkUsersQuery = "SELECT COUNT(*) FROM Users";
                        cmd.CommandText = checkUsersQuery;
                        int userCount = (int)cmd.ExecuteScalar();

                        if (userCount == 0)
                        {
                            MessageBox.Show("⚠ No users found in database!\n\n" +
                                          "Please register first or use default credentials.\n" +
                                          "Default Admin: admin/admin123",
                                          "No Users Found",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠ Database Connection Test Failed:\n" + ex.Message,
                              "Connection Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
            }
        }

        // Optional: Add a show/hide password functionality
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = txtPassword.PasswordChar == '*' ? '\0' : '*';
            // You'll need to add this button to your form designer
        }

        // Optional: Forgot password functionality
       
        // Form closing event
        private void Userlogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                                                "Exit Confirmation",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

       
    }
}