using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class EditSupplier : Form
    {
        // Your connection string
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=grocerystore;Integrated Security=True;TrustServerCertificate=True";
        private int currentSupplierID = -1;

        public EditSupplier()
        {
            InitializeComponent();
        }

        // ===============================
        // Form Load Event
        // ===============================
        private void EditSupplier_Load(object sender, EventArgs e)
        {
            ClearFields();
            EnableDisableFields(false);
        }

        // ===============================
        // Search Supplier by ID or Name
        // ===============================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchID.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter Supplier ID or Name.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearchID.Focus();
                return;
            }

            SearchSupplier(searchText);
        }

        // Search supplier method
        private void SearchSupplier(string searchText)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // آپ کے DB میں Suppliers ٹیبل کے کالم: SupplierID, SupplierName, Phone, Email, Address
                    string query = @"SELECT * FROM Suppliers 
                                     WHERE SupplierID = @id OR SupplierName LIKE '%' + @name + '%'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int id;
                        bool isID = int.TryParse(searchText, out id);

                        cmd.Parameters.AddWithValue("@id", isID ? id : 0);
                        cmd.Parameters.AddWithValue("@name", searchText);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            currentSupplierID = Convert.ToInt32(reader["SupplierID"]);
                            txtSupplierName.Text = reader["SupplierName"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();
                            txtEmail.Text = reader["Email"].ToString();

                            // Note: آپ کے DB میں Address کالم ہے، لیکن ڈیزائن میں txtCity ہے
                            // میں Address کو txtCity میں ڈال رہا ہوں
                            txtCity.Text = reader["Address"].ToString();

                            // Enable fields for editing
                            EnableDisableFields(true);

                            btnUpdate.Enabled = true;
                            btnDelete.Visible = true;

                            // Change form title to show supplier ID
                            label1.Text = $"✏️ Edit Supplier (ID: {currentSupplierID})";
                        }
                        else
                        {
                            MessageBox.Show("Supplier not found.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        reader.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===============================
        // Update Supplier
        // ===============================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentSupplierID == -1)
            {
                MessageBox.Show("No supplier selected. Please search for a supplier first.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validation
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Supplier Name is required", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSupplierName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone is required", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return;
            }

            // Email validation (optional but recommended)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return;
            }

            // Confirm update
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to update Supplier ID: {currentSupplierID}?",
                "Confirm Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Suppliers
                                     SET SupplierName = @name, 
                                         Phone = @phone, 
                                         Email = @email, 
                                         Address = @address
                                     WHERE SupplierID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", currentSupplierID);
                        cmd.Parameters.AddWithValue("@name", txtSupplierName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", txtCity.Text.Trim()); // Address میں city ڈال رہے ہیں

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Supplier updated successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Log update (optional)
                            LogSupplierUpdate(currentSupplierID, "Updated supplier information");

                            // Show success message
                            MessageBox.Show($"Supplier ID {currentSupplierID} has been updated successfully!",
                                "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Update failed or no changes made.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Unique constraint violation
                {
                    MessageBox.Show("Supplier with this name or email already exists.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===============================
        // Delete Supplier
        // ===============================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentSupplierID == -1)
            {
                MessageBox.Show("No supplier selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to delete Supplier ID: {currentSupplierID}?\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // First check if supplier has linked products in SupplierProducts table
                    string checkQuery = @"SELECT COUNT(*) FROM SupplierProducts WHERE SupplierID = @id";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@id", currentSupplierID);

                    int productCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (productCount > 0)
                    {
                        DialogResult deleteProducts = MessageBox.Show(
                            $"This supplier has {productCount} product(s) linked.\nDo you want to delete supplier and all linked products?",
                            "Linked Products Found",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (deleteProducts != DialogResult.Yes)
                            return;

                        // Delete linked products first
                        string deleteProductsQuery = "DELETE FROM SupplierProducts WHERE SupplierID = @id";
                        SqlCommand deleteProductsCmd = new SqlCommand(deleteProductsQuery, conn);
                        deleteProductsCmd.Parameters.AddWithValue("@id", currentSupplierID);
                        deleteProductsCmd.ExecuteNonQuery();
                    }

                    // Now delete supplier
                    string query = "DELETE FROM Suppliers WHERE SupplierID = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", currentSupplierID);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Supplier deleted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Log deletion
                            LogSupplierUpdate(currentSupplierID, "Deleted supplier");

                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Delete failed. Supplier not found.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547) // Foreign key constraint
                {
                    MessageBox.Show("Cannot delete supplier because there are related records in other tables.",
                        "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting supplier: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===============================
        // Cancel / Close Form
        // ===============================
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ===============================
        // Helper: Clear Text Fields
        // ===============================
        private void ClearFields()
        {
            txtSearchID.Clear();
            txtSupplierName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtCity.Clear();
            currentSupplierID = -1;
            btnUpdate.Enabled = false;
            btnDelete.Visible = false;

            // Reset form title
            label1.Text = "✏️ Edit Supplier";

            // Disable fields
            EnableDisableFields(false);

            txtSearchID.Focus();
        }

        // ===============================
        // Enable/Disable Input Fields
        // ===============================
        private void EnableDisableFields(bool enable)
        {
            txtSupplierName.Enabled = enable;
            txtPhone.Enabled = enable;
            txtEmail.Enabled = enable;
            txtCity.Enabled = enable;

            // Change border color based on state
            System.Drawing.Color borderColor = enable ?
                System.Drawing.Color.FromArgb(94, 148, 255) :
                System.Drawing.Color.FromArgb(208, 208, 208);

            txtSupplierName.FocusedState.BorderColor = borderColor;
            txtPhone.FocusedState.BorderColor = borderColor;
            txtEmail.FocusedState.BorderColor = borderColor;
            txtCity.FocusedState.BorderColor = borderColor;
        }

        // ===============================
        // Log Supplier Update (Optional)
        // ===============================
        private void LogSupplierUpdate(int supplierID, string action)
        {
            try
            {
                // Create audit log table if not exists
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SupplierAuditLog')
                    BEGIN
                        CREATE TABLE SupplierAuditLog (
                            LogID INT IDENTITY(1,1) PRIMARY KEY,
                            SupplierID INT NOT NULL,
                            Action NVARCHAR(200),
                            ActionBy NVARCHAR(100),
                            ActionDate DATETIME DEFAULT GETDATE()
                        )
                    END";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create table if not exists
                    SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                    createCmd.ExecuteNonQuery();

                    // Insert audit log
                    string insertQuery = @"INSERT INTO SupplierAuditLog (SupplierID, Action, ActionBy) 
                                          VALUES (@SupplierID, @Action, @ActionBy)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@SupplierID", supplierID);
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

        // ===============================
        // Email Validation Helper
        // ===============================
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // ===============================
        // Key Press Events for Better UX
        // ===============================
        private void txtSearchID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter key to trigger search
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, plus, hyphen, space, and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '+' && e.KeyChar != '-' && e.KeyChar != ' ' &&
                e.KeyChar != '(' && e.KeyChar != ')')
            {
                e.Handled = true;
            }
        }

        private void txtSupplierName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only letters, numbers, spaces, and common business name characters
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) &&
                e.KeyChar != ' ' && e.KeyChar != '.' && e.KeyChar != '&' &&
                e.KeyChar != '-' && e.KeyChar != '\'' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        // ===============================
        // Add New Supplier Feature (Optional)
        // ===============================
        private void AddNewSupplier()
        {
            ClearFields();
            EnableDisableFields(true);
            txtSupplierName.Focus();
            btnUpdate.Text = "Add Supplier";
            label1.Text = "✏️ Add New Supplier";
            currentSupplierID = -1; // Indicates new supplier
        }

        // ===============================
        // Event Handlers Attach (اگر ڈیزائنر میں نہیں ہیں تو)
        // ===============================
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // اگر ڈیزائنر میں event handlers نہیں ہیں تو یہاں attach کریں
            if (!this.DesignMode)
            {
                this.Load += EditSupplier_Load;
                this.btnSearch.Click += btnSearch_Click;
                this.btnUpdate.Click += btnUpdate_Click;
                this.btnDelete.Click += btnDelete_Click;
                this.btnCancel.Click += btnCancel_Click;
                this.txtSearchID.KeyPress += txtSearchID_KeyPress;
                this.txtPhone.KeyPress += txtPhone_KeyPress;
                this.txtSupplierName.KeyPress += txtSupplierName_KeyPress;
            }
        }

        // ===============================
        // Form Closing Event
        // ===============================
        private void EditSupplier_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Any cleanup if needed
        }

        // ===============================
        // Paint Event (اگر needed ہو)
        // ===============================
        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom painting if needed
        }
    }
}