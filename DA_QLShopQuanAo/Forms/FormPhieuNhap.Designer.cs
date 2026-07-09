namespace DA_QLShopQuanAo.Forms
{
    partial class FormPhieuNhap
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboNhanVien = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboNhaCungCap = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpNgayNhap = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMaPN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTonKho = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGiaNhap = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboSanPham = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnThemSP = new System.Windows.Forms.Button();
            this.btnXoaSP = new System.Windows.Forms.Button();
            this.btnSuaSP = new System.Windows.Forms.Button();
            this.btnLuuPhieu = new System.Windows.Forms.Button();
            this.btnHuyPhieu = new System.Windows.Forms.Button();
            this.dgvChiTietNhap = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietNhap)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGhiChu);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboNhanVien);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboNhaCungCap);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpNgayNhap);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtMaPN);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 250);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "THÔNG TIN PHIẾU NHẬP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(17, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ghi chú";
            // 
            // cboNhanVien
            // 
            this.cboNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhanVien.FormattingEnabled = true;
            this.cboNhanVien.Location = new System.Drawing.Point(280, 100);
            this.cboNhanVien.Name = "cboNhanVien";
            this.cboNhanVien.Size = new System.Drawing.Size(200, 26);
            this.cboNhanVien.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(277, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Nhân viên";
            // 
            // cboNhaCungCap
            // 
            this.cboNhaCungCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhaCungCap.FormattingEnabled = true;
            this.cboNhaCungCap.Location = new System.Drawing.Point(20, 100);
            this.cboNhaCungCap.Name = "cboNhaCungCap";
            this.cboNhaCungCap.Size = new System.Drawing.Size(200, 26);
            this.cboNhaCungCap.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(17, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "Nhà cung cấp";
            // 
            // dtpNgayNhap
            // 
            this.dtpNgayNhap.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayNhap.Location = new System.Drawing.Point(280, 45);
            this.dtpNgayNhap.Name = "dtpNgayNhap";
            this.dtpNgayNhap.Size = new System.Drawing.Size(200, 24);
            this.dtpNgayNhap.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(277, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 18);
            this.label6.TabIndex = 2;
            this.label6.Text = "Ngày nhập";
            // 
            // txtMaPN
            // 
            this.txtMaPN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMaPN.Location = new System.Drawing.Point(20, 45);
            this.txtMaPN.Name = "txtMaPN";
            this.txtMaPN.Size = new System.Drawing.Size(200, 24);
            this.txtMaPN.TabIndex = 0;
            this.txtMaPN.TextChanged += new System.EventHandler(this.txtMaPN_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã phiếu (*)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTonKho);
            this.groupBox2.Controls.Add(this.txtSoLuong);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtGiaNhap);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cboSanPham);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(540, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 120);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "THÊM SẢN PHẨM";
            // 
            // lblTonKho
            // 
            this.lblTonKho.AutoSize = true;
            this.lblTonKho.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic);
            this.lblTonKho.ForeColor = System.Drawing.Color.Gray;
            this.lblTonKho.Location = new System.Drawing.Point(137, 88);
            this.lblTonKho.Name = "lblTonKho";
            this.lblTonKho.Size = new System.Drawing.Size(49, 17);
            this.lblTonKho.TabIndex = 6;
            this.lblTonKho.Text = "Tồn: 0";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(270, 50);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(100, 24);
            this.txtSoLuong.TabIndex = 5;
            this.txtSoLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoLuong_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(267, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 18);
            this.label10.TabIndex = 4;
            this.label10.Text = "Số lượng";
            // 
            // txtGiaNhap
            // 
            this.txtGiaNhap.Location = new System.Drawing.Point(140, 50);
            this.txtGiaNhap.Name = "txtGiaNhap";
            this.txtGiaNhap.Size = new System.Drawing.Size(120, 24);
            this.txtGiaNhap.TabIndex = 3;
            this.txtGiaNhap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGiaNhap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGiaNhap_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(137, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 18);
            this.label9.TabIndex = 2;
            this.label9.Text = "Giá nhập";
            // 
            // cboSanPham
            // 
            this.cboSanPham.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSanPham.FormattingEnabled = true;
            this.cboSanPham.Location = new System.Drawing.Point(20, 50);
            this.cboSanPham.Name = "cboSanPham";
            this.cboSanPham.Size = new System.Drawing.Size(110, 26);
            this.cboSanPham.TabIndex = 1;
            this.cboSanPham.SelectedIndexChanged += new System.EventHandler(this.cboSanPham_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(17, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Sản phẩm";
            // 
            // btnThemSP
            // 
            this.btnThemSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnThemSP.Location = new System.Drawing.Point(960, 50);
            this.btnThemSP.Name = "btnThemSP";
            this.btnThemSP.Size = new System.Drawing.Size(100, 35);
            this.btnThemSP.TabIndex = 2;
            this.btnThemSP.Text = "THÊM";
            this.btnThemSP.UseVisualStyleBackColor = true;
            this.btnThemSP.Click += new System.EventHandler(this.btnThemSP_Click);
            // 
            // btnXoaSP
            // 
            this.btnXoaSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnXoaSP.Location = new System.Drawing.Point(960, 90);
            this.btnXoaSP.Name = "btnXoaSP";
            this.btnXoaSP.Size = new System.Drawing.Size(100, 35);
            this.btnXoaSP.TabIndex = 3;
            this.btnXoaSP.Text = "XÓA";
            this.btnXoaSP.UseVisualStyleBackColor = true;
            this.btnXoaSP.Click += new System.EventHandler(this.btnXoaSP_Click);
            // 
            // btnSuaSP
            // 
            this.btnSuaSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnSuaSP.Location = new System.Drawing.Point(1070, 50);
            this.btnSuaSP.Name = "btnSuaSP";
            this.btnSuaSP.Size = new System.Drawing.Size(100, 35);
            this.btnSuaSP.TabIndex = 4;
            this.btnSuaSP.Text = "SỬA";
            this.btnSuaSP.UseVisualStyleBackColor = true;
            this.btnSuaSP.Click += new System.EventHandler(this.btnSuaSP_Click);
            // 
            // btnLuuPhieu
            // 
            this.btnLuuPhieu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnLuuPhieu.Location = new System.Drawing.Point(1070, 90);
            this.btnLuuPhieu.Name = "btnLuuPhieu";
            this.btnLuuPhieu.Size = new System.Drawing.Size(100, 35);
            this.btnLuuPhieu.TabIndex = 5;
            this.btnLuuPhieu.Text = "LƯU";
            this.btnLuuPhieu.UseVisualStyleBackColor = true;
            this.btnLuuPhieu.Click += new System.EventHandler(this.btnLuuPhieu_Click);
            // 
            // btnHuyPhieu
            // 
            this.btnHuyPhieu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuyPhieu.Location = new System.Drawing.Point(1180, 90);
            this.btnHuyPhieu.Name = "btnHuyPhieu";
            this.btnHuyPhieu.Size = new System.Drawing.Size(100, 35);
            this.btnHuyPhieu.TabIndex = 6;
            this.btnHuyPhieu.Text = "HỦY";
            this.btnHuyPhieu.UseVisualStyleBackColor = true;
            this.btnHuyPhieu.Click += new System.EventHandler(this.btnHuyPhieu_Click);
            // 
            // dgvChiTietNhap
            // 
            this.dgvChiTietNhap.AllowUserToAddRows = false;
            this.dgvChiTietNhap.AllowUserToDeleteRows = false;
            this.dgvChiTietNhap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChiTietNhap.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTietNhap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTietNhap.Location = new System.Drawing.Point(40, 320);
            this.dgvChiTietNhap.Name = "dgvChiTietNhap";
            this.dgvChiTietNhap.ReadOnly = true;
            this.dgvChiTietNhap.RowHeadersWidth = 51;
            this.dgvChiTietNhap.RowTemplate.Height = 24;
            this.dgvChiTietNhap.Size = new System.Drawing.Size(1125, 266);
            this.dgvChiTietNhap.TabIndex = 7;
            this.dgvChiTietNhap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTietNhap_CellClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(36, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "DANH SÁCH SẢN PHẨM";
            // 
            // lblTongTien
            // 
            this.lblTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.Red;
            this.lblTongTien.Location = new System.Drawing.Point(120, 620);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(24, 25);
            this.lblTongTien.TabIndex = 10;
            this.lblTongTien.Text = "0";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(20, 620);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 25);
            this.label7.TabIndex = 9;
            this.label7.Text = "TỔNG: ";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(20, 171);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(460, 60);
            this.txtGhiChu.TabIndex = 4;
            // 
            // FormPhieuNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1300, 670);
            this.Controls.Add(this.lblTongTien);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvChiTietNhap);
            this.Controls.Add(this.btnHuyPhieu);
            this.Controls.Add(this.btnLuuPhieu);
            this.Controls.Add(this.btnSuaSP);
            this.Controls.Add(this.btnXoaSP);
            this.Controls.Add(this.btnThemSP);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Name = "FormPhieuNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PHIẾU NHẬP HÀNG";
            this.Load += new System.EventHandler(this.FormPhieuNhap_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietNhap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpNgayNhap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMaPN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTonKho;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGiaNhap;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboSanPham;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnThemSP;
        private System.Windows.Forms.Button btnXoaSP;
        private System.Windows.Forms.Button btnSuaSP;
        private System.Windows.Forms.Button btnLuuPhieu;
        private System.Windows.Forms.Button btnHuyPhieu;
        private System.Windows.Forms.DataGridView dgvChiTietNhap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtGhiChu;
    }
}