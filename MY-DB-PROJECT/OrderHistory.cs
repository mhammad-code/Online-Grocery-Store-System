using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class OrderHistory : Form
    {
        // 🔴 **CONNECTION STRING CORRECT KARO**
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";
        private int userID;

        public OrderHistory()
        {
            InitializeComponent();
        }

        public OrderHistory(int userId) : this()
        {
            this.userID = userId;
        }

        private void OrderHistory_Load(object sender, EventArgs e)
        {
            LoadOrderSummary();
            cmbOrderFilter.SelectedIndex = 0;
            dgvOrderHistory.Visible = false;
            lblNoData.Visible = true;
            lblNoData.Text = "Click 'Search' button to view your orders";
            FormatPanels();
            dgvOrderHistory.DataError += DgvOrderHistory_DataError;

            // Load initial data
            LoadAllOrders();
        }

        private void DgvOrderHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void FormatPanels()
        {
            panel1.BorderRadius = 15;
            panel2.BorderRadius = 15;
            panel3.BorderRadius = 15;
            panel4.BorderRadius = 15;
        }

        private void LoadOrderSummary()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Total Orders
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE UserID=@UserID", con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    lblTotalOrders.Text = cmd.ExecuteScalar().ToString();

                    // Total Amount
                    cmd.CommandText = "SELECT ISNULL(SUM(TotalAmount),0) FROM Orders WHERE UserID=@UserID";
                    decimal totalAmount = Convert.ToDecimal(cmd.ExecuteScalar());
                    lblTotalAmount.Text = $"PKR {totalAmount:N2}";

                    // Pending Orders
                    cmd.CommandText = "SELECT COUNT(*) FROM Orders WHERE UserID=@UserID AND OrderStatus='Pending'";
                    lblTotalPending.Text = cmd.ExecuteScalar().ToString();

                    // Completed Orders
                    cmd.CommandText = "SELECT COUNT(*) FROM Orders WHERE UserID=@UserID AND OrderStatus='Completed'";
                    lblTotalCompleted.Text = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order summary: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllOrders()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            OrderID AS 'Order ID', 
                            OrderDate AS 'Order Date',
                            TotalAmount AS 'Total Amount', 
                            OrderStatus AS 'Status', 
                            PaymentMethod AS 'Payment Method',
                            CustomerName AS 'Customer Name'
                        FROM Orders 
                        WHERE UserID = @UserID 
                        ORDER BY OrderDate DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DisplayOrders(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayOrders(DataTable dt)
        {
            dgvOrderHistory.DataSource = dt;
            dgvOrderHistory.Visible = dt.Rows.Count > 0;
            lblNoData.Visible = dt.Rows.Count == 0;

            if (dt.Rows.Count == 0)
            {
                lblNoData.Text = "No orders found for your account";
            }

            SetupDataGridViewColumns();
            FormatDataGridView();
        }

        private void SetupDataGridViewColumns()
        {
            if (dgvOrderHistory.Columns.Count > 0)
            {
                // Adjust column widths
                if (dgvOrderHistory.Columns.Contains("Order ID"))
                    dgvOrderHistory.Columns["Order ID"].Width = 80;

                if (dgvOrderHistory.Columns.Contains("Order Date"))
                {
                    dgvOrderHistory.Columns["Order Date"].Width = 150;
                    dgvOrderHistory.Columns["Order Date"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";
                }

                if (dgvOrderHistory.Columns.Contains("Total Amount"))
                {
                    dgvOrderHistory.Columns["Total Amount"].Width = 120;
                    dgvOrderHistory.Columns["Total Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvOrderHistory.Columns["Total Amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvOrderHistory.Columns["Total Amount"].DefaultCellStyle.Format = "N2";
                }

                if (dgvOrderHistory.Columns.Contains("Status"))
                {
                    dgvOrderHistory.Columns["Status"].Width = 120;
                    dgvOrderHistory.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvOrderHistory.Columns.Contains("Payment Method"))
                {
                    dgvOrderHistory.Columns["Payment Method"].Width = 150;
                    dgvOrderHistory.Columns["Payment Method"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvOrderHistory.Columns.Contains("Customer Name"))
                {
                    dgvOrderHistory.Columns["Customer Name"].Width = 150;
                }
            }
        }

        private void FormatDataGridView()
        {
            foreach (DataGridViewRow row in dgvOrderHistory.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString().ToLower();

                    if (status == "pending")
                    {
                        row.Cells["Status"].Style.BackColor = Color.FromArgb(255, 193, 7); // Orange
                        row.Cells["Status"].Style.ForeColor = Color.Black;
                    }
                    else if (status == "completed")
                    {
                        row.Cells["Status"].Style.BackColor = Color.FromArgb(40, 167, 69); // Green
                        row.Cells["Status"].Style.ForeColor = Color.White;
                    }
                    else if (status == "confirmed")
                    {
                        row.Cells["Status"].Style.BackColor = Color.FromArgb(0, 123, 255); // Blue
                        row.Cells["Status"].Style.ForeColor = Color.White;
                    }
                    else if (status == "cancelled")
                    {
                        row.Cells["Status"].Style.BackColor = Color.FromArgb(220, 53, 69); // Red
                        row.Cells["Status"].Style.ForeColor = Color.White;
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbOrderFilter.SelectedItem == null)
            {
                MessageBox.Show("Please select a filter option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filter = cmbOrderFilter.SelectedItem.ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"
                        SELECT 
                            OrderID AS 'Order ID', 
                            OrderDate AS 'Order Date',
                            TotalAmount AS 'Total Amount', 
                            OrderStatus AS 'Status', 
                            PaymentMethod AS 'Payment Method',
                            CustomerName AS 'Customer Name'
                        FROM Orders 
                        WHERE UserID = @UserID";

                    if (filter == "Pending Orders")
                        query += " AND OrderStatus = 'Pending'";
                    else if (filter == "Completed Orders")
                        query += " AND OrderStatus = 'Completed'";

                    query += " ORDER BY OrderDate DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DisplayOrders(dt);

                    if (dt.Rows.Count == 0)
                    {
                        lblNoData.Text = $"No {filter.ToLower()} found";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrderSummary();
            LoadAllOrders();
            cmbOrderFilter.SelectedIndex = 0;
            MessageBox.Show("Order history refreshed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvOrderHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvOrderHistory.Rows.Count)
            {
                DataGridViewRow row = dgvOrderHistory.Rows[e.RowIndex];

                // Get OrderID for order details
                int orderID = Convert.ToInt32(row.Cells["Order ID"].Value);

                // Show order details with items
                ShowOrderDetails(orderID);
            }
        }

        private void ShowOrderDetails(int orderID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Get order header details
                    string orderQuery = @"
                        SELECT 
                            OrderID,
                            OrderDate,
                            TotalAmount,
                            OrderStatus,
                            PaymentMethod,
                            CustomerName,
                            Phone,
                            Address,
                            Email
                        FROM Orders 
                        WHERE OrderID = @OrderID";

                    SqlCommand orderCmd = new SqlCommand(orderQuery, con);
                    orderCmd.Parameters.AddWithValue("@OrderID", orderID);

                    using (SqlDataReader orderReader = orderCmd.ExecuteReader())
                    {
                        if (orderReader.Read())
                        {
                            string details = "📋 ORDER DETAILS\n";
                            details += "═══════════════════════════\n\n";
                            details += $"🆔 Order ID: {orderReader["OrderID"]}\n";
                            details += $"📅 Order Date: {Convert.ToDateTime(orderReader["OrderDate"]):dd-MMM-yyyy HH:mm}\n";
                            details += $"💰 Total Amount: PKR {Convert.ToDecimal(orderReader["TotalAmount"]):N2}\n";
                            details += $"📊 Status: {orderReader["OrderStatus"]}\n";
                            details += $"💳 Payment Method: {orderReader["PaymentMethod"]}\n";
                            details += $"👤 Customer: {orderReader["CustomerName"]}\n";
                            details += $"📞 Phone: {orderReader["Phone"]}\n";
                            details += $"📍 Address: {orderReader["Address"]}\n";
                            details += $"📧 Email: {orderReader["Email"]}\n\n";
                            details += "═══════════════════════════\n";
                            details += "🛒 ORDER ITEMS\n";
                            details += "═══════════════════════════\n";

                            orderReader.Close();

                            // Get order items
                            string itemsQuery = @"
                                SELECT 
                                    ProductName,
                                    Quantity,
                                    Price,
                                    Quantity * Price AS Subtotal
                                FROM OrderItems 
                                WHERE OrderID = @OrderID";

                            SqlCommand itemsCmd = new SqlCommand(itemsQuery, con);
                            itemsCmd.Parameters.AddWithValue("@OrderID", orderID);

                            using (SqlDataReader itemsReader = itemsCmd.ExecuteReader())
                            {
                                int itemNumber = 1;
                                decimal total = 0;

                                while (itemsReader.Read())
                                {
                                    string productName = itemsReader["ProductName"].ToString();
                                    int quantity = Convert.ToInt32(itemsReader["Quantity"]);
                                    decimal price = Convert.ToDecimal(itemsReader["Price"]);
                                    decimal subtotal = Convert.ToDecimal(itemsReader["Subtotal"]);
                                    total += subtotal;

                                    details += $"\n{itemNumber}. {productName}\n";
                                    details += $"   Qty: {quantity} × PKR {price:N2} = PKR {subtotal:N2}\n";
                                    itemNumber++;
                                }

                                details += $"\n═══════════════════════════\n";
                                details += $"💵 TOTAL: PKR {total:N2}\n";
                                details += $"═══════════════════════════\n";
                            }

                            MessageBox.Show(details, "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}