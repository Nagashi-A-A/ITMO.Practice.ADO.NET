using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LINQsql_1
{
    public partial class Form1 : Form
    {
        DataContext db;

        public Form1()
        {
            InitializeComponent();
            db = new DataContext(@"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = Northwind; Integrated Security = True");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var results = from c in db.GetTable<Customer>()
                          where c.City == "London"
                          select c;

            foreach (var c in results)
                listBox1.Items.Add(c.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Customer cust = new Customer();
            cust.CustomerID = "WINGT";
            cust.City = "London";
            cust.CompanyName = "Steve Lasker";
            db.GetTable<Customer>().InsertOnSubmit(cust);
            db.SubmitChanges();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var deleteIndivCust =
                                from cust in db.GetTable<Customer>()
                                where cust.CustomerID == "WINGT"
                                select cust;
            if (deleteIndivCust.Count() > 0)
            {
                db.GetTable<Customer>().DeleteOnSubmit(deleteIndivCust.First());
                db.SubmitChanges();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var custQuery =
                            from cust in db.GetTable<Customer>()
                            where cust.Orders.Any()
                            select cust;
            foreach (var custObj in custQuery)
            {
                ListViewItem item =
                listView1.Items.Add(custObj.CustomerID.ToString());
                item.SubItems.Add(custObj.City.ToString());
                item.SubItems.Add(custObj.Orders.Count.ToString());
            }
        }
    }
}
