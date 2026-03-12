using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    public partial class PlaceOrderForm : Form
    {
        private int userID;
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public PlaceOrderForm(int userID)
        {
            InitializeComponent();
            this.userID = userID;
            InitializeDataGridView();
            LoadProductsFromDB();
            SetupEventHandlers();
            UpdateTotal();
        }

        private void SetupEventHandlers()
        {
            btnRefreshProducts.Click += (s, e) => LoadProductsFromDB();
            btnClearCart.Click += (s, e) => ClearCart();
            btnBack.Click += (s, e) => this.Close();
            btnPlaceOrder.Click += (s, e) => PlaceOrder();
            btnAddToCart.Click += btnAddToCart_Click;
            cmbProducts.SelectedIndexChanged += cmbProducts_SelectedIndexChanged;
            numQuantity.ValueChanged += numQuantity_ValueChanged;
            dgvCart.CellClick += dgvCart_CellClick;
        }

        private void InitializeDataGridView()
        {
            dgvCart.Columns.Clear();

            // DataGridView columns
            // Product ID (hidden)
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductID",
                HeaderText = "Product ID",
                Visible = false
            });

            // Product Name
            DataGridViewTextBoxColumn colProductName = new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Product Name",
                Width = 350,
                ReadOnly = true
            };
            colProductName.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F),
                Padding = new Padding(15, 0, 10, 0),
                ForeColor = Color.WhiteSmoke
            };
            dgvCart.Columns.Add(colProductName);

            // Price
            DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn
            {
                Name = "Price",
                HeaderText = "Price (PKR)",
                Width = 150,
                ReadOnly = true
            };
            colPrice.DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10F),
                Padding = new Padding(0, 0, 15, 0),
                ForeColor = Color.WhiteSmoke
            };
            dgvCart.Columns.Add(colPrice);

            // Quantity
            DataGridViewTextBoxColumn colQuantity = new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Quantity",
                Width = 120,
                ReadOnly = true
            };
            colQuantity.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke
            };
            dgvCart.Columns.Add(colQuantity);

            // Subtotal
            DataGridViewTextBoxColumn colSubtotal = new DataGridViewTextBoxColumn
            {
                Name = "Subtotal",
                HeaderText = "Subtotal (PKR)",
                Width = 180,
                ReadOnly = true
            };
            colSubtotal.DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Padding = new Padding(0, 0, 15, 0),
                ForeColor = Color.FromArgb(46, 204, 113) // Green color for subtotal
            };
            dgvCart.Columns.Add(colSubtotal);

            // Remove button
            DataGridViewButtonColumn colRemove = new DataGridViewButtonColumn
            {
                Name = "Remove",
                HeaderText = "Action",
                Text = "❌ Remove",
                UseColumnTextForButtonValue = true,
                Width = 130,
                FlatStyle = FlatStyle.Flat
            };
            colRemove.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60), // Red color
                ForeColor = Color.White
            };
            dgvCart.Columns.Add(colRemove);

            // Update button states
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasItems = dgvCart.Rows.Count > 0;
            btnClearCart.Enabled = hasItems;
            btnPlaceOrder.Enabled = hasItems;

            if (hasItems)
            {
                decimal total = CalculateTotal();
                btnPlaceOrder.Text = $"✅ Place Order (PKR {total:N2})";
            }
            else
            {
                btnPlaceOrder.Text = "✅ Place Order";
            }
        }

        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                }
            }
            return total;
        }

        private void LoadProductsFromDB()
        {
            try
            {
                cmbProducts.Items.Clear();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            P.ProductID, 
                            P.ProductName, 
                            P.Price, 
                            P.Stock
                        FROM Products P
                        WHERE P.Stock > 0
                        ORDER BY P.ProductName";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        int productID = Convert.ToInt32(dr["ProductID"]);
                        string productName = dr["ProductName"].ToString();
                        decimal price = Convert.ToDecimal(dr["Price"]);
                        int stock = Convert.ToInt32(dr["Stock"]);

                        cmbProducts.Items.Add(new ProductItem(
                            productID,
                            productName,
                            price,
                            stock,
                            ""
                        ));
                    }

                    dr.Close();
                }

                if (cmbProducts.Items.Count > 0)
                {
                    cmbProducts.SelectedIndex = 0;
                    UpdateStockInfo();
                }
                else
                {
                    MessageBox.Show("No active products available in stock.",
                                  "No Products",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void UpdateStockInfo()
        {
            if (cmbProducts.SelectedItem is ProductItem selectedProduct)
            {
                numQuantity.Maximum = selectedProduct.Stock;
                numQuantity.Value = Math.Min(numQuantity.Value, selectedProduct.Stock);
                if (selectedProduct.Stock == 0)
                {
                    numQuantity.Value = 0;
                    btnAddToCart.Enabled = false;
                }
                else
                {
                    btnAddToCart.Enabled = true;
                }
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product first.",
                              "No Product Selected",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            ProductItem selectedProduct = (ProductItem)cmbProducts.SelectedItem;
            int quantity = (int)numQuantity.Value;

            if (quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity (minimum 1).",
                              "Invalid Quantity",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            if (quantity > selectedProduct.Stock)
            {
                MessageBox.Show($"Cannot add {quantity} items. Only {selectedProduct.Stock} available in stock.",
                              "Insufficient Stock",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            // Check if product already in cart
            bool productExists = false;
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                int rowProductID = Convert.ToInt32(row.Cells["ProductID"].Value);
                if (rowProductID == selectedProduct.ProductID)
                {
                    int currentQuantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    int newQuantity = currentQuantity + quantity;

                    if (newQuantity > selectedProduct.Stock)
                    {
                        MessageBox.Show($"Cannot add more. Total would be {newQuantity}, but only {selectedProduct.Stock} available.",
                                      "Stock Limit Exceeded",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }

                    row.Cells["Quantity"].Value = newQuantity;
                    row.Cells["Subtotal"].Value = newQuantity * selectedProduct.Price;
                    productExists = true;
                    break;
                }
            }

            // If product not in cart, add new row
            if (!productExists)
            {
                int rowIndex = dgvCart.Rows.Add();
                DataGridViewRow newRow = dgvCart.Rows[rowIndex];

                newRow.Cells["ProductID"].Value = selectedProduct.ProductID;
                newRow.Cells["ProductName"].Value = selectedProduct.ProductName;
                newRow.Cells["Price"].Value = selectedProduct.Price;
                newRow.Cells["Quantity"].Value = quantity;
                newRow.Cells["Subtotal"].Value = quantity * selectedProduct.Price;
            }

            UpdateTotal();
            numQuantity.Value = 1;
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            int itemCount = 0;

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                    itemCount += Convert.ToInt32(row.Cells["Quantity"].Value);
                }
            }

            lblTotal.Text = $"Total ({itemCount} items): PKR {total:N2}";
            UpdateButtonStates();
        }

        private void ClearCart()
        {
            if (dgvCart.Rows.Count == 0)
                return;

            DialogResult result = MessageBox.Show("Are you sure you want to clear all items from the cart?",
                                                "Clear Cart",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dgvCart.Rows.Clear();
                UpdateTotal();
            }
        }

        private void dgvCart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvCart.Columns[e.ColumnIndex].Name == "Remove")
            {
                DataGridViewRow row = dgvCart.Rows[e.RowIndex];

                DialogResult result = MessageBox.Show($"Remove {row.Cells["Quantity"].Value} x {row.Cells["ProductName"].Value} from cart?",
                                                    "Remove Item",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dgvCart.Rows.RemoveAt(e.RowIndex);
                    UpdateTotal();
                }
            }
        }

        private void PlaceOrder()
        {
            if (dgvCart.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add products before placing an order.",
                              "Empty Cart",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            // First confirmation message box
            decimal totalAmount = CalculateTotal();
            int itemCount = 0;
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                itemCount += Convert.ToInt32(row.Cells["Quantity"].Value);
            }

            DialogResult confirm = MessageBox.Show(
                $"Order Summary:\n\n" +
                $"Total Items: {itemCount}\n" +
                $"Total Amount: PKR {totalAmount:N2}\n\n" +
                $"Proceed to checkout?\n" +
                $"Are you sure you want to place this order?",
                "Confirm Order",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            // Check if all products are still in stock
            if (!ValidateStockAvailability())
            {
                MessageBox.Show("Some products in your cart are no longer available in sufficient quantity.",
                              "Stock Issue",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                LoadProductsFromDB();
                return;
            }

            // Get current user's username and email from database
            string username = "", email = "";
            GetUserLoginDetails(ref username, ref email);

            // Open UserDetailsForm with username and email
            using (UserDetailsForm userDetailsForm = new UserDetailsForm(userID, username, email))
            {
                if (userDetailsForm.ShowDialog() == DialogResult.OK)
                {
                    // Proceed to payment method form
                    ShowPaymentMethodForm();
                }
            }
        }

        private void GetUserLoginDetails(ref string username, ref string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            Username,
                            Email
                        FROM Users 
                        WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            username = dr["Username"] != DBNull.Value ? dr["Username"].ToString() : "";
                            email = dr["Email"] != DBNull.Value ? dr["Email"].ToString() : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting user details: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔥 **YAHAN SE NAYA CODE SHURU HOTA HAI**
        // Stock update karne ka function
        private bool UpdateStockAfterOrder()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    foreach (DataGridViewRow row in dgvCart.Rows)
                    {
                        int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                        string query = @"
                            UPDATE Products 
                            SET Stock = Stock - @Quantity 
                            WHERE ProductID = @ProductID";

                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}",
                              "Stock Update Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return false;
            }
        }

        // 🔥 **ShowPaymentMethodForm() ko update karo**
        private void ShowPaymentMethodForm()
        {
            // Calculate total amount
            decimal totalAmount = 0;
            string productDetails = "";

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                string productName = row.Cells["ProductName"].Value.ToString();
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                decimal subtotal = Convert.ToDecimal(row.Cells["Subtotal"].Value);

                totalAmount += subtotal;
                productDetails += $"{productName} x{quantity} @ PKR {price:F2};";
            }

            // Open PaymentMethodForm
            using (PaymentMethodForm paymentMethodForm = new PaymentMethodForm(userID, productDetails, totalAmount, dgvCart))
            {
                if (paymentMethodForm.ShowDialog() == DialogResult.OK)
                {
                    // 🔥 **PEHLE STOCK UPDATE KARO**
                    if (UpdateStockAfterOrder())
                    {
                        // Phir cart clear karo
                        dgvCart.Rows.Clear();
                        UpdateTotal();
                        LoadProductsFromDB();

                        // Show success message
                        MessageBox.Show("✅ Order placed successfully!\n\n" +
                                      "Thank you for shopping with us.\n" +
                                      "Stock has been updated in the database.",
                                      "Order Confirmed",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        // Close the form
                        this.Hide();
                    }
                }
            }
        }
        // 🔥 **YAHAN TAK NAYA CODE HAI**

        private bool ValidateStockAvailability()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    foreach (DataGridViewRow row in dgvCart.Rows)
                    {
                        int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                        int cartQuantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                        string query = "SELECT Stock FROM Products WHERE ProductID = @ProductID";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ProductID", productID);

                        object result = cmd.ExecuteScalar();
                        if (result == null || Convert.ToInt32(result) < cartQuantity)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStockInfo();
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            UpdateStockInfo();
        }

        // Helper class for ComboBox items
        private class ProductItem
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string Category { get; set; }

            public ProductItem(int productID, string productName, decimal price, int stock, string category)
            {
                ProductID = productID;
                ProductName = productName;
                Price = price;
                Stock = stock;
                Category = category;
            }

            public override string ToString()
            {
                return $"{ProductName} - PKR {Price:N2}";
            }
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            // Ye empty rehne do, hum ne already SetupEventHandlers() mein handle kar diya hai
        }

        private void PlaceOrderForm_Load(object sender, EventArgs e)
        {
            // Ye empty hi rehne do
        }
    }
}