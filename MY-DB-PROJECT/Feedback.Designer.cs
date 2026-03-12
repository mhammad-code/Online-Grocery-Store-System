namespace MY_DB_PROJECT
{
    partial class Feedback
    {
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2ComboBox cmbProducts;
        private Guna.UI2.WinForms.Guna2ComboBox cmbRating;
        private Guna.UI2.WinForms.Guna2TextBox txtFeedback;
        private Guna.UI2.WinForms.Guna2Button btnSubmit;
        private Guna.UI2.WinForms.Guna2Button btnBack;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Feedback));
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFeedback = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbRating = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbProducts = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSubmit = new Guna.UI2.WinForms.Guna2Button();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2ShadowPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel2);
            this.guna2ShadowPanel1.Controls.Add(this.lblTitle);
            this.guna2ShadowPanel1.Controls.Add(this.btnSubmit);
            this.guna2ShadowPanel1.Controls.Add(this.btnBack);
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 200;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(1178, 644);
            this.guna2ShadowPanel1.TabIndex = 1;
            this.guna2ShadowPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2ShadowPanel1_Paint);
            // 
            // guna2ShadowPanel2
            // 
            this.guna2ShadowPanel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel2.Controls.Add(this.label3);
            this.guna2ShadowPanel2.Controls.Add(this.label1);
            this.guna2ShadowPanel2.Controls.Add(this.label2);
            this.guna2ShadowPanel2.Controls.Add(this.txtFeedback);
            this.guna2ShadowPanel2.Controls.Add(this.cmbRating);
            this.guna2ShadowPanel2.Controls.Add(this.cmbProducts);
            this.guna2ShadowPanel2.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.Location = new System.Drawing.Point(302, 75);
            this.guna2ShadowPanel2.Name = "guna2ShadowPanel2";
            this.guna2ShadowPanel2.Radius = 14;
            this.guna2ShadowPanel2.ShadowColor = System.Drawing.Color.WhiteSmoke;
            this.guna2ShadowPanel2.Size = new System.Drawing.Size(540, 468);
            this.guna2ShadowPanel2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(48, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 28);
            this.label3.TabIndex = 8;
            this.label3.Text = "Comment";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(48, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "Product Name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(-21, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 28);
            this.label2.TabIndex = 7;
            this.label2.Text = "Rating";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtFeedback
            // 
            this.txtFeedback.BorderRadius = 10;
            this.txtFeedback.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFeedback.DefaultText = "";
            this.txtFeedback.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtFeedback.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFeedback.ForeColor = System.Drawing.Color.White;
            this.txtFeedback.Location = new System.Drawing.Point(100, 298);
            this.txtFeedback.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtFeedback.Multiline = true;
            this.txtFeedback.Name = "txtFeedback";
            this.txtFeedback.PlaceholderText = "Write your feedback here...";
            this.txtFeedback.SelectedText = "";
            this.txtFeedback.Size = new System.Drawing.Size(384, 144);
            this.txtFeedback.TabIndex = 3;
            // 
            // cmbRating
            // 
            this.cmbRating.AutoRoundedCorners = true;
            this.cmbRating.BackColor = System.Drawing.Color.Transparent;
            this.cmbRating.BorderRadius = 17;
            this.cmbRating.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRating.FillColor = System.Drawing.Color.MediumSeaGreen;
            this.cmbRating.FocusedColor = System.Drawing.Color.Empty;
            this.cmbRating.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbRating.ForeColor = System.Drawing.Color.White;
            this.cmbRating.ItemHeight = 30;
            this.cmbRating.Location = new System.Drawing.Point(173, 183);
            this.cmbRating.Name = "cmbRating";
            this.cmbRating.Size = new System.Drawing.Size(300, 36);
            this.cmbRating.TabIndex = 2;
            // 
            // cmbProducts
            // 
            this.cmbProducts.AutoRoundedCorners = true;
            this.cmbProducts.BackColor = System.Drawing.Color.Transparent;
            this.cmbProducts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProducts.FillColor = System.Drawing.Color.MediumSeaGreen;
            this.cmbProducts.FocusedColor = System.Drawing.Color.Empty;
            this.cmbProducts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProducts.ForeColor = System.Drawing.Color.White;
            this.cmbProducts.ItemHeight = 30;
            this.cmbProducts.Location = new System.Drawing.Point(173, 84);
            this.cmbProducts.Name = "cmbProducts";
            this.cmbProducts.Size = new System.Drawing.Size(300, 36);
            this.cmbProducts.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitle.Location = new System.Drawing.Point(406, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(350, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "User Feedback ";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Animated = true;
            this.btnSubmit.AutoRoundedCorners = true;
            this.btnSubmit.BorderRadius = 21;
            this.btnSubmit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(120)))));
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(302, 560);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(150, 45);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click_1);
            // 
            // btnBack
            // 
            this.btnBack.Animated = true;
            this.btnBack.AutoRoundedCorners = true;
            this.btnBack.BorderRadius = 21;
            this.btnBack.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(70)))), ((int)(((byte)(90)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(692, 560);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 45);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseTransparentBackground = true;
            // 
            // Feedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1178, 644);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.Name = "Feedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Feedback";
            this.Load += new System.EventHandler(this.Feedback_Load);
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2ShadowPanel1.PerformLayout();
            this.guna2ShadowPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel2;
        private System.Windows.Forms.Label label3;
    }
}
