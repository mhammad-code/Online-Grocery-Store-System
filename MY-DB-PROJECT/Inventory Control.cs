using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class Inventory_Control : Form
    {
        // Database connection string for onlinestore database
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        // Alternative connection strings:
        // private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";
        // private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public Inventory_Control()
        {
            InitializeComponent();
        }

        private void Inventory_Control_Load(object sender, EventArgs e)
        {
            // 🔴 DASHBOARD STATISTICS WORKING ADD KI HAI 🔴
            LoadDashboardStats();
            LoadLowStockProducts();
            SetupDataGridView();
            SetupEventHandlers();

            // Test connection
            TestDatabaseConnection();
        }

        // Database connection test
        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    Console.WriteLine("✅ Database connection successful for Inventory Control");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Connection failed: {ex.Message}\n\nPlease check your database connection.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handlers setup
        private void SetupEventHandlers()
        {
            btnBack.Click += BtnBack_Click;
            // btnAddProduct aur btnViewProducts ke event handlers nahi add kiye (apke instruction ke mutabiq)
        }

        // Setup DataGridView columns
        private void SetupDataGridView()
        {
            try
            {
                // Clear existing columns
                dgvProducts.Columns.Clear();

                // Add columns for low stock products
                DataGridViewTextBoxColumn colID = new DataGridViewTextBoxColumn();
                colID.Name = "ProductID";
                colID.HeaderText = "PRODUCT ID";
                colID.Width = 100;

                DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                colName.Name = "ProductName";
                colName.HeaderText = "PRODUCT NAME";
                colName.Width = 200;

                DataGridViewTextBoxColumn colCategory = new DataGridViewTextBoxColumn();
                colCategory.Name = "Category";
                colCategory.HeaderText = "CATEGORY";
                colCategory.Width = 150;

                DataGridViewTextBoxColumn colStock = new DataGridViewTextBoxColumn();
                colStock.Name = "Stock";
                colStock.HeaderText = "STOCK";
                colStock.Width = 80;

                DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn();
                colPrice.Name = "Price";
                colPrice.HeaderText = "PRICE (PKR)";
                colPrice.Width = 120;

                DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                colStatus.Name = "Status";
                colStatus.HeaderText = "STATUS";
                colStatus.Width = 120;

                dgvProducts.Columns.AddRange(new DataGridViewColumn[] {
                    colID, colName, colCategory, colStock, colPrice, colStatus
                });

                // Configure DataGridView
                dgvProducts.ReadOnly = true;
                dgvProducts.AllowUserToAddRows = false;
                dgvProducts.AllowUserToDeleteRows = false;
                dgvProducts.AllowUserToResizeRows = false;
                dgvProducts.RowHeadersVisible = false;
                dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvProducts.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
                dgvProducts.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up DataGridView: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔴 DASHBOARD STATISTICS LOAD KARNE WALA METHOD 🔴
        private void LoadDashboardStats()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // 1. Total Products Count
                    string totalQuery = @"
                        SELECT COUNT(*) as TotalProducts 
                        FROM Products 
                        WHERE IsActive = 1";

                    SqlCommand totalCmd = new SqlCommand(totalQuery, con);
                    int totalProducts = Convert.ToInt32(totalCmd.ExecuteScalar());
                    lblTotalProducts.Text = totalProducts.ToString();
                    Console.WriteLine($"Total Products: {totalProducts}");

                    // 2. Low Stock Products (Stock < 10 and > 0)
                    string lowStockQuery = @"
                        SELECT COUNT(*) as LowStock 
                        FROM Products 
                        WHERE Stock > 0 AND Stock < 10 AND IsActive = 1";

                    SqlCommand lowStockCmd = new SqlCommand(lowStockQuery, con);
                    int lowStockCount = Convert.ToInt32(lowStockCmd.ExecuteScalar());
                    lblLowStock.Text = lowStockCount.ToString();
                    Console.WriteLine($"Low Stock Products: {lowStockCount}");

                    // 3. Out of Stock Products (Stock = 0)
                    string outOfStockQuery = @"
                        SELECT COUNT(*) as OutOfStock 
                        FROM Products 
                        WHERE Stock = 0 AND IsActive = 1";

                    SqlCommand outOfStockCmd = new SqlCommand(outOfStockQuery, con);
                    int outOfStockCount = Convert.ToInt32(outOfStockCmd.ExecuteScalar());
                    lblOutOfStock.Text = outOfStockCount.ToString();
                    Console.WriteLine($"Out of Stock Products: {outOfStockCount}");

                    // 4. Total Stock Value
                    string stockValueQuery = @"
                        SELECT ISNULL(SUM(Price * Stock), 0) as TotalValue 
                        FROM Products 
                        WHERE IsActive = 1";

                    SqlCommand stockValueCmd = new SqlCommand(stockValueQuery, con);
                    object result = stockValueCmd.ExecuteScalar();
                    decimal stockValue = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    lblStockValue.Text = $"PKR {stockValue:N2}";
                    Console.WriteLine($"Total Stock Value: PKR {stockValue:N2}");

                    // Update form title with stats
                    this.Text = $"Inventory Control - Products: {totalProducts} | Low Stock: {lowStockCount} | Out of Stock: {outOfStockCount}";
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error loading dashboard stats: {sqlEx.Message}\n\n" +
                              $"Make sure 'Products' table exists in 'onlinestore' database.",
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Default values show karein agar error aaye
                SetDefaultDashboardValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard stats: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetDefaultDashboardValues();
            }
        }

        // Default values agar database connection fail ho
        private void SetDefaultDashboardValues()
        {
            lblTotalProducts.Text = "0";
            lblLowStock.Text = "0";
            lblOutOfStock.Text = "0";
            lblStockValue.Text = "PKR 0.00";
        }

        // Load low stock and out of stock products
        private void LoadLowStockProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            p.ProductID,
                            p.ProductName,
                            c.CategoryName as Category,
                            p.Stock,
                            p.Price,
                            CASE 
                                WHEN p.Stock = 0 THEN 'Out of Stock'
                                WHEN p.Stock < 10 THEN 'Low Stock'
                                ELSE 'In Stock'
                            END as Status
                        FROM Products p
                        LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                        WHERE (p.Stock < 10 OR p.Stock = 0) AND p.IsActive = 1
                        ORDER BY 
                            CASE 
                                WHEN p.Stock = 0 THEN 1
                                WHEN p.Stock < 10 THEN 2
                                ELSE 3
                            END,
                            p.Stock ASC,
                            p.ProductName";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    dgvProducts.Rows.Clear();

                    int rowCount = 0;
                    while (reader.Read())
                    {
                        int rowIndex = dgvProducts.Rows.Add();
                        dgvProducts.Rows[rowIndex].Cells["ProductID"].Value = reader["ProductID"].ToString();
                        dgvProducts.Rows[rowIndex].Cells["ProductName"].Value = reader["ProductName"].ToString();
                        dgvProducts.Rows[rowIndex].Cells["Category"].Value = reader["Category"] != DBNull.Value ?
                            reader["Category"].ToString() : "Uncategorized";
                        dgvProducts.Rows[rowIndex].Cells["Stock"].Value = reader["Stock"].ToString();
                        dgvProducts.Rows[rowIndex].Cells["Price"].Value = Convert.ToDecimal(reader["Price"]).ToString("N2");
                        dgvProducts.Rows[rowIndex].Cells["Status"].Value = reader["Status"].ToString();

                        // Set row color based on status
                        string status = reader["Status"].ToString();
                        if (status == "Out of Stock")
                        {
                            dgvProducts.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                            dgvProducts.Rows[rowIndex].DefaultCellStyle.Font =
                                new System.Drawing.Font(dgvProducts.Font, System.Drawing.FontStyle.Bold);
                        }
                        else if (status == "Low Stock")
                        {
                            dgvProducts.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                            dgvProducts.Rows[rowIndex].DefaultCellStyle.Font =
                                new System.Drawing.Font(dgvProducts.Font, System.Drawing.FontStyle.Bold);
                        }

                        rowCount++;
                    }

                    reader.Close();

                    // Update label with count
                    label7.Text = $"📋 Low Stock Products ({rowCount})";
                    Console.WriteLine($"Loaded {rowCount} low/out of stock products");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error loading products: {sqlEx.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                label7.Text = "📋 Product List (Error)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                label7.Text = "📋 Product List (Error)";
            }
        }

        // Back button click - AdminDashboard mein wapis jaye
        private void BtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                // AdminDashboard form ko show karein
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error going back to Admin Dashboard: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // Refresh button (agar chahiye to add karein)
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardStats();
            LoadLowStockProducts();
            MessageBox.Show("Dashboard refreshed successfully!", "Refreshed",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Keyboard shortcuts
        private void Inventory_Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                LoadDashboardStats();
                LoadLowStockProducts();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnBack_Click(sender, e);
                e.Handled = true;
            }
        }

        // Form Closing Event
        private void Inventory_Control_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup if needed
        }

        // DataGridView Cell Click Event (agar context menu chahiye)
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Selected product ki information show karein
                string productID = dgvProducts.Rows[e.RowIndex].Cells["ProductID"].Value?.ToString();
                string productName = dgvProducts.Rows[e.RowIndex].Cells["ProductName"].Value?.ToString();
                string status = dgvProducts.Rows[e.RowIndex].Cells["Status"].Value?.ToString();

               
            }
        }
    }
}