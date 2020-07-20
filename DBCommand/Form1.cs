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

namespace DBCommand
{
    public partial class Form1 : Form
    {
        string connectOne = @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = Northwind; Integrated Security = True";
        string queryString = "SELECT CustomerID, CompanyName FROM Customers";
        string queryStringTwo = "SELECT CustomerID, CompanyName FROM Customers;" + "SELECT ProductName, UnitPrice, QuantityPerUnit FROM Products;";
        string procedureString = "EXEC [Ten Most Expensive Products]";
        string createNewTable = "CREATE TABLE SalesPersons (" + "[SalesPersonID] [int] IDENTITY(1,1) NOT NULL, " + "[FirstName] [nvarchar](50) NULL, " + "[LastName] [nvarchar](50) NULL)";
        string thisCity;
        public Form1()
        {
            InitializeComponent();
        }


        private static string CommandOne(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder results = new StringBuilder();
                    bool MoreResults = false;
                    do
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                        {
                                results.Append(reader[i].ToString() + "\t");
                            }
                            results.Append(Environment.NewLine);
                        }
                        MoreResults = reader.NextResult();
                    } while (MoreResults);
                    return results.ToString();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string exp = ex.Message;
                    return exp;
                }
            }
        }

        private static string CommandTwo(string queryString, string connectionString, string getCity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandText = "SELECT CustomerID, CompanyName, City FROM Customers WHERE City = @City";
                    command.Parameters.Add("@City", SqlDbType.VarChar, 80).Value = getCity;
                    command.Connection.Open();
                    //command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder results = new StringBuilder();
                    bool MoreResults = false;
                    do
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                results.Append(reader[i].ToString() + "\t");
                            }
                            results.Append(Environment.NewLine);
                        }
                        MoreResults = reader.NextResult();
                    } while (MoreResults);
                    return results.ToString();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string exp = ex.Message;
                    return exp;
                }
            }
        }

        private static string CommandThree(string queryString, string connectionString, string getCity, string getCategoryName, int getYear)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SalesByCategory";
                    command.Parameters.Add("@CategoryName", SqlDbType.VarChar, 80).Value = getCategoryName;
                    //command.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 80).Value = getValue;
                    command.Parameters.Add("@OrdYear", SqlDbType.Int).Value = getYear;
                    //@RETURN_VALUE , @CategoryName и @OrdYear.
                    command.Connection.Open();
                    //command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder results = new StringBuilder();
                    bool MoreResults = false;
                    do
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                results.Append(reader[i].ToString() + "\t");
                            }
                            results.Append(Environment.NewLine);
                        }
                        MoreResults = reader.NextResult();
                    } while (MoreResults);
                    return results.ToString();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string exp = ex.Message;
                    return exp;
                }
            }
        }

        private static void CreateTable(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResultsTextBox.Clear();
            ResultsTextBox.Text = CommandOne(queryString, connectOne);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResultsTextBox.Clear();
            ResultsTextBox.Text = CommandOne(queryStringTwo, connectOne);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResultsTextBox.Clear();
            //ResultsTextBox.Text = CommandOne(procedureString, connectOne);

            using (SqlConnection connection = new SqlConnection(connectOne))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Ten Most Expensive Products]";
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder results = new StringBuilder();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            results.Append(reader[i].ToString() + "\t");
                        }
                        results.Append(Environment.NewLine);
                    }
                    ResultsTextBox.Text = results.ToString();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CreateTable(createNewTable, connectOne);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ResultsTextBox.Clear();
            thisCity = CityTextBox.Text;
            string paramRequest = "SELECT CustomerID, CompanyName, City FROM Customers WHERE City = @City";
            ResultsTextBox.Text = CommandTwo(paramRequest, connectOne, thisCity);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ResultsTextBox.Clear();
            using (SqlConnection connection = new SqlConnection(connectOne))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SalesByCategory";
                    command.Parameters.Add("@CategoryName", SqlDbType.VarChar, 80).Value = CategoryNameTextBox.Text;
                    command.Parameters.Add("@OrdYear", SqlDbType.Int).Value = OrdYearTextBox.Text;
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder results = new StringBuilder();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            results.Append(reader[i].ToString() + "\t");
                        }
                        results.Append(Environment.NewLine);
                    }
                    ResultsTextBox.Text = results.ToString();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
