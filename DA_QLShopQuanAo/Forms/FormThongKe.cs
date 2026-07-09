using DA_QLShopQuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormThongKe : Form
    {
        private DataTable dtHoaDon;
        private DataTable dtChiTietHoaDon;
        private DataTable dtDoanhThu;
        private DataTable dtSanPham;
        private DataTable dtKhachHang;
        private DataTable dtLoiNhuan;

        public FormThongKe()
        {
            InitializeComponent();
        }

        private void FormThongKe_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadDuLieuBanDau();
            LoadComboBoxNam();
        }

        private void LoadTheme()
        {
            // Cài đặt màu sắc cho các nút
            btnTimKiem.BackColor = Color.FromArgb(23, 162, 184);
            btnXemChiTiet.BackColor = Color.FromArgb(40, 167, 69);
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnXuatExcel.BackColor = Color.FromArgb(52, 152, 219);

            // Tab Doanh thu
            btnLocDoanhThu.BackColor = Color.FromArgb(23, 162, 184);

            // Tab Sản phẩm
            btnLocSanPham.BackColor = Color.FromArgb(23, 162, 184);

            // Tab Khách hàng
            btnLocKhachHang.BackColor = Color.FromArgb(23, 162, 184);

            // Tab Lợi nhuận
            btnLocLoiNhuan.BackColor = Color.FromArgb(23, 162, 184);
        }

        private void LoadDuLieuBanDau()
        {
            // Tải tất cả hóa đơn mặc định
            LoadHoaDonTatCa();

            // Tải doanh thu năm hiện tại
            LoadDoanhThuNamHienTai();

            // Tải top sản phẩm 30 ngày gần nhất
            LoadTopSanPham30Ngay();

            // Tải top khách hàng
            LoadTopKhachHang();

            // Tải lợi nhuận mặc định
            LoadLoiNhuanMacDinh();
        }

        private void LoadComboBoxNam()
        {
            try
            {
                // Load combobox năm cho tab Hóa đơn
                string sqlNamHD = "EXEC sp_LayDanhSachNamThongKe";
                DataTable dtNam = DatabaseHelper.ExecuteQuery(sqlNamHD);

                // Tạo DataTable mới để bind đúng cách
                DataTable dtNamBind = new DataTable();
                dtNamBind.Columns.Add("Display", typeof(string));
                dtNamBind.Columns.Add("Value", typeof(int));

                foreach (DataRow row in dtNam.Rows)
                {
                    int nam = Convert.ToInt32(row["Nam"]);
                    dtNamBind.Rows.Add($"Năm {nam}", nam);
                }

                cboNam.DataSource = dtNamBind;
                cboNam.DisplayMember = "Display";
                cboNam.ValueMember = "Value";

                // Chọn năm hiện tại
                int namHienTai = DateTime.Now.Year;
                foreach (DataRowView drv in cboNam.Items)
                {
                    if (Convert.ToInt32(drv["Value"]) == namHienTai)
                    {
                        cboNam.SelectedValue = drv["Value"];
                        break;
                    }
                }

                // Load combobox năm cho tab Doanh thu
                cboNamDoanhThu.DataSource = dtNamBind.Copy();
                cboNamDoanhThu.DisplayMember = "Display";
                cboNamDoanhThu.ValueMember = "Value";

                // Load combobox năm cho tab Lợi nhuận
                cboNamLoiNhuan.DataSource = dtNamBind.Copy();
                cboNamLoiNhuan.DisplayMember = "Display";
                cboNamLoiNhuan.ValueMember = "Value";

                // Load combobox tháng
                LoadComboBoxThang();

                // Thiết lập giá trị mặc định cho combobox loại thống kê
                cboLoaiThongKeDoanhThu.SelectedIndex = 2; // NĂM
                cboLoaiThongKeLoiNhuan.SelectedIndex = 2; // NĂM

                // Đặt ngày mặc định cho tab Sản phẩm
                dateTuNgaySP.Value = DateTime.Now.AddDays(-30);
                dateDenNgaySP.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load combobox: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxThang()
        {
            try
            {
                // Tạo danh sách tháng 1-12
                DataTable dtThang = new DataTable();
                dtThang.Columns.Add("Value", typeof(int));
                dtThang.Columns.Add("Display", typeof(string));

                for (int i = 1; i <= 12; i++)
                {
                    dtThang.Rows.Add(i, $"Tháng {i}");
                }

                cboThangDoanhThu.DataSource = dtThang.Copy();
                cboThangDoanhThu.DisplayMember = "Display";
                cboThangDoanhThu.ValueMember = "Value";

                cboThangLoiNhuan.DataSource = dtThang.Copy();
                cboThangLoiNhuan.DisplayMember = "Display";
                cboThangLoiNhuan.ValueMember = "Value";

                // Chọn tháng hiện tại
                int thangHienTai = DateTime.Now.Month;
                cboThangDoanhThu.SelectedValue = thangHienTai;
                cboThangLoiNhuan.SelectedValue = thangHienTai;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load combobox tháng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region TAB HÓA ĐƠN
        private void LoadHoaDonTatCa()
        {
            try
            {
                string sql = @"
                    SELECT 
                        dh.MaDH,
                        dh.NgayLap,
                        kh.HoTen AS TenKhachHang,
                        nv.HoTen AS TenNhanVien,
                        dh.TongTien,
                        dh.DiaChiGiaoHang
                    FROM DonHang dh
                    LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                    LEFT JOIN NhanVien nv ON dh.MaNV = nv.MaNV
                    ORDER BY dh.NgayLap DESC";

                dtHoaDon = DatabaseHelper.ExecuteQuery(sql);
                dgvHoaDon.DataSource = dtHoaDon;

                // Định dạng DataGridView
                FormatDataGridViewHoaDon();

                // Tính tổng
                TinhTongHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHoaDonTheoNam(int nam)
        {
            try
            {
                string sql = @"
                    SELECT 
                        dh.MaDH,
                        dh.NgayLap,
                        kh.HoTen AS TenKhachHang,
                        nv.HoTen AS TenNhanVien,
                        dh.TongTien,
                        dh.DiaChiGiaoHang
                    FROM DonHang dh
                    LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                    LEFT JOIN NhanVien nv ON dh.MaNV = nv.MaNV
                    WHERE YEAR(dh.NgayLap) = @Nam
                    ORDER BY dh.NgayLap DESC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Nam", nam)
                };

                dtHoaDon = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvHoaDon.DataSource = dtHoaDon;
                FormatDataGridViewHoaDon();
                TinhTongHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load hóa đơn theo năm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridViewHoaDon()
        {
            if (dgvHoaDon.Columns.Count > 0)
            {
                dgvHoaDon.Columns["MaDH"].HeaderText = "Mã HĐ";
                dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
                dgvHoaDon.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
                dgvHoaDon.Columns["TenNhanVien"].HeaderText = "Nhân Viên";
                dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";
                dgvHoaDon.Columns["DiaChiGiaoHang"].HeaderText = "Địa Chỉ Giao";

                dgvHoaDon.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void TinhTongHoaDon()
        {
            if (dtHoaDon != null && dtHoaDon.Rows.Count > 0)
            {
                int soHoaDon = dtHoaDon.Rows.Count;
                decimal tongTien = dtHoaDon.AsEnumerable()
                    .Sum(row => Convert.ToDecimal(row["TongTien"]));

                lblSoHoaDon.Text = $"Số hóa đơn: {soHoaDon:N0}";
                txtTongDoanhThu.Text = $"{tongTien:N0} đ";
            }
            else
            {
                lblSoHoaDon.Text = "Số hóa đơn: 0";
                txtTongDoanhThu.Text = "0 đ";
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maDH = txtMaDH.Text.Trim();
            if (string.IsNullOrEmpty(maDH))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"
                    SELECT 
                        dh.MaDH,
                        dh.NgayLap,
                        kh.HoTen AS TenKhachHang,
                        nv.HoTen AS TenNhanVien,
                        dh.TongTien,
                        dh.DiaChiGiaoHang
                    FROM DonHang dh
                    LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                    LEFT JOIN NhanVien nv ON dh.MaNV = nv.MaNV
                    WHERE dh.MaDH LIKE @MaDH
                    ORDER BY dh.NgayLap DESC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaDH", "%" + maDH + "%")
                };

                dtHoaDon = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvHoaDon.DataSource = dtHoaDon;
                FormatDataGridViewHoaDon();
                TinhTongHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMaDH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
            }
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null)
            {
                string maDH = dgvHoaDon.CurrentRow.Cells["MaDH"].Value.ToString();
                LoadChiTietHoaDon(maDH);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maDH = dgvHoaDon.Rows[e.RowIndex].Cells["MaDH"].Value.ToString();
                LoadChiTietHoaDon(maDH);
            }
        }

        private void LoadChiTietHoaDon(string maDH)
        {
            try
            {
                string sql = @"
                    SELECT 
                        ct.MaSP,
                        sp.TenSP,
                        ct.SoLuong,
                        ct.DonGia,
                        ct.ThanhTien
                    FROM ChiTietDonHang ct
                    JOIN SanPham sp ON ct.MaSP = sp.MaSP
                    WHERE ct.MaDH = @MaDH";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaDH", maDH)
                };

                dtChiTietHoaDon = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvChiTiet.DataSource = dtChiTietHoaDon;

                // Định dạng DataGridView chi tiết
                FormatDataGridViewChiTiet();

                // Hiển thị thông tin tổng
                decimal tongTien = dtChiTietHoaDon.AsEnumerable()
                    .Sum(row => Convert.ToDecimal(row["ThanhTien"]));

                lblMaHDChiTiet.Text = $"Mã HĐ: {maDH}";
                lblTongTienChiTiet.Text = $"Tổng tiền: {tongTien:N0} đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chi tiết hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridViewChiTiet()
        {
            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["MaSP"].HeaderText = "Mã SP";
                dgvChiTiet.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";

                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMaDH.Clear();
            LoadHoaDonTatCa();
            dgvChiTiet.DataSource = null;
            lblMaHDChiTiet.Text = "Mã HĐ:";
            lblTongTienChiTiet.Text = "Tổng tiền: 0 đ";
        }

        private void cboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNam.SelectedItem != null)
            {
                try
                {
                    // Cách đúng để lấy giá trị từ combobox đã bind
                    if (cboNam.SelectedItem is DataRowView drv)
                    {
                        object value = drv["Value"];
                        if (value != null && value != DBNull.Value)
                        {
                            int nam = Convert.ToInt32(value);
                            LoadHoaDonTheoNam(nam);
                        }
                    }
                    else if (cboNam.SelectedValue != null)
                    {
                        // Nếu SelectedValue trực tiếp là số
                        int nam = Convert.ToInt32(cboNam.SelectedValue);
                        LoadHoaDonTheoNam(nam);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi chuyển đổi giá trị: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == tabPage1)
                {
                    XuatExcelHoaDon();
                }
                else if (tabControl1.SelectedTab == tabPage2)
                {
                    XuatExcelDoanhThu();
                }
                else if (tabControl1.SelectedTab == tabPage3)
                {
                    XuatExcelSanPham();
                }
                else if (tabControl1.SelectedTab == tabPage4)
                {
                    XuatExcelKhachHang();
                }
                else if (tabControl1.SelectedTab == tabPage5)
                {
                    XuatExcelLoiNhuan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region TAB DOANH THU
        private void LoadDoanhThuNamHienTai()
        {
            try
            {
                int namHienTai = DateTime.Now.Year;
                string sql = "EXEC sp_ThongKeDoanhThuNam @Nam";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Nam", namHienTai)
                };

                dtDoanhThu = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvDoanhThu.DataSource = dtDoanhThu;

                // Định dạng DataGridView
                FormatDataGridViewDoanhThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load doanh thu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDoanhThuTheoLoai(string loaiThongKe, int? nam = null, int? thang = null, int? ngay = null)
        {
            try
            {
                string sql = "";
                SqlParameter[] parameters = null;

                switch (loaiThongKe)
                {
                    case "NGÀY":
                        sql = "EXEC sp_ThongKeDoanhThuNgay @Ngay";
                        string ngayStr = $"{nam}-{thang:D2}-{ngay:D2}";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@Ngay", ngayStr)
                        };
                        break;

                    case "THÁNG":
                        sql = "EXEC sp_ThongKeDoanhThuThang @Thang, @Nam";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@Thang", thang),
                            new SqlParameter("@Nam", nam)
                        };
                        break;

                    case "NĂM":
                        sql = "EXEC sp_ThongKeDoanhThuNam @Nam";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@Nam", nam)
                        };
                        break;
                }

                dtDoanhThu = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvDoanhThu.DataSource = dtDoanhThu;
                FormatDataGridViewDoanhThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load doanh thu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridViewDoanhThu()
        {
            if (dgvDoanhThu.Columns.Count > 0)
            {
                // Chỉ định dạng cột nếu nó tồn tại
                foreach (DataGridViewColumn column in dgvDoanhThu.Columns)
                {
                    switch (column.Name)
                    {
                        case "Thang":
                            column.HeaderText = "Tháng";
                            break;
                        case "Ngay":
                            column.HeaderText = "Ngày";
                            break;
                        case "Nam":
                            column.HeaderText = "Năm";
                            break;
                        case "SoDonHang":
                            column.HeaderText = "Số Đơn Hàng";
                            break;
                        case "DoanhThu":
                            column.HeaderText = "Doanh Thu";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "TongDoanhThu":
                            column.HeaderText = "Tổng Doanh Thu";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "DonHangTrungBinh":
                            column.HeaderText = "Đơn Hàng TB";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "DoanhThuTrungBinh":
                            column.HeaderText = "Doanh Thu TB";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                }

                dgvDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void cboLoaiThongKeDoanhThu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string loaiThongKe = cboLoaiThongKeDoanhThu.Text;

            // Enable/disable combobox theo loại thống kê
            switch (loaiThongKe)
            {
                case "NGÀY":
                    cboNamDoanhThu.Enabled = true;
                    cboThangDoanhThu.Enabled = true;
                    cboNgayDoanhThu.Enabled = true;
                    LoadComboBoxNgayDoanhThu();
                    break;
                case "THÁNG":
                    cboNamDoanhThu.Enabled = true;
                    cboThangDoanhThu.Enabled = true;
                    cboNgayDoanhThu.Enabled = false;
                    break;
                case "NĂM":
                    cboNamDoanhThu.Enabled = true;
                    cboThangDoanhThu.Enabled = false;
                    cboNgayDoanhThu.Enabled = false;
                    break;
            }
        }

        private void cboNamDoanhThu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiThongKeDoanhThu.Text == "NGÀY")
            {
                LoadComboBoxNgayDoanhThu();
            }
        }

        private void cboThangDoanhThu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiThongKeDoanhThu.Text == "NGÀY")
            {
                LoadComboBoxNgayDoanhThu();
            }
        }

        private void LoadComboBoxNgayDoanhThu()
        {
            try
            {
                if (cboNamDoanhThu.SelectedValue == null || cboThangDoanhThu.SelectedValue == null)
                    return;

                int nam = Convert.ToInt32(cboNamDoanhThu.SelectedValue);
                int thang = Convert.ToInt32(cboThangDoanhThu.SelectedValue);

                // Lấy danh sách ngày có dữ liệu
                string sql = "EXEC sp_LayDanhSachNgayTheoThang @Thang, @Nam";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Thang", thang),
                    new SqlParameter("@Nam", nam)
                };

                DataTable dtNgay = DatabaseHelper.ExecuteQuery(sql, parameters);

                // Tạo DataTable mới với cột Ngày
                DataTable dtNgayNew = new DataTable();
                dtNgayNew.Columns.Add("Value", typeof(int));
                dtNgayNew.Columns.Add("Display", typeof(string));

                foreach (DataRow row in dtNgay.Rows)
                {
                    int ngay = Convert.ToInt32(row["Ngay"]);
                    dtNgayNew.Rows.Add(ngay, $"Ngày {ngay}");
                }

                cboNgayDoanhThu.DataSource = dtNgayNew;
                cboNgayDoanhThu.DisplayMember = "Display";
                cboNgayDoanhThu.ValueMember = "Value";

                // Chọn ngày đầu tiên nếu có
                if (dtNgayNew.Rows.Count > 0)
                {
                    cboNgayDoanhThu.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi nếu không có dữ liệu
            }
        }

        private void btnLocDoanhThu_Click(object sender, EventArgs e)
        {
            string loaiThongKe = cboLoaiThongKeDoanhThu.Text;

            if (cboNamDoanhThu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn năm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int nam = Convert.ToInt32(cboNamDoanhThu.SelectedValue);
            int? thang = null;
            int? ngay = null;

            if (loaiThongKe == "THÁNG" || loaiThongKe == "NGÀY")
            {
                if (cboThangDoanhThu.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn tháng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                thang = Convert.ToInt32(cboThangDoanhThu.SelectedValue);
            }

            if (loaiThongKe == "NGÀY")
            {
                if (cboNgayDoanhThu.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ngay = Convert.ToInt32(cboNgayDoanhThu.SelectedValue);
            }

            LoadDoanhThuTheoLoai(loaiThongKe, nam, thang, ngay);
        }
        #endregion

        #region TAB SẢN PHẨM
        private void LoadTopSanPham30Ngay()
        {
            try
            {
                DateTime tuNgay = DateTime.Now.AddDays(-30);
                DateTime denNgay = DateTime.Now;
                int soLuongTop = 10;

                LoadSanPhamTheoDieuKien(soLuongTop, tuNgay, denNgay);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSanPhamTheoDieuKien(int soLuongTop, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                string sql = "EXEC sp_TopSanPhamBanChay @SoLuongTop, @TuNgay, @DenNgay";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SoLuongTop", soLuongTop),
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay)
                };

                dtSanPham = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvSanPham.DataSource = dtSanPham;
                FormatDataGridViewSanPham();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridViewSanPham()
        {
            if (dgvSanPham.Columns.Count > 0)
            {
                foreach (DataGridViewColumn column in dgvSanPham.Columns)
                {
                    switch (column.Name)
                    {
                        case "MaSP":
                            column.HeaderText = "Mã SP";
                            break;
                        case "TenSP":
                            column.HeaderText = "Tên Sản Phẩm";
                            break;
                        case "ThuongHieu":
                            column.HeaderText = "Thương Hiệu";
                            break;
                        case "TenTheLoai":
                            column.HeaderText = "Thể Loại";
                            break;
                        case "TongSoLuongBan":
                            column.HeaderText = "Số Lượng Bán";
                            break;
                        case "TongDoanhThu":
                            column.HeaderText = "Doanh Thu";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "GiaBanTrungBinh":
                            column.HeaderText = "Giá Bán TB";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "PhanTram":
                            column.HeaderText = "Phần Trăm (%)";
                            column.DefaultCellStyle.Format = "N2";
                            break;
                    }
                }

                dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnLocSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtSoLuongTop.Text, out int soLuongTop) || soLuongTop <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime tuNgay = dateTuNgaySP.Value;
                DateTime denNgay = dateDenNgaySP.Value;

                if (tuNgay > denNgay)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadSanPhamTheoDieuKien(soLuongTop, tuNgay, denNgay);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region TAB KHÁCH HÀNG
        private void LoadTopKhachHang()
        {
            try
            {
                int soLuongTop = 10;
                LoadKhachHangTheoDieuKien(soLuongTop);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKhachHangTheoDieuKien(int soLuongTop)
        {
            try
            {
                string sql = "EXEC sp_KhachHangThanThiet @SoLuongTop";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SoLuongTop", soLuongTop)
                };

                dtKhachHang = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvKhachHang.DataSource = dtKhachHang;
                FormatDataGridViewKhachHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridViewKhachHang()
        {
            if (dgvKhachHang.Columns.Count > 0)
            {
                foreach (DataGridViewColumn column in dgvKhachHang.Columns)
                {
                    switch (column.Name)
                    {
                        case "MaKH":
                            column.HeaderText = "Mã KH";
                            break;
                        case "HoTen":
                            column.HeaderText = "Họ Tên";
                            break;
                        case "SDT":
                            column.HeaderText = "SĐT";
                            break;
                        case "DiaChi":
                            column.HeaderText = "Địa Chỉ";
                            break;
                        case "SoDonDaMua":
                            column.HeaderText = "Số Đơn";
                            break;
                        case "TongChiTieu":
                            column.HeaderText = "Tổng Chi Tiêu";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "DonHangTrungBinh":
                            column.HeaderText = "Đơn Hàng TB";
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "SoNgayChuaMua":
                            column.HeaderText = "Ngày Chưa Mua";
                            break;
                        case "LanMuaGanNhat":
                            column.HeaderText = "Lần Mua Gần Nhất";
                            column.DefaultCellStyle.Format = "dd/MM/yyyy";
                            break;
                        case "PhanLoai":
                            column.HeaderText = "Phân Loại";
                            break;
                    }
                }

                dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnLocKhachHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtSoLuongKhachHang.Text, out int soLuongTop) || soLuongTop <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadKhachHangTheoDieuKien(soLuongTop);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region TAB LỢI NHUẬN

        private void LoadLoiNhuanMacDinh()
        {
            try
            {
                // Load lợi nhuận năm hiện tại
                int namHienTai = DateTime.Now.Year;

                // Chọn năm 2025 trong combobox để test
                foreach (DataRowView item in cboNamLoiNhuan.Items)
                {
                    if (Convert.ToInt32(item["Value"]) == 2025)
                    {
                        cboNamLoiNhuan.SelectedValue = item["Value"];
                        break;
                    }
                }

                // Sử dụng sp_LoiNhuanChiTietSP thay vì sp_TinhLoiNhuanChinhXac
                LoadLoiNhuanTheoLoai("NĂM", 2025, null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lợi nhuận: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoiNhuanTheoLoai(string loaiThongKe, int? nam = null, int? thang = null, int? ngay = null)
        {
            try
            {
                // Chuyển đổi từ tiếng Việt sang tiếng Anh để phù hợp với stored procedure
                string loaiThongKeSQL;
                switch (loaiThongKe)
                {
                    case "NGÀY":
                        loaiThongKeSQL = "NGAY";
                        break;
                    case "THÁNG":
                        loaiThongKeSQL = "THANG";
                        break;
                    case "NĂM":
                        loaiThongKeSQL = "NAM";
                        break;
                    default:
                        loaiThongKeSQL = "NAM";
                        break;
                }

                // Sửa: Gọi đúng stored procedure có sẵn
                string sql = "EXEC sp_LoiNhuanChiTietSP @LoaiThongKe, @Ngay, @Thang, @Nam";

                // Tạo parameters
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@LoaiThongKe", loaiThongKeSQL) // Dùng loaiThongKeSQL đã chuyển đổi
        };

                // Thêm parameter ngày nếu có
                if (ngay.HasValue)
                    parameters.Add(new SqlParameter("@Ngay", ngay.Value));
                else
                    parameters.Add(new SqlParameter("@Ngay", DBNull.Value));

                // Thêm parameter tháng nếu có
                if (thang.HasValue)
                    parameters.Add(new SqlParameter("@Thang", thang.Value));
                else
                    parameters.Add(new SqlParameter("@Thang", DBNull.Value));

                // Thêm parameter năm nếu có
                if (nam.HasValue)
                    parameters.Add(new SqlParameter("@Nam", nam.Value));
                else
                    parameters.Add(new SqlParameter("@Nam", DBNull.Value));

                dtLoiNhuan = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());

                // Kiểm tra nếu có dữ liệu
                if (dtLoiNhuan != null && dtLoiNhuan.Rows.Count > 0)
                {
                    dgvLoiNhuan.DataSource = dtLoiNhuan;
                    FormatDataGridViewLoiNhuan();

                    // Tính tổng
                    TinhTongLoiNhuan();
                }
                else
                {
                    dgvLoiNhuan.DataSource = null;
                    MessageBox.Show("Không có dữ liệu lợi nhuận cho khoảng thời gian này!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lợi nhuận: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridViewLoiNhuan()
        {
            if (dgvLoiNhuan.Columns.Count > 0)
            {
                // Đặt tên cột phù hợp với kết quả từ sp_LoiNhuanChiTietSP
                foreach (DataGridViewColumn column in dgvLoiNhuan.Columns)
                {
                    switch (column.Name)
                    {
                        case "MaSP":
                            column.HeaderText = "Mã SP";
                            column.Width = 80;
                            break;
                        case "TenSP":
                            column.HeaderText = "Tên Sản Phẩm";
                            column.Width = 200;
                            break;
                        case "SoDonHang":
                            column.HeaderText = "Số Đơn";
                            column.Width = 80;
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "SoLuongDaBan":
                            column.HeaderText = "SL Bán";
                            column.Width = 80;
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "DoanhThu":
                            column.HeaderText = "Doanh Thu";
                            column.Width = 120;
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "GiaVon":
                            column.HeaderText = "Giá Vốn";
                            column.Width = 120;
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "LoiNhuan":
                            column.HeaderText = "Lợi Nhuận";
                            column.Width = 120;
                            column.DefaultCellStyle.Format = "N0";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            // Tô màu cột lợi nhuận
                            column.DefaultCellStyle.BackColor = Color.FromArgb(225, 245, 225);
                            break;
                        case "TyLeLoiNhuan":
                            column.HeaderText = "Tỷ Lệ LN (%)";
                            column.Width = 100;
                            column.DefaultCellStyle.Format = "N2";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "ThoiGian":
                            column.HeaderText = "Thời Gian";
                            column.Width = 150;
                            break;
                    }
                }

                dgvLoiNhuan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Tô màu xen kẽ các dòng
                dgvLoiNhuan.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            }
        }

        private void cboLoaiThongKeLoiNhuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string loaiThongKe = cboLoaiThongKeLoiNhuan.Text;

            // Enable/disable combobox theo loại thống kê
            switch (loaiThongKe)
            {
                case "NGÀY":
                    cboNamLoiNhuan.Enabled = true;
                    cboThangLoiNhuan.Enabled = true;
                    cboNgayLoiNhuan.Enabled = true;
                    LoadComboBoxNgayLoiNhuan();
                    break;
                case "THÁNG":
                    cboNamLoiNhuan.Enabled = true;
                    cboThangLoiNhuan.Enabled = true;
                    cboNgayLoiNhuan.Enabled = false;
                    break;
                case "NĂM":
                    cboNamLoiNhuan.Enabled = true;
                    cboThangLoiNhuan.Enabled = false;
                    cboNgayLoiNhuan.Enabled = false;
                    break;
            }
        }

        private void cboNamLoiNhuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiThongKeLoiNhuan.Text == "NGÀY")
            {
                LoadComboBoxNgayLoiNhuan();
            }
        }

        private void cboThangLoiNhuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiThongKeLoiNhuan.Text == "NGÀY")
            {
                LoadComboBoxNgayLoiNhuan();
            }
        }

        private void LoadComboBoxNgayLoiNhuan()
        {
            try
            {
                if (cboNamLoiNhuan.SelectedValue == null || cboThangLoiNhuan.SelectedValue == null)
                    return;

                int nam = Convert.ToInt32(cboNamLoiNhuan.SelectedValue);
                int thang = Convert.ToInt32(cboThangLoiNhuan.SelectedValue);

                // Lấy danh sách ngày có dữ liệu từ bán hàng
                string sql = "EXEC sp_LayDanhSachNgayTheoThang @Thang, @Nam";
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@Thang", thang),
            new SqlParameter("@Nam", nam)
                };

                DataTable dtNgay = DatabaseHelper.ExecuteQuery(sql, parameters);

                // Tạo DataTable mới với cột Ngày
                DataTable dtNgayNew = new DataTable();
                dtNgayNew.Columns.Add("Value", typeof(int));
                dtNgayNew.Columns.Add("Display", typeof(string));

                foreach (DataRow row in dtNgay.Rows)
                {
                    int ngay = Convert.ToInt32(row["Ngay"]);
                    dtNgayNew.Rows.Add(ngay, $"Ngày {ngay}");
                }

                cboNgayLoiNhuan.DataSource = dtNgayNew;
                cboNgayLoiNhuan.DisplayMember = "Display";
                cboNgayLoiNhuan.ValueMember = "Value";

                // Chọn ngày đầu tiên nếu có
                if (dtNgayNew.Rows.Count > 0)
                {
                    cboNgayLoiNhuan.SelectedIndex = 0;
                }
                else
                {
                    cboNgayLoiNhuan.DataSource = null;
                    cboNgayLoiNhuan.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi nếu không có dữ liệu
                cboNgayLoiNhuan.DataSource = null;
                cboNgayLoiNhuan.Items.Clear();
            }
        }

        private void btnLocLoiNhuan_Click(object sender, EventArgs e)
        {
            try
            {
                string loaiThongKe = cboLoaiThongKeLoiNhuan.Text;

                if (cboNamLoiNhuan.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn năm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int nam = Convert.ToInt32(cboNamLoiNhuan.SelectedValue);
                int? thang = null;
                int? ngay = null;

                if (loaiThongKe == "THÁNG" || loaiThongKe == "NGÀY")
                {
                    if (cboThangLoiNhuan.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn tháng!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    thang = Convert.ToInt32(cboThangLoiNhuan.SelectedValue);
                }

                if (loaiThongKe == "NGÀY")
                {
                    if (cboNgayLoiNhuan.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn ngày!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ngay = Convert.ToInt32(cboNgayLoiNhuan.SelectedValue);
                }

                // DEBUG: Hiển thị thông tin filter
                // MessageBox.Show($"Filter: Loại={loaiThongKe}, Năm={nam}, Tháng={thang}, Ngày={ngay}",
                //     "Debug Filter", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadLoiNhuanTheoLoai(loaiThongKe, nam, thang, ngay);

                // Tính tổng lợi nhuận và hiển thị
                TinhTongLoiNhuan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc lợi nhuận: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TinhTongLoiNhuan()
        {
            if (dtLoiNhuan != null && dtLoiNhuan.Rows.Count > 0)
            {
                decimal tongDoanhThu = 0;
                decimal tongGiaVon = 0;
                decimal tongLoiNhuan = 0;
                int tongSoLuong = 0;
                int tongDonHang = 0;

                foreach (DataRow row in dtLoiNhuan.Rows)
                {
                    tongDoanhThu += Convert.ToDecimal(row["DoanhThu"]);
                    tongGiaVon += Convert.ToDecimal(row["GiaVon"]);
                    tongLoiNhuan += Convert.ToDecimal(row["LoiNhuan"]);
                    tongSoLuong += Convert.ToInt32(row["SoLuongDaBan"]);
                    tongDonHang += Convert.ToInt32(row["SoDonHang"]);
                }

                decimal tyLeLoiNhuan = 0;
                if (tongDoanhThu > 0)
                {
                    tyLeLoiNhuan = (tongLoiNhuan / tongDoanhThu) * 100;
                }

                // Hiển thị thông tin tổng hợp (nếu có label trên form)
                

                // Hoặc hiển thị trong MessageBox
                MessageBox.Show($"Kết quả lợi nhuận:\n" +
                               $"• Số đơn hàng: {tongDonHang:N0}\n" +
                               $"• Tổng số lượng bán: {tongSoLuong:N0}\n" +
                               $"• Tổng doanh thu: {tongDoanhThu:N0} đ\n" +
                               $"• Tổng giá vốn: {tongGiaVon:N0} đ\n" +
                               $"• Tổng lợi nhuận: {tongLoiNhuan:N0} đ\n" +
                               $"• Tỷ lệ lợi nhuận: {tyLeLoiNhuan:N2}%",
                               "Tổng hợp lợi nhuận",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region XUẤT EXCEL (KHÔNG DÙNG INTEROP)
        private void XuatExcelHoaDon()
        {
            if (dtHoaDon == null || dtHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|CSV Files|*.csv";
                saveFileDialog.Title = "Lưu file Excel";
                saveFileDialog.FileName = $"HoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(dtHoaDon, saveFileDialog.FileName, "HÓA ĐƠN");
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XuatExcelDoanhThu()
        {
            if (dtDoanhThu == null || dtDoanhThu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|CSV Files|*.csv";
                saveFileDialog.Title = "Lưu file Excel";
                saveFileDialog.FileName = $"DoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(dtDoanhThu, saveFileDialog.FileName, "DOANH THU");
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XuatExcelSanPham()
        {
            if (dtSanPham == null || dtSanPham.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|CSV Files|*.csv";
                saveFileDialog.Title = "Lưu file Excel";
                saveFileDialog.FileName = $"SanPham_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(dtSanPham, saveFileDialog.FileName, "SẢN PHẨM BÁN CHẠY");
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XuatExcelKhachHang()
        {
            if (dtKhachHang == null || dtKhachHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|CSV Files|*.csv";
                saveFileDialog.Title = "Lưu file Excel";
                saveFileDialog.FileName = $"KhachHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(dtKhachHang, saveFileDialog.FileName, "KHÁCH HÀNG");
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XuatExcelLoiNhuan()
        {
            if (dtLoiNhuan == null || dtLoiNhuan.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|CSV Files|*.csv";
                saveFileDialog.Title = "Lưu file Excel";
                saveFileDialog.FileName = $"LoiNhuan_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(dtLoiNhuan, saveFileDialog.FileName, "LỢI NHUẬN");
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(DataTable dataTable, string filePath, string sheetName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Thêm tiêu đề
                sb.AppendLine($"BÁO CÁO {sheetName.ToUpper()}");
                sb.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sb.AppendLine();

                // Thêm tên cột
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                    .Select(column => column.ColumnName)
                    .ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                // Thêm dữ liệu
                foreach (DataRow row in dataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field =>
                        field.ToString().Replace(",", ";")).ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }

                // Lưu file
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất dữ liệu: " + ex.Message);
            }
        }
        #endregion
    }
}