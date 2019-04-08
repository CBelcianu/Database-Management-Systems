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
using System.Configuration;

namespace oJoaca
{
    public partial class Form1 : Form
    {
        private String conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
            myMethod();
            GenerateLabels();
        }

        public void GenerateLabels()
        {
            List<String> prms = new List<String>(ConfigurationManager.AppSettings["ChildColumnNames"].Split(','));
            label1.Text = prms[0];
            label2.Text = prms[3];
            label3.Text = prms[4];
            label4.Text = prms[2];
            label5.Text = prms[1];
        }

        public void myMethod()
        {
            using (SqlConnection dbCon = new SqlConnection(this.conStr))
            {
                dbCon.Open();
                String select1 = ConfigurationManager.AppSettings["SelectParents"];
                String select2 = ConfigurationManager.AppSettings["SelectChilds"];
                SqlDataAdapter dta = new SqlDataAdapter(select1, dbCon);
                DataTable tbl = new DataTable();
                dta.Fill(tbl);
                dataGridView1.DataSource = tbl;
                SqlDataAdapter dta1 = new SqlDataAdapter(select2, dbCon);
                DataTable tbl1 = new DataTable();
                dta1.Fill(tbl1);
                dataGridView2.DataSource = tbl1;
                dbCon.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int aid = Int32.Parse(dataGridView1[0, rowIndex].Value.ToString());
            SqlConnection dbCon = new SqlConnection(this.conStr);
            String pcc = ConfigurationManager.AppSettings["ParentCellClick"];
            String pccp = ConfigurationManager.AppSettings["ParentCellClickParam"];
            SqlCommand cmd = new SqlCommand(pcc, dbCon);
            SqlParameter pr = new SqlParameter(pccp, aid);
            cmd.Parameters.Add(pr);
            SqlDataAdapter dta = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            dta.Fill(tbl);
            dataGridView2.DataSource = tbl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int artid = Int32.Parse(textBox1.Text);
            String artistid = textBox2.Text;
            String sectionid = textBox3.Text;
            String title = textBox4.Text;
            String price = textBox5.Text;
            SqlConnection dbCon = new SqlConnection(this.conStr);
            String iqr = ConfigurationManager.AppSettings["InsertQuery"];
            List<String> pr = new List<String>(ConfigurationManager.AppSettings["ChildColumnParams"].Split(','));
            SqlCommand cmd = new SqlCommand(iqr, dbCon);
            SqlParameter paid = new SqlParameter(pr[0], artid);
            SqlParameter parid = new SqlParameter(pr[3], Int32.Parse(artistid));
            SqlParameter psid = new SqlParameter(pr[4], Int32.Parse(sectionid));
            SqlParameter ptitle = new SqlParameter(pr[2], title);
            SqlParameter pprice = new SqlParameter(pr[1], Int32.Parse(price));
            cmd.Parameters.Add(paid); cmd.Parameters.Add(parid); cmd.Parameters.Add(psid); cmd.Parameters.Add(ptitle); cmd.Parameters.Add(pprice);
            dbCon.Open();
            cmd.ExecuteNonQuery();
            dbCon.Close();
            myMethod();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int artid = Int32.Parse(textBox1.Text);
            String artistid = textBox2.Text;
            String sectionid = textBox3.Text;
            String title = textBox4.Text;
            String price = textBox5.Text;
            SqlConnection dbCon = new SqlConnection(this.conStr);
            String dqr = ConfigurationManager.AppSettings["DeleteQuery"];
            List<String> pr = new List<String>(ConfigurationManager.AppSettings["ChildColumnParams"].Split(','));
            SqlCommand cmd = new SqlCommand(dqr, dbCon);
            SqlParameter paid = new SqlParameter(pr[0], artid);
            cmd.Parameters.Add(paid);
            dbCon.Open();
            cmd.ExecuteNonQuery();
            dbCon.Close();
            myMethod();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            String childName = ConfigurationManager.AppSettings["ChildTable"];
            if (childName == "Arts")
            {
                int rowIndex = e.RowIndex;
                int artid = Int32.Parse(dataGridView2[0, rowIndex].Value.ToString());
                int artprice = Int32.Parse(dataGridView2[1, rowIndex].Value.ToString());
                String title = dataGridView2[2, rowIndex].Value.ToString();
                int artistid = Int32.Parse(dataGridView2[3, rowIndex].Value.ToString());
                int sectionid = Int32.Parse(dataGridView2[4, rowIndex].Value.ToString());
                textBox1.Text = artid.ToString();
                textBox5.Text = artprice.ToString();
                textBox4.Text = title;
                textBox2.Text = artistid.ToString();
                textBox3.Text = sectionid.ToString();
            }
            else
            {
                int rowIndex = e.RowIndex;
                int artid = Int32.Parse(dataGridView2[0, rowIndex].Value.ToString());
                int artprice = Int32.Parse(dataGridView2[2, rowIndex].Value.ToString());
                String title = dataGridView2[3, rowIndex].Value.ToString();
                int artistid = Int32.Parse(dataGridView2[1, rowIndex].Value.ToString());
                int sectionid = Int32.Parse(dataGridView2[4, rowIndex].Value.ToString());
                textBox1.Text = artid.ToString();
                textBox5.Text = artprice.ToString();
                textBox4.Text = title;
                textBox2.Text = artistid.ToString();
                textBox3.Text = sectionid.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int artid = Int32.Parse(textBox1.Text);
            String artistid = textBox2.Text;
            String sectionid = textBox3.Text;
            String title = textBox4.Text;
            String price = textBox5.Text;
            SqlConnection dbCon = new SqlConnection(this.conStr);
            String uqr = ConfigurationManager.AppSettings["UpdateQuery"];
            List<String> pr = new List<String>(ConfigurationManager.AppSettings["ChildColumnParams"].Split(','));
            SqlCommand cmd = new SqlCommand(uqr, dbCon);
            SqlParameter paid = new SqlParameter(pr[0], artid);
            SqlParameter ptitle = new SqlParameter(pr[2], title);
            SqlParameter pprice = new SqlParameter(pr[1], Int32.Parse(price));
            cmd.Parameters.Add(paid); cmd.Parameters.Add(ptitle); cmd.Parameters.Add(pprice);
            dbCon.Open();
            cmd.ExecuteNonQuery();
            dbCon.Close();
            myMethod();

        }
    }
}
