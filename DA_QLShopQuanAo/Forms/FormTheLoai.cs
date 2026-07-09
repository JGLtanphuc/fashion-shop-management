using DA_QLShopQuanAo.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormTheLoai : Form
    {
        // 1. CẤU HÌNH KẾT NỐI
        string connectionString = DatabaseHelper.GetConnectionString();

        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommandBuilder sqlBuilder;

        public FormTheLoai()
        {
            InitializeComponent();
        }

        private void FormTheLoai_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
            ResetValues();
            SetButtonState(false); // Mặc định ẩn nút Sửa/Xóa
        }

        // --- PHẦN 1: GIAO DIỆN VÀ TRẠNG THÁI NÚT ---

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
            if (label4 != null) label4.ForeColor = ThemeColor.PrimaryColor;
        }

        // Hàm bật/tắt các nút dựa vào trạng thái
        // status = true: Đang chọn dòng (Chế độ Sửa/Xóa)
        // status = false: Chế độ Thêm mới
        void SetButtonState(bool status)
        {
            btnThem.Enabled = !status; // Khi đang chọn sửa thì khóa nút Thêm
            btnLuu.Enabled = true;     // Nút Lưu luôn sáng

            btnSua.Enabled = status;   // Chỉ sáng khi chọn dòng
            btnXoa.Enabled = status;   // Chỉ sáng khi chọn dòng
        }

        void LoadData()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string query = "SELECT MaTheLoai, TenTheLoai, MoTa FROM TheLoai";
                adapter = new SqlDataAdapter(query, conn);

                sqlBuilder = new SqlCommandBuilder(adapter);

                dt = new DataTable();
                adapter.Fill(dt);

                // Thiết lập khóa chính
                dt.PrimaryKey = new DataColumn[] { dt.Columns["MaTheLoai"] };

                dgvTheLoai.DataSource = dt;

                dgvTheLoai.Columns["MaTheLoai"].HeaderText = "Mã Loại";
                dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên Loại";
                dgvTheLoai.Columns["MoTa"].HeaderText = "Mô Tả";

                dgvTheLoai.Columns["MaTheLoai"].ReadOnly = true;
                dgvTheLoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        void ResetValues()
        {
            txtMaLoai.Text = "";
            txtTenLoai.Text = "";
            txtMoTa.Text = "";

            txtMaLoai.Enabled = true;
            txtMaLoai.Focus();

            // Xóa báo lỗi (nếu có)
            if (errorProvider1 != null) errorProvider1.Clear();

            // Đặt lại trạng thái nút về ban đầu
            SetButtonState(false);
        }

        // --- PHẦN 2: XỬ LÝ SỰ KIỆN NHẬP LIỆU (VALIDATION) ---

        private void txtMaLoai_TextChanged(object sender, EventArgs e)
        {
            // 1. Tự động viết hoa (ví dụ gõ 'ml' thành 'ML')
            txtMaLoai.CharacterCasing = CharacterCasing.Upper;

            // 2. Kiểm tra quy tắc: Phải bắt đầu bằng "ML"
            if (txtMaLoai.Text.Length > 0 && !txtMaLoai.Text.StartsWith("ML"))
            {
                txtMaLoai.ForeColor = Color.Red; // Đổi màu chữ đỏ

                // Hiện dấu chấm than đỏ bên cạnh
                if (errorProvider1 != null)
                {
                    errorProvider1.SetError(txtMaLoai, "Mã thể loại phải bắt đầu bằng 'ML' (Ví dụ: ML01)");
                }
            }
            else
            {
                txtMaLoai.ForeColor = Color.Black; // Trả lại màu đen

                // Xóa thông báo lỗi
                if (errorProvider1 != null)
                {
                    errorProvider1.SetError(txtMaLoai, "");
                }
            }
        }

        // --- PHẦN 3: CÁC NÚT CHỨC NĂNG ---

        private void dgvTheLoai_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTheLoai.Rows[e.RowIndex];

                txtMaLoai.Text = row.Cells["MaTheLoai"].Value?.ToString();
                txtTenLoai.Text = row.Cells["TenTheLoai"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();

                txtMaLoai.Enabled = false; // Khóa mã không cho sửa

                // Bật chế độ Sửa/Xóa
                SetButtonState(true);
            }
        }

        // Sự kiện click vào vùng trắng của bảng để hủy chọn
        private void dgvTheLoai_Click(object sender, EventArgs e)
        {
            // Nếu click mà không trúng dòng nào (current row null) thì reset
            if (dgvTheLoai.CurrentRow == null)
            {
                ResetValues();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra rỗng
            if (string.IsNullOrEmpty(txtMaLoai.Text)) { MessageBox.Show("Vui lòng nhập Mã thể loại!"); txtMaLoai.Focus(); return; }
            if (string.IsNullOrEmpty(txtTenLoai.Text)) { MessageBox.Show("Vui lòng nhập Tên thể loại!"); txtTenLoai.Focus(); return; }

            // 2. Kiểm tra quy tắc "ML"
            if (!txtMaLoai.Text.StartsWith("ML"))
            {
                MessageBox.Show("Mã thể loại phải bắt đầu bằng 'ML'!");
                txtMaLoai.Focus();
                return;
            }

            // 3. Kiểm tra trùng mã
            DataRow[] foundRows = dt.Select("MaTheLoai = '" + txtMaLoai.Text + "'");
            if (foundRows.Length > 0)
            {
                MessageBox.Show("Mã loại này đã tồn tại! Vui lòng nhập mã khác.");
                return;
            }

            // 4. Thêm vào lưới
            DataRow row = dt.NewRow();
            row["MaTheLoai"] = txtMaLoai.Text;
            row["TenTheLoai"] = txtTenLoai.Text;
            row["MoTa"] = txtMoTa.Text;

            dt.Rows.Add(row);
            MessageBox.Show("Đã thêm vào danh sách chờ. Bấm 'Lưu' để cập nhật CSDL.");
            ResetValues();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLoai.Text)) return;

            DataRow foundRow = dt.Rows.Find(txtMaLoai.Text);

            if (foundRow != null)
            {
                foundRow["TenTheLoai"] = txtTenLoai.Text;
                foundRow["MoTa"] = txtMoTa.Text;

                MessageBox.Show("Đã sửa trên lưới. Bấm 'Lưu' để cập nhật vào CSDL.");
                ResetValues();
            }
            else
            {
                MessageBox.Show("Không tìm thấy dòng dữ liệu cần sửa!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLoai.Text)) return;

            if (MessageBox.Show("Bạn muốn xóa dòng này (sẽ xóa sau khi bấm Lưu)?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataRow foundRow = dt.Rows.Find(txtMaLoai.Text);

                if (foundRow != null)
                {
                    foundRow.Delete();
                    ResetValues();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật tất cả thay đổi từ DataTable về SQL
                adapter.Update(dt);
                MessageBox.Show("Đã lưu dữ liệu vào CSDL thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu (Kiểm tra xem mã có bị trùng hoặc lỗi ràng buộc): " + ex.Message);
            }
        }
    }
}