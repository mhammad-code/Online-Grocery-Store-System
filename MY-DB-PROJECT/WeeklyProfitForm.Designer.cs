using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    partial class WeeklyProfitForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeeklyProfitForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2ShadowPanel5 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblWeeklyExpenses = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblWeeklyRevenue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvWeeklyData = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2ShadowPanel4 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.cmbWeek = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblWeeklyProfit = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel3 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2ShadowPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeeklyData)).BeginInit();
            this.guna2ShadowPanel4.SuspendLayout();
            this.guna2ShadowPanel3.SuspendLayout();
            this.guna2ShadowPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("guna2ShadowPanel1.BackgroundImage")));
            this.guna2ShadowPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.guna2ShadowPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel5);
            this.guna2ShadowPanel1.Controls.Add(this.dgvWeeklyData);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel4);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel3);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel2);
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 150;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(1200, 743);
            this.guna2ShadowPanel1.TabIndex = 0;
            // 
            // guna2ShadowPanel5
            // 
            this.guna2ShadowPanel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel5.Controls.Add(this.lblWeeklyExpenses);
            this.guna2ShadowPanel5.Controls.Add(this.label6);
            this.guna2ShadowPanel5.Controls.Add(this.lblWeeklyRevenue);
            this.guna2ShadowPanel5.Controls.Add(this.label5);
            this.guna2ShadowPanel5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.guna2ShadowPanel5.Location = new System.Drawing.Point(30, 620);
            this.guna2ShadowPanel5.Name = "guna2ShadowPanel5";
            this.guna2ShadowPanel5.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel5.ShadowDepth = 150;
            this.guna2ShadowPanel5.Size = new System.Drawing.Size(1140, 70);
            this.guna2ShadowPanel5.TabIndex = 5;
            // 
            // lblWeeklyExpenses
            // 
            this.lblWeeklyExpenses.AutoSize = true;
            this.lblWeeklyExpenses.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeeklyExpenses.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblWeeklyExpenses.Location = new System.Drawing.Point(876, 15);
            this.lblWeeklyExpenses.Name = "lblWeeklyExpenses";
            this.lblWeeklyExpenses.Size = new System.Drawing.Size(172, 45);
            this.lblWeeklyExpenses.TabIndex = 3;
            this.lblWeeklyExpenses.Text = "PKR 3,050";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(640, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(247, 38);
            this.label6.TabIndex = 2;
            this.label6.Text = "Weekly Expenses:";
            // 
            // lblWeeklyRevenue
            // 
            this.lblWeeklyRevenue.AutoSize = true;
            this.lblWeeklyRevenue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeeklyRevenue.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblWeeklyRevenue.Location = new System.Drawing.Point(280, 15);
            this.lblWeeklyRevenue.Name = "lblWeeklyRevenue";
            this.lblWeeklyRevenue.Size = new System.Drawing.Size(190, 45);
            this.lblWeeklyRevenue.TabIndex = 1;
            this.lblWeeklyRevenue.Text = "PKR 11,800";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(20, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(240, 38);
            this.label5.TabIndex = 0;
            this.label5.Text = "Weekly Revenue:";
            // 
            // dgvWeeklyData
            // 
            this.dgvWeeklyData.AllowUserToAddRows = false;
            this.dgvWeeklyData.AllowUserToDeleteRows = false;
            this.dgvWeeklyData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvWeeklyData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvWeeklyData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWeeklyData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvWeeklyData.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvWeeklyData.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvWeeklyData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.dgvWeeklyData.Location = new System.Drawing.Point(30, 240);
            this.dgvWeeklyData.MultiSelect = false;
            this.dgvWeeklyData.Name = "dgvWeeklyData";
            this.dgvWeeklyData.ReadOnly = true;
            this.dgvWeeklyData.RowHeadersVisible = false;
            this.dgvWeeklyData.RowHeadersWidth = 62;
            this.dgvWeeklyData.RowTemplate.Height = 35;
            this.dgvWeeklyData.Size = new System.Drawing.Size(1140, 370);
            this.dgvWeeklyData.TabIndex = 4;
            this.dgvWeeklyData.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.dgvWeeklyData.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvWeeklyData.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this.dgvWeeklyData.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvWeeklyData.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvWeeklyData.ThemeStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.dgvWeeklyData.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.dgvWeeklyData.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvWeeklyData.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvWeeklyData.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dgvWeeklyData.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvWeeklyData.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvWeeklyData.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvWeeklyData.ThemeStyle.ReadOnly = true;
            this.dgvWeeklyData.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.dgvWeeklyData.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvWeeklyData.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvWeeklyData.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.dgvWeeklyData.ThemeStyle.RowsStyle.Height = 35;
            this.dgvWeeklyData.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvWeeklyData.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            // 
            // guna2ShadowPanel4
            // 
            this.guna2ShadowPanel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel4.Controls.Add(this.guna2Button1);
            this.guna2ShadowPanel4.Controls.Add(this.cmbWeek);
            this.guna2ShadowPanel4.Controls.Add(this.lblWeeklyProfit);
            this.guna2ShadowPanel4.Controls.Add(this.label3);
            this.guna2ShadowPanel4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.guna2ShadowPanel4.Location = new System.Drawing.Point(30, 120);
            this.guna2ShadowPanel4.Name = "guna2ShadowPanel4";
            this.guna2ShadowPanel4.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel4.ShadowDepth = 150;
            this.guna2ShadowPanel4.Size = new System.Drawing.Size(1140, 100);
            this.guna2ShadowPanel4.TabIndex = 3;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Animated = true;
            this.guna2Button1.AutoRoundedCorners = true;
            this.guna2Button1.BorderRadius = 16;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(974, 30);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(140, 35);
            this.guna2Button1.TabIndex = 4;
            this.guna2Button1.Text = "⬅️ Back";
            // 
            // cmbWeek
            // 
            this.cmbWeek.BackColor = System.Drawing.Color.Transparent;
            this.cmbWeek.BorderRadius = 17;
            this.cmbWeek.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeek.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWeek.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWeek.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbWeek.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbWeek.ItemHeight = 30;
            this.cmbWeek.Items.AddRange(new object[] {
            "Week 1 (1-7)",
            "Week 2 (8-14)",
            "Week 3 (15-21)",
            "Week 4 (22-28)",
            "Week 5 (29-31)"});
            this.cmbWeek.Location = new System.Drawing.Point(20, 30);
            this.cmbWeek.Name = "cmbWeek";
            this.cmbWeek.Size = new System.Drawing.Size(200, 36);
            this.cmbWeek.TabIndex = 2;
            // 
            // lblWeeklyProfit
            // 
            this.lblWeeklyProfit.AutoSize = true;
            this.lblWeeklyProfit.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblWeeklyProfit.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblWeeklyProfit.Location = new System.Drawing.Point(513, 17);
            this.lblWeeklyProfit.Name = "lblWeeklyProfit";
            this.lblWeeklyProfit.Size = new System.Drawing.Size(257, 65);
            this.lblWeeklyProfit.TabIndex = 1;
            this.lblWeeklyProfit.Text = "PKR 8,750";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(250, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(257, 48);
            this.label3.TabIndex = 0;
            this.label3.Text = "Weekly Profit:";
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
            this.guna2ShadowPanel2.Size = new System.Drawing.Size(1140, 84);
            this.guna2ShadowPanel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "📅 WEEKLY PROFIT";
            // 
            // WeeklyProfitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 743);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WeeklyProfitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weekly Profit";
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2ShadowPanel5.ResumeLayout(false);
            this.guna2ShadowPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeeklyData)).EndInit();
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
        private System.Windows.Forms.Label lblWeeklyExpenses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblWeeklyRevenue;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2DataGridView dgvWeeklyData;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel4;
        private System.Windows.Forms.Label lblWeeklyProfit;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox cmbWeek;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}