using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace MY_DB_PROJECT
{
    public partial class Feedback : Form
    {
        private int userID;
        private string username;
        private string connectionString = @"Data Source=DESKTOP-PN6UPNO\SQLEXPRESS01;Initial Catalog=onlinestore;Integrated Security=True;TrustServerCertificate=True";

        public Feedback(int userID, string username)
        {
            InitializeComponent();
            this.userID = userID;
            this.username = username;
        }

        private void Feedback_Load(object sender, EventArgs e)
        {
            LoadProducts();
            SetupRating();
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT ProductName FROM Products WHERE IsActive = 1 ORDER BY ProductName";
                    SqlCommand cmd = new SqlCommand(query, con);

                    SqlDataReader reader = cmd.ExecuteReader();
                    cmbProducts.Items.Clear();
                    cmbProducts.Items.Add("-- Select Product --");

                    while (reader.Read())
                    {
                        cmbProducts.Items.Add(reader["ProductName"].ToString());
                    }
                    reader.Close();
                    con.Close();

                    if (cmbProducts.Items.Count > 0)
                        cmbProducts.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupRating()
        {
            cmbRating.Items.Clear();
            cmbRating.Items.Add("1 ★ - Very Poor");
            cmbRating.Items.Add("2 ★★ - Poor");
            cmbRating.Items.Add("3 ★★★ - Average");
            cmbRating.Items.Add("4 ★★★★ - Good");
            cmbRating.Items.Add("5 ★★★★★ - Excellent");

            cmbRating.SelectedIndex = -1;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (cmbProducts.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a product.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbProducts.Focus();
                    return;
                }

                if (cmbRating.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a rating.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbRating.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFeedback.Text))
                {
                    MessageBox.Show("Please enter your feedback comment.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFeedback.Focus();
                    return;
                }

                string productName = cmbProducts.SelectedItem.ToString();
                int rating = cmbRating.SelectedIndex + 1;
                string comment = txtFeedback.Text.Trim();

                // Check if feedback already exists for this user and product
                bool feedbackExists = CheckExistingFeedback(productName);
                if (feedbackExists)
                {
                    DialogResult result = MessageBox.Show(
                        "You have already submitted feedback for this product. Do you want to update it?",
                        "Existing Feedback Found",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        UpdateFeedback(productName, rating, comment);
                    }
                    return;
                }

                // Insert new feedback
                InsertFeedback(productName, rating, comment);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting feedback: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckExistingFeedback(string productName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"SELECT COUNT(*) FROM Feedback 
                                   WHERE UserID = @UserID AND ProductName = @ProductName 
                                   AND IsActive = 1";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@ProductName", productName);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking existing feedback: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void InsertFeedback(string productName, int rating, string comment)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO Feedback 
                                   (UserID, Username, ProductName, Rating, Comment, IsActive) 
                                   VALUES (@UserID, @Username, @ProductName, @Rating, @Comment, 1)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comment", comment);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thank you for your feedback! Your review has been submitted successfully.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset form
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to submit feedback. Please try again.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFeedback(string productName, int rating, string comment)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"UPDATE Feedback 
                                   SET Rating = @Rating, 
                                       Comment = @Comment, 
                                       FeedbackDate = GETDATE() 
                                   WHERE UserID = @UserID AND ProductName = @ProductName";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comment", comment);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Your feedback has been updated successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset form
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update feedback. Please try again.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating feedback: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            if (cmbProducts.Items.Count > 0)
                cmbProducts.SelectedIndex = 0;

            cmbRating.SelectedIndex = -1;
            txtFeedback.Clear();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Designer event handlers
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e) { }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            // This is just a wrapper to maintain compatibility
            btnSubmit_Click(sender, e);
        }
    }
}