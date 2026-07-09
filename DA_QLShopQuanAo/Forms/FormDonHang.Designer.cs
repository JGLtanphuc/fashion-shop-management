namespace DA_QLShopQuanAo.Forms
{
    partial class FormDonHang
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
            this.txtMaDH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboNhanVien = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvSanPham = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.btnThemSP = new System.Windows.Forms.Button();
            this.dgvChiTietDonHang = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTaoDon = new System.Windows.Forms.Button();
            this.btnXuatHoaDon = new System.Windows.Forms.Button();
            this.cboKhachHang = new System.Windows.Forms.ComboBox();
            this.cboSDT = new System.Windows.Forms.ComboBox();
            this.btnXoaSP = new System.Windows.Forms.Button();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.cboMaDH = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietDonHang)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMaDH
            // 
            this.txtMaDH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaDH.Location = new System.Drawing.Point(16, 47);
            this.txtMaDH.Name = "txtMaDH";
            this.txtMaDH.Size = new System.Drawing.Size(112, 22);
            this.txtMaDH.TabIndex = 25;
            this.txtMaDH.TextChanged += new System.EventHandler(this.txtMaDH_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(408, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 23;
            this.label3.Text = "Số điện thoại";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(207, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "Tên khách hàng";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Mã đơn hàng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 20);
            this.label4.TabIndex = 28;
            this.label4.Text = "Địa chỉ";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiaChi.Location = new System.Drawing.Point(16, 120);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(334, 22);
            this.txtDiaChi.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(408, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 20);
            this.label5.TabIndex = 30;
            this.label5.Text = "Nhân viên xử lý";
            // 
            // cboNhanVien
            // 
            this.cboNhanVien.FormattingEnabled = true;
            this.cboNhanVien.Location = new System.Drawing.Point(412, 118);
            this.cboNhanVien.Name = "cboNhanVien";
            this.cboNhanVien.Size = new System.Drawing.Size(133, 24);
            this.cboNhanVien.TabIndex = 31;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 20);
            this.label6.TabIndex = 32;
            this.label6.Text = "Thời gian";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1202, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 20);
            this.label7.TabIndex = 34;
            this.label7.Text = "Chọn sản phẩm";
            // 
            // dgvSanPham
            // 
            this.dgvSanPham.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvSanPham.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSanPham.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPham.Location = new System.Drawing.Point(1206, 47);
            this.dgvSanPham.Name = "dgvSanPham";
            this.dgvSanPham.RowHeadersWidth = 51;
            this.dgvSanPham.RowTemplate.Height = 24;
            this.dgvSanPham.Size = new System.Drawing.Size(509, 178);
            this.dgvSanPham.TabIndex = 35;
            this.dgvSanPham.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSanPham_CellClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1200, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 20);
            this.label8.TabIndex = 36;
            this.label8.Text = "Nhập số lượng: ";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSoLuong.Location = new System.Drawing.Point(1360, 246);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(41, 22);
            this.txtSoLuong.TabIndex = 37;
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoLuong_KeyPress);
            // 
            // btnThemSP
            // 
            this.btnThemSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemSP.Location = new System.Drawing.Point(1505, 234);
            this.btnThemSP.Name = "btnThemSP";
            this.btnThemSP.Size = new System.Drawing.Size(210, 48);
            this.btnThemSP.TabIndex = 38;
            this.btnThemSP.Text = "Thêm sản phẩm";
            this.btnThemSP.UseVisualStyleBackColor = true;
            this.btnThemSP.Click += new System.EventHandler(this.btnThemSP_Click);
            // 
            // dgvChiTietDonHang
            // 
            this.dgvChiTietDonHang.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvChiTietDonHang.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvChiTietDonHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTietDonHang.Location = new System.Drawing.Point(16, 343);
            this.dgvChiTietDonHang.Name = "dgvChiTietDonHang";
            this.dgvChiTietDonHang.RowHeadersWidth = 51;
            this.dgvChiTietDonHang.RowTemplate.Height = 24;
            this.dgvChiTietDonHang.Size = new System.Drawing.Size(1699, 426);
            this.dgvChiTietDonHang.TabIndex = 39;
            this.dgvChiTietDonHang.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTietDonHang_CellClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(258, 20);
            this.label9.TabIndex = 40;
            this.label9.Text = "Danh sách sản phẩm đã chọn";
            // 
            // btnTaoDon
            // 
            this.btnTaoDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoDon.Location = new System.Drawing.Point(17, 816);
            this.btnTaoDon.Name = "btnTaoDon";
            this.btnTaoDon.Size = new System.Drawing.Size(171, 36);
            this.btnTaoDon.TabIndex = 41;
            this.btnTaoDon.Text = "Tạo đơn hàng";
            this.btnTaoDon.UseVisualStyleBackColor = true;
            this.btnTaoDon.Click += new System.EventHandler(this.btnTaoDon_Click);
            // 
            // btnXuatHoaDon
            // 
            this.btnXuatHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatHoaDon.Location = new System.Drawing.Point(211, 816);
            this.btnXuatHoaDon.Name = "btnXuatHoaDon";
            this.btnXuatHoaDon.Size = new System.Drawing.Size(171, 36);
            this.btnXuatHoaDon.TabIndex = 42;
            this.btnXuatHoaDon.Text = "Xuất hóa đơn";
            this.btnXuatHoaDon.UseVisualStyleBackColor = true;
            this.btnXuatHoaDon.Click += new System.EventHandler(this.btnXuatHoaDon_Click);
            // 
            // cboKhachHang
            // 
            this.cboKhachHang.FormattingEnabled = true;
            this.cboKhachHang.Location = new System.Drawing.Point(211, 47);
            this.cboKhachHang.Name = "cboKhachHang";
            this.cboKhachHang.Size = new System.Drawing.Size(133, 24);
            this.cboKhachHang.TabIndex = 43;
            this.cboKhachHang.SelectedIndexChanged += new System.EventHandler(this.cboKhachHang_SelectedIndexChanged);
            // 
            // cboSDT
            // 
            this.cboSDT.FormattingEnabled = true;
            this.cboSDT.Location = new System.Drawing.Point(412, 46);
            this.cboSDT.Name = "cboSDT";
            this.cboSDT.Size = new System.Drawing.Size(133, 24);
            this.cboSDT.TabIndex = 44;
            this.cboSDT.SelectedIndexChanged += new System.EventHandler(this.cboSDT_SelectedIndexChanged);
            // 
            // btnXoaSP
            // 
            this.btnXoaSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaSP.Location = new System.Drawing.Point(1505, 816);
            this.btnXoaSP.Name = "btnXoaSP";
            this.btnXoaSP.Size = new System.Drawing.Size(210, 36);
            this.btnXoaSP.TabIndex = 45;
            this.btnXoaSP.Text = "Xóa sản phẩm chọn";
            this.btnXoaSP.UseVisualStyleBackColor = true;
            this.btnXoaSP.Click += new System.EventHandler(this.btnXoaSP_Click);
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.Location = new System.Drawing.Point(17, 184);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(333, 22);
            this.dtpNgayLap.TabIndex = 46;
            // 
            // cboMaDH
            // 
            this.cboMaDH.FormattingEnabled = true;
            this.cboMaDH.Location = new System.Drawing.Point(388, 824);
            this.cboMaDH.Name = "cboMaDH";
            this.cboMaDH.Size = new System.Drawing.Size(121, 24);
            this.cboMaDH.TabIndex = 47;
            // 
            // FormDonHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1748, 873);
            this.Controls.Add(this.cboMaDH);
            this.Controls.Add(this.dtpNgayLap);
            this.Controls.Add(this.btnXoaSP);
            this.Controls.Add(this.cboSDT);
            this.Controls.Add(this.cboKhachHang);
            this.Controls.Add(this.btnXuatHoaDon);
            this.Controls.Add(this.btnTaoDon);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dgvChiTietDonHang);
            this.Controls.Add(this.btnThemSP);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgvSanPham);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboNhanVien);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDiaChi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMaDH);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormDonHang";
            this.Text = "FormDonHang";
            this.Load += new System.EventHandler(this.FormDonHang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietDonHang)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtMaDH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvSanPham;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Button btnThemSP;
        private System.Windows.Forms.DataGridView dgvChiTietDonHang;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnTaoDon;
        private System.Windows.Forms.Button btnXuatHoaDon;
        private System.Windows.Forms.ComboBox cboKhachHang;
        private System.Windows.Forms.ComboBox cboSDT;
        private System.Windows.Forms.Button btnXoaSP;
        private System.Windows.Forms.DateTimePicker dtpNgayLap;
        private System.Windows.Forms.ComboBox cboMaDH;
    }
}