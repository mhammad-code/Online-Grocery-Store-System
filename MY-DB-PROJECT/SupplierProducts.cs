using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MY_DB_PROJECT
{
    public partial class SupplierProducts : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private DataTable allProducts;

        public SupplierProducts()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadEventHandlers();
        }

        private void SetupDataGridView()
        {
            // Clear existing columns
            dgvSupplierProducts.Columns.Clear();

            // Add columns based on your database schema
            dgvSupplierProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductID",
                HeaderText = "ID",
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            });

            dgvSupplierProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Product Name",
                Width = 250,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 10)
                }
            });

            dgvSupplierProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CategoryName",
                HeaderText = "Category",
                Width = 150,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });

            dgvSupplierProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Price",
                HeaderText = "Price (PKR)",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N2",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 167, 69)
                }
            });

            dgvSupplierProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Stock",
                HeaderText = "Stock",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10)
                }
            });

            dgvSupplierProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            });

            // Row styling
            dgvSupplierProducts.RowTemplate.Height = 35;
            dgvSupplierProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSupplierProducts.AllowUserToAddRows = false;
            dgvSupplierProducts.ReadOnly = true;
            dgvSupplierProducts.MultiSelect = false;
        }

        private void LoadEventHandlers()
        {
            this.Load += SupplierProducts_Load;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            txtSearchProduct.TextChanged += txtSearchProduct_TextChanged;
            btnBack.Click += btnBack_Click;
            btnViewAll.Click += btnViewAll_Click;
            btnExportProducts.Click += btnExportProducts_Click;
            dgvSupplierProducts.CellDoubleClick += dgvSupplierProducts_CellDoubleClick;
        }

        private void SupplierProducts_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            ApplyDataGridViewStyling();
            ResetSummary();
        }

        private void ApplyDataGridViewStyling()
        {
            // Header styling
            dgvSupplierProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 152, 0);
            dgvSupplierProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSupplierProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSupplierProducts.ColumnHeadersHeight = 40;

            // Row styling
            dgvSupplierProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvSupplierProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvSupplierProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvSupplierProducts.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Grid styling
            dgvSupplierProducts.GridColor = Color.FromArgb(224, 224, 224);
            dgvSupplierProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        private void ResetSummary()
        {
            lblInStock.Text = "0";
            lblOutOfStock.Text = "0";
            lblTotalProducts.Text = "0";
            lblTotalValue.Text = "PKR 0.00";
            lblSupplierInfo.Text = "Select a supplier to view products";
        }

        private void LoadSuppliers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // CORRECTED: Query matches your Suppliers table schema
                    string query = @"
                        SELECT SupplierID, SupplierName, Phone, Email, Address 
                        FROM Suppliers 
                        ORDER BY SupplierName";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add a default "All Suppliers" option
                    DataRow allRow = dt.NewRow();
                    allRow["SupplierID"] = 0;
                    allRow["SupplierName"] = "All Suppliers";
                    allRow["Phone"] = "";
                    allRow["Email"] = "";
                    allRow["Address"] = "";
                    dt.Rows.InsertAt(allRow, 0);

                    cmbSupplier.DataSource = dt;
                    cmbSupplier.DisplayMember = "SupplierName";
                    cmbSupplier.ValueMember = "SupplierID";
                    cmbSupplier.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void LoadAllProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // CORRECTED: Get all products with category information
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
                        ORDER BY P.ProductName";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    allProducts = new DataTable();
                    da.Fill(allProducts);

                    DisplayProducts(allProducts);
                    UpdateSummary(allProducts);

                    lblSupplierInfo.Text = "Showing all products from all suppliers";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading all products: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void LoadSupplierProducts(int supplierID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // CORRECTED: Get products for specific supplier through SupplierProducts table
                    string query = @"
                        SELECT 
                            P.ProductID,
                            P.ProductName,
                            P.Price,
                            P.Stock,
                            P.IsActive,
                            C.CategoryName,
                            S.SupplierName,
                            S.Phone,
                            S.Email,
                            S.Address
                        FROM SupplierProducts SP
                        INNER JOIN Products P ON SP.ProductID = P.ProductID
                        INNER JOIN Categories C ON P.CategoryID = C.CategoryID
                        INNER JOIN Suppliers S ON SP.SupplierID = S.SupplierID
                        WHERE SP.SupplierID = @SupplierID AND P.IsActive = 1
                        ORDER BY P.ProductName";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    allProducts = dt;

                    DisplayProducts(dt);
                    UpdateSummary(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string supplierName = dt.Rows[0]["SupplierName"].ToString();
                        string phone = dt.Rows[0]["Phone"] != DBNull.Value ? dt.Rows[0]["Phone"].ToString() : "N/A";
                        string email = dt.Rows[0]["Email"] != DBNull.Value ? dt.Rows[0]["Email"].ToString() : "N/A";

                        lblSupplierInfo.Text = $"{supplierName} | 📞 {phone} | 📧 {email}";
                    }
                    else
                    {
                        lblSupplierInfo.Text = "No products found for this supplier";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supplier products: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void DisplayProducts(DataTable dt)
        {
            // Clear existing rows
            dgvSupplierProducts.Rows.Clear();

            foreach (DataRow row in dt.Rows)
            {
                int rowIndex = dgvSupplierProducts.Rows.Add();
                DataGridViewRow dataRow = dgvSupplierProducts.Rows[rowIndex];

                dataRow.Cells["ProductID"].Value = row["ProductID"];
                dataRow.Cells["ProductName"].Value = row["ProductName"];
                dataRow.Cells["CategoryName"].Value = row["CategoryName"];
                dataRow.Cells["Price"].Value = Convert.ToDecimal(row["Price"]);

                int stock = Convert.ToInt32(row["Stock"]);
                dataRow.Cells["Stock"].Value = stock;

                // Set status and color coding
                if (stock > 10)
                {
                    dataRow.Cells["Status"].Value = "✅ In Stock";
                    dataRow.Cells["Stock"].Style.ForeColor = Color.FromArgb(40, 167, 69);
                    dataRow.Cells["Stock"].Style.Font = new Font(dgvSupplierProducts.Font, FontStyle.Bold);
                    dataRow.Cells["Status"].Style.ForeColor = Color.FromArgb(40, 167, 69);
                }
                else if (stock > 0)
                {
                    dataRow.Cells["Status"].Value = "⚠️ Low Stock";
                    dataRow.Cells["Stock"].Style.ForeColor = Color.FromArgb(255, 193, 7);
                    dataRow.Cells["Stock"].Style.Font = new Font(dgvSupplierProducts.Font, FontStyle.Bold);
                    dataRow.Cells["Status"].Style.ForeColor = Color.FromArgb(255, 193, 7);
                }
                else
                {
                    dataRow.Cells["Status"].Value = "❌ Out of Stock";
                    dataRow.Cells["Stock"].Style.ForeColor = Color.FromArgb(220, 53, 69);
                    dataRow.Cells["Stock"].Style.Font = new Font(dgvSupplierProducts.Font, FontStyle.Bold);
                    dataRow.Cells["Status"].Style.ForeColor = Color.FromArgb(220, 53, 69);
                }

                // Color code row based on stock
                if (stock == 0)
                {
                    dataRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 245);
                }
                else if (stock <= 5)
                {
                    dataRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 252, 245);
                }
            }

            // Update label
            label7.Text = $"📦 Products List ({dgvSupplierProducts.Rows.Count})";
        }

        private void UpdateSummary(DataTable dt)
        {
            int inStock = 0;
            int outOfStock = 0;
            decimal totalValue = 0;

            foreach (DataRow row in dt.Rows)
            {
                int stock = Convert.ToInt32(row["Stock"]);
                decimal price = Convert.ToDecimal(row["Price"]);

                if (stock > 0)
                    inStock++;
                else
                    outOfStock++;

                totalValue += stock * price;
            }

            lblInStock.Text = inStock.ToString();
            lblOutOfStock.Text = outOfStock.ToString();
            lblTotalProducts.Text = dt.Rows.Count.ToString();
            lblTotalValue.Text = $"PKR {totalValue:N2}";
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedIndex >= 0)
            {
                int supplierID = Convert.ToInt32(cmbSupplier.SelectedValue);

                if (supplierID == 0)
                {
                    LoadAllProducts();
                }
                else
                {
                    LoadSupplierProducts(supplierID);
                }
            }
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            if (allProducts == null) return;

            string searchText = txtSearchProduct.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                DisplayProducts(allProducts);
                return;
            }

            DataTable filteredTable = allProducts.Clone();

            foreach (DataRow row in allProducts.Rows)
            {
                string productName = row["ProductName"].ToString().ToLower();
                string categoryName = row["CategoryName"].ToString().ToLower();

                if (productName.Contains(searchText) || categoryName.Contains(searchText))
                {
                    filteredTable.ImportRow(row);
                }
            }

            DisplayProducts(filteredTable);
            UpdateSummary(filteredTable);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            cmbSupplier.SelectedIndex = 0;
            LoadAllProducts();
            MessageBox.Show("Showing all products from all suppliers",
                          "View All",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private void btnExportProducts_Click(object sender, EventArgs e)
        {
            if (dgvSupplierProducts.Rows.Count == 0)
            {
                MessageBox.Show("No products to export.",
                              "Empty List",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = $"SupplierProducts_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (sfd.FilterIndex == 1) // CSV
                        {
                            ExportToCSV(sfd.FileName);
                        }
                        else if (sfd.FilterIndex == 2) // Excel
                        {
                            ExportToExcel(sfd.FileName);
                        }

                        MessageBox.Show($"Products exported successfully to:\n{sfd.FileName}",
                                      "Export Successful",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting products: {ex.Message}",
                                      "Export Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportToCSV(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            // Add headers
            sb.AppendLine("Product ID,Product Name,Category,Price,Stock,Status");

            // Add data
            foreach (DataGridViewRow row in dgvSupplierProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    sb.AppendLine($"{row.Cells["ProductID"].Value}," +
                                 $"\"{row.Cells["ProductName"].Value}\"," +
                                 $"\"{row.Cells["CategoryName"].Value}\"," +
                                 $"{row.Cells["Price"].Value}," +
                                 $"{row.Cells["Stock"].Value}," +
                                 $"\"{row.Cells["Status"].Value}\"");
                }
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        private void ExportToExcel(string filePath)
        {
            // Simple Excel export using CSV with .xlsx extension
            StringBuilder sb = new StringBuilder();

            // Add headers
            sb.AppendLine("Product ID\tProduct Name\tCategory\tPrice\tStock\tStatus");

            // Add data
            foreach (DataGridViewRow row in dgvSupplierProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    sb.AppendLine($"{row.Cells["ProductID"].Value}\t" +
                                 $"{row.Cells["ProductName"].Value}\t" +
                                 $"{row.Cells["CategoryName"].Value}\t" +
                                 $"{row.Cells["Price"].Value}\t" +
                                 $"{row.Cells["Stock"].Value}\t" +
                                 $"{row.Cells["Status"].Value}");
                }
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        private void btnPrintProducts_Click(object sender, EventArgs e)
        {
            if (dgvSupplierProducts.Rows.Count == 0)
            {
                MessageBox.Show("No products to print.",
                              "Empty List",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            // Create a simple print preview
            string printContent = CreatePrintableContent();

            MessageBox.Show(printContent,
                          "📄 Product List - Printable View",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private string CreatePrintableContent()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("╔══════════════════════════════════════════════════════════╗");
            sb.AppendLine("║                 SUPPLIER PRODUCTS REPORT                ║");
            sb.AppendLine("╚══════════════════════════════════════════════════════════╝");
            sb.AppendLine($"Date: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Supplier: {lblSupplierInfo.Text}");
            sb.AppendLine("══════════════════════════════════════════════════════════");
            sb.AppendLine();

            // Add summary
            sb.AppendLine("📊 SUMMARY:");
            sb.AppendLine($"   • Total Products: {lblTotalProducts.Text}");
            sb.AppendLine($"   • In Stock: {lblInStock.Text}");
            sb.AppendLine($"   • Out of Stock: {lblOutOfStock.Text}");
            sb.AppendLine($"   • Total Stock Value: {lblTotalValue.Text}");
            sb.AppendLine();
            sb.AppendLine("══════════════════════════════════════════════════════════");
            sb.AppendLine();

            // Add column headers
            sb.AppendLine("ID  Product Name                 Category       Price      Stock  Status");
            sb.AppendLine("──  ───────────────────────────  ────────────  ────────  ──────  ───────");

            // Add data rows
            foreach (DataGridViewRow row in dgvSupplierProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    string productID = row.Cells["ProductID"].Value.ToString().PadRight(4);
                    string productName = row.Cells["ProductName"].Value.ToString().PadRight(30);
                    string category = row.Cells["CategoryName"].Value.ToString().PadRight(13);
                    string price = row.Cells["Price"].Value.ToString().PadRight(10);
                    string stock = row.Cells["Stock"].Value.ToString().PadRight(7);
                    string status = row.Cells["Status"].Value.ToString();

                    sb.AppendLine($"{productID}  {productName}  {category}  {price}  {stock}  {status}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("══════════════════════════════════════════════════════════");
            sb.AppendLine("Generated by Grocery Store Management System");

            return sb.ToString();
        }

        private void dgvSupplierProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSupplierProducts.Rows.Count)
            {
                DataGridViewRow row = dgvSupplierProducts.Rows[e.RowIndex];

                string productID = row.Cells["ProductID"].Value?.ToString() ?? "N/A";
                string productName = row.Cells["ProductName"].Value?.ToString() ?? "N/A";
                string category = row.Cells["CategoryName"].Value?.ToString() ?? "N/A";
                string price = row.Cells["Price"].Value?.ToString() ?? "0.00";
                string stock = row.Cells["Stock"].Value?.ToString() ?? "0";
                string status = row.Cells["Status"].Value?.ToString() ?? "N/A";

                string details = "📦 PRODUCT DETAILS\n";
                details += "═══════════════════════\n\n";
                details += $"🆔 Product ID: {productID}\n";
                details += $"📝 Product Name: {productName}\n";
                details += $"🏷️ Category: {category}\n";
                details += $"💰 Price: PKR {price}\n";
                details += $"📊 Stock: {stock} units\n";
                details += $"📈 Status: {status}\n\n";
                details += $"🏢 Supplier: {lblSupplierInfo.Text}\n\n";
                details += "═══════════════════════\n";

                MessageBox.Show(details,
                              "Product Information",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
        }

        // Keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            else if (keyData == Keys.F5)
            {
                if (cmbSupplier.SelectedIndex >= 0)
                {
                    int supplierID = Convert.ToInt32(cmbSupplier.SelectedValue);
                    if (supplierID == 0)
                        LoadAllProducts();
                    else
                        LoadSupplierProducts(supplierID);
                }
                return true;
            }
            else if (keyData == (Keys.Control | Keys.F))
            {
                txtSearchProduct.Focus();
                txtSearchProduct.SelectAll();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}