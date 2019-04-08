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

namespace Lab2Binding
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlDataAdapter daParent, daChild;
        DataSet ds;
        SqlCommandBuilder cb;
        BindingSource bsParent, bsChild;

        public Form1()
        {
            InitializeComponent();

            String conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            connection = new SqlConnection(conStr);

            ds = new DataSet();
            String select1 = ConfigurationManager.AppSettings["SelectParents"];
            String select2 = ConfigurationManager.AppSettings["SelectChilds"];
            daParent = new SqlDataAdapter(select1, connection);
            daChild = new SqlDataAdapter(select2, connection);
            String parentTbl = ConfigurationManager.AppSettings["ParentTable"];
            String childTbl = ConfigurationManager.AppSettings["ChildTable"];
            daParent.Fill(ds, parentTbl);
            daChild.Fill(ds, childTbl);

            String fkn = ConfigurationManager.AppSettings["FKName"];
            String fkc = ConfigurationManager.AppSettings["FKColumn"];
            DataRelation drel = new DataRelation(fkn, ds.Tables[parentTbl].Columns[fkc],
                ds.Tables[childTbl].Columns[fkc]);
            ds.Relations.Add(drel);

            cb = new SqlCommandBuilder(daChild);

            bsParent = new BindingSource
            {
                DataSource = ds,
                DataMember = parentTbl
            };

            bsChild = new BindingSource
            {
                DataSource = bsParent,
                DataMember = fkn
            };

            dataGridView1.DataSource = bsParent;
            dataGridView2.DataSource = bsChild;
        }
    }
}
