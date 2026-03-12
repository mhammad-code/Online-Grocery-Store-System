using System;
using System.Data;
using System.Windows.Forms;

namespace MY_DB_PROJECT
{
    partial class ViewAllStaff
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewAllStaff));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2ShadowPanel5 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.dgvAllStaff = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel4 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblActiveStaff = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblInactiveStaff = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTotalStaff = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOnLeave = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel3 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.cmbDepartment = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2ShadowPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllStaff)).BeginInit();
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
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel4);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel3);
            this.guna2ShadowPanel1.Controls.Add(this.guna2ShadowPanel2);
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 150;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(1200, 700);
            this.guna2ShadowPanel1.TabIndex = 0;
            // 
            // guna2ShadowPanel5
            // 
            this.guna2ShadowPanel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel5.Controls.Add(this.dgvAllStaff);
            this.guna2ShadowPanel5.Controls.Add(this.label7);
            this.guna2ShadowPanel5.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel5.Location = new System.Drawing.Point(450, 190);
            this.guna2ShadowPanel5.Name = "guna2ShadowPanel5";
            this.guna2ShadowPanel5.ShadowColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.guna2ShadowPanel5.ShadowDepth = 150;
            this.guna2ShadowPanel5.Size = new System.Drawing.Size(730, 480);
            this.guna2ShadowPanel5.TabIndex = 4;
            // 
            // dgvAllStaff
            // 
            this.dgvAllStaff.AllowUserToAddRows = false;
            this.dgvAllStaff.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvAllStaff.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAllStaff.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAllStaff.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAllStaff.ColumnHeadersHeight = 50;
            this.dgvAllStaff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAllStaff.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvAllStaff.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAllStaff.Location = new System.Drawing.Point(20, 70);
            this.dgvAllStaff.Name = "dgvAllStaff";
            this.dgvAllStaff.ReadOnly = true;
            this.dgvAllStaff.RowHeadersVisible = false;
            this.dgvAllStaff.RowHeadersWidth = 62;
            this.dgvAllStaff.RowTemplate.Height = 35;
            this.dgvAllStaff.Size = new System.Drawing.Size(690, 390);
            this.dgvAllStaff.TabIndex = 1;
            this.dgvAllStaff.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvAllStaff.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvAllStaff.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvAllStaff.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvAllStaff.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvAllStaff.ThemeStyle.BackColor = System.Drawing.Color.Black;
            this.dgvAllStaff.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAllStaff.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.dgvAllStaff.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvAllStaff.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvAllStaff.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvAllStaff.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvAllStaff.ThemeStyle.HeaderStyle.Height = 50;
            this.dgvAllStaff.ThemeStyle.ReadOnly = true;
            this.dgvAllStaff.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvAllStaff.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAllStaff.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvAllStaff.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvAllStaff.ThemeStyle.RowsStyle.Height = 35;
            this.dgvAllStaff.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAllStaff.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(20, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(302, 38);
            this.label7.TabIndex = 0;
            this.label7.Text = "📋 All Staff Members";
            // 
            // guna2ShadowPanel4
            // 
            this.guna2ShadowPanel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel4.Controls.Add(this.lblActiveStaff);
            this.guna2ShadowPanel4.Controls.Add(this.label6);
            this.guna2ShadowPanel4.Controls.Add(this.lblInactiveStaff);
            this.guna2ShadowPanel4.Controls.Add(this.label4);
            this.guna2ShadowPanel4.Controls.Add(this.lblTotalStaff);
            this.guna2ShadowPanel4.Controls.Add(this.label3);
            this.guna2ShadowPanel4.Controls.Add(this.lblOnLeave);
            this.guna2ShadowPanel4.Controls.Add(this.label2);
            this.guna2ShadowPanel4.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel4.Location = new System.Drawing.Point(450, 20);
            this.guna2ShadowPanel4.Name = "guna2ShadowPanel4";
            this.guna2ShadowPanel4.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel4.ShadowDepth = 200;
            this.guna2ShadowPanel4.Size = new System.Drawing.Size(730, 150);
            this.guna2ShadowPanel4.TabIndex = 3;
            // 
            // lblActiveStaff
            // 
            this.lblActiveStaff.AutoSize = true;
            this.lblActiveStaff.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.lblActiveStaff.Location = new System.Drawing.Point(70, 70);
            this.lblActiveStaff.Name = "lblActiveStaff";
            this.lblActiveStaff.Size = new System.Drawing.Size(84, 65);
            this.lblActiveStaff.TabIndex = 1;
            this.lblActiveStaff.Text = "42";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.label6.Location = new System.Drawing.Point(70, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 28);
            this.label6.TabIndex = 0;
            this.label6.Text = "Active Staff";
            // 
            // lblInactiveStaff
            // 
            this.lblInactiveStaff.AutoSize = true;
            this.lblInactiveStaff.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInactiveStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.lblInactiveStaff.Location = new System.Drawing.Point(220, 70);
            this.lblInactiveStaff.Name = "lblInactiveStaff";
            this.lblInactiveStaff.Size = new System.Drawing.Size(56, 65);
            this.lblInactiveStaff.TabIndex = 3;
            this.lblInactiveStaff.Text = "3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.label4.Location = new System.Drawing.Point(220, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 28);
            this.label4.TabIndex = 2;
            this.label4.Text = "Inactive Staff";
            // 
            // lblTotalStaff
            // 
            this.lblTotalStaff.AutoSize = true;
            this.lblTotalStaff.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.lblTotalStaff.Location = new System.Drawing.Point(370, 70);
            this.lblTotalStaff.Name = "lblTotalStaff";
            this.lblTotalStaff.Size = new System.Drawing.Size(84, 65);
            this.lblTotalStaff.TabIndex = 5;
            this.lblTotalStaff.Text = "45";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.label3.Location = new System.Drawing.Point(370, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 28);
            this.label3.TabIndex = 4;
            this.label3.Text = "Total Staff";
            // 
            // lblOnLeave
            // 
            this.lblOnLeave.AutoSize = true;
            this.lblOnLeave.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnLeave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.lblOnLeave.Location = new System.Drawing.Point(520, 70);
            this.lblOnLeave.Name = "lblOnLeave";
            this.lblOnLeave.Size = new System.Drawing.Size(56, 65);
            this.lblOnLeave.TabIndex = 7;
            this.lblOnLeave.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.label2.Location = new System.Drawing.Point(520, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 28);
            this.label2.TabIndex = 6;
            this.label2.Text = "On Leave";
            // 
            // guna2ShadowPanel3
            // 
            this.guna2ShadowPanel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel3.Controls.Add(this.label9);
            this.guna2ShadowPanel3.Controls.Add(this.label8);
            this.guna2ShadowPanel3.Controls.Add(this.btnSearch);
            this.guna2ShadowPanel3.Controls.Add(this.cmbDepartment);
            this.guna2ShadowPanel3.Controls.Add(this.cmbStatus);
            this.guna2ShadowPanel3.Controls.Add(this.btnRefresh);
            this.guna2ShadowPanel3.Controls.Add(this.btnBack);
            this.guna2ShadowPanel3.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel3.Location = new System.Drawing.Point(30, 120);
            this.guna2ShadowPanel3.Name = "guna2ShadowPanel3";
            this.guna2ShadowPanel3.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel3.ShadowDepth = 80;
            this.guna2ShadowPanel3.Size = new System.Drawing.Size(400, 550);
            this.guna2ShadowPanel3.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(30, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 30);
            this.label9.TabIndex = 7;
            this.label9.Text = "Status:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(30, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 30);
            this.label8.TabIndex = 6;
            this.label8.Text = "Department:";
            // 
            // btnSearch
            // 
            this.btnSearch.Animated = true;
            this.btnSearch.AutoRoundedCorners = true;
            this.btnSearch.BorderRadius = 24;
            this.btnSearch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(30, 268);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(307, 50);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "🔍 Search";
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.BackColor = System.Drawing.Color.Transparent;
            this.cmbDepartment.BorderRadius = 17;
            this.cmbDepartment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDepartment.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDepartment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbDepartment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDepartment.ItemHeight = 30;
            this.cmbDepartment.Items.AddRange(new object[] {
            "All Departments",
            "Administration",
            "Sales",
            "Marketing",
            "IT",
            "Finance",
            "HR",
            "Operations"});
            this.cmbDepartment.Location = new System.Drawing.Point(30, 70);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(307, 36);
            this.cmbDepartment.TabIndex = 4;
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatus.BorderRadius = 17;
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbStatus.ItemHeight = 30;
            this.cmbStatus.Items.AddRange(new object[] {
            "All Status",
            "Active",
            "Inactive",
            "On Leave"});
            this.cmbStatus.Location = new System.Drawing.Point(30, 170);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(307, 36);
            this.cmbStatus.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Animated = true;
            this.btnRefresh.AutoRoundedCorners = true;
            this.btnRefresh.BorderRadius = 24;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(30, 347);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(307, 50);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "🔄 Refresh All";
            // 
            // btnBack
            // 
            this.btnBack.Animated = true;
            this.btnBack.AutoRoundedCorners = true;
            this.btnBack.BorderRadius = 24;
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(30, 450);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(307, 50);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "⬅️ Back";
            // 
            // guna2ShadowPanel2
            // 
            this.guna2ShadowPanel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.guna2ShadowPanel2.Controls.Add(this.label1);
            this.guna2ShadowPanel2.FillColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.Location = new System.Drawing.Point(30, 20);
            this.guna2ShadowPanel2.Name = "guna2ShadowPanel2";
            this.guna2ShadowPanel2.ShadowColor = System.Drawing.Color.White;
            this.guna2ShadowPanel2.ShadowDepth = 200;
            this.guna2ShadowPanel2.Size = new System.Drawing.Size(400, 80);
            this.guna2ShadowPanel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "👁️ View All Staff";
            // 
            // ViewAllStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.Name = "ViewAllStaff";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View All Staff";
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2ShadowPanel5.ResumeLayout(false);
            this.guna2ShadowPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllStaff)).EndInit();
            this.guna2ShadowPanel4.ResumeLayout(false);
            this.guna2ShadowPanel4.PerformLayout();
            this.guna2ShadowPanel3.ResumeLayout(false);
            this.guna2ShadowPanel3.PerformLayout();
            this.guna2ShadowPanel2.ResumeLayout(false);
            this.guna2ShadowPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel3;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDepartment;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel4;
        private System.Windows.Forms.Label lblActiveStaff;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblInactiveStaff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotalStaff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOnLeave;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel5;
        private Guna.UI2.WinForms.Guna2DataGridView dgvAllStaff;
        private System.Windows.Forms.Label label7;
    }
}