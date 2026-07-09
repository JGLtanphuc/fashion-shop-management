using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions; // Thư viện để kiểm tra Email
using System.Windows.Forms;
using DA_QLShopQuanAo.Helpers;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormNhaCungCap : Form
    {
        // 1. CẤU HÌNH KẾT NỐI
        string connectionString = DatabaseHelper.GetConnectionString();

        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dt;

        public FormNhaCungCap()
        {
            InitializeComponent();
        }

        private void FormNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadTheme();
            LoadData();
            ResetValues();
            SetButtonState(false);
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

        void SetButtonState(bool status)
        {
            btnThem.Enabled = !status; // Khóa thêm khi đang sửa
            btnLuu.Enabled = true;

            btnSua.Enabled = status;   // Chỉ sáng khi chọn dòng
            btnXoa.Enabled = status;
        }

        void LoadData()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string query = "SELECT MaNCC, TenNCC, SDT, DiaChi, Email FROM NhaCungCap";
                adapter = new SqlDataAdapter(query, conn);
                dt = new DataTable();
                adapter.Fill(dt);

                DataColumn[] keys = new DataColumn[1];
                keys[0] = dt.Columns["MaNCC"];
                dt.PrimaryKey = keys;

                dgvNhaCungCap.DataSource = dt;

                // Đặt tên cột tiếng Việt
                dgvNhaCungCap.Columns["MaNCC"].HeaderText = "Mã NCC";
                dgvNhaCungCap.Columns["TenNCC"].HeaderText = "Tên Nhà Cung Cấp";
                dgvNhaCungCap.Columns["SDT"].HeaderText = "Số Điện Thoại";
                dgvNhaCungCap.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvNhaCungCap.Columns["Email"].HeaderText = "Email";

                dgvNhaCungCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        void ResetValues()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            // txtDiaChi.Text = ""; // Bỏ comment nếu bạn có ô địa chỉ

            txtMaNCC.Enabled = true;
            txtMaNCC.Focus();

            if (errorProvider1 != null) errorProvider1.Clear();
            SetButtonState(false);
        }

        // --- PHẦN 2: VALIDATION (KIỂM TRA DỮ LIỆU) ---

        // 1. Chỉ nhập số cho SĐT
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Số lượng phải là số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 2. Kiểm tra Mã NCC (Phải bắt đầu bằng MCC)
        

        // 3. Kiểm tra định dạng Email
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

        // --- PHẦN 3: XỬ LÝ SỰ KIỆN CLICK BẢNG ---
        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];

                txtMaNCC.Text = row.Cells["MaNCC"].Value?.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value?.ToString();
                txtSDT.Text = row.Cells["SDT"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString(); 

                txtMaNCC.Enabled = false;
                SetButtonState(true);
            }
        }

        private void dgvNhaCungCap_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow == null) ResetValues();
        }

        // --- PHẦN 4: CÁC NÚT CHỨC NĂNG (CRUD) ---
        bool IsValidSDT(string sdt)
        {
            // Kiểm tra SĐT phải có đúng 10 ký tự
            return sdt.Length == 10;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập Số Điện Thoại!"); txtSDT.Focus(); return; }
            if (!IsValidSDT(txtSDT.Text)) { MessageBox.Show("Số Điện Thoại phải có đúng 10 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSDT.Focus(); return; }
            // Validation
            if (string.IsNullOrEmpty(txtMaNCC.Text)) { MessageBox.Show("Vui lòng nhập Mã NCC!"); txtMaNCC.Focus(); return; }
            if (!txtMaNCC.Text.StartsWith("MCC")) { MessageBox.Show("Mã NCC sai quy định!"); txtMaNCC.Focus(); return; }
            if (string.IsNullOrEmpty(txtTenNCC.Text)) { MessageBox.Show("Vui lòng nhập Tên NCC!"); txtTenNCC.Focus(); return; }

            // Check trùng
            if (dt.Rows.Find(txtMaNCC.Text) != null)
            {
                MessageBox.Show("Mã nhà cung cấp này đã tồn tại!");
                return;
            }

            // Thêm vào lưới
            try
            {
                DataRow row = dt.NewRow();
                row["MaNCC"] = txtMaNCC.Text;
                row["TenNCC"] = txtTenNCC.Text;
                row["SDT"] = txtSDT.Text;
                row["Email"] = txtEmail.Text;
                row["DiaChi"] = txtDiaChi.Text; // Nếu có ô địa chỉ thì mở dòng này

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
            if (string.IsNullOrEmpty(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập Số Điện Thoại!"); txtSDT.Focus(); return; }
            if (!IsValidSDT(txtSDT.Text)) { MessageBox.Show("Số Điện Thoại phải có đúng 10 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); txtSDT.Focus(); return; }
            DataRow row = dt.Rows.Find(txtMaNCC.Text);
            if (row != null)
            {
                row["TenNCC"] = txtTenNCC.Text;
                row["SDT"] = txtSDT.Text;
                row["Email"] = txtEmail.Text;
                row["DiaChi"] = txtDiaChi.Text;

                MessageBox.Show("Đã sửa trên lưới. Bấm 'Lưu' để cập nhật.");
                ResetValues();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataRow row = dt.Rows.Find(txtMaNCC.Text);
            if (row != null)
            {
                if (MessageBox.Show("Bạn chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    row.Delete();
                    ResetValues();
                }
            }
        }

        // --- PHẦN 5: LƯU DATABASE ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
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
                            string sql = "INSERT INTO NhaCungCap (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (@ma, @ten, @sdt, @dc, @email)";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", row["MaNCC"]);
                            cmd.Parameters.AddWithValue("@ten", row["TenNCC"]);
                            cmd.Parameters.AddWithValue("@sdt", row["SDT"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@dc", row["DiaChi"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@email", row["Email"] ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string sql = "UPDATE NhaCungCap SET TenNCC=@ten, SDT=@sdt, DiaChi=@dc, Email=@email WHERE MaNCC=@ma";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", row["MaNCC"]);
                            cmd.Parameters.AddWithValue("@ten", row["TenNCC"]);
                            cmd.Parameters.AddWithValue("@sdt", row["SDT"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@dc", row["DiaChi"] ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@email", row["Email"] ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                        else if (row.RowState == DataRowState.Deleted)
                        {
                            string maNCC = row["MaNCC", DataRowVersion.Original].ToString();
                            string sql = "DELETE FROM NhaCungCap WHERE MaNCC = @ma";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@ma", maNCC);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Lưu dữ liệu thành công!");
                    dt.AcceptChanges();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi lưu (Kiểm tra ràng buộc dữ liệu): " + ex.Message);
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
                dt.DefaultView.RowFilter = $"MaNCC LIKE '%{tuKhoa}%' OR TenNCC LIKE '%{tuKhoa}%'";
            }
        }

        private void txtMaNCC_TextChanged(object sender, EventArgs e)
        {
          
            txtMaNCC.CharacterCasing = CharacterCasing.Upper;

            if (txtMaNCC.Text.Length > 0 && !txtMaNCC.Text.StartsWith("MCC"))
            {
                txtMaNCC.ForeColor = Color.Red;
                if (errorProvider1 != null)
                    errorProvider1.SetError(txtMaNCC, "Mã NCC phải bắt đầu bằng 'MCC' (Ví dụ: MCC01)");
            }
            else
            {
                txtMaNCC.ForeColor = Color.Black;
                if (errorProvider1 != null)
                    errorProvider1.SetError(txtMaNCC, "");
            }
        }
    }
    
}