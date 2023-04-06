using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySieuThi
{
    public partial class Addform : Form
    {
        public Loader loader;
        bool edit = false;
        public Addform()
        {
            InitializeComponent();
        }
        public Addform(Loader k)
        {
            InitializeComponent();
            this.loader = k;
        }
        public Addform(Loader k,string s)
        {
            InitializeComponent();
            this.loader = k;
            edit = true;
            string query = "select MaSanPham,TenSanPham,NgayNhapHang,TenMatHang,NhaSanXuat,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang where MaSanPham = "+s;
            DataTable dt = DBHelper.Instance.GetRecords(query);
            foreach(DataRow dr in dt.Rows)
            {
                txtMaSanPham.Text = dr["MaSanPham"].ToString();
                txtTenSanPham.Text = dr["TenSanPham"].ToString();
                cbbMatHang.Text = dr["TenMatHang"].ToString();
                cbbNhaSX.Text = dr["NhaSanXuat"].ToString();
                
                rdbtnConHang.Checked = (Convert.ToBoolean(dr["TinhTrang"])) ? true : false;
                if(!rdbtnConHang.Checked)rdbtnHetHang.Checked = true;
                dtpNgayNhap.Value = DateTime.ParseExact(dr["NgayNhapHang"].ToString(), "yyyy-MM-dd", null);
            }
            txtMaSanPham.ReadOnly = true;
        }
        private void Addform_Load(object sender, EventArgs e)
        {
            DataTable dt = DBHelper.Instance.GetRecords("select TenMatHang from MatHang");
            foreach(DataRow dr in dt.Rows)
            {
                cbbMatHang.Items.Add(dr["TenMatHang"].ToString());
            }
            if(edit == false)
            {
                rdbtnConHang.Checked = true;

            }
            
        }

        private void cbbMatHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = DBHelper.Instance.GetRecords("select TenMatHang,NhaSanXuat from MatHang");
            foreach (DataRow dr in dt.Rows)
            {
                if(cbbMatHang.Text == dr["TenMatHang"].ToString())
                {
                    cbbNhaSX.Text = dr["NhaSanXuat"].ToString();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool status = true;
            errorProvider1.SetError(txtMaSanPham, "");
            errorProvider1.SetError(txtTenSanPham, "");
            errorProvider1.SetError(cbbMatHang, "");
            if(txtMaSanPham.Text == "")
            {
                errorProvider1.SetError(txtMaSanPham, "Vui lòng nhập Mã sản phẩm");
                status = false;
            }
            if (txtTenSanPham.Text == "")
            {
                errorProvider1.SetError(txtTenSanPham, "Vui lòng nhập Tên sản phẩm");
                status = false;
            }
            if (cbbMatHang.SelectedItem == null)
            {
                errorProvider1.SetError(cbbMatHang, "Vui lòng chọn mặt hàng");
                status = false;
            }
             
            DataTable dt1 = DBHelper.Instance.GetRecords("select * from MatHang");
            if (status==true && edit == false)
            {
                
                bool status1 = true;
                DataTable dt = DBHelper.Instance.GetRecords("select MaSanPham from SanPham");
                
                foreach(DataRow dr in dt.Rows)
                {
                    if (txtMaSanPham.Text == dr["MaSanPham"].ToString())
                    {
                        MessageBox.Show("Mã sản phẩm đã trùng");
                        status1=false; }
                }
                if(status1)
                {
                    int tinhtrang = 1;
                    if (rdbtnHetHang.Checked==true) tinhtrang=0;
                    string date = dtpNgayNhap.Value.ToString("yyyy-MM-dd");
                    string idMatHang ="";
                    foreach(DataRow dr in dt1.Rows)
                    {
                        if(cbbMatHang.Text == dr["TenMatHang"].ToString())
                        {
                            idMatHang = dr["MaMatHang"].ToString();
                        }
                    }
                    string query = "insert into SanPham values ('"+txtMaSanPham.Text+"',N'"+txtTenSanPham.Text+"','"+date+"',"+tinhtrang+",'"+idMatHang+"')";
                    DBHelper.Instance.ExecuteDB(query);
                    this.loader("select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang");
                    this.Close();
                }    
            }
            else
            {
                int tinhtrang = 1;
                if (rdbtnHetHang.Checked==true) tinhtrang=0;
                string date = dtpNgayNhap.Value.ToString("yyyy-MM-dd");
                string idMatHang = "";
                foreach (DataRow dr in dt1.Rows)
                {
                    if (cbbMatHang.Text == dr["TenMatHang"].ToString())
                    {
                        idMatHang = dr["MaMatHang"].ToString();
                    }
                }
                string query = "update SanPham set TenSanPham = N'"+txtTenSanPham.Text+"', NgayNhapHang = '"+date+"',TinhTrang = "+tinhtrang+", MaMatHang = "+idMatHang+" where MaSanPham = " + txtMaSanPham.Text;
                DBHelper.Instance.ExecuteDB(query);
                this.loader("select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang");
                this.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
