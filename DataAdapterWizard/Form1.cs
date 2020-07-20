using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataAdapterWizard
{
    public partial class Form1 : Form
    {
        static SqlConnection NorthwindConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = Northwind; Integrated Security = True");
        static string query = "SELECT * FROM Customers";
        static SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, NorthwindConnection);
        DataSet NorthwindDataset = new DataSet("Northwind");
        SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlDataAdapter1.Fill(NorthwindDataset, "Customers");
            dataGridView1.DataSource = NorthwindDataset.Tables["Customers"];
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            SqlDataAdapter1.Update(NorthwindDataset, "Customers");
        }
    }
}
