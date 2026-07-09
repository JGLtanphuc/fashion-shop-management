using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DA_QLShopQuanAo.Helpers;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormDoiMatKhau : Form
    {
        private string username;
        private string maNV;
        private string connectionString;
        private string matKhauHienTai;

        public FormDoiMatKhau(string username, string maNV)
        {
            InitializeComponent();
            this.username = username;
            this.maNV = maNV;
            this.connectionString = DatabaseHelper.GetConnectionString();
            LoadMatKhauHienTai();
        }

        private void LoadMatKhauHienTai()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MatKhau FROM TaiKhoan WHERE TenDangNhap = @Username";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        matKhauHienTai = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormDoiMatKhau_Load(object sender, EventArgs e)
        {
            
            lblTenDangNhap.Text = username;
            lblMaNV.Text = maNV;
        }



        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (DoiMatKhau())
            {
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private bool ValidateInput()
        {
            // Kiểm tra mật khẩu cũ
            if (string.IsNullOrEmpty(txtMatKhauCu.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cũ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauCu.Focus();
                return false;
            }

            if (txtMatKhauCu.Text != matKhauHienTai)
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauCu.Focus();
                txtMatKhauCu.SelectAll();
                return false;
            }

            // Kiểm tra mật khẩu mới
            if (string.IsNullOrEmpty(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return false;
            }

            // Kiểm tra độ mạnh mật khẩu
            if (!IsStrongPassword(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Mật khẩu không đủ mạnh!\n\n" +
                    "Yêu cầu:\n" +
                    "• Ít nhất 8 ký tự\n" +
                    "• Có chữ hoa và chữ thường\n" +
                    "• Có ít nhất 1 số\n" +
                    "• Có ít nhất 1 ký tự đặc biệt",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return false;
            }

            // Kiểm tra xác nhận mật khẩu
            if (txtMatKhauMoi.Text != txtXacNhanMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhanMatKhau.Focus();
                txtXacNhanMatKhau.SelectAll();
                return false;
            }

            // Kiểm tra mật khẩu mới không trùng mật khẩu cũ
            if (txtMatKhauMoi.Text == matKhauHienTai)
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu cũ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return false;
            }

            return true;
        }

        private bool DoiMatKhau()
        {
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

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi mật khẩu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool IsStrongPassword(string password)
        {
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
                else if (!char.IsWhiteSpace(c)) hasSpecial = true;
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
                pbPasswordStrength.Value = 0;
                lblDoManhMatKhau.ForeColor = Color.Black;
                return;
            }

            int score = 0;

            // Độ dài
            if (password.Length >= 8) score += 20;
            if (password.Length >= 12) score += 10;

            // Đa dạng ký tự
            if (Regex.IsMatch(password, @"[A-Z]")) score += 20;
            if (Regex.IsMatch(password, @"[a-z]")) score += 20;
            if (Regex.IsMatch(password, @"[0-9]")) score += 20;
            if (Regex.IsMatch(password, @"[^A-Za-z0-9]")) score += 20;

            // Không trùng mật khẩu cũ
            if (password != matKhauHienTai) score += 10;

            // Giới hạn score tối đa 100
            score = Math.Min(score, 100);

            pbPasswordStrength.Value = score;

            // Hiển thị độ mạnh
            if (score <= 30)
            {
                lblDoManhMatKhau.Text = "Yếu";
                lblDoManhMatKhau.ForeColor = Color.Red;
                pbPasswordStrength.ForeColor = Color.Red;
            }
            else if (score <= 60)
            {
                lblDoManhMatKhau.Text = "Trung bình";
                lblDoManhMatKhau.ForeColor = Color.Orange;
                pbPasswordStrength.ForeColor = Color.Orange;
            }
            else if (score <= 80)
            {
                lblDoManhMatKhau.Text = "Mạnh";
                lblDoManhMatKhau.ForeColor = Color.Green;
                pbPasswordStrength.ForeColor = Color.Green;
            }
            else
            {
                lblDoManhMatKhau.Text = "Rất mạnh";
                lblDoManhMatKhau.ForeColor = Color.DarkGreen;
                pbPasswordStrength.ForeColor = Color.DarkGreen;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtMatKhauCu.PasswordChar = '\0';
                txtMatKhauMoi.PasswordChar = '\0';
                txtXacNhanMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhauCu.PasswordChar = '*';
                txtMatKhauMoi.PasswordChar = '*';
                txtXacNhanMatKhau.PasswordChar = '*';
            }
        }

       

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}