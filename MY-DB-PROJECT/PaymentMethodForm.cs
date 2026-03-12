using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class PaymentMethodForm : Form
    {
        private int userID;
        private string productDetails;
        private decimal totalAmount;
        private DataGridView cartGrid;
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        // Constructor should match designer
        public PaymentMethodForm(int userID, string productDetails, decimal totalAmount, DataGridView cartGrid)
        {
            InitializeComponent();
            this.userID = userID;
            this.productDetails = productDetails;
            this.totalAmount = totalAmount;
            this.cartGrid = cartGrid;
        }

        private void PaymentMethodForm_Load(object sender, EventArgs e)
        {
            // Set default payment method
            rbtncash.Checked = true;

            // Add total amount label to form (if not in designer)
            AddTotalAmountLabel();
        }

        private void AddTotalAmountLabel()
        {
            // Create and add total amount label dynamically
            Label lblTotal = new Label();
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold);
            lblTotal.ForeColor = Color.Maroon;
            lblTotal.Location = new Point(50, 340); // Adjust position as needed
            lblTotal.Text = $"Total: PKR {totalAmount:N2}";
            lblTotal.Name = "lblTotalAmount";

            // Add to group box
            guna2GroupBox1.Controls.Add(lblTotal);
        }

        // Note: Button name in designer is guna2Button1, not btnPayNow
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string paymentMethod = GetSelectedPaymentMethod();

            if (string.IsNullOrEmpty(paymentMethod))
            {
                MessageBox.Show("Please select a payment method!", "Payment Required",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Process the order
            if (ProcessOrder(paymentMethod))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // Note: Button name in designer is guna2Button2, not btnEditOrder
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to edit order details?",
                                                "Edit Order",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private string GetSelectedPaymentMethod()
        {
            if (rbtncard.Checked)
                return "Credit/Debit Card";
            else if (rbtnbank.Checked)
                return "Bank Transfer";
            else if (rbtncash.Checked)
                return "Cash on Delivery";
            else
                return "";
        }

        private bool ProcessOrder(string paymentMethod)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // Get user details
                        string customerName = "", phone = "", address = "", email = "";
                        GetUserDetails(con, transaction, ref customerName, ref phone, ref address, ref email);

                        // 1. Insert into Orders table
                        string insertOrderQuery = @"
                            INSERT INTO Orders (
                                UserID, 
                                TotalAmount, 
                                CustomerName, 
                                Phone, 
                                Address, 
                                Email, 
                                PaymentMethod,
                                OrderStatus
                            ) 
                            VALUES (
                                @UserID, 
                                @TotalAmount, 
                                @CustomerName, 
                                @Phone, 
                                @Address, 
                                @Email, 
                                @PaymentMethod,
                                'Pending'
                            );
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand orderCmd = new SqlCommand(insertOrderQuery, con, transaction);
                        orderCmd.Parameters.AddWithValue("@UserID", userID);
                        orderCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        orderCmd.Parameters.AddWithValue("@CustomerName", customerName);
                        orderCmd.Parameters.AddWithValue("@Phone", phone);
                        orderCmd.Parameters.AddWithValue("@Address", address);
                        orderCmd.Parameters.AddWithValue("@Email", email);
                        orderCmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        int orderID = Convert.ToInt32(orderCmd.ExecuteScalar());

                        // 2. Insert into OrderItems table and Update Stock
                        foreach (DataGridViewRow row in cartGrid.Rows)
                        {
                            int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                            string productName = row.Cells["ProductName"].Value.ToString();
                            int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                            decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                            // Insert Order Item
                            string insertItemQuery = @"
                                INSERT INTO OrderItems (
                                    OrderID, 
                                    ProductID, 
                                    ProductName, 
                                    Quantity, 
                                    Price
                                ) 
                                VALUES (
                                    @OrderID, 
                                    @ProductID, 
                                    @ProductName, 
                                    @Quantity, 
                                    @Price
                                )";

                            SqlCommand itemCmd = new SqlCommand(insertItemQuery, con, transaction);
                            itemCmd.Parameters.AddWithValue("@OrderID", orderID);
                            itemCmd.Parameters.AddWithValue("@ProductID", productID);
                            itemCmd.Parameters.AddWithValue("@ProductName", productName);
                            itemCmd.Parameters.AddWithValue("@Quantity", quantity);
                            itemCmd.Parameters.AddWithValue("@Price", price);
                            itemCmd.ExecuteNonQuery();

                            // Update Product Stock
                            string updateStockQuery = @"
                                UPDATE Products 
                                SET Stock = Stock - @Quantity 
                                WHERE ProductID = @ProductID AND Stock >= @Quantity";

                            SqlCommand stockCmd = new SqlCommand(updateStockQuery, con, transaction);
                            stockCmd.Parameters.AddWithValue("@ProductID", productID);
                            stockCmd.Parameters.AddWithValue("@Quantity", quantity);
                            stockCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        // Show order summary
                        ShowOrderSummary(orderID, customerName, paymentMethod);

                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing order: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return false;
            }
        }

        private void GetUserDetails(SqlConnection con, SqlTransaction transaction,
                                  ref string customerName, ref string phone,
                                  ref string address, ref string email)
        {
            string query = @"
                SELECT 
                    Username,
                    Phone,
                    Address,
                    Email
                FROM Users 
                WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, con, transaction);
            cmd.Parameters.AddWithValue("@UserID", userID);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    customerName = dr["Username"] != DBNull.Value ? dr["Username"].ToString() : "";
                    phone = dr["Phone"] != DBNull.Value ? dr["Phone"].ToString() : "";
                    address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : "";
                    email = dr["Email"] != DBNull.Value ? dr["Email"].ToString() : "";
                }
                dr.Close();
            }
        }

        private void ShowOrderSummary(int orderID, string customerName, string paymentMethod)
        {
            StringBuilder summary = new StringBuilder();
            summary.AppendLine("🎉 ORDER PLACED SUCCESSFULLY! 🎉");
            summary.AppendLine("═══════════════════════════════");
            summary.AppendLine($"📄 Order ID: #{orderID}");
            summary.AppendLine($"📅 Date: {DateTime.Now:dd/MM/yyyy HH:mm}");
            summary.AppendLine($"👤 Customer: {customerName}");
            summary.AppendLine($"💰 Payment Method: {paymentMethod}");
            summary.AppendLine($"💵 Total Amount: PKR {totalAmount:N2}");
            summary.AppendLine();
            summary.AppendLine("🛒 Ordered Items:");
            summary.AppendLine("────────────────");

            foreach (DataGridViewRow row in cartGrid.Rows)
            {
                summary.AppendLine($"{row.Cells["ProductName"].Value} x{row.Cells["Quantity"].Value} = PKR {row.Cells["Subtotal"].Value:N2}");
            }

            summary.AppendLine("═══════════════════════════════");
            summary.AppendLine("✅ Thank you for your order!");
            summary.AppendLine("Your order will be processed soon.");

            MessageBox.Show(summary.ToString(), "Order Summary",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}