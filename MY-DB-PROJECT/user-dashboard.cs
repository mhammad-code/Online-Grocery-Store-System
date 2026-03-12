using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class user_dashboard : Form
    {
        private int userID;
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public user_dashboard(int userID)
        {
            try
            {
                InitializeComponent();
                this.userID = userID;

                // Initialize statistics labels
                lblTotalOrders = new Label();
                lblPendingOrders = new Label();
                lblCompletedOrders = new Label();
                btnRefresh = new Button();
                btnExit = new Button();

                SetupStatisticsLabels();

                // Load user's name for welcome message
                LoadUserName();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing dashboard: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserName()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT Username FROM Users WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string username = result.ToString();
                        label1.Text = $"Welcome Dear {username}";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Silently fail - keep default welcome message
                Console.WriteLine($"Error loading username: {ex.Message}");
            }
        }

        private void user_dashboard_Load(object sender, EventArgs e)
        {
            // Load initial statistics from database
            LoadOrderStatistics();
        }

        private void SetupStatisticsLabels()
        {
            // Total Orders Label
            lblTotalOrders.AutoSize = true;
            lblTotalOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblTotalOrders.ForeColor = System.Drawing.Color.DarkBlue;
            lblTotalOrders.Location = new System.Drawing.Point(50, 580);
            lblTotalOrders.Text = "Total Orders: Loading...";
            this.Controls.Add(lblTotalOrders);

            // Pending Orders Label
            lblPendingOrders.AutoSize = true;
            lblPendingOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblPendingOrders.ForeColor = System.Drawing.Color.Orange;
            lblPendingOrders.Location = new System.Drawing.Point(250, 580);
            lblPendingOrders.Text = "Pending Orders: Loading...";
            this.Controls.Add(lblPendingOrders);

            // Completed Orders Label
            lblCompletedOrders.AutoSize = true;
            lblCompletedOrders.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblCompletedOrders.ForeColor = System.Drawing.Color.Green;
            lblCompletedOrders.Location = new System.Drawing.Point(500, 580);
            lblCompletedOrders.Text = "Completed Orders: Loading...";
            this.Controls.Add(lblCompletedOrders);

            // Refresh Button
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnRefresh.Location = new System.Drawing.Point(800, 575);
            btnRefresh.Size = new System.Drawing.Size(100, 30);
            btnRefresh.BackColor = System.Drawing.Color.LightBlue;
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // Exit Button
            btnExit.Text = "❌ Exit";
            btnExit.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnExit.Location = new System.Drawing.Point(950, 575);
            btnExit.Size = new System.Drawing.Size(100, 30);
            btnExit.BackColor = System.Drawing.Color.LightCoral;
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);
        }

        private void LoadOrderStatistics()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if Orders table exists
                    string checkTable = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders'";
                    SqlCommand checkCmd = new SqlCommand(checkTable, con);
                    int tableExists = (int)checkCmd.ExecuteScalar();

                    if (tableExists > 0)
                    {
                        // Total Orders Count
                        string totalQuery = "SELECT COUNT(*) FROM Orders WHERE UserID = @UserID";
                        SqlCommand totalCmd = new SqlCommand(totalQuery, con);
                        totalCmd.Parameters.AddWithValue("@UserID", userID);
                        int totalOrders = Convert.ToInt32(totalCmd.ExecuteScalar());

                        // Pending Orders Count
                        string pendingQuery = "SELECT COUNT(*) FROM Orders WHERE UserID = @UserID AND OrderStatus = 'Pending'";
                        SqlCommand pendingCmd = new SqlCommand(pendingQuery, con);
                        pendingCmd.Parameters.AddWithValue("@UserID", userID);
                        int pendingOrders = Convert.ToInt32(pendingCmd.ExecuteScalar());

                        // Completed Orders Count
                        string completedQuery = "SELECT COUNT(*) FROM Orders WHERE UserID = @UserID AND OrderStatus = 'Completed'";
                        SqlCommand completedCmd = new SqlCommand(completedQuery, con);
                        completedCmd.Parameters.AddWithValue("@UserID", userID);
                        int completedOrders = Convert.ToInt32(completedCmd.ExecuteScalar());

                        lblTotalOrders.Text = $"✅ Total Orders: {totalOrders}";
                        lblPendingOrders.Text = $"⏳ Pending Orders: {pendingOrders}";
                        lblCompletedOrders.Text = $"🎉 Completed Orders: {completedOrders}";
                    }
                    else
                    {
                        // If Orders table doesn't exist yet
                        lblTotalOrders.Text = "✅ Total Orders: 0";
                        lblPendingOrders.Text = "⏳ Pending Orders: 0";
                        lblCompletedOrders.Text = "🎉 Completed Orders: 0";
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Fallback to sample data if database error
                lblTotalOrders.Text = "✅ Total Orders: 15";
                lblPendingOrders.Text = "⏳ Pending Orders: 5";
                lblCompletedOrders.Text = "🎉 Completed Orders: 10";

                MessageBox.Show($"Using sample data. Database error: {ex.Message}",
                              "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Open Product Catalog form
            try
            {
                ProductCatalog product = new ProductCatalog();
                product.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Product Catalog: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Open PlaceOrderForm
            try
            {
                PlaceOrderForm placeOrderForm = new PlaceOrderForm(userID);
                placeOrderForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Place Order form: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Open Order History form
            try
            {
                OrderHistory orderHistory = new OrderHistory(userID);
                orderHistory.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Open Feedback form with user information
            try
            {
                // Get current user's username
                string username = "";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT Username FROM Users WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        username = result.ToString();

                    con.Close();
                }

                // Open Feedback form with user info
                Feedback feedbackForm = new Feedback(userID, username);
                feedbackForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Feedback form: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Logout and return to login form
            DialogResult result = MessageBox.Show("Are you sure you want to logout?",
                                                "Confirm Logout",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Userlogin loginForm = new Userlogin();
                loginForm.Show();
                this.Close();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // Refresh statistics from database
            LoadOrderStatistics();
            MessageBox.Show("Statistics refreshed!",
                          "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            // Exit application with confirmation
            DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                                                "Confirm Exit",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Form closing event
        private void user_dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to logout before closing?",
                                                "Confirm",
                                                MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Userlogin login = new Userlogin();
                login.Show();
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}