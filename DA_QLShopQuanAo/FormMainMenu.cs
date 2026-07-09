using DA_QLShopQuanAo.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_QLShopQuanAo
{
    public partial class FormMainMenu : Form
    {
        //Fields
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;

        // Thêm các field để lưu thông tin user
        private string userRole;     // "Admin" hoặc "Nhân viên"
        private string username;     // Tên đăng nhập
        private string maNV;         // Mã nhân viên
        private string hoTen;        // Họ tên nhân viên

        //Constructor MỚI - nhận thông tin từ Form đăng nhập
        public FormMainMenu(string role, string username, string maNV, string hoTen = "")
        {
            InitializeComponent();
            random = new Random();

            // Lưu thông tin user
            this.userRole = role;
            this.username = username;
            this.maNV = maNV;
            this.hoTen = hoTen;

            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            // Gắn sự kiện
            panelTitleBar.Resize += panelTitleBar_Resize;
        }
        //Constructor
        public FormMainMenu()
        {
            InitializeComponent();
            random = new Random();

            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            this.Load += FormMainMenu_Load;

            // Khi panel thay đổi kích thước (kéo co form)
            panelTitleBar.Resize += panelTitleBar_Resize;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        //Methods
        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitleBar.BackColor = color;
                    //panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    ThemeColor.PrimaryColor = color;
                    ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);

                }
            }
        }

        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void OpenChildForm(Form childForm, object btnSender, string title = "")
        {
            if (activeForm != null)
                activeForm.Close();

            ActivateButton(btnSender);

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;

            // QUAN TRỌNG: giãn full panel
            childForm.Dock = DockStyle.Fill;

            this.panelDesktopPane.Controls.Add(childForm);
            this.panelDesktopPane.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();

            label1.Text = string.IsNullOrEmpty(title) ? childForm.Text : title;
        }







        //private void Reset()
        //{
        //    DisableButton();
        //    label1.Text = "HOME";
        //   // panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
        //   // panelLogo.BackColor = Color.FromArgb(39, 39, 58);
        //    currentButton = null;

        //}

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }





        private void label1_Click(object sender, EventArgs e)
        {
            CenterLabel();
        }
        private void CenterLabel()
        {
            // căn giữa label trong panel
            label1.Left = (panelTitleBar.Width - label1.Width) / 2 - 50;
            label1.Top = (panelTitleBar.Height - label1.Height) / 2;
        }
        private void ShowLoginSuccessMessage()
        {
            string message = "";
            string title = "Đăng nhập thành công";

            if (userRole == "Admin")
            {
                message = $"CHÀO MỪNG QUẢN TRỊ VIÊN!\n\n" +
                         $"👤 Họ tên: {(!string.IsNullOrEmpty(hoTen) ? hoTen : username)}\n" +
                         $"🔑 Tài khoản: {username}\n" +
                         $"📋 Mã NV: {maNV}\n" +
                         $"🎯 Quyền: {userRole}\n\n" +
                         $"Bạn có toàn quyền truy cập hệ thống!";
            }
            else if (userRole == "NhanVien") // Hoặc check "NhanVien" tùy dữ liệu của bạn
            {
                message = $"CHÀO MỪNG NHÂN VIÊN!\n\n" +
                         $"👤 Họ tên: {(!string.IsNullOrEmpty(hoTen) ? hoTen : username)}\n" +
                         $"🔑 Tài khoản: {username}\n" +
                         $"📋 Mã NV: {maNV}\n" +
                         $"🎯 Quyền: {userRole}\n\n" +
                         $"Chúc bạn một ngày làm việc hiệu quả!";
            }
            else
            {
                message = $"CHẾ ĐỘ KHÁCH!\n\n" +
                         $"Bạn đang đăng nhập với quyền hạn hạn chế.";
            }

            // SỬA Ở ĐÂY: Luôn dùng icon Information cho thân thiện
            MessageBoxIcon icon = MessageBoxIcon.Information;

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        // Hiển thị thông tin user trên giao diện
        private void DisplayUserInfo()
        {
            // Nếu có Label để hiển thị thông tin


            // Hoặc hiển thị trên Title Bar
            this.Text = $"HỆ THỐNG QUẢN LÝ BÁN HÀNG - {userRole.ToUpper()}: {username}";
        }
        private void FormMainMenu_Load(object sender, EventArgs e)
        {
            ShowLoginSuccessMessage();
            DisplayUserInfo();
            this.WindowState = FormWindowState.Maximized;
            CenterLabel();
            PhanQuyen();


        }

        private void panelTitleBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelTitleBar_Resize(object sender, EventArgs e)
        {
            CenterLabel();
        }






        private void btnThongke_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormThongKe(), sender, "THỐNG KÊ");
        }

        private void btnTheLoai_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormTheLoai(), sender, "QUẢN LÝ THỂ LOẠI");
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormSanPham(), sender, "QUẢN LÝ SẢN PHẨM");
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormKhachHang(), sender, "QUẢN LÝ KHÁCH HÀNG");
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormNhaCungCap(), sender, "QUẢN LÝ NHÀ CUNG CẤP");
        }

        private void btnDonHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormDonHang(), sender, "QUẢN LÝ ĐƠN HÀNG");
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormNhanVien(), sender, "QUẢN LÝ NHÂN VIÊN");
        }

        //private void btnCloseChildForm_Click(object sender, EventArgs e)
        //{
        //    if (activeForm != null)
        //        activeForm.Close();
        //    Reset();
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Bạn có chắc chắn muốn thoát chương trình không?",
        "Xác nhận thoát",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormWelcom formWelcom = new FormWelcom();
            formWelcom.Show();
            this.Close();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            // Sử dụng OpenChildForm để vừa mở form vừa đổi màu nút
            OpenChildForm(new FormTaiKhoanCaNhan(username, maNV, userRole), sender, "THÔNG TIN TÀI KHOẢN");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormTaiKhoanCaNhan formTaiKhoan = new FormTaiKhoanCaNhan(username, maNV, userRole);
            formTaiKhoan.Show();
        }
        private void PhanQuyen()
        {
            // Kiểm tra xem quyền có phải là Nhân viên hay không
            // Lưu ý: Chuỗi "NhanVien" phải khớp y hệt với dữ liệu trong cột Quyen ở SQL của bạn
            // Dựa vào code cũ của bạn (hàm ShowLoginSuccessMessage), mình thấy bạn đang dùng "NhanVien"
            if (userRole == "NhanVien" || userRole == "Nhân viên")
            {
                // Nếu là nhân viên -> Ẩn nút Quản lý nhân viên
                btnNhanVien.Enabled = false;
            }
            else
            {
                // Ngược lại (Admin) -> Hiện nút
                btnNhanVien.Enabled = true;
            }
        }

        private void btnNhanVien_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormNhanVien(), sender, "QUẢN LÝ NHÂN VIÊN");
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormPhieuNhap(), sender, "QUẢN LÝ PHIẾU NHẬP");
        }
    }
}
