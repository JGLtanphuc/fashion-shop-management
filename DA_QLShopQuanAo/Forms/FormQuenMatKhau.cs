using DA_QLShopQuanAo.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// CHỈ DÙNG MỘT TRONG HAI, XÓA HAI DÒNG DƯỚI NÀY:
// using System.Net.Mail;  // XÓA DÒNG NÀY
// using System.Web.Mail;  // XÓA DÒNG NÀY

// THAY BẰNG CÁC ALIAS RÕ RÀNG:
using SmtpClient = System.Net.Mail.SmtpClient;
using MailMessage = System.Net.Mail.MailMessage;
using MailAddress = System.Net.Mail.MailAddress;
using MailPriority = System.Net.Mail.MailPriority;
using SmtpException = System.Net.Mail.SmtpException;
using SmtpStatusCode = System.Net.Mail.SmtpStatusCode;
using SmtpDeliveryMethod = System.Net.Mail.SmtpDeliveryMethod;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormQuenMatKhau : Form
    {
        public FormQuenMatKhau()
        {
            InitializeComponent();
        }

        private void FormQuenMatKhau_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        // Nút GỬI MẬT KHẨU
        private void btnGuiMatKhau_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();

            try
            {
                // 1. Kiểm tra thông tin trong database
                (bool isValid, string maNV, string currentEmail) = ValidateUserInfo(username, email);

                if (!isValid)
                {
                    MessageBox.Show("Tên đăng nhập hoặc email không đúng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Tạo mật khẩu mới (8 ký tự cả chữ và số)
                string newPassword = GenerateRandomPassword();

                // 3. Cập nhật mật khẩu mới vào database
                if (UpdatePasswordInDatabase(username, newPassword))
                {
                    // 4. Gửi email chứa mật khẩu mới
                    if (SendPasswordEmail(email, username, newPassword, maNV))
                    {
                        MessageBox.Show("Đã gửi mật khẩu mới về email của bạn!\nVui lòng kiểm tra hộp thư.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Không thể gửi email. Vui lòng thử lại sau!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật mật khẩu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kiểm tra dữ liệu nhập
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Kiểm tra định dạng email
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                MessageBox.Show("Email không đúng định dạng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        // Kiểm tra thông tin user trong database
        private (bool isValid, string maNV, string email) ValidateUserInfo(string username, string email)
        {
            try
            {
                string query = @"
                    SELECT t.TenDangNhap, t.MaNV, n.Email 
                    FROM TaiKhoan t
                    INNER JOIN NhanVien n ON t.MaNV = n.MaNV
                    WHERE t.TenDangNhap = @username";

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string dbEmail = reader["Email"].ToString();
                        string maNV = reader["MaNV"].ToString();

                        // So sánh email (không phân biệt hoa thường)
                        bool emailMatches = string.Equals(dbEmail, email, StringComparison.OrdinalIgnoreCase);

                        return (emailMatches, maNV, dbEmail);
                    }

                    return (false, "", "");
                }
            }
            catch
            {
                return (false, "", "");
            }
        }

        // Tạo mật khẩu ngẫu nhiên (8 ký tự: chữ + số)
        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";

            var random = new Random();

            // Đảm bảo có ít nhất 1 chữ và 1 số
            char[] password = new char[8];

            // 4 ký tự chữ
            for (int i = 0; i < 4; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            // 4 số
            for (int i = 4; i < 8; i++)
            {
                password[i] = digits[random.Next(digits.Length)];
            }

            // Trộn ngẫu nhiên
            for (int i = 0; i < 8; i++)
            {
                int randomIndex = random.Next(i, 8);
                char temp = password[i];
                password[i] = password[randomIndex];
                password[randomIndex] = temp;
            }

            return new string(password);
        }

        // Cập nhật mật khẩu mới vào database
        private bool UpdatePasswordInDatabase(string username, string newPassword)
        {
            try
            {
                string query = "UPDATE TaiKhoan SET MatKhau = @password WHERE TenDangNhap = @username";

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@password", newPassword);
                    cmd.Parameters.AddWithValue("@username", username);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Gửi email chứa mật khẩu mới
        // Gửi email chứa mật khẩu mới
        private bool SendPasswordEmail(string toEmail, string username, string newPassword, string maNV)
        {
            try
            {
                // ===== DÙNG MẬT KHẨU ỨNG DỤNG NÀY =====
                string gmailAccount = "quanalger@gmail.com";
                string appPassword = "wubz uluz egco hfiw"; // Mật khẩu ứng dụng của bạn

                // ===== THÊM DÒNG NÀY (QUAN TRỌNG) =====
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // ===== CẤU HÌNH SMTP GMAIL =====
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(gmailAccount, appPassword);
                    client.Timeout = 20000; // 20 giây

                    // ===== TẠO EMAIL =====
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(gmailAccount, "Hệ thống Quản lý Bán hàng");
                        mail.To.Add(toEmail);
                        mail.Subject = "CẤP LẠI MẬT KHẨU - HỆ THỐNG QUẢN LÝ BÁN HÀNG";
                        mail.Body = CreateEmailBody(username, newPassword, maNV, toEmail);
                        mail.IsBodyHtml = true;

                        // ===== GỬI EMAIL =====
                        client.Send(mail);
                    }
                }

                MessageBox.Show($"✓ Đã gửi mật khẩu mới đến: {toEmail}",
                               "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show($"Lỗi SMTP: {smtpEx.Message}\n\n" +
                               "Đảm bảo đã:\n" +
                               "1. Bật xác minh 2 bước Gmail\n" +
                               "2. Dùng đúng mật khẩu ứng dụng\n" +
                               "3. Thử bật 'Cho phép ứng dụng kém an toàn'",
                               "Lỗi gửi email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ===== THÊM PHƯƠNG THỨC NÀY =====
        private string CreateEmailBody(string username, string newPassword, string maNV, string toEmail)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <style>
            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
            .header {{ background: #007bff; color: white; padding: 15px; text-align: center; border-radius: 5px 5px 0 0; }}
            .content {{ background: #f8f9fa; padding: 20px; border-radius: 0 0 5px 5px; }}
            .password-box {{ 
                background: white; 
                border: 2px dashed #007bff; 
                padding: 15px; 
                text-align: center; 
                margin: 20px 0;
                font-size: 24px;
                font-weight: bold;
                color: #dc3545;
            }}
            .info-box {{ background: white; padding: 15px; border-left: 4px solid #28a745; margin: 15px 0; }}
            .footer {{ margin-top: 20px; padding-top: 15px; border-top: 1px solid #ddd; color: #666; font-size: 12px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h2>🔐 HỆ THỐNG QUẢN LÝ BÁN HÀNG</h2>
                <h3>CẤP LẠI MẬT KHẨU</h3>
            </div>
            
            <div class='content'>
                <p>Chào bạn,</p>
                <p>Chúng tôi đã nhận được yêu cầu cấp lại mật khẩu cho tài khoản của bạn.</p>
                
                <div class='info-box'>
                    <p><strong>📋 THÔNG TIN TÀI KHOẢN:</strong></p>
                    <ul>
                        <li><strong>Tên đăng nhập:</strong> {username}</li>
                        <li><strong>Mã nhân viên:</strong> {maNV}</li>
                        <li><strong>Email:</strong> {toEmail}</li>
                    </ul>
                </div>
                
                <p><strong>🔑 MẬT KHẨU MỚI CỦA BẠN:</strong></p>
                <div class='password-box'>{newPassword}</div>
                
                <p><strong>📝 HƯỚNG DẪN SỬ DỤNG:</strong></p>
                <ol>
                    <li>Sử dụng mật khẩu trên để đăng nhập vào hệ thống</li>
                    <li>Vào phần <strong>'Đổi mật khẩu'</strong> để thiết lập mật khẩu mới</li>
                    <li>Không chia sẻ mật khẩu này với người khác</li>
                </ol>
                
                <div class='footer'>
                    <p><em>⚠️ LƯU Ý QUAN TRỌNG:</em></p>
                    <ul>
                        <li>Mật khẩu này chỉ có hiệu lực trong 24 giờ</li>
                        <li>Nếu bạn không yêu cầu cấp lại mật khẩu, vui lòng bỏ qua email này</li>
                        <li>Liên hệ quản trị viên nếu gặp vấn đề: admin@quanlybanhang.com</li>
                    </ul>
                    <p style='text-align: center; margin-top: 20px;'>
                        ---<br>
                        <strong>Đội ngũ hỗ trợ kỹ thuật</strong><br>
                        Hệ thống Quản lý Bán hàng
                    </p>
                </div>
            </div>
        </div>
    </body>
    </html>";
        }
        // Reset form
        private void ResetForm()
        {
            txtUsername.Clear();
            txtEmail.Clear();
            txtUsername.Focus();
        }

        // Nút THOÁT
        private void btnThoat_Click(object sender, EventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
            this.Close();
        }

        // Nút RESET
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
    }
}