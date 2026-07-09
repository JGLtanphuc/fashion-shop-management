using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DA_QLShopQuanAo.Helpers;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormTaiKhoanCaNhan : Form
    {
        private string username;
        private string maNV;
        private string userRole;
        private string connectionString;
        private bool isEditing = false;
        private string matKhauHienTai = "";

        public FormTaiKhoanCaNhan(string username, string maNV, string userRole)
        {
            InitializeComponent();
            this.username = username;
            this.maNV = maNV;
            this.userRole = userRole;
            this.connectionString = DatabaseHelper.GetConnectionString();

            // Ẩn panel đổi mật khẩu ban đầu
            panelDoiMatKhau.Visible = false;
            panelDoiMatKhau.Height = 0;
        }

        private void FormTaiKhoanCaNhan_Load(object sender, EventArgs e)
        {
           
            LoadThongTinTaiKhoan();
            SetReadOnlyMode(true);

            // Kiểm tra nếu là nhân viên thì disable một số chức năng
            if (userRole != "Admin")
            {
                // Chỉ cho phép sửa thông tin cơ bản
                lblQuyen.Text = "Nhân viên";
            }
        }

       

        private void LoadThongTinTaiKhoan()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            n.HoTen, n.MaNV, n.ChucVu, n.Email, n.SDT, n.DiaChi, 
                            t.TenDangNhap, t.Quyen, t.MatKhau
                        FROM NhanVien n
                        INNER JOIN TaiKhoan t ON n.MaNV = t.MaNV
                        WHERE t.TenDangNhap = @Username";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Hiển thị thông tin cá nhân
                        txtHoTen.Text = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                        txtMaNV.Text = reader["MaNV"] != DBNull.Value ? reader["MaNV"].ToString() : "";
                        txtChucVu.Text = reader["ChucVu"] != DBNull.Value ? reader["ChucVu"].ToString() : "";
                        txtEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                        txtSDT.Text = reader["SDT"] != DBNull.Value ? reader["SDT"].ToString() : "";
                        txtDiaChi.Text = reader["DiaChi"] != DBNull.Value ? reader["DiaChi"].ToString() : "";
                        txtTenDangNhap.Text = reader["TenDangNhap"] != DBNull.Value ? reader["TenDangNhap"].ToString() : "";

                        // Lưu mật khẩu hiện tại (ẩn)
                        matKhauHienTai = reader["MatKhau"] != DBNull.Value ? reader["MatKhau"].ToString() : "";

                        // Hiển thị quyền
                        string quyen = reader["Quyen"] != DBNull.Value ? reader["Quyen"].ToString() : "";
                        lblQuyen.Text = quyen;
                        if (quyen == "Admin")
                        {
                            lblQuyen.ForeColor = Color.Red;
                            lblQuyen.Font = new Font(lblQuyen.Font, FontStyle.Bold);
                        }

                        // Hiển thị thông tin user trên title
                        this.Text = $"Tài khoản: {txtHoTen.Text} - {txtMaNV.Text}";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetReadOnlyMode(bool readOnly)
        {
            txtHoTen.ReadOnly = true;
            txtMaNV.ReadOnly = true;
            txtChucVu.ReadOnly = true;
            txtTenDangNhap.ReadOnly = true;

            txtEmail.ReadOnly = !isEditing;
            txtSDT.ReadOnly = !isEditing;
            txtDiaChi.ReadOnly = !isEditing;

            // Đổi màu để phân biệt
            txtEmail.BackColor = isEditing ? Color.White : Color.FromArgb(240, 240, 240);
            txtSDT.BackColor = isEditing ? Color.White : Color.FromArgb(240, 240, 240);
            txtDiaChi.BackColor = isEditing ? Color.White : Color.FromArgb(240, 240, 240);

            btnSua.Text = isEditing ? "Hủy" : "Sửa thông tin";
            btnLuu.Enabled = isEditing;
        }
        bool IsValidSDT(string sdt)
        {
            // Kiểm tra SĐT phải có đúng 10 ký tự
            return sdt.Length == 10;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {

            isEditing = !isEditing;
            SetReadOnlyMode(!isEditing);

            if (isEditing)
            {
                txtEmail.Focus();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE NhanVien 
                        SET Email = @Email, 
                            SDT = @SDT, 
                            DiaChi = @DiaChi
                        WHERE MaNV = @MaNV";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isEditing = false;
                        SetReadOnlyMode(true);
                        LoadThongTinTaiKhoan(); // Load lại để hiển thị đúng
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            FormDoiMatKhau formDoiMatKhau = new FormDoiMatKhau(username, maNV);

            // Thiết lập Owner để có thể quản lý form cha
            formDoiMatKhau.Owner = this;

            // Ẩn form hiện tại
            this.Hide();

            // Dùng ShowDialog() để đợi form đóng
            DialogResult result = formDoiMatKhau.ShowDialog();

            // Hiện lại form tài khoản (sau khi form đổi mật khẩu đóng)
            this.Show();
            this.BringToFront(); // Đưa lên trước nếu cần

            // Kiểm tra kết quả
            if (result == DialogResult.OK)
            {
                MessageBox.Show("Mật khẩu đã được đổi thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cập nhật lại mật khẩu hiện tại trong form tài khoản
                LoadThongTinTaiKhoan();
            }
            else if (result == DialogResult.Cancel)
            {
                // Người dùng nhấn Hủy - không làm gì
            }
        }


        private void btnLuuMatKhau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMatKhauCu.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cũ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauCu.Focus();
                return;
            }

            if (txtMatKhauCu.Text != matKhauHienTai)
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauCu.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return;
            }

            if (txtMatKhauMoi.Text != txtXacNhanMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhanMatKhau.Focus();
                return;
            }

            if (!IsStrongPassword(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Mật khẩu không đủ mạnh!\n" +
                    "Yêu cầu: Ít nhất 8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE TaiKhoan 
                        SET MatKhau = @MatKhauMoi
                        WHERE TenDangNhap = @Username";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MatKhauMoi", txtMatKhauMoi.Text);
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Cập nhật lại mật khẩu hiện tại
                        matKhauHienTai = txtMatKhauMoi.Text;

                        // Reset các field
                        txtMatKhauCu.Clear();
                        txtMatKhauMoi.Clear();
                        txtXacNhanMatKhau.Clear();

                        // Ẩn panel
                        panelDoiMatKhau.Visible = false;
                        panelDoiMatKhau.Height = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi mật khẩu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsStrongPassword(string password)
        {
            // Kiểm tra mật khẩu mạnh:
            // - Ít nhất 8 ký tự
            // - Có ít nhất 1 chữ hoa
            // - Có ít nhất 1 chữ thường
            // - Có ít nhất 1 số
            // - Có ít nhất 1 ký tự đặc biệt
            if (password.Length < 8) return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else hasSpecial = true;
            }

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        private void txtMatKhauMoi_TextChanged(object sender, EventArgs e)
        {
            CheckPasswordStrength();
        }

        private void CheckPasswordStrength()
        {
            string password = txtMatKhauMoi.Text;

            if (string.IsNullOrEmpty(password))
            {
                lblDoManhMatKhau.Text = "";
                lblDoManhMatKhau.ForeColor = Color.Black;
                return;
            }

            int score = 0;

            // Độ dài
            if (password.Length >= 8) score++;
            if (password.Length >= 12) score++;

            // Đa dạng ký tự
            if (Regex.IsMatch(password, @"[A-Z]")) score++;
            if (Regex.IsMatch(password, @"[a-z]")) score++;
            if (Regex.IsMatch(password, @"[0-9]")) score++;
            if (Regex.IsMatch(password, @"[^A-Za-z0-9]")) score++;

            // Hiển thị độ mạnh
            if (score <= 2)
            {
                lblDoManhMatKhau.Text = "Yếu";
                lblDoManhMatKhau.ForeColor = Color.Red;
            }
            else if (score <= 4)
            {
                lblDoManhMatKhau.Text = "Trung bình";
                lblDoManhMatKhau.ForeColor = Color.Orange;
            }
            else if (score <= 6)
            {
                lblDoManhMatKhau.Text = "Mạnh";
                lblDoManhMatKhau.ForeColor = Color.Green;
            }
            else
            {
                lblDoManhMatKhau.Text = "Rất mạnh";
                lblDoManhMatKhau.ForeColor = Color.DarkGreen;
            }
        }

        private void btnHuyMatKhau_Click(object sender, EventArgs e)
        {
            panelDoiMatKhau.Visible = false;
            panelDoiMatKhau.Height = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}