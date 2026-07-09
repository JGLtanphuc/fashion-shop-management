using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DA_QLShopQuanAo.Helpers;
using DA_QLShopQuanAo.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormDangNhap : Form
    {
        // Biến lưu thông tin user sau khi đăng nhập thành công
        public string UserRole { get; private set; } = "";    // "Admin" hoặc "Nhân viên"
        public string Username { get; private set; } = "";    // Tên đăng nhập
        public string MaNV { get; private set; } = "";        // Mã nhân viên
        public string HoTen { get; private set; } = "";

        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        // Nút ĐĂNG NHẬP
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

         

            try
            {
                // Kiểm tra thông tin đăng nhập trong database
                string query = @"
                    SELECT t.TenDangNhap, t.Quyen, t.MaNV, n.HoTen
                    FROM TaiKhoan t
                    INNER JOIN NhanVien n ON t.MaNV = n.MaNV
                    WHERE t.TenDangNhap = @username 
                    AND t.MatKhau = @password";

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Đăng nhập thành công
                        this.Username = reader["TenDangNhap"].ToString();
                        this.UserRole = reader["Quyen"].ToString();
                        this.MaNV = reader["MaNV"].ToString();
                        string hoTen = reader["HoTen"].ToString();
                        this.HoTen = reader["HoTen"].ToString();

                        // Hiển thị thông báo thành công
                        

                        // Đóng form đăng nhập
                        FormMainMenu frmMain = new FormMainMenu(UserRole, Username, MaNV, hoTen);
                        this.Hide();
                        frmMain.Show();
                       
                    }
                    else
                    {
                        // Đăng nhập thất bại
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Focus();
                        txtPassword.SelectAll();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút THOÁT
        private void btnThoat_Click(object sender, EventArgs e)
        {
           FormWelcom formWelcom = new FormWelcom();
            formWelcom.Show();
            this.Close();

        }

        // Nút QUÊN MẬT KHẨU (chưa implement chi tiết)
        private void btnQuenMatKhau_Click(object sender, EventArgs e)
        {
            FormQuenMatKhau formQuenMatKhau = new FormQuenMatKhau();
            formQuenMatKhau.Show();
            this.Close();
        }

        // Hiển thị/ẩn mật khẩu
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
               
            }
            else
            {
                txtPassword.PasswordChar = '•';
                
            }
        }

        // Nhấn Enter để đăng nhập
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick();
            }
        }

        private void txtNhapLaiMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick();
            }
        }
    }
}