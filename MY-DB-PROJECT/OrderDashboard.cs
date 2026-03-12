using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class OrderDashboard : Form
    {
        // آپ کا connection string
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";
        private DataTable ordersTable;
        private int selectedOrderID = -1;

        public OrderDashboard()
        {
            InitializeComponent();
        }

        private void OrderDashboard_Load(object sender, EventArgs e)
        {
            LoadOrders();
            SetupDataGridView();
            cmbStatus.SelectedIndex = 0; // Default select "Pending"
        }

        // Load orders from database
        private void LoadOrders()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // آپ کے DB ڈیزائن کے مطابق query:
                    // Orders ٹیبل: OrderID, UserID, OrderDate, TotalAmount, OrderStatus, CustomerName, Phone, Address, Email, PaymentMethod
                    // Users ٹیبل: UserID, Username, Email, Role, etc.

                    string query = @"
                        SELECT 
                            o.OrderID,
                            o.OrderDate,
                            ISNULL(o.CustomerName, u.Username) AS CustomerName,
                            o.TotalAmount AS Total,
                            o.OrderStatus AS Status,
                            o.PaymentMethod,
                            o.Phone,
                            o.Address
                        FROM Orders o
                        LEFT JOIN Users u ON o.UserID = u.UserID
                        ORDER BY o.OrderDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    ordersTable = new DataTable();
                    da.Fill(ordersTable);

                    dgvOrders.DataSource = ordersTable;

                    // Apply formatting
                    FormatDataGridView();

                    // Update order count
                    UpdateOrderCount();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Setup DataGridView
        private void SetupDataGridView()
        {
            // Enable selection
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.MultiSelect = false;
            dgvOrders.ReadOnly = true;

            // Add event handler for selection
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;
            dgvOrders.CellDoubleClick += DgvOrders_CellDoubleClick;
        }

        // Format DataGridView
        private void FormatDataGridView()
        {
            if (dgvOrders.Columns.Count > 0)
            {
                // Format columns
                foreach (DataGridViewColumn column in dgvOrders.Columns)
                {
                    switch (column.Name)
                    {
                        case "OrderID":
                            column.HeaderText = "ORDER ID";
                            column.Width = 100;
                            break;
                        case "OrderDate":
                            column.HeaderText = "ORDER DATE";
                            column.Width = 150;
                            column.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                            break;
                        case "CustomerName":
                            column.HeaderText = "CUSTOMER NAME";
                            column.Width = 200;
                            break;
                        case "Total":
                            column.HeaderText = "TOTAL (PKR)";
                            column.Width = 120;
                            column.DefaultCellStyle.Format = "N2";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "Status":
                            column.HeaderText = "STATUS";
                            column.Width = 120;
                            break;
                        case "PaymentMethod":
                            column.HeaderText = "PAYMENT";
                            column.Width = 100;
                            break;
                    }
                }

                // Apply status colors
                foreach (DataGridViewRow row in dgvOrders.Rows)
                {
                    if (row.Cells["Status"].Value != null)
                    {
                        string status = row.Cells["Status"].Value.ToString();

                        switch (status)
                        {
                            case "Pending":
                                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                                row.DefaultCellStyle.Font = new System.Drawing.Font(dgvOrders.Font, System.Drawing.FontStyle.Bold);
                                break;
                            case "Completed":
                                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                                break;
                            case "Confirmed":
                                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                                break;
                            case "Cancelled":
                                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                row.DefaultCellStyle.Font = new System.Drawing.Font(dgvOrders.Font, System.Drawing.FontStyle.Italic);
                                break;
                        }
                    }
                }
            }
        }

        // DataGridView selection changed
        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvOrders.SelectedRows[0];
                if (selectedRow.Cells["OrderID"].Value != null)
                {
                    selectedOrderID = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);

                    // Show current status in combo box
                    if (selectedRow.Cells["Status"].Value != null)
                    {
                        string currentStatus = selectedRow.Cells["Status"].Value.ToString();

                        // Find and select the status in combo box
                        for (int i = 0; i < cmbStatus.Items.Count; i++)
                        {
                            if (cmbStatus.Items[i].ToString() == currentStatus)
                            {
                                cmbStatus.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                selectedOrderID = -1;
            }
        }

        // DataGridView cell double click
        private void DgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get order details
                int orderID = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["OrderID"].Value);
                ShowOrderDetails(orderID);
            }
        }

        // Show order details
        private void ShowOrderDetails(int orderID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            o.OrderID,
                            o.OrderDate,
                            o.CustomerName,
                            o.Phone,
                            o.Address,
                            o.Email,
                            o.TotalAmount,
                            o.OrderStatus,
                            o.PaymentMethod,
                            COUNT(oi.OrderItemID) AS ItemCount
                        FROM Orders o
                        LEFT JOIN OrderItems oi ON o.OrderID = oi.OrderID
                        WHERE o.OrderID = @OrderID
                        GROUP BY o.OrderID, o.OrderDate, o.CustomerName, o.Phone, 
                                 o.Address, o.Email, o.TotalAmount, o.OrderStatus, o.PaymentMethod";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string details = $"Order ID: {reader["OrderID"]}\n" +
                                        $"Order Date: {Convert.ToDateTime(reader["OrderDate"]):dd/MM/yyyy HH:mm}\n" +
                                        $"Customer: {reader["CustomerName"]}\n" +
                                        $"Phone: {reader["Phone"]}\n" +
                                        $"Address: {reader["Address"]}\n" +
                                        $"Email: {reader["Email"]}\n" +
                                        $"Total: PKR {Convert.ToDecimal(reader["TotalAmount"]):N2}\n" +
                                        $"Status: {reader["OrderStatus"]}\n" +
                                        $"Payment: {reader["PaymentMethod"]}\n" +
                                        $"Items: {reader["ItemCount"]}";

                        MessageBox.Show(details, "Order Details",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update order count label
        private void UpdateOrderCount()
        {
            int totalOrders = ordersTable?.Rows.Count ?? 0;
            int pendingOrders = 0;
            int completedOrders = 0;

            if (ordersTable != null)
            {
                foreach (DataRow row in ordersTable.Rows)
                {
                    string status = row["Status"].ToString();
                    if (status == "Pending")
                        pendingOrders++;
                    else if (status == "Completed")
                        completedOrders++;
                }
            }

            label1.Text = $"Order Dashboard (Total: {totalOrders}, Pending: {pendingOrders}, Completed: {completedOrders})";
        }

        // Back button click
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Update selected order status
        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedOrderID == -1)
            {
                MessageBox.Show("Please select an order from the list.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status.", "No Status Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedStatus = cmbStatus.SelectedItem.ToString();

            // Confirm update
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to update Order ID {selectedOrderID} to '{selectedStatus}'?",
                "Confirm Status Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Orders SET OrderStatus = @Status WHERE OrderID = @OrderID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Status", selectedStatus);
                        cmd.Parameters.AddWithValue("@OrderID", selectedOrderID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Order ID {selectedOrderID} status updated to '{selectedStatus}'.",
                                "Status Updated",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Update DataGridView
                            foreach (DataGridViewRow row in dgvOrders.Rows)
                            {
                                if (row.Cells["OrderID"].Value != null &&
                                    Convert.ToInt32(row.Cells["OrderID"].Value) == selectedOrderID)
                                {
                                    row.Cells["Status"].Value = selectedStatus;

                                    // Update row color
                                    switch (selectedStatus)
                                    {
                                        case "Pending":
                                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                                            row.DefaultCellStyle.Font = new System.Drawing.Font(dgvOrders.Font, System.Drawing.FontStyle.Bold);
                                            break;
                                        case "Completed":
                                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                                            row.DefaultCellStyle.Font = dgvOrders.Font;
                                            break;
                                        case "Confirmed":
                                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                                            row.DefaultCellStyle.Font = dgvOrders.Font;
                                            break;
                                        case "Cancelled":
                                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                            row.DefaultCellStyle.Font = new System.Drawing.Font(dgvOrders.Font, System.Drawing.FontStyle.Italic);
                                            break;
                                    }
                                    break;
                                }
                            }

                            // Log the update
                            LogOrderUpdate(selectedOrderID, selectedStatus);

                            // Update order count
                            UpdateOrderCount();
                        }
                        else
                        {
                            MessageBox.Show("Order not found.", "Update Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
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
                MessageBox.Show($"Error updating status: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mark all orders as completed
        private void btnMarkAllCompleted_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to mark ALL orders as 'Completed'?\nThis action cannot be undone!",
                "Confirm Mark All Completed",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Orders SET OrderStatus = 'Completed' WHERE OrderStatus != 'Completed'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        MessageBox.Show($"{rowsAffected} order(s) marked as completed.",
                            "All Done",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh the orders
                        LoadOrders();

                        // Log the bulk update
                        LogBulkOrderUpdate(rowsAffected, "Marked all as completed");
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
                MessageBox.Show($"Error updating all orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refresh orders
        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        // Log order update
        private void LogOrderUpdate(int orderID, string newStatus)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Create audit log table if not exists
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderAuditLog')
                        BEGIN
                            CREATE TABLE OrderAuditLog (
                                LogID INT IDENTITY(1,1) PRIMARY KEY,
                                OrderID INT NOT NULL,
                                OldStatus NVARCHAR(20),
                                NewStatus NVARCHAR(20),
                                ActionBy NVARCHAR(100),
                                ActionDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    SqlCommand createCmd = new SqlCommand(createTableQuery, con);
                    createCmd.ExecuteNonQuery();

                    // Insert log
                    string insertQuery = @"
                        INSERT INTO OrderAuditLog (OrderID, NewStatus, ActionBy)
                        VALUES (@OrderID, @NewStatus, @ActionBy)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@OrderID", orderID);
                    insertCmd.Parameters.AddWithValue("@NewStatus", newStatus);
                    insertCmd.Parameters.AddWithValue("@ActionBy", Environment.UserName);

                    insertCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }

        // Log bulk order update
        private void LogBulkOrderUpdate(int count, string action)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Create bulk audit log table if not exists
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderBulkAuditLog')
                        BEGIN
                            CREATE TABLE OrderBulkAuditLog (
                                LogID INT IDENTITY(1,1) PRIMARY KEY,
                                OrderCount INT,
                                Action NVARCHAR(200),
                                ActionBy NVARCHAR(100),
                                ActionDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    SqlCommand createCmd = new SqlCommand(createTableQuery, con);
                    createCmd.ExecuteNonQuery();

                    // Insert log
                    string insertQuery = @"
                        INSERT INTO OrderBulkAuditLog (OrderCount, Action, ActionBy)
                        VALUES (@OrderCount, @Action, @ActionBy)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@OrderCount", count);
                    insertCmd.Parameters.AddWithValue("@Action", action);
                    insertCmd.Parameters.AddWithValue("@ActionBy", Environment.UserName);

                    insertCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }

        // Form Closing Event
        private void OrderDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup if needed
        }

        private void btnUpdateStatus_Click_1(object sender, EventArgs e)
        {

        }
    }
}