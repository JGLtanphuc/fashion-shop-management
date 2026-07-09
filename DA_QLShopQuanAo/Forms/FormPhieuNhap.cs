using DA_QLShopQuanAo.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormPhieuNhap : Form
    {
        DataTable dtChiTiet;
        decimal tongTienPhieu = 0;
        string _maNVHienTai;
        private ErrorProvider errorProvider1 = new ErrorProvider();

        public FormPhieuNhap(string maNV = "")
        {
            InitializeComponent();
            _maNVHienTai = maNV;
        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadComboBox();
            KhoiTaoBangTam();
            TaoMaPhieuTuDong();
            dtpNgayNhap.Value = DateTime.Now;

            if (!string.IsNullOrEmpty(_maNVHienTai))
            {
                try
                {
                    cboNhanVien.SelectedValue = _maNVHienTai;
                }
                catch
                {
                    cboNhanVien.SelectedIndex = -1;
                }
            }
        }

        private void LoadTheme()
        {
            btnThemSP.BackColor = ThemeColor.PrimaryColor;
            btnXoaSP.BackColor = Color.FromArgb(220, 53, 69);
            btnSuaSP.BackColor = Color.FromArgb(40, 167, 69);
            btnLuuPhieu.BackColor = Color.FromArgb(23, 162, 184);
            btnHuyPhieu.BackColor = Color.FromArgb(108, 117, 125);
        }

        private void TaoMaPhieuTuDong()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Tạo mã phiếu tự động: PN + 6 số (PN000001)
                    string query = @"
                        SELECT 'PN' + RIGHT('000000' + 
                            CAST(ISNULL(MAX(CAST(SUBSTRING(MaPN, 3, LEN(MaPN)-2) AS INT)), 0) + 1 AS VARCHAR(6)), 6) 
                        AS MaMoi 
                        FROM PhieuNhap";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    string maMoi = cmd.ExecuteScalar()?.ToString() ?? "PN000001";
                    txtMaPN.Text = maMoi;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Lỗi tạo mã phiếu tự động", ex);
                txtMaPN.Text = "PN000001";
            }
        }

        private void LoadComboBox()
        {
            try
            {
                // Load Nhà Cung Cấp
                string sqlNCC = "SELECT MaNCC, TenNCC FROM NhaCungCap ORDER BY TenNCC";
                DataTable dtNCC = DatabaseHelper.ExecuteQuery(sqlNCC);
                cboNhaCungCap.DataSource = dtNCC;
                cboNhaCungCap.DisplayMember = "TenNCC";
                cboNhaCungCap.ValueMember = "MaNCC";
                cboNhaCungCap.SelectedIndex = -1;

                // Load Sản Phẩm
                string sqlSP = "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham ORDER BY TenSP";
                DataTable dtSP = DatabaseHelper.ExecuteQuery(sqlSP);
                cboSanPham.DataSource = dtSP;
                cboSanPham.DisplayMember = "TenSP";
                cboSanPham.ValueMember = "MaSP";
                cboSanPham.SelectedIndex = -1;

                // Load Nhân Viên
                string sqlNV = "SELECT MaNV, HoTen FROM NhanVien ORDER BY HoTen";
                DataTable dtNV = DatabaseHelper.ExecuteQuery(sqlNV);
                cboNhanVien.DataSource = dtNV;
                cboNhanVien.DisplayMember = "HoTen";
                cboNhanVien.ValueMember = "MaNV";
                cboNhanVien.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLog("Lỗi load combobox", ex);
            }
        }

        private void KhoiTaoBangTam()
        {
            dtChiTiet = new DataTable();
            dtChiTiet.Columns.Add("MaSP", typeof(string));
            dtChiTiet.Columns.Add("TenSP", typeof(string));
            dtChiTiet.Columns.Add("SoLuong", typeof(int));
            dtChiTiet.Columns.Add("GiaNhap", typeof(decimal));
            dtChiTiet.Columns.Add("ThanhTien", typeof(decimal));

            dgvChiTietNhap.DataSource = dtChiTiet;

            // Định dạng DataGridView
            dgvChiTietNhap.Columns["MaSP"].HeaderText = "Mã SP";
            dgvChiTietNhap.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
            dgvChiTietNhap.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvChiTietNhap.Columns["GiaNhap"].HeaderText = "Giá Nhập";
            dgvChiTietNhap.Columns["ThanhTien"].HeaderText = "Thành Tiền";

            dgvChiTietNhap.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTietNhap.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            dgvChiTietNhap.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvChiTietNhap.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvChiTietNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTietNhap.ReadOnly = true;

            // Đặt chiều cao dòng
            dgvChiTietNhap.RowTemplate.Height = 30;
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cboSanPham, "");

            if (cboSanPham.SelectedValue != null)
            {
                try
                {
                    string maSP = cboSanPham.SelectedValue.ToString();
                    string query = "SELECT GiaBan, SoLuongTon FROM SanPham WHERE MaSP = @MaSP";

                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            decimal giaBan = Convert.ToDecimal(reader["GiaBan"]);
                            int tonKho = Convert.ToInt32(reader["SoLuongTon"]);

                            // Gợi ý giá nhập (80% giá bán)
                            txtGiaNhap.Text = (giaBan * 0.8m).ToString("N0");
                            lblTonKho.Text = $"Tồn kho: {tonKho:N0}";
                            lblTonKho.ForeColor = tonKho < 10 ? Color.Red : Color.Black;
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Không hiển thị lỗi nếu không tìm thấy
                }
            }
            else
            {
                txtGiaNhap.Clear();
                lblTonKho.Text = "Tồn kho: 0";
            }
        }

        private bool KiemTraDuLieuThemSP()
        {
            bool isValid = true;

            // Kiểm tra sản phẩm
            if (cboSanPham.SelectedValue == null)
            {
                errorProvider1.SetError(cboSanPham, "Vui lòng chọn sản phẩm!");
                cboSanPham.Focus();
                isValid = false;
            }

            // Kiểm tra số lượng
            if (string.IsNullOrEmpty(txtSoLuong.Text))
            {
                errorProvider1.SetError(txtSoLuong, "Vui lòng nhập số lượng!");
                txtSoLuong.Focus();
                isValid = false;
            }
            else if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                errorProvider1.SetError(txtSoLuong, "Số lượng phải là số nguyên dương!");
                txtSoLuong.Focus();
                isValid = false;
            }

            // Kiểm tra giá nhập
            if (string.IsNullOrEmpty(txtGiaNhap.Text))
            {
                errorProvider1.SetError(txtGiaNhap, "Vui lòng nhập giá nhập!");
                txtGiaNhap.Focus();
                isValid = false;
            }
            else if (!decimal.TryParse(txtGiaNhap.Text.Replace(",", ""), out decimal giaNhap) || giaNhap <= 0)
            {
                errorProvider1.SetError(txtGiaNhap, "Giá nhập phải lớn hơn 0!");
                txtGiaNhap.Focus();
                isValid = false;
            }

            return isValid;
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu
                if (!KiemTraDuLieuThemSP())
                    return;

                int soLuong = int.Parse(txtSoLuong.Text);
                decimal giaNhap = decimal.Parse(txtGiaNhap.Text.Replace(",", ""));
                string maSP = cboSanPham.SelectedValue.ToString();
                string tenSP = cboSanPham.Text;
                decimal thanhTien = soLuong * giaNhap;

                // Kiểm tra trùng sản phẩm
                foreach (DataRow row in dtChiTiet.Rows)
                {
                    if (row["MaSP"].ToString() == maSP)
                    {
                        MessageBox.Show("Sản phẩm đã có trong phiếu! Vui lòng xóa hoặc sửa số lượng.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Kiểm tra tồn kho hiện tại (cảnh báo nếu tồn kho cao)
                int tonKhoHienTai = 0;
                string queryTonKho = "SELECT SoLuongTon FROM SanPham WHERE MaSP = @MaSP";
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(queryTonKho, conn);
                    cmd.Parameters.AddWithValue("@MaSP", maSP);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        tonKhoHienTai = Convert.ToInt32(result);
                    }
                }

                // Cảnh báo nếu tồn kho quá cao
                if (tonKhoHienTai > 100 && soLuong > 50)
                {
                    DialogResult warning = MessageBox.Show(
                        $"Sản phẩm '{tenSP}' hiện có {tonKhoHienTai:N0} cái trong kho.\n" +
                        $"Bạn có chắc muốn nhập thêm {soLuong:N0} cái?\n\n" +
                        $"Tổng tồn kho sẽ là: {tonKhoHienTai + soLuong:N0} cái",
                        "Cảnh báo tồn kho cao",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (warning == DialogResult.No) return;
                }

                // Thêm vào DataTable
                DataRow newRow = dtChiTiet.NewRow();
                newRow["MaSP"] = maSP;
                newRow["TenSP"] = tenSP;
                newRow["SoLuong"] = soLuong;
                newRow["GiaNhap"] = giaNhap;
                newRow["ThanhTien"] = thanhTien;
                dtChiTiet.Rows.Add(newRow);

                // Cập nhật tổng tiền
                tongTienPhieu += thanhTien;
                lblTongTien.Text = tongTienPhieu.ToString("N0") + " đ";
                lblTongTien.ForeColor = Color.Red;

                // Reset input
                ResetInputChiTiet();

                // Xóa lỗi
                errorProvider1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLog("Lỗi thêm sản phẩm vào phiếu", ex);
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTietNhap.CurrentRow != null && dgvChiTietNhap.CurrentRow.Index >= 0)
            {
                // Xác nhận xóa
                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc muốn xóa sản phẩm này khỏi phiếu?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    // Trừ khỏi tổng tiền
                    decimal thanhTien = Convert.ToDecimal(dgvChiTietNhap.CurrentRow.Cells["ThanhTien"].Value);
                    tongTienPhieu -= thanhTien;
                    lblTongTien.Text = tongTienPhieu.ToString("N0") + " đ";

                    // Xóa dòng
                    dtChiTiet.Rows.RemoveAt(dgvChiTietNhap.CurrentRow.Index);

                    MessageBox.Show("Đã xóa sản phẩm khỏi phiếu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTietNhap.CurrentRow != null && dgvChiTietNhap.CurrentRow.Index >= 0)
            {
                try
                {
                    if (!KiemTraDuLieuThemSP())
                        return;

                    DataGridViewRow row = dgvChiTietNhap.CurrentRow;

                    // Trừ tiền cũ
                    decimal thanhTienCu = Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                    tongTienPhieu -= thanhTienCu;

                    // Tính tiền mới
                    int soLuongMoi = int.Parse(txtSoLuong.Text);
                    decimal giaNhapMoi = decimal.Parse(txtGiaNhap.Text.Replace(",", ""));
                    decimal thanhTienMoi = soLuongMoi * giaNhapMoi;
                    tongTienPhieu += thanhTienMoi;

                    // Cập nhật dòng
                    row.Cells["SoLuong"].Value = soLuongMoi;
                    row.Cells["GiaNhap"].Value = giaNhapMoi;
                    row.Cells["ThanhTien"].Value = thanhTienMoi;

                    // Cập nhật tổng tiền
                    lblTongTien.Text = tongTienPhieu.ToString("N0") + " đ";

                    MessageBox.Show("Đã cập nhật sản phẩm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetInputChiTiet();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi sửa sản phẩm: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogHelper.WriteLog("Lỗi sửa sản phẩm", ex);
                }
            }
        }

        private void dgvChiTietNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvChiTietNhap.Rows[e.RowIndex];
                    string maSP = row.Cells["MaSP"].Value.ToString();

                    // Chọn sản phẩm trong combobox
                    cboSanPham.SelectedValue = maSP;

                    // Hiển thị thông tin
                    txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                    txtGiaNhap.Text = Convert.ToDecimal(row.Cells["GiaNhap"].Value).ToString("N0");
                }
                catch (Exception ex)
                {
                    // Bỏ qua lỗi
                }
            }
        }

        private void ResetInputChiTiet()
        {
            cboSanPham.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtGiaNhap.Clear();
            lblTonKho.Text = "Tồn kho: 0";
            cboSanPham.Focus();
        }

        private bool KiemTraDuLieuPhieuNhap()
        {
            bool isValid = true;

            // Kiểm tra nhà cung cấp
            if (cboNhaCungCap.SelectedValue == null)
            {
                errorProvider1.SetError(cboNhaCungCap, "Vui lòng chọn nhà cung cấp!");
                cboNhaCungCap.Focus();
                isValid = false;
            }

            // Kiểm tra nhân viên
            if (cboNhanVien.SelectedValue == null)
            {
                errorProvider1.SetError(cboNhanVien, "Vui lòng chọn nhân viên!");
                cboNhanVien.Focus();
                isValid = false;
            }

            // Kiểm tra có sản phẩm nào không
            if (dtChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Phiếu nhập chưa có sản phẩm nào!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSanPham.Focus();
                isValid = false;
            }

            return isValid;
        }

        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu
                if (!KiemTraDuLieuPhieuNhap())
                    return;

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // BƯỚC 1: Lưu PHIẾU NHẬP
                        string sqlPhieuNhap = @"
                            INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien, GhiChu)
                            VALUES (@MaPN, @MaNCC, @MaNV, @NgayNhap, @TongTien, @GhiChu)";

                        SqlCommand cmdPhieuNhap = new SqlCommand(sqlPhieuNhap, conn, transaction);
                        cmdPhieuNhap.Parameters.AddWithValue("@MaPN", txtMaPN.Text.Trim());
                        cmdPhieuNhap.Parameters.AddWithValue("@MaNCC", cboNhaCungCap.SelectedValue);
                        cmdPhieuNhap.Parameters.AddWithValue("@MaNV", cboNhanVien.SelectedValue);
                        cmdPhieuNhap.Parameters.AddWithValue("@NgayNhap", dtpNgayNhap.Value);
                        cmdPhieuNhap.Parameters.AddWithValue("@TongTien", tongTienPhieu);
                        cmdPhieuNhap.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text ?? "");
                        cmdPhieuNhap.ExecuteNonQuery();

                        // Biến theo dõi có sản phẩm nào được cập nhật giá không
                        bool coSanPhamDuocCapNhatGia = false;

                        // BƯỚC 2: Lưu CHI TIẾT PHIẾU NHẬP (Trigger sẽ tự động tạo Lô Hàng)
                        string sqlChiTiet = @"
                            INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuongNhap, GiaNhap, ThanhTien)
                            VALUES (@MaPN, @MaSP, @SoLuongNhap, @GiaNhap, @ThanhTien)";

                        foreach (DataRow row in dtChiTiet.Rows)
                        {
                            SqlCommand cmdChiTiet = new SqlCommand(sqlChiTiet, conn, transaction);
                            cmdChiTiet.Parameters.AddWithValue("@MaPN", txtMaPN.Text.Trim());
                            cmdChiTiet.Parameters.AddWithValue("@MaSP", row["MaSP"]);
                            cmdChiTiet.Parameters.AddWithValue("@SoLuongNhap", row["SoLuong"]);
                            cmdChiTiet.Parameters.AddWithValue("@GiaNhap", row["GiaNhap"]);
                            cmdChiTiet.Parameters.AddWithValue("@ThanhTien", row["ThanhTien"]);
                            cmdChiTiet.ExecuteNonQuery();

                            // ====================================================
                            // BƯỚC 3: LOGIC CẬP NHẬT GIÁ BÁN THÔNG MINH
                            // ====================================================
                            string maSP = row["MaSP"].ToString();
                            decimal giaNhapMoi = Convert.ToDecimal(row["GiaNhap"]);

                            // Lấy giá bán hiện tại của sản phẩm
                            string sqlGetGiaBan = "SELECT GiaBan FROM SanPham WHERE MaSP = @MaSP";
                            SqlCommand cmdGetGia = new SqlCommand(sqlGetGiaBan, conn, transaction);
                            cmdGetGia.Parameters.AddWithValue("@MaSP", maSP);
                            object giaBanResult = cmdGetGia.ExecuteScalar();

                            if (giaBanResult != null && giaBanResult != DBNull.Value)
                            {
                                decimal giaBanHienTai = Convert.ToDecimal(giaBanResult);

                                // Tính giá bán đề xuất từ giá nhập mới (lãi 30%)
                                decimal giaBanDeXuat = giaNhapMoi * 1.3m;

                                // LOGIC CẬP NHẬT THEO YÊU CẦU:
                                // 1. Nếu giá nhập mới LỚN HƠN giá bán hiện tại → CẬP NHẬT lên 1.3*giaNhap
                                // 2. Nếu giá nhập mới NHỎ HƠN giá bán hiện tại → GIỮ NGUYÊN giá bán cũ

                                if (giaBanDeXuat > giaBanHienTai)
                                {
                                    // Trường hợp 1: Giá nhập cao hơn → tăng giá bán
                                    string sqlUpdateGia = @"
                                        UPDATE SanPham 
                                        SET GiaBan = @GiaBanMoi 
                                        WHERE MaSP = @MaSP";

                                    SqlCommand cmdUpdateGia = new SqlCommand(sqlUpdateGia, conn, transaction);
                                    cmdUpdateGia.Parameters.AddWithValue("@GiaBanMoi", giaBanDeXuat);
                                    cmdUpdateGia.Parameters.AddWithValue("@MaSP", maSP);
                                    cmdUpdateGia.ExecuteNonQuery();

                                    coSanPhamDuocCapNhatGia = true;

                                    // Ghi log cập nhật giá
                                    LogHelper.WriteLog($"Cập nhật giá sản phẩm {maSP}: {giaBanHienTai:N0} → {giaBanDeXuat:N0}");
                                }
                                // else: Giá nhập thấp hơn → KHÔNG LÀM GÌ CẢ (giữ nguyên giá bán cũ)
                            }
                            // ====================================================
                        }

                        // BƯỚC 4: COMMIT TRANSACTION
                        transaction.Commit();

                        string thongBao = "Lưu phiếu nhập thành công!\n\n" +
                            "✓ Đã cập nhật tồn kho\n" +
                            "✓ Đã tạo lô hàng\n";

                        if (coSanPhamDuocCapNhatGia)
                        {
                            thongBao += "✓ Giá bán đã được điều chỉnh tăng\n";
                        }
                        else
                        {
                            thongBao += "✓ Giá bán giữ nguyên (giá nhập thấp hơn giá bán hiện tại)\n";
                        }

                        MessageBox.Show(thongBao, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // BƯỚC 5: QUAN TRỌNG - Cập nhật Form Sản phẩm nếu đang mở
                        FormSanPham formSP = Application.OpenForms.OfType<FormSanPham>().FirstOrDefault();
                        if (formSP != null)
                        {
                            formSP.RefreshData();
                            MessageBox.Show("Đã cập nhật dữ liệu mới nhất trên Form Sản phẩm!\n" +
                                "(Giá bán, tồn kho, lô hàng)",
                                "Cập nhật thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // BƯỚC 6: Reset form để nhập phiếu mới
                        ResetForm();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        if (ex.Number == 2627) // Violation of PRIMARY KEY constraint
                        {
                            MessageBox.Show("Mã phiếu nhập đã tồn tại! Vui lòng nhập mã khác.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtMaPN.Focus();
                        }
                        else if (ex.Number == 547) // Foreign key constraint
                        {
                            MessageBox.Show("Lỗi khóa ngoại: Dữ liệu không tồn tại trong hệ thống.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi SQL khi lưu phiếu nhập: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        LogHelper.WriteLog("Lỗi SQL khi lưu phiếu nhập", ex);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogHelper.WriteLog("Lỗi hệ thống khi lưu phiếu nhập", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLog("Lỗi tổng quát khi lưu phiếu", ex);
            }
        }

        private void btnHuyPhieu_Click(object sender, EventArgs e)
        {
            if (dtChiTiet.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn hủy phiếu nhập này?\n" +
                    "Tất cả sản phẩm đã thêm sẽ bị mất!",
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ResetForm();
                }
            }
            else
            {
                ResetForm();
            }
        }

        private void ResetForm()
        {
            // Giữ lại mã nhân viên nếu có
            string maNV = cboNhanVien.SelectedValue?.ToString() ?? _maNVHienTai;

            // Reset dữ liệu
            dtChiTiet.Clear();
            tongTienPhieu = 0;
            lblTongTien.Text = "0 đ";
            lblTongTien.ForeColor = Color.Black;
            txtGhiChu.Clear();
            cboNhaCungCap.SelectedIndex = -1;
            cboSanPham.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtGiaNhap.Clear();
            lblTonKho.Text = "Tồn kho: 0";
            dtpNgayNhap.Value = DateTime.Now;

            // Tạo mã phiếu mới
            TaoMaPhieuTuDong();

            // Chọn lại nhân viên
            if (!string.IsNullOrEmpty(maNV))
            {
                try
                {
                    cboNhanVien.SelectedValue = maNV;
                }
                catch
                {
                    cboNhanVien.SelectedIndex = -1;
                }
            }

            // Xóa lỗi
            errorProvider1.Clear();

            // Focus
            cboNhaCungCap.Focus();
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Chỉ được nhập số!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Chỉ được nhập số và dấu chấm!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtSoLuong, "");
        }

        private void txtGiaNhap_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtGiaNhap, "");

            // Tự động định dạng số
            if (!string.IsNullOrEmpty(txtGiaNhap.Text))
            {
                try
                {
                    string text = txtGiaNhap.Text.Replace(",", "");
                    if (decimal.TryParse(text, out decimal value))
                    {
                        txtGiaNhap.TextChanged -= txtGiaNhap_TextChanged;
                        txtGiaNhap.Text = value.ToString("N0");
                        txtGiaNhap.TextChanged += txtGiaNhap_TextChanged;
                        txtGiaNhap.Select(txtGiaNhap.Text.Length, 0);
                    }
                }
                catch
                {
                    // Bỏ qua lỗi
                }
            }
        }

        private void txtMaPN_TextChanged(object sender, EventArgs e)
        {
            txtMaPN.CharacterCasing = CharacterCasing.Upper;
        }

        private void FormPhieuNhap_KeyDown(object sender, KeyEventArgs e)
        {
            // Shortcut keys
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnLuuPhieu_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                btnXoaSP_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                ResetForm();
            }
        }

        // PHƯƠNG THỨC HỖ TRỢ ĐỂ TEST
        public void TestCapNhatGia()
        {
            // Phương thức test để kiểm tra cập nhật Form Sản phẩm
            FormSanPham formSP = Application.OpenForms.OfType<FormSanPham>().FirstOrDefault();
            if (formSP != null)
            {
                formSP.RefreshData();
                MessageBox.Show("Test: Đã gọi RefreshData() trên Form Sản phẩm");
            }
            else
            {
                MessageBox.Show("Form Sản phẩm chưa được mở");
            }
        }
    }

    // Class LogHelper để ghi log
    public static class LogHelper
    {
        public static void WriteLog(string message, Exception ex = null)
        {
            try
            {
                string logPath = Application.StartupPath + "\\Logs\\";

                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                string logFile = logPath + $"PhieuNhap_{DateTime.Now:yyyyMMdd}.log";

                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
                    if (ex != null)
                    {
                        sw.WriteLine($"Exception: {ex.Message}");
                        sw.WriteLine($"StackTrace: {ex.StackTrace}");
                    }
                    sw.WriteLine(new string('-', 50));
                }
            }
            catch
            {
                // Không làm gì nếu không ghi được log
            }
        }
    }
}