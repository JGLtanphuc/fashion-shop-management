using DA_QLShopQuanAo.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormKhachHang : Form
    {
        // 1. CẤU HÌNH KẾT NỐI
        string connectionString = DatabaseHelper.GetConnectionString();

        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dt;

        // Cần có ErrorProvider trong thiết kế Form (giả định đã có)
        // private ErrorProvider errorProvider1 = new ErrorProvider(); 

        public FormKhachHang()
        {
            InitializeComponent();
            // Đảm bảo ErrorProvider được khởi tạo nếu chưa có
            // errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void FormKhachHang_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
            ResetValues();
            SetButtonState(false); // Mặc định ẩn nút Sửa/Xóa
        }

        // --- PHẦN 1: GIAO DIỆN & HỖ TRỢ ---

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns is Button btn)
                {
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
        }

        // Hàm bật/tắt nút bấm
        void SetButtonState(bool status)
        {
            btnThem.Enabled = !status; // Đang chọn dòng thì khóa nút Thêm
            btnLuu.Enabled = true;     // Nút Lưu luôn mở

            btnSua.Enabled = status;   // Chỉ sáng khi chọn dòng
            btnXoa.Enabled = status;
        }

        void LoadData()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string query = "SELECT MaKH, HoTen, SDT, DiaChi FROM KhachHang";
                adapter = new SqlDataAdapter(query, conn);
                dt = new DataTable();
                adapter.Fill(dt);

                // Thiết lập khóa chính để tìm kiếm/sửa/xóa trên DataTable
                DataColumn[] keys = new DataColumn[1];
                keys[0] = dt.Columns["MaKH"];
                dt.PrimaryKey = keys;

                dgvKhachHang.DataSource = dt;

                // Đặt tên cột tiếng Việt
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã Khách Hàng";
                dgvKhachHang.Columns["HoTen"].HeaderText = "Họ Tên";
                dgvKhachHang.Columns["SDT"].HeaderText = "Số Điện Thoại";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa Chỉ";

                dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open) conn.Close();
            }
        }

        void ResetValues()
        {
            txtMaKH.Text = "";
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";

            txtMaKH.Enabled = true; // Cho phép nhập lại Mã
            txtMaKH.Focus();

            // Xóa lỗi cũ
            if (errorProvider1 != null) errorProvider1.Clear();

            // Reset nút
            SetButtonState(false);
        }

        // --- PHẦN 2: VALIDATION (KIỂM TRA DỮ LIỆU) ---

        // 1. Chỉ cho nhập số vào SĐT
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Số điện thoại phải là số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 2. Kiểm tra Mã Khách Hàng (Phải bắt đầu bằng MKH)
        private void txtMaKH_TextChanged(object sender, EventArgs e)
        {
            txtMaKH.CharacterCasing = CharacterCasing.Upper; // Tự động viết hoa

            if (txtMaKH.Text.Length > 0 && !txtMaKH.Text.StartsWith("MKH"))
            {
                txtMaKH.ForeColor = Color.Red;
                if (errorProvider1 != null)
                    errorProvider1.SetError(txtMaKH, "Mã khách hàng phải bắt đầu bằng 'MKH' (Ví dụ: MKH01)");
            }
            else
            {
                txtMaKH.ForeColor = Color.Black;
                if (errorProvider1 != null)
                    errorProvider1.SetError(txtMaKH, "");
            }
        }

        // 3. KIỂM TRA ĐỘ DÀI SĐT (BỔ SUNG)
        bool IsValidSDT(string sdt)
        {
            // Kiểm tra SĐT phải có đúng 10 ký tự
            return sdt.Length == 10;
        }

        // --- PHẦN 3: XỬ LÝ SỰ KIỆN CLICK BẢNG ---

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

                txtMaKH.Text = row.Cells["MaKH"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtSDT.Text = row.Cells["SDT"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();

                txtMaKH.Enabled = false; // Không cho sửa Mã khi đang chọn

                SetButtonState(true); // Bật nút Sửa/Xóa
            }
        }

        // Click ra ngoài vùng dữ liệu để reset form
        private void dgvKhachHang_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null) ResetValues();
        }

        // --- PHẦN 4: CÁC NÚT CHỨC NĂNG (CRUD) ---

        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(txtMaKH.Text)) { MessageBox.Show("Vui lòng nhập Mã Khách Hàng!"); txtMaKH.Focus(); return; }
            if (!txtMaKH.Text.StartsWith("MKH")) { MessageBox.Show("Mã Khách Hàng sai quy định (phải là MKH...)!"); txtMaKH.Focus(); return; }
            if (string.IsNullOrEmpty(txtHoTen.Text)) { MessageBox.Show("Vui lòng nhập Họ Tên!"); txtHoTen.Focus(); return; }

            // **KIỂM TRA SĐT BỔ SUNG**
            if (string.IsNullOrEmpty(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập Số Điện Thoại!"); txtSDT.Focus(); return; }
            if (!IsValidSDT(txtSDT.Text)) { MessageBox.Show("Số Điện Thoại phải có đúng 10 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSDT.Focus(); return; }
            // -------------------------

            // 2. Kiểm tra trùng mã
            if (dt.Rows.Find(txtMaKH.Text) != null)
            {
                MessageBox.Show("Mã khách hàng này đã tồn tại!");
                return;
            }

            // 3. Thêm vào lưới
            try
            {
                DataRow row = dt.NewRow();
                row["MaKH"] = txtMaKH.Text;
                row["HoTen"] = txtHoTen.Text;
                row["SDT"] = txtSDT.Text;
                row["DiaChi"] = txtDiaChi.Text;

                dt.Rows.Add(row);
                MessageBox.Show("Đã thêm vào danh sách chờ. Bấm 'Lưu' để cập nhật CSDL.");
                ResetValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // **KIỂM TRA SĐT BỔ SUNG**
            if (string.IsNullOrEmpty(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập Số Điện Thoại!"); txtSDT.Focus(); return; }
            if (!IsValidSDT(txtSDT.Text)) { MessageBox.Show("Số Điện Thoại phải có đúng 10 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSDT.Focus(); return; }
            // -------------------------

            DataRow row = dt.Rows.Find(txtMaKH.Text);
            if (row != null)
            {
                row["HoTen"] = txtHoTen.Text;
                row["SDT"] = txtSDT.Text;
                row["DiaChi"] = txtDiaChi.Text;

                MessageBox.Show("Đã sửa trên lưới. Bấm 'Lưu' để cập nhật.");
                ResetValues();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataRow row = dt.Rows.Find(txtMaKH.Text);
            if (row != null)
            {
                if (MessageBox.Show("Bạn chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    row.Delete();
                    ResetValues();
                }
            }
        }

        // --- PHẦN 5: LƯU VÀO DATABASE ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Sử dụng SqlDataAdapter.Update thay vì vòng lặp thủ công (Tùy chọn, ở đây vẫn dùng thủ công để giữ cấu trúc ban đầu)
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Added)
                        {
                            string sql = "INSERT INTO KhachHang (MaKH, HoTen, SDT, DiaChi) VALUES (@ma, @ten, @sdt, @dc)";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", row["MaKH"]);
                            cmd.Parameters.AddWithValue("@ten", row["HoTen"]);
                            cmd.Parameters.AddWithValue("@sdt", row["SDT"] ?? (object)DBNull.Value); // Xử lý DBNull cho trường có thể null
                            cmd.Parameters.AddWithValue("@dc", row["DiaChi"] ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string sql = "UPDATE KhachHang SET HoTen=@ten, SDT=@sdt, DiaChi=@dc WHERE MaKH=@ma";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", row["MaKH"]); // Khóa chính
                            cmd.Parameters.AddWithValue("@ten", row["HoTen"]);
                            cmd.Parameters.AddWithValue("@sdt", row["SDT"] ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@dc", row["DiaChi"] ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                        else if (row.RowState == DataRowState.Deleted)
                        {
                            // Lấy giá trị gốc của dòng đã xóa
                            string maKH = row["MaKH", DataRowVersion.Original].ToString();

                            string sql = "DELETE FROM KhachHang WHERE MaKH = @ma";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", maKH);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Lưu dữ liệu thành công!");
                    dt.AcceptChanges(); // Đồng bộ trạng thái lưới
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi lưu (Có thể do mã khách hàng đang được sử dụng trong Đơn Hàng): " + ex.Message);
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
            }
            else
            {
                // Tìm theo Mã KH hoặc Họ Tên hoặc SDT
                dt.DefaultView.RowFilter = $"MaKH LIKE '%{tuKhoa}%' OR HoTen LIKE '%{tuKhoa}%' OR SDT LIKE '%{tuKhoa}%'";
            }
        }

        // --- PHẦN 6: TÌM KIẾM ---

    }
}