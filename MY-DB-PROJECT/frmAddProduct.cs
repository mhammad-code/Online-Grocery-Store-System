using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class frmAddProduct : Form
    {
        // آپ کا connection string
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";

        public frmAddProduct()
        {
            InitializeComponent();
            LoadCategoriesFromDatabase();
            SetupEventHandlers();
        }

        // Event handlers setup
        private void SetupEventHandlers()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Numeric validation for price and stock
            txtPrice.KeyPress += TxtPrice_KeyPress;
            txtStock.KeyPress += TxtStock_KeyPress;
        }

        // Load categories from database
        private void LoadCategoriesFromDatabase()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // آپ کے DB میں Categories ٹیبل: CategoryID, CategoryName
                    string query = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("-- Select Category --");

                    while (reader.Read())
                    {
                        string categoryName = reader["CategoryName"].ToString();
                        cmbCategory.Items.Add(categoryName);
                    }

                    reader.Close();
                    cmbCategory.SelectedIndex = 0;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Save Product Button Click
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("Please enter product name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductName.Focus();
                    return;
                }

                if (cmbCategory.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a category.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCategory.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Please enter a valid price (greater than 0).", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStock.Text) || !int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("Please enter a valid stock quantity (0 or greater).", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                // Get category ID from selected category name
                int categoryID = GetCategoryID(cmbCategory.SelectedItem.ToString());
                if (categoryID == -1)
                {
                    MessageBox.Show("Selected category not found. Please refresh categories.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string productName = txtProductName.Text.Trim();
                string description = txtDescription.Text.Trim();

                // Save to database
                if (SaveProductToDatabase(productName, categoryID, price, stock, description))
                {
                    MessageBox.Show("Product saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Log activity
                    LogProductActivity(productName, "Added new product");

                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to save product. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Get Category ID from Category Name
        private int GetCategoryID(string categoryName)
        {
            if (cmbCategory.SelectedIndex <= 0) return -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        // Save product to database
        private bool SaveProductToDatabase(string productName, int categoryID, decimal price, int stock, string description)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // آپ کے DB میں Products ٹیبل: ProductID, CategoryID, ProductName, Price, Stock, IsActive
                    // Description کالم نہیں ہے، اس لیے میں نے Comment کی جگہ description استعمال کیا ہے
                    string query = @"
                        INSERT INTO Products (CategoryID, ProductName, Price, Stock, IsActive)
                        VALUES (@CategoryID, @ProductName, @Price, @Stock, 1)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);

                    int rows = cmd.ExecuteNonQuery();

                    // اگر description موجود ہو تو اسے Feedback یا کسی اور ٹیبل میں save کریں
                    if (rows > 0 && !string.IsNullOrWhiteSpace(description))
                    {
                        SaveProductDescription(GetLastInsertedProductID(), description);
                    }

                    return rows > 0;
                }
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Get last inserted product ID
        private int GetLastInsertedProductID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MAX(ProductID) FROM Products";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    object result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToInt32(result) : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        // Save product description (optional - آپ کے DB میں description کالم نہیں ہے)
        private void SaveProductDescription(int productID, string description)
        {
            try
            {
                // اگر آپ product description کو store کرنا چاہتے ہیں تو:
                // 1. Products ٹیبل میں Description کالم شامل کریں
                // 2. یا الگ ProductDetails ٹیبل بنائیں

                // Temporary: میں اسے Feedback ٹیبل میں save کر رہا ہوں (admin کے لیے comment کے طور پر)
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create table if not exists
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductDescriptions')
                        BEGIN
                            CREATE TABLE ProductDescriptions (
                                DescriptionID INT IDENTITY(1,1) PRIMARY KEY,
                                ProductID INT NOT NULL,
                                Description NVARCHAR(MAX),
                                CreatedDate DATETIME DEFAULT GETDATE(),
                                FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
                            )
                        END";

                    SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                    createCmd.ExecuteNonQuery();

                    // Insert description
                    string insertQuery = @"
                        INSERT INTO ProductDescriptions (ProductID, Description)
                        VALUES (@ProductID, @Description)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@ProductID", productID);
                    insertCmd.Parameters.AddWithValue("@Description", description);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail - description saving is optional
            }
        }

        // Handle SQL exceptions
        private void HandleSqlException(SqlException sqlEx)
        {
            if (sqlEx.Number == 2627) // Unique constraint violation
            {
                MessageBox.Show("A product with this name already exists.", "Duplicate Product",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (sqlEx.Number == 547) // Foreign key constraint
            {
                MessageBox.Show("Invalid category selected.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clear form after saving
        private void ClearForm()
        {
            txtProductName.Clear();
            cmbCategory.SelectedIndex = 0;
            txtPrice.Clear();
            txtStock.Clear();
            txtDescription.Clear();
            txtProductName.Focus();
        }

        // Cancel button click
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel? All unsaved data will be lost.",
                "Confirm Cancel",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Numeric validation for price
        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, decimal point, and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        // Numeric validation for stock
        private void TxtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Form Load Event
        private void frmAddProduct_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }

        // Log product activity (optional)
        private void LogProductActivity(string productName, string action)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create audit log table if not exists
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductAuditLog')
                        BEGIN
                            CREATE TABLE ProductAuditLog (
                                LogID INT IDENTITY(1,1) PRIMARY KEY,
                                ProductName NVARCHAR(150),
                                Action NVARCHAR(200),
                                ActionBy NVARCHAR(100),
                                ActionDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                    createCmd.ExecuteNonQuery();

                    // Insert log
                    string insertQuery = @"
                        INSERT INTO ProductAuditLog (ProductName, Action, ActionBy)
                        VALUES (@ProductName, @Action, @ActionBy)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@ProductName", productName);
                    insertCmd.Parameters.AddWithValue("@Action", action);
                    insertCmd.Parameters.AddWithValue("@ActionBy", Environment.UserName);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail if audit logging fails
            }
        }

        // Form Closing Event
        private void frmAddProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup if needed
        }
    }
}