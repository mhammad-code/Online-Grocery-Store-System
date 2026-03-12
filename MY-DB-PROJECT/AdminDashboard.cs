using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class AdminDashboard : Form
    {
        private int currentAdminID;  // Store the admin/user ID
        private string adminUsername;

        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";


        // Alternative connection strings if needed:
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        // private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        // DEFAULT CONSTRUCTOR (for parameterless calls)
        public AdminDashboard()
        {
            InitializeComponent();
            this.currentAdminID = 0;
            this.adminUsername = "Administrator";
            this.Text = "Admin Dashboard";
            LoadAdminStatistics();
        }

        // PARAMETERIZED CONSTRUCTOR (for AdminLogin.cs line 63)
        public AdminDashboard(int adminID)
        {
            InitializeComponent();
            this.currentAdminID = adminID;
            LoadAdminInfo(adminID);
            LoadAdminStatistics();
        }

        // Load admin information from database
        private void LoadAdminInfo(int adminID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT Username, Email FROM Users WHERE UserID = @UserID AND Role = 'Admin'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", adminID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.adminUsername = reader["Username"].ToString();
                            string email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";

                            this.Text = $"Admin Dashboard - Welcome, {adminUsername}!";

                            // Optional: Update status labels
                            UpdateStatusLabels(adminUsername, email);
                        }
                        else
                        {
                            this.adminUsername = "Administrator";
                            this.Text = "Admin Dashboard";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading admin info: {ex.Message}");
                this.adminUsername = "Administrator";
                this.Text = "Admin Dashboard";
            }
        }

        // Load admin dashboard statistics
        private void LoadAdminStatistics()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Get total orders count
                    string ordersQuery = "SELECT COUNT(*) FROM Orders";
                    SqlCommand ordersCmd = new SqlCommand(ordersQuery, con);
                    int totalOrders = Convert.ToInt32(ordersCmd.ExecuteScalar());

                    // Get total products count
                    string productsQuery = "SELECT COUNT(*) FROM Products WHERE IsActive = 1";
                    SqlCommand productsCmd = new SqlCommand(productsQuery, con);
                    int totalProducts = Convert.ToInt32(productsCmd.ExecuteScalar());

                    // Get total suppliers count
                    string suppliersQuery = "SELECT COUNT(*) FROM Suppliers";
                    SqlCommand suppliersCmd = new SqlCommand(suppliersQuery, con);
                    int totalSuppliers = Convert.ToInt32(suppliersCmd.ExecuteScalar());

                    // Get today's orders
                    string todayOrdersQuery = "SELECT COUNT(*) FROM Orders WHERE CONVERT(DATE, OrderDate) = CONVERT(DATE, GETDATE())";
                    SqlCommand todayOrdersCmd = new SqlCommand(todayOrdersQuery, con);
                    int todayOrders = Convert.ToInt32(todayOrdersCmd.ExecuteScalar());

                    // Update UI with statistics (if you have labels for these)
                    UpdateStatisticsUI(totalOrders, totalProducts, totalSuppliers, todayOrders);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading statistics: {ex.Message}");
                // Silently fail - statistics are not critical
            }
        }

        // Update UI with statistics (if you have labels on your form)
        private void UpdateStatisticsUI(int totalOrders, int totalProducts, int totalSuppliers, int todayOrders)
        {
            // If you have labels on your AdminDashboard form to show stats, update them here
            // Example:
            // lblTotalOrders.Text = $"Total Orders: {totalOrders}";
            // lblTotalProducts.Text = $"Products: {totalProducts}";
            // lblTotalSuppliers.Text = $"Suppliers: {totalSuppliers}";
            // lblTodayOrders.Text = $"Today's Orders: {todayOrders}";
        }

        // Update status labels (optional)
        private void UpdateStatusLabels(string username, string email)
        {
            // If you have status labels on your form
            // lblLoggedInUser.Text = $"Logged in as: {username}";
            // lblUserEmail.Text = $"Email: {email}";
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            // Optional: initialization code
            if (currentAdminID > 0)
            {
                // You can load admin-specific data here if needed
            }

            // Center the form on screen
            this.CenterToScreen();
        }

        // Open Order Dashboard
        private void btnOrderDashboard_Click(object sender, EventArgs e)
        {
            OrderDashboard orderDashboard = new OrderDashboard();
            orderDashboard.Show();
            this.Hide();
        }

        // Open Inventory Control
        private void btnInventoryControl_Click(object sender, EventArgs e)
        {
            Inventory_Control inventory = new Inventory_Control();
            inventory.Show();
            this.Hide();
        }

        // Open Supplier Network
        private void btnSupplierNetwork_Click(object sender, EventArgs e)
        {
            Supplier_Management supplierNetwork = new Supplier_Management();
            supplierNetwork.Show();
            this.Hide();
        }

        // Open Profit Check
        private void btnProfitCheck_Click(object sender, EventArgs e)
        {
            Check_Profit profitCheck = new Check_Profit();
            profitCheck.Show();
            this.Hide();
        }

        // Logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?",
                                                  "Logout Confirmation",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Log logout activity (optional)
                LogAdminActivity("Logged out");

                // Go back to AdminLogin form
                AdminLogin loginForm = new AdminLogin();
                loginForm.Show();
                this.Close();
            }
        }

        // Open Staff Management
        private void btnStaffManagement_Click(object sender, EventArgs e)
        {
            Staff_Management staffManagement = new Staff_Management();
            staffManagement.Show();
            this.Hide();
        }

        // Property to access admin ID if needed
        public int CurrentAdminID
        {
            get { return currentAdminID; }
        }

        // Property to access admin username
        public string AdminUsername
        {
            get { return adminUsername; }
        }

        // Log admin activity (optional - for auditing)
        private void LogAdminActivity(string activity)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO AdminActivityLog (AdminID, Username, Activity, ActivityDate) 
                                   VALUES (@AdminID, @Username, @Activity, GETDATE())";

                    // Note: You need to create AdminActivityLog table first
                    /*
                    CREATE TABLE AdminActivityLog (
                        LogID INT IDENTITY(1,1) PRIMARY KEY,
                        AdminID INT NOT NULL,
                        Username NVARCHAR(50),
                        Activity NVARCHAR(255),
                        ActivityDate DATETIME DEFAULT GETDATE()
                    )
                    */

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AdminID", currentAdminID);
                    cmd.Parameters.AddWithValue("@Username", adminUsername);
                    cmd.Parameters.AddWithValue("@Activity", activity);

                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail - logging is optional
            }
        }

        // Form closing event
        private void AdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Optional: Confirm exit
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit the Admin Dashboard?",
                                                    "Exit Confirmation",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    // Log logout activity when closing
                    if (currentAdminID > 0)
                    {
                        LogAdminActivity("Application closed");
                    }
                }
            }
        }

        // Refresh dashboard button click (if you have a refresh button)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAdminStatistics();
            MessageBox.Show("Dashboard statistics refreshed!", "Refreshed",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Keyboard shortcuts
        private void AdminDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                btnOrderDashboard_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                btnInventoryControl_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                btnSupplierNetwork_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                btnProfitCheck_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.T)
            {
                btnStaffManagement_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnLogout_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                LoadAdminStatistics();
                e.Handled = true;
            }
        }

        // Test database connection (optional - if you have a test button)
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    MessageBox.Show($"✅ Database connection successful!\n\n" +
                                  $"Database: grocerystore\n" +
                                  $"Server: {con.DataSource}\n" +
                                  $"Admin: {adminUsername}",
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OrderDashboard order=new OrderDashboard();
            order.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Inventory_Control inventory = new Inventory_Control();
            inventory.Show();
            this.Hide();
        }
    }
}