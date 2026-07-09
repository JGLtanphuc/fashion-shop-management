using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DA_QLShopQuanAo.Helpers;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormNhanVien : Form
    {
        string connectionString = DatabaseHelper.GetConnectionString();
        DataTable dt;

        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
            ResetValues();
            SetButtonState(false);
        }

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

        void SetButtonState(bool status)
        {
            btnThem.Enabled = !status;
            btnLuu.Enabled = true;
            btnSua.Enabled = status;
            btnXoa.Enabled = status;
        }

        // THÊM ComboBoxCell cho DataGridView
        private DataGridViewComboBoxColumn CreateQuyenComboBoxColumn()
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            col.Name = "Quyen";
            col.HeaderText = "Quyền";
            col.DataPropertyName = "Quyen"; // Map với column trong DataTable
            col.Width = 100;

            // Chỉ cho chọn 2 giá trị
            col.Items.AddRange("Admin", "Nhân viên");
            col.DefaultCellStyle.NullValue = "Nhân viên"; // Giá trị mặc định

            return col;
        }

        void LoadData()
        {
            try
            {
                // Sửa query: Lấy Quyen thay vì TenDangNhap và MatKhau
                string query = @"
                    SELECT n.MaNV, n.HoTen, n.SDT, n.DiaChi, n.Email, n.ChucVu, 
                           t.TenDangNhap, t.Quyen 
                    FROM NhanVien n 
                    LEFT JOIN TaiKhoan t ON n.MaNV = t.MaNV";

                dt = DatabaseHelper.ExecuteQuery(query);

                // Thiết lập khóa chính
                DataColumn[] keys = new DataColumn[1];
                keys[0] = dt.Columns["MaNV"];
                dt.PrimaryKey = keys;

                // Xóa các column cũ
                dgvNhanVien.Columns.Clear();

                // Tạo các column
                dgvNhanVien.Columns.Add("MaNV", "Mã NV");
                dgvNhanVien.Columns["MaNV"].DataPropertyName = "MaNV";
                dgvNhanVien.Columns["MaNV"].ReadOnly = true;

                dgvNhanVien.Columns.Add("HoTen", "Họ Tên");
                dgvNhanVien.Columns["HoTen"].DataPropertyName = "HoTen";

                dgvNhanVien.Columns.Add("SDT", "SĐT");
                dgvNhanVien.Columns["SDT"].DataPropertyName = "SDT";

                dgvNhanVien.Columns.Add("DiaChi", "Địa Chỉ");
                dgvNhanVien.Columns["DiaChi"].DataPropertyName = "DiaChi";

                dgvNhanVien.Columns.Add("Email", "Email");
                dgvNhanVien.Columns["Email"].DataPropertyName = "Email";

                // THÊM COLUMN QUYỀN (ComboBox)
                DataGridViewComboBoxColumn colQuyen = CreateQuyenComboBoxColumn();
                dgvNhanVien.Columns.Add(colQuyen);

                dgvNhanVien.Columns.Add("TenDangNhap", "Tài Khoản");
                dgvNhanVien.Columns["TenDangNhap"].DataPropertyName = "TenDangNhap";
                dgvNhanVien.Columns["TenDangNhap"].ReadOnly = true;

                dgvNhanVien.AllowUserToAddRows = false;

                dgvNhanVien.DataSource = dt;
                dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Xử lý giá trị null trong column Quyen - SỬA LẠI
                foreach (DataGridViewRow row in dgvNhanVien.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if (row.Cells["Quyen"].Value == DBNull.Value ||
                            string.IsNullOrEmpty(row.Cells["Quyen"].Value?.ToString()))
                        {
                            row.Cells["Quyen"].Value = "Nhân viên";
                        }
                        else
                        {
                            string quyenValue = row.Cells["Quyen"].Value.ToString();
                            // Đảm bảo giá trị chỉ là "Admin" hoặc "Nhân viên"
                            if (quyenValue != "Admin" && quyenValue != "Nhân viên")
                            {
                                row.Cells["Quyen"].Value = "Nhân viên";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        void ResetValues()
        {
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            cboQuyen.SelectedIndex = 0; // Mặc định là "Nhân viên"

            txtMaNV.Enabled = true;
            txtMaNV.Focus();

            if (errorProvider1 != null) errorProvider1.Clear();
            SetButtonState(false);
        }

        // --- VALIDATION ---
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Số điện thoại chỉ được nhập số!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            txtMaNV.CharacterCasing = CharacterCasing.Upper;

            if (txtMaNV.Text.Length > 0 && !txtMaNV.Text.StartsWith("MNV"))
            {
                txtMaNV.ForeColor = Color.Red;
                if (errorProvider1 != null)
                    errorProvider1.SetError(txtMaNV, "Mã nhân viên phải bắt đầu bằng 'MNV'");
            }
            else
            {
                txtMaNV.ForeColor = Color.Black;
                if (errorProvider1 != null)
                    errorProvider1.SetError(txtMaNV, "");
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (txtEmail.Text.Length > 0)
            {
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    txtEmail.ForeColor = Color.Red;
                    if (errorProvider1 != null)
                        errorProvider1.SetError(txtEmail, "Email không đúng định dạng!");
                }
                else
                {
                    txtEmail.ForeColor = Color.Black;
                    if (errorProvider1 != null)
                        errorProvider1.SetError(txtEmail, "");
                }
            }
            else
            {
                if (errorProvider1 != null) errorProvider1.SetError(txtEmail, "");
            }
        }

        // --- XỬ LÝ SỰ KIỆN ---
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanVien.Rows.Count)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString() ?? "";
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";
                txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtTaiKhoan.Text = row.Cells["TenDangNhap"].Value?.ToString() ?? "";

                // Lấy giá trị Quyền từ ComboBox
                if (row.Cells["Quyen"].Value != null)
                {
                    string quyenValue = row.Cells["Quyen"].Value.ToString();
                    if (quyenValue == "Admin" || quyenValue == "Nhân viên")
                    {
                        cboQuyen.Text = quyenValue;
                    }
                    else
                    {
                        cboQuyen.SelectedIndex = 0;
                    }
                }
                else
                {
                    cboQuyen.SelectedIndex = 0;
                }

                txtMaNV.Enabled = false;
                SetButtonState(true);
            }
        }

        private void dgvNhanVien_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null) ResetValues();
        }
        bool IsValidSDT(string sdt)
        {
            // Kiểm tra SĐT phải có đúng 10 ký tự
            return sdt.Length == 10;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập Số Điện Thoại!"); txtSDT.Focus(); return; }
            if (!IsValidSDT(txtSDT.Text)) { MessageBox.Show("Số Điện Thoại phải có đúng 10 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSDT.Focus(); return; }
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Chưa nhập Mã NV!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                return;
            }
            if (!txtMaNV.Text.StartsWith("MNV"))
            {
                MessageBox.Show("Mã NV phải bắt đầu bằng 'MNV'!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                MessageBox.Show("Chưa nhập Họ tên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (dt.Rows.Find(txtMaNV.Text) != null)
            {
                MessageBox.Show("Mã nhân viên này đã tồn tại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow row = dt.NewRow();
            row["MaNV"] = txtMaNV.Text;
            row["HoTen"] = txtHoTen.Text;
            row["SDT"] = txtSDT.Text;
            row["DiaChi"] = txtDiaChi.Text;
            row["Email"] = txtEmail.Text;
            row["ChucVu"] = "Nhân viên";
            row["TenDangNhap"] = txtTaiKhoan.Text;
            row["Quyen"] = cboQuyen.Text; // Lưu quyền từ ComboBox

            dt.Rows.Add(row);
            MessageBox.Show("Đã thêm vào danh sách chờ. Bấm 'Lưu' để cập nhật CSDL.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetValues();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập Số Điện Thoại!"); txtSDT.Focus(); return; }
            if (!IsValidSDT(txtSDT.Text)) { MessageBox.Show("Số Điện Thoại phải có đúng 10 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSDT.Focus(); return; }
            DataRow row = dt.Rows.Find(txtMaNV.Text);
            if (row != null)
            {
                row["HoTen"] = txtHoTen.Text;
                row["SDT"] = txtSDT.Text;
                row["DiaChi"] = txtDiaChi.Text;
                row["Email"] = txtEmail.Text;
                row["TenDangNhap"] = txtTaiKhoan.Text;
                row["Quyen"] = cboQuyen.Text; // Cập nhật quyền

                MessageBox.Show("Đã sửa trên lưới. Bấm 'Lưu' để cập nhật CSDL.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValues();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Tìm dòng dữ liệu theo Mã NV
            DataRow row = dt.Rows.Find(txtMaNV.Text);

            if (row != null)
            {
                // --- BỔ SUNG KIỂM TRA QUYỀN ADMIN ---
                string quyen = row["Quyen"] != DBNull.Value ? row["Quyen"].ToString() : "";

                if (quyen == "Admin")
                {
                    MessageBox.Show("Không được phép xóa tài khoản Quản trị viên (Admin)!",
                        "Cấm xóa", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return; // Dừng lại ngay lập tức, không chạy code xóa bên dưới
                }
                // ------------------------------------

                // Nếu không phải Admin thì mới hiện hộp thoại xác nhận xóa
                if (MessageBox.Show("Bạn chắc chắn muốn xóa nhân viên này?\n(Tài khoản liên quan cũng sẽ bị xóa)",
                    "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    row.Delete();
                    ResetValues();
                }
            }
        }

        // --- LƯU DATABASE ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        // TH1: THÊM MỚI
                        if (row.RowState == DataRowState.Added)
                        {
                            // 1. Thêm Nhân viên
                            string sqlNV = @"INSERT INTO NhanVien (MaNV, HoTen, SDT, DiaChi, Email, ChucVu) 
                                          VALUES (@ma, @ten, @sdt, @dc, @email, @cv)";
                            SqlCommand cmdNV = new SqlCommand(sqlNV, conn, transaction);
                            cmdNV.Parameters.AddWithValue("@ma", row["MaNV"]);
                            cmdNV.Parameters.AddWithValue("@ten", row["HoTen"]);
                            cmdNV.Parameters.AddWithValue("@sdt", row["SDT"]);
                            cmdNV.Parameters.AddWithValue("@dc", row["DiaChi"]);
                            cmdNV.Parameters.AddWithValue("@email", row["Email"]);
                            cmdNV.Parameters.AddWithValue("@cv", "Nhân viên");
                            cmdNV.ExecuteNonQuery();

                            // 2. Thêm Tài khoản (nếu có tên đăng nhập)
                            if (row["TenDangNhap"] != DBNull.Value &&
                                !string.IsNullOrEmpty(row["TenDangNhap"].ToString()))
                            {
                                string sqlTK = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV, Quyen) 
                                              VALUES (@user, @pass, @ma, @quyen)";
                                SqlCommand cmdTK = new SqlCommand(sqlTK, conn, transaction);
                                cmdTK.Parameters.AddWithValue("@user", row["TenDangNhap"]);
                                cmdTK.Parameters.AddWithValue("@pass", "123456"); // Mật khẩu mặc định
                                cmdTK.Parameters.AddWithValue("@ma", row["MaNV"]);
                                cmdTK.Parameters.AddWithValue("@quyen",
                                    row["Quyen"] != DBNull.Value ? row["Quyen"] : "Nhân viên");
                                cmdTK.ExecuteNonQuery();
                            }
                        }

                        // TH2: SỬA ĐỔI
                        else if (row.RowState == DataRowState.Modified)
                        {
                            // 1. Cập nhật Nhân viên
                            string sqlNV = @"UPDATE NhanVien SET HoTen=@ten, SDT=@sdt, DiaChi=@dc, Email=@email 
                                          WHERE MaNV=@ma";
                            SqlCommand cmdNV = new SqlCommand(sqlNV, conn, transaction);
                            cmdNV.Parameters.AddWithValue("@ma", row["MaNV"]);
                            cmdNV.Parameters.AddWithValue("@ten", row["HoTen"]);
                            cmdNV.Parameters.AddWithValue("@sdt", row["SDT"]);
                            cmdNV.Parameters.AddWithValue("@dc", row["DiaChi"]);
                            cmdNV.Parameters.AddWithValue("@email", row["Email"]);
                            cmdNV.ExecuteNonQuery();

                            // 2. Xử lý Tài khoản
                            string tenDangNhap = row["TenDangNhap"] != DBNull.Value ?
                                                row["TenDangNhap"].ToString() : "";
                            string quyen = row["Quyen"] != DBNull.Value ?
                                          row["Quyen"].ToString() : "Nhân viên";

                            if (!string.IsNullOrEmpty(tenDangNhap))
                            {
                                // Kiểm tra đã có tài khoản chưa
                                string checkTK = "SELECT COUNT(*) FROM TaiKhoan WHERE MaNV = @ma";
                                SqlCommand cmdCheck = new SqlCommand(checkTK, conn, transaction);
                                cmdCheck.Parameters.AddWithValue("@ma", row["MaNV"]);
                                int count = (int)cmdCheck.ExecuteScalar();

                                if (count > 0) // Đã có -> Update Quyền
                                {
                                    string sqlTK = @"UPDATE TaiKhoan SET Quyen=@quyen 
                                                  WHERE MaNV=@ma";
                                    SqlCommand cmdTK = new SqlCommand(sqlTK, conn, transaction);
                                    cmdTK.Parameters.AddWithValue("@quyen", quyen);
                                    cmdTK.Parameters.AddWithValue("@ma", row["MaNV"]);
                                    cmdTK.ExecuteNonQuery();
                                }
                                else // Chưa có -> Insert
                                {
                                    string sqlTK = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV, Quyen) 
                                                  VALUES (@user, @pass, @ma, @quyen)";
                                    SqlCommand cmdTK = new SqlCommand(sqlTK, conn, transaction);
                                    cmdTK.Parameters.AddWithValue("@user", tenDangNhap);
                                    cmdTK.Parameters.AddWithValue("@pass", "123456");
                                    cmdTK.Parameters.AddWithValue("@ma", row["MaNV"]);
                                    cmdTK.Parameters.AddWithValue("@quyen", quyen);
                                    cmdTK.ExecuteNonQuery();
                                }
                            }
                        }

                        // TH3: XÓA
                        else if (row.RowState == DataRowState.Deleted)
                        {
                            string maNV = row["MaNV", DataRowVersion.Original].ToString();

                            // 1. Xóa Tài khoản trước
                            string sqlTK = "DELETE FROM TaiKhoan WHERE MaNV = @ma";
                            SqlCommand cmdTK = new SqlCommand(sqlTK, conn, transaction);
                            cmdTK.Parameters.AddWithValue("@ma", maNV);
                            cmdTK.ExecuteNonQuery();

                            // 2. Xóa Nhân viên sau
                            string sqlNV = "DELETE FROM NhanVien WHERE MaNV = @ma";
                            SqlCommand cmdNV = new SqlCommand(sqlNV, conn, transaction);
                            cmdNV.Parameters.AddWithValue("@ma", maNV);
                            cmdNV.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.AcceptChanges();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
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
                dt.DefaultView.RowFilter = $"MaNV LIKE '%{tuKhoa}%' OR HoTen LIKE '%{tuKhoa}%'";
            }
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Không cần xử lý
        }

        // Sự kiện khi chọn quyền trong DataGridView
        private void dgvNhanVien_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvNhanVien.Columns["Quyen"].Index)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                string quyen = row.Cells["Quyen"].Value?.ToString();

                // Kiểm tra chỉ được chọn 2 giá trị
                if (quyen != "Admin" && quyen != "Nhân viên")
                {
                    MessageBox.Show("Chỉ được chọn 'Admin' hoặc 'Nhân viên'!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["Quyen"].Value = "Nhân viên";
                }
            }
        }

        // THÊM PHƯƠNG THỨC XỬ LÝ LỖI DATAGRIDVIEW
        private void dgvNhanVien_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Chỉ xử lý lỗi cho column Quyen
            if (dgvNhanVien.Columns[e.ColumnIndex].Name == "Quyen")
            {
                MessageBox.Show("Giá trị quyền không hợp lệ. Chỉ được chọn 'Admin' hoặc 'Nhân viên'.", "Lỗi dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Đặt giá trị mặc định
                if (e.RowIndex >= 0 && e.RowIndex < dgvNhanVien.Rows.Count)
                {
                    dgvNhanVien.Rows[e.RowIndex].Cells["Quyen"].Value = "Nhân viên";
                }

                e.Cancel = true; // Ngăn lỗi hiển thị hộp thoại mặc định
            }
        }
    }
}