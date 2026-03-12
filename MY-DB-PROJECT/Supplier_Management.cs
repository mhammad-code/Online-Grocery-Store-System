using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class Supplier_Management : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public Supplier_Management()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadEventHandlers();
        }

        private void SetupDataGridView()
        {
            // Clear existing columns
            dgvSuppliers.Columns.Clear();

            // Add columns based on your Suppliers table schema
            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SupplierID",
                HeaderText = "ID",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SupplierName",
                HeaderText = "Supplier Name",
                Width = 200,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                HeaderText = "Phone",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                Width = 180,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                HeaderText = "Address",
                Width = 200,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });

            // Row styling
            dgvSuppliers.RowTemplate.Height = 35;
            dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.AllowUserToAddRows = false;
            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.MultiSelect = false;
        }

        private void LoadEventHandlers()
        {
            this.Load += Supplier_Management_Load;
            btnViewSuppliers.Click += btnViewSuppliers_Click;
            guna2Button1.Click += guna2Button1_Click;
            btnSupplierProducts.Click += btnSupplierProducts_Click;
            dgvSuppliers.CellDoubleClick += dgvSuppliers_CellDoubleClick;
            dgvSuppliers.CellMouseClick += dgvSuppliers_CellMouseClick;
        }

        private void Supplier_Management_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            LoadSupplierStats();
            LoadPendingOrders();
            ApplyDataGridViewStyling();
        }

        private void ApplyDataGridViewStyling()
        {
            // Header styling
            dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSuppliers.ColumnHeadersHeight = 40;

            // Row styling
            dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvSuppliers.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvSuppliers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvSuppliers.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Grid styling
            dgvSuppliers.GridColor = Color.FromArgb(224, 224, 224);
            dgvSuppliers.BorderStyle = BorderStyle.None;
            dgvSuppliers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        private void LoadSuppliers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // CORRECTED: Query matches your Suppliers table schema
                    string query = @"
                        SELECT 
                            SupplierID, 
                            SupplierName, 
                            Phone, 
                            Email, 
                            Address 
                        FROM Suppliers 
                        ORDER BY SupplierName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Clear existing rows
                    dgvSuppliers.Rows.Clear();

                    // Populate DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvSuppliers.Rows.Add();
                        DataGridViewRow dataRow = dgvSuppliers.Rows[rowIndex];

                        dataRow.Cells["SupplierID"].Value = row["SupplierID"];
                        dataRow.Cells["SupplierName"].Value = row["SupplierName"];
                        dataRow.Cells["Phone"].Value = row["Phone"] != DBNull.Value ? row["Phone"] : "N/A";
                        dataRow.Cells["Email"].Value = row["Email"] != DBNull.Value ? row["Email"] : "N/A";
                        dataRow.Cells["Address"].Value = row["Address"] != DBNull.Value ? row["Address"] : "N/A";

                        // Color code suppliers with email
                        if (row["Email"] != DBNull.Value && !string.IsNullOrEmpty(row["Email"].ToString()))
                        {
                            dataRow.Cells["Email"].Style.ForeColor = Color.FromArgb(33, 150, 243);
                            dataRow.Cells["Email"].Style.Font = new Font(dgvSuppliers.Font, FontStyle.Bold);
                        }
                    }

                    // Update label
                    label7.Text = $"🏢 Supplier Companies ({dgvSuppliers.Rows.Count})";

                    if (dgvSuppliers.Rows.Count == 0)
                    {
                        MessageBox.Show("No suppliers found in the database.",
                                      "No Suppliers",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
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

        private void LoadSupplierStats()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Total suppliers
                    SqlCommand cmdTotal = new SqlCommand("SELECT COUNT(*) FROM Suppliers", conn);
                    int totalSuppliers = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    lblTotalSuppliers.Text = totalSuppliers.ToString();

                    // Active suppliers (those with at least 1 product in SupplierProducts)
                    SqlCommand cmdActive = new SqlCommand(@"
                        SELECT COUNT(DISTINCT s.SupplierID) 
                        FROM Suppliers s
                        INNER JOIN SupplierProducts sp ON s.SupplierID = sp.SupplierID", conn);
                    int activeSuppliers = Convert.ToInt32(cmdActive.ExecuteScalar());
                    lblActiveSuppliers.Text = activeSuppliers.ToString();

                    // Inactive suppliers (those with no products in SupplierProducts)
                    SqlCommand cmdInactive = new SqlCommand(@"
                        SELECT COUNT(*) FROM Suppliers 
                        WHERE SupplierID NOT IN 
                        (SELECT DISTINCT SupplierID FROM SupplierProducts)", conn);
                    int inactiveSuppliers = Convert.ToInt32(cmdInactive.ExecuteScalar());
                    lblInactiveSuppliers.Text = inactiveSuppliers.ToString();

                    // Update form title
                    this.Text = $"Supplier Management - {totalSuppliers} Suppliers";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supplier statistics: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void LoadPendingOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // CORRECTED: Using OrderStatus column from your Orders table
                    SqlCommand cmdPending = new SqlCommand(
                        "SELECT COUNT(*) FROM Orders WHERE OrderStatus='Pending'",
                        conn);

                    int pendingOrders = Convert.ToInt32(cmdPending.ExecuteScalar());
                    lblPendingOrders.Text = pendingOrders.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending orders: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            using (AddSupplierDialog addDialog = new AddSupplierDialog())
            {
                if (addDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string query = @"
                                INSERT INTO Suppliers (SupplierName, Phone, Email, Address)
                                VALUES (@SupplierName, @Phone, @Email, @Address)";

                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@SupplierName", addDialog.SupplierName);
                            cmd.Parameters.AddWithValue("@Phone", addDialog.Phone);
                            cmd.Parameters.AddWithValue("@Email", addDialog.Email);
                            cmd.Parameters.AddWithValue("@Address", addDialog.Address);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Supplier '{addDialog.SupplierName}' added successfully!",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                LoadSuppliers();
                                LoadSupplierStats();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding supplier: {ex.Message}",
                                      "Database Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to edit.",
                              "No Selection",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvSuppliers.SelectedRows[0];
            int supplierID = Convert.ToInt32(selectedRow.Cells["SupplierID"].Value);
            string supplierName = selectedRow.Cells["SupplierName"].Value.ToString();
            string phone = selectedRow.Cells["Phone"].Value.ToString();
            string email = selectedRow.Cells["Email"].Value.ToString();
            string address = selectedRow.Cells["Address"].Value.ToString();

            using (EditSupplierDialog editDialog = new EditSupplierDialog(supplierName, phone, email, address))
            {
                if (editDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string query = @"
                                UPDATE Suppliers 
                                SET SupplierName = @SupplierName,
                                    Phone = @Phone,
                                    Email = @Email,
                                    Address = @Address
                                WHERE SupplierID = @SupplierID";

                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@SupplierName", editDialog.SupplierName);
                            cmd.Parameters.AddWithValue("@Phone", editDialog.Phone);
                            cmd.Parameters.AddWithValue("@Email", editDialog.Email);
                            cmd.Parameters.AddWithValue("@Address", editDialog.Address);
                            cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Supplier '{editDialog.SupplierName}' updated successfully!",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                LoadSuppliers();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating supplier: {ex.Message}",
                                      "Database Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSupplierProducts_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to view products.",
                              "No Selection",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvSuppliers.SelectedRows[0];
            int supplierID = Convert.ToInt32(selectedRow.Cells["SupplierID"].Value);
            string supplierName = selectedRow.Cells["SupplierName"].Value.ToString();

            // Show supplier products in a message box or open a new form
            ShowSupplierProducts(supplierID, supplierName);
        }

        private void ShowSupplierProducts(int supplierID, string supplierName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            P.ProductName,
                            P.Price,
                            P.Stock,
                            C.CategoryName
                        FROM SupplierProducts SP
                        INNER JOIN Products P ON SP.ProductID = P.ProductID
                        INNER JOIN Categories C ON P.CategoryID = C.CategoryID
                        WHERE SP.SupplierID = @SupplierID
                        ORDER BY P.ProductName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        string products = $"📦 Products Supplied by {supplierName}\n";
                        products += "═══════════════════════════════\n\n";

                        int productCount = 0;
                        while (reader.Read())
                        {
                            productCount++;
                            string productName = reader["ProductName"].ToString();
                            decimal price = Convert.ToDecimal(reader["Price"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            string category = reader["CategoryName"].ToString();

                            products += $"🛒 {productName}\n";
                            products += $"   📍 Category: {category}\n";
                            products += $"   💰 Price: PKR {price:N2}\n";
                            products += $"   📊 Stock: {stock} units\n\n";
                        }

                        if (productCount == 0)
                        {
                            products += "No products found for this supplier.\n";
                        }
                        else
                        {
                            products += $"═══════════════════════════════\n";
                            products += $"Total Products: {productCount}\n";
                        }

                        MessageBox.Show(products,
                                      $"Supplier Products - {supplierName}",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
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

        private void btnViewSuppliers_Click(object sender, EventArgs e)
        {
            LoadSuppliers();
            MessageBox.Show("Supplier list refreshed successfully!",
                          "Refresh Complete",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvSuppliers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSuppliers.Rows.Count)
            {
                DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];

                string supplierID = row.Cells["SupplierID"].Value?.ToString() ?? "N/A";
                string supplierName = row.Cells["SupplierName"].Value?.ToString() ?? "N/A";
                string phone = row.Cells["Phone"].Value?.ToString() ?? "N/A";
                string email = row.Cells["Email"].Value?.ToString() ?? "N/A";
                string address = row.Cells["Address"].Value?.ToString() ?? "N/A";

                string details = "🏢 SUPPLIER DETAILS\n";
                details += "═══════════════════════\n\n";
                details += $"🆔 Supplier ID: {supplierID}\n";
                details += $"🏢 Company: {supplierName}\n";
                details += $"📞 Phone: {phone}\n";
                details += $"📧 Email: {email}\n";
                details += $"📍 Address: {address}\n\n";
                details += "═══════════════════════\n";

                MessageBox.Show(details,
                              "Supplier Information",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
        }

        private void dgvSuppliers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvSuppliers.ClearSelection();
                dgvSuppliers.Rows[e.RowIndex].Selected = true;

                ContextMenuStrip contextMenu = new ContextMenuStrip();

                ToolStripMenuItem viewDetailsItem = new ToolStripMenuItem("View Details");
                ToolStripMenuItem editItem = new ToolStripMenuItem("Edit Supplier");
                ToolStripMenuItem viewProductsItem = new ToolStripMenuItem("View Products");
                ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete Supplier");
                ToolStripSeparator separator = new ToolStripSeparator();
                ToolStripMenuItem refreshItem = new ToolStripMenuItem("Refresh");

                viewDetailsItem.Click += (s, ev) => dgvSuppliers_CellDoubleClick(sender,
                    new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));

                editItem.Click += (s, ev) => btnEditSupplier_Click(sender, e);

                viewProductsItem.Click += (s, ev) => btnSupplierProducts_Click(sender, e);

                deleteItem.Click += (s, ev) =>
                {
                    if (dgvSuppliers.SelectedRows.Count > 0)
                    {
                        DataGridViewRow selectedRow = dgvSuppliers.SelectedRows[0];
                        string supplierName = selectedRow.Cells["SupplierName"].Value.ToString();

                        DialogResult result = MessageBox.Show($"Are you sure you want to delete {supplierName}?\nThis will also remove all associated products.",
                                                            "Confirm Delete",
                                                            MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            DeleteSupplier(Convert.ToInt32(selectedRow.Cells["SupplierID"].Value));
                        }
                    }
                };

                refreshItem.Click += (s, ev) => LoadSuppliers();

                contextMenu.Items.Add(viewDetailsItem);
                contextMenu.Items.Add(editItem);
                contextMenu.Items.Add(viewProductsItem);
                contextMenu.Items.Add(deleteItem);
                contextMenu.Items.Add(separator);
                contextMenu.Items.Add(refreshItem);

                contextMenu.Show(dgvSuppliers, e.Location);
            }
        }

        private void DeleteSupplier(int supplierID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // First delete from SupplierProducts
                    string deleteProductsQuery = "DELETE FROM SupplierProducts WHERE SupplierID = @SupplierID";
                    SqlCommand deleteProductsCmd = new SqlCommand(deleteProductsQuery, conn);
                    deleteProductsCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    deleteProductsCmd.ExecuteNonQuery();

                    // Then delete from Suppliers
                    string deleteSupplierQuery = "DELETE FROM Suppliers WHERE SupplierID = @SupplierID";
                    SqlCommand deleteSupplierCmd = new SqlCommand(deleteSupplierQuery, conn);
                    deleteSupplierCmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    int rowsAffected = deleteSupplierCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Supplier deleted successfully!",
                                      "Deleted",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        LoadSuppliers();
                        LoadSupplierStats();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting supplier: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        // Keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                LoadSuppliers();
                return true;
            }
            else if (keyData == Keys.Delete && dgvSuppliers.Focused)
            {
                if (dgvSuppliers.SelectedRows.Count > 0)
                {
                    btnEditSupplier_Click(null, null);
                    return true;
                }
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    // Dialog classes for adding/editing suppliers
    public class AddSupplierDialog : Form
    {
        private TextBox txtSupplierName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private Button btnOK;
        private Button btnCancel;

        public string SupplierName => txtSupplierName.Text;
        public string Phone => txtPhone.Text;
        public string Email => txtEmail.Text;
        public string Address => txtAddress.Text;

        public AddSupplierDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Add New Supplier";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblName = new Label { Text = "Supplier Name:", Location = new Point(20, 20), Width = 120 };
            txtSupplierName = new TextBox { Location = new Point(150, 20), Width = 220 };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 60), Width = 120 };
            txtPhone = new TextBox { Location = new Point(150, 60), Width = 220 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), Width = 120 };
            txtEmail = new TextBox { Location = new Point(150, 100), Width = 220 };

            Label lblAddress = new Label { Text = "Address:", Location = new Point(20, 140), Width = 120 };
            txtAddress = new TextBox { Location = new Point(150, 140), Width = 220, Multiline = true, Height = 60 };

            btnOK = new Button { Text = "OK", Location = new Point(150, 220), Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", Location = new Point(240, 220), Width = 80, DialogResult = DialogResult.Cancel };

            this.Controls.AddRange(new Control[] { lblName, txtSupplierName, lblPhone, txtPhone, lblEmail, txtEmail,
                                                   lblAddress, txtAddress, btnOK, btnCancel });

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }

    public class EditSupplierDialog : Form
    {
        private TextBox txtSupplierName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private Button btnOK;
        private Button btnCancel;

        public string SupplierName => txtSupplierName.Text;
        public string Phone => txtPhone.Text;
        public string Email => txtEmail.Text;
        public string Address => txtAddress.Text;

        public EditSupplierDialog(string name, string phone, string email, string address)
        {
            InitializeComponent();
            txtSupplierName.Text = name;
            txtPhone.Text = phone;
            txtEmail.Text = email;
            txtAddress.Text = address;
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Supplier";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblName = new Label { Text = "Supplier Name:", Location = new Point(20, 20), Width = 120 };
            txtSupplierName = new TextBox { Location = new Point(150, 20), Width = 220 };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 60), Width = 120 };
            txtPhone = new TextBox { Location = new Point(150, 60), Width = 220 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), Width = 120 };
            txtEmail = new TextBox { Location = new Point(150, 100), Width = 220 };

            Label lblAddress = new Label { Text = "Address:", Location = new Point(20, 140), Width = 120 };
            txtAddress = new TextBox { Location = new Point(150, 140), Width = 220, Multiline = true, Height = 60 };

            btnOK = new Button { Text = "OK", Location = new Point(150, 220), Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", Location = new Point(240, 220), Width = 80, DialogResult = DialogResult.Cancel };

            this.Controls.AddRange(new Control[] { lblName, txtSupplierName, lblPhone, txtPhone, lblEmail, txtEmail,
                                                   lblAddress, txtAddress, btnOK, btnCancel });

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }
}