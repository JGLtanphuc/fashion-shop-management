using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DA_QLShopQuanAo.Helpers;
using DA_QLShopQuanAo.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormDangKy : Form
    {
        public FormDangKy()
        {
            InitializeComponent();
        }

        private void FormDangKy_Load(object sender, EventArgs e)
        {
            
            ResetValues();
            AutoGenerateMaNV();
            txtHoTen.Focus();
        }

       

        // Tự động sinh mã NV mới
        private void AutoGenerateMaNV()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaNV, 4, LEN(MaNV)-3) AS INT)), 0) FROM NhanVien WHERE MaNV LIKE 'MNV%'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    int maxNumber = (int)cmd.ExecuteScalar();

                    // Tạo mã NV: MNV + số tiếp theo (VD: MNV001, MNV002...)
                    string newMaNV = "MNV" + (maxNumber + 1).ToString("D3");
                    txtMaNV.Text = newMaNV;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sinh mã NV: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Reset form
        private void ResetValues()
        {
            // Xóa tất cả textbox
            txtHoTen.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtNhapLaiMatKhau.Clear();

            // Set mặc định
            txtChucVu.Text = "Nhân viên";

            errorProvider1.Clear();

            // Tạo lại mã NV mới
            AutoGenerateMaNV();
            txtHoTen.Focus();
        }

        // Kiểm tra dữ liệu
        private bool ValidateData()
        {
            // Kiểm tra Họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Kiểm tra SĐT
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (!Regex.IsMatch(txtSDT.Text, @"^\d{10,11}$"))
            {
                MessageBox.Show("Số điện thoại phải là 10-11 chữ số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            // Kiểm tra Email (không bắt buộc nhưng phải đúng định dạng nếu có)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    MessageBox.Show("Email không đúng định dạng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // Kiểm tra tên đăng nhập
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            if (txtTenDangNhap.Text.Length < 3)
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            // Kiểm tra trùng tên đăng nhập
            if (CheckUsernameExists(txtTenDangNhap.Text.Trim()))
            {
                MessageBox.Show("Tên đăng nhập này đã tồn tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            // Kiểm tra mật khẩu
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            // Kiểm tra nhập lại mật khẩu
            if (txtMatKhau.Text != txtNhapLaiMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhapLaiMatKhau.Focus();
                return false;
            }

            return true;
        }

        // Kiểm tra tên đăng nhập đã tồn tại chưa
        private bool CheckUsernameExists(string username)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @username";

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Nút Đăng Ký
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. THÊM NHÂN VIÊN
                    string sqlNhanVien = @"
                        INSERT INTO NhanVien (MaNV, HoTen, SDT, DiaChi, Email, ChucVu) 
                        VALUES (@maNV, @hoTen, @sdt, @diaChi, @email, @chucVu)";

                    SqlCommand cmdNV = new SqlCommand(sqlNhanVien, conn, transaction);
                    cmdNV.Parameters.AddWithValue("@maNV", txtMaNV.Text);
                    cmdNV.Parameters.AddWithValue("@hoTen", txtHoTen.Text.Trim());
                    cmdNV.Parameters.AddWithValue("@sdt", txtSDT.Text);
                    cmdNV.Parameters.AddWithValue("@diaChi",
                        string.IsNullOrWhiteSpace(txtDiaChi.Text) ? DBNull.Value : (object)txtDiaChi.Text);
                    cmdNV.Parameters.AddWithValue("@email",
                        string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text);
                    cmdNV.Parameters.AddWithValue("@chucVu", txtChucVu.Text);
                    cmdNV.ExecuteNonQuery();

                    // 2. THÊM TÀI KHOẢN
                    string sqlTaiKhoan = @"
                        INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Quyen, MaNV) 
                        VALUES (@username, @password, @quyen, @maNV)";

                    SqlCommand cmdTK = new SqlCommand(sqlTaiKhoan, conn, transaction);
                    cmdTK.Parameters.AddWithValue("@username", txtTenDangNhap.Text.Trim());
                    cmdTK.Parameters.AddWithValue("@password", txtMatKhau.Text);
                    cmdTK.Parameters.AddWithValue("@quyen", "NhanVien"); // Mặc định là Nhân viên
                    cmdTK.Parameters.AddWithValue("@maNV", txtMaNV.Text);
                    cmdTK.ExecuteNonQuery();

                    transaction.Commit();

                    string message = "Đăng ký thành công!\n\n" +
                                   "=== THÔNG TIN NHÂN VIÊN ===\n" +
                                   $"Mã NV: {txtMaNV.Text}\n" +
                                   $"Họ tên: {txtHoTen.Text}\n" +
                                   $"SĐT: {txtSDT.Text}\n" +
                                   $"Chức vụ: {txtChucVu.Text}\n\n" +
                                   "=== THÔNG TIN TÀI KHOẢN ===\n" +
                                   $"Tên đăng nhập: {txtTenDangNhap.Text}\n" +
                                   $"Quyền: Nhân viên";

                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetValues();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    if (ex.Number == 2627) // Lỗi duplicate key
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại! Vui lòng thử lại.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AutoGenerateMaNV();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi đăng ký: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi đăng ký: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Nút Reset
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

        // Nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            FormWelcom formWelcom = new FormWelcom();
            formWelcom.Show();
            this.Close();

        }

        // Hiển thị/Ẩn mật khẩu
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtMatKhau.PasswordChar = '\0';
                txtNhapLaiMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhau.PasswordChar = '•';
                txtNhapLaiMatKhau.PasswordChar = '•';
            }
        }

        // Chỉ cho nhập số vào SĐT
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Kiểm tra email khi rời khỏi textbox
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    errorProvider1.SetError(txtEmail, "Email không hợp lệ!");
                }
                else
                {
                    errorProvider1.SetError(txtEmail, "");
                }
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

        // Tự động uppercase mã NV
        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            txtMaNV.CharacterCasing = CharacterCasing.Upper;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            
            formDangNhap.Show();
            this.Close();

        }
    }
}