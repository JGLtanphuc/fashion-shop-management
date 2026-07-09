using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DA_QLShopQuanAo.Helpers;
// Thư viện Crystal Report
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormDonHang : Form
    {
        // 1. CẤU HÌNH KẾT NỐI - Sử dụng từ DatabaseHelper
        string connectionString = DatabaseHelper.GetConnectionString(); // DÒNG QUAN TRỌNG

        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dtSanPham;    // Dữ liệu bảng sản phẩm (bảng trên)
        DataTable dtChiTiet;    // Dữ liệu giỏ hàng (bảng dưới)

        // Biến cờ để chặn vòng lặp vô tận khi 2 combobox Khách hàng tự động cập nhật nhau
        bool isSyncing = false;

        public FormDonHang()
        {
            InitializeComponent();
        }

        private void FormDonHang_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadComboBoxData();   // Load Khách hàng, SĐT, Nhân viên
            LoadSanPham();        // Load danh sách Sản phẩm
            InitGioHang();        // Tạo cấu trúc bảng giỏ hàng
            LoadComboBoxMaDH();   // Load danh sách Mã Đơn Hàng để in

            // Thiết lập mặc định
            txtSoLuong.Text = "1";
            dtpNgayLap.Value = DateTime.Now;
            txtDiaChi.Clear();

            // Tự động sinh mã đơn hàng (Ví dụ: DH + NgàyGiờPhútGiây)
            txtMaDH.Text = "DH" + DateTime.Now.ToString("ddHHmmss");
            txtMaDH.Enabled = false; // Khóa không cho sửa mã
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
            if (label7 != null) label7.ForeColor = ThemeColor.PrimaryColor;
            if (label9 != null) label9.ForeColor = ThemeColor.PrimaryColor;
        }

        // --- CÁC HÀM HỖ TRỢ LOAD DỮ LIỆU TỪ SQL ---

        void LoadComboBoxMaDH()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // SqlDataAdapter sẽ tự động mở kết nối nếu cần
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaDH FROM DonHang ORDER BY NgayLap DESC", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboMaDH.DataSource = dt;
                    cboMaDH.DisplayMember = "MaDH";
                    cboMaDH.ValueMember = "MaDH";
                    cboMaDH.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load mã đơn: " + ex.Message);
            }
        }

        void LoadComboBoxData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Load dữ liệu Khách Hàng
                    string queryKH = "SELECT MaKH, HoTen, SDT, DiaChi FROM KhachHang";
                    SqlDataAdapter daKH = new SqlDataAdapter(queryKH, conn);
                    DataTable dtKH = new DataTable();
                    daKH.Fill(dtKH);

                    // -- Setup ComboBox Tên Khách --
                    cboKhachHang.DataSource = dtKH;
                    cboKhachHang.DisplayMember = "HoTen";
                    cboKhachHang.ValueMember = "MaKH";
                    cboKhachHang.SelectedIndex = -1;
                    cboKhachHang.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cboKhachHang.AutoCompleteSource = AutoCompleteSource.ListItems;

                    // -- Setup ComboBox SĐT (Dùng bản Copy để tránh lỗi binding 2 chiều) --
                    cboSDT.DataSource = dtKH.Copy();
                    cboSDT.DisplayMember = "SDT";
                    cboSDT.ValueMember = "MaKH";
                    cboSDT.SelectedIndex = -1;
                    cboSDT.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cboSDT.AutoCompleteSource = AutoCompleteSource.ListItems;

                    // 2. Load dữ liệu Nhân Viên
                    string queryNV = "SELECT MaNV, HoTen FROM NhanVien";
                    SqlDataAdapter daNV = new SqlDataAdapter(queryNV, conn);
                    DataTable dtNV = new DataTable();
                    daNV.Fill(dtNV);
                    cboNhanVien.DataSource = dtNV;
                    cboNhanVien.DisplayMember = "MaNV";
                    cboNhanVien.ValueMember = "MaNV";
                } // Kết nối tự động đóng ở đây
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }
        void LoadSanPham()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Lấy Mã, Tên, Giá và Số lượng tồn kho
                    string query = "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtSanPham = new DataTable();
                    adapter.Fill(dtSanPham);

                    // Kiểm tra nếu cần gọi từ thread khác
                    if (dgvSanPham.InvokeRequired)
                    {
                        dgvSanPham.Invoke(new Action(() =>
                        {
                            dgvSanPham.DataSource = dtSanPham;

                            // Định dạng hiển thị
                            dgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                            dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                            dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                            dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
                            dgvSanPham.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
                            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }));
                    }
                    else
                    {
                        dgvSanPham.DataSource = dtSanPham;

                        // Định dạng hiển thị
                        dgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                        dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                        dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                        dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
                        dgvSanPham.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
                        dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load sản phẩm: " + ex.Message);
            }
        }

        void InitGioHang()
        {
            // Tạo bảng tạm chứa chi tiết đơn hàng (Giỏ hàng)
            dtChiTiet = new DataTable();
            dtChiTiet.Columns.Add("MaSP", typeof(string));
            dtChiTiet.Columns.Add("TenSP", typeof(string));
            dtChiTiet.Columns.Add("SoLuong", typeof(int));
            dtChiTiet.Columns.Add("DonGia", typeof(decimal));
            dtChiTiet.Columns.Add("ThanhTien", typeof(decimal));

            dgvChiTietDonHang.DataSource = dtChiTiet;

            // Định dạng hiển thị lưới giỏ hàng
            dgvChiTietDonHang.Columns["MaSP"].HeaderText = "Mã SP";
            dgvChiTietDonHang.Columns["TenSP"].HeaderText = "Tên SP";
            dgvChiTietDonHang.Columns["SoLuong"].HeaderText = "SL";
            dgvChiTietDonHang.Columns["DonGia"].HeaderText = "Đơn Giá";
            dgvChiTietDonHang.Columns["ThanhTien"].HeaderText = "Thành Tiền";

            dgvChiTietDonHang.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTietDonHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvChiTietDonHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- XỬ LÝ SỰ KIỆN GIAO DIỆN (UI EVENTS) ---

        // 1. Click vào bảng Sản phẩm -> Reset số lượng về 1 để chọn nhanh
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSoLuong.Text = "1";
                txtSoLuong.Focus();
            }
        }

        // 2. Chặn nhập chữ vào ô Số Lượng
        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        // 3. Nút THÊM SẢN PHẨM vào giỏ
        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách trên!");
                return;
            }

            int soLuongMua = 0;
            if (!int.TryParse(txtSoLuong.Text, out soLuongMua) || soLuongMua <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0"); return;
            }

            // Lấy thông tin từ dòng đang chọn
            string maSP = dgvSanPham.CurrentRow.Cells["MaSP"].Value.ToString();
            string tenSP = dgvSanPham.CurrentRow.Cells["TenSP"].Value.ToString();
            decimal giaBan = decimal.Parse(dgvSanPham.CurrentRow.Cells["GiaBan"].Value.ToString());
            int tonKho = int.Parse(dgvSanPham.CurrentRow.Cells["SoLuongTon"].Value.ToString());

            // Kiểm tra tồn kho thực tế
            if (soLuongMua > tonKho)
            {
                MessageBox.Show($"Trong kho chỉ còn {tonKho} sản phẩm này!");
                return;
            }

            // Kiểm tra xem SP này đã có trong giỏ chưa
            DataRow existingRow = dtChiTiet.AsEnumerable().FirstOrDefault(r => r.Field<string>("MaSP") == maSP);

            if (existingRow != null)
            {
                // Nếu có rồi -> Cộng dồn số lượng
                int slCu = (int)existingRow["SoLuong"];
                if (slCu + soLuongMua > tonKho)
                {
                    MessageBox.Show($"Tổng số lượng vượt quá tồn kho ({tonKho})!");
                    return;
                }
                existingRow["SoLuong"] = slCu + soLuongMua;
                existingRow["ThanhTien"] = (slCu + soLuongMua) * giaBan;
            }
            else
            {
                // Nếu chưa có -> Thêm dòng mới
                dtChiTiet.Rows.Add(maSP, tenSP, soLuongMua, giaBan, soLuongMua * giaBan);
            }
        }

        // 4. Nút XÓA SẢN PHẨM khỏi giỏ
        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTietDonHang.CurrentRow != null)
                dgvChiTietDonHang.Rows.Remove(dgvChiTietDonHang.CurrentRow);
            else
                MessageBox.Show("Vui lòng chọn sản phẩm trong giỏ hàng để xóa!");
        }

        // --- CÁC NÚT CHỨC NĂNG CHÍNH (CRUD) ---

        // 5. Nút TẠO ĐƠN HÀNG (Lưu vào CSDL)
        private void btnTaoDon_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu đầu vào
            if (dtChiTiet.Rows.Count == 0) { MessageBox.Show("Giỏ hàng đang trống!"); return; }
            if (cboKhachHang.SelectedValue == null) { MessageBox.Show("Chưa chọn khách hàng!"); return; }

            // 2. Tính tổng tiền
            decimal tongTien = 0;
            foreach (DataRow r in dtChiTiet.Rows) tongTien += Convert.ToDecimal(r["ThanhTien"]);

            using (SqlConnection conn = new SqlConnection(connectionString)) // Sử dụng connectionString từ DatabaseHelper
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Bắt đầu giao dịch an toàn

                try
                {
                    // A. Insert bảng DonHang
                    string sqlDonHang = "INSERT INTO DonHang (MaDH, MaKH, MaNV, NgayLap, TongTien, DiaChiGiaoHang) VALUES (@ma, @kh, @nv, @ngay, @tong, @dc)";
                    SqlCommand cmdDH = new SqlCommand(sqlDonHang, conn, transaction);
                    cmdDH.Parameters.AddWithValue("@ma", txtMaDH.Text);
                    cmdDH.Parameters.AddWithValue("@kh", cboKhachHang.SelectedValue);
                    cmdDH.Parameters.AddWithValue("@nv", cboNhanVien.SelectedValue ?? DBNull.Value);
                    cmdDH.Parameters.AddWithValue("@ngay", dtpNgayLap.Value);
                    cmdDH.Parameters.AddWithValue("@tong", tongTien);
                    cmdDH.Parameters.AddWithValue("@dc", txtDiaChi.Text);
                    cmdDH.ExecuteNonQuery();

                    // B. Insert bảng ChiTietDonHang
                    foreach (DataRow row in dtChiTiet.Rows)
                    {
                        string sqlChiTiet = "INSERT INTO ChiTietDonHang (MaDH, MaSP, TenSP, SoLuong, DonGia, ThanhTien) VALUES (@madh, @masp, @tensp, @sl, @gia, @thanhTien)";
                        SqlCommand cmdCT = new SqlCommand(sqlChiTiet, conn, transaction);
                        cmdCT.Parameters.AddWithValue("@madh", txtMaDH.Text);
                        cmdCT.Parameters.AddWithValue("@masp", row["MaSP"]);
                        cmdCT.Parameters.AddWithValue("@tensp", row["TenSP"]);
                        cmdCT.Parameters.AddWithValue("@sl", row["SoLuong"]);
                        cmdCT.Parameters.AddWithValue("@gia", row["DonGia"]);
                        cmdCT.Parameters.AddWithValue("@thanhTien", row["ThanhTien"]);
                        cmdCT.ExecuteNonQuery();
                    }

                    transaction.Commit(); // Xác nhận lưu thành công
                    MessageBox.Show("Tạo đơn hàng thành công!");

                    // Cập nhật ComboBox để in ngay
                    string maDonVuaTao = txtMaDH.Text;
                    LoadComboBoxMaDH();
                    cboMaDH.SelectedValue = maDonVuaTao;

                    // Reset form cho đơn tiếp theo
                    dtChiTiet.Clear();
                    txtMaDH.Text = "DH" + DateTime.Now.ToString("ddHHmmss");
                    LoadSanPham(); // Cập nhật lại kho
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi tạo đơn: " + ex.Message);
                }
            }
        }

        // 6. Nút XUẤT HÓA ĐƠN (In Report)
        private void btnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (cboMaDH.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Đơn Hàng cần in trong ô danh sách!");
                cboMaDH.Focus();
                return;
            }

            try
            {
                string maDH = cboMaDH.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt.TableName = "dtHoaDon";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    d.MaDH, d.NgayLap, d.TongTien,
                    ISNULL(k.HoTen, N'Khách vãng lai') as HoTen, 
                    ISNULL(k.SDT, '') as SDT, 
                    ISNULL(k.DiaChi, '') as DiaChi,
                    ISNULL(ct.TenSP, ISNULL(s.TenSP, N'Sản phẩm lỗi')) as TenSP, 
                    ct.SoLuong, ct.DonGia, ct.ThanhTien
                FROM DonHang d
                JOIN ChiTietDonHang ct ON d.MaDH = ct.MaDH
                LEFT JOIN KhachHang k ON d.MaKH = k.MaKH
                LEFT JOIN SanPham s ON ct.MaSP = s.MaSP
                WHERE d.MaDH = @maDH";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maDH", maDH);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    CrystalReport1 rpt = new CrystalReport1();

                    // QUAN TRỌNG: Thiết lập connection cho report
                    SetReportConnection(rpt);

                    rpt.SetDataSource(dt);

                    FormInHoaDon frm = new FormInHoaDon();
                    frm.crystalReportViewer1.ReportSource = rpt;
                    frm.crystalReportViewer1.Refresh();
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cho đơn hàng này!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in ấn: " + ex.Message);
            }
        }

        // PHƯƠNG THỨC MỚI: Thiết lập connection cho Crystal Report
        private void SetReportConnection(CrystalReport1 report)
        {
            try
            {
                ConnectionInfo connectionInfo = new ConnectionInfo();

                // Parse connection string từ DatabaseHelper
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                connectionInfo.ServerName = builder.DataSource;           // Tên server
                connectionInfo.DatabaseName = builder.InitialCatalog;     // Tên database
                connectionInfo.IntegratedSecurity = builder.IntegratedSecurity; // Windows Authentication

                // Nếu dùng SQL Authentication (có username/password)
                if (!builder.IntegratedSecurity && !string.IsNullOrEmpty(builder.UserID))
                {
                    connectionInfo.UserID = builder.UserID;
                    connectionInfo.Password = builder.Password;
                }

                // Áp dụng connection info cho tất cả tables trong report
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in report.Database.Tables)
                {
                    TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                    tableLogOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(tableLogOnInfo);
                }

                // Cập nhật lại các subreport nếu có
                foreach (ReportDocument subReport in report.Subreports)
                {
                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in subReport.Database.Tables)
                    {
                        TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                        tableLogOnInfo.ConnectionInfo = connectionInfo;
                        table.ApplyLogOnInfo(tableLogOnInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thiết lập kết nối report: " + ex.Message);
            }
        }

        // --- ĐỒNG BỘ COMBOBOX TÊN VÀ SĐT ---

        private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSyncing) return;
            if (cboKhachHang.SelectedIndex != -1)
            {
                try
                {
                    isSyncing = true;
                    DataRowView drv = (DataRowView)cboKhachHang.SelectedItem;
                    txtDiaChi.Text = drv["DiaChi"].ToString();
                    cboSDT.SelectedValue = cboKhachHang.SelectedValue;
                }
                finally { isSyncing = false; }
            }
        }

        private void cboSDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSyncing) return;
            if (cboSDT.SelectedIndex != -1)
            {
                try
                {
                    isSyncing = true;
                    DataRowView drv = (DataRowView)cboSDT.SelectedItem;
                    txtDiaChi.Text = drv["DiaChi"].ToString();
                    cboKhachHang.SelectedValue = cboSDT.SelectedValue;
                }
                finally { isSyncing = false; }
            }
        }

        // Các sự kiện không dùng đến
        private void dgvChiTietDonHang_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtMaDH_TextChanged(object sender, EventArgs e) { }
    }
}