using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oJoaca
{
    public partial class Form1 : Form
    {
        private String conStr = @"Data Source=DESKTOP-740KALD\SQLEXPRESS; Initial Catalog=ArtGallery; Integrated Security=true";

        public Form1()
        {
            InitializeComponent();
            myMethod();
        }

        public void myMethod()
        {
            using (SqlConnection dbCon = new SqlConnection(this.conStr))
            {
                dbCon.Open();
                SqlDataAdapter dta = new SqlDataAdapter("SELECT * FROM Artists", dbCon);
                DataTable tbl = new DataTable();
                dta.Fill(tbl);
                dataGridView1.DataSource = tbl;
                SqlDataAdapter dta1 = new SqlDataAdapter("SELECT * FROM Arts", dbCon);
                DataTable tbl1 = new DataTable();
                dta1.Fill(tbl1);
                dataGridView2.DataSource = tbl1;
                //dataGridView1.Columns["ArtistID"].Visible = false;
                dbCon.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int aid = Int32.Parse(dataGridView1[0, rowIndex].Value.ToString());
            SqlConnection dbCon = new SqlConnection(this.conStr);
            SqlCommand cmd = new SqlCommand("select * from Arts where Arts.ArtistID=@aid",dbCon);
            SqlParameter pr = new SqlParameter("@aid", aid);
            cmd.Parameters.Add(pr);
            SqlDataAdapter dta = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            dta.Fill(tbl);
            dataGridView2.DataSource = tbl;
            //dataGridView2.Columns["ArtistID"].Visible = false;
            //dataGridView2.Columns["SectionID"].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int artid = Int32.Parse(textBox1.Text);
            int artistid = Int32.Parse(textBox2.Text);
            int sectionid = Int32.Parse(textBox3.Text);
            String title = textBox4.Text;
            int price = Int32.Parse(textBox5.Text);
            SqlConnection dbCon = new SqlConnection(this.conStr);
            SqlCommand cmd = 
                new SqlCommand("insert into Arts(ArtID, ArtistID, SectionID, ArtTitle, ArtPrice) values (@artid,@artistid,@sectionid,@title,@price)", dbCon);
            SqlParameter paid = new SqlParameter("@artid", artid);
            SqlParameter parid = new SqlParameter("@artistid", artistid);
            SqlParameter psid = new SqlParameter("@sectionid", sectionid);
            SqlParameter ptitle = new SqlParameter("@title", title);
            SqlParameter pprice = new SqlParameter("@price", price);
            cmd.Parameters.Add(paid); cmd.Parameters.Add(parid); cmd.Parameters.Add(psid); cmd.Parameters.Add(ptitle); cmd.Parameters.Add(pprice);
            dbCon.Open();
            cmd.ExecuteNonQuery();
            dbCon.Close();
            myMethod();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int artid = Int32.Parse(textBox1.Text);
            int artprice = Int32.Parse(textBox2.Text);
            String title = textBox3.Text;
            int artistid = Int32.Parse(textBox4.Text);
            int sectionid = Int32.Parse(textBox5.Text);

            SqlConnection dbCon = new SqlConnection(this.conStr);
            SqlCommand cmd =
                new SqlCommand("delete from Arts where ArtID=@artid", dbCon);
            SqlParameter paid = new SqlParameter("@artid", artid);
            cmd.Parameters.Add(paid);
            dbCon.Open();
            cmd.ExecuteNonQuery();
            dbCon.Close();
            myMethod();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int artid = Int32.Parse(dataGridView2[0, rowIndex].Value.ToString());
            int artprice = Int32.Parse(dataGridView2[1, rowIndex].Value.ToString());
            String title = dataGridView2[2, rowIndex].Value.ToString();
            int artistid = Int32.Parse(dataGridView2[3, rowIndex].Value.ToString());
            int sectionid = Int32.Parse(dataGridView2[4, rowIndex].Value.ToString());

            textBox1.Text = artid.ToString();
            textBox2.Text = artprice.ToString();
            textBox3.Text = title;
            textBox4.Text = artistid.ToString();
            textBox5.Text = sectionid.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int artid = Int32.Parse(textBox1.Text);
            int artprice = Int32.Parse(textBox2.Text);
            String title = textBox3.Text;
            int artistid = Int32.Parse(textBox4.Text);
            int sectionid = Int32.Parse(textBox5.Text);

            int uartid = Int32.Parse(textBox6.Text);
            int uartprice = Int32.Parse(textBox7.Text);
            String utitle = textBox8.Text;
            int uartistid = Int32.Parse(textBox9.Text);
            int usectionid = Int32.Parse(textBox10.Text);

            SqlConnection dbCon = new SqlConnection(this.conStr);
            SqlCommand cmd =
                new SqlCommand("update Arts set ArtTitle=@atitle, ArtPrice=@aprice where ArtID=@artid", dbCon);
            SqlParameter paid = new SqlParameter("@artid", artid);
            SqlParameter ptitle = new SqlParameter("@atitle", utitle);
            SqlParameter pprice = new SqlParameter("@aprice", uartprice);
            cmd.Parameters.Add(paid); cmd.Parameters.Add(ptitle); cmd.Parameters.Add(pprice);
            dbCon.Open();
            cmd.ExecuteNonQuery();
            dbCon.Close();
            myMethod();

        }
    }
}
