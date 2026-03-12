namespace MY_DB_PROJECT
{
    partial class PaymentMethodForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentMethodForm));
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.rbtncash = new Guna.UI2.WinForms.Guna2RadioButton();
            this.rbtnbank = new Guna.UI2.WinForms.Guna2RadioButton();
            this.rbtncard = new Guna.UI2.WinForms.Guna2RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel1.Controls.Add(this.guna2GroupBox1);
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(392, 76);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 200;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(531, 424);
            this.guna2ShadowPanel1.TabIndex = 0;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox1.BorderRadius = 20;
            this.guna2GroupBox1.Controls.Add(this.guna2Button2);
            this.guna2GroupBox1.Controls.Add(this.guna2Button1);
            this.guna2GroupBox1.Controls.Add(this.rbtncash);
            this.guna2GroupBox1.Controls.Add(this.rbtnbank);
            this.guna2GroupBox1.Controls.Add(this.rbtncard);
            this.guna2GroupBox1.Controls.Add(this.label1);
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(50, 29);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(427, 372);
            this.guna2GroupBox1.TabIndex = 0;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Animated = true;
            this.guna2Button1.AutoRoundedCorners = true;
            this.guna2Button1.BorderRadius = 22;
            this.guna2Button1.FillColor = System.Drawing.Color.Maroon;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI Black", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(227, 296);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(152, 47);
            this.guna2Button1.TabIndex = 0;
            this.guna2Button1.Text = "Pay Now";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // rbtncash
            // 
            this.rbtncash.CheckedState.BorderThickness = 0;
            this.rbtncash.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold);
            this.rbtncash.Location = new System.Drawing.Point(98, 230);
            this.rbtncash.Name = "rbtncash";
            this.rbtncash.Size = new System.Drawing.Size(243, 40);
            this.rbtncash.TabIndex = 1;
            this.rbtncash.Text = "Cash On Delivery";
            this.rbtncash.UncheckedState.BorderThickness = 0;
            // 
            // rbtnbank
            // 
            this.rbtnbank.CheckedState.BorderThickness = 0;
            this.rbtnbank.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold);
            this.rbtnbank.Location = new System.Drawing.Point(98, 159);
            this.rbtnbank.Name = "rbtnbank";
            this.rbtnbank.Size = new System.Drawing.Size(243, 40);
            this.rbtnbank.TabIndex = 2;
            this.rbtnbank.Text = "Bank Transfer";
            this.rbtnbank.UncheckedState.BorderThickness = 0;
            // 
            // rbtncard
            // 
            this.rbtncard.Checked = true;
            this.rbtncard.CheckedState.BorderThickness = 0;
            this.rbtncard.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold);
            this.rbtncard.Location = new System.Drawing.Point(98, 81);
            this.rbtncard.Name = "rbtncard";
            this.rbtncard.Size = new System.Drawing.Size(243, 40);
            this.rbtncard.TabIndex = 3;
            this.rbtncard.TabStop = true;
            this.rbtncard.Text = "Credit/Debit Card";
            this.rbtncard.UncheckedState.BorderThickness = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(43, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 38);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select Payment Method";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // guna2Button2
            // 
            this.guna2Button2.Animated = true;
            this.guna2Button2.AutoRoundedCorners = true;
            this.guna2Button2.BorderRadius = 22;
            this.guna2Button2.FillColor = System.Drawing.Color.Maroon;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI Black", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(50, 296);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(152, 47);
            this.guna2Button2.TabIndex = 5;
            this.guna2Button2.Text = "Edit Details";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // PaymentMethodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1309, 644);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.Name = "PaymentMethodForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaymentMethodForm";
            this.Load += new System.EventHandler(this.PaymentMethodForm_Load);
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2RadioButton rbtncard;
        private Guna.UI2.WinForms.Guna2RadioButton rbtncash;
        private Guna.UI2.WinForms.Guna2RadioButton rbtnbank;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}
