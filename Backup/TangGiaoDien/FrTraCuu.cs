using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BUS;
using DTO; 
namespace TangGiaoDien
{
    public partial class FrTraCuu : Form
    {
        public FrTraCuu()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void FrTraCuu_Load(object sender, EventArgs e)
        { 
            //dataGridView1.AutoGenerateColumns = false;
            txtloaitietkiem.DataSource = LoaiTietKiem_BUS.DSLoaiTietKiem();
            txtloaitietkiem.DisplayMember = "TenLoaiTietKiem";
            txtloaitietkiem.ValueMember = "TenLoaiTietKiem";
            txtloaitietkiem.SelectedIndex = 0;
            txttinhtrang.SelectedIndex = 0;
        } 
        private void txtmaso_TextChanged(object sender, EventArgs e)
        {
            if (txtmaso.Text.ToString() != "")
            {
                txtloaitietkiem.Enabled = false;
                txtkhachhang.Enabled = false;
                txtcmnd.Enabled = false;
                txtdiachi.Enabled = false;
                txttinhtrang.Enabled = false;
                txttongtien1.Enabled = false;
                txttongtien2.Enabled = false;
                dtngay1.Enabled = false;
                dtngay2.Enabled = false; 
            }
            else
            {
                txtloaitietkiem.Enabled = true;
                txtkhachhang.Enabled = true;
                txtcmnd.Enabled = true;
                txtdiachi.Enabled = true;
                txttinhtrang.Enabled = true;
                txttongtien1.Enabled = true;
                txttongtien2.Enabled = true;
                dtngay1.Enabled = true;
                dtngay2.Enabled = true;
            }
        }
        public void TraCuu()
        {
            long maso;
            DataTable ketqua;
            if (long.TryParse(txtmaso.Text.ToString(), out maso) == true)
            {
                // luc nhap ma so
                ketqua = TraCuuSo_BUS.LayDSSTKThoaTraCuu2(maso);
            }
            else
            {   //luc ko nhap ma so
                string khachhang = txtkhachhang.Text;
                string diachi = txtdiachi.Text;
                float tongtien1;
                if (float.TryParse(txttongtien1.Text, out tongtien1) == false)
                {
                    tongtien1 = -1;
                }

                float tongtien2;
                if (float.TryParse(txttongtien2.Text, out tongtien2) == false)
                {
                    tongtien2 = -1;
                } 
                DateTime ngay1 = dtngay1.Value; 
                DateTime ngay2 = dtngay2.Value;
                string tenloaitietkiem = "";
                tenloaitietkiem = txtloaitietkiem.SelectedValue.ToString();
                string cmnd = txtcmnd.Text;
                bool tinhtrang;
                if (txttinhtrang.Text == "mở")
                {
                    tinhtrang = true;
                }
                else
                {
                    tinhtrang = false;
                }
                //ketqua = TraCuuSo_BUS.LayDSSTKThoaTraCuu1(khachhang, diachi, tongtien1, tongtien2, ngay1, ngay2, tenloaitietkiem, cmnd, tinhtrang);
                ketqua = TraCuuSo_BUS.LayDSSOTKThoaTraCuu_KhongTraCuuNgay(khachhang, diachi, tongtien1, tongtien2, tenloaitietkiem, cmnd, tinhtrang);
            }
            if (ketqua == null)
            {
                List<TraCuuSo_DTO> tam = new List<TraCuuSo_DTO>();
                DataTable bang_tracuu = new DataTable();
                gridControl_TraCuu.DataSource = bang_tracuu; 
                DialogResult t;
                t = MessageBox.Show("Dữ liệu không có ", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (DialogResult.OK == t)
                {
                    txtmaso.Text = "";
                    txtkhachhang.Text = "";
                    txtdiachi.Text = "";
                    txtloaitietkiem.Text = "";
                    txtcmnd.Text = "";
                }
                return;
            }

            DataTable table_form = new DataTable();
            table_form.Columns.Add("STT");
            table_form.Columns.Add("MaSo");
            table_form.Columns.Add("TenLoaiTietKiem");
            table_form.Columns.Add("TenKhachHang");
            table_form.Columns.Add("TongTien"); 
            string[] mang_chung1 = new string[5] { "", "", "", "", ""};           
          
            for (int i = 0; i < ketqua.Rows.Count; i++)
            {
                mang_chung1[0] = (i+1).ToString();
                mang_chung1[1] = ketqua.Rows[i]["MaSo"].ToString();
                mang_chung1[2] = ketqua.Rows[i]["TenLoaiTietKiem"].ToString();
                mang_chung1[3] = ketqua.Rows[i]["TenKhachHang"].ToString();
                mang_chung1[4] = ketqua.Rows[i]["TongTien"].ToString();
                table_form.Rows.Add(mang_chung1);
            } 
            gridControl_TraCuu.DataSource = table_form;  
        }
        private void bttracuu_Click(object sender, EventArgs e)
        {
            TraCuu();
        } 
        private void btthoat_Click(object sender, EventArgs e)
        {
            DialogResult t;
            t = MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == t)
            {
                this.Close();
            }
        }

        private void txttongtien1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            { 
                e.Handled = true;
                MessageBox.Show("Chỉ nhận số nguyên dương không nhập ký tự chữ, ký tự đặc biệt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void txttongtien2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Chỉ nhận số nguyên dương không nhập ký tự chữ, ký tự đặc biệt ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void FrTraCuu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TraCuu();
            }
            else
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult t;
                    t = MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (DialogResult.OK == t)
                    {
                        this.Close();
                    }
                }
            }
        } 
        private void bttracuu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TraCuu();
            }
        } 
        private void bttracuulai_Click(object sender, EventArgs e)
        {
            tracuulai();
        } 
        private void btthoat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult t;
                t = MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (DialogResult.OK == t)
                {
                    this.Close();
                }
            }
        }
        void tracuulai()
        {
            txtmaso.Text = "";
            txtkhachhang.Text = "";
            txtdiachi.Text = "";
            txtloaitietkiem.Text = "";
            txtcmnd.Text = "";
            txttongtien1.Text = "";
            txttongtien2.Text = "";
        }
        private void bttracuulai_KeyDown(object sender, KeyEventArgs e)
        {
            tracuulai(); 
        } 
    }
}