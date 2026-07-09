using DA_QLShopQuanAo.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormSanPham : Form
    {
        // 1. CẤU HÌNH KẾT NỐI
        string connectionString = DatabaseHelper.GetConnectionString();
        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dt;

        // Biến cờ để theo dõi trạng thái load dữ liệu
        private bool _isDataLoaded = false;

        // 2. KHỞI TẠO
        public FormSanPham()
        {
            InitializeComponent();
        }

        private void FormSanPham_Load(object sender, EventArgs e)
        {
            LoadTheme();      // Load màu sắc giao diện
            LoadComboBox();   // Load dữ liệu cho ComboBox
            LoadData();       // Load dữ liệu lên lưới
            ResetValues();    // Đưa về trạng thái ban đầu
            _isDataLoaded = true;
        }

        // --- PHẦN 1: CÁC HÀM HỖ TRỢ (LOAD DATA, GIAO DIỆN) ---

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
        }

        // Hàm bật/tắt các nút dựa vào trạng thái
        void SetButtonState(bool status)
        {
            btnThem.Enabled = !status;
            btnLuu.Enabled = true;
            btnSua.Enabled = status;
            btnXoa.Enabled = status;
        }

        void LoadComboBox()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // Load Thể Loại
                string query = "SELECT MaTheLoai, TenTheLoai FROM TheLoai";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dtTheLoai = new DataTable();
                da.Fill(dtTheLoai);

                cboTheLoai.DataSource = dtTheLoai;
                cboTheLoai.DisplayMember = "TenTheLoai";
                cboTheLoai.ValueMember = "MaTheLoai";
                cboTheLoai.SelectedIndex = -1;

                // Load Xuất Xứ (Hardcode)
                cboXuatXu.Items.Clear();
                string[] xuatXuList = { "Việt Nam", "Trung Quốc", "Hàn Quốc", "Nhật Bản", "Mỹ", "Châu Âu" };
                cboXuatXu.Items.AddRange(xuatXuList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ComboBox: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        void LoadData()
        {
            try
            {
                if (conn == null)
                    conn = new SqlConnection(connectionString);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                // JOIN bảng để lấy Tên Thể Loại
                string query = @"
                    SELECT s.MaSP, s.TenSP, t.TenTheLoai, s.MaTheLoai, 
                           s.ThuongHieu, s.MauSac, s.ChatLieu, s.XuatXu, s.GiaBan, s.SoLuongTon 
                    FROM SanPham s 
                    JOIN TheLoai t ON s.MaTheLoai = t.MaTheLoai
                    ORDER BY s.MaSP";

                adapter = new SqlDataAdapter(query, conn);
                dt = new DataTable();
                adapter.Fill(dt);

                // Thiết lập khóa chính để tìm kiếm/sửa/xóa trên DataTable
                dt.PrimaryKey = new DataColumn[] { dt.Columns["MaSP"] };

                dgvSanPham.DataSource = dt;

                // Cấu hình hiển thị cột
                dgvSanPham.Columns["MaTheLoai"].Visible = false;
                dgvSanPham.Columns["TenTheLoai"].HeaderText = "Thể Loại";
                dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvSanPham.Columns["ThuongHieu"].HeaderText = "Thương Hiệu";
                dgvSanPham.Columns["MauSac"].HeaderText = "Màu Sắc";
                dgvSanPham.Columns["ChatLieu"].HeaderText = "Chất Liệu";
                dgvSanPham.Columns["XuatXu"].HeaderText = "Xuất Xứ";
                dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
                dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số Lượng";

                // Định dạng cột
                dgvSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dgvSanPham.Columns["GiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSanPham.Columns["SoLuongTon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Đẩy cột Thể loại lên vị trí dễ nhìn
                dgvSanPham.Columns["TenTheLoai"].DisplayIndex = 2;

                // Cập nhật thông tin
                lblTongSoSP.Text = $"Tổng số: {dt.Rows.Count} sản phẩm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ======== PHƯƠNG THỨC QUAN TRỌNG: REFRESH DỮ LIỆU ========
        public void RefreshData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Lưu vị trí dòng đang chọn (nếu có)
                string selectedMaSP = null;
                if (dgvSanPham.CurrentRow != null && dgvSanPham.CurrentRow.Index >= 0)
                {
                    selectedMaSP = dgvSanPham.CurrentRow.Cells["MaSP"].Value?.ToString();
                }

                // RELOAD hoàn toàn từ CSDL
                if (conn == null)
                    conn = new SqlConnection(connectionString);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = @"
                    SELECT s.MaSP, s.TenSP, t.TenTheLoai, s.MaTheLoai, 
                           s.ThuongHieu, s.MauSac, s.ChatLieu, s.XuatXu, s.GiaBan, s.SoLuongTon 
                    FROM SanPham s 
                    JOIN TheLoai t ON s.MaTheLoai = t.MaTheLoai
                    ORDER BY s.MaSP";

                adapter = new SqlDataAdapter(query, conn);
                DataTable dtNew = new DataTable();
                adapter.Fill(dtNew);

                // Gán Primary Key mới - SỬA LỖI Ở ĐÂY
                dtNew.PrimaryKey = new DataColumn[] { dtNew.Columns["MaSP"] };

                // Thay thế DataTable cũ bằng DataTable mới
                dt = dtNew;
                dgvSanPham.DataSource = dt;

                // Chọn lại dòng cũ (nếu có)
                if (!string.IsNullOrEmpty(selectedMaSP))
                {
                    foreach (DataGridViewRow row in dgvSanPham.Rows)
                    {
                        if (row.Cells["MaSP"].Value?.ToString() == selectedMaSP)
                        {
                            dgvSanPham.ClearSelection();
                            row.Selected = true;
                            dgvSanPham.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }

                // Cập nhật thông tin
                lblTongSoSP.Text = $"Tổng số: {dt.Rows.Count} sản phẩm";

                MessageBox.Show("Đã cập nhật dữ liệu mới nhất từ CSDL!\n\n" +
                    "✓ Giá bán mới nhất\n" +
                    "✓ Số lượng tồn kho\n" +
                    "✓ Thông tin sản phẩm",
                    "Cập nhật thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi refresh: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        void ResetValues()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            cboTheLoai.SelectedIndex = -1;
            txtThuongHieu.Text = "";
            txtMauSac.Text = "";
            txtChatLieu.Text = "";
            cboXuatXu.Text = "";
            txtGiaBan.Text = "";
            

            txtMaSP.Enabled = true;
            txtMaSP.Focus();

            // Xóa báo lỗi nếu có
            if (errorProvider1 != null)
                errorProvider1.Clear();

            // Reset trạng thái nút
            SetButtonState(false);
            btnThem.Enabled = true;
        }

        // --- PHẦN 2: XỬ LÝ SỰ KIỆN NHẬP LIỆU (VALIDATION) ---

        // THÊM PHƯƠNG THỨC NÀY
        private void txtGiaBan_TextChanged(object sender, EventArgs e)
        {
            // Xóa lỗi nếu có
            errorProvider1.SetError(txtGiaBan, "");
            txtGiaBan.ForeColor = Color.Black;
        }

        private void txtGiaBan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                errorProvider1.SetError(txtGiaBan, "Chỉ được nhập số và dấu chấm!");
            }
            else
            {
                errorProvider1.SetError(txtGiaBan, "");
            }
        }

        

        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
            txtMaSP.CharacterCasing = CharacterCasing.Upper;

            if (txtMaSP.Text.Length > 0 && !txtMaSP.Text.StartsWith("SP"))
            {
                txtMaSP.ForeColor = Color.Red;
                errorProvider1.SetError(txtMaSP, "Mã sản phẩm phải bắt đầu bằng 'SP' (Ví dụ: SP01)");
            }
            else
            {
                txtMaSP.ForeColor = Color.Black;
                errorProvider1.SetError(txtMaSP, "");
            }
        }

        // --- PHẦN 3: CÁC NÚT CHỨC NĂNG (CRUD) ---

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString();

                if (row.Cells["MaTheLoai"].Value != DBNull.Value)
                {
                    cboTheLoai.SelectedValue = row.Cells["MaTheLoai"].Value.ToString();
                }

                txtThuongHieu.Text = row.Cells["ThuongHieu"].Value?.ToString() ?? "";
                txtMauSac.Text = row.Cells["MauSac"].Value?.ToString() ?? "";
                txtChatLieu.Text = row.Cells["ChatLieu"].Value?.ToString() ?? "";
                cboXuatXu.Text = row.Cells["XuatXu"].Value?.ToString() ?? "";
                txtGiaBan.Text = Convert.ToDecimal(row.Cells["GiaBan"].Value).ToString("N0");
                

                txtMaSP.Enabled = false;
                SetButtonState(true);
            }
        }

        private void dgvSanPham_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null)
                ResetValues();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Chưa nhập Mã SP!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSP.Focus();
                return;
            }

            if (!txtMaSP.Text.StartsWith("SP"))
            {
                MessageBox.Show("Mã SP phải bắt đầu bằng 'SP'!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSP.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtTenSP.Text))
            {
                MessageBox.Show("Chưa nhập Tên SP!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return;
            }

            if (cboTheLoai.SelectedValue == null)
            {
                MessageBox.Show("Chưa chọn Thể loại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTheLoai.Focus();
                return;
            }

            if (dt.Rows.Find(txtMaSP.Text) != null)
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow row = dt.NewRow();
                row["MaSP"] = txtMaSP.Text;
                row["MaTheLoai"] = cboTheLoai.SelectedValue;
                row["TenTheLoai"] = cboTheLoai.Text;
                row["TenSP"] = txtTenSP.Text;
                row["ThuongHieu"] = txtThuongHieu.Text;
                row["MauSac"] = txtMauSac.Text;
                row["ChatLieu"] = txtChatLieu.Text;
                row["XuatXu"] = cboXuatXu.Text;

                decimal giaBan = 0;
                decimal.TryParse(txtGiaBan.Text.Replace(",", ""), out giaBan);
                int soLuong = 0;
                

                row["GiaBan"] = giaBan;
                row["SoLuongTon"] = soLuong;

                dt.Rows.Add(row);

                MessageBox.Show("Đã thêm vào danh sách chờ. Bấm 'Lưu' để cập nhật CSDL.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetValues();
                lblTongSoSP.Text = $"Tổng số: {dt.Rows.Count} sản phẩm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DataRow row = dt.Rows.Find(txtMaSP.Text);
            if (row != null)
            {
                row.BeginEdit();
                row["MaTheLoai"] = cboTheLoai.SelectedValue;
                row["TenTheLoai"] = cboTheLoai.Text;
                row["TenSP"] = txtTenSP.Text;
                row["ThuongHieu"] = txtThuongHieu.Text;
                row["MauSac"] = txtMauSac.Text;
                row["ChatLieu"] = txtChatLieu.Text;
                row["XuatXu"] = cboXuatXu.Text;

                decimal giaBan = 0;
                decimal.TryParse(txtGiaBan.Text.Replace(",", ""), out giaBan);
                int soLuong = 0;
               

                row["GiaBan"] = giaBan;
                row["SoLuongTon"] = soLuong;
                row.EndEdit();

                MessageBox.Show("Đã sửa trên lưới. Bấm 'Lưu' để cập nhật CSDL.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetValues();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataRow row = dt.Rows.Find(txtMaSP.Text);
            if (row != null)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa sản phẩm này?\n\n" +
                    $"Mã: {txtMaSP.Text}\n" +
                    $"Tên: {txtTenSP.Text}",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    row.Delete();
                    MessageBox.Show("Đã xóa khỏi danh sách. Bấm 'Lưu' để cập nhật CSDL.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetValues();
                    lblTongSoSP.Text = $"Tổng số: {dt.Rows.Count} sản phẩm";
                }
            }
        }

        // --- PHẦN 4: LƯU VÀO CSDL ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (dt.GetChanges() == null)
            {
                MessageBox.Show("Không có thay đổi nào để lưu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Added)
                        {
                            string sql = @"INSERT INTO SanPham 
                                (MaSP, MaTheLoai, TenSP, ThuongHieu, MauSac, ChatLieu, XuatXu, GiaBan, SoLuongTon) 
                                VALUES (@ma, @loai, @ten, @hieu, @mau, @chat, @xuat, @gia, @sl)";

                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", row["MaSP"]);
                            cmd.Parameters.AddWithValue("@loai", row["MaTheLoai"]);
                            cmd.Parameters.AddWithValue("@ten", row["TenSP"]);
                            cmd.Parameters.AddWithValue("@hieu", row["ThuongHieu"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@mau", row["MauSac"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@chat", row["ChatLieu"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@xuat", row["XuatXu"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@gia", row["GiaBan"]);
                            cmd.Parameters.AddWithValue("@sl", row["SoLuongTon"]);
                            cmd.ExecuteNonQuery();
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string sql = @"UPDATE SanPham SET 
                                MaTheLoai=@loai, TenSP=@ten, ThuongHieu=@hieu, 
                                MauSac=@mau, ChatLieu=@chat, XuatXu=@xuat, 
                                GiaBan=@gia, SoLuongTon=@sl 
                                WHERE MaSP=@ma";

                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", row["MaSP"]);
                            cmd.Parameters.AddWithValue("@loai", row["MaTheLoai"]);
                            cmd.Parameters.AddWithValue("@ten", row["TenSP"]);
                            cmd.Parameters.AddWithValue("@hieu", row["ThuongHieu"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@mau", row["MauSac"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@chat", row["ChatLieu"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@xuat", row["XuatXu"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@gia", row["GiaBan"]);
                            cmd.Parameters.AddWithValue("@sl", row["SoLuongTon"]);
                            cmd.ExecuteNonQuery();
                        }
                        else if (row.RowState == DataRowState.Deleted)
                        {
                            string maSP = row["MaSP", DataRowVersion.Original].ToString();
                            string sql = "DELETE FROM SanPham WHERE MaSP = @ma";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", maSP);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    dt.AcceptChanges();

                    MessageBox.Show("Lưu dữ liệu thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();

                    if (ex.Number == 547) // Foreign key constraint
                    {
                        MessageBox.Show("Không thể xóa sản phẩm vì đã có dữ liệu liên quan!\n" +
                            "(Đã bán, đã nhập hàng, hoặc trong đơn hàng)", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData(); // Load lại để tránh sai lệch
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData(); // Load lại để tránh sai lệch
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                dt.DefaultView.RowFilter = "";
                lblKetQuaTim.Text = "";
            }
            else
            {
                string filter = $"MaSP LIKE '%{tuKhoa}%' OR TenSP LIKE '%{tuKhoa}%' OR ThuongHieu LIKE '%{tuKhoa}%'";
                dt.DefaultView.RowFilter = filter;

                int count = dt.DefaultView.Count;
                lblKetQuaTim.Text = $"Tìm thấy {count} kết quả";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Sử dụng phương thức RefreshData
            RefreshData();
        }

        // Thêm phương thức này nếu muốn Form tự động refresh khi được active lại
        private void FormSanPham_Activated(object sender, EventArgs e)
        {
            if (_isDataLoaded)
            {
                // Có thể thêm logic để hỏi người dùng có muốn refresh không
                // Hoặc tự động refresh sau một khoảng thời gian
            }
        }

        private void txtTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter key
            {
                btnTim_Click(sender, e);
            }
        }
    }
}