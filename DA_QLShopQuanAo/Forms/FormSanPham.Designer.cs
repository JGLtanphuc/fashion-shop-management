namespace DA_QLShopQuanAo.Forms
{
    partial class FormSanPham
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSanPham));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaSP = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtThuongHieu = new System.Windows.Forms.TextBox();
            this.txtMauSac = new System.Windows.Forms.TextBox();
            this.txtChatLieu = new System.Windows.Forms.TextBox();
            this.txtTenSP = new System.Windows.Forms.TextBox();
            this.cboTheLoai = new System.Windows.Forms.ComboBox();
            this.cboXuatXu = new System.Windows.Forms.ComboBox();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.dgvSanPham = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.lblTongSoSP = new System.Windows.Forms.Label();
            this.lblKetQuaTim = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mã sản phẩm";
            // 
            // txtMaSP
            // 
            this.txtMaSP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaSP.Location = new System.Drawing.Point(47, 66);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.Size = new System.Drawing.Size(117, 22);
            this.txtMaSP.TabIndex = 8;
            this.txtMaSP.TextChanged += new System.EventHandler(this.txtMaSP_TextChanged);
            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.Location = new System.Drawing.Point(1575, 31);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(149, 47);
            this.btnThem.TabIndex = 9;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.Location = new System.Drawing.Point(1575, 84);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(149, 48);
            this.btnXoa.TabIndex = 10;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.Location = new System.Drawing.Point(1575, 138);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(149, 47);
            this.btnSua.TabIndex = 11;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.Location = new System.Drawing.Point(1575, 191);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(149, 42);
            this.btnLuu.TabIndex = 12;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(248, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Thương hiệu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(413, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Màu sắc";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(574, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Chất liệu";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(43, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Tên sản phẩm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(248, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Thể loại";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(416, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 20);
            this.label7.TabIndex = 18;
            this.label7.Text = "Xuất xứ";
            // 
            // txtThuongHieu
            // 
            this.txtThuongHieu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThuongHieu.Location = new System.Drawing.Point(252, 66);
            this.txtThuongHieu.Name = "txtThuongHieu";
            this.txtThuongHieu.Size = new System.Drawing.Size(107, 22);
            this.txtThuongHieu.TabIndex = 19;
            // 
            // txtMauSac
            // 
            this.txtMauSac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMauSac.Location = new System.Drawing.Point(420, 66);
            this.txtMauSac.Name = "txtMauSac";
            this.txtMauSac.Size = new System.Drawing.Size(79, 22);
            this.txtMauSac.TabIndex = 20;
            // 
            // txtChatLieu
            // 
            this.txtChatLieu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChatLieu.Location = new System.Drawing.Point(579, 66);
            this.txtChatLieu.Name = "txtChatLieu";
            this.txtChatLieu.Size = new System.Drawing.Size(79, 22);
            this.txtChatLieu.TabIndex = 21;
            // 
            // txtTenSP
            // 
            this.txtTenSP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTenSP.Location = new System.Drawing.Point(47, 160);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.Size = new System.Drawing.Size(117, 22);
            this.txtTenSP.TabIndex = 22;
            // 
            // cboTheLoai
            // 
            this.cboTheLoai.FormattingEnabled = true;
            this.cboTheLoai.Location = new System.Drawing.Point(252, 159);
            this.cboTheLoai.Name = "cboTheLoai";
            this.cboTheLoai.Size = new System.Drawing.Size(121, 24);
            this.cboTheLoai.TabIndex = 23;
            // 
            // cboXuatXu
            // 
            this.cboXuatXu.FormattingEnabled = true;
            this.cboXuatXu.Location = new System.Drawing.Point(417, 159);
            this.cboXuatXu.Name = "cboXuatXu";
            this.cboXuatXu.Size = new System.Drawing.Size(121, 24);
            this.cboXuatXu.TabIndex = 24;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTimKiem.Location = new System.Drawing.Point(47, 250);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(326, 22);
            this.txtTimKiem.TabIndex = 25;
            this.txtTimKiem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimKiem_KeyPress);
            // 
            // btnTim
            // 
            this.btnTim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTim.Location = new System.Drawing.Point(395, 239);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(104, 42);
            this.btnTim.TabIndex = 26;
            this.btnTim.Text = "Tìm";
            this.btnTim.UseVisualStyleBackColor = true;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // dgvSanPham
            // 
            this.dgvSanPham.AllowUserToAddRows = false;
            this.dgvSanPham.AllowUserToDeleteRows = false;
            this.dgvSanPham.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPham.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvSanPham.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSanPham.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPham.Location = new System.Drawing.Point(47, 345);
            this.dgvSanPham.Name = "dgvSanPham";
            this.dgvSanPham.ReadOnly = true;
            this.dgvSanPham.RowHeadersWidth = 51;
            this.dgvSanPham.RowTemplate.Height = 24;
            this.dgvSanPham.Size = new System.Drawing.Size(1677, 412);
            this.dgvSanPham.TabIndex = 27;
            this.dgvSanPham.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSanPham_CellClick);
            this.dgvSanPham.Click += new System.EventHandler(this.dgvSanPham_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(43, 322);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 20);
            this.label8.TabIndex = 28;
            this.label8.Text = "Danh sách các sản phẩm";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(1529, 199);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(40, 38);
            this.pictureBox4.TabIndex = 32;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1518, 142);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(51, 47);
            this.pictureBox3.TabIndex = 31;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1518, 88);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(51, 38);
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1518, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 47);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(722, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 33;
            this.label9.Text = "Giá bán";
            // 
            // txtGiaBan
            // 
            this.txtGiaBan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGiaBan.Location = new System.Drawing.Point(726, 66);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.Size = new System.Drawing.Size(100, 22);
            this.txtGiaBan.TabIndex = 34;
            this.txtGiaBan.TextChanged += new System.EventHandler(this.txtGiaBan_TextChanged);
            this.txtGiaBan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGiaBan_KeyPress);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(1575, 239);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(149, 42);
            this.btnRefresh.TabIndex = 37;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Location = new System.Drawing.Point(1518, 239);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(51, 42);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 38;
            this.pictureBox5.TabStop = false;
            // 
            // lblTongSoSP
            // 
            this.lblTongSoSP.AutoSize = true;
            this.lblTongSoSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongSoSP.ForeColor = System.Drawing.Color.Blue;
            this.lblTongSoSP.Location = new System.Drawing.Point(321, 323);
            this.lblTongSoSP.Name = "lblTongSoSP";
            this.lblTongSoSP.Size = new System.Drawing.Size(167, 18);
            this.lblTongSoSP.TabIndex = 39;
            this.lblTongSoSP.Text = "Tổng số: 0 sản phẩm";
            // 
            // lblKetQuaTim
            // 
            this.lblKetQuaTim.AutoSize = true;
            this.lblKetQuaTim.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKetQuaTim.ForeColor = System.Drawing.Color.Green;
            this.lblKetQuaTim.Location = new System.Drawing.Point(505, 253);
            this.lblKetQuaTim.Name = "lblKetQuaTim";
            this.lblKetQuaTim.Size = new System.Drawing.Size(0, 18);
            this.lblKetQuaTim.TabIndex = 40;
            // 
            // FormSanPham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1748, 873);
            this.Controls.Add(this.lblKetQuaTim);
            this.Controls.Add(this.lblTongSoSP);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtGiaBan);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgvSanPham);
            this.Controls.Add(this.btnTim);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.cboXuatXu);
            this.Controls.Add(this.cboTheLoai);
            this.Controls.Add(this.txtTenSP);
            this.Controls.Add(this.txtChatLieu);
            this.Controls.Add(this.txtMauSac);
            this.Controls.Add(this.txtThuongHieu);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.txtMaSP);
            this.Controls.Add(this.label1);
            this.Name = "FormSanPham";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Sản Phẩm";
            this.Load += new System.EventHandler(this.FormSanPham_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaSP;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtThuongHieu;
        private System.Windows.Forms.TextBox txtMauSac;
        private System.Windows.Forms.TextBox txtChatLieu;
        private System.Windows.Forms.TextBox txtTenSP;
        private System.Windows.Forms.ComboBox cboTheLoai;
        private System.Windows.Forms.ComboBox cboXuatXu;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.DataGridView dgvSanPham;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGiaBan;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label lblTongSoSP;
        private System.Windows.Forms.Label lblKetQuaTim;
    }
}