using System;
using System.Collections;
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
    public delegate void Loader(string s);
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
            cbbSort.Items.Add("Ngày Nhập");
            cbbSort.Items.Add("Mã sản phẩm");
        }
        private void LoadForm(string s)
        {

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn {ColumnName = "STT",DataType = typeof(int)},
                new DataColumn {ColumnName = "Mã sản phẩm",DataType = typeof(string)},
                new DataColumn {ColumnName = "Tên sản phẩm",DataType = typeof(string)},
                new DataColumn {ColumnName = "Nhà SX",DataType = typeof(string)},
                new DataColumn {ColumnName = "Ngày nhập",DataType = typeof(DateTime)},
                new DataColumn {ColumnName = "Mặt hàng",DataType = typeof(string)},
                new DataColumn {ColumnName = "Tình trạng",DataType = typeof(bool)},
            });
            DataTable dt2 = new DataTable();
            dt2 = DBHelper.Instance.GetRecords(s);
            int i = 0;
            foreach (DataRow r in dt2.Rows)
            {
                i++;
                dt.Rows.Add(i, r["MaSanPham"].ToString(), r["TenSanPham"].ToString(), r["NhaSanXuat"].ToString(), DateTime.ParseExact(r["NgayNhapHang"].ToString(), "yyyy-MM-dd", null), r["TenMatHang"].ToString(), Convert.ToBoolean(r["TinhTrang"].ToString()));
            } 
            dataGridView1.DataSource = dt;
            
            cbbSort.SelectedIndex = 0;

        }
        private void Mainform_Load(object sender, EventArgs e)
        {
            LoadForm("select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
            string s = selectedRow.Cells["Mã sản phẩm"].Value.ToString();
            string query = "delete from SanPham where MaSanPham = "+s;
            DialogResult rs = MessageBox.Show("Bạn có muốn xóa sản phẩm", "Thông báo", MessageBoxButtons.OKCancel);
            if(rs == DialogResult.OK)
            {

            DBHelper.Instance.ExecuteDB(query);
            LoadForm("select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang");
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keySearch = txtSearch.Text;
            if(keySearch != null)
            {
                string query = "select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang where TenSanPham like N'%"+keySearch+"%'";
                LoadForm(query);
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            string keySearch = cbbSort.Text;
            if(keySearch == "Ngày Nhập")
            {
                string query = "select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang order by NgayNhapHang asc";
                LoadForm(query);
            }
            else
            {
                string query = "select MaSanPham,TenSanPham,NhaSanXuat,NgayNhapHang,TenMatHang,TinhTrang from SanPham as sp inner join MatHang as mh on sp.MaMatHang = mh.MaMatHang order by MaSanPham asc";
                LoadForm(query);
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Addform addform = new Addform(LoadForm);
            addform.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
            string s = selectedRow.Cells["Mã sản phẩm"].Value.ToString();
            Addform addform = new Addform(LoadForm, s);
            addform.ShowDialog();

        }

        private void cbbSort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
