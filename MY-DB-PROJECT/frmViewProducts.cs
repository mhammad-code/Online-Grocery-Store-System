using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class frmViewProducts : Form
    {
        // آپ کا connection string
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private DataTable productsTable;

        public frmViewProducts()
        {
            InitializeComponent();
            LoadCategoriesForFilter();
            LoadProducts();
            SetupDataGridView();
        }

        // Load categories for filter dropdown
        private void LoadCategoriesForFilter()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // آپ کے DB میں Categories ٹیبل سے categories load کریں
                    string query = "SELECT CategoryName FROM Categories ORDER BY CategoryName";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmbFilter.Items.Clear();
                    cmbFilter.Items.Add("All Categories"); // Default option

                    while (reader.Read())
                    {
                        cmbFilter.Items.Add(reader["CategoryName"].ToString());
                    }

                    reader.Close();
                    cmbFilter.SelectedIndex = 0; // Default: All Categories
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbFilter.Items.Add("All Categories");
                cmbFilter.SelectedIndex = 0;
            }
        }

        // Load products from database
        private void LoadProducts(string categoryFilter = "All Categories", string searchText = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // آپ کے DB ڈیزائن کے مطابق query:
                    // Products ٹیبل: ProductID, CategoryID, ProductName, Price, Stock, IsActive
                    // Categories ٹیبل: CategoryID, CategoryName

                    string query = @"
                        SELECT 
                            p.ProductID AS 'PRODUCT ID',
                            p.ProductName AS 'PRODUCT NAME',
                            c.CategoryName AS 'CATEGORY',
                            p.Price AS 'PRICE (PKR)',
                            p.Stock AS 'CURRENT STOCK',
                            CASE 
                                WHEN p.Stock <= 0 THEN 'Out of Stock'
                                WHEN p.Stock < 10 THEN 'Low Stock'
                                WHEN p.IsActive = 0 THEN 'Inactive'
                                ELSE 'In Stock'
                            END AS 'STATUS'
                        FROM Products p
                        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                        WHERE 1=1";

                    // Add category filter if not "All Categories"
                    if (categoryFilter != "All Categories")
                        query += " AND c.CategoryName = @CategoryFilter";

                    // Add search filter if search text provided
                    if (!string.IsNullOrWhiteSpace(searchText))
                        query += " AND (p.ProductName LIKE @SearchText OR p.ProductID LIKE @SearchText)";

                    // Order by ProductID descending (newest first)
                    query += " ORDER BY p.ProductID DESC";

                    SqlCommand cmd = new SqlCommand(query, con);

                    if (categoryFilter != "All Categories")
                        cmd.Parameters.AddWithValue("@CategoryFilter", categoryFilter);

                    if (!string.IsNullOrWhiteSpace(searchText))
                        cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    productsTable = new DataTable();
                    da.Fill(productsTable);

                    // Bind to DataGridView
                    dgvProducts.DataSource = productsTable;

                    // Format DataGridView
                    FormatDataGridView();

                    // Update header with product count
                    UpdateHeaderWithCount();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Format DataGridView appearance
        private void FormatDataGridView()
        {
            if (dgvProducts.Rows.Count > 0)
            {
                // Format Price column
                if (dgvProducts.Columns.Contains("PRICE (PKR)"))
                {
                    dgvProducts.Columns["PRICE (PKR)"].DefaultCellStyle.Format = "N2";
                    dgvProducts.Columns["PRICE (PKR)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Format Stock column
                if (dgvProducts.Columns.Contains("CURRENT STOCK"))
                {
                    dgvProducts.Columns["CURRENT STOCK"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Format Product ID column
                if (dgvProducts.Columns.Contains("PRODUCT ID"))
                {
                    dgvProducts.Columns["PRODUCT ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Auto resize columns
                dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Set row height
                dgvProducts.RowTemplate.Height = 35;

                // Set selection mode
                dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProducts.MultiSelect = false;

                // Enable double buffering for better performance
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.SetProperty,
                    null, dgvProducts, new object[] { true });

                // Apply cell formatting for status colors
                dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            }
        }

        // Cell formatting for status colors
        private void DgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns[e.ColumnIndex].Name == "STATUS" && e.Value != null)
            {
                string status = e.Value.ToString();

                switch (status)
                {
                    case "Out of Stock":
                        e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        e.CellStyle.Font = new System.Drawing.Font(dgvProducts.Font, System.Drawing.FontStyle.Bold);
                        break;

                    case "Low Stock":
                        e.CellStyle.ForeColor = System.Drawing.Color.Orange;
                        e.CellStyle.Font = new System.Drawing.Font(dgvProducts.Font, System.Drawing.FontStyle.Bold);
                        break;

                    case "In Stock":
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                        break;

                    case "Inactive":
                        e.CellStyle.ForeColor = System.Drawing.Color.Gray;
                        e.CellStyle.Font = new System.Drawing.Font(dgvProducts.Font, System.Drawing.FontStyle.Italic);
                        break;
                }
            }

            // Format price cells
            if (dgvProducts.Columns[e.ColumnIndex].Name == "PRICE (PKR)" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("N2");
                    e.FormattingApplied = true;
                }
            }
        }

        // Update header with product count
        private void UpdateHeaderWithCount()
        {
            int totalProducts = productsTable?.Rows.Count ?? 0;
            lblHeader.Text = $"ALL PRODUCTS ({totalProducts})";
        }

        // Category filter changed
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.SelectedItem != null)
            {
                string selectedCategory = cmbFilter.SelectedItem.ToString();
                LoadProducts(selectedCategory);
            }
        }

        // Search button clicked
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Create a simple search dialog
            using (var searchForm = new Form()
            {
                Text = "Search Product",
                Size = new System.Drawing.Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            })
            {
                var txtSearch = new TextBox()
                {
                    Location = new System.Drawing.Point(20, 30),
                    Size = new System.Drawing.Size(240, 25),
                    Font = new System.Drawing.Font("Segoe UI", 10F)
                };

                var btnOk = new Button()
                {
                    Text = "Search",
                    Location = new System.Drawing.Point(100, 70),
                    Size = new System.Drawing.Size(80, 30),
                    DialogResult = DialogResult.OK
                };

                searchForm.Controls.Add(txtSearch);
                searchForm.Controls.Add(btnOk);
                searchForm.AcceptButton = btnOk;

                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string searchText = txtSearch.Text.Trim();
                    string selectedCategory = cmbFilter.SelectedItem?.ToString() ?? "All Categories";

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        LoadProducts(selectedCategory, searchText);
                    }
                    else
                    {
                        LoadProducts(selectedCategory);
                    }
                }
            }
        }

        // DataGridView cell click - for editing or viewing details
        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Rows[e.RowIndex].Cells["PRODUCT ID"].Value != null)
            {
                // Get selected product ID
                string productID = dgvProducts.Rows[e.RowIndex].Cells["PRODUCT ID"].Value.ToString();

                // You can open an edit form here or show details
                // ShowProductDetails(productID);

                // For now, just show a message
                MessageBox.Show($"Selected Product ID: {productID}", "Product Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Double click on row to edit
        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Rows[e.RowIndex].Cells["PRODUCT ID"].Value != null)
            {
                string productID = dgvProducts.Rows[e.RowIndex].Cells["PRODUCT ID"].Value.ToString();
                OpenEditProductForm(productID);
            }
        }

        // Open edit product form (you need to create this form)
        private void OpenEditProductForm(string productID)
        {
            try
            {
                // یہاں آپ Edit Product form open کر سکتے ہیں
                // Example:
                // frmEditProduct editForm = new frmEditProduct(Convert.ToInt32(productID));
                // editForm.ShowDialog();

                // Reload products after editing
                // LoadProducts(cmbFilter.SelectedItem?.ToString() ?? "All Categories");

                MessageBox.Show($"Edit Product ID: {productID}\n(This feature needs implementation)", "Edit Product",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Export to Excel/CSV (optional feature)
        private void ExportToExcel()
        {
            if (dgvProducts.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Products";
                saveFileDialog.FileName = $"Products_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        // Write headers
                        for (int i = 0; i < dgvProducts.Columns.Count; i++)
                        {
                            writer.Write(dgvProducts.Columns[i].HeaderText);
                            if (i < dgvProducts.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                        // Write data
                        foreach (DataGridViewRow row in dgvProducts.Rows)
                        {
                            for (int i = 0; i < dgvProducts.Columns.Count; i++)
                            {
                                object value = row.Cells[i].Value;
                                string text = value?.ToString() ?? "";

                                // Handle commas and quotes in CSV
                                if (text.Contains(",") || text.Contains("\"") || text.Contains("\n"))
                                {
                                    text = "\"" + text.Replace("\"", "\"\"") + "\"";
                                }

                                writer.Write(text);
                                if (i < dgvProducts.Columns.Count - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show("Products exported successfully!", "Export Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refresh button (you can add this to your form)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategoriesForFilter();
            LoadProducts(cmbFilter.SelectedItem?.ToString() ?? "All Categories");
        }

        // Form Load event
        private void frmViewProducts_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }

        // Event handlers for paint events
        private void mainPanel_Paint(object sender, PaintEventArgs e) { }
        private void headerPanel_Paint(object sender, PaintEventArgs e) { }
        private void contentPanel_Paint(object sender, PaintEventArgs e) { }
        private void lblHeader_Click(object sender, EventArgs e) { }

        // Setup event handlers if not already in designer
        private void SetupDataGridView()
        {
            dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;
        }
    }
}