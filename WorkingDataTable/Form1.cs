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

namespace WorkingDataTable
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
            CustomersDataGridView.DataSource = NorthwindDataset.Tables["Customers"];
            CustomersDataGridView.MultiSelect = false;
            CustomersDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            CustomersDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private DataRow GetSelectedRow()
        {
            string SelectedCustomerID = CustomersDataGridView.CurrentRow.Cells["CustomerID"].Value.ToString();
            DataTable table = NorthwindDataset.Tables["Customers"];
            string FilterString = "WHERE CustomerID = WINGT";
            DataRow[] foundRows;
            foundRows = table.Select(FilterString);
            return foundRows[0];
        }

        private void UpdateRowVersionDisplay()
        {
            try
            {
                CurrentDRVTextBox.Text = GetSelectedRow()[CustomersDataGridView.CurrentCell.OwningColumn.Name, DataRowVersion.Current].ToString();
            }
            catch (Exception ex)
            {
                CurrentDRVTextBox.Text = ex.Message;
            }
            try
            {
                OriginalDRVTextBox.Text = GetSelectedRow()[CustomersDataGridView.CurrentCell.OwningColumn.Name, DataRowVersion.Original].ToString();
            }
            catch (Exception ex)
            {
                OriginalDRVTextBox.Text = ex.Message;
            }

            RowStateTextBox.Text = GetSelectedRow().RowState.ToString();
        }

        private void FillTableButton_Click(object sender, EventArgs e)
        {
            SqlDataAdapter1.Fill(NorthwindDataset, "Customers");
            CustomersDataGridView.DataSource = NorthwindDataset.Tables["Customers"];
        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow NewRow = NorthwindDataset.Tables["Customers"].NewRow();
                Object[] CustRecord =
                {"WINGT", "Wingtip Toys", "Steve Lasker", "CEO", "1234 Main Street", "Buffalo", "NY", "98052", "USA", "206 -555-0111", "206 -555-0112"};
                NewRow.ItemArray = CustRecord;
                NorthwindDataset.Tables["Customers"].Rows.Add(NewRow);
                SqlDataAdapter1.Update(NorthwindDataset.Tables["Customers"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Row Failed");
            }
        }

        private void DeleteRowButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow().Delete();
        }

        private void UpdateValueButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow()[CustomersDataGridView.CurrentCell.OwningColumn.Name] = CellValueTextBox.Text;
            UpdateRowVersionDisplay();
        }

        private void CustomersDataGridView_Click(object sender, EventArgs e)
        {
            CellValueTextBox.Text = CustomersDataGridView.CurrentCell.Value.ToString();
            UpdateRowVersionDisplay();
        }

        private void AcceptChangesButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow().AcceptChanges();
            UpdateRowVersionDisplay();
        }

        private void RejectChangesButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow().RejectChanges();
            UpdateRowVersionDisplay();
        }
    }
}
