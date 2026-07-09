using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_QLShopQuanAo.Forms
{
    public partial class FormWelcom : Form
    {
        public FormWelcom()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            FormDangKy formDangKy = new FormDangKy();
            this.Hide();
            formDangKy.Show();
            
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            this.Hide();
            formDangNhap.Show();
            
        }
    }
}
