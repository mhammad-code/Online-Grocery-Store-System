using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class ProductCatalog : Form
    {
        // 🔴 **YAHAN CONNECTION STRING CHANGE KARO**
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";
        private DataTable allProducts;

        public ProductCatalog()
        {
            InitializeComponent();
            dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;
        }

        private void ProductCatalog_Load(object sender, EventArgs e)
        {
            LoadAndSetupData();
        }

        private void LoadAndSetupData()
        {
            try
            {
                allProducts = LoadAllProductsFromDatabase();

                if (allProducts != null)
                {
                    PopulateComboBoxWithCategories();
                    DisplayAllProducts();
                }
                else
                {
                    MessageBox.Show("No products found in the database.",
                                  "No Products",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private DataTable LoadAllProductsFromDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // 🟢 **YEH QUERY SAHI HAI, KOI CHANGE NAHI KARNA**
                    string query = @"
                        SELECT 
                            P.ProductID,
                            P.ProductName,
                            P.Price,
                            P.Stock,
                            P.IsActive,
                            C.CategoryName
                        FROM Products P
                        INNER JOIN Categories C ON P.CategoryID = C.CategoryID
                        WHERE P.IsActive = 1
                        ORDER BY C.CategoryName, P.ProductName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return null;
            }
        }

        private void PopulateComboBoxWithCategories()
        {
            try
            {
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");

                if (allProducts != null && allProducts.Rows.Count > 0)
                {
                    DataView distinctCategories = new DataView(allProducts);
                    DataTable dtCategories = distinctCategories.ToTable(true, "CategoryName");

                    foreach (DataRow row in dtCategories.Rows)
                    {
                        string category = row["CategoryName"].ToString();
                        if (!string.IsNullOrWhiteSpace(category))
                        {
                            cmbCategory.Items.Add(category);
                        }
                    }
                }

                cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating categories: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
            }
        }

        private void DisplayAllProducts()
        {
            try
            {
                dgvProducts.DataSource = null;

                if (allProducts == null || allProducts.Rows.Count == 0)
                {
                    MessageBox.Show("No products available.",
                                  "Information",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    return;
                }

                // Create a copy for display
                DataTable displayTable = allProducts.Clone();
                foreach (DataRow row in allProducts.Rows)
                {
                    displayTable.ImportRow(row);
                }

                dgvProducts.DataSource = displayTable;
                ApplyGridStyling();
                ApplyStockColorCoding();

                this.Text = $"Product Catalog - {dgvProducts.Rows.Count} Products";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying products: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ApplyGridStyling()
        {
            if (dgvProducts.Columns.Count == 0) return;

            // Set column headers
            if (dgvProducts.Columns.Contains("ProductID"))
            {
                dgvProducts.Columns["ProductID"].HeaderText = "PRODUCT ID";
                dgvProducts.Columns["ProductID"].Width = 100;
                dgvProducts.Columns["ProductID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvProducts.Columns.Contains("ProductName"))
            {
                dgvProducts.Columns["ProductName"].HeaderText = "PRODUCT NAME";
                dgvProducts.Columns["ProductName"].Width = 250;
                dgvProducts.Columns["ProductName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvProducts.Columns.Contains("CategoryName"))
            {
                dgvProducts.Columns["CategoryName"].HeaderText = "CATEGORY";
                dgvProducts.Columns["CategoryName"].Width = 150;
                dgvProducts.Columns["CategoryName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvProducts.Columns.Contains("Price"))
            {
                dgvProducts.Columns["Price"].HeaderText = "PRICE (PKR)";
                dgvProducts.Columns["Price"].Width = 120;
                dgvProducts.Columns["Price"].DefaultCellStyle.Format = "N2";
                dgvProducts.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvProducts.Columns["Price"].DefaultCellStyle.ForeColor = Color.FromArgb(40, 167, 69); // Green for price
                dgvProducts.Columns["Price"].DefaultCellStyle.Font = new Font(dgvProducts.Font, FontStyle.Bold);
            }

            if (dgvProducts.Columns.Contains("Stock"))
            {
                dgvProducts.Columns["Stock"].HeaderText = "STOCK";
                dgvProducts.Columns["Stock"].Width = 100;
                dgvProducts.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Hide IsActive column if present
            if (dgvProducts.Columns.Contains("IsActive"))
            {
                dgvProducts.Columns["IsActive"].Visible = false;
            }

            // Style the DataGridView
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Header styling
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProducts.ColumnHeadersHeight = 40;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row styling
            dgvProducts.RowTemplate.Height = 35;
            dgvProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void ApplyStockColorCoding()
        {
            if (!dgvProducts.Columns.Contains("Stock")) return;

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["Stock"].Value != null)
                {
                    if (int.TryParse(row.Cells["Stock"].Value.ToString(), out int stock))
                    {
                        if (stock > 10)
                        {
                            // Good stock - green
                            row.Cells["Stock"].Style.ForeColor = Color.FromArgb(40, 167, 69);
                            row.Cells["Stock"].Style.Font = new Font(dgvProducts.Font, FontStyle.Bold);
                        }
                        else if (stock > 0)
                        {
                            // Low stock - orange
                            row.Cells["Stock"].Style.ForeColor = Color.FromArgb(255, 193, 7);
                            row.Cells["Stock"].Style.Font = new Font(dgvProducts.Font, FontStyle.Bold);
                        }
                        else
                        {
                            // Out of stock - red
                            row.Cells["Stock"].Style.ForeColor = Color.FromArgb(220, 53, 69);
                            row.Cells["Stock"].Style.Font = new Font(dgvProducts.Font, FontStyle.Bold);
                            row.Cells["Stock"].Style.BackColor = Color.FromArgb(255, 245, 245);
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            try
            {
                if (allProducts == null || allProducts.Rows.Count == 0)
                {
                    MessageBox.Show("No products available.",
                                  "Information",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    return;
                }

                string selected = cmbCategory.Text;

                DataTable filteredTable;

                if (selected == "All Categories")
                {
                    filteredTable = allProducts.Copy();
                }
                else
                {
                    // CORRECTED: Use DataView to filter
                    DataView dv = allProducts.DefaultView;
                    dv.RowFilter = $"CategoryName = '{selected.Replace("'", "''")}'";
                    filteredTable = dv.ToTable();
                }

                // Show all products including out of stock (remove stock filter)
                dgvProducts.DataSource = filteredTable;
                ApplyGridStyling();
                ApplyStockColorCoding();

                // Update window title with count
                int totalProducts = dgvProducts.Rows.Count;
                int inStockCount = 0;
                int lowStockCount = 0;
                int outOfStockCount = 0;

                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    if (row.Cells["Stock"].Value != null &&
                        int.TryParse(row.Cells["Stock"].Value.ToString(), out int stock))
                    {
                        if (stock > 10) inStockCount++;
                        else if (stock > 0) lowStockCount++;
                        else outOfStockCount++;
                    }
                }

                this.Text = $"Product Catalog - {totalProducts} Products ({inStockCount} In Stock, {lowStockCount} Low, {outOfStockCount} Out)";

                if (dgvProducts.Rows.Count == 0)
                {
                    MessageBox.Show($"No products found in category '{selected}'.",
                                  "No Results",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if user is logged in (userID available)
                // Agar userID save kiya hai form mein to use karo

                // Pehle is form ko hide karo
                this.Hide();

                // UserDashboard open karo
                // Agar userID chahiye
                                                                         // Ya: UserDashboard userDashboard = new UserDashboard(); // Agar userID nahi chahiye

               

                // Optional: Is form ko close karna hai ya nahi?
                // this.Close(); // Agar close karna hai
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening dashboard: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvProducts.Rows.Count)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                string productId = row.Cells["ProductID"].Value?.ToString() ?? "N/A";
                string productName = row.Cells["ProductName"].Value?.ToString() ?? "N/A";
                string category = row.Cells["CategoryName"].Value?.ToString() ?? "N/A";
                string price = row.Cells["Price"].Value?.ToString() ?? "0.00";
                string stock = row.Cells["Stock"].Value?.ToString() ?? "0";

                if (!decimal.TryParse(price, out decimal priceValue))
                    priceValue = 0;

                if (!int.TryParse(stock, out int stockValue))
                    stockValue = 0;

                string stockStatus = "🟢 In Stock";
                if (stockValue > 10)
                {
                    stockStatus = "🟢 In Stock (Good)";
                }
                else if (stockValue > 0)
                {
                    stockStatus = "🟡 Low Stock";
                }
                else
                {
                    stockStatus = "🔴 Out of Stock";
                }

                string details = "📦 PRODUCT DETAILS\n";
                details += "═══════════════════════\n\n";
                details += $"🆔 Product ID: {productId}\n";
                details += $"📝 Product Name: {productName}\n";
                details += $"🏷️ Category: {category}\n";
                details += $"💰 Price: PKR {priceValue:N2}\n";
                details += $"📊 Stock: {stockValue} units\n";
                details += $"📈 Status: {stockStatus}\n\n";
                details += "═══════════════════════\n";

                MessageBox.Show(details,
                              "Product Information",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAndSetupData();
            cmbCategory.SelectedIndex = 0;
            MessageBox.Show("Product catalog refreshed successfully!",
                          "Refresh Complete",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        // Optional: Add category change handler for auto-filter
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Uncomment if you want auto-filter on category change
            // ApplyFilter();
        }

        // Optional: Add key shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnRefresh_Click(null, null);
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Optional: Add context menu for right-click
        private void dgvProducts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvProducts.ClearSelection();
                dgvProducts.Rows[e.RowIndex].Selected = true;

                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripMenuItem viewDetailsItem = new ToolStripMenuItem("View Details");
                ToolStripMenuItem copyNameItem = new ToolStripMenuItem("Copy Product Name");

                viewDetailsItem.Click += (s, ev) => dgvProducts_CellDoubleClick(sender,
                    new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));

                copyNameItem.Click += (s, ev) =>
                {
                    if (dgvProducts.SelectedRows.Count > 0)
                    {
                        string productName = dgvProducts.SelectedRows[0].Cells["ProductName"].Value?.ToString();
                        if (!string.IsNullOrEmpty(productName))
                        {
                            Clipboard.SetText(productName);
                            MessageBox.Show("Product name copied to clipboard.",
                                          "Copied",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                        }
                    }
                };

                contextMenu.Items.Add(viewDetailsItem);
                contextMenu.Items.Add(copyNameItem);
                contextMenu.Show(dgvProducts, e.Location);
            }
        }
    }
}