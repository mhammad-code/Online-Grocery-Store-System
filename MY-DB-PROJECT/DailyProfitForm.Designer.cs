using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    partial class DailyProfitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2ShadowPanel5 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblDailyExpenses = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDailyRevenue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvDailyData = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2ShadowPanel4 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.dtpDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblDailyProfit = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel3 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2ShadowPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyData)).BeginInit();
            this.guna2ShadowPanel4.SuspendLayout();
            this.guna2ShadowPanel3.SuspendLayout();
            this.guna2ShadowPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.guna2ShadowPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel5);
            this.guna2ShadowPanel1.Controls.Add(this.dgvDailyData);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel4);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel3);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel2);
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 150;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(1200, 718);
            this.guna2ShadowPanel1.TabIndex = 0;
            this.guna2ShadowPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2ShadowPanel1_Paint);
            // 
            // guna2ShadowPanel5
            // 
            this.guna2ShadowPanel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel5.Controls.Add(this.lblDailyExpenses);
            this.guna2ShadowPanel5.Controls.Add(this.label6);
            this.guna2ShadowPanel5.Controls.Add(this.lblDailyRevenue);
            this.guna2ShadowPanel5.Controls.Add(this.label5);
            this.guna2ShadowPanel5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.guna2ShadowPanel5.Location = new System.Drawing.Point(30, 620);
            this.guna2ShadowPanel5.Name = "guna2ShadowPanel5";
            this.guna2ShadowPanel5.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel5.ShadowDepth = 150;
            this.guna2ShadowPanel5.Size = new System.Drawing.Size(1140, 70);
            this.guna2ShadowPanel5.TabIndex = 5;
            // 
            // lblDailyExpenses
            // 
            this.lblDailyExpenses.AutoSize = true;
            this.lblDailyExpenses.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDailyExpenses.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblDailyExpenses.Location = new System.Drawing.Point(860, 15);
            this.lblDailyExpenses.Name = "lblDailyExpenses";
            this.lblDailyExpenses.Size = new System.Drawing.Size(172, 45);
            this.lblDailyExpenses.TabIndex = 3;
            this.lblDailyExpenses.Text = "PKR 1,020";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(640, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(218, 38);
            this.label6.TabIndex = 2;
            this.label6.Text = "Daily Expenses:";
            // 
            // lblDailyRevenue
            // 
            this.lblDailyRevenue.AutoSize = true;
            this.lblDailyRevenue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDailyRevenue.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblDailyRevenue.Location = new System.Drawing.Point(400, 15);
            this.lblDailyRevenue.Name = "lblDailyRevenue";
            this.lblDailyRevenue.Size = new System.Drawing.Size(172, 45);
            this.lblDailyRevenue.TabIndex = 1;
            this.lblDailyRevenue.Text = "PKR 2,270";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(20, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(211, 38);
            this.label5.TabIndex = 0;
            this.label5.Text = "Daily Revenue:";
            // 
            // dgvDailyData
            // 
            this.dgvDailyData.AllowUserToAddRows = false;
            this.dgvDailyData.AllowUserToDeleteRows = false;
            this.dgvDailyData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDailyData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDailyData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDailyData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDailyData.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDailyData.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDailyData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.dgvDailyData.Location = new System.Drawing.Point(30, 240);
            this.dgvDailyData.MultiSelect = false;
            this.dgvDailyData.Name = "dgvDailyData";
            this.dgvDailyData.ReadOnly = true;
            this.dgvDailyData.RowHeadersVisible = false;
            this.dgvDailyData.RowHeadersWidth = 62;
            this.dgvDailyData.RowTemplate.Height = 35;
            this.dgvDailyData.Size = new System.Drawing.Size(1140, 370);
            this.dgvDailyData.TabIndex = 4;
            this.dgvDailyData.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.dgvDailyData.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvDailyData.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDailyData.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvDailyData.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDailyData.ThemeStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.dgvDailyData.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.dgvDailyData.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDailyData.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDailyData.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dgvDailyData.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDailyData.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDailyData.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvDailyData.ThemeStyle.ReadOnly = true;
            this.dgvDailyData.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.dgvDailyData.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDailyData.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvDailyData.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDailyData.ThemeStyle.RowsStyle.Height = 35;
            this.dgvDailyData.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvDailyData.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            // 
            // guna2ShadowPanel4
            // 
            this.guna2ShadowPanel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel4.Controls.Add(this.dtpDate);
            this.guna2ShadowPanel4.Controls.Add(this.lblDailyProfit);
            this.guna2ShadowPanel4.Controls.Add(this.label3);
            this.guna2ShadowPanel4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.guna2ShadowPanel4.Location = new System.Drawing.Point(30, 120);
            this.guna2ShadowPanel4.Name = "guna2ShadowPanel4";
            this.guna2ShadowPanel4.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel4.ShadowDepth = 150;
            this.guna2ShadowPanel4.Size = new System.Drawing.Size(1140, 100);
            this.guna2ShadowPanel4.TabIndex = 3;
            // 
            // dtpDate
            // 
            this.dtpDate.BorderRadius = 17;
            this.dtpDate.Checked = true;
            this.dtpDate.FillColor = System.Drawing.Color.White;
            this.dtpDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(20, 30);
            this.dtpDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 36);
            this.dtpDate.TabIndex = 2;
            this.dtpDate.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // lblDailyProfit
            // 
            this.lblDailyProfit.AutoSize = true;
            this.lblDailyProfit.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblDailyProfit.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblDailyProfit.Location = new System.Drawing.Point(820, 20);
            this.lblDailyProfit.Name = "lblDailyProfit";
            this.lblDailyProfit.Size = new System.Drawing.Size(257, 65);
            this.lblDailyProfit.TabIndex = 1;
            this.lblDailyProfit.Text = "PKR 1,250";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(250, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 48);
            this.label3.TabIndex = 0;
            this.label3.Text = "Daily Profit:";
            // 
            // guna2ShadowPanel3
            // 
            this.guna2ShadowPanel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel3.Controls.Add(this.btnRefresh);
            this.guna2ShadowPanel3.Controls.Add(this.btnExport);
            this.guna2ShadowPanel3.Controls.Add(this.btnBack);
            this.guna2ShadowPanel3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.guna2ShadowPanel3.Location = new System.Drawing.Point(30, 630);
            this.guna2ShadowPanel3.Name = "guna2ShadowPanel3";
            this.guna2ShadowPanel3.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel3.ShadowDepth = 80;
            this.guna2ShadowPanel3.Size = new System.Drawing.Size(1140, 60);
            this.guna2ShadowPanel3.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Animated = true;
            this.btnRefresh.AutoRoundedCorners = true;
            this.btnRefresh.BorderRadius = 16;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(730, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(140, 35);
            this.btnRefresh.TabIndex = 27;
            this.btnRefresh.Text = "🔄 Refresh";
            // 
            // btnExport
            // 
            this.btnExport.Animated = true;
            this.btnExport.AutoRoundedCorners = true;
            this.btnExport.BorderRadius = 16;
            this.btnExport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExport.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(880, 13);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(140, 35);
            this.btnExport.TabIndex = 26;
            this.btnExport.Text = "📊 Export";
            // 
            // btnBack
            // 
            this.btnBack.Animated = true;
            this.btnBack.AutoRoundedCorners = true;
            this.btnBack.BorderRadius = 16;
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(20, 13);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(140, 35);
            this.btnBack.TabIndex = 25;
            this.btnBack.Text = "⬅️ Back";
            // 
            // guna2ShadowPanel2
            // 
            this.guna2ShadowPanel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel2.Controls.Add(this.label1);
            this.guna2ShadowPanel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.guna2ShadowPanel2.Location = new System.Drawing.Point(30, 30);
            this.guna2ShadowPanel2.Name = "guna2ShadowPanel2";
            this.guna2ShadowPanel2.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel2.ShadowDepth = 200;
            this.guna2ShadowPanel2.Size = new System.Drawing.Size(1140, 70);
            this.guna2ShadowPanel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "📅 DAILY PROFIT";
            // 
            // DailyProfitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 718);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DailyProfitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daily Profit";
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2ShadowPanel5.ResumeLayout(false);
            this.guna2ShadowPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyData)).EndInit();
            this.guna2ShadowPanel4.ResumeLayout(false);
            this.guna2ShadowPanel4.PerformLayout();
            this.guna2ShadowPanel3.ResumeLayout(false);
            this.guna2ShadowPanel2.ResumeLayout(false);
            this.guna2ShadowPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel3;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel5;
        private System.Windows.Forms.Label lblDailyExpenses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDailyRevenue;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDailyData;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel4;
        private System.Windows.Forms.Label lblDailyProfit;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpDate;
    }
}